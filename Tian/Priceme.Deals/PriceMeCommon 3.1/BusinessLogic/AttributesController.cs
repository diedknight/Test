using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using PriceMeCommon.Convert;
using PriceMeCommon.Data;
using SubSonic.Schema;
using System.Data;
using PriceMeCache;
using System.Data.SqlClient;

namespace PriceMeCommon
{
    public static class AttributesController
    {
        static List<PriceMeCache.AttributeTitleCache> _attributesTitles = null;
        static List<PriceMeCache.CategoryAttributeTitleMapCache> _categoryAttributeTilteMaps = null;
        static List<PriceMeCache.AttributeValueCache> _attributeValues = null;
        static List<CSK_Store_AttributeValueRange> _attributeValueRanges = null;
        static Dictionary<int, List<PriceMeCache.AttributeTitleCache>> _categoryAttributeTitleDictionary = null;
        static Dictionary<int, List<PriceMeCache.AttributeValueCache>> _attributeTitlesValuesDictionary = null;
        static Dictionary<int, PriceMeCache.AttributeTitleCache> _attributeTitleDictionary = null;
        static Dictionary<int, PriceMeCache.AttributeValueCache> _attributeValueDictionary = null;
        static Dictionary<int, PriceMeCache.AttributeTitleCache> _attributeValueTitleDictionary = null;
        static Dictionary<int, List<CategoryCache>> _attributeTitleCategoryDictionary = null;
        static Dictionary<int, CSK_Store_AttributeValueRange> _attributeValueRangeDictionary = null;
        static Dictionary<string, CategoryAttributeTitleMapCache> _categoryAttributeTitleMapDictionary = null;
        static Dictionary<string, PriceMeCache.AttributeValueCache> _attributeTitleIDAndListOrderDictionary = null;
        static Dictionary<string, string> _productDescriptorDictionary = null;
        public static Dictionary<int, List<AttributeGroup>> attGroupDic = null;
        static Dictionary<int, string> attGroupTranslations = null;
        public static Dictionary<string, string> attNameTranslations = null;

        static Dictionary<int, CatalogAttributeGroupInfo> Static_CatalogAttributeGroupInfo = null;//分类页面的Attributes分组信息
        static Dictionary<int, int> Static_CatalogAttributeValue_GroupDic = null;

        static List<PriceMeCache.AttributeDisplayType> _AttributeDisplayType = null;

        static List<PriceMeCache.AttributeCategoryComparison> _AttributeCategoryComparison = null;
        static List<PriceMeCache.AttributeDisplayTypeValue> _AttributeDisplayTypeValue = null;

        static List<PriceMeCache.ProductDescAndAttr> _ProDescAndAttr = null;

        public static List<PriceMeCache.ProductDescAndAttr> ProDescAndAttr
        {
            get { return AttributesController._ProDescAndAttr; }
        }
        public static List<PriceMeCache.AttributeDisplayTypeValue> AttributeDisplayTypeValue
        {
            get { return AttributesController._AttributeDisplayTypeValue; }
        }

        public static List<PriceMeCache.AttributeCategoryComparison> AttributeCategoryComparison
        {
            get { return AttributesController._AttributeCategoryComparison; }
        }

        public static List<PriceMeCache.AttributeDisplayType> AttributeDisplayType
        {
            get { return AttributesController._AttributeDisplayType; }
        }
        public static Dictionary<string, CategoryAttributeTitleMapCache> CategoryAttributeTitleMapDictionary
        {
            get { return AttributesController._categoryAttributeTitleMapDictionary; }
        }

        public static Dictionary<int, List<CategoryCache>> AttributeTitleCategoryDictionary
        {
            get { return AttributesController._attributeTitleCategoryDictionary; }
        }

        public static Dictionary<int, List<PriceMeCache.AttributeTitleCache>> CategoryAttributeTitleDictionary
        {
            get { return AttributesController._categoryAttributeTitleDictionary; }
        }

        public static Dictionary<int, List<PriceMeCache.AttributeValueCache>> AttributeTitlesValuesDictionary
        {
            get { return AttributesController._attributeTitlesValuesDictionary; }
        }

        public static List<PriceMeCache.AttributeValueCache> AttributeValuesOrderByList
        {
            get { return AttributesController._attributeValues; }
        }

        public static List<PriceMeCache.AttributeTitleCache> AttributesTitles
        {
            get { return AttributesController._attributesTitles; }
        }

        public static List<CSK_Store_AttributeValueRange> AttributeValueRanges
        {
            get { return AttributesController._attributeValueRanges; }
        }

        public static List<PriceMeCache.CategoryAttributeTitleMapCache> CategoryAttributeTilteMapsOrderByList
        {
            get { return AttributesController._categoryAttributeTilteMaps; }
        }

        static AttributesController()
        {

        }

        public static void Load()
        {
            LoadCache(null);
        }

        public static void Load(Timer.DKTimer dkTimer)
        {
            if (ConfigAppString.StartDebug == false) dkTimer = null;
            LoadCache(dkTimer);
        }

