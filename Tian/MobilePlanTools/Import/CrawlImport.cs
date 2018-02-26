using Common;
using Common.Data;
using Crawl;
using PriceMePlansDBA;
using SubSonic.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using SubSonic.Schema;

namespace Import
{
    public class CrawlImport
    {
        ImportCarrierLog plog;
        CrawlReportLog crlog;
        int MatchCount = 0;
        int NoMatchCount = 0;

        public CrawlImport(string carrierName, CrawlReportLog _crlog)
        {
            crlog = _crlog;

            string date = DateTime.Now.ToString("yyyyMMdd_HH");
            string path = AppConfig.ImportCarrierRootPath + @"\" + date;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            plog = new ImportCarrierLog(path + @"\" + carrierName + ".txt", System.IO.FileMode.Create);
        }

        public void Import(CrawlFinishedEventArgs crawlFinishedEvent)
        {
            plog.WriteLine("Begin Import...");
            int cid = crawlFinishedEvent.EventCrawlResults.Fetcher.ProviderID;

            List<MobilePlanInfo> newCrawlPlans = new List<MobilePlanInfo>();
            List<MobilePhoneInfo> newCrawlPhones = new List<MobilePhoneInfo>();
            List<CSK_Store_MobilePlanPhoneMap> newMaps = new List<CSK_Store_MobilePlanPhoneMap>();
            List<CSK_Store_MobilePlanPhoneMap> updMaps = new List<CSK_Store_MobilePlanPhoneMap>();
            //Mobile Plan
            foreach (MobilePlanInfo info in crawlFinishedEvent.EventCrawlResults.MobilePlanInfoList)
            {
                try
                {
                    if (!(info.Price > 0)) continue;

                    #region Plan
                    CSK_Store_MobilePlan plan = CSK_Store_MobilePlan.SingleOrDefault(p => p.CarrierId == cid && p.PlanName == info.MobilePlanName);
                    if (plan == null)
                    {
                        plan = new CSK_Store_MobilePlan();
                        plan.CarrierId = cid;
                        plan.PlanName = info.MobilePlanName;
                        plan.CreatedBy = "Import";
                        plan.CreatedOn = DateTime.Now;
                        NoMatchCount++;
                    }
                    else
                        MatchCount++;
                    plan.PlanUrl = info.MobilePlanURL;
                    plan.Price = info.Price;
                    plan.DataMB = ConversionDataCompany(info.DataMB);
                    plan.Minutes = info.Minutes;
                    plan.Texts = info.Texts;
                    plan.CallRate = info.CallRate;
                    plan.TextCostPer = info.TextCostPer;
                    plan.DataRate = info.DataRate;
                    plan.Status = true;
                    plan.ModifiedBy = "import";
                    plan.ModifiedOn = DateTime.Now;
                    if (AppConfig.IsUpdate) plan.Save();
                    #endregion

                    if (info.Phones != null && info.Phones.Count > 0)
                    {
                        foreach (MobilePhoneInfo ph in info.Phones)
                        {
                            try
                            {
                                #region Mobile Phone
                                CSK_Store_MobilePhone phone = null;
                                if (!string.IsNullOrEmpty(ph.PhoneName))
                                {
                                    ph.PhoneName = ph.PhoneName.Replace("&nbsp;", " ");
                                    while (ph.PhoneName.Contains("  "))
                                    {
                                        ph.PhoneName = ph.PhoneName.Replace("  ", " ");
                                    }
                                    ph.PhoneName = ph.PhoneName.Trim();
                                    phone = CSK_Store_MobilePhone.SingleOrDefault(p => p.Name == ph.PhoneName && p.Id > 0);
                                    if (phone == null)
                                    {
                                        newCrawlPhones.Add(ph);
                                        phone = new CSK_Store_MobilePhone();
                                        phone.Name = ph.PhoneName;
                                        phone.Image = string.Empty;// ph.PhoneImage;
                                        phone.CreatedBy = "Import";
                                        phone.CreatedOn = DateTime.Now;

                                        MobilePhoneMoreInfo moreInfo = this.GetPriceMeMobilePhone(phone.Name);
                                        if (moreInfo != null)
                                        {
                                            phone.Pid = 0;// moreInfo.ProductID;
                                            phone.ManufacturerID = moreInfo.ManufacturerID;
                                            phone.ManufacturerName = moreInfo.ManufacturerName;
                                            phone.Description = moreInfo.Description;
                                            phone.Image = moreInfo.DefaultImage;
                                        }

                                        phone.Save();
                                    }
                                }
                                else
                                {
                                    phone = new CSK_Store_MobilePhone();
                                    phone.Id = 0;
                                }
                                #endregion

                                #region Plan Phone Map
                                CSK_Store_MobilePlanPhoneMap map =
                                    CSK_Store_MobilePlanPhoneMap.SingleOrDefault(m => m.MobilePlanId == plan.Id
                                        && m.MobilePhoneId == phone.Id && m.ContractTypeId == ph.ContractTypeID);
                                if (map == null)
                                {
                                    map = new CSK_Store_MobilePlanPhoneMap();
                                    map.MobilePlanId = plan.Id;
                                    map.MobilePhoneId = phone.Id;
                                    map.ContractTypeId = ph.ContractTypeID;
                                    map.CreatedBy = "Import";
                                    map.CreatedOn = DateTime.Now;
                                }
                                
                                map.Status = true;
                                if (!string.IsNullOrEmpty(ph.phoneURL))
                                    map.PlanPhoneUrl = ph.phoneURL;
                                map.UpfrontPrice = ph.UpfrontPrice;
                                map.ModifiedBy = "import";
                                map.ModifiedOn = DateTime.Now;
                                if (map.Id > 0)
                                {
                                    map.CreatedBy = ph.PhoneName;
                                    updMaps.Add(map);
                                }
                                else newMaps.Add(map);

                                if(AppConfig.IsUpdate) map.Save();
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            #region set status = 0

            SetNoMatchRecordToInactive(cid, crawlFinishedEvent.EventCrawlResults.Fetcher.ProviderName);

            #endregion

            crlog.WriteLine(StringInfo(crawlFinishedEvent.EventCrawlResults.Fetcher.ProviderName, 25) + "\t"
                + MatchCount + "\t" + NoMatchCount + "\t" + crawlFinishedEvent.EventCrawlResults.MobilePlanInfoList.Count);
            
            #region newCrawlPlans

            plog.WriteLine("Import new plans:\t" + newCrawlPlans.Count);
            if (newCrawlPlans.Count > 0)
            {
                plog.WriteLine("PhoneName\tPlanURL\tPrice\tDataMB\tMinutes\tTexts");
                foreach (var plan in newCrawlPlans)
                {
                    var str = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                        plan.MobilePlanName, plan.MobilePlanURL, plan.Price, plan.DataMB, plan.Minutes, plan.Texts);

                    plog.WriteLine(str);
                }
            }
            #endregion

            #region newCrawlPhones
            plog.WriteLine("Import new phones:\t" + newCrawlPhones.Count);
            if (newCrawlPhones.Count > 0)
            {
                plog.WriteLine("PhoneName\tPhoneImage\tUpfrontPrice\tPhoneURL");
                foreach (var phone in newCrawlPhones)
                {
                    var str = string.Format("{0}\t{1}\t{2}\t{3}",
                        phone.PhoneName, phone.PhoneImage, phone.UpfrontPrice, phone.phoneURL);

                    plog.WriteLine(str);
                }
            }
            #endregion

            #region maps
            plog.WriteLine("Import new Maps:\t" + newMaps.Count);
            if (newMaps.Count > 0)
            {
                plog.WriteLine("PlanID\tPhoneID\tContractType\tUpfrontPrice\tPlanPhoneUrl");
                foreach (var map in newMaps)
                {
                    var str = string.Format("{0}\t{1}\t{2}\t{3}\t{4}",
                        map.MobilePlanId, map.MobilePhoneId, map.ContractTypeId, map.UpfrontPrice, map.PlanPhoneUrl);

                    plog.WriteLine(str);
                }
            }

            plog.WriteLine("update Maps:\t" + updMaps.Count);
            if (updMaps.Count > 0)
            {
                plog.WriteLine("PlanID\tPhoneID\tContractType\tUpfrontPrice\tPlanPhoneUrl\tPlanPhoneName");
                foreach (var map in updMaps)
                {

                    var str = string.Format("{0}\t{1}\t{2}\t\t{3}\t{4}\t{5}",
                        map.MobilePlanId, map.MobilePhoneId, map.ContractTypeId, map.UpfrontPrice, map.PlanPhoneUrl, map.CreatedBy);

                    plog.WriteLine(str);
                }
            }
            #endregion

            plog.WriteLine("Import " + crawlFinishedEvent.EventCrawlResults.Fetcher.ProviderName + " Finish...");
        }

        private int ConversionDataCompany(string data)
        {
            int dataMB = 0;
            if (data.ToUpper().Contains("MB"))
                dataMB = int.Parse(data.Replace("MB", ""));
            else if (data.ToUpper().Contains("GB"))
                dataMB = Convert.ToInt32(decimal.Parse(data.Replace("GB", "")) * 1024m);

            return dataMB;
        }

        public string StringInfo(string info, int length)
        {
            int count = length - info.Length;
            for (int i = 0; i < count; i++)
                info += " ";
            return info;
        }

        /// <summary>
        /// 获取PriceMe的MobilePhone
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private MobilePhoneMoreInfo GetPriceMeMobilePhone(string phone)
        {
            MobilePhoneMoreInfo info = null;
            try
            {
                #region select sql
                string sql = string.Format(
                    @"SELECT ProductID,ProductName, pd.ManufacturerID,ShortDescriptionEN,DefaultImage,mf.ManufacturerName
                        FROM web.[Priceme_NZ].DBO.CSK_Store_Product pd
                        INNER JOIN web.[Priceme_NZ].DBO.CSK_Store_Manufacturer mf ON mf.ManufacturerID = pd.ManufacturerID
                        WHERE CategoryID IN (1283,11) AND ProductID > 0 AND ProductName = '{0}'", phone);
                
                #endregion
                var query = new CodingHorror(sql);
                IDataReader idr = query.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(idr, LoadOption.OverwriteChanges);

                if (dt.Rows.Count > 0)
                {
                    info = new MobilePhoneMoreInfo();
                    info.ProductID = Convert.ToInt32(dt.Rows[0]["ProductID"]);
                    info.ProductName = Convert.ToString(dt.Rows[0]["ProductName"]);
                    info.Description = Convert.ToString(dt.Rows[0]["ShortDescriptionEN"]);
                    info.DefaultImage = "http://images.pricemestatic.com" + Convert.ToString(dt.Rows[0]["DefaultImage"]).Replace("\\", "/");
                    info.ManufacturerID = Convert.ToInt32(dt.Rows[0]["ManufacturerID"]);
                    info.ManufacturerName = Convert.ToString(dt.Rows[0]["ManufacturerName"]);
                }

            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter("log.txt"))
                {
                    sw.WriteLine(ex.Message);
                    sw.Flush();
                }
            }

            return info;
        }

        /// <summary>
        /// 把产品的status更新为0时先计算 modifiedon为当天产品数量在这个provider的比例
        /// 如果超过AppConfig.InActiveRate 则可以放inactive
        /// 如果小于AppConfig.InActiveRate 则看下没有更新的产品的modifiedon时间距当天是否有AppConfig.InActiveDay. 
        /// 如果小于AppConfig.InActiveDay 则不修改数据库，只发邮件。 
        /// 如果 >= AppConfig.InActiveDay 则放这些没有更新的产品为inactive
        /// </summary>
        private void SetNoMatchRecordToInactive(int pid, string pName)
        {
            try
            {
                #region plan
                decimal rate = 0m;

                var sql = string.Format(@"SELECT COUNT(Id) FROM [dbo].[CSK_Store_MobilePlan] WHERE [CarrierId] = {0} AND [Status] = 1 AND DATEDIFF(HH,ModifiedOn,GETDATE()) <= {1};
SELECT COUNT(Id) FROM [dbo].[CSK_Store_MobilePlan] WHERE [CarrierId] = {0} AND [STATUS] = 1;
SELECT TOP 1 ModifiedOn, DATEDIFF(DD, ModifiedOn,GETDATE()) FROM [dbo].[CSK_Store_MobilePlan] WHERE [CarrierId] = {0} AND [STATUS] = 1 AND ModifiedOn < '{2}' ORDER BY ModifiedOn",
                pid, AppConfig.InActiveHour, DateTime.Now.ToString("yyyy-MM-dd"));
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandType = System.Data.CommandType.Text;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandSql = sql;
                var ds = sp.ExecuteDataSet();
                plog.WriteLine("----------SetNoMatchRecordToInactive:\t" + pid + "--------");
                plog.WriteLine(sql);

                var r1 = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
                var r2 = decimal.Parse(ds.Tables[1].Rows[0][0].ToString());
                var dt_ = DateTime.Now;
                if (ds.Tables[2].Rows.Count > 0) 
                    DateTime.TryParse(ds.Tables[2].Rows[0][0].ToString(), out dt_);
                if (r2 > 0)
                    rate = r1 / r2 * 100;
                plog.WriteLine(string.Format("Provider {0} rate:\t {1}/{2} = {3}%", pid, r1, r2, rate));
                plog.WriteLine(string.Format("Not today updated MobilePlan Last ModifiedOn:{0}",
                    ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0][0].ToString(): "NULL"));
                
                bool setInactive = false;
                if (rate > AppConfig.InActiveRate)
                {
                    setInactive = true;
                }
                else
                {
                    var days = 0;
                    if (ds.Tables[2].Rows.Count > 0)
                        days = int.Parse(ds.Tables[2].Rows[0][1].ToString());
                    if (days >= AppConfig.InActiveDay)
                    {
                        setInactive = true;
                    }
                }
                if (setInactive)
                {
                    sql = string.Format(@"UPDATE [CSK_Store_MobilePlan] SET [Status] = 0,ModifiedOn = GETDATE(),ModifiedBy='import' WHERE [CarrierId] = {0} AND [Status] = 1 AND (DATEDIFF(HH,ModifiedOn,GETDATE()) > {1} OR ModifiedOn IS NULL);",
                    pid, AppConfig.InActiveHour);
                    sp = new StoredProcedure("");
                    sp.Command.CommandType = System.Data.CommandType.Text;
                    sp.Command.CommandTimeout = 0;
                    sp.Command.CommandSql = sql;
                    sp.Execute();
                    plog.WriteLine("Set inactive:\t" + sql);
                }
                #region get email body

                plog.WriteLine("----------Get rate again------------------------------------------");
                sql = string.Format(@"SELECT COUNT(Id) FROM [dbo].[CSK_Store_MobilePlan] WHERE [CarrierId] = {0} AND [Status] = 1 AND DATEDIFF(HH,ModifiedOn,GETDATE()) <= {1};
SELECT COUNT(Id) FROM [dbo].[CSK_Store_MobilePlan] WHERE [CarrierId] = {0} AND [STATUS] = 1;
SELECT TOP 1 ModifiedOn, DATEDIFF(DD, ModifiedOn,GETDATE()) FROM [dbo].[CSK_Store_MobilePlan] WHERE [CarrierId] = {0} AND [STATUS] = 1 AND ModifiedOn < '{2}' ORDER BY ModifiedOn",
            pid, AppConfig.InActiveHour, DateTime.Now.ToString("yyyy-MM-dd"));
                sp = new StoredProcedure("");
                sp.Command.CommandType = System.Data.CommandType.Text;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandSql = sql;
                ds = sp.ExecuteDataSet();

                rate = 0;
                r1 = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
                r2 = decimal.Parse(ds.Tables[1].Rows[0][0].ToString());
                if (r2 > 0)
                    rate = r1 / r2 * 100;
                plog.WriteLine(string.Format("Provider {0} rate:\t {1}/{2} = {3}%", pid, r1, r2, rate));
                if (rate < AppConfig.InActiveRate)
                {
                    var days = 0;
                    if (ds.Tables[2].Rows.Count > 0)
                        days = int.Parse(ds.Tables[2].Rows[0][1].ToString());
                    crlog.PlanEmailBody.AppendFormat("{0} MobilePlan No Update {1}%, CarrierID: {2}, Repeat {3} days.",
                        pName, AppConfig.InActiveRate, pid, days);
                    if (days <= 0)
                        crlog.PlanEmailBody.Append(
                            string.Format("&nbsp;&nbsp;(Not today updated MobilePlan Last ModifiedOn:{0})<br />",
                        ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0][0].ToString() : "NULL"));
                    else
                        crlog.PlanEmailBody.Append("<br />");
                    plog.WriteLine(string.Format("Provider {0} MobilePlan rate:{1}% < {2}%, email will be send.", pid, rate, AppConfig.InActiveRate));
                    plog.WriteLine("AppConfig.InActiveDay:\t" + AppConfig.InActiveDay);
                    plog.WriteLine("Not today updated MobilePlan Last Modified <--> today :\t" + days);
                }

                #endregion

                #endregion

                #region map
                rate = 0m;

                plog.WriteLine("--------------CSK_Store_MobilePlanPhoneMap--------------");
                sql = string.Format(@"SELECT COUNT(M.Id) FROM [dbo].[CSK_Store_MobilePlanPhoneMap] M INNER JOIN [dbo].[CSK_Store_MobilePlan] P ON P.Id = M.MobilePlanId
WHERE [CarrierId] = {0} AND M.[Status] = 1 AND DATEDIFF(HH,M.ModifiedOn,GETDATE()) <= {1};
SELECT COUNT(M.Id) FROM [dbo].[CSK_Store_MobilePlanPhoneMap] M INNER JOIN [dbo].[CSK_Store_MobilePlan] P ON P.Id = M.MobilePlanId WHERE [CarrierId] = {0} AND M.[STATUS] = 1;
SELECT TOP 1 M.ModifiedOn, DATEDIFF(DD, M.ModifiedOn,GETDATE()) FROM [dbo].[CSK_Store_MobilePlanPhoneMap] M INNER JOIN [dbo].[CSK_Store_MobilePlan] P ON P.Id = M.MobilePlanId
WHERE [CarrierId] = {0} AND M.[Status] = 1 AND M.ModifiedOn < '{2}' ORDER BY ModifiedOn",
                pid, AppConfig.InActiveHour, DateTime.Now.ToString("yyyy-MM-dd"));
                sp = new StoredProcedure("");
                sp.Command.CommandType = System.Data.CommandType.Text;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandSql = sql;
                ds = sp.ExecuteDataSet();
                plog.WriteLine(sql);

                r1 = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
                r2 = decimal.Parse(ds.Tables[1].Rows[0][0].ToString()); 
                dt_ = DateTime.Now;
                if (ds.Tables[2].Rows.Count > 0)
                    DateTime.TryParse(ds.Tables[2].Rows[0][0].ToString(), out dt_);
                if (r2 > 0)
                    rate = r1 / r2 * 100;
                plog.WriteLine(string.Format("Provider {0} rate:\t {1}/{2} = {3}%", pid, r1, r2, rate));
                plog.WriteLine(string.Format("Not today updated Map Last ModifiedOn:{0}",
                    ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0][0].ToString() : "NULL"));

                setInactive = false;
                if (rate > AppConfig.InActiveRate)
                {
                    setInactive = true;
                }
                else
                {
                    //TimeSpan ts = DateTime.Now - dt_;
                    var days = 0;
                    if (ds.Tables[2].Rows.Count > 0)
                        days = int.Parse(ds.Tables[2].Rows[0][1].ToString());
                    
                    if (days >= AppConfig.InActiveDay)
                    {
                        setInactive = true;
                    }
                }
                if (setInactive)
                {
                    sql = string.Format(@"UPDATE [CSK_Store_MobilePlanPhoneMap] SET [Status]= 0,ModifiedOn = GETDATE(),ModifiedBy='import' WHERE
[MobilePlanId] IN (SELECT [Id] FROM [CSK_Store_MobilePlan] WHERE [CarrierId] = {0}) AND [Status]= 1 AND (DATEDIFF(HH,ModifiedOn,GETDATE()) > {1} OR ModifiedOn IS NULL);",
                    pid, AppConfig.InActiveHour);
                    sp = new StoredProcedure("");
                    sp.Command.CommandType = System.Data.CommandType.Text;
                    sp.Command.CommandTimeout = 0;
                    sp.Command.CommandSql = sql;
                    sp.Execute();
                }

                #region get email body

                plog.WriteLine("----------Get rate again------------------------------------------");
                sql = string.Format(@"SELECT COUNT(M.Id) FROM [dbo].[CSK_Store_MobilePlanPhoneMap] M INNER JOIN [dbo].[CSK_Store_MobilePlan] P ON P.Id = M.MobilePlanId
WHERE [CarrierId] = {0} AND M.[Status] = 1 AND DATEDIFF(HH,M.ModifiedOn,GETDATE()) <= {1};
SELECT COUNT(M.Id) FROM [dbo].[CSK_Store_MobilePlanPhoneMap] M INNER JOIN [dbo].[CSK_Store_MobilePlan] P ON P.Id = M.MobilePlanId WHERE [CarrierId] = {0} AND M.[STATUS] = 1;
SELECT TOP 1 M.ModifiedOn, DATEDIFF(DD, M.ModifiedOn,GETDATE()) FROM [dbo].[CSK_Store_MobilePlanPhoneMap] M INNER JOIN [dbo].[CSK_Store_MobilePlan] P ON P.Id = M.MobilePlanId
WHERE [CarrierId] = {0} AND M.[Status] = 1 AND M.ModifiedOn < '{2}' ORDER BY ModifiedOn",
                pid, AppConfig.InActiveHour, DateTime.Now.ToString("yyyy-MM-dd"));
                sp = new StoredProcedure("");
                sp.Command.CommandType = System.Data.CommandType.Text;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandSql = sql;
                ds = sp.ExecuteDataSet();
                plog.WriteLine(sql);

                rate = 0;
                r1 = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
                r2 = decimal.Parse(ds.Tables[1].Rows[0][0].ToString());
                if (r2 > 0)
                    rate = r1 / r2 * 100;
                plog.WriteLine(string.Format("Provider {0} rate:\t {1}/{2} = {3}%", pid, r1, r2, rate));
                if (rate < AppConfig.InActiveRate)
                {
                    var days = 0;
                    if (ds.Tables[2].Rows.Count > 0)
                        days = int.Parse(ds.Tables[2].Rows[0][1].ToString());
                    crlog.PlanEmailBody.AppendFormat("{0} MobilePlanPhoneMap No Update {1}%, CarrierID: {2}, Repeat {3} days.",
                        pName, AppConfig.InActiveRate, pid, days);
                    if (days <= 0)
                        crlog.PlanEmailBody.Append(
                            string.Format("&nbsp;&nbsp;(Not today updated Map Last ModifiedOn:{0})<br />",
                        ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0][0].ToString() : "NULL"));
                    else
                        crlog.PlanEmailBody.Append("<br />");
                    plog.WriteLine(string.Format("Provider {0} MobilePlanPhoneMap rate:{1}% < {2}%, email will be send.", pid, rate, AppConfig.InActiveRate));
                    plog.WriteLine("AppConfig.InActiveDay:\t" + AppConfig.InActiveDay);
                    plog.WriteLine("Not today updated Map Last Modified <--> today :\t" + days);
                }

                #endregion
                #endregion                
            }
            catch (Exception ex)
            {

            }
        }
    }
}
