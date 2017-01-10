using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using SubSonic.Schema;
using PriceMeCommon.Data;
using PriceMeCache;
using PriceMeCommon.Convert;
using PriceMeCommon.BusinessLogic;
using System.Data.SqlClient;
using System.Configuration;
using Lucene.Net.Search;
using Lucene.Net.Index;

namespace PriceMeCommon
{
    public static class RetailerController
    {
        const int MAXDOCS = 100000;

        static PriceMeDBDB db = PriceMeStatic.PriceMeDB;
        static List<RetailerCategoryCache> _retailerCategoryCache = null;
        static Hashtable _retailerCategoryHT = null;
        public static List<RetailerCategoryCache> RetailerCategoryCache
        {
            get { return RetailerController._retailerCategoryCache; }
        }

        static List<RetailerCache> _retailerListOrderByClicks = null;
        public static List<RetailerCache> RetailerListOrderByClicks
        {
            get { return RetailerController._retailerListOrderByClicks; }
        }

        static List<RetailerCache> _retailerListOrderByName = null;
        public static List<RetailerCache> retailerListOrderByName
        {
            get { return RetailerController._retailerListOrderByName; }
        }

        static List<CSK_Store_RetailerVotesSum> _retailerVotes = null;
        public static List<CSK_Store_RetailerVotesSum> RetailerVotes
        {
            get
            {
                if (_retailerVotes == null)
                    _retailerVotes = GetRetailerVotesSums(ConfigAppString.CountryID);
                return RetailerController._retailerVotes;
            }
        }

        static List<RetailerReviewCache> _retailerReviews = null;
        public static List<RetailerReviewCache> RetailerReviews
        {
            get { return RetailerController._retailerReviews; }
        }
        public static List<RetailerReviewCache> TotalRetailerReviews;

        static Dictionary<int, CountryInfo> _InternationalRetailers;
        public static Dictionary<int, CountryInfo> InternationalRetailers
        { get { return _InternationalRetailers; } }

        static object lockObj = new object();

        private static Hashtable _allActiveDirectoryRetailers;
        public static Hashtable AllActiveDirectoryRetailers
        {
            get { return _allActiveDirectoryRetailers; }
        }

        private static List<GLatLngCache> _AllGlatlng = null;
        public static List<GLatLngCache> AllGlatlng
        {
            get { return RetailerController._AllGlatlng; }
        }

        private static Dictionary<int, List<PriceMeCache.GLatLngCache>> _GLatLngCacheDictionary;
        public static Dictionary<int, List<PriceMeCache.GLatLngCache>> GLatLngCacheDictionary
        {
            get { return RetailerController._GLatLngCacheDictionary; }
        }
        private static List<PriceMeCache.RetailerOperatingHours> _OperatingHoursList = null;
        public static List<PriceMeCache.RetailerOperatingHours> OperatingHoursList
        {
            get { return RetailerController._OperatingHoursList; }
        }

        static Dictionary<int, string> RetailerInactiveReason = null;

        private static List<PriceMeCache.RetailerPaymentCache> _RetailerPaymentList = null;
        public static List<PriceMeCache.RetailerPaymentCache> RetailerPaymentList
        {
            get
            {
                //if (_RetailerPaymentList == null)
                //    _RetailerPaymentList = GetRetailerPaymentCache();
                return RetailerController._RetailerPaymentList;
            }
        }
        /// <summary>
        /// 刷新时间
        /// </summary>
        //private static DateTime _refreshTime = DateTime.Now;
        private static Dictionary<int, RetailerCache> _allActiveRetailers = null;
        private static Hashtable _allPPCMember;
        private static Hashtable _allNolinkPPCMember;
        private static Hashtable _allActiveRetailersGroupByCategory;
        private static List<int> _isCompleteId;
        private static List<int> _retailerStatusList;
        //private static Hashtable _allPickUpRetailers; 这个从定义上看和 AllActiveRetailers 是完全一样的。

        //Retailer Map
        public static List<StoreUserLocation> userLocations;
        public static List<StoreGLatLng> gLatLngs;
        public static List<StoreGRegion> gRegions;

        public static Dictionary<int, GovernmentBadgeCache> GoverCache;

        public static List<TradeMeSeller> TradeMeSeller;


        public static bool IsPPC(int retailerId)
        {
            if (_allPPCMember.ContainsKey(retailerId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Hashtable AllActiveRetailersGroupByCategory
        { get { return _allActiveRetailersGroupByCategory; } }

        #region by ruangang

        public static Dictionary<int, RetailerCache> AllActiveRetailers
        {
            get
            {
                if (System.Web.HttpRuntime.Cache["Retailers"] != null)
                    return (System.Web.HttpRuntime.Cache["Retailers"] as Dictionary<int, RetailerCache>);
                else
                {
                    UpdateRetailerCache();
                    return _allActiveRetailers;
                }
            }
            private set
            {
                _allActiveRetailers = value;
            }
        }

        public static Hashtable AllPPCMember
        {
            get { return _allPPCMember; }
        }

        public static List<int> IsCompleteId
        {
            get { return _isCompleteId; }
        }

        public static List<int> RetailerStatusList
        {
            get { return _retailerStatusList; }
        }

        public static Hashtable AllNoLinkPPCMember
        {
            get
            {
                return _allNolinkPPCMember;
            }
        }

        //public static Hashtable AllPickUpRetailers {
        //    get { return RetailerController._allPickUpRetailers; }
        //    private set { RetailerController._allPickUpRetailers = value; }
        //}

        #endregion

        public static void Load(Timer.DKTimer dkTimer)
        {
            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor load all retailer");
            }

            _retailerListOrderByName = VelocityController.GetCache<List<RetailerCache>>(VelocityCacheKey.RetailerListOrderByName);
            if (_retailerListOrderByName == null || _retailerListOrderByName.Count == 0)
            {
                GetRetailerListOrderByName();
            }
            else
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "Velocity RetailerListOrderByName Count : " + _retailerListOrderByName.Count);
            }

            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor load all retailer maps and operating hours");
            }
            _AllGlatlng = VelocityController.GetCache<List<PriceMeCache.GLatLngCache>>(VelocityCacheKey.AllGlatLngs);
            if (_AllGlatlng == null)
            {
                List<Store_GLatLng> _GlatlngList = Store_GLatLng.Find(g => g.Retailerid > 0 && !String.IsNullOrEmpty(g.GLat) && !String.IsNullOrEmpty(g.Glng)).ToList();
                ConvertMap map = new ConvertMap();
                map.AddMap("RetailerId", "Retailerid");
                map.AddMap("GLat", "Glat");
                _AllGlatlng = ConvertController<GLatLngCache, Store_GLatLng>.ConvertData(_GlatlngList);
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AllGlatlng no velocity");
            }

