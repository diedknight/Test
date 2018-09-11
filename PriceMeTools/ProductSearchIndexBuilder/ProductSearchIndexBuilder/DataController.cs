using ProductSearchIndexBuilder.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProductSearchIndexBuilder
{
    class DataController
    {
        static Dictionary<int, CategoryCache> CategoryDic_Static;
        static Dictionary<int, List<CategoryCache>> NextLevelActiveCategories_Static;
        static Dictionary<int, AttributeTitleCache> AttributeTitleDic_Static;
        static List<AttributeValueRangeCache> AttributeValueRangeList_Static;
        static List<AttributeValueCache> AttributeValueList_Static;
        static Dictionary<string, AttributeValueCache> AttributeTitleIDAndListOrderDic_Static;
        static Dictionary<int, CategoryHWDInfo> CategoryHWDInfoDic_Static;
        static List<CategoryAttributeTitleMapCache> CategoryAttributeTitleMapList_Static;
        static Dictionary<int, List<AttributeTitleCache>> CategoryAttributeTitleDic_Static;
        static Dictionary<int, List<CategoryExtend>> CategoryExtendListDic_Static;
        static List<int> IsSearchOnlyCategoryIdList_Static;
        static List<int> NotOverseasRetailer_Static;

        static List<CategoryCache> CategoryListOrderByName_Static;
        static Dictionary<int, int> CategoryClicksDic_Static;
        static Dictionary<int, int> CategoryProductsCountDic_Static;

        static DbInfo Priceme205DbInfo_Static;
        static DbInfo PamUserDbInfo_Static;
        static DbInfo SubDbInfo_Static;

        public static List<CategoryCache> CategoryListOrderByName
        {
            get
            {
                return CategoryListOrderByName_Static;
            }
        }

        public static Dictionary<int, int> CategoryClicksDic
        {
            get
            {
                return CategoryClicksDic_Static;
            }
        }

        public static Dictionary<int, int> CategoryProductsCountDic
        {
            get
            {
                return CategoryProductsCountDic_Static;
            }
        }

        public static void Init(DbInfo priceme205DbInfo, DbInfo pamUserDbInfo, DbInfo subDbInfo)
        {
            Priceme205DbInfo_Static = priceme205DbInfo;
            PamUserDbInfo_Static = pamUserDbInfo;
            SubDbInfo_Static = subDbInfo;

            CategoryListOrderByName_Static = GetCatOrderByNameOrderByName(priceme205DbInfo, AppValue.CountryId);
            CategoryClicksDic_Static = GetCategoryClicksDic(priceme205DbInfo, AppValue.CountryId);
            CategoryProductsCountDic_Static = GetCategoryProductsCountDicDic(priceme205DbInfo, AppValue.CountryId);
            CategoryDic_Static = CategoryListOrderByName_Static.ToDictionary(c => c.CategoryID, c => c);
            IsSearchOnlyCategoryIdList_Static = CategoryListOrderByName_Static.FindAll(c => c.IsSearchOnly).Select(c => c.CategoryID).ToList();
            NextLevelActiveCategories_Static = new Dictionary<int, List<CategoryCache>>();
            foreach (var c in CategoryListOrderByName_Static)
            {
                List<CategoryCache> categories = CategoryListOrderByName_Static.Where(cat => cat.ParentID == c.CategoryID).ToList();
                NextLevelActiveCategories_Static.Add(c.CategoryID, categories);
            }

            CategoryExtendListDic_Static = GetCategoryExtendDic(pamUserDbInfo, AppValue.CountryId);

            AttributeTitleDic_Static = GetAttributeTitleDicFromDB(priceme205DbInfo);
            AttributeValueRangeList_Static = GetAttributeValueRangeCacheListFromDB(priceme205DbInfo);
            AttributeValueList_Static = GetAttributeValuesCacheListFromDB(priceme205DbInfo);
            AttributeTitleIDAndListOrderDic_Static = GetAttributeTitleIDAndListOrderDic(AttributeValueList_Static, AttributeValueRangeList_Static);

            CategoryHWDInfoDic_Static = GetCategoryHWDInfoDicFromDB(pamUserDbInfo);
            CategoryAttributeTitleMapList_Static = GetCategoryAttributeTilteMapsFromDB(CategoryHWDInfoDic_Static, priceme205DbInfo);
            CategoryAttributeTitleDic_Static = GetCategoryAttributeTitleDic(CategoryAttributeTitleMapList_Static);

            NotOverseasRetailer_Static = LoadNotOverseasRetailer(priceme205DbInfo, AppValue.CountryId);
        }

        private static Dictionary<int, int> GetCategoryProductsCountDicDic(DbInfo priceme205DbInfo, int countryId)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            string sqlString = @"select CategoryID, count(productId) as ps from CSK_Store_Product
                                  where ProductID in 
                                  (select ProductID from CSK_Store_RetailerProduct where RetailerProductStatus = 1 and RetailerId in (
                                  select RetailerId from CSK_Store_Retailer where RetailerCountry = " + countryId + " and RetailerStatus <> 99))group by CategoryID";

            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sqlString, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            dic.Add(sqlDR.GetInt32(0), sqlDR.GetInt32(1));
                        }
                    }
                }
            }

            return dic;
        }

        private static Dictionary<int, int> GetCategoryClicksDic(DbInfo priceme205DbInfo, int countryId)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            string sqlString = @"SELECT CSK_Store_Product.CategoryID, count(Clicks) as clicks
                                  FROM ProductClickTemp
                                  inner join CSK_Store_Product on ProductClickTemp.ProductId = CSK_Store_Product.ProductID
                                  where ProductClickTemp.CountryID = " + countryId + " group by CSK_Store_Product.CategoryID";

            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sqlString, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            dic.Add(sqlDR.GetInt32(0), sqlDR.GetInt32(1));
                        }
                    }
                }
            }

            return dic;
        }

        private static List<int> LoadNotOverseasRetailer(DbInfo priceme205DbInfo, int countryId)
        {
            List<int> notOverseasRetailer = new List<int>();

            string sqlString = "select retailerid from csk_store_ppcmember where retailerCountry = " + AppValue.CountryId;
            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sqlString, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int rid = int.Parse(sqlDR["retailerid"].ToString());
                            notOverseasRetailer.Add(rid);
                        }
                    }
                }
            }

            return notOverseasRetailer;
        }

        public static bool IsOverseasRetailer(int retailerId)
        {
            return !NotOverseasRetailer_Static.Contains(retailerId);
        }

        public static CategoryHWDInfo GetCategoryHWDInfo(int categoryId)
        {
            if(CategoryHWDInfoDic_Static.ContainsKey(categoryId))
            {
                return CategoryHWDInfoDic_Static[categoryId];
            }
            return null;
        }

        public static List<int> GetAllSubCategoryId(int cId)
        {
            List<int> categoryIDs = new List<int>();

            List<CategoryCache> categoryCaches = GetNextLevelSubCategories(cId);
            foreach (var cache in categoryCaches)
            {
                SetAllSubCategoryIds(categoryIDs, cache.CategoryID);
            }

            return categoryIDs;
        }

        private static void SetAllSubCategoryIds(List<int> categoryIDs, int cid)
        {
            categoryIDs.Add(cid);

            List<CategoryCache> categoryCaches = GetNextLevelSubCategories(cid);
            foreach (var cache in categoryCaches)
            {
                SetAllSubCategoryIds(categoryIDs, cache.CategoryID);
            }
        }

        /// <summary>
        /// 获取下一级分类
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<CategoryCache> GetNextLevelSubCategories(int cid)
        {
            if (NextLevelActiveCategories_Static.ContainsKey(cid))
                return NextLevelActiveCategories_Static[cid];

            return new List<CategoryCache>();
        }

        private static List<CategoryCache> GetCatOrderByNameOrderByName(DbInfo priceme205DbInfo, int countryId)
        {
            List<CategoryCache> categoryOrderByName = new List<CategoryCache>();
            string selectSql = "select CT.*,LCT.CategoryName as LocalName,LCT.Description as LocalDesc,LCT.ShortDescription as LocalShortDesc from CSK_Store_Category as CT left join Local_CategoryName as LCT on CT.CategoryID = LCT.CategoryID and LCT.CountryID = " + countryId + " where CT.IsActive = 1 order by CT.CategoryName";

            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            CategoryCache categoryCache = new CategoryCache();
                            categoryCache.AdminComments = sqlDR["AdminComments"].ToString();
                            categoryCache.AttributeID = sqlDR["AttributeID"].ToString();
                            categoryCache.AttributesCategory = bool.Parse(sqlDR["AttributesCategory"].ToString());
                            categoryCache.CategoryID = int.Parse(sqlDR["CategoryID"].ToString());
                            categoryCache.CategoryName = sqlDR["CategoryName"].ToString();
                            categoryCache.CategoryNameEN = categoryCache.CategoryName;
                            string localName = sqlDR["LocalName"].ToString();
                            if (!string.IsNullOrEmpty(localName))
                            {
                                categoryCache.CategoryName = localName;
                            }
                            categoryCache.LocalDescription = sqlDR["LocalDesc"].ToString();
                            categoryCache.ImageFile = sqlDR["ImageFile"].ToString();
                            categoryCache.IsAccessories = bool.Parse(sqlDR["IsAccessories"].ToString());
                            categoryCache.IsActive = bool.Parse(sqlDR["IsActive"].ToString());
                            categoryCache.IsDisplayIsMerged = bool.Parse(sqlDR["IsDisplayIsMerged"].ToString());
                            categoryCache.IsFilterByBrand = bool.Parse(sqlDR["IsFilterByBrand"].ToString());
                            categoryCache.IsSiteMap = bool.Parse(sqlDR["IsSiteMap"].ToString());
                            categoryCache.IsSiteMapDetail = bool.Parse(sqlDR["IsSiteMapDetail"].ToString());
                            categoryCache.IsSiteMapDetailPopular = bool.Parse(sqlDR["IsSiteMapDetailPopular"].ToString());
                            categoryCache.IsSiteMapPopular = bool.Parse(sqlDR["IsSiteMapPopular"].ToString());
                            categoryCache.LongDescription = sqlDR["LongDescription"].ToString();

                            int maxPrice;
                            int.TryParse(sqlDR["MaxPrice"].ToString(), out maxPrice);
                            categoryCache.MaxPrice = maxPrice;

                            int minPrice;
                            int.TryParse(sqlDR["MinPrice"].ToString(), out minPrice);
                            categoryCache.MinPrice = minPrice;

                            categoryCache.ParentID = int.Parse(sqlDR["ParentID"].ToString());
                            categoryCache.PopularCategory = bool.Parse(sqlDR["PopularCategory"].ToString());
                            categoryCache.ShortDescription = sqlDR["LocalShortDesc"].ToString();
                            categoryCache.TopBrands = sqlDR["TopBrands"].ToString();
                            categoryCache.Categoryicon = sqlDR["categoryicon"].ToString();
                            categoryCache.CategoryIconCode = sqlDR["iconcode"].ToString();
                            categoryCache.IconUrl = sqlDR["categoryicon"].ToString();
                            categoryCache.IsSearchOnly = bool.Parse(sqlDR["IsSearchOnly"].ToString());
                            categoryCache.CategoryViewType = int.Parse(sqlDR["CategoryViewType"].ToString());
                            categoryCache.WeightUnit = bool.Parse(sqlDR["WeightUnit"].ToString());
                            categoryCache.ListOrder = int.Parse(sqlDR["ListOrder"].ToString());

                            categoryOrderByName.Add(categoryCache);
                        }
                    }
                }
            }

            return categoryOrderByName;
        }

        internal static List<CategoryCache> GetAllRootCategoriesOrderByName()
        {
            return CategoryListOrderByName_Static.FindAll(c => c.ParentID == 0).ToList();
        }

        private static Dictionary<int, AttributeTitleCache> GetAttributeTitleDicFromDB(DbInfo priceme205DbInfo)
        {
            Dictionary<int, AttributeTitleCache> dic = new Dictionary<int, AttributeTitleCache>();
            string sql = "SELECT * FROM CSK_Store_ProductDescriptorTitle";
            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                sqlConn.Open();

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var rs = DbConvertController<AttributeTitleCache>.ReadDataFromDataReader(sqlDR);
                            if (rs != null)
                            {
                                dic.Add(rs.TypeID, rs);
                            }
                        }
                    }
                }
            }

            dic.Add(SpecialAttributesTitle.HeightAttribute.TypeID, SpecialAttributesTitle.HeightAttribute);
            dic.Add(SpecialAttributesTitle.WidthAttribute.TypeID, SpecialAttributesTitle.WidthAttribute);
            dic.Add(SpecialAttributesTitle.LengthAttribute.TypeID, SpecialAttributesTitle.LengthAttribute);
            dic.Add(SpecialAttributesTitle.WeightAttribute.TypeID, SpecialAttributesTitle.WeightAttribute);
            return dic;
        }

        public static AttributeTitleCache GetAttributeTitleById(int attrTitleId)
        {
            if(AttributeTitleDic_Static.ContainsKey(attrTitleId))
            {
                return AttributeTitleDic_Static[attrTitleId];
            }
            return null;
        }

        public static CategoryCache GetCategoryById(int categoryId)
        {
            if (CategoryDic_Static.ContainsKey(categoryId))
            {
                return CategoryDic_Static[categoryId];
            }
            return null;
        }

        private static List<AttributeValueRangeCache> GetAttributeValueRangeCacheListFromDB(DbInfo priceme205DbInfo)
        {
            List<AttributeValueRangeCache> list = new List<AttributeValueRangeCache>();
            string sql = "SELECT * FROM CSK_Store_AttributeValueRange";
            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                sqlConn.Open();

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var rs = DbConvertController<AttributeValueRangeCache>.ReadDataFromDataReader(sqlDR);
                            if (rs != null)
                            {
                                list.Add(rs);
                            }
                        }
                    }
                }
            }

            return list;
        }

        public static List<AttributeValueRangeCache> GetAttributeValueRangesByTitleIdAndCategoryId(int attributeTitleId, int categoryId)
        {
            List<AttributeValueRangeCache> attributeValueRangeCollection = AttributeValueRangeList_Static.FindAll(avr => avr.AttributeTitleID == attributeTitleId && avr.CategoryID == categoryId).ToList();

            return attributeValueRangeCollection;
        }

        private static List<AttributeValueCache> GetAttributeValuesCacheListFromDB(DbInfo priceme205DbInfo)
        {
            List<AttributeValueCache> attributeValues = new List<AttributeValueCache>();
            string sql = "SELECT * FROM CSK_Store_AttributeValue";
            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                sqlConn.Open();

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var attributeValueCache = DbConvertController<AttributeValueCache>.ReadDataFromDataReader(sqlDR);
                            if (attributeValueCache != null)
                            {
                                attributeValues.Add(attributeValueCache);
                            }
                        }
                    }
                }
            }

            return attributeValues;
        }

        private static Dictionary<string, AttributeValueCache> GetAttributeTitleIDAndListOrderDic(List<AttributeValueCache> attributeValues, List<AttributeValueRangeCache> attributeValueRangeList)
        {
            Dictionary<string, AttributeValueCache> attributeTitleIDAndListOrderDictionary = new Dictionary<string, AttributeValueCache>();
            foreach (AttributeValueCache attributeValue in attributeValues)
            {
                if (attributeValue.ListOrder > 0)
                {
                    foreach (AttributeValueRangeCache attributeValueRange in attributeValueRangeList)
                    {
                        if (attributeValueRange.AttributeTitleID == attributeValue.AttributeTitleID)
                        {
                            string key = attributeValue.AttributeTitleID + "," + attributeValue.ListOrder;
                            if (!attributeTitleIDAndListOrderDictionary.ContainsKey(key))
                            {
                                attributeTitleIDAndListOrderDictionary.Add(key, attributeValue);
                            }
                            break;
                        }
                    }
                }
            }
            return attributeTitleIDAndListOrderDictionary;
        }

        public static string GetAttributeValueString(AttributeValueRangeCache avr, string unit)
        {
            string avString = "";
            int minValue = avr.MinValue;
            int maxValue = avr.MaxValue;

            if (minValue == 0 && maxValue == 0)
            {
                return avString;
            }

            string minString = "";
            string maxString = "";

            if (avr.MinValue == -1)
            {
                minString = "Below ";
                string key = avr.AttributeTitleID + "," + (avr.MaxValue + 1);
                if (AttributeTitleIDAndListOrderDic_Static.ContainsKey(key))
                {
                    maxString = AttributeTitleIDAndListOrderDic_Static[key].Value + " " + unit;
                }
            }
            else if (avr.MaxValue == -1)
            {
                minString = "Above ";
                string key = avr.AttributeTitleID + "," + (avr.MinValue - 1);
                if (AttributeTitleIDAndListOrderDic_Static.ContainsKey(key))
                {
                    maxString = AttributeTitleIDAndListOrderDic_Static[key].Value + " " + unit;
                }
            }
            else if (avr.MaxValue == 0)
            {
                minString = "";
                string key = avr.AttributeTitleID + "," + avr.MinValue;
                if (AttributeTitleIDAndListOrderDic_Static.ContainsKey(key))
                {
                    maxString = AttributeTitleIDAndListOrderDic_Static[key].Value + " " + unit;
                }
            }
            else
            {
                List<AttributeValueCache> attributeValues = GetAttributeValuesByTitleId(avr.AttributeTitleID);
                if (attributeValues != null)
                {
                    foreach (AttributeValueCache av in attributeValues)
                    {
                        if (av.ListOrder == minValue)
                        {
                            minString = av.Value + " " + unit + "-";
                        }
                        else if (av.ListOrder == maxValue)
                        {
                            maxString = av.Value + " " + unit;
                        }
                    }
                }

            }
            return minString + maxString;
        }

        private static List<AttributeValueCache> GetAttributeValuesByTitleId(int attributeTitleId)
        {
            List<AttributeValueCache> list = AttributeValueList_Static.FindAll(attr => attr.AttributeTitleID == attributeTitleId).ToList();
            return list;
        }

        private static Dictionary<int, CategoryHWDInfo> GetCategoryHWDInfoDicFromDB(DbInfo pamUserDbInfo)
        {
            Dictionary<int, CategoryHWDInfo> dic = new Dictionary<int, CategoryHWDInfo>();

            string sql = @"select HWDSliderAttribute.CategoryID,Heightslider,Widthslider,Depthslider,Weightslider, CSK_Store_Category.WeightUnit from HWDSliderAttribute
                            left join CSK_Store_Category on CSK_Store_Category.CategoryID = HWDSliderAttribute.CategoryID";

            using (var sqlConn = DBController.CreateDBConnection(pamUserDbInfo))
            {
                sqlConn.Open();
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int categoryId = sqlDR.GetInt32(0);
                            bool isHeightSlider = sqlDR.GetBoolean(1);
                            bool isWidthslider = sqlDR.GetBoolean(2);
                            bool isDepthslider = sqlDR.GetBoolean(3);
                            bool isWeightslider = sqlDR.GetBoolean(4);
                            bool weightUnitIsKG = sqlDR.GetBoolean(5);

                            CategoryHWDInfo hwd = new CategoryHWDInfo();
                            hwd.CategoryId = categoryId;
                            hwd.Heightslider = isHeightSlider;
                            hwd.Widthslider = isWidthslider;
                            hwd.Depthslider = isDepthslider;
                            hwd.Weightslider = isWeightslider;
                            hwd.WeightUnitIsKG = weightUnitIsKG;

                            dic.Add(categoryId, hwd);
                        }
                    }
                }
            }

            return dic;
        }

        private static List<CategoryAttributeTitleMapCache> GetCategoryAttributeTilteMapsFromDB(Dictionary<int, CategoryHWDInfo> categoryHWDInfoDic, DbInfo priceme205DbInfo)
        {
            List<CategoryAttributeTitleMapCache> categoryAttributeTilteMaps = new List<CategoryAttributeTitleMapCache>();
            //可以考虑放到Pam_User
            string sql = @"SELECT MapID
                              ,CategoryID
                              ,AttributeTitleID
                              ,IsPrimary
                              ,AttributeOrder
                              ,IsSlider
                              ,Step
                              ,Step2
                              ,MinValue
                              ,MaxValue
                                ,Step3 FROM CSK_Store_Category_AttributeTitle_Map";

            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                if (sqlConn is MySql.Data.MySqlClient.MySqlConnection)
                {
                    sql = @"SELECT MapID
                              ,CategoryID
                              ,AttributeTitleID
                              ,IsPrimary
                              ,AttributeOrder
                              ,IsSlider
                              ,Step
                              ,Step2
                              ,MinValue
                              ,`MaxValue`
                                ,Step3 FROM CSK_Store_Category_AttributeTitle_Map";
                }
                sqlConn.Open();
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            CategoryAttributeTitleMapCache cache = new CategoryAttributeTitleMapCache();
                            cache.MapID = sqlDR.GetInt32(0);
                            cache.CategoryID = sqlDR.GetInt32(1);
                            cache.AttributeTitleID = sqlDR.GetInt32(2);
                            cache.IsPrimary = sqlDR.GetBoolean(3);
                            cache.AttributeOrder = sqlDR.GetInt32(4);
                            cache.IsSlider = sqlDR.GetBoolean(5);

                            string floatString = sqlDR["Step"].ToString();
                            float floatValue = 0f;
                            float.TryParse(floatString, out floatValue);
                            cache.Step = floatValue;

                            floatString = sqlDR["Step2"].ToString();
                            floatValue = 0f;
                            float.TryParse(floatString, out floatValue);
                            cache.Step2 = floatValue;

                            string step3String = sqlDR["Step3"].ToString();
                            int step3;
                            int.TryParse(step3String, out step3);
                            cache.Step3 = step3;

                            floatString = sqlDR["MinValue"].ToString();
                            floatValue = 0f;
                            float.TryParse(floatString, out floatValue);
                            cache.MinValue = floatValue;

                            floatString = sqlDR["MaxValue"].ToString();
                            floatValue = 0f;
                            float.TryParse(floatString, out floatValue);
                            cache.MaxValue = floatValue;
                            categoryAttributeTilteMaps.Add(cache);
                        }
                    }
                }
            }

            foreach (var cid in categoryHWDInfoDic.Keys)
            {
                var hwd = categoryHWDInfoDic[cid];
                if (hwd.Heightslider)
                {
                    CategoryAttributeTitleMapCache cache = new CategoryAttributeTitleMapCache();
                    cache.MapID = -99;
                    cache.CategoryID = cid;
                    cache.AttributeTitleID = SpecialAttributesTitle.HeightAttribute.TypeID;
                    cache.IsPrimary = true;
                    cache.IsSlider = true;
                    cache.AttributeOrder = 9990;
                    categoryAttributeTilteMaps.Add(cache);
                }

                if (hwd.Widthslider)
                {
                    CategoryAttributeTitleMapCache cache = new CategoryAttributeTitleMapCache();
                    cache.MapID = -99;
                    cache.CategoryID = cid;
                    cache.AttributeTitleID = SpecialAttributesTitle.WidthAttribute.TypeID;
                    cache.IsPrimary = true;
                    cache.IsSlider = true;
                    cache.AttributeOrder = 9991;
                    categoryAttributeTilteMaps.Add(cache);
                }

                if (hwd.Depthslider)
                {
                    CategoryAttributeTitleMapCache cache = new CategoryAttributeTitleMapCache();
                    cache.MapID = -99;
                    cache.CategoryID = cid;
                    cache.AttributeTitleID = SpecialAttributesTitle.LengthAttribute.TypeID;
                    cache.IsPrimary = true;
                    cache.IsSlider = true;
                    cache.AttributeOrder = 9992;
                    categoryAttributeTilteMaps.Add(cache);
                }

                if (hwd.Weightslider)
                {
                    CategoryAttributeTitleMapCache cache = new CategoryAttributeTitleMapCache();
                    cache.MapID = -99;
                    cache.CategoryID = cid;
                    cache.AttributeTitleID = SpecialAttributesTitle.WeightAttribute.TypeID;
                    cache.IsPrimary = true;
                    cache.IsSlider = true;
                    cache.AttributeOrder = 9993;
                    categoryAttributeTilteMaps.Add(cache);
                }
            }

            return categoryAttributeTilteMaps;
        }

        public static List<AttributeTitleCache> GetAttributesTitleByCategoryID(int cid)
        {
            if (CategoryAttributeTitleDic_Static.ContainsKey(cid))
            {
                return CategoryAttributeTitleDic_Static[cid];
            }
            return new List<AttributeTitleCache>();
        }

        private static Dictionary<int, List<AttributeTitleCache>> GetCategoryAttributeTitleDic(List<CategoryAttributeTitleMapCache> categoryAttributeTilteMaps)
        {
            Dictionary<int, List<AttributeTitleCache>> dic = new Dictionary<int, List<AttributeTitleCache>>();

            foreach (CategoryAttributeTitleMapCache catm in categoryAttributeTilteMaps)
            {
                if (catm.IsPrimary)
                {
                    AttributeTitleCache atc = GetAttributeTitleById(catm.AttributeTitleID);
                    if (atc != null)
                    {
                        if (dic.ContainsKey(catm.CategoryID))
                        {
                            dic[catm.CategoryID].Add(atc);
                        }
                        else
                        {
                            List<AttributeTitleCache> list = new List<AttributeTitleCache>();
                            list.Add(atc);
                            dic.Add(catm.CategoryID, list);
                        }
                    }
                }
            }

            return dic;
        }

        public static CategoryAttributeTitleMapCache GetCategoryAttributeTitleMapByCidAttrTId(int cid, int attrTitleId)
        {
            return CategoryAttributeTitleMapList_Static.FirstOrDefault(catm => catm.CategoryID == cid && catm.AttributeTitleID == attrTitleId);
        }

        public static List<CategoryExtend> GetCategorySynonym(int categoryId)
        {
            if(CategoryExtendListDic_Static.ContainsKey(categoryId))
            {
                return CategoryExtendListDic_Static[categoryId];
            }
            return null;
        }

        private static Dictionary<int, List<CategoryExtend>> GetCategoryExtendDic(DbInfo pamUserDbInfo, int countryId)
        {
            Dictionary<int, List<CategoryExtend>> listDic = new Dictionary<int, List<CategoryExtend>>();
            string selectCategorySynonymSql = "SELECT CategoryID,Synonym,LocalName, CountryID FROM CategorySynonym_New where CountryID = " + countryId;
            using (var sqlConn = DBController.CreateDBConnection(pamUserDbInfo))
            {
                sqlConn.Open();
                using (var sqlCMD = DBController.CreateDbCommand(selectCategorySynonymSql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int cid = sqlDR.GetInt32(0);
                            string synonym = sqlDR.GetString(1);
                            string localName = sqlDR.GetString(2);

                            CategoryExtend categorySynonym = new CategoryExtend();
                            categorySynonym.CategoryID = cid;
                            categorySynonym.Synonym = synonym;
                            categorySynonym.LocalName = localName;
                            categorySynonym.CountryID = countryId;

                            if (listDic.ContainsKey(cid))
                            {
                                listDic[cid].Add(categorySynonym);
                            }
                            else
                            {
                                List<CategoryExtend> list = new List<CategoryExtend>();
                                list.Add(categorySynonym);
                                listDic.Add(cid, list);
                            }
                            
                        }
                    }
                }
            }

            return listDic;
        }

        public static bool IsSearchOnly(int cid)
        {
            return IsSearchOnlyCategoryIdList_Static.Contains(cid);
        }

        public static string GetCategoryNameByCategoryID(int cid)
        {
            var category = GetCategoryById(cid);
            if(category != null)
            {
                return category.CategoryName;
            }
            return "";
        }

        public static List<int> GetAllCategoryAttribute()
        {
            List<int> cidList = new List<int>();

            string sql = "select distinct(CID) from product_descandattr";

            using (var sqlConn = DBController.CreateDBConnection(PamUserDbInfo_Static))
            {
                sqlConn.Open();
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int cid = sqlDR.GetInt32(0);
                            cidList.Add(cid);
                        }
                    }
                }
            }

            return cidList;
        }

        public static void SetAllAttributeByCategoryId(int cId, Dictionary<int, AttributeGroup> groupDic, Dictionary<int, AttributeGroup> attributeGroupDic)
        {

            string sql = "select m.AttributeTitleID, t.Title, t.AttributeGroupID, t.ShortDescription, t.AttributeTypeID from CSK_Store_Category_AttributeTitle_Map m "
                        + "inner join CSK_Store_ProductDescriptorTitle t on m.AttributeTitleID = t.TypeID where m.CategoryID = " + cId
                        + " order by m.AttributeOrder";

            using (var sqlConn = DBController.CreateDBConnection(SubDbInfo_Static))
            {
                sqlConn.Open();
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            string groupid = sqlDR["AttributeGroupID"].ToString();
                            int gid = 0;
                            int.TryParse(groupid, out gid);
                            int titleId = int.Parse(sqlDR["AttributeTitleID"].ToString());
                            string title = sqlDR["Title"].ToString();
                            string des = sqlDR["ShortDescription"].ToString();
                            string typeId = sqlDR["AttributeTypeID"].ToString();
                            int tid = 0;
                            int.TryParse(typeId, out tid);

                            if (groupDic.ContainsKey(gid))
                            {
                                List<AttributeGroupList> glistList = groupDic[gid].AttributeGroupList;
                                AttributeGroupList glist = new AttributeGroupList();
                                glist.AttributeId = titleId;
                                glist.AttributeName = title;
                                glist.ShortDescription = des;
                                glist.T = 1;
                                glist.AttributeTypeID = tid;
                                glist.IsCategoryAttribute = true;
                                glistList.Add(glist);
                                groupDic[gid].AttributeGroupList = glistList;
                            }
                            else
                            {
                                AttributeGroupList glist = new AttributeGroupList();
                                glist.AttributeId = titleId;
                                glist.AttributeName = title;
                                glist.ShortDescription = des;
                                glist.T = 1;
                                glist.IsCategoryAttribute = true;
                                glist.AttributeTypeID = tid;
                                List<AttributeGroupList> glistList = new List<AttributeGroupList>();
                                glistList.Add(glist);

                                AttributeGroup group = new AttributeGroup();
                                if(attributeGroupDic.ContainsKey(gid))
                                {
                                    group.AttributeGroupName = attributeGroupDic[gid].AttributeGroupName;
                                    group.OrderID = attributeGroupDic[gid].OrderID;
                                    group.AttributeGroupList = glistList;
                                    groupDic.Add(gid, group);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  组名翻译过
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static Dictionary<int, AttributeGroup> GetAllAttributeGroupDic(int countryId)
        {
            Dictionary<int, AttributeGroup> dic = new Dictionary<int, AttributeGroup>();
            string sql = @"select ag.AttributeGroupID, ag.AttributeGroupName, OrderID, agt.AttributeGroupName from CSK_Store_AttributeGroup ag
                           left join CSK_Store_AttributeGroupTranslation agt on ag.AttributeGroupID = agt.AttributeGroupId and agt.CountryId = " + countryId;
            using (var sqlConn = DBController.CreateDBConnection(Priceme205DbInfo_Static))
            {
                sqlConn.Open();
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int agId = sqlDR.GetInt32(0);
                            string agName = sqlDR.GetString(1);
                            int agOrderId = sqlDR.GetInt32(2);
                            if (!sqlDR.IsDBNull(3))
                            {
                                agName = sqlDR.GetString(3);
                            }
                            AttributeGroup attributeGroup = new AttributeGroup();
                            attributeGroup.AttributeGroupName = agName;
                            attributeGroup.OrderID = agOrderId;
                            attributeGroup.AttributeGroupId = agId;
                            dic.Add(agId, attributeGroup);
                        }
                    }
                }
            }

            return dic;
        }

        public static void SetAllCompareAttributeByCategoryId(int cId, Dictionary<int, AttributeGroup> groupDic, Dictionary<int, AttributeGroup> attributeGroupDic)
        {
            string sql = "select CompareAttributeID, Name, AttributeGroupID, ShortDescription, AttributeTypeID from Store_Compare_Attributes where CategoryID = " + cId;
            using (var sqlConn = DBController.CreateDBConnection(SubDbInfo_Static))
            {
                sqlConn.Open();
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            string groupid = sqlDR["AttributeGroupID"].ToString();
                            int gid = 0;
                            int.TryParse(groupid, out gid);
                            int titleId = int.Parse(sqlDR["CompareAttributeID"].ToString());
                            string title = sqlDR["Name"].ToString();
                            string des = sqlDR["ShortDescription"].ToString();
                            string typeid = sqlDR["AttributeTypeID"].ToString();
                            int tid = 0;
                            int.TryParse(typeid, out tid);

                            if (groupDic.ContainsKey(gid))
                            {
                                List<AttributeGroupList> glistList = groupDic[gid].AttributeGroupList;
                                AttributeGroupList glist = new AttributeGroupList();
                                glist.AttributeId = titleId;
                                glist.AttributeName = title;
                                glist.ShortDescription = des;
                                glist.AttributeTypeID = tid;
                                glist.IsCategoryAttribute = false;
                                glistList.Add(glist);
                                groupDic[gid].AttributeGroupList = glistList;
                            }
                            else
                            {
                                AttributeGroupList glist = new AttributeGroupList();
                                glist.AttributeId = titleId;
                                glist.AttributeName = title;
                                glist.ShortDescription = des;
                                glist.AttributeTypeID = tid;
                                glist.IsCategoryAttribute = false;
                                List<AttributeGroupList> glistList = new List<AttributeGroupList>();
                                glistList.Add(glist);

                                AttributeGroup group = new AttributeGroup();
                                if (attributeGroupDic.ContainsKey(gid))
                                {
                                    group.AttributeGroupName = attributeGroupDic[gid].AttributeGroupName;
                                    group.OrderID = attributeGroupDic[gid].OrderID;
                                    group.AttributeGroupList = glistList;
                                    groupDic.Add(gid, group);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static List<RetailerCache> GetRetailersWithVotesSumOrderByClicksFromDB()
        {
            List<RetailerCache> list = new List<RetailerCache>();

            try
            {
                string sql = "select * from CSK_Store_Retailer where IsSetupComplete = 1 and RetailerCountry = " + AppValue.CountryId + " and RetailerStatus <> 99 order by RetailerName";

                using (var sqlConn = DBController.CreateDBConnection(PamUserDbInfo_Static))
                {
                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        sqlConn.Open();
                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            while (sqlDR.Read())
                            {
                                var rs = DbConvertController<RetailerCache>.ReadDataFromDataReader(sqlDR);
                                if (rs.RetailerId > 0)
                                {
                                    list.Add(rs);
                                }
                            }
                        }
                    }
                }

                List<RetailerVotesSum> retailerVotes = GetRetailerVotesSums(AppValue.CountryId);
                Dictionary<int, string> retailerStoreTypeDic = GetRetailerStroeType(AppValue.CountryId);
                Dictionary<int, int> clicksDic = GetRetailerClicks(AppValue.CountryId);

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
                        if (AppValue.ListVersionNoEnglishCountryId.Contains(item.RetailerCountry))
                            reviewStr = string.Format("{0} " + AppValue.ReviewStr, totalReviews);
                        else
                            reviewStr = string.Format("{0} " + AppValue.ReviewStr + "{1}", totalReviews, totalReviews > 1 ? "s" : "");

                        item.ReviewString = reviewStr;

                        decimal avRating = decimal.Round(((item.RetailerRatingSum - 3m) / ((item.RetailerTotalRatingVotes == 0 ? 2 : item.RetailerTotalRatingVotes) - 1m)), 1);
                        avRating = avRating.ToString().Length > 3 ? decimal.Parse(avRating.ToString().Substring(0, 3)) : avRating;
                        item.AvRating = avRating;
                    }

                    int clicks = 0;
                    clicksDic.TryGetValue(item.RetailerId, out clicks);
                    item.Clicks = clicks;
                }
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }

            return list.OrderByDescending(r => r.Clicks).ToList();
        }

        public static List<RetailerVotesSum> GetRetailerVotesSums(int countryId)
        {
            List<RetailerVotesSum> votes = new List<RetailerVotesSum>();

            string sql = @"select retailerid as ID,RetailerId ,SUM(OverallRating)+3 as RetailerRatingSum,COUNT(RetailerId)+1 as RetailerTotalRatingVotes from Merchant_Reviews
                            where RetailerId in (select RetailerId from CSK_Store_Retailer where RetailerCountry = " + countryId + @")
                            and ReviewStatus in (4, 5)
                            group by RetailerId";

            using (var sqlConn = DBController.CreateDBConnection(PamUserDbInfo_Static))
            {
                sqlConn.Open();

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            RetailerVotesSum vote = new RetailerVotesSum();
                            vote.ID = int.Parse(sqlDR["ID"].ToString());
                            vote.RetailerID = int.Parse(sqlDR["RetailerID"].ToString());
                            vote.RetailerRatingSum = int.Parse(sqlDR["RetailerRatingSum"].ToString());
                            vote.RetailerTotalRatingVotes = int.Parse(sqlDR["RetailerTotalRatingVotes"].ToString());

                            votes.Add(vote);
                        }
                    }
                }
            }

            return votes;
        }

        private static Dictionary<int, string> GetRetailerStroeType(int countryId)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string sql = "SELECT * FROM CSK_Store_RetailerStoreType";
            using (var sqlConn = DBController.CreateDBConnection(SubDbInfo_Static))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var retailerStoreType = DbConvertController<RetailerStoreType>.ReadDataFromDataReader(sqlDR);
                            if (retailerStoreType != null)
                            {
                                dic.Add(retailerStoreType.RetailerStoreTypeID, retailerStoreType.StoreTypeName);
                            }
                        }
                    }
                }
            }

            return dic;
        }

        private static Dictionary<int, int> GetRetailerClicks(int countryId)
        {
            Dictionary<int, int> retailerClicks = new Dictionary<int, int>();

            string sql = "CSK_Store_12RMB_Index_GetRetailerClicks";

            using (var sqlConn = DBController.CreateDBConnection(Priceme205DbInfo_Static))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlCMD.CommandTimeout = 0;

                    var countryIdParam = sqlCMD.CreateParameter();
                    countryIdParam.ParameterName = "@country";
                    countryIdParam.DbType = DbType.Int32;
                    countryIdParam.Value = countryId;
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

        public static Dictionary<string, string> GetStatusBarData()
        {
            var dic = new Dictionary<string, string>();

            try
            {
                string sql = @"select @ur :=(SUM(ProductRatingVotes) - count(*)) from CSK_Store_ProductVotesSum;
                            select @er := count(*) from CSK_Store_ExpertReviewAU;

                            set @ur = IFNULL(@ur,0);

                            select @rc := count(*) from CSK_Store_Retailer where CSK_Store_Retailer.RetailerStatus = 1 and IsSetupComplete = 1;

                            select count(DISTINCT CSK_Store_RetailerProductNew.productid) as productNum,count(CSK_Store_RetailerProductNew.RetailerProductID) as totalnum,@rc as retailerNum, @er + @ur as reviewNum from CSK_Store_RetailerProductNew
                            inner join CSK_Store_ProductNew on CSK_Store_ProductNew.productid = CSK_Store_RetailerProductNew.productid
                            inner join CSK_Store_Retailer on CSK_Store_Retailer.retailerid = CSK_Store_RetailerProductNew.retailerid
                            where CSK_Store_ProductNew.ProductStatus = 1 and CSK_Store_Retailer.RetailerStatus = 1 and CSK_Store_Retailer.IsSetupComplete = 1;";

                using (var sqlConn = DBController.CreateDBConnection(SubDbInfo_Static))
                {
                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        sqlConn.Open();
                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            sqlDR.NextResult();
                            sqlDR.NextResult();
                            sqlDR.NextResult();

                            if (sqlDR.Read())
                            {
                                string name = sqlDR.GetName(0);
                                string num = sqlDR[0].ToString();

                                string numString = GetNumberString(num);
                                dic.Add(name, numString);

                                name = sqlDR.GetName(1);
                                num = sqlDR[1].ToString();

                                numString = GetNumberString(num);
                                dic.Add(name, numString);

                                name = sqlDR.GetName(2);
                                num = sqlDR[2].ToString();

                                numString = GetNumberString(num);
                                dic.Add(name, numString);

                                name = sqlDR.GetName(3);
                                num = sqlDR[3].ToString();

                                numString = GetNumberString(num);
                                dic.Add(name, numString);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }

            return dic;
        }

        private static string GetNumberString(string num)
        {
            string numberString = "";
            int count = 0;
            if (int.TryParse(num, out count))
            {
                numberString = count.ToString("N0");
            }
            return numberString;
        }

        public static List<RetailerReviewCache> GetAllRetailerReviewList(int countryId)
        {
            List<RetailerReviewCache> _retailerReviewList = new List<RetailerReviewCache>();

            try
            {
                int ii = 0;
                using (var sqlConn = DBController.CreateDBConnection(Priceme205DbInfo_Static))
                {
                    sqlConn.Open();

                    string sql = "GetRetailerReviewByCountryID";
                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        sqlCMD.CommandType = CommandType.StoredProcedure;

                        var countryIdParam = sqlCMD.CreateParameter();
                        countryIdParam.ParameterName = "@countryID";
                        countryIdParam.DbType = DbType.Int32;
                        countryIdParam.Value = countryId;
                        sqlCMD.Parameters.Add(countryIdParam);

                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            while (sqlDR.Read())
                            {
                                RetailerReviewCache rrc = new RetailerReviewCache();

                                int.TryParse(sqlDR["RetailerReviewId"].ToString(), out ii);
                                rrc.ReviewID = ii;

                                int.TryParse(sqlDR["RetailerId"].ToString(), out ii);
                                rrc.RetailerID = ii;

                                int.TryParse(sqlDR["EasyOfOrdering"].ToString(), out ii);
                                rrc.EasyOfOrdering = ii;

                                int.TryParse(sqlDR["OnTimeDelivery"].ToString(), out ii);
                                rrc.OnTimeDelivery = ii;

                                int.TryParse(sqlDR["CustomerCare"].ToString(), out ii);
                                rrc.CustomerCare = ii;

                                int.TryParse(sqlDR["Availability"].ToString(), out ii);
                                rrc.Availability = ii;

                                int.TryParse(sqlDR["OverallStoreRating"].ToString(), out ii);
                                rrc.OverallStoreRating = ii;
                                rrc.OverallRating = ii;

                                rrc.Goods = sqlDR["Goods"].ToString();
                                rrc.Title = sqlDR["Title"].ToString();
                                rrc.Body = sqlDR["Body"].ToString();
                                rrc.IsApproved = bool.Parse(sqlDR["IsApproved"].ToString());
                                rrc.AdminComments = sqlDR["AdminComments"].ToString();
                                rrc.UserIP = sqlDR["UserIP"].ToString();
                                int totalComment = 0;
                                int.TryParse(sqlDR["TotalComment"].ToString(), out totalComment);
                                rrc.TotalComment = totalComment;
                                rrc.CreatedBy = sqlDR["CreatedBy"].ToString();
                                DateTime dt = DateTime.Now;
                                DateTime.TryParse(sqlDR["CreatedOn"].ToString(), out dt);
                                rrc.CreatedOn = dt;

                                _retailerReviewList.Add(rrc);
                            }
                        }
                        _retailerReviewList.ForEach(r => r.SourceType = "web");
                    }


                    List<RetailerReviewCache> rrcList = new List<RetailerReviewCache>();

                    sql = "GetRetailerReviewDetailByCountryID";
                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        sqlCMD.CommandType = CommandType.StoredProcedure;

                        sqlCMD.CommandType = CommandType.StoredProcedure;

                        var countryIdParam = sqlCMD.CreateParameter();
                        countryIdParam.ParameterName = "@countryID";
                        countryIdParam.DbType = DbType.Int32;
                        countryIdParam.Value = countryId;
                        sqlCMD.Parameters.Add(countryIdParam);

                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            while (sqlDR.Read())
                            {
                                RetailerReviewCache rrc = new RetailerReviewCache();

                                int.TryParse(sqlDR["ReviewID"].ToString(), out ii);
                                rrc.ReviewID = ii;

                                int.TryParse(sqlDR["RetailerID"].ToString(), out ii);
                                rrc.RetailerID = ii;

                                float ff = 0;
                                float.TryParse(sqlDR["Delivery"].ToString(), out ff);
                                rrc.Delivery = ff;

                                float.TryParse(sqlDR["Service"].ToString(), out ff);
                                rrc.Service = ff;

                                float.TryParse(sqlDR["EaseOfPurchase"].ToString(), out ff);
                                rrc.EaseOfPurchase = ff;

                                float.TryParse(sqlDR["OverallRating"].ToString(), out ff);
                                rrc.OverallRating = ff;

                                float.TryParse(sqlDR["ProductInfo"].ToString(), out ff);
                                rrc.ProductInfo = ff;

                                bool boo = true;
                                bool.TryParse(sqlDR["PurchaseAgain"].ToString(), out boo);
                                rrc.PurchaseAgain = boo;
                                rrc.Email = sqlDR["Email"].ToString();
                                rrc.Descriptive = sqlDR["Descriptive"].ToString();
                                rrc.UserIP = sqlDR["UserIP"].ToString();
                                rrc.CreatedBy = sqlDR["CreatedBy"].ToString();

                                DateTime dt = DateTime.Now;
                                DateTime.TryParse(sqlDR["CreatedOn"].ToString(), out dt);
                                rrc.CreatedOn = dt;
                                rrcList.Add(rrc);
                            }
                        }
                        rrcList.ForEach(r => r.SourceType = "review-system");
                    }

                    _retailerReviewList.AddRange(rrcList);
                }


                _retailerReviewList = _retailerReviewList.OrderByDescending(r => r.CreatedOn).ToList();
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + ex.StackTrace);
            }

            return _retailerReviewList;
        }

        public static List<FeaturedTabCache> GetAllFeaturedProducts(int countryId)
        {
            List<FeaturedTabCache> featuredProducts = new List<FeaturedTabCache>();

            try
            {
                string sql = "SELECT CategoryID,Label,Title,ListOrder FROM CSK_Store_FeaturedTab order by ListOrder";
                using (var sqlConn = DBController.CreateDBConnection(SubDbInfo_Static))
                {
                    sqlConn.Open();
                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            while (sqlDR.Read())
                            {
                                FeaturedTabCache ftc = new FeaturedTabCache();
                                ftc.CategoryID = sqlDR.GetInt32(0);
                                ftc.Label = sqlDR.GetString(1);
                                ftc.Title = sqlDR.GetString(2);
                                ftc.ListOrder = sqlDR.GetInt16(3);

                                featuredProducts.Add(ftc);
                            }
                        }
                    }

                    foreach (var ft in featuredProducts)
                    {
                        string selectFeaturedProductsSql = "";
                        if (sqlConn is MySql.Data.MySqlClient.MySqlConnection)
                        {
                            selectFeaturedProductsSql = @"SELECT  P.ProductID,P.ProductName,P.DefaultImage,P.CategoryID,RPP.MinPrice FROM CSK_Store_ProductNew P 
                                                            INNER JOIN
                                                            (SELECT RP.ProductID, MIN(RP.RetailerPrice) AS MinPrice FROM CSK_Store_RetailerProductNew RP 
                                                                LEFT JOIN
                                                                CSK_Store_Retailer R ON RP.RetailerID = R.RetailerID 
                                                                LEFT JOIN
                                                                CSK_Store_PPCMember PPC ON R.RetailerId = PPC.RetailerId
                                                                WHERE R.RetailerCountry= " + countryId + " AND PPC.RetailerCountry= " + countryId + @" AND R.RetailerStatus = 1
                                                                GROUP BY ProductID) as RPP ON RPP.ProductID = P.ProductID
                                                                WHERE P.CategoryID = " + ft.CategoryID + @" AND P.IsMerge = 1 AND P.DefaultImage is not null ORDER BY P.CreatedOn DESC
                                                                limit 10";

                        }
                        else
                        {
                            selectFeaturedProductsSql = @"SELECT top 10 P.ProductID,P.ProductName,P.DefaultImage,P.CategoryID,RPP.MinPrice FROM CSK_Store_ProductNew P 
                                                            INNER JOIN
                                                            (SELECT RP.ProductID, MIN(RP.RetailerPrice) AS MinPrice FROM CSK_Store_RetailerProductNew RP 
                                                                LEFT JOIN
                                                                CSK_Store_Retailer R ON RP.RetailerID = R.RetailerID 
                                                                LEFT JOIN
                                                                CSK_Store_PPCMember PPC ON R.RetailerId = PPC.RetailerId
                                                                WHERE R.RetailerCountry= " + countryId + " AND PPC.RetailerCountry= " + countryId + @" AND R.RetailerStatus = 1
                                                                GROUP BY ProductID) as RPP ON RPP.ProductID = P.ProductID
                                                                WHERE P.CategoryID = " + ft.CategoryID + @" AND P.IsMerge = 1 AND P.DefaultImage is not null ORDER BY P.CreatedOn DESC";

                        }


                        List<FeaturedProduct> featuredProductList = new List<FeaturedProduct>();

                        using (var sqlCMD = DBController.CreateDbCommand(selectFeaturedProductsSql, sqlConn))
                        {
                            using (var sqlDR = sqlCMD.ExecuteReader())
                            {
                                while (sqlDR.Read())
                                {
                                    FeaturedProduct featuredProduct = new FeaturedProduct();
                                    featuredProduct.ProductID = int.Parse(sqlDR["ProductID"].ToString());
                                    featuredProduct.ProductName = sqlDR["ProductName"].ToString();
                                    featuredProduct.DefaultImage = sqlDR["DefaultImage"].ToString();
                                    featuredProduct.CategoryID = int.Parse(sqlDR["CategoryID"].ToString());
                                    featuredProduct.RootID = 0;
                                    featuredProduct.MinPrice = double.Parse(sqlDR["MinPrice"].ToString());
                                    featuredProductList.Add(featuredProduct);
                                }
                            }

                            var cate = GetCategoryById(ft.CategoryID);
                            if (cate != null)
                            {
                                ft.Label = cate.CategoryName;
                            }

                            ft.FeaturedProductList = featuredProductList.Take(3).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }

            return featuredProducts;
        }

        public static Dictionary<int, string> GetEnergyImgs()
        {
            Dictionary<int, string> energyImgsDic = new Dictionary<int, string>();

            try
            {
                string sql = "select ProductId, EnergyImage from CSK_Store_Energy where ProductId is not null And ProductId != ''";
                using (var sqlConn = DBController.CreateDBConnection(Priceme205DbInfo_Static))
                {
                    sqlConn.Open();
                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            while (sqlDR.Read())
                            {
                                string pidString = sqlDR["ProductId"].ToString();
                                string image = sqlDR["EnergyImage"].ToString();
                                if (pidString.Contains(","))
                                {
                                    string[] temps = pidString.Split(',');
                                    for (int i = 0; i < temps.Length; i++)
                                    {
                                        int pid = 0;
                                        int.TryParse(temps[i], out pid);
                                        if (!energyImgsDic.ContainsKey(pid))
                                            energyImgsDic.Add(pid, image);
                                    }
                                }
                                else
                                {
                                    int pid = 0;
                                    int.TryParse(pidString, out pid);
                                    if (!energyImgsDic.ContainsKey(pid))
                                        energyImgsDic.Add(pid, image);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }

            return energyImgsDic;
        }

        private static List<ProductVariants> GetProductVariants()
        {
            List<ProductVariants> datas = new List<ProductVariants>();

            var sql = "select i.ProductID, i.LinedPID, i.VariantProductValue, i.BaseProductValue, i.VariantTypeID, t.VariantTitleName, t.Unit "
                    + "from IntraLinkingGenerationAndRelated i inner join VariantType t On i.VariantTypeID = t.VariantTypeID where LinkType = 'Variant'";
            using (var sqlConn = DBController.CreateDBConnection(PamUserDbInfo_Static))
            {
                sqlConn.Open();

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int pid = 0, lpid = 0, tid = 0;
                            int.TryParse(sqlDR["ProductID"].ToString(), out pid);
                            int.TryParse(sqlDR["LinedPID"].ToString(), out lpid);
                            int.TryParse(sqlDR["VariantTypeID"].ToString(), out tid);

                            ProductVariants pv = new ProductVariants();
                            pv.ProductId = pid;
                            pv.LinedPID = lpid;
                            pv.VariantType = tid;
                            pv.VariantProductValue = sqlDR["VariantProductValue"].ToString();
                            pv.BaseProductValue = sqlDR["BaseProductValue"].ToString();
                            pv.Unit = sqlDR["Unit"].ToString();
                            if (!string.IsNullOrEmpty(pv.Unit))
                                pv.DisplayName = pv.VariantProductValue + " " + pv.Unit;
                            else
                                pv.DisplayName = pv.VariantProductValue;

                            datas.Add(pv);
                        }
                    }
                }
            }

            return datas;
        }

        private static List<ProductVariants> GetVariantTpey()
        {
            List<ProductVariants> list = new List<ProductVariants>();

            var sql = "select * from VariantType";
            using (var sqlConn = DBController.CreateDBConnection(PamUserDbInfo_Static))
            {
                sqlConn.Open();

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int tid = 0;
                            int.TryParse(sqlDR["VariantTypeID"].ToString(), out tid);

                            ProductVariants pv = new ProductVariants();
                            pv.VariantType = tid;
                            pv.VariantTitleName = sqlDR["VariantTitleName"].ToString();

                            list.Add(pv);
                        }
                    }
                }
            }

            return list;
        }

        public static Dictionary<int, Dictionary<string, List<ProductVariants>>> GetVariants()
        {
            var dicVariants = new Dictionary<int, Dictionary<string, List<ProductVariants>>>();

            try
            {
                List<ProductVariants> listPv = GetProductVariants();
                List<ProductVariants> listVariantsType = GetVariantTpey();

                List<int> listPid = new List<int>();
                foreach (ProductVariants pv in listPv)
                {
                    if (!listPid.Contains(pv.ProductId))
                    {
                        listPid.Add(pv.ProductId);
                        List<ProductVariants> temps = listPv.Where(p => p.ProductId == pv.ProductId).ToList();
                        ProductVariants(temps, listVariantsType, dicVariants);
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }

            return dicVariants;
        }

        private static void ProductVariants(List<ProductVariants> temps, List<ProductVariants> listVariantsType, Dictionary<int, Dictionary<string, List<ProductVariants>>> dicVariants)
        {
            string pids = temps[0].ProductId.ToString();
            foreach (ProductVariants pv in temps)
            {
                pids += "," + pv.LinedPID.ToString();
            }
            Dictionary<int, string> dicProduct = GetProductNames(pids);
            if (dicProduct.Count == 0)
                return;

            List<ProductVariants> datas = new List<ProductVariants>();
            foreach (ProductVariants pv in temps)
            {
                if (dicProduct.ContainsKey(pv.LinedPID))
                {
                    pv.ProductName = dicProduct[pv.LinedPID];
                    datas.Add(pv);
                }
            }

            List<int> listType = datas.Select(t => t.VariantType).Distinct().ToList();

            if (listVariantsType.Count > 0)
            {
                foreach (int t in listType)
                {
                    ProductVariants type = listVariantsType.FirstOrDefault(lt => lt.VariantType == t);
                    if (type == null || type.VariantType == 0)
                        continue;

                    string titlename = type.VariantTitleName;

                    List<ProductVariants> listVt = datas.Where(p => p.VariantType == t).ToList();
                    if (listVt != null && listVt.Count > 0)
                    {
                        if (dicProduct.ContainsKey(listVt[0].ProductId))
                        {
                            ProductVariants basepv = new ProductVariants();
                            basepv.LinedPID = listVt[0].ProductId;
                            basepv.VariantProductValue = listVt[0].BaseProductValue;
                            basepv.VariantType = listVt[0].VariantType;
                            if (dicProduct.ContainsKey(listVt[0].ProductId))
                                basepv.ProductName = dicProduct[listVt[0].ProductId];
                            basepv.Unit = listVt[0].Unit;
                            if (!string.IsNullOrEmpty(listVt[0].Unit))
                                basepv.DisplayName = basepv.VariantProductValue + " " + listVt[0].Unit;
                            else
                                basepv.DisplayName = basepv.VariantProductValue;
                            listVt.Add(basepv);
                        }

                        foreach (ProductVariants pv in listVt)
                        {
                            List<ProductVariants> pvs = listVt.Where(p => p.LinedPID != pv.LinedPID).ToList();
                            if (pvs.Count > 0)
                            {
                                Dictionary<string, List<ProductVariants>> dicVt = new Dictionary<string, List<ProductVariants>>();

                                if (!dicVariants.ContainsKey(pv.LinedPID))
                                {
                                    dicVt.Add(titlename, pvs);
                                    dicVariants.Add(pv.LinedPID, dicVt);
                                }
                                else
                                {
                                    dicVt = dicVariants[pv.LinedPID];
                                    if (!dicVt.ContainsKey(titlename))
                                        dicVt.Add(titlename, pvs);
                                    dicVariants[pv.LinedPID] = dicVt;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static Dictionary<int, string> GetProductNames(string pids)
        {
            Dictionary<int, string> dicProduct = new Dictionary<int, string>();

            var sql = "Select ProductID, ProductName from CSK_Store_ProductNew Where ProductID in (" + pids + ")";
            using (var sqlConn = DBController.CreateDBConnection(SubDbInfo_Static))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int pid = 0;
                            int.TryParse(sqlDR["ProductID"].ToString(), out pid);

                            if (!dicProduct.ContainsKey(pid))
                                dicProduct.Add(pid, sqlDR["ProductName"].ToString());
                        }
                    }
                }
            }

            return dicProduct;
        }
    }
}
