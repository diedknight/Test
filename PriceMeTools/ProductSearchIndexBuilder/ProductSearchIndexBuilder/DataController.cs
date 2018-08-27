using ProductSearchIndexBuilder.Data;
using System;
using System.Collections.Generic;
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
        static List<CategoryCache> CategoryListOrderByName_Static;
        static Dictionary<int, List<CategoryExtend>> CategoryExtendListDic_Static;
        static List<int> IsSearchOnlyCategoryIdList_Static;
        static List<int> NotOverseasRetailer_Static;

        public static void Init(DbInfo priceme205DbInfo, DbInfo pamUserbInfo, int countryId)
        {
            CategoryListOrderByName_Static = GetCatOrderByNameOrderByName(priceme205DbInfo, countryId);
            CategoryDic_Static = CategoryListOrderByName_Static.ToDictionary(c => c.CategoryID, c => c);
            IsSearchOnlyCategoryIdList_Static = CategoryListOrderByName_Static.FindAll(c => c.IsSearchOnly).Select(c => c.CategoryID).ToList();
            NextLevelActiveCategories_Static = new Dictionary<int, List<CategoryCache>>();
            foreach (var c in CategoryListOrderByName_Static)
            {
                List<CategoryCache> categories = CategoryListOrderByName_Static.Where(cat => cat.ParentID == c.CategoryID).ToList();
                NextLevelActiveCategories_Static.Add(c.CategoryID, categories);
            }

            CategoryExtendListDic_Static = GetCategoryExtendDic(pamUserbInfo, countryId);

            AttributeTitleDic_Static = GetAttributeTitleDicFromDB(priceme205DbInfo);
            AttributeValueRangeList_Static = GetAttributeValueRangeCacheListFromDB(priceme205DbInfo);
            AttributeValueList_Static = GetAttributeValuesCacheListFromDB(priceme205DbInfo);
            AttributeTitleIDAndListOrderDic_Static = GetAttributeTitleIDAndListOrderDic(AttributeValueList_Static, AttributeValueRangeList_Static);

            CategoryHWDInfoDic_Static = GetCategoryHWDInfoDicFromDB(pamUserbInfo);
            CategoryAttributeTitleMapList_Static = GetCategoryAttributeTilteMapsFromDB(CategoryHWDInfoDic_Static, priceme205DbInfo);
            CategoryAttributeTitleDic_Static = GetCategoryAttributeTitleDic(CategoryAttributeTitleMapList_Static);

            NotOverseasRetailer_Static = LoadNotOverseasRetailer(priceme205DbInfo, countryId);
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

        private static Dictionary<int, List<CategoryExtend>> GetCategoryExtendDic(DbInfo pamUserbInfo, int countryId)
        {
            Dictionary<int, List<CategoryExtend>> listDic = new Dictionary<int, List<CategoryExtend>>();
            string selectCategorySynonymSql = "SELECT CategoryID,Synonym,LocalName, CountryID FROM CategorySynonym_New where CountryID = " + countryId;
            using (var sqlConn = DBController.CreateDBConnection(pamUserbInfo))
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
    }
}
