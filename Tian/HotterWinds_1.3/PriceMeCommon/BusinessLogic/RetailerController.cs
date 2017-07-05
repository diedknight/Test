using PriceMeCache;
using PriceMeCommon.Data;
using PriceMeDBA;
using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class RetailerController
    {
        static Dictionary<int, List<RetailerCache>> MultiCountryAllActiveRetailersWithVotesSumOrderByClicksDic_Static;
        static Dictionary<int, Dictionary<int, RetailerCache>> MultiCountryAllActiveRetailersDic_Static;
        static Dictionary<int, Dictionary<int, List<RetailerCache>>> MultiCountryRetailerCategoryDic_Static;
        static Dictionary<int, Dictionary<int, List<StoreGLatLng>>> MultiCountryGLatLngCacheDic_Static;
        static Dictionary<int, List<StoreGLatLng>> MultiCountryAllGLatLngCaches_Static;
        static Dictionary<int, List<RetailerReviewCache>> MultiCountryRetailerReviewCaches_Static;
        static Dictionary<int, Dictionary<int, RetailerCategoryCache>> MultiCountryRetailerCategoryCacheDic_Static;
        static Dictionary<int, Dictionary<int, string>> MultiCountryRetailerInactiveReasonDic_Static;
        static Dictionary<int, Dictionary<int, RetailerCache>> MultiCountryDirectoryRetailersDic_Static;

        static Dictionary<int, Dictionary<int, CommonPPCMember>> MultiCountryPPcDic_Static;
        static Dictionary<int, Dictionary<int, CommonPPCMember>> MultiCountryNolinkDic_Static;
        static Dictionary<int, Dictionary<int, CommonPPCMember>> MultiCountryInternationalDic_Static;
        static Dictionary<int, Dictionary<int, CommonPPCMember>> MultiCountryAllPPcMemberDic_Static;

        static Dictionary<int, List<RetailerPaymentCache>> MultiCountryRetailerPaymentDic_Static;

        public static Dictionary<int, GovernmentBadgeCache> NZGovernmentBadgeDic_Static { get; private set; }
        public static List<TradeMeSeller> NZTradeMeSellers_Static { get; private set; }

        //static Dictionary<int, List<StoreGLatLng>> MultiCountryStoreGLatLngs_Static;
        static Dictionary<int, List<StoreGRegion>> MultiCountryStoreGRegions_Static;
        static Dictionary<int, DBCountryInfo> UtilCountryInfoDic_Static;

        public static void Load(Timer.DKTimer dkTimer)
        {
            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor load all retailer");
            }
            MultiCountryAllActiveRetailersWithVotesSumOrderByClicksDic_Static = GetMultiCountryRetailersWithVotesSumOrderByClicksDic();
            MultiCountryAllActiveRetailersDic_Static = GetMultiCountryAllActiveRetailersDic(MultiCountryAllActiveRetailersWithVotesSumOrderByClicksDic_Static);
            MultiCountryRetailerCategoryDic_Static = GetMultiCountryRetailerCategoryDic(MultiCountryAllActiveRetailersWithVotesSumOrderByClicksDic_Static);
            MultiCountryGLatLngCacheDic_Static = GetMultiCountryGLatLngCacheDic();
            MultiCountryAllGLatLngCaches_Static = GetMultiCountryAllGLatLngCaches(MultiCountryGLatLngCacheDic_Static);
            MultiCountryRetailerReviewCaches_Static = GetMultiCountryRetailerReviewCaches();
            MultiCountryRetailerCategoryCacheDic_Static = GetMultiCountryRetailerCategoryCacheDic();
            MultiCountryDirectoryRetailersDic_Static = GetMultiCountryDirectoryRetailersDic();

            SetPPCMemberCaches();

            MultiCountryRetailerInactiveReasonDic_Static = GetMultiCountryRetailerInactiveReasonDic();
            MultiCountryRetailerPaymentDic_Static = GetMultiCountryRetailerPaymentDic();

            MultiCountryStoreGRegions_Static = GetMultiCountryStoreGRegions();

            NZGovernmentBadgeDic_Static = GetNZGovernmentBadgeDic();
            NZTradeMeSellers_Static = GetNZTradeMeSellers();
        }

        public static void LoadForBuildIndex()
        {
            MultiCountryAllActiveRetailersWithVotesSumOrderByClicksDic_Static = GetMultiCountryRetailersWithVotesSumOrderByClicksDic();
            MultiCountryAllActiveRetailersDic_Static = GetMultiCountryAllActiveRetailersDic(MultiCountryAllActiveRetailersWithVotesSumOrderByClicksDic_Static);
            MultiCountryRetailerCategoryDic_Static = GetMultiCountryRetailerCategoryDic(MultiCountryAllActiveRetailersWithVotesSumOrderByClicksDic_Static);
        }

        private static Dictionary<int, Dictionary<int, RetailerCache>> GetMultiCountryDirectoryRetailersDic()
        {
            Dictionary<int, Dictionary<int, RetailerCache>> multiDic = new Dictionary<int, Dictionary<int, RetailerCache>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, RetailerCache> dic = LoadDirectoryRetailers(countryId);
                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        private static Dictionary<int, RetailerCache> LoadDirectoryRetailers(int countryId)
        {
            Dictionary<int, RetailerCache> allActiveDirectoryRetailers = new Dictionary<int, RetailerCache>();
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                StoredProcedure sp = new StoredProcedure("GetALLActiveDirectory");
                using (IDataReader idrAllActiveDirectoryRetailers = sp.ExecuteReader())
                {
                    while (idrAllActiveDirectoryRetailers.Read())
                    {
                        RetailerCache retailer = new RetailerCache();
                        int retailerid = int.Parse(idrAllActiveDirectoryRetailers["RetailerId"].ToString());
                        retailer.RetailerId = retailerid;
                        retailer.StoreType = byte.Parse(idrAllActiveDirectoryRetailers["StoreType"].ToString());
                        string retailerMsg = idrAllActiveDirectoryRetailers["RetailerMessage"].ToString();
                        retailer.RetailerMessage = retailerMsg.Length > 50 ? retailerMsg.Substring(0, 50) : retailerMsg;
                        retailer.RetailerName = idrAllActiveDirectoryRetailers["RetailerName"].ToString();
                        retailer.RetailerURL = idrAllActiveDirectoryRetailers["RetailerURL"].ToString();
                        retailer.RetailerRatingSum = int.Parse(idrAllActiveDirectoryRetailers["RetailerRatingSum"].ToString());
                        //retailer.RetailerTotalRatingVotes = int.Parse(idrAllActiveDirectoryRetailers["RetailerTotalRatingVotes"].ToString());
                        retailer.LogoFile = idrAllActiveDirectoryRetailers["LogoFile"].ToString();
                        retailer.RetailerPhone = idrAllActiveDirectoryRetailers["RetailerPhone"].ToString();
                        retailer.RetailerShortDescription = idrAllActiveDirectoryRetailers["RetailerShortDescription"].ToString();
                        retailer.ContactEmail = idrAllActiveDirectoryRetailers["ContactEmail"].ToString();
                        retailer.PriceDescriptor = idrAllActiveDirectoryRetailers["PriceDescriptor"].ToString();
                        retailer.RetailerAffiliates = idrAllActiveDirectoryRetailers["RetailerAffiliates"].ToString();
                        retailer.LocationCategoryId = int.Parse(idrAllActiveDirectoryRetailers["LocationCategoryId"].ToString());
                        if (!allActiveDirectoryRetailers.ContainsKey(retailerid))
                            allActiveDirectoryRetailers.Add(retailerid, retailer);
                    }
                }
            }

            return allActiveDirectoryRetailers;
        }

        private static Dictionary<int, List<StoreGRegion>> GetMultiCountryStoreGRegions()
        {
            Dictionary<int, List<StoreGRegion>> multiDic = new Dictionary<int, List<StoreGRegion>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
                {
                    var gRegions = new List<StoreGRegion>();
                    var sp = new StoredProcedure("");
                    string sql = "select * from Store_GRegion";
                    sp.Command.CommandType = CommandType.Text;
                    sp.Command.CommandTimeout = 0;
                    sp.Command.CommandSql = sql;

                    using (var dr = sp.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            StoreGRegion gr = new StoreGRegion();
                            gr.GRegionID = int.Parse(dr["GRegionID"].ToString());
                            gr.RegionID = int.Parse(dr["RegionID"].ToString());
                            gr.RegionCenterGLat = dr["RegionCenterGLat"].ToString();
                            gr.RegionCenterGLng = dr["RegionCenterGLng"].ToString();
                            gr.Zoom = int.Parse(dr["Zoom"].ToString());
                            gr.RegionCode = dr["RegionCode"].ToString();

                            gRegions.Add(gr);
                        }
                    }
                    multiDic.Add(countryId, gRegions);
                }
            }

            return multiDic;
        }

        private static Dictionary<int, List<StoreGLatLng>> GetMultiCountryAllGLatLngCaches(Dictionary<int, Dictionary<int, List<StoreGLatLng>>> multiCountryGLatLngCacheDic_Static)
        {
            Dictionary<int, List<StoreGLatLng>> multiDic = new Dictionary<int, List<StoreGLatLng>>();
            foreach (int countryId in multiCountryGLatLngCacheDic_Static.Keys)
            {
                var dic = multiCountryGLatLngCacheDic_Static[countryId];
                List<StoreGLatLng> list = new List<StoreGLatLng>();
                foreach(int rId in dic.Keys)
                {
                    list.AddRange(dic[rId]);
                }
                multiDic.Add(countryId, list);
            }

            return multiDic;
        }

        private static List<TradeMeSeller> GetNZTradeMeSellers()
        {
            List<TradeMeSeller> tradeMeSellers = new List<TradeMeSeller>();

            if (MultiCountryController.CountryIdList.Contains(3))
            {
                string sql = "SELECT [Id],[MemberId],[MemberName],[RetailerId],[Status] FROM [pam_user].[dbo].[TradeMeSeller]";
                string connString = MultiCountryController.GetDBConnectionString(3);
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (SqlCommand sqlCMD = new SqlCommand(sql, sqlConn))
                    {
                        sqlCMD.CommandTimeout = 0;
                        sqlConn.Open();
                        using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                        {
                            while (sqlDR.Read())
                            {
                                TradeMeSeller tms = new TradeMeSeller();
                                int id = 0;
                                int.TryParse(sqlDR["Id"].ToString(), out id);
                                bool Status = false;
                                bool.TryParse(sqlDR["Status"].ToString(), out Status);
                                string MemberId = sqlDR["MemberId"].ToString();
                                string MemberName = sqlDR["MemberName"].ToString();
                                int RetailerId = 0;
                                int.TryParse(sqlDR["RetailerId"].ToString(), out RetailerId);

                                tms.id = id;
                                tms.MemberId = MemberId;
                                tms.RetailerId = RetailerId;
                                tms.Status = Status;
                                tms.MemberName = MemberName;

                                tradeMeSellers.Add(tms);
                            }
                        }
                    }
                }
            }

            return tradeMeSellers;
        }

        private static Dictionary<int, List<RetailerPaymentCache>> GetMultiCountryRetailerPaymentDic()
        {
            Dictionary<int, List<RetailerPaymentCache>> multiDic = new Dictionary<int, List<RetailerPaymentCache>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                List<RetailerPaymentCache> list = GetRetailerPaymentCache(countryId);
                multiDic.Add(countryId, list);
            }

            return multiDic;
        }

        public static List<RetailerPaymentCache> GetRetailerPaymentCache(int countryId)
        {
            List<RetailerPaymentCache> retailerPaymentList = new List<RetailerPaymentCache>();

            string sql = @"select ID,RetailerId,PaymentOptionId,Name,ImageUrl,Country from [dbo].[CSK_Store_RetailerPaymentOption] r
                        inner join [dbo].[CSK_Store_PaymentOption] p
                        on r.PaymentOptionId = p.PaymentId";
            string connString = MultiCountryController.GetDBConnectionString(countryId);
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                using (SqlCommand sqlCMD = new SqlCommand(sql, sqlConn))
                {
                    sqlCMD.CommandTimeout = 0;
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            RetailerPaymentCache cache = new RetailerPaymentCache();
                            cache.RetailerID = int.Parse(sqlDR["RetailerID"].ToString());
                            cache.PaymentID = int.Parse(sqlDR["PaymentOptionId"].ToString());
                            cache.PaymentImage = sqlDR["ImageUrl"].ToString();
                            cache.PaymentName = sqlDR["Name"].ToString();
                            cache.Country = sqlDR["Country"].ToString();
                            retailerPaymentList.Add(cache);
                        }
                    }
                }
            }

            return retailerPaymentList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, GovernmentBadgeCache> GetNZGovernmentBadgeDic()
        {
            Dictionary<int, GovernmentBadgeCache> dic = new Dictionary<int, GovernmentBadgeCache>();

            if (MultiCountryController.CountryIdList.Contains(3))
            {
                List<GovernmentBadgeCache> goverCaches = new List<GovernmentBadgeCache>();
                string sql = "Select * From CSK_Store_GovernmentBadge";
                string connString = MultiCountryController.GetDBConnectionString(3);
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (SqlCommand sqlCMD = new SqlCommand(sql, sqlConn))
                    {
                        sqlCMD.CommandTimeout = 0;
                        sqlConn.Open();
                        using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                        {
                            while (sqlDR.Read())
                            {
                                GovernmentBadgeCache goverCache = new GovernmentBadgeCache();
                                int id = 0;
                                int.TryParse(sqlDR["ID"].ToString(), out id);
                                int RetailerID = 0;
                                int.TryParse(sqlDR["RetailerID"].ToString(), out RetailerID);
                                string CompanyName = sqlDR["CompanyName"].ToString();
                                int CompanyID = 0;
                                int.TryParse(sqlDR["CompanyID"].ToString(), out CompanyID);
                                goverCache.ID = id;
                                goverCache.RetailerID = RetailerID;
                                goverCache.CompanyName = CompanyName;
                                goverCache.CompanyID = CompanyID;

                                dic.Add(goverCache.RetailerID, goverCache);
                            }
                        }
                    }
                }
            }

            return dic;
        }

        private static Dictionary<int, Dictionary<int, string>> GetMultiCountryRetailerInactiveReasonDic()
        {
            Dictionary<int, Dictionary<int, string>> multiDic = new Dictionary<int, Dictionary<int, string>>();

            foreach(int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, string> dic = GetRetailerInactiveReason(countryId);
                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        static Dictionary<int, string> GetRetailerInactiveReason(int countryId)
        {
            Dictionary<int, string> retailerInactiveReason = new Dictionary<int, string>();

            string sqlString = @"select ReasonTabel.ID,ReasonTabel.Reason,RetailerTable.RetailerId from Inactive_Retailer_Reason as ReasonTabel
                                left join CSK_Store_Retailer as RetailerTable on ReasonTabel.ID = RetailerTable.SetInactiveReason
                                where RetailerTable.RetailerStatus = 99";

            string connString = MultiCountryController.GetDBConnectionString(countryId);
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                using (SqlCommand sqlCMD = new SqlCommand(sqlString, sqlConn))
                {
                    sqlCMD.CommandTimeout = 0;
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            string reason = sqlDR.GetString(1);
                            int retailerID = sqlDR.GetInt32(2);

                            retailerInactiveReason.Add(retailerID, reason);
                        }
                    }
                }
            }

            return retailerInactiveReason;
        }

        private static Dictionary<int, Dictionary<int, RetailerCache>> GetMultiCountryAllActiveRetailersDic(Dictionary<int, List<RetailerCache>> multiCountryAllActiveRetailersWithVotesSumOrderByClicksDic)
        {
            Dictionary<int, Dictionary<int, RetailerCache>> multiDic = new Dictionary<int, Dictionary<int, RetailerCache>>();
            foreach(int countryId in multiCountryAllActiveRetailersWithVotesSumOrderByClicksDic.Keys)
            {
                multiDic.Add(countryId, multiCountryAllActiveRetailersWithVotesSumOrderByClicksDic[countryId].ToDictionary(r => r.RetailerId, r => r));
            }

            return multiDic;
        }

        private static void SetPPCMemberCaches()
        {
            MultiCountryPPcDic_Static = new Dictionary<int, Dictionary<int, CommonPPCMember>>();
            MultiCountryNolinkDic_Static = new Dictionary<int, Dictionary<int, CommonPPCMember>>();
            MultiCountryInternationalDic_Static = new Dictionary<int, Dictionary<int, CommonPPCMember>>();
            MultiCountryAllPPcMemberDic_Static = new Dictionary<int, Dictionary<int, CommonPPCMember>>();

            UtilCountryInfoDic_Static = GetUtilCountryInfoDic();
            List<CommonPPCMember> ppcs = GetAllCommonPPCMember();

            foreach (int countryId in MultiCountryAllActiveRetailersWithVotesSumOrderByClicksDic_Static.Keys)
            {
                Dictionary<int, CommonPPCMember> ppcDic = new Dictionary<int, CommonPPCMember>();
                Dictionary<int, CommonPPCMember> noLinkDic = new Dictionary<int, CommonPPCMember>();
                Dictionary<int, CommonPPCMember> internationalDic = new Dictionary<int, CommonPPCMember>();
                Dictionary<int, CommonPPCMember> allPPcMemberDic = new Dictionary<int, CommonPPCMember>();

                Dictionary<int, RetailerCache> allActiveRetailerDic = MultiCountryAllActiveRetailersDic_Static[countryId];
                foreach (var ppc in ppcs)
                {
                    if (allActiveRetailerDic.ContainsKey(ppc.RetailerId))
                    {
                        if (ppc.PPCMemberTypeID == 2)
                        {
                            RetailerCache retailer = allActiveRetailerDic[ppc.RetailerId];
                            string ppcLogo = "";
                            if (!string.IsNullOrEmpty(retailer.LogoFile) && retailer.LogoFile.Contains('.'))
                            {
                                ppcLogo = retailer.LogoFile.Insert(retailer.LogoFile.LastIndexOf('.'), "_s");
                            }
                            ppc.PPCLogo = ppcLogo;

                            ppcDic.Add(ppc.RetailerId, ppc);
                        }
                        else if (ppc.PPCMemberTypeID == 5)
                        {
                            noLinkDic.Add(ppc.RetailerId, ppc);
                        }

                        if (ppc.RetailerCountry != countryId)
                        {
                            internationalDic.Add(ppc.RetailerId, ppc);
                        }

                        allPPcMemberDic.Add(ppc.RetailerId, ppc);
                    }
                }

                MultiCountryPPcDic_Static.Add(countryId, ppcDic);
                MultiCountryNolinkDic_Static.Add(countryId, noLinkDic);
                MultiCountryInternationalDic_Static.Add(countryId, internationalDic);
                MultiCountryAllPPcMemberDic_Static.Add(countryId, allPPcMemberDic);
            }
        }

        private static Dictionary<int, DBCountryInfo> GetUtilCountryInfoDic()
        {
            Dictionary<int, DBCountryInfo> dic = new Dictionary<int, DBCountryInfo>();
            var list = CSK_Util_Country.All().ToList();
            foreach (CSK_Util_Country country in list)
            {
                DBCountryInfo info = new DBCountryInfo();
                info.CountryID = country.countryID;
                info.KeyName = country.country;
                info.CountryFlag = country.CountryImage;
                info.PriceMeExchangeRate = country.PriceMeExchangeRate ?? 1d;

                dic.Add(info.CountryID, info);
            }

            return dic;
        }

        private static List<CommonPPCMember> GetAllCommonPPCMember()
        {
            List<CommonPPCMember> list = new List<CommonPPCMember>();

            using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
            {
                string sql = @"SELECT 
                                [RetailerId]
                                ,[PPCMemberTypeID]
                                ,[RetailerCountry]
                                ,[PPCforInStockOnly]
                                ,[PPCIndex]
                                FROM [CSK_Store_PPCMember]";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandTimeout = 0;
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        CommonPPCMember ppc = new CommonPPCMember();
                        ppc.RetailerId = dr.GetInt32(0);
                        ppc.PPCMemberTypeID = dr.GetInt32(1);
                        ppc.RetailerCountry = dr.GetInt32(2);
                        bool PPCforInStockOnly = false;
                        bool.TryParse(dr["PPCforInStockOnly"].ToString(), out PPCforInStockOnly);
                        ppc.PPCforInStockOnly = PPCforInStockOnly;

                        ppc.PPCIndex = dr.GetDecimal(4);

                        list.Add(ppc);
                    }
                }

                return list;
            }
        }

        private static Dictionary<int, Dictionary<int, List<RetailerCache>>> GetMultiCountryRetailerCategoryDic(Dictionary<int, List<RetailerCache>> multiCountryAllActiveRetailersWithVotesSumOrderByClicksDic)
        {
            Dictionary<int, Dictionary<int, List<RetailerCache>>> multiDic = new Dictionary<int, Dictionary<int, List<RetailerCache>>>();

            foreach (int countryId in multiCountryAllActiveRetailersWithVotesSumOrderByClicksDic.Keys)
            {
                Dictionary<int, List<RetailerCache>> dic = GetRetailersGroupByCategory(multiCountryAllActiveRetailersWithVotesSumOrderByClicksDic[countryId], countryId);
                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        private static Dictionary<int, List<RetailerCache>> GetRetailersGroupByCategory(List<RetailerCache> list, int countryId)
        {
            Dictionary<int, List<RetailerCache>> dic = new Dictionary<int, List<RetailerCache>>();

            foreach (RetailerCache each in list)
            {
                if (!dic.ContainsKey(each.RetailerCategory))
                {
                    List<RetailerCache> retailers = new List<RetailerCache>();
                    retailers.Add(each);
                    dic.Add(each.RetailerCategory, retailers);
                }
                else
                {
                    dic[each.RetailerCategory].Add(each);
                }
            }

            return dic;
        }

        private static Dictionary<int, Dictionary<int, RetailerCategoryCache>> GetMultiCountryRetailerCategoryCacheDic()
        {
            Dictionary<int, Dictionary<int, RetailerCategoryCache>> multiDic = new Dictionary<int, Dictionary<int, RetailerCategoryCache>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, RetailerCategoryCache> dic = GetRetailerCategoryDic(countryId);
                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        public static Dictionary<int, RetailerCategoryCache> GetRetailerCategoryDic(int countryId)
        {
            Dictionary<int, RetailerCategoryCache> dic = new Dictionary<int, RetailerCategoryCache>();

            string connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            var sql = string.Format("CSK_Store_Retailer_GetRetailerCategoryTotal");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                conn.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int retailerCategoryID = int.Parse(dr["RetailerCategoryId"].ToString());

                    RetailerCategoryCache rtc = new RetailerCategoryCache();
                    rtc.RetailerCategoryID = retailerCategoryID.ToString();
                    rtc.RetailerCategoryTotal = int.Parse(dr["Total"].ToString());
                    rtc.RetailerCategoryName = dr["RetailerCategoryName"].ToString();

                    if (!dic.ContainsKey(retailerCategoryID))
                        dic.Add(retailerCategoryID, rtc);
                }
            }

            return dic;
        }

        private static Dictionary<int, List<RetailerReviewCache>> GetMultiCountryRetailerReviewCaches()
        {
            Dictionary<int, List<RetailerReviewCache>> multiDic = new Dictionary<int, List<RetailerReviewCache>>();
            List<int> reviewstatus = GetReviewstatus();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                VelocityController vc = MultiCountryController.GetVelocityController(countryId);
                List<RetailerReviewCache> list = GetRetailerReviewsCache(vc, reviewstatus, countryId);
                multiDic.Add(countryId, list);
            }

            return multiDic;
        }

        private static List<int> GetReviewstatus()
        {
            List<int> listReviewid = new List<int>();
            string connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            var sql = string.Format("select ReviewID from dbo.Merchant_Reviews Where ReviewStatus in (4, 5)");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int reviewid = 0;
                    int.TryParse(dr["ReviewID"].ToString(), out reviewid);
                    listReviewid.Add(reviewid);
                }
            }

            return listReviewid;
        }

        private static List<RetailerReviewCache> GetRetailerReviewsCache(VelocityController vc, List<int> reviewstatus, int countryId)
        {
            List<RetailerReviewCache> list = null;
            if (vc != null)
            {
                list = vc.GetCache<List<RetailerReviewCache>>(VelocityCacheKey.RetailerReviewList);
            }

            if (list == null || list.Count == 0)
            {
                using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
                {
                    //为什么和造velocity的方法不一样？
                    List<CSK_Store_RetailerReview> csReviewList = CSK_Store_RetailerReview.All().ToList();
                    list = ConvertController<RetailerReviewCache, CSK_Store_RetailerReview>.ConvertData(csReviewList);
                    list.ForEach(r => r.SourceType = "web");
                    list.ForEach(r => r.RetailerID = r.RetailerId);
                    list.ForEach(r => r.ReviewID = r.RetailerReviewId);
                    list.ForEach(r => r.OverallRating = r.OverallStoreRating);

                    List<RetailerReviewCache> rRetailerReviewDetailList = null;
                    List<RetailerReviewDetail> rReviewDetailList = RetailerReviewDetail.All().ToList();
                    rRetailerReviewDetailList = ConvertController<RetailerReviewCache, RetailerReviewDetail>.ConvertData(rReviewDetailList);
                    rRetailerReviewDetailList.ForEach(r => r.SourceType = "review-system");
                    list.AddRange(rRetailerReviewDetailList);

                    list = list.Where(r => reviewstatus.Contains(r.ReviewID)).ToList();
                    list = list.OrderByDescending(r => r.CreatedOn).ToList();

                    LogController.WriteLog("CountryId: " + countryId + " RetailerReviewList no velocity");
                }
            }
            else
            {
                LogController.WriteLog("CountryId: " + countryId + " RetailerReviewList velocity cache count : " + list.Count);
            }

            return list;
        }

        /// <summary>
        /// 需要修改velocity结构 AllGlatLngs直接保存GLatLngCacheDictionary
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, Dictionary<int, List<StoreGLatLng>>> GetMultiCountryGLatLngCacheDic()
        {
            Dictionary<int, Dictionary<int, List<StoreGLatLng>>> multiDic = new Dictionary<int, Dictionary<int, List<StoreGLatLng>>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                VelocityController vc = MultiCountryController.GetVelocityController(countryId);
                Dictionary<int, List<StoreGLatLng>> dic = GetGLatLngCache(vc, countryId);
                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        private static Dictionary<int, List<StoreGLatLng>> GetGLatLngCache(VelocityController vc, int countryId)
        {
            Dictionary<int, List<StoreGLatLng>> dic = null;
            if (vc != null)
            {
                dic = vc.GetCache<Dictionary<int, List<StoreGLatLng>>>(VelocityCacheKey.AllGlatLngs);
            }

            if (dic == null || dic.Count == 0)
            {
                using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
                {
                    List<Store_GLatLng> glatlngList = Store_GLatLng.Find(g => g.Retailerid > 0 && !string.IsNullOrEmpty(g.GLat) && !string.IsNullOrEmpty(g.Glng)).ToList();
                    ConvertMap map = new ConvertMap();
                    map.AddMap("RetailerId", "Retailerid");
                    map.AddMap("GLat", "Glat");
                    List<StoreGLatLng> allGlatlng = ConvertController<StoreGLatLng, Store_GLatLng>.ConvertData(glatlngList);

                    dic = new Dictionary<int, List<StoreGLatLng>>();
                    foreach (var gl in allGlatlng)
                    {
                        if (dic.ContainsKey(gl.Retailerid))
                        {
                            dic[gl.Retailerid].Add(gl);
                        }
                        else
                        {
                            List<StoreGLatLng> list = new List<StoreGLatLng>();
                            list.Add(gl);
                            dic.Add(gl.Retailerid, list);
                        }
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, List<RetailerCache>> GetMultiCountryRetailersWithVotesSumOrderByClicksDic()
        {
            Dictionary<int, List<RetailerCache>> multiDic = new Dictionary<int, List<RetailerCache>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                VelocityController vc = MultiCountryController.GetVelocityController(countryId);
                List<RetailerCache> list = GetRetailersWithVotesSumOrderByClicksFromVelocity(vc, countryId);
                multiDic.Add(countryId, list);
            }

            return multiDic;
        }

        public static List<CSK_Store_RetailerVotesSum> GetRetailerVotesSums(int countryId)
        {
            List<CSK_Store_RetailerVotesSum> votes = new List<CSK_Store_RetailerVotesSum>();

            var connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            string sql = "GetAllRetailerReview";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                SqlParameter pm = new SqlParameter("@countryid", System.Data.SqlDbType.Int);
                pm.Value = countryId;
                cmd.Parameters.Add(pm);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        CSK_Store_RetailerVotesSum vote = new CSK_Store_RetailerVotesSum();
                        vote.ID = int.Parse(idr["ID"].ToString());
                        vote.RetailerID = int.Parse(idr["RetailerID"].ToString());
                        vote.RetailerRatingSum = int.Parse(idr["RetailerRatingSum"].ToString());
                        vote.RetailerTotalRatingVotes = int.Parse(idr["RetailerTotalRatingVotes"].ToString());

                        votes.Add(vote);
                    }
                }
            }

            return votes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vc"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<RetailerCache> GetRetailersWithVotesSumOrderByClicksFromDB(int countryId)
        {
            List<RetailerCache> list = null;
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(SubSonic.DataProviders.ProviderFactory.GetProvider("CommerceTemplate")))//造Index velocity专用
            {
                List<CSK_Store_Retailer> retailerList = CSK_Store_Retailer
                            .Find(rt => rt.IsSetupComplete == true && rt.RetailerStatus != 99 && rt.RetailerCountry == countryId)
                            .OrderBy(rt => rt.RetailerName).ToList();

                list = ConvertController<RetailerCache, CSK_Store_Retailer>.ConvertData(retailerList);

                List<CSK_Store_RetailerVotesSum> retailerVotes = GetRetailerVotesSums(countryId);
                Dictionary<int, string> retailerStoreTypeDic = GetRetailerStroeType(countryId);
                Dictionary<int, int> clicksDic = new Dictionary<int, int>();

                clicksDic = GetRetailerClicks(countryId, System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString);

                foreach (var item in list)
                {
                    var vote = retailerVotes.FirstOrDefault(v => v.RetailerID == item.RetailerId);
                    if (vote != null)
                    {
                        item.RetailerRatingSum = vote.RetailerRatingSum;
                        item.RetailerTotalRatingVotes = vote.RetailerTotalRatingVotes;
                    }
                    else
                    {
                        item.RetailerRatingSum = 3;
                        item.RetailerTotalRatingVotes = 1;
                    }

                    if (retailerStoreTypeDic.ContainsKey(item.StoreType))
                    {
                        item.StoreTypeName = retailerStoreTypeDic[item.StoreType];
                    }

                    item.AvRating = 0;
                    if (item.RetailerTotalRatingVotes > 1)
                    {
                        int totalReviews = (item.RetailerTotalRatingVotes == 0 ? 2 : item.RetailerTotalRatingVotes) - 1;
                        string reviewStr = "";
                        if (PriceMeDBStatic.ListVersionNoEnglishCountryid.Contains(item.RetailerCountry))
                            reviewStr = string.Format("{0} " + System.Configuration.ConfigurationSettings.AppSettings["reviewStr"], totalReviews);
                        else
                            reviewStr = string.Format("{0} " + System.Configuration.ConfigurationSettings.AppSettings["reviewStr"] + "{1}", totalReviews, totalReviews > 1 ? "s" : "");
                        item.ReviewString = reviewStr;

                        decimal avRating = decimal.Round(((item.RetailerRatingSum - 3m) / ((item.RetailerTotalRatingVotes == 0 ? 2 : item.RetailerTotalRatingVotes) - 1m)), 1);
                        avRating = avRating.ToString().Length > 3 ? decimal.Parse(avRating.ToString().Substring(0, 3)) : avRating;
                        item.AvRating = avRating;
                    }

                    int clicks = 0;
                    clicksDic.TryGetValue(item.RetailerId, out clicks);
                    item.Clicks = clicks;
                }

                LogController.WriteLog("CountryId: " + countryId + " RetailerListOrderByName no velocity");
            }

            return list.OrderByDescending(r => r.Clicks).ToList();
        }

        public static List<RetailerCache> GetRetailersWithVotesSumOrderByClicksFromVelocity(VelocityController vc, int countryId)
        {
            List<RetailerCache> list = null;
            if (vc != null)
            {
                list = vc.GetCache<List<RetailerCache>>(VelocityCacheKey.RetailerListOrderByNameWithClicks);
            }

            if (list == null || list.Count == 0)
            {
                using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(SubSonic.DataProviders.ProviderFactory.GetProvider("CommerceTemplate")))//造Index velocity专用
                {
                    List<CSK_Store_Retailer> retailerList = CSK_Store_Retailer
                             .Find(rt => rt.IsSetupComplete == true && rt.RetailerStatus != 99 && rt.RetailerCountry == countryId)
                             .OrderBy(rt => rt.RetailerName).ToList();

                    list = ConvertController<RetailerCache, CSK_Store_Retailer>.ConvertData(retailerList);

                    List<CSK_Store_RetailerVotesSum> retailerVotes = GetRetailerVotesSums(countryId);

                    Dictionary<int, string> retailerStoreTypeDic = GetRetailerStroeType(countryId);

                    foreach (var item in list)
                    {
                        var vote = retailerVotes.FirstOrDefault(v => v.RetailerID == item.RetailerId);
                        if (vote != null)
                        {
                            item.RetailerRatingSum = vote.RetailerRatingSum;
                            item.RetailerTotalRatingVotes = vote.RetailerTotalRatingVotes;
                        }
                        else
                        {
                            item.RetailerRatingSum = 3;
                            item.RetailerTotalRatingVotes = 1;
                        }

                        if(retailerStoreTypeDic.ContainsKey(item.StoreType))
                        {
                            item.StoreTypeName = retailerStoreTypeDic[item.StoreType];
                        }

                        item.AvRating = 0;
                        if (item.RetailerTotalRatingVotes > 1)
                        {
                            int totalReviews = (item.RetailerTotalRatingVotes == 0 ? 2 : item.RetailerTotalRatingVotes) - 1;
                            string reviewStr = "";
                            if (PriceMeDBStatic.ListVersionNoEnglishCountryid.Contains(item.RetailerCountry))
                                reviewStr = string.Format("{0} " + System.Configuration.ConfigurationSettings.AppSettings["reviewStr"], totalReviews);
                            else
                                reviewStr = string.Format("{0} " + System.Configuration.ConfigurationSettings.AppSettings["reviewStr"] + "{1}", totalReviews, totalReviews > 1 ? "s" : "");
                            item.ReviewString = reviewStr;

                            decimal avRating = decimal.Round(((item.RetailerRatingSum - 3m) / ((item.RetailerTotalRatingVotes == 0 ? 2 : item.RetailerTotalRatingVotes) - 1m)), 1);
                            avRating = avRating.ToString().Length > 3 ? decimal.Parse(avRating.ToString().Substring(0, 3)) : avRating;
                            item.AvRating = avRating;
                        }

                    }

                    LogController.WriteLog("CountryId: " + countryId + " RetailerListOrderByNameWithClicks no velocity");
                }
            }

            return list;
        }

        private static Dictionary<int, string> GetRetailerStroeType(int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return CSK_Store_RetailerStoreType.All().ToDictionary(rst => (int)rst.RetailerStoreTypeID, rst => rst.StoreTypeName);
            }
        }

        private static Dictionary<int, int> GetRetailerClicks(int countryId, string connString)
        {
            Dictionary<int, int> retailerClicks = new Dictionary<int, int>();

            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand("CSK_Store_12RMB_Index_GetRetailerClicks", sqlConn))
                {
                    sqlConn.Open();
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCMD.CommandTimeout = 0;
                    SqlParameter countryIdParam = new SqlParameter("@country", countryId);
                    sqlCMD.Parameters.Add(countryIdParam);

                    using (IDataReader idr = sqlCMD.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            retailerClicks.Add(int.Parse(idr["RetailerId"].ToString()), int.Parse(idr["clicks"].ToString()));
                        }
                    }
                }
            }

            return retailerClicks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retailerID"></param>
        /// <returns></returns>
        public static RetailerCache GetRetailerFromCache(int retailerId, int countryId)
        {
            if (MultiCountryAllActiveRetailersDic_Static.ContainsKey(countryId) && MultiCountryAllActiveRetailersDic_Static[countryId].ContainsKey(retailerId))
            {
                return MultiCountryAllActiveRetailersDic_Static[countryId][retailerId];
            }

            return null;
        }

        public static RetailerCache GetRetailerFromDB(int retailerId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                CSK_Store_Retailer retailer = CSK_Store_Retailer.SingleOrDefault(rt => rt.RetailerId == retailerId);
                if (retailer == null)
                    return null;

                retailer.RetailerRatingSum = 3;
                retailer.RetailerTotalRatingVotes = 1;

                var retailerCache = ConvertController<RetailerCache, CSK_Store_Retailer>.ConvertData(retailer);
                return retailerCache;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retailerId"></param>
        /// <returns></returns>
        public static RetailerCache GetRetailerDeep(int retailerId, int countryId)
        {
            RetailerCache retailer = GetRetailerFromCache(retailerId, countryId);
            if(retailer == null)
            {
                retailer = GetRetailerFromDB(retailerId, countryId);
            }

            if (retailer != null && string.IsNullOrEmpty(retailer.LogoFile))
                retailer.LogoFile = @"\images\retailerimages\no_retailer_image.png";

            return retailer;
        }

        public static List<RetailerReviewCache> GetRetailerReviewByRetailerId(int retailerId, int countryId)
        {
            if (MultiCountryRetailerReviewCaches_Static.ContainsKey(countryId))
            {
                var list = MultiCountryRetailerReviewCaches_Static[countryId].FindAll(p => p.RetailerID == retailerId).ToList();

                return list;
            }

            return null;
        }

        public static List<RetailerReviewCache> GetAllRetailerReviews(int countryId)
        {
            if (MultiCountryRetailerReviewCaches_Static.ContainsKey(countryId))
            {
                var list = MultiCountryRetailerReviewCaches_Static[countryId];
                return list;
            }

            return null;
        }

        public static List<CSK_Store_RetailerReview> GetRetailerReviewByAuthorAndIsApproved(string author, bool isApproved, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                List<CSK_Store_RetailerReview> rrc = CSK_Store_RetailerReview.Find(rv => rv.CreatedBy == author && (rv.IsApproved ?? false) == isApproved).ToList();
                return rrc;
            }
        }

        public static CSK_Store_RetailerReview GetRetailerReview(int rrId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return CSK_Store_RetailerReview.SingleOrDefault(rrc => rrc.RetailerReviewId == rrId);
            }
        }

        /// <summary>
        /// （需要修改）
        /// </summary>
        /// <param name="retailerId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static IDataReader GetRetailerCrumbs(int retailerId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("CSK_Store_Retailer_GetRetailerCrumbs");
                sp.Command.AddParameter("@RetailerID", retailerId, DbType.Int32);
                IDataReader dr = sp.ExecuteReader();

                return dr;
            }
        }

        public static void UpdataSolidShopLocation(int gID, string lat, string lng, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("CSK_Store_GLatLng_UpdateLatLng");
                sp.Command.AddParameter("@gID", gID, DbType.Int32);
                sp.Command.AddParameter("@GLat", lat, DbType.String);
                sp.Command.AddParameter("@GLng", lng, DbType.String);

                sp.Execute();
            }
        }

        public static DataTable GetLocationByRetailerID(int retailerId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT ID, GLat,Glng,Location,Description from dbo.Store_GLatLng sg where sg.Retailerid={0}", retailerId), conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

        public static List<RetailerPaymentCache> GetRetailerPaymentByRetailerID(int retailerId, int countryId)
        {
            if (MultiCountryRetailerPaymentDic_Static.ContainsKey(countryId))
            {
                List<RetailerPaymentCache> retailerPaymentCacheList = MultiCountryRetailerPaymentDic_Static[countryId].Where(p => p.RetailerID == retailerId).ToList();
                return retailerPaymentCacheList;
            }

            return null;
        }

        public static RetailerCache GetRetailerByEN(string en, int countryId)
        {
            string ridString = PriceMeCommon.DesEncrypt.Decrypt(en);

            int rId = int.Parse(ridString);

            return GetRetailerDeep(rId, countryId);
        }

        public static string GetInactiveReason(int retailerId, int countryId)
        {
            if(MultiCountryRetailerInactiveReasonDic_Static.ContainsKey(countryId) && MultiCountryRetailerInactiveReasonDic_Static[countryId].ContainsKey(retailerId))
            { 
                    return MultiCountryRetailerInactiveReasonDic_Static[countryId][retailerId];
            }
            return "";
        }

        public static List<RetailerReviewCache> GetPageRetailerReviews(int pageIndex, int pageSize, int retailerId, int soryBy, string type, out int totalCount, int countryId)
        {
            if (MultiCountryRetailerReviewCaches_Static.ContainsKey(countryId))
            {
                List<RetailerReviewCache> rrcs = new List<RetailerReviewCache>();
                if (type == "web")
                    rrcs = MultiCountryRetailerReviewCaches_Static[countryId].Where(r => r.RetailerID == retailerId && r.SourceType == "web").ToList();
                else if (type == "sys")
                    rrcs = MultiCountryRetailerReviewCaches_Static[countryId].Where(r => r.RetailerID == retailerId && r.SourceType != "web").ToList();
                else
                    rrcs = MultiCountryRetailerReviewCaches_Static[countryId].Where(r => r.RetailerID == retailerId).ToList();

                if (soryBy == 1)//Highest Rating
                    rrcs = rrcs.OrderByDescending(r => r.OverallRating).ThenByDescending(r => r.CreatedOn).ToList();
                else if (soryBy == 2)//Lowest Rating
                    rrcs = rrcs.OrderBy(r => r.OverallRating).ThenByDescending(r => r.CreatedOn).ToList();
                else//Recent (default option)
                    rrcs = rrcs.OrderByDescending(r => r.CreatedOn).ToList();

                totalCount = rrcs.Count;

                List<RetailerReviewCache> retailerReviews = rrcs.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return retailerReviews;
            }

            totalCount = 0;
            return new List<RetailerReviewCache>();
        }

        public static List<RetailerCache> GetRetailerListByIDs(List<int> retailerIds, int countryId)
        {
            List<RetailerCache> retailerList = new List<RetailerCache>();

            foreach (int rId in retailerIds)
            {
                RetailerCache retailer = GetRetailerDeep(rId, countryId);
                if (retailer == null || retailer.RetailerStatus == 99) continue;
                retailerList.Add(retailer);
            }

            return retailerList;
        }

        public static bool IsPPcRetailer(int retailerId, int countryId)
        {
            if(MultiCountryPPcDic_Static.ContainsKey(countryId) && MultiCountryPPcDic_Static[countryId].ContainsKey(retailerId))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取PPCtype=2的
        /// </summary>
        /// <param name="retailerId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static CommonPPCMember GetPPcInfoByRetailerId(int retailerId, int countryId)
        {
            if (MultiCountryPPcDic_Static.ContainsKey(countryId) && MultiCountryPPcDic_Static[countryId].ContainsKey(retailerId))
            {
                return MultiCountryPPcDic_Static[countryId][retailerId];
            }
            return null;
        }

        public static bool IsInternationalRetailer(int retailerId, int countryId)
        {
            if (MultiCountryInternationalDic_Static.ContainsKey(countryId) && MultiCountryInternationalDic_Static[countryId].ContainsKey(retailerId))
            {
                return true;
            }
            return false;
        }

        public static bool IsCompleteRetailer(int retailerId, int countryId)
        {
            if (MultiCountryAllActiveRetailersDic_Static.ContainsKey(countryId) && MultiCountryAllActiveRetailersDic_Static[countryId].ContainsKey(retailerId))
            {
                return true;
            }
            return false;
        }

        public static List<StoreGLatLng> GetRetailerGLatLng(int retailerId, int countryId)
        {
            if(MultiCountryGLatLngCacheDic_Static.ContainsKey(countryId) && MultiCountryGLatLngCacheDic_Static[countryId].ContainsKey(retailerId))
            {
                return MultiCountryGLatLngCacheDic_Static[countryId][retailerId];
            }

            return new List<StoreGLatLng>();
        }

        public static List<StoreGLatLng> FindStoreGLatLngByCity(string city, int countryId)
        {
            if(MultiCountryAllGLatLngCaches_Static.ContainsKey(countryId))
            {
                return MultiCountryAllGLatLngCaches_Static[countryId].FindAll(gl => gl.PostalCity.Equals(city));
            }

            return new List<StoreGLatLng>();
        }

        public static List<int> GetAllPPcRetaielrIds(int countryId)
        {
            if(MultiCountryPPcDic_Static.ContainsKey(countryId))
            {
                return MultiCountryPPcDic_Static[countryId].Keys.ToList();
            }
            return new List<int>();
        }

        public static List<int> GetAllNoLinkRetaielrIds(int countryId)
        {
            if (MultiCountryNolinkDic_Static.ContainsKey(countryId))
            {
                return MultiCountryNolinkDic_Static[countryId].Keys.ToList();
            }
            return new List<int>();
        }

        public static CommonPPCMember GetRetailerPPcMember(int retailerId, int countryId)
        {
            if(MultiCountryAllPPcMemberDic_Static.ContainsKey(countryId) && MultiCountryAllPPcMemberDic_Static[countryId].ContainsKey(retailerId))
            {
                return MultiCountryAllPPcMemberDic_Static[countryId][retailerId];
            }

            return null;
        }

        //public static List<StoreGLatLng> GetStoreGLatLngs(int retailerId, int countryId)
        //{
        //    if(MultiCountryStoreGLatLngs_Static.ContainsKey(countryId))
        //    {
        //        return MultiCountryStoreGLatLngs_Static[countryId].Where(sgl => sgl.Retailerid == retailerId).ToList();
        //    }

        //    return null;
        //}

        public static StoreGRegion GetStoreGRegionByCode(string regionCode, int countryId)
        {
            if (MultiCountryStoreGRegions_Static.ContainsKey(countryId))
            {
                StoreGRegion gr = MultiCountryStoreGRegions_Static[countryId].SingleOrDefault(g => g.RegionCode == regionCode);
            }

            return new StoreGRegion();
        }

        public static StoreGRegion GetStoreGRegion(int regionId, int countryId)
        {
            if (MultiCountryStoreGRegions_Static.ContainsKey(countryId))
            {
                StoreGRegion gr = MultiCountryStoreGRegions_Static[countryId].SingleOrDefault(g => g.RegionID == regionId);
            }

            return new StoreGRegion();
        }

        public static string GetRetailerCategoryName(int retailerCategoryId, int countryId)
        {
            if(MultiCountryRetailerCategoryCacheDic_Static.ContainsKey(countryId) && MultiCountryRetailerCategoryCacheDic_Static[countryId].ContainsKey(retailerCategoryId))
            {
                return MultiCountryRetailerCategoryCacheDic_Static[countryId][retailerCategoryId].RetailerCategoryName;
            }

            return "";
        }

        public static List<RetailerCategoryCache> GetRetailerCategoryCaches(int countryId)
        {
            if (MultiCountryRetailerCategoryCacheDic_Static.ContainsKey(countryId))
            {
                return MultiCountryRetailerCategoryCacheDic_Static[countryId].Values.ToList();
            }

            return null;
        }

        public static List<RetailerCache> GetRetailersByCategory(int retailerCategoryId, int countryId)
        {
            if(MultiCountryRetailerCategoryDic_Static.ContainsKey(countryId) && MultiCountryRetailerCategoryDic_Static[countryId].ContainsKey(retailerCategoryId))
            {
                return MultiCountryRetailerCategoryDic_Static[countryId][retailerCategoryId];
            }

            return null;
        }

        public static List<RetailerCache> GetAllActiveRetailersWithVotesSumOrderByClicks(int countryId)
        {
            if(MultiCountryAllActiveRetailersWithVotesSumOrderByClicksDic_Static.ContainsKey(countryId))
            {
                return MultiCountryAllActiveRetailersWithVotesSumOrderByClicksDic_Static[countryId];
            }

            return null;
        }

        public static DBCountryInfo GetUtilCountry(int countryId)
        {
            if (UtilCountryInfoDic_Static.ContainsKey(countryId))
            {
                return UtilCountryInfoDic_Static[countryId];
            }
            return null;
        }

        public static Dictionary<int, RetailerCache> GetAllActiveDirectoryRetailers(int countryId)
        {
            if(MultiCountryDirectoryRetailersDic_Static.ContainsKey(countryId))
            {
                return MultiCountryDirectoryRetailersDic_Static[countryId];
            }

            return null;
        }
    }
}