            _GLatLngCacheDictionary = new Dictionary<int, List<PriceMeCache.GLatLngCache>>();
            foreach (PriceMeCache.RetailerCache retailer in PriceMeCommon.RetailerController.retailerListOrderByName)
            {
                if (_AllGlatlng.Count > 0)
                {
                    List<PriceMeCache.GLatLngCache> glatlngCacle = _AllGlatlng.Where(g => g.Retailerid == retailer.RetailerId).ToList();
                    _GLatLngCacheDictionary.Add(retailer.RetailerId, glatlngCacle);
                }
            }

            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor load RetailerReviewList");
            }
            _retailerReviews = VelocityController.GetCache<List<PriceMeCache.RetailerReviewCache>>(VelocityCacheKey.RetailerReviewList);
            if (_retailerReviews == null)
            {
                GetRetailerReviewsCache();
            }
            _OperatingHoursList = VelocityController.GetCache<List<PriceMeCache.RetailerOperatingHours>>(VelocityCacheKey.RetailerOperatingHours);
            if (_OperatingHoursList == null)
            {
                List<CSK_Store_RetailerOperatingHour> _OperatHoursList = CSK_Store_RetailerOperatingHour.Find(g => g.RetailerId > 0).ToList();
                _OperatingHoursList = ConvertController<RetailerOperatingHours, CSK_Store_RetailerOperatingHour>.ConvertData(_OperatHoursList);
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AllGlatlng no velocity");
            }

            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor load all retailer clicks");
            }

            Dictionary<int, int> retailerClicks = VelocityController.GetCache<Dictionary<int, int>>(Data.VelocityCacheKey.RetailerClicks);
            if (retailerClicks != null)
            {
                for (int i = 0; i < _retailerListOrderByName.Count; i++)
                {
                    if (retailerClicks.ContainsKey(_retailerListOrderByName[i].RetailerId))
                    {
                        _retailerListOrderByName[i].Clicks = retailerClicks[_retailerListOrderByName[i].RetailerId];
                    }
                }
                _retailerListOrderByClicks = _retailerListOrderByName.OrderByDescending(r => r.Clicks).ToList();
            }
            else
            {
                _retailerListOrderByClicks = _retailerListOrderByName;
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "RetailerClicks no velocity");
            }

           // _RetailerPaymentOptionList = ConvertController<RetailerPaymentCache, CSK_Store_RetailerOperatingHour>.ConvertData(_RetailerPaymentOptionList);

            LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "Retailer cache count : " + _retailerListOrderByName.Count);

            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor LoadRetailerCategoryCache()");
            }
            LoadRetailerCategoryCache();

            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor LoadActiveRetailersGroupByCategory()");
            }
            LoadActiveRetailersGroupByCategory();

            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor UpdateRetailerCache()");
            }
            UpdateRetailerCache();

            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor UpdatePPCMemberCache()");
            }
            UpdatePPCMemberCache();

            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor BindRetailerMap()");
            }
            BindRetailerMap();

            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor GetRetailerInactiveReason()");
            }
            RetailerInactiveReason = GetRetailerInactiveReason();

            if (dkTimer != null)
            {
                dkTimer.Set("RetailerController.Load() --- Befor load GovernmentBadge");
            }
            BindGovernmentBadge();
            GetRetailerPaymentCache();
            GetTradeMeSeller();
            //StartCheckModify();
        }

        //static System.Threading.Thread checkModifyThread;
        //public static void StartCheckModify()
        //{
        //    checkModifyThread = new System.Threading.Thread(new System.Threading.ThreadStart(CheckUpdatePPCMemberCache));
        //    checkModifyThread.Start();
        //}

        //private static void CheckUpdatePPCMemberCache()
        //{
        //    int interval = ConfigAppString.Interval;

        //    while (true)
        //    {
        //        UpdatePPCMemberCache();
        //        System.Threading.Thread.CurrentThread.Join(interval);
        //    }
        //}

        //public static void EndCheck()
        //{
        //    if (checkModifyThread != null && checkModifyThread.IsAlive)
        //        checkModifyThread.Abort();
        //}
        public static void GetTradeMeSeller() {
            
            try
            {
                TradeMeSeller = new List<TradeMeSeller>();

                if (ConfigAppString.CountryID == 3)
                {
                    string sql = "SELECT [Id],[MemberId],[MemberName],[RetailerId],[Status] FROM [pam_user].[dbo].[TradeMeSeller]";
                    StoredProcedure sp = new StoredProcedure("");
                    sp.Command.CommandSql = sql;
                    sp.Command.CommandTimeout = 0;
                    sp.Command.CommandType = CommandType.Text;
                    IDataReader dr = sp.ExecuteReader();
                    while (dr.Read())
                    {
                        TradeMeSeller tms = new TradeMeSeller();
                        int id = 0;
                        int.TryParse(dr["Id"].ToString(), out id);
                        bool Status = false;
                        bool.TryParse(dr["Status"].ToString(), out Status);
                        string MemberId = dr["MemberId"].ToString();
                        string MemberName = dr["MemberName"].ToString();
                        int RetailerId = 0;
                        int.TryParse(dr["RetailerId"].ToString(), out RetailerId);

                        tms.id = id;
                        tms.MemberId = MemberId;
                        tms.RetailerId = RetailerId;
                        tms.Status = Status;
                        tms.MemberName = MemberName;

                        TradeMeSeller.Add(tms);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static void Load()
        {
            Load(null);
        }

        public static RetailerCache GetRetailerFromDBByID(int retailerID)
        {
            CSK_Store_Retailer _retailer = CSK_Store_Retailer.SingleOrDefault(rt => rt.RetailerId == retailerID);
            if (_retailer == null) return null;
            var vote = RetailerVotes.FirstOrDefault(v => v.RetailerID == _retailer.RetailerId);
            if (vote != null)
            {
                _retailer.RetailerRatingSum = vote.RetailerRatingSum;
                _retailer.RetailerTotalRatingVotes = vote.RetailerTotalRatingVotes;
            }
            else
            {
                _retailer.RetailerRatingSum = 3;
                _retailer.RetailerTotalRatingVotes = 1;
            }
            var retailer = ConvertController<RetailerCache, CSK_Store_Retailer>.ConvertData(_retailer);
            return retailer;
        }

        public static RetailerCache GetRetailerByID(int retailerID)
        {
            if (AllActiveRetailers != null && AllActiveRetailers.ContainsKey(retailerID))
            {
                return AllActiveRetailers[retailerID];
            }

            CSK_Store_Retailer _retailer = CSK_Store_Retailer.SingleOrDefault(rt => rt.RetailerId == retailerID);
            if (_retailer == null) return null;
            var vote = RetailerVotes.FirstOrDefault(v => v.RetailerID == _retailer.RetailerId);
            if (vote != null)
            {
                _retailer.RetailerRatingSum = vote.RetailerRatingSum;
                _retailer.RetailerTotalRatingVotes = vote.RetailerTotalRatingVotes;
            }
            else
            {
                _retailer.RetailerRatingSum = 3;
                _retailer.RetailerTotalRatingVotes = 1;
            }
            var retailer = ConvertController<RetailerCache, CSK_Store_Retailer>.ConvertData(_retailer);

            lock (lockObj)
            {
                if (_allActiveRetailers == null)
                {
                    _allActiveRetailers = new Dictionary<int, RetailerCache>();
                    _allActiveRetailers.Add(retailerID, retailer);
                }
                else if (!_allActiveRetailers.ContainsKey(retailerID))
                {
                    _allActiveRetailers.Add(retailerID, retailer);
                }
            }
            return retailer;
        }

        public static int GetAllRetailerReviewsCountByAuthorName(string author)
        {
            int count = 0;
            SubSonic.Schema.StoredProcedure sp = db.GetAllRetailerReviewCountByAuthorName();
            sp.Command.AddParameter("@author", author, DbType.String);
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
                count = int.Parse(dr[0].ToString());
            dr.Close();

            return count;
        }

        #region GetReviewsCount(int retailerID)

        public static int GetReviewsCount(int retailerID)
        {
            return CSK_Store_RetailerReview.Find(r => r.RetailerId == retailerID).Count();
        }

        #endregion

        #region yuanxiang

        public static Hashtable RetailerCategoryHT
        {
            get { return RetailerController._retailerCategoryHT; }
        }

        public static void LoadRetailerCategoryCache()
        {
            _retailerCategoryCache = new List<RetailerCategoryCache>();
            _retailerCategoryHT = new Hashtable();
            PriceMeDBDB db = PriceMeStatic.PriceMeDB;
            SubSonic.Schema.StoredProcedure sp = db.CSK_Store_Retailer_GetRetailerCategoryTotal();
            IDataReader idrRTC = sp.ExecuteReader();
            RetailerCategoryCache rtc = null;
            while (idrRTC.Read())
            {
                rtc = new RetailerCategoryCache();
                rtc.RetailerCategoryID = idrRTC["RetailerCategoryId"].ToString();
                rtc.RetailerCategoryTotal = int.Parse(idrRTC["Total"].ToString());
                rtc.RetailerCategoryName = idrRTC["RetailerCategoryName"].ToString();

                _retailerCategoryCache.Add(rtc);

                if (!_retailerCategoryHT.Contains(int.Parse(rtc.RetailerCategoryID)))
                    _retailerCategoryHT.Add(int.Parse(rtc.RetailerCategoryID), rtc.RetailerCategoryName);
            }
            idrRTC.Close();
        }

        private static void LoadDirectoryRetailers()
        {
            _allActiveDirectoryRetailers = new Hashtable();
            SubSonic.Schema.StoredProcedure sp = db.GetALLActiveDirectory();
            IDataReader idrAllActiveDirectoryRetailers = sp.ExecuteReader();

            while (idrAllActiveDirectoryRetailers.Read())
            {
                CSK_Store_Retailer retailer = new CSK_Store_Retailer();
                int retailerid = int.Parse(idrAllActiveDirectoryRetailers["RetailerId"].ToString());
                retailer.RetailerId = retailerid;
                retailer.StoreType = byte.Parse(idrAllActiveDirectoryRetailers["StoreType"].ToString());
                string retailerMsg = idrAllActiveDirectoryRetailers["RetailerMessage"].ToString();
                retailer.RetailerMessage = retailerMsg.Length > 50 ? retailerMsg.Substring(0, 50) : retailerMsg;
                retailer.RetailerName = idrAllActiveDirectoryRetailers["RetailerName"].ToString();
                retailer.RetailerURL = idrAllActiveDirectoryRetailers["RetailerURL"].ToString();
                retailer.RetailerRatingSum = int.Parse(idrAllActiveDirectoryRetailers["RetailerRatingSum"].ToString());
                retailer.RetailerTotalRatingVotes = int.Parse(idrAllActiveDirectoryRetailers["RetailerTotalRatingVotes"].ToString());
                retailer.LogoFile = idrAllActiveDirectoryRetailers["LogoFile"].ToString();
                retailer.RetailerPhone = idrAllActiveDirectoryRetailers["RetailerPhone"].ToString();
                retailer.RetailerShortDescription = idrAllActiveDirectoryRetailers["RetailerShortDescription"].ToString();
                retailer.ContactEmail = idrAllActiveDirectoryRetailers["ContactEmail"].ToString();
                retailer.PriceDescriptor = idrAllActiveDirectoryRetailers["PriceDescriptor"].ToString();
                retailer.RetailerAffiliates = idrAllActiveDirectoryRetailers["RetailerAffiliates"].ToString();
                retailer.LocationCategoryId = int.Parse(idrAllActiveDirectoryRetailers["LocationCategoryId"].ToString());
                if (!_allActiveDirectoryRetailers.ContainsKey(retailerid))
                    _allActiveDirectoryRetailers.Add(retailerid, retailer);
            }
        }

        public static Data.RetailerCategoryCache GetRetailerCategoryCacheByRCId(int rcId)
        {
            if (_retailerCategoryCache != null)
            {
                foreach (Data.RetailerCategoryCache retailerCategoryCache in _retailerCategoryCache)
                {
                    if (retailerCategoryCache.RetailerCategoryID == rcId.ToString())
                        return retailerCategoryCache;
                }
            }

            return null;
        }

        public static void LoadActiveRetailersGroupByCategory()
        {
            _allActiveRetailersGroupByCategory = new Hashtable();

            foreach (RetailerCache each in retailerListOrderByName)
            {
                if (!_allActiveRetailersGroupByCategory.Contains(each.RetailerCategory))
                {
                    List<RetailerCache> retailers = new List<RetailerCache>();
                    retailers.Add(each);
                    _allActiveRetailersGroupByCategory.Add(each.RetailerCategory, retailers);
                }
                else
                {
                    List<RetailerCache> retailers = _allActiveRetailersGroupByCategory[each.RetailerCategory] as List<RetailerCache>;
                    retailers.Add(each);
                    _allActiveRetailersGroupByCategory.Remove(each.RetailerCategory);
                    _allActiveRetailersGroupByCategory.Add(each.RetailerCategory, retailers);
                }
            }
        }

        public static RetailerCache GetRetailerDeep(int retailerId)
        {
            if (AllActiveRetailers.ContainsKey(retailerId))
            {
                RetailerCache retailer = AllActiveRetailers[retailerId];
                if (retailer != null && string.IsNullOrEmpty(retailer.LogoFile))
                    retailer.LogoFile = @"\images\retailerimages\no_retailer_image.png";

                return retailer;
            }
            else
            {
                CSK_Store_Retailer _retailer = CSK_Store_Retailer.SingleOrDefault(rt => rt.RetailerId == retailerId);
                if (_retailer == null) return null;
                var vote = RetailerVotes.FirstOrDefault(v => v.RetailerID == _retailer.RetailerId);
                if (vote != null)
                {
                    _retailer.RetailerRatingSum = vote.RetailerRatingSum;
                    _retailer.RetailerTotalRatingVotes = vote.RetailerTotalRatingVotes;
                }
                else
                {
                    _retailer.RetailerRatingSum = 3;
                    _retailer.RetailerTotalRatingVotes = 1;
                }
                var retailer = ConvertController<RetailerCache, CSK_Store_Retailer>.ConvertData(_retailer);

                return retailer;
            }
        }

        public static List<RetailerReviewCache> GetRetailerReviewByRetailerId(int retailerId)
        {
            //List<CSK_Store_RetailerReview> rrc = (from rv in db.CSK_Store_RetailerReviews where (rv.RetailerId == retailerId && rv.IsApproved == true) orderby rv.CreatedOn descending select rv).ToList();
            //return rrc;

            var list = _retailerReviews.FindAll(p => p.RetailerID == retailerId || p.RetailerId==retailerId).OrderByDescending(r => r.CreatedOn).ToList();
            return list;
        }

        public static List<CSK_Store_RetailerReview> GetRetailerReviewByAuthorAndIsApproved(string author, bool isApproved)
        {
            List<CSK_Store_RetailerReview> rrc = db.CSK_Store_RetailerReviews.Where(rv => rv.CreatedBy == author && (rv.IsApproved ?? false) == isApproved).ToList();
            return rrc;
        }

        public static CSK_Store_RetailerReview GetRetailerReviewById(int Id)
        {
            return CSK_Store_RetailerReview.SingleOrDefault(rrc => rrc.RetailerReviewId == Id);
        }

        public static IDataReader GetRetailerCrumbs(int retailerId)
        {
            SubSonic.Schema.StoredProcedure sp = db.CSK_Store_Retailer_GetRetailerCrumbs();
            sp.Command.AddParameter("@RetailerID", retailerId, DbType.Int32);
            IDataReader dr = sp.ExecuteReader();

            return dr;
        }

        #endregion

        #region ruangang

        public static void UpdateRetailerCache()
        {
            _isCompleteId = new List<int>();
            _retailerStatusList = new List<int>();

            List<CSK_Store_RetailerStoreType> rsts = CSK_Store_RetailerStoreType.All().ToList();
            Dictionary<int, string> rstDic = new Dictionary<int, string>();
            foreach (CSK_Store_RetailerStoreType rst in rsts)
                rstDic.Add(rst.RetailerStoreTypeID, rst.StoreTypeName);

            Dictionary<int, RetailerCache> allActiveRetailers = new Dictionary<int, RetailerCache>();

            foreach (RetailerCache retailer in _retailerListOrderByName)
            {
                if (!retailer.IsSetupComplete || (retailer.RetailerStatus) != 1)
                {
                    if (!retailer.IsSetupComplete)
                        _isCompleteId.Add(retailer.RetailerId);
                    if (retailer.RetailerStatus != 1)
                        _retailerStatusList.Add(retailer.RetailerId);

                    continue;
                }

                if (rstDic.ContainsKey(retailer.StoreType))
                    retailer.StoreTypeName = rstDic[retailer.StoreType];

                if (!allActiveRetailers.ContainsKey(retailer.RetailerId))
                    allActiveRetailers.Add(retailer.RetailerId, retailer);
            }

            _allActiveRetailers = allActiveRetailers;

            if (System.Web.HttpRuntime.Cache["Retailers"] != null)
                System.Web.HttpRuntime.Cache.Remove("Retailers");

            System.Web.HttpRuntime.Cache.Insert("Retailers", allActiveRetailers, null, DateTime.Now.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        static Dictionary<int, CSK_Util_Country> UtilCountryDic = new Dictionary<int, CSK_Util_Country>();
        static System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;
        public static void UpdatePPCMemberCache()
        {
            object _allPPCMemberOBJ = webCache["_allPPCMember"];
            object _allNolinkPPCMemberOBJ = webCache["_allNolinkPPCMember"];

            if (_allPPCMemberOBJ == null || _allNolinkPPCMemberOBJ == null)
            {
                Hashtable ppcMeberHT = new Hashtable();
                Hashtable ppcNoLinkHT = new Hashtable();

                List<CommonPPCMember> ppcs = GetAllCommonPPCMember();

                Dictionary<int, CountryInfo> internationalRetailers = new Dictionary<int, CountryInfo>();
                List<CSK_Util_Country> countrys = CSK_Util_Country.All().ToList();
                UtilCountryDic = countrys.ToDictionary(c => c.countryID);
                foreach (var ppc in ppcs)
                {
                    if (ppc.PPCMemberTypeID == 2)
                    {
                        if (AllActiveRetailers.ContainsKey(ppc.RetailerId))
                        {
                            RetailerCache retailer = (RetailerCache)AllActiveRetailers[ppc.RetailerId];
                            string ppcLogo = "";
                            if (!string.IsNullOrEmpty(retailer.LogoFile) && retailer.LogoFile.Contains('.'))
                            {
                                ppcLogo = retailer.LogoFile.Insert(retailer.LogoFile.LastIndexOf('.'), "_s");
                            }
                            ppc.PPCLogo = ppcLogo;
                            if (!ppcMeberHT.ContainsKey(ppc.RetailerId))
                                ppcMeberHT.Add(ppc.RetailerId, ppc);
                        }
                    }
                    else if (ppc.PPCMemberTypeID == 5 && !ppcNoLinkHT.Contains(ppc.RetailerId))
                    {
                        ppcNoLinkHT.Add(ppc.RetailerId, ppc.RetailerId);
                    }
                    if (ppc.RetailerCountry != ConfigAppString.CountryID)
                    {
                        CountryInfo countryInfo = BindInternationalCountry(countrys, ppc.RetailerCountry);
                        internationalRetailers.Add(ppc.RetailerId, countryInfo);
                    }
                }

                CountryInfo defaultcountryInfo = BindInternationalCountry(countrys, ConfigAppString.CountryID);
                internationalRetailers.Add(-1, defaultcountryInfo);

                _allPPCMember = ppcMeberHT;
                _allNolinkPPCMember = ppcNoLinkHT;
                _InternationalRetailers = internationalRetailers;

                webCache.Add("_allPPCMember", _allPPCMember, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0,20,0), System.Web.Caching.CacheItemPriority.High, null);
                webCache.Add("_allNolinkPPCMember", _allNolinkPPCMember, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 20, 0), System.Web.Caching.CacheItemPriority.High, ReloadPPCMemberCache);
            }
        }

        public static void ReloadPPCMemberCache(string key, object value, System.Web.Caching.CacheItemRemovedReason reason)
        {
            UpdatePPCMemberCache();
        }

        private static List<CommonPPCMember> GetAllCommonPPCMember()
        {
            List<CommonPPCMember> list = new List<CommonPPCMember>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString))
                {
                    string sql = @"SELECT 
                                [RetailerId]
                                ,[PPCMemberTypeID]
                                ,[RetailerCountry]
                                FROM [CSK_Store_PPCMember]";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandTimeout = 0;
                    using(IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            CommonPPCMember ppc = new CommonPPCMember();
                            ppc.RetailerId = dr.GetInt32(0);
                            ppc.PPCMemberTypeID = dr.GetInt32(1);
                            ppc.RetailerCountry = dr.GetInt32(2);
                            list.Add(ppc);
                        }
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.ExceptionLogPath, "GetAllCommonPPCMember exception : " + ex.Message);
            }
            return null;
        }

        public static IDataReader StoreRetailerGetRetailerCrumbs(int RetailerID)
        {
            //StoredProcedure sp = new StoredProcedure("CSK_Store_Retailer_GetRetailerCrumbs");
            StoredProcedure sp = db.CSK_Store_Retailer_GetRetailerCrumbs();
            sp.Command.AddParameter("@RetailerID", RetailerID, DbType.Int32);
            return sp.ExecuteReader();
        }

        #endregion

        #region Retailer Map

        private static void BindRetailerMap()
        {
            userLocations = new List<StoreUserLocation>();
            SubSonic.Schema.StoredProcedure sp = new StoredProcedure("");
            string sql = "select * from CSK_Store_UserLocation";
            sp.Command.CommandSql = sql;
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandTimeout = 0;

            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                StoreUserLocation l = new StoreUserLocation();
                l.Id = int.Parse(dr["ID"].ToString());
                l.UserLocation = dr["UserLocation"].ToString();
                bool isn = false;
                bool.TryParse(dr["IsNorthIsland"].ToString(), out isn);
                l.IsNorthIsland = isn;

                userLocations.Add(l);
            }
            dr.Close();

            gLatLngs = new List<StoreGLatLng>();
            sp = new StoredProcedure("");
            sql = "select * from Store_GLatLng";
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandSql = sql;

            dr = sp.ExecuteReader();
            while (dr.Read())
            {
                StoreGLatLng gl = new StoreGLatLng();
                gl.Id = int.Parse(dr["ID"].ToString());
                gl.Retailerid = int.Parse(dr["Retailerid"].ToString());
                gl.GLat = dr["GLat"].ToString();
                gl.Glng = dr["Glng"].ToString();
                gl.Region = int.Parse(dr["Region"].ToString());
                gl.Location = dr["Location"].ToString();
                gl.Description = dr["Description"].ToString();
                gl.Phone = dr["Phone"].ToString();
                gl.Postcode = dr["Postcode"].ToString();
                gl.PostalCity = dr["PostalCity"].ToString();
                gl.email = dr["email"].ToString();
                gl.LocationName = dr["LocationName"].ToString();
                gl.DescriptionNew = dr["DescriptionNew"].ToString();
                gLatLngs.Add(gl);
            }
            dr.Close();

            gRegions = new List<StoreGRegion>();
            sp = new StoredProcedure("");
            sql = "select * from Store_GRegion";
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandSql = sql;

            dr = sp.ExecuteReader();
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
            dr.Close();
        }

        public static string GetUserLocation(int region)
        {
            StoreUserLocation ul = userLocations.SingleOrDefault(u => u.Id == region);
            if (ul != null)
                return ul.UserLocation;
            else

                return string.Empty;
        }

        public static StoreGRegion GetStoreGRegion(int regionID)
        {
            StoreGRegion gr = gRegions.SingleOrDefault(g => g.RegionID == regionID);

            return gr;
        }

        public static StoreGRegion GetStoreGRegionByCode(string regionCode)
        {
            StoreGRegion gr = gRegions.SingleOrDefault(g => g.RegionCode == regionCode);

            return gr;
        }
        #endregion

        public static void UpdataSolidShopLocation(int gID, string lat, string lng)
        {
            try
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("CSK_Store_GLatLng_UpdateLatLng");
                sp.Command.AddParameter("@gID", gID, DbType.Int32);
                sp.Command.AddParameter("@GLat", lat, DbType.String);
                sp.Command.AddParameter("@GLng", lng, DbType.String);

                sp.Execute();
            }
            catch (Exception ex)
            {

            }
        }

        //public static void UpdateRetailerInfoByRetailer(int retailerID, int storeType, string securePayment, string phoneSupport, string returnsAccepted)
        //{
        //    try
        //    {
        //        SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("CSK_Store_Retailer_UpdateRetailerByretailer");
        //        sp.Command.AddParameter("@RetailerID", retailerID, DbType.Int32);
        //        sp.Command.AddParameter("@StoreType", storeType, DbType.Int32);
        //        sp.Command.AddParameter("@SecurePayment", securePayment, DbType.String);
        //        sp.Command.AddParameter("@PhoneSupport", phoneSupport, DbType.String);
        //        sp.Command.AddParameter("@ReturnsAccepted", returnsAccepted, DbType.String);

        //        sp.Execute();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        public static DataTable GetLocationByRetailerID(int retailerID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(String.Format("SELECT ID, GLat,Glng,Location,Description from dbo.Store_GLatLng sg where sg.Retailerid={0}", retailerID), conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public static IDataReader GetRetailerInfoByID(int retailerID)
        {
            //IDataReader dr;

            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
            //{
            //    conn.Open();
            //    StoredProcedure sp = new StoredProcedure("");
            //    string sql = @"select StoreType,SecurePayment,PhoneSupport,ReturnsAccepted from dbo.CSK_Store_Retailer where RetailerId={0}";
            //     sql = string.Format(sql, retailerID);
            //   SqlCommand cmd = new SqlCommand(sql, conn);
            //    sp.Command.CommandType = CommandType.Text;
            //    sp.Command.CommandSql = sql;
            //    dr = sp.GetReader();
            //    conn.Close();
            //}
            //return dr;

            try
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
                string sql = @"select StoreType,SecurePayment,PhoneSupport,ReturnsAccepted from dbo.CSK_Store_RetailerST where RetailerId={0}";
                sql = string.Format(sql, retailerID);
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandSql = sql;

                if (!sp.ExecuteReader().Read())
                {
                    sql = @"select StoreType,SecurePayment,PhoneSupport,ReturnsAccepted from dbo.CSK_Store_Retailer where RetailerId={0}";
                    sql = string.Format(sql, retailerID);
                    sp.Command.CommandType = CommandType.Text;
                    sp.Command.CommandSql = sql;
                }

                return sp.ExecuteReader();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        

        public static List<RetailerPaymentCache> GetRetailerPaymentCache()
        {
            try
            {
                _RetailerPaymentList = new List<RetailerPaymentCache>();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
                {
                    string sql = @"select ID,RetailerId,PaymentOptionId,Name,ImageUrl,Country from [dbo].[CSK_Store_RetailerPaymentOption] r
                                inner join [dbo].[CSK_Store_PaymentOption] p
                                on r.PaymentOptionId = p.PaymentId";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandTimeout = 0;
                    IDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        RetailerPaymentCache cache = new RetailerPaymentCache();
                        cache.RetailerID = int.Parse(dr["RetailerID"].ToString());
                        cache.PaymentID = int.Parse(dr["PaymentOptionId"].ToString());
                        cache.PaymentImage = dr["ImageUrl"].ToString();
                        cache.PaymentName = dr["Name"].ToString();
                        cache.Country = dr["Country"].ToString();
                        _RetailerPaymentList.Add(cache);
                    }
                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
            }

            //SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
           
            //sp.Command.CommandType = CommandType.Text;
            //sp.Command.CommandTimeout = 0;
            //sp.Command.CommandSql = sql;
            //IDataReader dr = sp.ExecuteReader();
            //while (dr.Read())
            //{
            //    RetailerPaymentCache cache = new RetailerPaymentCache();
            //    cache.RetailerID = int.Parse(dr["RetailerID"].ToString());
            //    cache.PaymentID = int.Parse(dr["PaymentOptionId"].ToString());
            //    cache.PaymentImage = dr["ImageUrl"].ToString();
            //    cache.PaymentName = dr["Name"].ToString();
            //    cache.Country = dr["Country"].ToString();
            //    RetailerPaymentList.Add(cache);
            //}
            //dr.Close();

            return RetailerPaymentList;
        }

        public static List<RetailerPaymentCache> GetRetailerPaymentByRetailerID(int retailerID)
        {
            try
            {
                List<RetailerPaymentCache> retailerPaymentCacheList = RetailerPaymentList.Where(p => p.RetailerID == retailerID).ToList();
                return retailerPaymentCacheList;
            }
            catch (Exception ex)
            {
                return new List<RetailerPaymentCache>();
            }
        }

        public static List<CSK_Store_RetailerVotesSum> GetRetailerVotesSums(int country)
        {
            List<CSK_Store_RetailerVotesSum> votes = new List<CSK_Store_RetailerVotesSum>();
            try
            {
                StoredProcedure sp = PriceMeStatic.PriceMeDB.GetAllRetailerReview();
                sp.Command.AddParameter("@countryid", country, DbType.Int32);
                IDataReader dr = sp.ExecuteReader();

                while (dr.Read())
                {
                    CSK_Store_RetailerVotesSum vote = new CSK_Store_RetailerVotesSum();
                    vote.ID = int.Parse(dr["ID"].ToString());
                    vote.RetailerID = int.Parse(dr["RetailerID"].ToString());
                    vote.RetailerRatingSum = int.Parse(dr["RetailerRatingSum"].ToString());
                    vote.RetailerTotalRatingVotes = int.Parse(dr["RetailerTotalRatingVotes"].ToString());

                    votes.Add(vote);
                }
                dr.Close();
            }
            catch (Exception ex)
            {

            }

            return votes;
        }

        public static RetailerCache GetRetailerByEN(string en)
        {
            string ridString = PriceMeCommon.DesEncrypt.Decrypt(en);

            int rid = int.Parse(ridString);

            return RetailerController.GetRetailerByID(rid);
        }

        static Dictionary<int, string> GetRetailerInactiveReason()
        {
            Dictionary<int, string> retailerInactiveReason = new Dictionary<int, string>();
            try
            {
                string sqlString = @"select ReasonTabel.ID,ReasonTabel.Reason,RetailerTable.RetailerId from Inactive_Retailer_Reason as ReasonTabel
                                 left join CSK_Store_Retailer as RetailerTable on ReasonTabel.ID = RetailerTable.SetInactiveReason
                                 where RetailerTable.RetailerStatus = 99";

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
                using (System.Data.SqlClient.SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (System.Data.SqlClient.SqlCommand sqlCMD = new SqlCommand(sqlString, sqlConn))
                    {
                        sqlConn.Open();
                        using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
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
            }
            catch (Exception ex)
            {

            }
            return retailerInactiveReason;
        }

        public static string GetInactiveReason(int retailerID)
        {
            try
            {
                if (RetailerInactiveReason.ContainsKey(retailerID))
                {
                    return RetailerInactiveReason[retailerID];
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static List<RetailerReviewCache> GetPageRetailerReviews(int pageIndex, int pageSize, int retailerId, int soryBy, string type, out int totalCount)
        {
            try
            {
                List<RetailerReviewCache> rrcs = new List<RetailerReviewCache>();//RetailerReviews
                if (type == "web")
                    rrcs = RetailerReviews.Where(r => r.RetailerID == retailerId && r.SourceType == "web").ToList();
                else if(type == "sys")
                    rrcs = RetailerReviews.Where(r => r.RetailerID == retailerId && r.SourceType != "web").ToList();
                else
                    rrcs = RetailerReviews.Where(r => r.RetailerID == retailerId).ToList();

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
            catch (Exception ex)
            {
                totalCount = 0;
                return new List<RetailerReviewCache>();
            }
        }

        private static void BindGovernmentBadge()
        {
            try
            {
                GoverCache = new Dictionary<int, GovernmentBadgeCache>();

                if (ConfigAppString.CountryID == 3)
                {
                    List<GovernmentBadgeCache> goverCaches = new List<GovernmentBadgeCache>();
                    string sql = "Select * From CSK_Store_GovernmentBadge";
                    StoredProcedure sp = new StoredProcedure("");
                    sp.Command.CommandSql = sql;
                    sp.Command.CommandTimeout = 0;
                    sp.Command.CommandType = CommandType.Text;
                    IDataReader dr = sp.ExecuteReader();
                    while (dr.Read())
                    {
                        GovernmentBadgeCache goverCache = new GovernmentBadgeCache();
                        int id = 0;
                        int.TryParse(dr["ID"].ToString(), out id);
                        int RetailerID = 0;
                        int.TryParse(dr["RetailerID"].ToString(), out RetailerID);
                        string CompanyName = dr["CompanyName"].ToString();
                        int CompanyID = 0;
                        int.TryParse(dr["CompanyID"].ToString(), out CompanyID);
                        goverCache.ID = id;
                        goverCache.RetailerID = RetailerID;
                        goverCache.CompanyName = CompanyName;
                        goverCache.CompanyID = CompanyID;

                        if (!GoverCache.ContainsKey(goverCache.RetailerID))
                            GoverCache.Add(goverCache.RetailerID, goverCache);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static List<RetailerCache> GetRetailerListOrderByName()
        {
            List<CSK_Store_Retailer> _retailerList = null;
            try
            {
                bool loadAllCountryRetailer = false;
                if (ConfigurationManager.AppSettings["LoadAllCountryRetailer"] != null)
                    bool.TryParse(ConfigurationManager.AppSettings["LoadAllCountryRetailer"], out loadAllCountryRetailer);

                if (loadAllCountryRetailer)
                {
                    _retailerList = PriceMeDBStatic.PriceMeDB.CSK_Store_Retailers
                         .Where(rt => rt.IsSetupComplete == true && rt.RetailerStatus != 99)
                         .OrderBy(rt => rt.RetailerName).ToList();
                }
                else
                {
                    _retailerList = PriceMeDBStatic.PriceMeDB.CSK_Store_Retailers
                         .Where(rt => rt.IsSetupComplete == true && rt.RetailerStatus != 99 && rt.RetailerCountry == ConfigAppString.CountryID)
                         .OrderBy(rt => rt.RetailerName).ToList();
                }
                foreach (var item in _retailerList)
                {
                    var vote = RetailerVotes.FirstOrDefault(v => v.RetailerID == item.RetailerId);
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
                }
                _retailerListOrderByName = ConvertController<RetailerCache, CSK_Store_Retailer>.ConvertData(_retailerList);
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "RetailerListOrderByName no velocity");
            }
            catch (Exception ex)
            {

            }
            return _retailerListOrderByName;
        }

        private static void GetRetailerReviewsCache()
        {
            try
            {
                List<int> listReviewid = GetReviewstatus();

                List<CSK_Store_RetailerReview> csReviewList = CSK_Store_RetailerReview.All().OrderByDescending(rr => rr.CreatedOn).ToList();
                _retailerReviews = PriceMeCommon.Convert.ConvertController<RetailerReviewCache,
                    CSK_Store_RetailerReview>.ConvertData(csReviewList);
                _retailerReviews.ForEach(r => r.SourceType = "web");
                _retailerReviews.ForEach(r => r.RetailerID = r.RetailerId);
                _retailerReviews.ForEach(r => r.ReviewID = r.RetailerReviewId);
                _retailerReviews.ForEach(r => r.OverallRating = r.OverallStoreRating);

                List<RetailerReviewCache> _rRetailerReviewDetailList = null;
                List<RetailerReviewDetail> rReviewDetailList = PriceMeDBStatic.PriceMeDB.RetailerReviewDetails.ToList();
                _rRetailerReviewDetailList = PriceMeCommon.Convert.ConvertController<RetailerReviewCache,
                    RetailerReviewDetail>.ConvertData(rReviewDetailList);
                _rRetailerReviewDetailList.ForEach(r => r.SourceType = "review-system");
                _retailerReviews.AddRange(_rRetailerReviewDetailList);

                _retailerReviews = _retailerReviews.Where(r => listReviewid.Contains(r.ReviewID)).ToList();
                _retailerReviews = _retailerReviews.OrderByDescending(r => r.CreatedOn).ToList();

                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "RetailerReviewList no velocity");
            }
            catch (Exception ex)
            {

            }
        }

        private static List<int> GetReviewstatus()
        {
            List<int> listReviewid = new List<int>();
            string connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            var sql = string.Format("select ReviewID from dbo.Merchant_Reviews Where ReviewStatus in (4, 5)");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int reviewid = 0;
                    int.TryParse(dr["ReviewID"].ToString(), out reviewid);
                    listReviewid.Add(reviewid);
                }
                dr.Close();

                conn.Close();
            }

            return listReviewid;
        }

        private static CountryInfo BindInternationalCountry(List<CSK_Util_Country> countrys, int countryId)
        {
            CountryInfo info = new CountryInfo();
            try
            {
                foreach (CSK_Util_Country country in countrys)
                {
                    if (countryId == country.countryID)
                    {
                        info.CountryID = country.countryID;
                        info.KeyName = country.country;
                        info.CountryFlag = country.CountryImage;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return info;
        }

        public static List<RetailerCache> GetRetailerListByIDs(List<int> retailerIDs)
        {
            List<RetailerCache> _retailerList = new List<RetailerCache>();
            try
            {
                foreach (int rid in retailerIDs)
                {
                    RetailerCache retailer = RetailerController.GetRetailerByID(rid);
                    if (retailer == null || retailer.RetailerStatus == 99) continue;
                    _retailerList.Add(retailer);
                }
            }
            catch (Exception ex)
            {

            }
            return _retailerList;
        }

        public static CSK_Util_Country GetUtilCountry(int countryID)
        {
            if (UtilCountryDic.ContainsKey(countryID))
            {
                return UtilCountryDic[countryID];
            }
            return null;
        }
    }
}