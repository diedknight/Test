using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceMeCache;
using PriceMeCommon.Data;
using PriceMeDBA;
using System.Data.SqlClient;

namespace PriceMeCommon.BusinessLogic
{
    public static class AttributesController
    {
        static Dictionary<int, AttributeTitleCache> AttributeTitleDic_Static;
        static Dictionary<int, AttributeValueCache> AttributeValueDic_Static;
        static Dictionary<int, AttributeTitleCache> AttributeValueTitleDic_Static;
        static Dictionary<int, List<AttributeValueCache>> AttributeTitlesValuesDic_Static;
        static List<AttributeValueRangeCache> AttributeValueRangeCacheList_Static;
        static Dictionary<int, AttributeValueRangeCache> AttributeValueRangeDic_Static;
        static Dictionary<string, AttributeValueCache> AttributeTitleIDAndListOrderDic_Static;

        static List<CategoryAttributeTitleMapCache> CategoryAttributeTilteMaps_Static;
        static Dictionary<int, List<AttributeTitleCache>> CategoryAttributeTitleDic_Static;
        static Dictionary<string, CategoryAttributeTitleMapCache> CategoryAttributeTitleMapDic_Static;


        /// <summary>
        /// attribute group的名字本地化
        /// </summary>
        static Dictionary<int, Dictionary<int, string>> MultiCountryAttGroupTranslations_Static;
        /// <summary>
        /// attribute的名字本地化
        /// </summary>
        static Dictionary<int, Dictionary<string, string>> MultiCountryAttNameTranslations_Static;
        /// <summary>
        /// 所有的attribute group名字的翻译
        /// </summary>
        static Dictionary<int, Dictionary<int, string>> MultiCountryAttrGroupNameTranslationsDic_Static;

        static Dictionary<int, Dictionary<int, List<AttributeGroup>>> MultiCountryCategoryAttGroupDic_Static;
        static Dictionary<int, CatalogAttributeGroupInfo> CatalogAttributeGroupInfo_Static;
        static Dictionary<int, int> CatalogAttributeValue_GroupDic_Static;
        static List<AttributeDisplayType> AttributeDisplayType_Static;
        static List<AttributeCategoryComparison> AttributeCategoryComparison_Static;
        static List<ProductDescAndAttr> ProDescAndAttr_Static;

        public static List<AttributeDisplayTypeValue> AllAttributeDisplayTypeValue_Static { get; private set; }
        public static List<AttributeDisplayType> AllAttributeDisplayType_Static { get; private set; }
        public static List<AttributeCategoryComparison> AllAttributeCategoryComparison_Static { get; private set; }

        public static void Load(Timer.DKTimer dkTimer)
        {
            if (dkTimer != null)
            {
                dkTimer.Set("AttributesController.Load() --- Befor SetCategoryInfo()");
            }

            AttributeTitleDic_Static = GetAttributeTitleDicFromDB();

            List<AttributeValueCache> attributeValues = GetAttributeValuesCacheListFromDB();
            AttributeValueDic_Static = attributeValues.ToDictionary(a => a.AttributeValueID, a => a);

            AttributeValueTitleDic_Static = new Dictionary<int, AttributeTitleCache>();
            foreach (AttributeValueCache attributeValue in attributeValues)
            {
                AttributeValueTitleDic_Static.Add(attributeValue.AttributeValueID, GetAttributeTitleByID(attributeValue.AttributeTitleID));
            }
            AttributeTitlesValuesDic_Static = GetAttributeTitlesValuesDic(attributeValues, AttributeTitleDic_Static);

            AttributeValueRangeCacheList_Static = GetAttributeValueRangeCacheListFromDB();
            AttributeValueRangeDic_Static = AttributeValueRangeCacheList_Static.ToDictionary(ar => ar.ValueRangeID, ar => ar);
            AttributeTitleIDAndListOrderDic_Static = GetAttributeTitleIDAndListOrderDic(attributeValues, AttributeValueRangeCacheList_Static);

            CategoryAttributeTilteMaps_Static = GetCategoryAttributeTilteMapsFromDB();
            CategoryAttributeTitleDic_Static = GetCategoryAttributeTitleDic(CategoryAttributeTilteMaps_Static);
            CategoryAttributeTitleMapDic_Static = GetCategoryAttributeTitleMapDic(CategoryAttributeTilteMaps_Static);

            MultiCountryAttGroupTranslations_Static = GetMultiCountryAttGroupTranslations();
            MultiCountryAttNameTranslations_Static = GetMultiCountryAttNameTranslations();
            MultiCountryAttrGroupNameTranslationsDic_Static = GetMultiCountryGroupDic();
            MultiCountryCategoryAttGroupDic_Static = GetMultiCountryAttGroupDic();

            SetCatalogAttributeGroupInfo();
            SetAttributeCategoryComparison();
            GetProductDescAndAttrData();

            AllAttributeDisplayTypeValue_Static = GetAllAttributeDisplayTypeValue();
            AllAttributeDisplayType_Static = GetAllAttributeDisplayType();
            AllAttributeCategoryComparison_Static = GetAllAttributeCategoryComparison();
        }

