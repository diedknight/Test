using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PriceMeCommon;
using PriceMeDBA;
using PriceMe;
using PriceMeCache;
using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Data;

/// <summary>
/// Summary description for RetailerLogic
/// </summary>
public class RetailerLogic
{
    public RetailerLogic()
    {

    }

    #region Retailer Page

    public List<RetailerCache> RetailerPageBindRetailerList(List<RetailerCache> rc, List<RetailerCache> rcResult, string sortletter, int rtCatID, Hashtable retailerCategoryHT, out string title, out bool isShowPage)
    {
        title = string.Empty;
        isShowPage = false;

        if (sortletter != string.Empty)
        {
            if (rtCatID == 0)
            {
                foreach (RetailerCache retailer in rc)
                {
                    if (retailer.RetailerName.ToUpper().StartsWith(sortletter))
                        rcResult.Add(retailer);
                }
            }
            else
            {
                int retailerCategoryID = rtCatID;
                foreach (RetailerCache retailer in rc)
                {
                    if (retailer.RetailerCategory == retailerCategoryID)
                    {
                        if (retailer.RetailerName.ToUpper().StartsWith(sortletter))
                            rcResult.Add(retailer);
                    }
                }
            }
        }
        else
        {
            if (rtCatID != 0)
            {
                int rcid = rtCatID;
                rcResult = RetailerController.GetRetailersByCategory(rcid, WebConfig.CountryId);

                string retailerCategoryName = "";
                if (retailerCategoryHT.Contains(rcid))
                    retailerCategoryName = retailerCategoryHT[rcid].ToString();

                title = "PriceMe " + GetSepRetailerCategoryName(retailerCategoryName.Trim()) + " Retailer List";
            }
        }

        if (rtCatID == 0 && sortletter == string.Empty)
        {
            isShowPage = true;
            rcResult = RetailerController.GetAllActiveRetailersWithVotesSumOrderByClicks(WebConfig.CountryId);
        }

        return rcResult;
    }

    public List<RetailerCache> RetailerPageBindRetailerList(string sortletter, int rtCatID, 
        out string title, out bool isShowPage, int pageIndex, int pageSize, out int totalCount)
    {
        title = string.Empty;
        isShowPage = false;
        var rcResult = new List<RetailerCache>();

        if (!string.IsNullOrEmpty(sortletter))
        {
            var rList = RetailerController.GetAllActiveRetailersWithVotesSumOrderByClicks(WebConfig.CountryId);

            if (rtCatID == 0)
            {
                foreach (RetailerCache retailer in rList)
                {
                    var fl = GetFirstLetter(retailer.RetailerName);

                    if (sortletter == "num")
                    {
                        if (isNumber(fl))
                        {
                            rcResult.Add(retailer);
                        }
                    }
                    else if (fl.ToUpper().Equals(sortletter))
                    {
                        rcResult.Add(retailer);
                    }
                }
            }
            else
            {
                int retailerCategoryID = rtCatID;
                foreach (RetailerCache retailer in rList)
                {
                    if (retailer.RetailerCategory == retailerCategoryID)
                    {
                        var fl = GetFirstLetter(retailer.RetailerName);

                        if (sortletter == "num")
                        {
                            if (isNumber(fl))
                            {
                                rcResult.Add(retailer);
                            }
                        }
                        else if (fl.ToUpper().Equals(sortletter))
                        {
                            rcResult.Add(retailer);
                        }
                    }
                }
            }
            if (rcResult.Count > pageSize)
                isShowPage = true;
        }
        else
        {
            if (rtCatID != 0)
            {
                int rcid = rtCatID;

                rcResult = RetailerController.GetRetailersByCategory(rcid, WebConfig.CountryId);
                if (rcResult.Count > pageSize)
                    isShowPage = true;

                //string retailerCategoryName = RetailerController.GetRetailerCategoryName(rcid, WebConfig.CountryId);
       
                title = Resources.Resource.TextString_RetailerLogicTitle;
            }
        }

        if (rtCatID == 0 && sortletter == string.Empty)
        {
            isShowPage = true;
            rcResult = RetailerController.GetAllActiveRetailersWithVotesSumOrderByClicks(WebConfig.CountryId);
        }

        totalCount = rcResult.Count;
        return rcResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
    }

    /// <summary>
    /// 获取字符串的第一个字符
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    public string GetFirstLetter(string txt)
    {
        return txt.Substring(0, 1);
    }