        public static void LoadCache(Timer.DKTimer dkTimer)
        {
            if (dkTimer != null)
                dkTimer.Set("AttributesController.Load() --- Befor AttributeTitleCache");
            //
            _attributesTitles = VelocityController.GetCache<List<PriceMeCache.AttributeTitleCache>>(VelocityCacheKey.AttributeTitleCacheList);
            if (_attributesTitles == null)
            {
                _attributesTitles = ConvertController<PriceMeCache.AttributeTitleCache, CSK_Store_ProductDescriptorTitle>.ConvertData(CSK_Store_ProductDescriptorTitle.All().ToList());
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AttributeTitleCacheList no velocity");
            }

            _attributeTitleDictionary = new Dictionary<int, PriceMeCache.AttributeTitleCache>();
            foreach (PriceMeCache.AttributeTitleCache attributesTitle in _attributesTitles)
            {
                _attributeTitleDictionary.Add(attributesTitle.TypeID, attributesTitle);
            }

            if (dkTimer != null)
                dkTimer.Set("AttributesController.Load() --- Befor AttributeValueCache");
            //
            _attributeValues = VelocityController.GetCache<List<PriceMeCache.AttributeValueCache>>(VelocityCacheKey.AttributeValueCacheList);
            if (_attributeValues == null)
            {
                List<CSK_Store_AttributeValue> _attributeValuesDB = CSK_Store_AttributeValue.All().OrderBy(av => av.ListOrder).ToList();
                _attributeValues = ConvertController<PriceMeCache.AttributeValueCache, CSK_Store_AttributeValue>.ConvertData(_attributeValuesDB);
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AttributeValueCacheList no velocity");
            }

            _attributeValueDictionary = VelocityController.GetCache<Dictionary<int, PriceMeCache.AttributeValueCache>>(VelocityCacheKey.AttributeValueDictionary);
            if (_attributeValueDictionary == null)
            {
                _attributeValueDictionary = new Dictionary<int, AttributeValueCache>();
                foreach (PriceMeCache.AttributeValueCache attributeValue in _attributeValues)
                {
                    _attributeValueDictionary.Add(attributeValue.AttributeValueID, attributeValue);
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AttributeValueDictionary no velocity");
            }

            _attributeValueTitleDictionary = VelocityController.GetCache<Dictionary<int, PriceMeCache.AttributeTitleCache>>(VelocityCacheKey.AttributeValueTitleDictionary);
            if (_attributeValueTitleDictionary == null)
            {
                _attributeValueTitleDictionary = new Dictionary<int, AttributeTitleCache>();
                foreach (PriceMeCache.AttributeValueCache attributeValue in _attributeValues)
                {
                    _attributeValueTitleDictionary.Add(attributeValue.AttributeValueID, GetAttributeTitleByID(attributeValue.AttributeTitleID));
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AttributeValueTitleDictionary no velocity");
            }

            if (dkTimer != null)
                dkTimer.Set("AttributesController.Load() --- Befor AttributeValueRangeCache");
            //
            List<PriceMeCache.AttributeValueRangeCache> attributeValueRangeCacheList = VelocityController.GetCache<List<PriceMeCache.AttributeValueRangeCache>>(VelocityCacheKey.AttributeValueRangeCacheList);
            if (attributeValueRangeCacheList != null)
            {
                if (attributeValueRangeCacheList != null)
                {
                    _attributeValueRanges = ConvertController<CSK_Store_AttributeValueRange, PriceMeCache.AttributeValueRangeCache>.ConvertData(attributeValueRangeCacheList);
                }
            }
            else
            {
                _attributeValueRanges = CSK_Store_AttributeValueRange.All().ToList();
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AttributeValueRangeCacheList no velocity");
            }
            _attributeValueRangeDictionary = new Dictionary<int, CSK_Store_AttributeValueRange>();
            foreach (CSK_Store_AttributeValueRange attributeValueRange in _attributeValueRanges)
            {
                _attributeValueRangeDictionary.Add(attributeValueRange.ValueRangeID, attributeValueRange);
            }

            _attributeTitleIDAndListOrderDictionary = new Dictionary<string, PriceMeCache.AttributeValueCache>();
            foreach (AttributeValueCache attributeValue in _attributeValues)
            {
                if (attributeValue.ListOrder > 0)
                {
                    foreach (CSK_Store_AttributeValueRange attributeValueRange in _attributeValueRanges)
                    {
                        if (attributeValueRange.AttributeTitleID == attributeValue.AttributeTitleID)
                        {
                            string key = attributeValue.AttributeTitleID + "," + attributeValue.ListOrder;
                            if (!_attributeTitleIDAndListOrderDictionary.ContainsKey(key))
                            {
                                _attributeTitleIDAndListOrderDictionary.Add(key, attributeValue);
                            }
                            break;
                        }
                    }
                }
            }
            if (dkTimer != null)
                dkTimer.Set("AttributesController.Load() --- Befor CategoryAttributeTitleMapCache");
            //
            _categoryAttributeTilteMaps = VelocityController.GetCache<List<PriceMeCache.CategoryAttributeTitleMapCache>>(VelocityCacheKey.CategoryAttributeTilteMapList);
            if (_categoryAttributeTilteMaps == null)
            {
                _categoryAttributeTilteMaps = GetAllCategoryAttributeTitleMap();
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "categoryAttributeTitleMapCacheList no velocity");
            }

            if (dkTimer != null)
                dkTimer.Set("AttributesController.Load() --- Befor SetData()");
            //
            SetData(dkTimer);

            if (dkTimer != null)
            {
                dkTimer.Set("AttributesController.Load() --- Befor GetAllProductAttributeInfo");
            }
            //
            object cache = VelocityController.GetCache("CacheProductAttributeName");
            if (cache != null)
            {
                _productDescriptorDictionary = cache as Dictionary<string, string>;
            }
            if (_productDescriptorDictionary == null)
            {
                _productDescriptorDictionary = new Dictionary<string, string>();
                StoredProcedure sp = PriceMeCommon.PriceMeStatic.PriceMeDB.GetAllProductAttributeInfo();

                using (IDataReader idr = sp.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        string productDescriptor_ProductID = idr["ProductID"].ToString();
                        string productDescriptor_ProductDescriptorName = idr["ProductDescriptorName"].ToString();
                        int productDescriptor_TypeID = idr.GetInt32(2);

                        string key = productDescriptor_ProductID + "," + productDescriptor_TypeID;
                        PriceMeCache.AttributeTitleCache attributeTitle = GetAttributeTitleByID(productDescriptor_TypeID);
                        string value = productDescriptor_ProductDescriptorName;
                        if (attributeTitle != null)
                        {
                            value = value.Replace("&nbsp;", " ").Trim();
                            value += " " + attributeTitle.Unit;
                        }
                        if (!_productDescriptorDictionary.ContainsKey(key))
                        {
                            _productDescriptorDictionary.Add(key, value);
                        }
                    }
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "CacheProductAttributeName no velocity");
            }

            if (dkTimer != null)
            {
                dkTimer.Set("AttributesController.Load() --- Befor GetAllCategoryAttributeGroup()");
            }
            //Load att translation
            LoadAttGroupTranslations();
            LoadAttributeTranslations();

            attGroupDic = VelocityController.GetCache(VelocityCacheKey.AllCategoryAttributeGroup) as Dictionary<int, List<AttributeGroup>>;
            if (attGroupDic == null)
            {
                attGroupDic = GetAllCategoryAttributeGroup();
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AllCategoryAttributeGroup no velocity");
            }

            SetCatalogAttributeGroupInfo();

            SetAttributeDisplayType();
            SetAttributeCategoryComparison();
            //SetAttributeDisplayTypeValue();
            GetProductDescAndAttrData();
        }


        //select COUNT(*) from [Product_DescAndAttr]