        public static void LoadForBuildIndex()
        {
            AttributeTitleDic_Static = GetAttributeTitleDicFromDB();

            List<AttributeValueCache> attributeValues = GetAttributeValuesCacheListFromDB();
            AttributeValueDic_Static = attributeValues.ToDictionary(a => a.AttributeValueID, a => a);

            AttributeValueTitleDic_Static = new Dictionary<int, AttributeTitleCache>();
            foreach (AttributeValueCache attributeValue in attributeValues)
            {
                AttributeValueTitleDic_Static.Add(attributeValue.AttributeValueID, GetAttributeTitleByID(attributeValue.AttributeTitleID));
            }
            AttributeTitlesValuesDic_Static = GetAttributeTitlesValuesDic(attributeValues, AttributeTitleDic_Static);

            CategoryAttributeTilteMaps_Static = GetCategoryAttributeTilteMapsFromDB();
            CategoryAttributeTitleDic_Static = GetCategoryAttributeTitleDic(CategoryAttributeTilteMaps_Static);
            CategoryAttributeTitleMapDic_Static = GetCategoryAttributeTitleMapDic(CategoryAttributeTilteMaps_Static);

            AttributeValueRangeCacheList_Static = GetAttributeValueRangeCacheListFromDB();
            AttributeValueRangeDic_Static = AttributeValueRangeCacheList_Static.ToDictionary(ar => ar.ValueRangeID, ar => ar);
            AttributeTitleIDAndListOrderDic_Static = GetAttributeTitleIDAndListOrderDic(attributeValues, AttributeValueRangeCacheList_Static);

            MultiCountryAttGroupTranslations_Static = GetMultiCountryAttGroupTranslations();
            MultiCountryAttNameTranslations_Static = GetMultiCountryAttNameTranslations();
            MultiCountryAttrGroupNameTranslationsDic_Static = GetMultiCountryGroupDic();
        }

        private static List<ProductDescAndAttr> GetAllProductDescAndAttrData()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append(" [SID],");
            sql.Append(" [ProductID],");
            sql.Append(" [T]");
            sql.Append(" FROM");
            sql.Append(" [Product_DescAndAttr]");

            List<ProductDescAndAttr> proDescAndAttrList = new List<ProductDescAndAttr>();
            string connString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;

            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var desc = new ProductDescAndAttr();

                            int t = 0;
                            int proid = 0;
                            int aid = 0;
                            int.TryParse(sqlDR["SID"].ToString(), out aid);
                            int.TryParse(sqlDR["ProductID"].ToString(), out proid);
                            int.TryParse(sqlDR["T"].ToString(), out t);

                            desc.T = t;
                            desc.ProductId = proid;
                            desc.AttributeTypeID = aid;