    /// <summary>
    /// 判断字符串是不是数字
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public bool isNumber(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] <= '0' || str[i] >= '9')
                return false;
        }
        return true;
    }

    public string GetRetailerCategoryName(Dictionary<int, RetailerCategoryCache> retailerCategoryHT, int rtCatID)
    {
        string retailerCategoryName = "";
        if (rtCatID != 0)
        {
            if (retailerCategoryHT.ContainsKey(rtCatID))
                retailerCategoryName = retailerCategoryHT[rtCatID].RetailerCategoryName;

            retailerCategoryName = GetSepRetailerCategoryName(retailerCategoryName);

            retailerCategoryName += "&nbsp;";
        }

        return retailerCategoryName;
    }

    protected string GetSepRetailerCategoryName(string retailerCategoryName)
    {
        if (retailerCategoryName.EndsWith("s"))
            retailerCategoryName = retailerCategoryName.Substring(0, retailerCategoryName.Length - 1);

        return retailerCategoryName;
    }

    public string RetailerPageGetFirstLetterOfRetailerNames(string sortletter, int rtCatID, string resolveUrl)
    {
        List<char> lst = new List<char>();
        char firstLetter;
        string result = "";
        bool showNum = false;

        var rList = RetailerController.GetAllActiveRetailersWithVotesSumOrderByClicks(WebConfig.CountryId);
        rList = rList.OrderBy(r => r.RetailerName).ToList();

        if (rtCatID != 0)
        {
            foreach (RetailerCache retailer in rList)
            {
                if (retailer.RetailerCategory == rtCatID)
                {
                    firstLetter = Char.Parse(GetFirstLetter(retailer.RetailerName).ToUpper());
                    bool isNum = false;
                    int num =0;
                    isNum = int.TryParse(firstLetter.ToString(), out num);
                    if (isNum) { showNum = true; continue; }
                    if (!lst.Contains(firstLetter))
                    {
                        lst.Add(firstLetter);
                        if (!string.IsNullOrEmpty(sortletter) && firstLetter.Equals(char.Parse(sortletter.Substring(0, 1))))
                            result += "<span style='color:red'>" + firstLetter + "</span> - ";
                        else
                        {
                            Dictionary<string, string> ps = new Dictionary<string, string>();
                            if (rtCatID != 0)
                                ps.Add("rcId", rtCatID.ToString());
                            ps.Add("sortletter", firstLetter.ToString());
                            result += "<a href=\"" + UrlController.GetRewriterUrl(PageName.RetailerList, ps) + "\">" + firstLetter + "</a> - ";
                        }
                    }
                }
            }
        }
        else
        {
            foreach (RetailerCache retailer in rList)
            {
                firstLetter = Char.Parse(GetFirstLetter(retailer.RetailerName).ToUpper());
                if (!lst.Contains(firstLetter))
                {
                    lst.Add(firstLetter);
                    bool isNum = false;
                    int num = 0;
                    isNum = int.TryParse(firstLetter.ToString(), out num);
                    if (isNum) { showNum = true; continue; }
                    if (!string.IsNullOrEmpty(sortletter) && firstLetter.Equals(char.Parse(sortletter.Substring(0, 1))))
                        result += "<span style='color:red'>" + firstLetter + "</span> - ";
                    else
                        result += "<a href=\"" + resolveUrl + "sortletter=" + firstLetter + "\">" + firstLetter + "</a> - ";
                }
            }
        }
        if (showNum)//#
        {
            if (rtCatID != 0)
            {
                Dictionary<string, string> ps = new Dictionary<string, string>();
                ps.Add("rcId", rtCatID.ToString());
                ps.Add("sortletter", "num");
                result += "<a href=\"" + UrlController.GetRewriterUrl(PageName.RetailerList, ps) + "\">#</a>";
            }else
                result += "<a href=\"" + resolveUrl + "sortletter=num\">#</a>";
        }
        else
            result = result.Substring(0, result.Length - 3);

        return result;
    }

    #endregion

    #region Retailer Modules

    //public IDataReader ModulesRetailerNavigationLoad()
    //{
    //    SubSonic.Schema.StoredProcedure sp = db.CSK_Store_Retailer_GetRetailerCategoryID();
    //    IDataReader idr = sp.ExecuteReader();

    //    return idr;
    //}

    //public List<Store_GLatLng> GetGLatLngCollection(int retailerId)
    //{
    //    return (from gl in db.Store_GLatLngs where gl.Retailerid == retailerId select gl).ToList();
    //}

    //public List<PriceMeCache.GLatLngCache> GetGLatLngCollection(int retailerId)
    //{
    //    return RetailerController.AllGlatlng.Where(g => g.Retailerid == retailerId).ToList();
    //}

    public List<CSK_Store_RetailerPaymentOption> GetRetailerPaymentOption(int retailerId, int countryId)
    {
        using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
        {
            return CSK_Store_RetailerPaymentOption.Find(pay => pay.RetailerId == retailerId).ToList();
        }
    }

    public CSK_Store_PaymentOption GetPaymentOption(int paymentOptionId, int countryId)
    {
        using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
        {
            return CSK_Store_PaymentOption.SingleOrDefault(option => option.PaymentId == paymentOptionId);
        }
    }

    //public List<RetailerOperatingHours> GetRetailerOperatingHourByRetailerId(int retailerId)
    //{
    //    return RetailerController.OperatingHoursList.Where(h => h.RetailerId == retailerId).ToList();
    //}

    #endregion


    public CSK_Store_RetailerCategory GetRetailerCategoryByRCaId(int rcaid)
    {
        return CSK_Store_RetailerCategory.SingleOrDefault(rc => rc.RetailerCategoryId == rcaid);
    }

    //public static IDataReader GetRetailerCrumbs(int retailerId, int countryId)
    //{

    //    SubSonic.Schema.StoredProcedure sp = db.CSK_Store_Retailer_GetRetailerCrumbs();
    //    sp.Command.AddParameter("@RetailerID", retailerId, DbType.Int32);
    //    IDataReader dr = sp.ExecuteReader();

    //    return dr;
    //}
}