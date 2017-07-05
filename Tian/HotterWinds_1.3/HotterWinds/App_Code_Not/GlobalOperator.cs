using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Threading;
using PriceMe;
using PriceMeDBA;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using PriceMeCommon;

/// <summary>
/// Summary description for GlobalOperator
/// </summary>
public class GlobalOperator
{
    static int clickMax = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ClickMax"]);

    static Thread viewTrackingThread;
    static int ViewTrackingCachDuration;// seconds.
    static HttpApplicationState app = HttpContext.Current.Application;

    static object lockOBJ = new object();

    static bool recordClick = true;

    public static void StartRecordClick()
    {
        recordClick = true;
    }

    public static void EndRecordClick()
    {
        recordClick = false;
    }

    static GlobalOperator() 
    {
        int.TryParse(ConfigurationManager.AppSettings.Get("ViewTrackingCachDuration"), out ViewTrackingCachDuration);
    }

    public static void Init()
    {
        //ThreadPool.QueueUserWorkItem(new WaitCallback(SaveProductViewTrackingToDB));
    }

    public static void PCMCount(int categoryId)
    {
        if (HttpContext.Current.Application["PCMCollection"] == null)
            HttpContext.Current.Application["PCMCollection"] = new List<PCM>();

        List<PCM> pcmCollection = HttpContext.Current.Application["PCMCollection"] as List<PCM>;
        PCM pcm = new PCM();
        pcm.CategoryId = categoryId;
        try
        {
            pcm.UserIP = Utility.GetClientIPAddress(HttpContext.Current);
        }
        catch(Exception ex)
        {
            string exceptionType = ex.Message;
            string exceptionInfo = ex.GetBaseException().ToString();

            LogWriter.PriceMeDataBaseExceptionWriter.WriteException(exceptionInfo + " ----- " + ex.Message, "PCMCount", "GlobalOperator.PCMCount", "PriceMe", 1);
            return;
        }
        if (pcm.UserIP == "000.000.000.000")
            return;
        pcmCollection.Add(pcm);
        if (pcmCollection.Count >= 1000)
        {
            app = HttpContext.Current.Application;
            Thread t = new Thread(new ThreadStart(SavePCM));
            t.Start();
        }
    }

    public static void SavePCM()
    {
        lock (lockOBJ)
        {
            List<PCM> pcmCollection = app["PCMCollection"] as List<PCM>;

            if (pcmCollection != null && pcmCollection.Count > 0)
            {
                PCM[] pcms = pcmCollection.ToArray();
                pcmCollection.Clear();
                foreach (PCM pcm in pcms)
                {
                    try
                    {
                        pcm.Save();
                    }
                    catch { continue; }
                }
            }
        }
    }

    public static void SaveSearchKeyWords(string keyWords,int categoryid)
    {
        if (keyWords.Length > 100)
        {
            keyWords = keyWords.Substring(0, 99);
        }
        if (HttpContext.Current.Application["UserSearchCollection"] == null)
            HttpContext.Current.Application["UserSearchCollection"] = new List<CSK_Store_UserSearch>();


        List<CSK_Store_UserSearch> userSearchCollection = HttpContext.Current.Application["UserSearchCollection"] as List<CSK_Store_UserSearch>;
        CSK_Store_UserSearch us = new CSK_Store_UserSearch();
        us.CategoryID = categoryid;
        us.KeyWords = keyWords;
        us.UserIP = Utility.GetClientIPAddress(HttpContext.Current);

        userSearchCollection.Add(us);
        if(userSearchCollection.Count >= 100)
        {
            app = HttpContext.Current.Application;
            Thread t = new Thread(new ThreadStart(SaveSearchKeyWordsToDB));
            t.Start();
        }
    }