                            proDescAndAttrList.Add(desc);

                        }
                    }
                }
            }
            return proDescAndAttrList;
        }

        private static List<AttributeCategoryComparison> GetAllAttributeCategoryComparison()
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


            List<AttributeCategoryComparison> attributeCategoryComparisonList = new List<AttributeCategoryComparison>();
            string connString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;

            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            AttributeCategoryComparison adt = new AttributeCategoryComparison();

                            int aid = 0;
                            bool isbetter = false, is_com = false;
                            DateTime modifion = DateTime.Now, createon = DateTime.Now;

                            int.TryParse(sqlDR["Aid"].ToString(), out aid);

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

                            attributeCategoryComparisonList.Add(adt);

                        }
                    }
                }
            }

            return attributeCategoryComparisonList;
        }

        private static List<AttributeDisplayType> GetAllAttributeDisplayType()
        {
            List<AttributeDisplayType> attributeDisplayTypeList = new List<AttributeDisplayType>();

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

            string connString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;

            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            AttributeDisplayType adt = new AttributeDisplayType();

                            int id = 1, aid = 0, tid = 1;
                            bool iscom = false, is_com = false;

                            int.TryParse(sqlDR["ID"].ToString(), out id);
                            int.TryParse(sqlDR["AttributeID"].ToString(), out aid);
                            int.TryParse(sqlDR["TypeID"].ToString(), out tid);

                            bool.TryParse(sqlDR["IsComparison"].ToString(), out iscom);
                            bool.TryParse(sqlDR["IsCompareAttribute"].ToString(), out is_com);

                            adt.ID = id;
                            adt.AttributeID = aid;
                            adt.TypeID = tid;
                            adt.IsComparison = iscom;
                            adt.IsCompareAttribute = is_com;
                            adt.DisplayAdjectiveBetter = sqlDR["DisplayAdjectiveBetter"].ToString();
                            adt.DisplayAdjectiveWorse = sqlDR["DisplayAdjectiveWorse"].ToString();

                            attributeDisplayTypeList.Add(adt);

                        }
                    }
                }
            }

            return attributeDisplayTypeList;
        }

        private static List<AttributeDisplayTypeValue> GetAllAttributeDisplayTypeValue()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append(" ID,");
            sql.Append(" DisplayTypeName");
            sql.Append(" FROM");
            sql.Append(" [AttributeDisplayTypeValue]");

            List<AttributeDisplayTypeValue> attributeDisplayTypeValueList = new List<AttributeDisplayTypeValue>();
            string connString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;

            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var adt = new AttributeDisplayTypeValue();

                            int id = 1;

                            int.TryParse(sqlDR["ID"].ToString(), out id);

                            adt.ID = id;
                            adt.DisplayTypeName = sqlDR["DisplayTypeName"].ToString();

                            attributeDisplayTypeValueList.Add(adt);

                        }
                    }
                }
            }

            return attributeDisplayTypeValueList;
        }

        private static void GetProductDescAndAttrData()
        {
            var sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append(" [SID],");
            sql.Append(" [ProductID],");
            sql.Append(" [T]");
            sql.Append(" FROM");
            sql.Append(" [Product_DescAndAttr]");

            ProDescAndAttr_Static = new List<ProductDescAndAttr>();
            string connString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;

            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var desc = new ProductDescAndAttr();

                            int t = 0;
                            int proid = 0;
                            int aid = 0;
                            int.TryParse(sqlDR["SID"].ToString(), out aid);
                            int.TryParse(sqlDR["ProductID"].ToString(), out proid);
                            int.TryParse(sqlDR["T"].ToString(), out t);

                            desc.T = t;
                            desc.ProductId = proid;
                            desc.AttributeTypeID = aid;

                            ProDescAndAttr_Static.Add(desc);
                        }
                    }
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

            AttributeCategoryComparison_Static = new List<AttributeCategoryComparison>();
            string connString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;

            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            AttributeCategoryComparison adt = new AttributeCategoryComparison();

                            int aid = 0;
                            bool isbetter = false, is_com = false;
                            DateTime modifion = DateTime.Now, createon = DateTime.Now;

                            int.TryParse(sqlDR["Aid"].ToString(), out aid);

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

                            AttributeCategoryComparison_Static.Add(adt);
                        }
                    }
                }
            }
        }

        private static void SetAttributeDisplayType()
        {
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
            AttributeDisplayType_Static = new List<AttributeDisplayType>();
            string connString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;

            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand(sql.ToString(), sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            AttributeDisplayType adt = new AttributeDisplayType();

                            int id = 1, aid = 0, tid = 1;
                            bool iscom = false, is_com = false;

                            int.TryParse(sqlDR["ID"].ToString(), out id);
                            int.TryParse(sqlDR["AttributeID"].ToString(), out aid);
                            int.TryParse(sqlDR["TypeID"].ToString(), out tid);

                            bool.TryParse(sqlDR["IsComparison"].ToString(), out iscom);
                            bool.TryParse(sqlDR["IsCompareAttribute"].ToString(), out is_com);

                            adt.ID = id;
                            adt.AttributeID = aid;
                            adt.TypeID = tid;
                            adt.IsComparison = iscom;
                            adt.IsCompareAttribute = is_com;
                            adt.DisplayAdjectiveBetter = sqlDR["DisplayAdjectiveBetter"].ToString();
                            adt.DisplayAdjectiveWorse = sqlDR["DisplayAdjectiveWorse"].ToString();

                            AttributeDisplayType_Static.Add(adt);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 生成catalog页面attribute value的分组信息
        /// </summary>
        private static void SetCatalogAttributeGroupInfo()
        {
            CatalogAttributeGroupInfo_Static = new Dictionary<int, CatalogAttributeGroupInfo>();
            CatalogAttributeValue_GroupDic_Static = new Dictionary<int, int>();

            string selectSql = "SELECT CAGM.AttributeValueID, CAGM.GroupID, GroupName FROM [CatalogAttributeGroupMap] as [CAGM] left join [CatalogAttributesGroup] as [CAG] on [CAGM].GroupID = [CAG].GroupID";
            string connString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                using (SqlCommand sqlCMD = new SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int valueID = sqlDR.GetInt32(0);
                            int groupID = sqlDR.GetInt32(1);
                            string groupName = sqlDR.GetString(2);

                            CatalogAttributeValue_GroupDic_Static.Add(valueID, groupID);
                            if (CatalogAttributeGroupInfo_Static.ContainsKey(groupID))
                            {
                                CatalogAttributeGroupInfo_Static[groupID].AttributeValues.Add(valueID);
                            }
                            else
                            {
                                CatalogAttributeGroupInfo cagi = new CatalogAttributeGroupInfo();
                                cagi.CatalogAttributeGroupID = groupID;
                                cagi.CatalogAttributeGroupName = groupName;
                                cagi.AttributeValues.Add(valueID);
                                CatalogAttributeGroupInfo_Static.Add(groupID, cagi);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ???
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, Dictionary<int, List<AttributeGroup>>> GetMultiCountryAttGroupDic()
        {
            Dictionary<int, Dictionary<int, List<AttributeGroup>>> multiDic = new Dictionary<int, Dictionary<int, List<AttributeGroup>>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                VelocityController vc = MultiCountryController.GetVelocityController(countryId);
                Dictionary<int, List<AttributeGroup>> attGroupDic = GetAttGroupDic(countryId, vc);
                multiDic.Add(countryId, attGroupDic);
            }

            return multiDic;
        }

        public static Dictionary<int, List<AttributeGroup>> GetAttGroupDic(int countryId, VelocityController vc)
        {
            Dictionary<int, List<AttributeGroup>> attGroupDic = null;

            if (vc != null)
            {
                attGroupDic = vc.GetCache(VelocityCacheKey.AllCategoryAttributeGroup) as Dictionary<int, List<AttributeGroup>>;
            }

            if (attGroupDic == null || attGroupDic.Count == 0)
            {
                List<int> cidList = GetAllCategoryAttribute(countryId);
                attGroupDic = new Dictionary<int, List<AttributeGroup>>();
                foreach (int cid in cidList)
                {
                    Dictionary<int, AttributeGroup> groupDic = new Dictionary<int, AttributeGroup>();
                    GetAllAttributeByCategoryId(cid, groupDic, countryId);
                    GetAllCompareAttributeByCategoryId(cid, groupDic, countryId);
                    List<AttributeGroup> groupList = new List<AttributeGroup>(groupDic.Values);
                    groupList = groupList.OrderBy(g => g.OrderID).ToList();
                    attGroupDic.Add(cid, groupList);
                }

                LogController.WriteLog("CountryId:" + countryId + " AllCategoryAttributeGroup no velocity");
            }

            return attGroupDic;
        }

        private static void GetAllCompareAttributeByCategoryId(int cId, Dictionary<int, AttributeGroup> groupDic, int countryId)
        {
            string sql = "select CompareAttributeID, Name, AttributeGroupID, ShortDescription, AttributeTypeID from Store_Compare_Attributes where CategoryID = " + cId;
            string connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 0;
                    using (var dr = cmd.ExecuteReader())
                    {
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
                                string temp = GetAttributeGroupNameById(gid, countryId);
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
                    }
                }
            }
        }

        private static void GetAllAttributeByCategoryId(int cId, Dictionary<int, AttributeGroup> groupDic, int countryId)
        {
            string sql = "select m.AttributeTitleID, t.Title, t.AttributeGroupID, t.ShortDescription, t.AttributeTypeID from dbo.CSK_Store_Category_AttributeTitle_Map m "
                        + "inner join CSK_Store_ProductDescriptorTitle t on m.AttributeTitleID = t.TypeID where m.CategoryID = " + cId
                        + " order by m.AttributeOrder";
            string connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 0;
                    using (var dr = cmd.ExecuteReader())
                    {
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
                                string temp = GetAttributeGroupNameById(gid, countryId);
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
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        private static string GetAttributeGroupNameById(int gId, int countryId)
        {
            if (MultiCountryAttrGroupNameTranslationsDic_Static.ContainsKey(countryId))
            {
                if (MultiCountryAttrGroupNameTranslationsDic_Static[countryId].ContainsKey(gId))
                    return MultiCountryAttrGroupNameTranslationsDic_Static[countryId][gId];
            }

            return string.Empty;
        }

        /// <summary>
        /// 经和小邹讨论， 可以放到公共数据库，下版本再改
        /// </summary>
        /// <returns></returns>
        private static List<int> GetAllCategoryAttribute(int countryId)
        {
            List<int> cidList = new List<int>();
            //string connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            string connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            string sql1 = "select distinct(CategoryID) from dbo.CSK_Store_Category_AttributeTitle_Map";
            string sql2 = "select distinct(CategoryID) from dbo.Store_Compare_Attributes";

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql1, conn))
                {
                    cmd.CommandTimeout = 0;

                    using (var dr = cmd.ExecuteReader())
                        while (dr.Read())
                        {
                            int cid = dr.GetInt32(0);
                            cidList.Add(cid);
                        }
                }

                using (SqlCommand cmd2 = new SqlCommand(sql2, conn))
                {
                    cmd2.CommandTimeout = 0;
                    using (var dr = cmd2.ExecuteReader())
                        while (dr.Read())
                        {
                            int cid = dr.GetInt32(0);
                            if (!cidList.Contains(cid))
                                cidList.Add(cid);
                        }
                }
            }

            return cidList;
        }

        private static Dictionary<int, Dictionary<int, string>> GetMultiCountryGroupDic()
        {
            Dictionary<int, Dictionary<int, string>> multiDic = new Dictionary<int, Dictionary<int, string>>();

            string connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            var sql = "select AttributeGroupID, AttributeGroupName, OrderID from dbo.CSK_Store_AttributeGroup";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int aid = dr.GetInt32(0);
                        string aname = dr.GetString(1);
                        int orderID = dr.GetInt32(2);

                        foreach (int countryId in MultiCountryController.CountryIdList)
                        {
                            string tName = aname;
                            if (MultiCountryAttGroupTranslations_Static.ContainsKey(countryId))
                            {
                                Dictionary<int, string> countryAttGroupTranslationsDic = MultiCountryAttGroupTranslations_Static[countryId];
                                //Translation
                                if (countryAttGroupTranslationsDic.ContainsKey(aid))
                                    tName = countryAttGroupTranslationsDic[aid];
                            }


                            if (multiDic.ContainsKey(countryId))
                            {
                                Dictionary<int, string> groupDic = multiDic[countryId];
                                groupDic.Add(aid, tName + "|" + orderID);
                            }
                            else
                            {
                                Dictionary<int, string> groupDic = new Dictionary<int, string>();
                                groupDic.Add(aid, tName + "|" + orderID);

                                multiDic.Add(countryId, groupDic);
                            }
                        }
                    }
                }
            }

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, string> groupDic = multiDic[countryId];

                string stringOther = "Other";

                if (MultiCountryAttGroupTranslations_Static.ContainsKey(countryId))
                {
                    Dictionary<int, string> countryAttGroupTranslationsDic = MultiCountryAttGroupTranslations_Static[countryId];
                    //Translation
                    if (countryAttGroupTranslationsDic.ContainsKey(0))
                        stringOther = countryAttGroupTranslationsDic[0];
                }

                groupDic.Add(0, stringOther + "|999");
            }


            return multiDic;
        }

        private static Dictionary<int, Dictionary<string, string>> GetMultiCountryAttNameTranslations()
        {
            Dictionary<int, Dictionary<string, string>> multiDic = new Dictionary<int, Dictionary<string, string>>();

            string sql = string.Format("Select AttributName, AttributNameTranslation, CountryId From dbo.CSK_Store_AttributeTranslation Where CountryId in ({0})", string.Join(",", MultiCountryController.CountryIdList));
            using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        string attName = idr.GetString(0).ToLower();
                        string attTranslation = idr.GetString(1);
                        int countryId = idr.GetInt32(2);

                        if (multiDic.ContainsKey(countryId))
                        {
                            var attGroupTranslations = multiDic[countryId];
                            if (!attGroupTranslations.ContainsKey(attName))
                                attGroupTranslations.Add(attName, attTranslation);
                        }
                        else
                        {
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            dic.Add(attName, attTranslation);
                            multiDic.Add(countryId, dic);
                        }
                    }
                }
            }

            return multiDic;
        }

        private static Dictionary<int, Dictionary<int, string>> GetMultiCountryAttGroupTranslations()
        {
            Dictionary<int, Dictionary<int, string>> multiDic = new Dictionary<int, Dictionary<int, string>>();

            string sql = string.Format("Select AttributeGroupId, AttributeGroupName, CountryId From CSK_Store_AttributeGroupTranslation Where CountryId in ({0})", string.Join(",", MultiCountryController.CountryIdList));
            using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        int agId = idr.GetInt32(0);
                        string attName = idr.GetString(1);
                        int countryId = idr.GetInt32(2);

                        if (multiDic.ContainsKey(countryId))
                        {
                            var attGroupTranslations = multiDic[countryId];
                            if (!attGroupTranslations.ContainsKey(agId))
                                attGroupTranslations.Add(agId, attName);
                        }
                        else
                        {
                            Dictionary<int, string> dic = new Dictionary<int, string>();
                            dic.Add(agId, attName);
                            multiDic.Add(countryId, dic);
                        }
                    }
                }
            }

            return multiDic;
        }

        private static Dictionary<string, CategoryAttributeTitleMapCache> GetCategoryAttributeTitleMapDic(List<CategoryAttributeTitleMapCache> categoryAttributeTilteMaps)
        {
            Dictionary<string, CategoryAttributeTitleMapCache> categoryAttributeTitleMapDictionary = new Dictionary<string, CategoryAttributeTitleMapCache>();

            foreach (CategoryAttributeTitleMapCache categoryAttributeTitleMap in categoryAttributeTilteMaps)
            {
                string key = categoryAttributeTitleMap.CategoryID + "," + categoryAttributeTitleMap.AttributeTitleID;
                if (!categoryAttributeTitleMapDictionary.ContainsKey(key))
                {
                    categoryAttributeTitleMapDictionary.Add(key, categoryAttributeTitleMap);
                }
            }

            return categoryAttributeTitleMapDictionary;
        }

        private static Dictionary<int, List<AttributeTitleCache>> GetCategoryAttributeTitleDic(List<CategoryAttributeTitleMapCache> categoryAttributeTilteMaps)
        {
            Dictionary<int, List<AttributeTitleCache>> dic = new Dictionary<int, List<AttributeTitleCache>>();

            foreach (CategoryAttributeTitleMapCache catm in categoryAttributeTilteMaps)
            {
                if (catm.IsPrimary)
                {
                    AttributeTitleCache atc = GetAttributeTitleByID(catm.AttributeTitleID);
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

        private static Dictionary<int, List<AttributeValueCache>> GetAttributeTitlesValuesDic(List<AttributeValueCache> attributeValues, Dictionary<int, AttributeTitleCache> attributeValueTitleDic)
        {
            Dictionary<int, List<AttributeValueCache>> dic = new Dictionary<int, List<AttributeValueCache>>();

            foreach (int titleId in attributeValueTitleDic.Keys)
            {
                List<AttributeValueCache> list = attributeValues.Where(av => av.AttributeTitleID == titleId).ToList();
                if (list.Count > 0)
                {
                    dic.Add(titleId, list);
                }
            }

            return dic;
        }

        private static List<CategoryAttributeTitleMapCache> GetCategoryAttributeTilteMapsFromDB()
        {
            List<CategoryAttributeTitleMapCache> categoryAttributeTilteMaps = new List<CategoryAttributeTitleMapCache>();
            //可以考虑放到Pam_User
            string selectSql = @"SELECT [MapID]
                              ,[CategoryID]
                              ,[AttributeTitleID]
                              ,[IsPrimary]
                              ,[AttributeOrder]
                              ,[IsSlider]
                              ,[Step]
                              ,[Step2]
                              ,[MinValue]
                              ,[MaxValue]
                               ,[Step3] FROM [CSK_Store_Category_AttributeTitle_Map]";

            string connString = MultiCountryController.GetDBConnectionString(MultiCountryController.CountryIdList[0]);
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand sqlCMD = new SqlCommand(selectSql, sqlConn))
                {
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
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

            return categoryAttributeTilteMaps;
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

        private static List<AttributeValueCache> GetAttributeValuesCacheListFromDB()
        {
            List<CSK_Store_AttributeValue> attributeValuesDB = CSK_Store_AttributeValue.All().OrderBy(av => av.ListOrder).ToList();
            List<AttributeValueCache> attributeValues = ConvertController<AttributeValueCache, CSK_Store_AttributeValue>.ConvertData(attributeValuesDB);
            return attributeValues;
        }

        private static List<AttributeValueRangeCache> GetAttributeValueRangeCacheListFromDB()
        {
            List<CSK_Store_AttributeValueRange> attributeValueRanges = CSK_Store_AttributeValueRange.All().ToList();
            List<AttributeValueRangeCache> attributeRanges = ConvertController<AttributeValueRangeCache, CSK_Store_AttributeValueRange>.ConvertData(attributeValueRanges);
            return attributeRanges;
        }

        private static Dictionary<int, AttributeTitleCache> GetAttributeTitleDicFromDB()
        {
            //可能出现内存问题
            List<AttributeTitleCache> attributesTitles = ConvertController<AttributeTitleCache, CSK_Store_ProductDescriptorTitle>.ConvertData(CSK_Store_ProductDescriptorTitle.All().ToList());

            Dictionary<int, AttributeTitleCache> attributesTitleDic = attributesTitles.ToDictionary(a => a.TypeID, a => a);

            return attributesTitleDic;
        }

        private static T GetFromAllVelocity<T>(VelocityCacheKey velocityCacheKey) where T : class
        {
            T result = null;

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                VelocityController vc = MultiCountryController.GetVelocityController(countryId);
                if(vc != null)
                {
                    result = vc.GetCache<T>(velocityCacheKey);
                    if (result != null)
                        return result;
                }
            }

            return result;
        }

        public static List<AttributeValueRangeCache> GetAttributeValueRangesByTitleIDAndCategoryID(int attributeTitleID, int categoryID)
        {
            List<AttributeValueRangeCache> attributeValueRangeCollection = new List<AttributeValueRangeCache>();
            foreach (AttributeValueRangeCache avr in AttributeValueRangeCacheList_Static)
            {
                if (avr.AttributeTitleID == attributeTitleID && avr.CategoryID == categoryID)
                {
                    attributeValueRangeCollection.Add(avr);
                }
            }
            return attributeValueRangeCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeTitleID"></param>
        /// <returns></returns>
        public static AttributeTitleCache GetAttributeTitleByID(int attributeTitleID)
        {
            if (AttributeTitleDic_Static.ContainsKey(attributeTitleID))
            {
                return AttributeTitleDic_Static[attributeTitleID];
            }
            return null;
        }

        /// <summary>
        /// 根据catalog页面的AttributeGroupID获取对应的AttributeValues
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static List<int> GetCatalogAttributeValues(int groupID)
        {
            if (CatalogAttributeGroupInfo_Static.ContainsKey(groupID))
            {
                return CatalogAttributeGroupInfo_Static[groupID].AttributeValues;
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
            if (CatalogAttributeValue_GroupDic_Static.ContainsKey(attributeValueID))
            {
                return CatalogAttributeValue_GroupDic_Static[attributeValueID];
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
            if (CatalogAttributeGroupInfo_Static.ContainsKey(groupID))
            {
                return CatalogAttributeGroupInfo_Static[groupID].CatalogAttributeGroupName;
            }
            return null;
        }

        /// <summary>
        /// key的格式为CategoryID + "," + AttributeTitleID
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static CategoryAttributeTitleMapCache GetCategoryAttributeTitleMapByKey(string key)
        {
            if (CategoryAttributeTitleMapDic_Static.ContainsKey(key))
            {
                return CategoryAttributeTitleMapDic_Static[key];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeValueID"></param>
        /// <returns></returns>
        public static AttributeValueCache GetAttributeValueByID(int attributeValueID)
        {
            if (AttributeValueDic_Static.ContainsKey(attributeValueID))
            {
                return AttributeValueDic_Static[attributeValueID];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeValueID"></param>
        /// <returns></returns>
        public static AttributeTitleCache GetAttributeTitleByVauleID(int attributeValueID)
        {
            if (AttributeValueTitleDic_Static.ContainsKey(attributeValueID))
            {
                return AttributeValueTitleDic_Static[attributeValueID];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static List<AttributeTitleCache> GetAttributesTitleByCategoryID(int categoryID)
        {
            if (CategoryAttributeTitleDic_Static.ContainsKey(categoryID))
            {
                return CategoryAttributeTitleDic_Static[categoryID];
            }
            return new List<AttributeTitleCache>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeValueRangeID"></param>
        /// <returns></returns>
        public static AttributeValueRangeCache GetAttributeValueRangeByID(int attributeValueRangeID)
        {
            if (AttributeValueRangeDic_Static.ContainsKey(attributeValueRangeID))
            {
                return AttributeValueRangeDic_Static[attributeValueRangeID];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeTitleID"></param>
        /// <returns></returns>
        public static List<AttributeValueCache> GetAttributeValuesByTitleID(int attributeTitleID)
        {
            if (AttributeTitlesValuesDic_Static.ContainsKey(attributeTitleID))
            {
                return AttributeTitlesValuesDic_Static[attributeTitleID];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeValueRangeID"></param>
        /// <returns></returns>
        public static List<AttributeValueCache> GetAttributeValuesByValueRangeID(int attributeValueRangeID)
        {
            List<AttributeValueCache> attributeValueCollection = new List<AttributeValueCache>();
            int minValue = 0;
            int maxValue = 0;
            int attributeTitleID = 0;
            foreach (AttributeValueRangeCache avr in AttributeValueRangeCacheList_Static)
            {
                if (avr.ValueRangeID == attributeValueRangeID)
                {
                    minValue = avr.MinValue;
                    maxValue = avr.MaxValue;
                    attributeTitleID = avr.AttributeTitleID;
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

            List<AttributeValueCache> attributeValues = GetAttributeValuesByTitleID(attributeTitleID);
            if (attributeValues != null)
            {
                foreach (AttributeValueCache av in attributeValues)
                {
                    if (av.AttributeTitleID == attributeTitleID && av.ListOrder >= minValue && av.ListOrder <= maxValue)
                    {
                        attributeValueCollection.Add(av);
                    }
                }
            }

            return attributeValueCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeTitleID"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static bool HasAttributeRange(int attributeTitleID, int categoryID)
        {
            foreach (AttributeValueRangeCache avr in AttributeValueRangeCacheList_Static)
            {
                if (avr.AttributeTitleID == attributeTitleID && avr.CategoryID == categoryID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avr"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
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
                List<AttributeValueCache> attributeValues = GetAttributeValuesByTitleID(avr.AttributeTitleID);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeTitleID"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static NarrowByInfo GetAttributesResulteList(int attributeTitleID, int categoryID)
        {
            AttributeTitleCache attributeTitle = GetAttributeTitleByID(attributeTitleID);
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
                List<AttributeValueRangeCache> attributeValueRangeCollection = GetAttributeValueRangesByTitleIDAndCategoryID(attributeTitleID, categoryID);
                foreach (AttributeValueRangeCache avr in attributeValueRangeCollection)
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
                List<AttributeValueCache> attributeValueCollection = GetAttributeValuesByTitleID(attributeTitleID);
                if (attributeValueCollection != null)
                {
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arId"></param>
        /// <returns></returns>
        public static AttributeTitleCache GetAttributeTitleByAttributeRangeID(int arId)
        {
            AttributeValueRangeCache attributeValueRange = AttributeValueRangeCacheList_Static.SingleOrDefault(avr => avr.ValueRangeID == arId);

            if (attributeValueRange != null)
            {
                return GetAttributeTitleByID(attributeValueRange.AttributeTitleID);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<AttributeGroup> GetAttributeGroupByCategoryId(int cId, int countryId)
        {
            if(MultiCountryCategoryAttGroupDic_Static.ContainsKey(countryId))
            {
                if(MultiCountryCategoryAttGroupDic_Static[countryId].ContainsKey(cId))
                {
                    return MultiCountryCategoryAttGroupDic_Static[countryId][cId];
                }
            }

            return null;
        }

        public static string GetAttNameTranslations(string name, int countryId)
        {
            string lowerName = name.ToLower();

            if(MultiCountryAttNameTranslations_Static.ContainsKey(countryId) && MultiCountryAttNameTranslations_Static[countryId].ContainsKey(lowerName))
            {
                return MultiCountryAttNameTranslations_Static[countryId][lowerName];
            }

            return null;
        }

        public static Dictionary<int, Dictionary<string, int>> GetProductsAttributes(List<int> attributeTypes, List<string> productIDs, int countryId)
        {
            Dictionary<int, Dictionary<string, int>> dic = new Dictionary<int, Dictionary<string, int>>();
            string selectSql = @"SELECT 
                                [ProductID]
                                ,[TypeID]
                                ,[AttributeValueID]
                                FROM [dbo].[CSK_Store_ProductDescriptor]
                                where TypeID in (" + string.Join(",", attributeTypes) + ") and productid in (" + string.Join(",", productIDs) + ")";
            string connString = MultiCountryController.GetDBConnectionString(countryId);
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                using (SqlCommand sqlCMD = new SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
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
                                PriceMeCommon.BusinessLogic.LogController.WriteException("GetProductsAttributes() " + ex.Message);
                            }
                        }
                    }
                }
            }

            return dic;
        }
    }
}