        private static void GetProductDescAndAttrData()
        {
            //select [SID],[ProductID],[T] from [Product_DescAndAttr]


            var sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append(" [SID],");
            sql.Append(" [ProductID],");
            sql.Append(" [T]");
            sql.Append(" FROM");
            sql.Append(" [Product_DescAndAttr]");

            _ProDescAndAttr = new List<PriceMeCache.ProductDescAndAttr>();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;

            using (var sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (var sqlCMD = new System.Data.SqlClient.SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var desc = new PriceMeCache.ProductDescAndAttr();

                            int t = 0;
                            int proid = 0;
                            int aid = 0;
                            int.TryParse(sqlDR["SID"].ToString(), out aid);
                            int.TryParse(sqlDR["ProductID"].ToString(), out proid);
                            int.TryParse(sqlDR["T"].ToString(), out t);

                            desc.T = t;
                            desc.productid = proid;
                            desc.AttributeTypeID = aid;

                            _ProDescAndAttr.Add(desc);

                        }
                        sqlDR.Close();
                    }
                    sqlConn.Close();
                }
            }

        }


        private static void SetAttributeDisplayTypeValue()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append(" ID,");
            sql.Append(" DisplayTypeName");
            sql.Append(" FROM");
            sql.Append(" [AttributeDisplayTypeValue]");


            _AttributeDisplayTypeValue = new List<PriceMeCache.AttributeDisplayTypeValue>();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;

            using (var sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (var sqlCMD = new System.Data.SqlClient.SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var adt = new PriceMeCache.AttributeDisplayTypeValue();

                            int id = 1;

                            int.TryParse(sqlDR["ID"].ToString(), out id);

                            adt.ID = id;
                            adt.DisplayTypeName = sqlDR["DisplayTypeName"].ToString();

                            _AttributeDisplayTypeValue.Add(adt);

                        }
                        sqlDR.Close();
                    }
                    sqlConn.Close();
                }
            }
        } 

        private static void SetAttributeCategoryComparison()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append(" Aid,");
            sql.Append(" IsHigherBetter,");
            sql.Append(" Top10,");
            sql.Append(" Top20,");
            sql.Append(" Top30,");
            sql.Append(" Average,");
            sql.Append(" Bottom20,");
            sql.Append(" Bottom10,");
            sql.Append(" Createdon,");
            sql.Append(" Modifiedon,");
            sql.Append(" Bottom30,");
            sql.Append(" IsCompareAttribute");
            sql.Append(" FROM");
            sql.Append(" [AttributeCategoryComparison]");


            _AttributeCategoryComparison = new List<PriceMeCache.AttributeCategoryComparison>();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;

            using (var sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (var sqlCMD = new System.Data.SqlClient.SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            PriceMeCache.AttributeCategoryComparison adt = new PriceMeCache.AttributeCategoryComparison();

                            int id=1,aid=0,tid=1;
                            bool isbetter=false,is_com=false;
                            DateTime modifion = DateTime.Now,createon=DateTime.Now;

                            //int.TryParse(sqlDR["ID"].ToString(),out id);
                            int.TryParse(sqlDR["Aid"].ToString(), out aid);
                           // int.TryParse(sqlDR["TypeID"].ToString(),out tid);

                            bool.TryParse(sqlDR["IsHigherBetter"].ToString(), out isbetter);
                            bool.TryParse(sqlDR["IsCompareAttribute"].ToString(), out is_com);
                            DateTime.TryParse(sqlDR["Modifiedon"].ToString(), out modifion);
                            DateTime.TryParse(sqlDR["Createdon"].ToString(), out createon);


                            adt.Aid = aid;
                            adt.IsHigherBetter = isbetter;
                            adt.Top10 = sqlDR["Top10"].ToString();
                            adt.Top20 = sqlDR["Top20"].ToString();
                            adt.Top30 = sqlDR["Top30"].ToString();

                            adt.Bottom10 = sqlDR["Bottom10"].ToString();
                            adt.Bottom20 = sqlDR["Bottom20"].ToString();
                            adt.Bottom30 = sqlDR["Bottom30"].ToString();
                            adt.Average = sqlDR["Average"].ToString();

                            adt.IsCompareAttribute = is_com;
                            adt.Modifiedon = modifion;
                            adt.Createdon = createon;

                            _AttributeCategoryComparison.Add(adt);

                        }
                        sqlDR.Close();
                    }
                    sqlConn.Close();
                }
            }
        }
        private static void SetAttributeDisplayType() {
            var sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append(" ID,");
            sql.Append(" AttributeID,");
            sql.Append(" TypeID,");
            sql.Append(" IsComparison,");
            sql.Append(" DisplayAdjectiveBetter,");
            sql.Append(" DisplayAdjectiveWorse,");
            sql.Append(" IsCompareAttribute");
            sql.Append(" FROM");
            sql.Append(" [AttributeDisplayType]");
            _AttributeDisplayType = new List<PriceMeCache.AttributeDisplayType>();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;

            using (var sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (var sqlCMD = new System.Data.SqlClient.SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            PriceMeCache.AttributeDisplayType adt = new PriceMeCache.AttributeDisplayType();

                            int id=1,aid=0,tid=1;
                            bool iscom=false,is_com=false;

                            int.TryParse(sqlDR["ID"].ToString(),out id);
                            int.TryParse(sqlDR["AttributeID"].ToString(),out aid);
                            int.TryParse(sqlDR["TypeID"].ToString(),out tid);

                            bool.TryParse(sqlDR["IsComparison"].ToString(),out iscom);
                            bool.TryParse(sqlDR["IsCompareAttribute"].ToString(), out is_com);

                            adt.ID = id;
                            adt.AttributeID = aid;
                            adt.TypeID = tid;
                            adt.IsComparison = iscom;
                            adt.IsCompareAttribute = is_com;
                            adt.DisplayAdjectiveBetter = sqlDR["DisplayAdjectiveBetter"].ToString();
                            adt.DisplayAdjectiveWorse = sqlDR["DisplayAdjectiveWorse"].ToString();

                            _AttributeDisplayType.Add(adt);

                        }
                        sqlDR.Close();
                    }
                    sqlConn.Close();
                }
            }
        }

        private static void SetCatalogAttributeGroupInfo()
        {
            Static_CatalogAttributeGroupInfo = new Dictionary<int, CatalogAttributeGroupInfo>();
            Static_CatalogAttributeValue_GroupDic = new Dictionary<int, int>();

            string selectSql = "SELECT CAGM.AttributeValueID, CAGM.GroupID, GroupName FROM [CatalogAttributeGroupMap] as [CAGM] left join [CatalogAttributesGroup] as [CAG] on [CAGM].GroupID = [CAG].GroupID";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int valueID = sqlDR.GetInt32(0);
                            int groupID = sqlDR.GetInt32(1);
                            string groupName = sqlDR.GetString(2);

                            Static_CatalogAttributeValue_GroupDic.Add(valueID, groupID);
                            if (Static_CatalogAttributeGroupInfo.ContainsKey(groupID))
                            {
                                Static_CatalogAttributeGroupInfo[groupID].AttributeValues.Add(valueID);
                            }
                            else
                            {
                                CatalogAttributeGroupInfo cagi = new CatalogAttributeGroupInfo();
                                cagi.CatalogAttributeGroupID = groupID;
                                cagi.CatalogAttributeGroupName = groupName;
                                cagi.AttributeValues.Add(valueID);
                                Static_CatalogAttributeGroupInfo.Add(groupID, cagi);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据catalog页面的AttributeGroupID获取对应的AttributeValues
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static List<int> GetCatalogAttributeValues(int groupID)
        {
            if (Static_CatalogAttributeGroupInfo.ContainsKey(groupID))
            {
                return Static_CatalogAttributeGroupInfo[groupID].AttributeValues;
            }
            return null;
        }

        /// <summary>
        /// 根据attributeValueID获取catalog页面的AttributeGroupID
        /// </summary>
        /// <param name="attributeValueID"></param>
        /// <returns></returns>
        public static int GetCatalogAttributeGroupID(int attributeValueID)
        {
            if(Static_CatalogAttributeValue_GroupDic.ContainsKey(attributeValueID))
            {
                return Static_CatalogAttributeValue_GroupDic[attributeValueID];
            }
            return 0;
        }

        /// <summary>
        /// 根据catalog页面的AttributeGroupID获取对应的AttributeGroup名字
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static string GetCatalogAttributeGroupName(int groupID)
        {
            if (Static_CatalogAttributeGroupInfo.ContainsKey(groupID))
            {
                return Static_CatalogAttributeGroupInfo[groupID].CatalogAttributeGroupName;
            }
            return null;
        }

        //private static Dictionary<int, string> GetCatalogAttributeGroupInfo()
        //{
        //    Dictionary<int, string> dic = new Dictionary<int, string>();
        //    string selectSql = "SELECT GroupID,GroupName FROM [dbo].[CatalogAttributesGroup]";
        //    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
        //    using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
        //    {
        //        using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
        //        {
        //            sqlConn.Open();
        //            using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
        //            {
        //                while (sqlDR.Read())
        //                {
        //                    int groupID = sqlDR.GetInt32(0);
        //                    string groupName = sqlDR.GetString(1);
        //                    dic.Add(groupID, groupName);
        //                }
        //            }
        //        }
        //    }

        //    return dic;
        //}

        private static Dictionary<int, int> GetCatalogAttributeGroupMapDic()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            string selectSql = "SELECT AttributeValueID, GroupID FROM [dbo].[CatalogAttributeGroupMap]";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int attributeValueID = sqlDR.GetInt32(0);
                            int groupID = sqlDR.GetInt32(1);
                            dic.Add(attributeValueID, groupID);
                        }
                    }
                }
            }

            return dic;
        }

        public static Dictionary<int, Dictionary<string, int>> GetProductsAttributes(List<int> attributeTypes, List<string> productIDs)
        {
            Dictionary<int, Dictionary<string, int>> dic = new Dictionary<int, Dictionary<string, int>>();
            string selectSql = @"SELECT 
                                [ProductID]
                                ,[TypeID]
                                ,[AttributeValueID]
                                FROM[dbo].[CSK_Store_ProductDescriptor]
                                where TypeID in (" + string.Join(",", attributeTypes) + ") and productid in (" + string.Join(",", productIDs) + ")";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            try
                            {
                                string productID = sqlDR.GetInt32(0).ToString();
                                int attributeTitleID = sqlDR.GetInt32(1);
                                int AttributeValueID = sqlDR.GetInt32(2);

                                if (dic.ContainsKey(attributeTitleID))
                                {
                                    dic[attributeTitleID].Add(productID, AttributeValueID);
                                }
                                else
                                {
                                    dic.Add(attributeTitleID, new Dictionary<string, int>() { { productID, AttributeValueID } });
                                }
                            }
                            catch (Exception ex)
                            {
                                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.ExceptionLogPath, "GetProductsAttributes() " + ex.Message);
                            }
                        }
                    }
                }
            }

            return dic;
        }

        public static string GetProductAttributeValueName(string key)
        {
            if (_productDescriptorDictionary.ContainsKey(key))
            {
                return _productDescriptorDictionary[key];
            }
            return "";
        }

        private static void SetData(Timer.DKTimer dkTimer)
        {
            if (dkTimer != null)
                dkTimer.Set("AttributesController.Load() --- SetData()  --- Befor AttributeTitlesValuesCollection");
            //
            _attributeTitlesValuesDictionary = VelocityController.GetCache(VelocityCacheKey.AttributeTitlesValuesCollection) as Dictionary<int, List<PriceMeCache.AttributeValueCache>>;
            if (_attributeTitlesValuesDictionary == null)
            {
                _attributeTitlesValuesDictionary = new Dictionary<int, List<AttributeValueCache>>();
                foreach (PriceMeCache.AttributeTitleCache pdt in _attributesTitles)
                {
                    List<PriceMeCache.AttributeValueCache> attributeValueCollection = new List<AttributeValueCache>();
                    foreach (PriceMeCache.AttributeValueCache av in _attributeValues)
                    {
                        if (av.AttributeTitleID == pdt.TypeID)
                        {
                            attributeValueCollection.Add(av);
                        }
                    }
                    _attributeTitlesValuesDictionary.Add(pdt.TypeID, attributeValueCollection);
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AttributeTitlesValuesCollection no velocity");
            }


            //
            _categoryAttributeTitleDictionary = VelocityController.GetCache(VelocityCacheKey.CategoryAttributeTitleDictionary) as Dictionary<int, List<PriceMeCache.AttributeTitleCache>>;
            if (_categoryAttributeTitleDictionary == null)
            {
                _categoryAttributeTitleDictionary = new Dictionary<int, List<AttributeTitleCache>>();
                foreach (CategoryCache category in CategoryController.CategoryOrderByName)
                {
                    List<PriceMeCache.AttributeTitleCache> productDescriptorTitleCollection = new List<PriceMeCache.AttributeTitleCache>();
                    foreach (PriceMeCache.CategoryAttributeTitleMapCache categoryAttributeTitleMap in _categoryAttributeTilteMaps)
                    {
                        if (categoryAttributeTitleMap.IsPrimary)
                        {
                            if (categoryAttributeTitleMap.CategoryID == category.CategoryID)
                            {
                                productDescriptorTitleCollection.Add(GetAttributeTitleByID(categoryAttributeTitleMap.AttributeTitleID));
                            }
                        }
                    }
                    _categoryAttributeTitleDictionary.Add(category.CategoryID, productDescriptorTitleCollection);
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "CategoryAttributeTitleDictionary no velocity");
            }

            if (dkTimer != null)
                dkTimer.Set("AttributesController.Load() --- SetData()  --- Befor AttributeTitleCategoryDictionary");
            //
            object cache = VelocityController.GetCache(VelocityCacheKey.AttributeTitleCategoryDictionary);
            if (cache != null)
            {
                Dictionary<int, List<PriceMeCache.CategoryCache>> attributeTitleCategoryCache = cache as Dictionary<int, List<PriceMeCache.CategoryCache>>;
                if (attributeTitleCategoryCache != null)
                {
                    _attributeTitleCategoryDictionary = new Dictionary<int, List<CategoryCache>>();
                    foreach (int key in attributeTitleCategoryCache.Keys)
                    {
                        _attributeTitleCategoryDictionary.Add(key, ConvertController<CategoryCache, PriceMeCache.CategoryCache>.ConvertData(attributeTitleCategoryCache[key]));
                    }
                }
            }
            if (_attributeTitleCategoryDictionary == null)
            {
                _attributeTitleCategoryDictionary = new Dictionary<int, List<CategoryCache>>();
                foreach (PriceMeCache.AttributeTitleCache productDescriptorTitle in _attributesTitles)
                {
                    List<CategoryCache> categoryCollection = new List<CategoryCache>();
                    foreach (PriceMeCache.CategoryAttributeTitleMapCache catm in _categoryAttributeTilteMaps)
                    {
                        if (catm.AttributeTitleID == productDescriptorTitle.TypeID)
                        {
                            categoryCollection.Add(CategoryController.GetCategoryByCategoryID(catm.CategoryID));
                        }
                    }
                    _attributeTitleCategoryDictionary.Add(productDescriptorTitle.TypeID, categoryCollection);
                }
            }
            if (dkTimer != null)
                dkTimer.Set("AttributesController.Load() --- SetData()  --- Befor CategoryAttributeTitleMapDictionary");

            //categoryAttributeTitleMapDictionary
            cache = VelocityController.GetCache(VelocityCacheKey.CategoryAttributeTitleMapDictionary);
            if (cache != null)
            {
                Dictionary<string, PriceMeCache.CategoryAttributeTitleMapCache> categoryAttributeTitleMapDictionaryCache = cache as Dictionary<string, PriceMeCache.CategoryAttributeTitleMapCache>;
                if (categoryAttributeTitleMapDictionaryCache != null)
                {
                    _categoryAttributeTitleMapDictionary = new Dictionary<string, CategoryAttributeTitleMapCache>();
                    foreach (string key in categoryAttributeTitleMapDictionaryCache.Keys)
                    {
                        _categoryAttributeTitleMapDictionary.Add(key, categoryAttributeTitleMapDictionaryCache[key]);
                    }
                }
            }
            if (_categoryAttributeTitleMapDictionary == null)
            {
                _categoryAttributeTitleMapDictionary = new Dictionary<string, CategoryAttributeTitleMapCache>();
                List<CategoryAttributeTitleMapCache> categoryAttributeTitleMapCollection = GetAllCategoryAttributeTitleMap();
                foreach (CategoryAttributeTitleMapCache categoryAttributeTitleMap in categoryAttributeTitleMapCollection)
                {
                    string key = categoryAttributeTitleMap.CategoryID + "," + categoryAttributeTitleMap.AttributeTitleID;
                    if (!_categoryAttributeTitleMapDictionary.ContainsKey(key))
                    {
                        _categoryAttributeTitleMapDictionary.Add(key, categoryAttributeTitleMap);
                    }
                }
            }
        }

        private static void SetMinMaxValue(Dictionary<string, CategoryAttributeTitleMapCache> _categoryAttributeTitleMapDictionary)
        {
            foreach(var camc in _categoryAttributeTitleMapDictionary.Values)
            {
                if(camc.IsSlider)
                {
                    var avList = GetAttributeValueByTitleID(camc.AttributeTitleID);
                    if(avList.Count > 1)
                    {
                        foreach (var av in avList)
                        {
                            av.NumericValue = float.Parse(av.Value);
                        }

                        avList = avList.OrderBy(av => av.NumericValue).ToList();

                        //
                        float minValue = avList[0].NumericValue;
                        int min = (int)minValue;
                        if(minValue < 10)
                        {
                            minValue = min;
                        }
                        else if (minValue < 100)
                        {
                            minValue = min / 10 * 10f;
                        }
                        else
                        {
                            minValue = min / 100 * 100f;
                        }
                        camc.MinValue = minValue;

                        //
                        float maxValue = avList[avList.Count - 1].NumericValue;
                        int max = (int)maxValue;

                        if (maxValue < 100)
                        {
                            maxValue = (max / 10 + 1) * 10f;
                        }
                        else
                        {
                            maxValue = (max / 100 + 1 ) * 100f;
                        }
                        camc.MaxValue = maxValue;
                    }
                }
            }
        }

        public static List<CategoryAttributeTitleMapCache> GetAllCategoryAttributeTitleMap()
        {
            string sql = @"SELECT [MapID]
                          ,[CategoryID]
                          ,[AttributeTitleID]
                          ,[IsPrimary]
                          ,[AttributeOrder]
                          ,[IsSlider]
                          ,[Step]
                          ,[Step2]
                          ,[MinValue]
                          ,[MaxValue]
                           ,[Step3]
                            FROM [CSK_Store_Category_AttributeTitle_Map]";

            SubSonic.Schema.StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandTimeout = 0;

            List<CategoryAttributeTitleMapCache> list = new List<CategoryAttributeTitleMapCache>();
            using (IDataReader dr = sp.ExecuteReader())
            {
                while (dr.Read())
                {
                    CategoryAttributeTitleMapCache cache = new CategoryAttributeTitleMapCache();
                    cache.MapID = dr.GetInt32(0);
                    cache.CategoryID = dr.GetInt32(1);
                    cache.AttributeTitleID = dr.GetInt32(2);
                    cache.IsPrimary = dr.GetBoolean(3);
                    cache.AttributeOrder = dr.GetInt32(4);
                    cache.IsSlider = dr.GetBoolean(5);

                    string floatString = dr["Step"].ToString();
                    float floatValue = 0f;
                    float.TryParse(floatString, out floatValue);
                    cache.Step = floatValue;

                    floatString = dr["Step2"].ToString();
                    floatValue = 0f;
                    float.TryParse(floatString, out floatValue);
                    cache.Step2 = floatValue;

                    string step3String = dr["Step3"].ToString();
                    int step3;
                    int.TryParse(step3String, out step3);
                    cache.Step3 = step3;

                    floatString = dr["MinValue"].ToString();
                    floatValue = 0f;
                    float.TryParse(floatString, out floatValue);
                    cache.MinValue = floatValue;

                    floatString = dr["MaxValue"].ToString();
                    floatValue = 0f;
                    float.TryParse(floatString, out floatValue);
                    cache.MaxValue = floatValue;
                    list.Add(cache);
                }
            }

            return list;
        }

        public static CategoryAttributeTitleMapCache GetCategoryAttributeTitleMapByKey(string key)
        {
            if (_categoryAttributeTitleMapDictionary == null)
                return null;
            if (_categoryAttributeTitleMapDictionary.ContainsKey(key))
            {
                return _categoryAttributeTitleMapDictionary[key];
            }
            return null;
        }

        public static PriceMeCache.AttributeTitleCache GetAttributeTitleByID(int attributeTitleID)
        {
            if (_attributeTitleDictionary == null)
                return null;
            if (_attributeTitleDictionary.ContainsKey(attributeTitleID))
            {
                return _attributeTitleDictionary[attributeTitleID];
            }
            return null;
        }

        public static PriceMeCache.AttributeValueCache GetAttributeValueByID(int attributeValueID)
        {
            if (_attributeTitleDictionary == null)
                return null;
            if (_attributeValueDictionary.ContainsKey(attributeValueID))
            {
                return _attributeValueDictionary[attributeValueID];
            }
            return null;
        }

        public static PriceMeCache.AttributeTitleCache GetAttributeTitleByVauleID(int attributeValueID)
        {
            if (_attributeValueTitleDictionary == null)
                return null;
            if (_attributeValueTitleDictionary.ContainsKey(attributeValueID))
            {
                return _attributeValueTitleDictionary[attributeValueID];
            }
            return null;
        }

        public static List<PriceMeCache.AttributeTitleCache> GetAttributesTitleByCategoryID(int categoryID)
        {
            if (_categoryAttributeTitleDictionary == null)
                return null;
            if (_categoryAttributeTitleDictionary.ContainsKey(categoryID))
            {
                return _categoryAttributeTitleDictionary[categoryID];
            }
            return null;
        }

        public static List<PriceMeCache.CategoryCache> GetCategoriesAttributesTitleID(int attributesTitleID)
        {
            if (_attributeTitleCategoryDictionary == null)
                return null;
            if (_attributeTitleCategoryDictionary.ContainsKey(attributesTitleID))
            {
                return _attributeTitleCategoryDictionary[attributesTitleID];
            }
            return null;
        }

        public static CSK_Store_AttributeValueRange GetAttributeValueRangeByID(int attributeValueRangeID)
        {
            if (_attributeValueRangeDictionary == null)
                return null;
            if (_attributeValueRangeDictionary.ContainsKey(attributeValueRangeID))
            {
                return _attributeValueRangeDictionary[attributeValueRangeID];
            }
            return null;
        }

        public static List<CSK_Store_AttributeValueRange> GetAttributeValueRangesByTitleIDAndCategoryID(int attributeTitleID, int categoryID)
        {
            List<CSK_Store_AttributeValueRange> attributeValueRangeCollection = new List<CSK_Store_AttributeValueRange>();
            foreach (CSK_Store_AttributeValueRange avr in _attributeValueRanges)
            {
                if (avr.AttributeTitleID == attributeTitleID && avr.CategoryID == categoryID)
                {
                    attributeValueRangeCollection.Add(avr);
                }
            }
            return attributeValueRangeCollection;
        }

        public static List<AttributeValueCache> GetAttributeValuesByValueRangeID(int attributeValueRangeID)
        {
            List<AttributeValueCache> attributeValueCollection = new List<AttributeValueCache>();
            int minValue = 0;
            int maxValue = 0;
            int attributeTitleID = 0;
            foreach (CSK_Store_AttributeValueRange avr in _attributeValueRanges)
            {
                if (avr.ValueRangeID == attributeValueRangeID)
                {
                    minValue = avr.MinValue.Value;
                    maxValue = avr.MaxValue.Value;
                    attributeTitleID = avr.AttributeTitleID.Value;
                    break;
                }
            }
            if ((minValue == 0 && maxValue == 0) || attributeTitleID == 0)
            {
                return attributeValueCollection;
            }

            if (maxValue == 0)
            {
                maxValue = minValue;
            }
            else if (maxValue == -1)
            {
                maxValue = 9999;
            }

            if (_attributeTitlesValuesDictionary.ContainsKey(attributeTitleID))
            {
                List<AttributeValueCache> _attributeValues = _attributeTitlesValuesDictionary[attributeTitleID];
                foreach (AttributeValueCache av in _attributeValues)
                {
                    if (av.AttributeTitleID == attributeTitleID && av.ListOrder >= minValue && av.ListOrder <= maxValue)
                    {
                        attributeValueCollection.Add(av);
                    }
                }
            }
            return attributeValueCollection;
        }

        public static List<PriceMeCache.AttributeValueCache> GetAttributeValueByTitleID(int attributeTitleID)
        {
            if (_attributeTitlesValuesDictionary.ContainsKey(attributeTitleID))
            {
                return _attributeTitlesValuesDictionary[attributeTitleID];
            }
            else
            {
                return new List<AttributeValueCache>();
            }
        }

        public static NarrowByInfo GetAttributesResulteList(int attributeTitleID, int categoryID)
        {
            PriceMeCache.AttributeTitleCache attributeTitle = GetAttributeTitleByID(attributeTitleID);
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            if (attributeTitle == null)
            {
                return narrowByInfo;
            }

            //判断是否slider attribute
            var isSlider = false;
            var key = categoryID + "," + attributeTitleID;
            var map = GetCategoryAttributeTitleMapByKey(key);
            if (map != null && map.IsSlider)
            {
                isSlider = true;
            }

            List<NarrowItem> narrowItemList = new List<NarrowItem>();
            if (HasAttributeRange(attributeTitleID, categoryID) && !isSlider)
            {
                narrowByInfo.Name = "AttributeRange";
                List<CSK_Store_AttributeValueRange> attributeValueRangeCollection = GetAttributeValueRangesByTitleIDAndCategoryID(attributeTitleID, categoryID);
                foreach (CSK_Store_AttributeValueRange avr in attributeValueRangeCollection)
                {
                    NarrowItem narrowItem = new NarrowItem();
                    narrowItem.DisplayName = GetAttributeValueString(avr, attributeTitle.Unit);
                    narrowItem.IsPopular = true;
                    narrowItem.Value = avr.ValueRangeID.ToString();
                    narrowItemList.Add(narrowItem);
                }
            }
            else
            {
                narrowByInfo.Name = "Attribute";
                List<AttributeValueCache> attributeValueCollection = GetAttributeValueByTitleID(attributeTitleID);
                foreach (AttributeValueCache av in attributeValueCollection)
                {
                    NarrowItem narrowItem = new NarrowItem();
                    if (!string.IsNullOrEmpty(attributeTitle.Unit))
                    {
                        narrowItem.DisplayName = av.Value + " " + attributeTitle.Unit;
                        narrowItem.OtherInfo = attributeTitle.Unit;
                    }
                    else
                    {
                        narrowItem.DisplayName = av.Value;
                    }
                    narrowItem.IsPopular = av.PopularAttribute;
                    narrowItem.Value = av.AttributeValueID.ToString();
                    //把attr value 转换成int， 排序
                    var vv = 0;
                    var vv_ = av.Value.Replace(",", "");
                    float floatValue;
                    float.TryParse(vv_, out floatValue);
                    narrowItem.FloatValue = floatValue;
                    if (vv_.Contains("."))
                        vv_ = vv_.Substring(0, vv_.IndexOf("."));
                    int.TryParse(vv_, out vv);
                    narrowItem.ListOrder = vv;
                    narrowItemList.Add(narrowItem);
                }
                //按attr value 排序
                //Value 有小数， 都会转成int， 所以要.ThenBy(p => p.Value)
                if (narrowItemList.Count > 0 && narrowItemList[0].ListOrder > 0)
                    narrowItemList = narrowItemList.OrderBy(p => p.ListOrder).ThenBy(p => p.Value).ToList();
            }

            narrowByInfo.IsSlider = isSlider;
            narrowByInfo.CategoryAttributeTitleMap = map;
            narrowByInfo.NarrowItemList = narrowItemList;
            return narrowByInfo;
        }

        public static string GetAttributeValueString(CSK_Store_AttributeValueRange avr, string unit)
        {
            string avString = "";
            int minValue = avr.MinValue.Value;
            int maxValue = avr.MaxValue.Value;

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
                if (_attributeTitleIDAndListOrderDictionary.ContainsKey(key))
                {
                    maxString = _attributeTitleIDAndListOrderDictionary[key].Value + " " + unit;
                }
            }
            else if (avr.MaxValue == -1)
            {
                minString = "Above ";
                string key = avr.AttributeTitleID + "," + (avr.MinValue - 1);
                if (_attributeTitleIDAndListOrderDictionary.ContainsKey(key))
                {
                    maxString = _attributeTitleIDAndListOrderDictionary[key].Value + " " + unit;
                }
            }
            else if (avr.MaxValue == 0)
            {
                minString = "";
                string key = avr.AttributeTitleID + "," + avr.MinValue;
                if (_attributeTitleIDAndListOrderDictionary.ContainsKey(key))
                {
                    maxString = _attributeTitleIDAndListOrderDictionary[key].Value + " " + unit;
                }
            }
            else
            {
                if (_attributeTitlesValuesDictionary.ContainsKey(avr.AttributeTitleID.Value))
                {
                    List<PriceMeCache.AttributeValueCache> _attributeValues = _attributeTitlesValuesDictionary[avr.AttributeTitleID.Value];
                    foreach (PriceMeCache.AttributeValueCache av in _attributeValues)
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

        public static CSK_Store_AttributeValueRange GetAttributeRange(PriceMeCache.AttributeValueCache av, int categoryID)
        {
            CSK_Store_AttributeValueRange attributeValueRange = null;

            foreach (CSK_Store_AttributeValueRange avr in _attributeValueRanges)
            {
                if (avr.AttributeTitleID == av.AttributeTitleID && avr.CategoryID == categoryID)
                {
                    if (av.ListOrder == avr.MinValue || av.ListOrder == avr.MaxValue || (av.ListOrder > avr.MinValue && av.ListOrder < avr.MaxValue))
                    {
                        attributeValueRange = avr;
                    }
                }
            }
            return attributeValueRange;
        }

        public static bool HasAttributeRange(int attributeTitleID, int categoryID)
        {
            foreach (CSK_Store_AttributeValueRange avr in _attributeValueRanges)
            {
                if (avr.AttributeTitleID == attributeTitleID && avr.CategoryID == categoryID)
                {
                    return true;
                }
            }
            return false;
        }

        public static CSK_Store_ProductDescriptorTitle GetAttributeTitleByAttributeRangeID(int arid)
        {
            CSK_Store_AttributeValueRange attributeValueRange = CSK_Store_AttributeValueRange.SingleOrDefault(avr => avr.ValueRangeID == arid);
            if (attributeValueRange != null)
            {
                return CSK_Store_ProductDescriptorTitle.SingleOrDefault(avT => avT.TypeID == attributeValueRange.AttributeTitleID.Value);
            }
            return null;
        }

        public static void LoadAttGroupTranslations()
        {
            attGroupTranslations = new Dictionary<int, string>();
            string sql = string.Format("Select AttributeGroupId, AttributeGroupName From CSK_Store_AttributeGroupTranslation Where CountryId = {0}", ConfigAppString.CountryID);
            using (SqlConnection conn = new SqlConnection(ConfigAppString.CommerceTemplateCommon))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                IDataReader idr = cmd.ExecuteReader();
                while (idr.Read())
                {
                    int aid = 0;
                    int.TryParse(idr["AttributeGroupId"].ToString(), out aid);
                    string attName = idr["AttributeGroupName"].ToString();
                    if (!attGroupTranslations.ContainsKey(aid))
                        attGroupTranslations.Add(aid, attName);
                }
                conn.Close();
            }
        }

        public static void LoadAttributeTranslations()
        {
            attNameTranslations = new Dictionary<string, string>();
            string sql = string.Format("Select AttributName, AttributNameTranslation From dbo.CSK_Store_AttributeTranslation Where CountryId = {0}", ConfigAppString.CountryID);
            using (SqlConnection conn = new SqlConnection(ConfigAppString.CommerceTemplateCommon))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                IDataReader idr = cmd.ExecuteReader();
                while (idr.Read())
                {
                    string attName = idr["AttributName"].ToString().ToLower();
                    string attNameTrans = idr["AttributNameTranslation"].ToString().ToLower();
                    if (!attNameTranslations.ContainsKey(attName))
                        attNameTranslations.Add(attName, attNameTrans);
                }
                conn.Close();
            }
        }

        public static List<AttributeGroup> GetAttributeGroupByCategoryId(int cid)
        {
            if (attGroupDic != null && attGroupDic.ContainsKey(cid))
                return attGroupDic[cid];
            else
                return null;
        }

        #region 所有分类的 Attribute Group
        private static Dictionary<int, string> groupDic;

        public static Dictionary<int, List<AttributeGroup>> GetAllCategoryAttributeGroup()
        {
            GetAllAttributeGroup();
            Dictionary<int, List<AttributeGroup>> attGroupDic = new Dictionary<int, List<AttributeGroup>>();
            List<int> cidList = GetAllCategoryAttribute();
            foreach (int cid in cidList)
            {
                Dictionary<int, AttributeGroup> groupDic = new Dictionary<int, AttributeGroup>();
                GetAllAttributeByCategoryId(cid, groupDic);
                GetAllCompareAttributeByCategoryId(cid, groupDic);
                List<AttributeGroup> groupList = new List<AttributeGroup>(groupDic.Values);
                groupList = groupList.OrderBy(g => g.OrderID).ToList();
                attGroupDic.Add(cid, groupList);
            }

            return attGroupDic;
        }

        private static void GetAllAttributeByCategoryId(int cid, Dictionary<int, AttributeGroup> groupDic)
        {
            string sql = "select m.AttributeTitleID, t.Title, t.AttributeGroupID, t.ShortDescription, t.AttributeTypeID from dbo.CSK_Store_Category_AttributeTitle_Map m "
                        + "inner join CSK_Store_ProductDescriptorTitle t on m.AttributeTitleID = t.TypeID where m.CategoryID = " + cid
                        + " order by m.AttributeOrder";
            SubSonic.Schema.StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandTimeout = 0;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                string groupid = dr["AttributeGroupID"].ToString();
                int gid = 0;
                int.TryParse(groupid, out gid);
                int titleId = int.Parse(dr["AttributeTitleID"].ToString());
                string title = dr["Title"].ToString();
                string des = dr["ShortDescription"].ToString();
                string typeId = dr["AttributeTypeID"].ToString();
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
                    string temp = GetAttributeGroupById(gid);
                    string[] temps = temp.Split('|');
                    if (temps.Length > 1)
                    {
                        group.AttributeGroupName = temps[0];
                        group.OrderID = int.Parse(temps[1]);
                        group.AttributeGroupList = glistList;
                        groupDic.Add(gid, group);
                    }
                }
            }
            dr.Close();
        }

        private static void GetAllCompareAttributeByCategoryId(int cid, Dictionary<int, AttributeGroup> groupDic)
        {
            string sql = "select CompareAttributeID, Name, AttributeGroupID, ShortDescription, AttributeTypeID from Store_Compare_Attributes where CategoryID = " + cid;
            SubSonic.Schema.StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandTimeout = 0;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                string groupid = dr["AttributeGroupID"].ToString();
                int gid = 0;
                int.TryParse(groupid, out gid);
                int titleId = int.Parse(dr["CompareAttributeID"].ToString());
                string title = dr["Name"].ToString();
                string des = dr["ShortDescription"].ToString();
                string typeid = dr["AttributeTypeID"].ToString();
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
                    string temp = GetAttributeGroupById(gid);
                    string[] temps = temp.Split('|');
                    if (temps.Length > 1)
                    {
                        group.AttributeGroupName = temps[0];
                        group.OrderID = int.Parse(temps[1]);
                        group.AttributeGroupList = glistList;
                        groupDic.Add(gid, group);
                    }
                }
            }
            dr.Close();
        }

        private static void GetAllAttributeGroup()
        {
            groupDic = new Dictionary<int, string>();
            string connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            var sql = "select AttributeGroupID, AttributeGroupName, OrderID from dbo.CSK_Store_AttributeGroup";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();

                IDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int aid = int.Parse(dr["AttributeGroupID"].ToString());
                    string aname = dr["AttributeGroupName"].ToString();
                    string orderID = dr["OrderID"].ToString();

                    //Translation
                    if (attGroupTranslations.ContainsKey(aid))
                        aname = attGroupTranslations[aid];

                    groupDic.Add(aid, aname + "|" + orderID);
                }

                dr.Close();
                conn.Close();
            }
            string stringOther = "Other";
            if (attGroupTranslations.ContainsKey(0))
                stringOther = attGroupTranslations[0];

            groupDic.Add(0, stringOther + "|999");
        }

        private static string GetAttributeGroupById(int gid)
        {
            if (groupDic.ContainsKey(gid))
                return groupDic[gid];
            else
                return string.Empty;
        }

        /// <summary>
        /// 所有有Attribute的CategoryId
        /// </summary>
        /// <returns></returns>
        private static List<int> GetAllCategoryAttribute()
        {
            List<int> cidList = new List<int>();
            string sql = "select distinct(CategoryID) from dbo.CSK_Store_Category_AttributeTitle_Map";
            SubSonic.Schema.StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandTimeout = 0;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int cid = int.Parse(dr["CategoryID"].ToString());
                cidList.Add(cid);
            }
            dr.Close();

            sql = "select distinct(CategoryID) from dbo.Store_Compare_Attributes";
            sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandTimeout = 0;
            dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int cid = int.Parse(dr["CategoryID"].ToString());
                if (!cidList.Contains(cid))
                    cidList.Add(cid);
            }
            dr.Close();

            return cidList;
        }

        #endregion
    }
}