    public static void SaveSearchKeyWordsToDB()
    {
        List<string> isExistList = new List<string>();
        List<CSK_Store_UserSearch> userSearchCollection = app["UserSearchCollection"] as List<CSK_Store_UserSearch>;
        if( userSearchCollection != null && userSearchCollection.Count > 0)
        {
            CSK_Store_UserSearch[] uss = userSearchCollection.ToArray();
            userSearchCollection.Clear();
            foreach (CSK_Store_UserSearch us in uss)
            {
                if (isExistList.Contains((us.UserIP + us.KeyWords.ToLower())))
                        continue;
                isExistList.Add(us.UserIP + us.KeyWords.ToLower());
                try
                {
                    us.Save();
                }
                catch
                {
                    continue;
                }
            }
        }
    }

    public static void SaveRetailerTrack(CSK_Store_RetailerTracker retailerTracker, HttpApplicationState Application)
    {

        if (Application["RetailerTrackerCollection"] == null)
            Application["RetailerTrackerCollection"] = new List<CSK_Store_RetailerTracker>();

        List<CSK_Store_RetailerTracker> retailerTrackerCollection = Application["RetailerTrackerCollection"] as List<CSK_Store_RetailerTracker>;
        retailerTrackerCollection.Add(retailerTracker);

        if (retailerTrackerCollection.Count >= clickMax)
        {
            Thread t = new Thread(new ThreadStart(SaveRetailerTrackerToDB));
            t.Start();
        }
    }

    public static void SaveRetailerTrackerToDB()
    {
        List<CSK_Store_RetailerTracker> retailerTrackerCollection = app["RetailerTrackerCollection"] as List<CSK_Store_RetailerTracker>;
        if (retailerTrackerCollection != null && retailerTrackerCollection.Count > 0)
        {
            CSK_Store_RetailerTracker[] rts = retailerTrackerCollection.ToArray();
            retailerTrackerCollection.Clear();
            foreach (CSK_Store_RetailerTracker rt in rts)
            {
                try
                {
                    rt.Save();
                    GetRetailerIdByRetailerProductId(rt.RetailerProductID);
                }
                catch
                {
                    continue;
                }
            }
        }
    }

    public static void GetRetailerIdByRetailerProductId(int retailerProductId)
    {
        CSK_Store_RetailerProduct retailerProduct = PriceMeCommon.BusinessLogic.ProductController.GetRetailerProduct(retailerProductId, WebConfig.CountryId);
        List<CSK_Store_PPCMember> ppcMembers = CSK_Store_PPCMember.Find(ppc => ppc.RetailerId == retailerProduct.RetailerId).ToList();
        if (ppcMembers == null || ppcMembers.Count == 0)
            return;
        List<CSK_Store_PPCMemberDailyBudget> dailyBudgets = CSK_Store_PPCMemberDailyBudget.Find(ppcDailyBudget => ppcDailyBudget.PPCMemberId == ppcMembers[0].PPCMemberId).ToList();
        if (dailyBudgets != null && dailyBudgets.Count > 0)
        {
            if (dailyBudgets[0].DailyBalance >= 0)
            {
                dailyBudgets[0].DailyBalance = dailyBudgets[0].DailyBalance - ppcMembers[0].FixedCPCRate;
                if (dailyBudgets[0].DailyBalance < 0)
                    dailyBudgets[0].DailyBalance = 0;
                dailyBudgets[0].ModifiedOn = DateTime.Now;
                dailyBudgets[0].Save();
            }
        }
    }

    public static void SaveProductViewTracking( int pid, List<string> rids ) {
        string APP_KEY = "ProdectViewTracking";
        if (app == null)
            app = HttpContext.Current.Application;
        if (app[APP_KEY] == null)
            app[APP_KEY] = new Dictionary<string, int>();
        Dictionary<string, int> pvt = (Dictionary<string, int>)app[APP_KEY];

        // {pid}:{rid}:{DateTime}
        string key;
        foreach (string rid in rids) {
            key = string.Format("{0}:{1}:{2}", pid, rid, DateTime.Now.ToString("yyyy-MM-dd"));
            if (pvt.ContainsKey(key))
                pvt[key]++;
            else
                pvt.Add(key, 1);
        }

        app[APP_KEY] = pvt;
    }

    private static void SaveProductViewTrackingToDB(Object state) {
        //while (recordClick)
        //{
        //    SaveProductViewTrackingToDB();
        //    Thread.Sleep(ViewTrackingCachDuration * 1000);
        //}
    }

    public static void SaveProductViewTrackingToDB()
    {
        try
        {
            string APP_KEY = "ProdectViewTracking";

            if (app == null || app[APP_KEY] == null)
            {
                return;
            }
            Dictionary<string, int> pvt;
            pvt = (Dictionary<string, int>)app[APP_KEY];
            if (pvt.Count == 0)
            {
                return;
            }

            List<string> tmp = new List<string>();
            foreach (string key in pvt.Keys)
            {
                // {pid}:{rid}:{Date}:{Count}
                tmp.Add(string.Format("{0}:{1}", key, pvt[key]));
            }

            //每次插入300条，避免数据太多。
            // int 最大 2147483647 , 10 位
            // pid:rid:Date:Count 最大 43 个字符
            // 300 * 43 ＝ 12900 , 每次最大发送 12900 个字符到数据库
            string[] tmp_;
            string tmp__;
            int count;

            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(PriceMeCommon.BusinessLogic.MultiCountryController.GetDBProvider(WebConfig.CountryId)))
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("CSK_Store_Upadate_ProductViewTracking");

                for (int i = 0; i <= tmp.Count; i += 300)
                {
                    count = i + 300 > tmp.Count ? tmp.Count - i : 300;
                    tmp_ = new string[count];
                    tmp.CopyTo(i, tmp_, 0, count);

                    //{pid}:{rid}:{Date}:{Count},{pid}:{rid}:{Date}:{Count},{pid}:{rid}:{Date}:{Count},...
                    tmp__ = string.Join(",", tmp_);
                    tmp__ += ",";

                    sp.Command.Parameters.Clear();
                    sp.Command.AddParameter("@PARAM", tmp__, DbType.String);
                    sp.Execute();
                }
            }

            lock (app[APP_KEY])
            {
                app[APP_KEY] = null; // clear   
            }
        }
        catch { }
    }

    public static RetailerProductInfoList GetRetailerProductInfoListByInfoString(string infoString)
    {
        int hour = DateTime.Now.Hour;
        
        //System.Collections.Hashtable ppcNoLink = System.Web.HttpContext.Current.Application["NoLink"] as System.Collections.Hashtable;
        RetailerProductInfoList retailerProductInfoList = new RetailerProductInfoList();
        string[] retailerProductList = infoString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //int retailerProductCount = retailerProductList.Length;
        //int count = 0;
        foreach (string retailerProductString in retailerProductList)
        {
            string[] infos = retailerProductString.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (infos.Length >= 3)
            {
                RetailerProductInfo retailerProductInfo = new RetailerProductInfo();
                int rid = int.Parse(infos[0]);
                int rpid = int.Parse(infos[1]);
                double price = double.Parse(infos[2], PriceMeCommon.PriceMeStatic.Provider);

                retailerProductInfo.RetailerID = rid;
                retailerProductInfo.RetaielrPrdocutID = rpid;
                retailerProductInfo.Price = price;

                var ppcInfo = PriceMeCommon.BusinessLogic.RetailerController.GetPPcInfoByRetailerId(rid, WebConfig.CountryId);

                if (ppcInfo != null)
                {
                    retailerProductInfo.PPCMemberType = ppcInfo.PPCMemberTypeID;
                    retailerProductInfo.PPCLogo = Resources.Resource.ImageWebsite2 + GlobalOperator.FixImagePath(ppcInfo.PPCLogo);
                    retailerProductInfo.PPCIndex = ppcInfo.PPCIndex;
                    retailerProductInfo.IsRestricted = false;
                    //retailerProductInfo.IsDropOff = ppcInfo.PPCDropOff ?? false;
                    //retailerProductInfo.UpperHour = ppcm.PPCDropOnTime ?? 0;
                    //retailerProductInfo.LowerHour = ppcm.PPCTime ?? 0;
                }
                else
                {
                    retailerProductInfo.PPCMemberType = 1;
                    retailerProductInfo.UpperHour = 0;
                    retailerProductInfo.LowerHour = 24;
                    retailerProductInfo.IsDropOff = false;
                    retailerProductInfo.IsRestricted = false;
                }
                retailerProductInfoList.Add(retailerProductInfo);
            }
            //count++;
        }
        return retailerProductInfoList;
    }

    public static string FixImagePath(string path)
    {
        string str = path.Replace("\\", "/");
        if (!str.StartsWith("/") && !str.StartsWith("http:") && !str.StartsWith("https:"))
        {
            str = "/" + str;
        }
        return str;
    }

    public static string GetPriceRange(string bestPrice, string maxPrice)
    {
        string returnStr = "";
        if (bestPrice.IndexOf(".") > 0)
        {
            bestPrice = bestPrice.Substring(0, bestPrice.IndexOf("."));
        }
        if (maxPrice.IndexOf(".") > 0)
        {
            maxPrice = maxPrice.Substring(0, maxPrice.IndexOf("."));
        }

        if (int.Parse(bestPrice) == int.Parse(maxPrice))
        {
            returnStr = "$" + bestPrice;
        }
        else
        {
            returnStr = "$" + bestPrice + " - $" + maxPrice;
        }
        return returnStr;
    }

    public static bool ContactUsEmail(string yourname, string youremail, string message, List<string> toEmails, string topic)
    {
        try
        {
            PriceMeDBA.CSK_Store_EmailDatum ed = new PriceMeDBA.CSK_Store_EmailDatum();
            ed.FullName = yourname;
            ed.EMail = youremail;
            ed.Message = message;
            ed.Save();

            string toemail = string.Empty;
            if (toEmails == null || toEmails.Count == 0)
            {
                toEmails = new List<string>();
                toemail = System.Configuration.ConfigurationManager.AppSettings["InfoEmail"];
            }
            else
            {
                foreach (string e in toEmails)
                {
                    toemail += e + ",";
                }
                toemail = toemail.Substring(0, toemail.LastIndexOf(','));
            }
            
            string countryInfo = "";
            if (Resources.Resource.Country.Equals("New Zealand"))
            {
                countryInfo = "User feedback NZ <";
            }
            else if (Resources.Resource.Country.Equals("Australia"))
            {
                countryInfo = "User feedback AU <";
            }
            else if (Resources.Resource.Country.Equals("Hong Kong"))
            {
                countryInfo = "User feedback HK <";
            }
            else
            {
                countryInfo = "User feedback " + Resources.Resource.Country + " <";
            }

            string subject = (countryInfo + youremail + ">").Replace('\r', ' ').Replace('\n', ' ');
            string body = ("Topic: " + topic + "<br /><br />" + message + "<br/> <br/>From:" + youremail).Replace('\r', ' ').Replace('\n', ' ');

            PriceMe.Utility.AmazonEmailOutside(body, subject, toemail, PriceMeCommon.ConfigAppString.EmailAddress, yourname, youremail);
            return true;
        }
        catch (Exception ex)
        {
            //LogWriter.PriceMeDataBaseExceptionWriter.WriteException(ex.Message + "-----" + ex.StackTrace, "ContactUsEmail", "ContactUsEmail", "PriceMe", 1);
            PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + "-----" + ex.StackTrace);
            if (ex.InnerException != null)
            {
                PriceMeCommon.BusinessLogic.LogController.WriteException("\t--------\nInnerException : " + ex.InnerException.Message + "-----------" + ex.InnerException.StackTrace + "\n");
            }
            return false;
        }
    }

    public static string SubString(string source, int length)
    {
        if (string.IsNullOrEmpty(source)) return "";
        if (source.Length > length)
        {
            return source.Substring(0, length - 3) + "...";
        }
        else return source;
    }
}