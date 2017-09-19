using PriceMeCache;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class CategoryController
    {
        /// <summary>
        /// 需要做301的分类，全球通用
        /// </summary>
        public static Dictionary<int, int> Category301Dictionary_Static { get; private set; }

        /// <summary>
        /// 用于显示根分类菜单
        /// </summary>
        static Dictionary<int, List<CategoryCache>> MultiCountryCategoriesMenuListDic_Static;

        /// <summary>
        /// Catalog页面不需要显示Brands的分类
        /// </summary>
        static Dictionary<int, List<int>> MultiHiddenBrandsCategoryIdDic_Static;
        /// <summary>
        /// 保存多国家的根分类信息
        /// </summary>
        static Dictionary<int, List<CategoryCache>> AllRootCategoriesOrderByName_Static;
        /// <summary>
        /// 分类的在catalog页面的默认view类型
        /// </summary>
        static Dictionary<int, Dictionary<int, int>> CategoryDefaultViewDic_Static;
        /// <summary>
        /// 保存多国家的分类的同义词信息
        /// </summary>
        static Dictionary<int, Dictionary<int, List<CategoryExtend>>> MultiCountryCategorySynonymDic_Static;
        /// <summary>
        /// 保存多国家的分类信息
        /// </summary>
        static Dictionary<int, Dictionary<int, CategoryCache>> MultiCountryCategoryOrderByNameDic_Static;
        static Dictionary<int, List<CategoryCache>> MultiCountryCategoryOrderByNameList_Static;
        /// <summary>
        /// 保存多国家的MoneyPlans链接信息
        /// ShortDescription用于保存Url
        /// </summary>
        static Dictionary<int, List<CategoryCache>> MultiCountryMoneyPlansList_Static;
        /// <summary>
        /// 保存多国家的分类的下一级分类信息
        /// </summary>
        static Dictionary<int, Dictionary<int, List<CategoryCache>>> MultiCountryNextLevelActiveCategories_Static;
        static Dictionary<int, List<int>> MultiCountryCategoryIsSearchOnlyDic_Static;

        static Dictionary<int, List<CategoryCache>> MultiCountrySiteMapRootCategoriesList_Static;

        static Dictionary<int, Dictionary<int, Dictionary<int, bool>>> MultiCountryCategoryShortCutsDic_Static;
        static Dictionary<int, List<int>> MultiCountryCategoryWuDic_Static;
        static Dictionary<int, List<CategoryFilterData>> MultiCountryCategoryFilterDataDic_Static;
        static Dictionary<int, List<CategoryCache>> MultiCountryReviewCategoriesDic_Static;

        public static void Load(Timer.DKTimer dkTimer)
        {
            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor SetCategoryInfo()");
            }
            SetCategoryInfo();

            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor SetCategoryExtendInfo()");
            }
            SetCategoryExtendInfo();

            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor SetCategoryOtherInfo()");
            }
            SetCategoryOtherInfo();
        }

        public static void LoadForBuildIndex()
        {
            MultiCountryCategoryOrderByNameDic_Static = new Dictionary<int, Dictionary<int, CategoryCache>>();
            MultiCountryCategoryIsSearchOnlyDic_Static = new Dictionary<int, List<int>>();
            MultiHiddenBrandsCategoryIdDic_Static = new Dictionary<int, List<int>>();
            CategoryDefaultViewDic_Static = new Dictionary<int, Dictionary<int, int>>();
            AllRootCategoriesOrderByName_Static = new Dictionary<int, List<CategoryCache>>();
            MultiCountryCategoriesMenuListDic_Static = new Dictionary<int, List<CategoryCache>>();
            MultiCountryNextLevelActiveCategories_Static = new Dictionary<int, Dictionary<int, List<CategoryCache>>>();
            MultiCountryCategoryWuDic_Static = new Dictionary<int, List<int>>();
            MultiCountryCategoryShortCutsDic_Static = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                List<CategoryCache> categoryOrderByName = GetCatOrderByNameOrderByName(countryId);

                MultiCountryCategoryIsSearchOnlyDic_Static.Add(countryId, categoryOrderByName.FindAll(c => c.IsSearchOnly).Select(c => c.CategoryID).ToList());

                Dictionary<int, CategoryCache> ccDic = categoryOrderByName.ToDictionary(c => c.CategoryID, c => c);
                MultiCountryCategoryOrderByNameDic_Static.Add(countryId, ccDic);
                MultiHiddenBrandsCategoryIdDic_Static.Add(countryId, categoryOrderByName.Where(cat => !cat.IsFilterByBrand).Select(cat => cat.CategoryID).ToList());

                Dictionary<int, int> categoryDefaultViewDic = categoryOrderByName.ToDictionary(c => c.CategoryID, c => c.CategoryViewType);
                CategoryDefaultViewDic_Static.Add(countryId, categoryDefaultViewDic);
                MultiCountryCategoryWuDic_Static.Add(countryId, categoryOrderByName.Where(c => c.WeightUnit == false).Select(c => c.CategoryID).ToList());

                //
                var allRootCategoriesOrderByName = categoryOrderByName.FindAll(cat => cat.ParentID == 0).OrderBy(cat => cat.CategoryName).ToList();
                AllRootCategoriesOrderByName_Static.Add(countryId, allRootCategoriesOrderByName);

                //
                List<CategoryCache> allPopularParentCategoriesOrderByName = categoryOrderByName.FindAll(cat => cat.ParentID == 0 && cat.IsActive && cat.PopularCategory == true).ToList();
                //List<CategoryCache> categoriesManuList = new List<CategoryCache>(allPopularParentCategoriesOrderByName);
                //if (MultiCountryController.HasFinanceSite(countryId))
                //{
                //    CategoryCache cc = new CategoryCache();
                //    cc.CategoryID = -99;
                //    cc.CategoryName = "Money & Plans";
                //    cc.PopularCategory = true;
                //    categoriesManuList.Add(cc);
                //    categoriesManuList = categoriesManuList.OrderBy(c => c.CategoryName).ToList();
                //}
                //MultiCountryCategoriesMenuListDic_Static.Add(countryId, categoriesManuList);

                Dictionary<int, List<CategoryCache>> nextLevelCategories = new Dictionary<int, List<CategoryCache>>();
                foreach (var c in categoryOrderByName)
                {
                    List<CategoryCache> categories = categoryOrderByName.Where(cat => cat.ParentID == c.CategoryID).ToList();
                    nextLevelCategories.Add(c.CategoryID, categories);
                }
                MultiCountryNextLevelActiveCategories_Static.Add(countryId, nextLevelCategories);

                MultiCountryCategoryShortCutsDic_Static.Add(countryId, GetCategoryShortCutsDictionary(countryId));
            }

            SetCategoryExtendInfo();
        }

        private static List<CategoryCache> GetCatOrderByNameOrderByName(int countryId)
        {
            List<CategoryCache> categoryOrderByName = new List<CategoryCache>();
            string selectSql = "select CT.*,LCT.CategoryName as LocalName,LCT.Description as LocalDesc from CSK_Store_Category as CT left join Local_CategoryName as LCT on CT.CategoryID = LCT.CategoryID and LCT.CountryID = " + countryId + " where CT.IsActive = 1 order by CT.CategoryName";
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
                            categoryCache.ShortDescription = sqlDR["ShortDescription"].ToString();
                            categoryCache.TopBrands = sqlDR["TopBrands"].ToString();
                            categoryCache.Categoryicon = sqlDR["categoryicon"].ToString();
                            categoryCache.CategoryIconCode = sqlDR["iconcode"].ToString();
                            categoryCache.IconUrl = sqlDR["categoryicon"].ToString();
                            categoryCache.IsSearchOnly = bool.Parse(sqlDR["IsSearchOnly"].ToString());
                            categoryCache.CategoryViewType = int.Parse(sqlDR["CategoryViewType"].ToString());
                            categoryCache.WeightUnit = bool.Parse(sqlDR["WeightUnit"].ToString());

                            categoryOrderByName.Add(categoryCache);
                        }
                    }
                }
            }

            return categoryOrderByName;
        }

        private static void SetCategoryOtherInfo()
        {
            MultiCountryCategoryFilterDataDic_Static = new Dictionary<int, List<CategoryFilterData>>();
            MultiCountryReviewCategoriesDic_Static = new Dictionary<int, List<CategoryCache>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                List<CategoryFilterData> categoryFilters = GetCategoryFilter(countryId);
                MultiCountryCategoryFilterDataDic_Static.Add(countryId, categoryFilters);

                List<CategoryCache> reviewCategories = GetReviewCategories(countryId);
                MultiCountryReviewCategoriesDic_Static.Add(countryId, reviewCategories);
            }
        }

        static void SetCategoryInfo()
        {
            MultiCountryCategoryOrderByNameDic_Static = new Dictionary<int, Dictionary<int, CategoryCache>>();
            MultiCountryCategoryOrderByNameList_Static = new Dictionary<int, List<CategoryCache>>();
            MultiCountryCategoryIsSearchOnlyDic_Static = new Dictionary<int, List<int>>();
            MultiHiddenBrandsCategoryIdDic_Static = new Dictionary<int, List<int>>();
            CategoryDefaultViewDic_Static = new Dictionary<int, Dictionary<int, int>>();
            AllRootCategoriesOrderByName_Static = new Dictionary<int, List<CategoryCache>>();
            MultiCountryCategoriesMenuListDic_Static = new Dictionary<int, List<CategoryCache>>();
            MultiCountryNextLevelActiveCategories_Static = new Dictionary<int, Dictionary<int, List<CategoryCache>>>();
            MultiCountryCategoryWuDic_Static = new Dictionary<int, List<int>>();
            MultiCountryCategoryShortCutsDic_Static = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();
            MultiCountrySiteMapRootCategoriesList_Static = new Dictionary<int, List<CategoryCache>>();
            MultiCountryMoneyPlansList_Static = new Dictionary<int, List<CategoryCache>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                VelocityController vc = MultiCountryController.GetVelocityController(countryId);

                List<CategoryCache> categoryOrderByName = GetCatOrderByNameOrderByNameWithClicks(countryId, vc);
                MultiCountryCategoryOrderByNameList_Static.Add(countryId, categoryOrderByName);
                MultiCountryCategoryIsSearchOnlyDic_Static.Add(countryId, categoryOrderByName.FindAll(c => c.IsSearchOnly).Select(c => c.CategoryID).ToList());

                Dictionary<int, CategoryCache> ccDic = categoryOrderByName.ToDictionary(c => c.CategoryID, c => c);
                MultiCountryCategoryOrderByNameDic_Static.Add(countryId, ccDic);
                MultiHiddenBrandsCategoryIdDic_Static.Add(countryId, categoryOrderByName.Where(cat => !cat.IsFilterByBrand).Select(cat => cat.CategoryID).ToList());

                Dictionary<int, int> categoryDefaultViewDic = categoryOrderByName.ToDictionary(c => c.CategoryID, c => c.CategoryViewType);
                CategoryDefaultViewDic_Static.Add(countryId, categoryDefaultViewDic);
                MultiCountryCategoryWuDic_Static.Add(countryId, categoryOrderByName.Where(c => c.WeightUnit == false).Select(c => c.CategoryID).ToList());

                //
                var allRootCategoriesOrderByName = categoryOrderByName.FindAll(cat => cat.ParentID == 0).OrderBy(cat => cat.CategoryName).ToList();
                AllRootCategoriesOrderByName_Static.Add(countryId, allRootCategoriesOrderByName);

                //
                List<CategoryCache> allPopularParentCategoriesOrderByName = categoryOrderByName.FindAll(cat => cat.ParentID == 0 && cat.IsActive && cat.PopularCategory == true).ToList();
                List<CategoryCache> categoriesManuList = new List<CategoryCache>(allPopularParentCategoriesOrderByName);
                if (MultiCountryController.HasFinanceSite(countryId))
                {
                    CategoryCache cc = new CategoryCache();
                    cc.CategoryID = -99;
                    cc.CategoryName = "Money & Plans";
                    cc.PopularCategory = true;
                    categoriesManuList.Add(cc);
                    categoriesManuList = categoriesManuList.OrderBy(c => c.CategoryName).ToList();
                    List<CategoryCache> plansList = GetPlansInfo(countryId);
                    if (plansList.Count > 0)
                    {
                        MultiCountryMoneyPlansList_Static.Add(countryId, plansList);
                    }
                }
                MultiCountryCategoriesMenuListDic_Static.Add(countryId, categoriesManuList);

                List<CategoryCache> siteMapRootCategoriesList = categoryOrderByName.FindAll(cat => cat.ParentID == 0 && cat.IsActive).ToList();

                Dictionary<int, List<CategoryCache>> nextLevelCategories = new Dictionary<int, List<CategoryCache>>();
                foreach (var c in categoryOrderByName)
                {
                    List<CategoryCache> categories = categoryOrderByName.Where(cat => cat.ParentID == c.CategoryID).ToList();
                    nextLevelCategories.Add(c.CategoryID, categories);
                }
                MultiCountryNextLevelActiveCategories_Static.Add(countryId, nextLevelCategories);

                MultiCountryCategoryShortCutsDic_Static.Add(countryId, GetCategoryShortCutsDictionary(countryId));
            }
        }

        private static List<CategoryCache> GetPlansInfo(int countryId)
        {
            List<CategoryCache> list = new List<CategoryCache>();
            string sql = "SELECT [Name],[url],[IconUrl] FROM [MoneyPlan]";
            string commonConnString = MultiCountryController.GetDBConnectionString(countryId);
            using (SqlConnection commonSqlConn = new SqlConnection(commonConnString))
            {
                commonSqlConn.Open();

                using (SqlCommand sqlCMD = new SqlCommand(sql, commonSqlConn))
                {
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            CategoryCache cc = new CategoryCache();
                            cc.CategoryName = sqlDR.GetString(0);
                            cc.ShortDescription = sqlDR.GetString(1);
                            cc.CategoryIconCode = sqlDR.GetString(2);
                            list.Add(cc);
                        }
                    }
                }
            }

            return list;
        }

        static List<CategoryCache> RemoveNoProductCategoryAndSetClicks(List<CategoryCache> categoryOrderByName, Dictionary<int, int> categoryProductCountDic)
        {
            if (categoryProductCountDic.Count > 0)
            {
                for (int i = 0; i < categoryOrderByName.Count;)
                {
                    if (!categoryProductCountDic.ContainsKey(categoryOrderByName[i].CategoryID))
                    {
                        categoryOrderByName.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return categoryOrderByName;
        }

        public static List<CategoryCache> GetCatOrderByNameOrderByNameWithClicks(int countryId, VelocityController vc)
        {
            List<CategoryCache> categoryOrderByName = null;

            if (vc != null)
            {
                categoryOrderByName = vc.GetCache<List<CategoryCache>>(VelocityCacheKey.CategoryByNameWithClicks);
            }

            if (categoryOrderByName == null || categoryOrderByName.Count == 0)
            {
                categoryOrderByName = GetCatOrderByNameOrderByName(countryId);

                Dictionary<int, int> categoryProductClickCount = new Dictionary<int, int>();
                //去掉没有产品的分类以及获取分类的点击数量和产品数量
                foreach (CategoryCache category in categoryOrderByName)
                {
                    try
                    {
                        List<int> categoryIdList = new List<int>();
                        categoryIdList.Add(category.CategoryID);
                        HitsInfo hitsInfo = SearchController.SearchProducts("", categoryIdList, null, null, null, null, "", null, SearchController.MaxSearchCount_Static, countryId, false, true, false, null, false, null, "");
                        if (hitsInfo != null && hitsInfo.ResultCount > 0)
                        {
                            int totalClicks = 0;
                            for (int i = 0; i < hitsInfo.ResultCount; i++)
                            {
                                var doc = hitsInfo.GetDocument(i, new string[] { "Clicks" });
                                string clicks = doc.Get("Clicks");
                                totalClicks += int.Parse(clicks);
                            }
                            categoryProductClickCount.Add(category.CategoryID, totalClicks);
                            category.ProductsCount = hitsInfo.ResultCount;
                        }
                    }
                    catch(Exception ex)
                    {
                        LogController.WriteException("Country: " + countryId + " ex:" + ex.Message + ex.StackTrace);
                    }
                }

                for (int i = 0; i < categoryOrderByName.Count;)
                {
                    if (categoryOrderByName[i].ProductsCount == 0)
                    {
                        categoryOrderByName.RemoveAt(i);
                    }
                    else 
                    {
                        if (categoryProductClickCount.ContainsKey(categoryOrderByName[i].CategoryID))
                        {
                            categoryOrderByName[i].Clicks = categoryProductClickCount[categoryOrderByName[i].CategoryID];
                        }
                        i++;
                    }
                }

                LogController.WriteLog("Country: " + countryId + " CatOrderByNameOrderByNameWithClicks no velocity");
            }

            return categoryOrderByName;
        }

        static void SetCategoryExtendInfo()
        {
            Category301Dictionary_Static = new Dictionary<int, int>();
            string selectCategory301Sql = "select FromCategoryid, ToCategoryid from [Category301Redirect]";
            string commonConnString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            using (SqlConnection commonSqlConn = new SqlConnection(commonConnString))
            {
                commonSqlConn.Open();

                using (SqlCommand sqlCMD = new SqlCommand(selectCategory301Sql, commonSqlConn))
                {
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int fcid = int.Parse(sqlDR["FromCategoryid"].ToString());
                            int tcid = int.Parse(sqlDR["ToCategoryid"].ToString());
                            Category301Dictionary_Static.Add(fcid, tcid);
                        }
                    }
                }

                MultiCountryCategorySynonymDic_Static = new Dictionary<int, Dictionary<int, List<CategoryExtend>>>();
                string selectCategorySynonymSql = "SELECT CategoryID,[Synonym],LocalName, CountryID FROM [dbo].[CategorySynonym_New]";
                using (SqlCommand sqlCMD = new SqlCommand(selectCategorySynonymSql, commonSqlConn))
                {
                    using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int cid = sqlDR.GetInt32(0);
                            string synonym = sqlDR.GetString(1);
                            string localName = sqlDR.GetString(2);
                            int countryId = sqlDR.GetInt32(3);
                            CategoryExtend categorySynonym = new CategoryExtend();
                            categorySynonym.CategoryID = cid;
                            categorySynonym.Synonym = synonym;
                            categorySynonym.LocalName = localName;
                            categorySynonym.CountryID = countryId;

                            if (MultiCountryCategorySynonymDic_Static.ContainsKey(countryId))
                            {
                                Dictionary<int, List<CategoryExtend>> dic = MultiCountryCategorySynonymDic_Static[countryId];
                                if (dic.ContainsKey(cid))
                                {
                                    dic[cid].Add(categorySynonym);
                                }
                                else
                                {
                                    List<CategoryExtend> list = new List<CategoryExtend>();
                                    list.Add(categorySynonym);
                                    dic.Add(cid, list);
                                }
                            }
                            else
                            {
                                if (MultiCountryController.IsExistCountry(countryId))
                                {
                                    Dictionary<int, List<CategoryExtend>> dic = new Dictionary<int, List<CategoryExtend>>();

                                    List<CategoryExtend> list = new List<CategoryExtend>();
                                    list.Add(categorySynonym);
                                    dic.Add(cid, list);

                                    MultiCountryCategorySynonymDic_Static.Add(countryId, dic);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static CategoryCache GetCategoryCacheFromDB(int categoryID, int countryId)
        {
            PriceMeDBA.CSK_Store_Category category = null;

            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                category = PriceMeDBA.CSK_Store_Category.SingleOrDefault(cat => cat.CategoryID == categoryID);

                if (category != null)
                {
                    CategoryCache cc = ConvertController<CategoryCache, PriceMeDBA.CSK_Store_Category>.ConvertData(category);
                    cc.CategoryNameEN = cc.CategoryName;

                    return cc;
                }
                else
                {
                    if (Category301Dictionary_Static.ContainsKey(categoryID))
                    {
                        int _301Cid = Category301Dictionary_Static[categoryID];
                        category = PriceMeDBA.CSK_Store_Category.SingleOrDefault(cat => cat.CategoryID == _301Cid);

                        if (category != null)
                        {
                            CategoryCache cc = ConvertController<CategoryCache, PriceMeDBA.CSK_Store_Category>.ConvertData(category);
                            cc.CategoryNameEN = cc.CategoryName;

                            return cc;
                        }
                    }
                }
            }

            return null;
        }

        private static List<CategoryCache> GetReviewCategories(int countryId)
        {
            List<CategoryCache> reviewCategoryList = new List<CategoryCache>();
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("GetReviewsCatgory");
                sp.Command.CommandTimeout = 0;
                using (System.Data.IDataReader dr = sp.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        CategoryCache c = new CategoryCache();
                        c.CategoryID = int.Parse(dr["CategoryID"].ToString());
                        c.CategoryName = dr["CategoryName"].ToString();
                        c.ParentID = int.Parse(dr["ParentID"].ToString());

                        reviewCategoryList.Add(c);
                    }

                }
            }

            return reviewCategoryList;
        }

        private static Dictionary<int, Dictionary<int, bool>> GetCategoryShortCutsDictionary(int countryId)
        {
            Dictionary<int, Dictionary<int, bool>> dictionary = new Dictionary<int, Dictionary<int, bool>>();
            string selectSql = "select * from ShortCutCategory_Map";
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
                            int cid = int.Parse(sqlDR["CategoryID"].ToString());
                            int scID = int.Parse(sqlDR["ShortCutCategoryID"].ToString());
                            bool isPopular = bool.Parse(sqlDR["IsPopular"].ToString());
                            if (dictionary.ContainsKey(cid))
                            {
                                dictionary[cid].Add(scID, isPopular);
                            }
                            else
                            {
                                Dictionary<int, bool> scIDDictionary = new Dictionary<int, bool>();
                                scIDDictionary.Add(scID, isPopular);
                                dictionary.Add(cid, scIDDictionary);
                            }
                        }
                    }
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Catalog页面的自定义链接
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        private static List<CategoryFilterData> GetCategoryFilter(int countryId)
        {
            var listCategoryFilter = new List<CategoryFilterData>();
            string selectSql = "Select * From CategoryFilter Where CountryId = " + countryId;
            string connString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                using (SqlCommand sqlCMD = new SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (SqlDataReader dr = sqlCMD.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int id = 0, cid = 0, countryid = 0;
                            int.TryParse(dr["Id"].ToString(), out id);
                            int.TryParse(dr["CategoryID"].ToString(), out cid);
                            int.TryParse(dr["CountryId"].ToString(), out countryid);
                            string name = dr["FilterName"].ToString();
                            string url = dr["FilterUrl"].ToString();
                            CategoryFilterData data = new CategoryFilterData();
                            data.Id = id;
                            data.CategoryId = cid;
                            data.FilterName = name;
                            data.FilterUrl = url;
                            data.CountryId = countryid;
                            listCategoryFilter.Add(data);
                        }
                        dr.Close();
                    }
                }
            }

            return listCategoryFilter;
        }

        /// <summary>
        /// 获取Finance的小类
        /// </summary>
        /// <returns></returns>
        public static List<CategoryCache> GetAllFinanceCategory()
        {
            List<CategoryCache> ccList = new List<CategoryCache>();
            CategoryCache cc = new CategoryCache();
            cc.CategoryName = "Credit Cards";
            cc.CategoryID = -991;
            ccList.Add(cc);

            cc = new CategoryCache();
            cc.CategoryName = "Home Loans";
            cc.CategoryID = -992;
            ccList.Add(cc);

            cc = new CategoryCache();
            cc.CategoryName = "Kiwisaver";
            cc.CategoryID = -993;
            ccList.Add(cc);

            cc = new CategoryCache();
            cc.CategoryName = "Savings Accounts";
            cc.CategoryID = -994;
            ccList.Add(cc);

            cc = new CategoryCache();
            cc.CategoryName = "Term Deposits";
            cc.CategoryID = -995;
            ccList.Add(cc);

            return ccList;
        }

        public static CategoryCache GetCategoryFromCache(int categoryID, int countryId)
        {
            CategoryCache category = null;
            if (MultiCountryCategoryOrderByNameDic_Static.ContainsKey(countryId))
            {
                var categoryCache = MultiCountryCategoryOrderByNameDic_Static[countryId];
                if (categoryCache != null && categoryCache.ContainsKey(categoryID))
                {
                    category = categoryCache[categoryID];
                }
            }

            return category;
        }

        public static CategoryCache GetCategoryByCategoryID(int categoryID, int countryId)
        {
            CategoryCache category = null;
            if (MultiCountryCategoryOrderByNameDic_Static.ContainsKey(countryId))
            {
                var categoryCache = MultiCountryCategoryOrderByNameDic_Static[countryId];
                if (categoryCache != null && categoryCache.ContainsKey(categoryID))
                {
                    category = categoryCache[categoryID];
                }
                else
                {
                    category = GetCategoryCacheFromDB(categoryID, countryId);
                }
            }

            return category;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static CategoryCache GetRootCategory(int categoryId, int countryId)
        {
            int rootId = SearchController.GetRootCategoryId(categoryId);
            if(rootId > 0)
            {
                return GetCategoryByCategoryID(rootId, countryId);
            }
            else
            {
                return GetCategoryByCategoryID(categoryId, countryId);
            }
        }

        /// <summary>
        /// 获取下一级分类
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<CategoryCache> GetNextLevelSubCategories(int cid, int countryId)
        {
            if (MultiCountryNextLevelActiveCategories_Static.ContainsKey(countryId))
            {
                if(MultiCountryNextLevelActiveCategories_Static[countryId].ContainsKey(cid))
                    return MultiCountryNextLevelActiveCategories_Static[countryId][cid];
            }
            else if (cid == -99)
            {
                 return GetAllFinanceCategory();
            }

            return new List<CategoryCache>();
        }

        /// <summary>
        /// 是否不需要显示Brands过滤
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static bool IsHiddenBrandsCategoryID(int cid, int countryId)
        {
            if (MultiHiddenBrandsCategoryIdDic_Static.ContainsKey(countryId))
            {
                return MultiHiddenBrandsCategoryIdDic_Static[countryId].Contains(cid);
            }
            return true;
        }

        /// <summary>
        /// 获取Sitmap分类的下一级分类信息
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="includeAccessories"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<CatalogSitemapCategory> GetCatalogSitemapCategories(int categoryID, bool includeAccessories, int countryId)
        {
            if (categoryID == 0)
                return null;

            List<CatalogSitemapCategory> CatalogSitemapCategories = new List<CatalogSitemapCategory>();
            List<CategoryCache> categories = GetNextLevelSubCategories(categoryID, countryId);
            categories = categories.OrderByDescending(c => c.Clicks).ToList();

            foreach (CategoryCache c in categories)
            {
                List<CategoryCache> subCategories = GetNextLevelSubCategories(c.CategoryID, countryId);
                subCategories = subCategories.OrderByDescending(s => s.IsSiteMapDetailPopular).ThenByDescending(s => s.Clicks).ToList();

                CatalogSitemapCategory csc = new CatalogSitemapCategory();
                csc.Link.LinkText = c.CategoryName;
                csc.CategoryID = c.CategoryID;
                csc.Link.Value = c.CategoryID.ToString();
                csc.ImageURL = c.ImageFile;
                csc.IconUrl = c.IconUrl;
                csc.IconCode = c.CategoryIconCode;

                csc.SubCategoryInfos = new List<LinkInfo>();
                foreach (CategoryCache _c in subCategories)
                {
                    if (!includeAccessories && _c.IsAccessories)
                    {
                        continue;
                    }
                    LinkInfo sci = new LinkInfo();
                    sci.LinkText = _c.CategoryName;
                    sci.Value = _c.CategoryID.ToString();
                    sci.IconUrl = _c.IconUrl;

                    csc.SubCategoryInfos.Add(sci);
                }
                if (subCategories.Count == 0)
                {
                    if (!includeAccessories && c.IsAccessories)
                    {
                        continue;
                    }
                    LinkInfo sci = new LinkInfo();
                    sci.LinkText = c.CategoryName;
                    sci.Value = c.CategoryID.ToString();
                    sci.IconUrl = c.IconUrl;

                    csc.SubCategoryInfos.Add(sci);
                }
                CatalogSitemapCategories.Add(csc);
            }

            return CatalogSitemapCategories;
        }

        public static List<CatalogSitemapCategory> GetCatalogSitemapPopularCategories(int categoryId, int countryId)
        {
            if (categoryId == 0)
                return null;

            List<CatalogSitemapCategory> catalogSitemapCategoryList = new List<CatalogSitemapCategory>();

            List<CategoryCache> categories = GetAllSubSiteMapPopularCategoriesByParentID(categoryId, countryId);

            foreach (CategoryCache c in categories)
            {
                CatalogSitemapCategory csc = new CatalogSitemapCategory();
                csc.Link.LinkText = c.CategoryName;
                csc.Link.Value = c.CategoryID.ToString();
                csc.ImageURL = c.ImageFile;
                catalogSitemapCategoryList.Add(csc);
            }
            return catalogSitemapCategoryList;
        }

        /// <summary>
        /// 获取下一级IsSiteMapPopular为true的分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        private static List<CategoryCache> GetAllSubSiteMapPopularCategoriesByParentID(int categoryId, int countryId)
        {
            List<CategoryCache> resultCategory = new List<CategoryCache>();
            List<CategoryCache> categoryCollection = GetNextLevelSubCategories(categoryId, countryId);
            return categoryCollection.Where(c => c.IsSiteMapPopular).ToList();
        }

        /// <summary>
        /// 获取下一级非附件的分类信息，结果按点击排序（从多到少）
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<CategoryCache> GetNextLevelSubCategoriesWithoutAccessoiesOrderByClicks(int categoryId, int countryId)
        {
            List<CategoryCache> ccList = GetNextLevelSubCategories(categoryId, countryId);
            if (ccList != null && ccList.Count > 0)
            {
                return ccList.Where(c => c.IsAccessories == false).OrderByDescending(c => c.Clicks).ToList();
            }
            return ccList;
        }

        /// <summary>
        /// 获取分类名的同义词
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<CategoryExtend> GetCategorySynonym(int categoryID, int countryId)
        {
            if(MultiCountryCategorySynonymDic_Static.ContainsKey(countryId))
            {
                if(MultiCountryCategorySynonymDic_Static[countryId].ContainsKey(categoryID))
                {
                    return MultiCountryCategorySynonymDic_Static[countryId][categoryID];
                }
            }

            return null;
        }

        /// <summary>
        /// 获取ShortCuts的分类
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static List<CategoryCache> GetShortCutsCategories(int categoryId, int countryId)
        {
            List<CategoryCache> categoryCache = new List<CategoryCache>();
            if (MultiCountryCategoryShortCutsDic_Static.ContainsKey(countryId))
            {
                if (MultiCountryCategoryShortCutsDic_Static[countryId].ContainsKey(categoryId))
                {
                    Dictionary<int, bool> categoryIDs = MultiCountryCategoryShortCutsDic_Static[countryId][categoryId];
                    foreach (int cid in categoryIDs.Keys)
                    {
                        CategoryCache cc = GetCategoryByCategoryID(cid, countryId);
                        cc.IsShortCutPopular = categoryIDs[cid];
                        categoryCache.Add(cc);
                    }
                }
            }

            return categoryCache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static string GetCategoryNameByCategoryID(int categoryId, int countryId)
        {
            CategoryCache category = GetCategoryByCategoryID(categoryId, countryId);
            if (category != null)
            {
                return category.CategoryName;
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static int GetCategoryProductCount(int categoryId, int countryId)
        {
            CategoryCache category = GetCategoryByCategoryID(categoryId, countryId);
            if (category != null)
            {
                return category.ProductsCount;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<CategoryCache> GetNextLevelSubCategoriesIsSiteMapDetailPopular(int categoryId, int countryId)
        {
            List<CategoryCache> list = GetNextLevelSubCategories(categoryId, countryId);
            return list.Where(cat => cat.IsSiteMapDetailPopular).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryid"></param>
        /// <returns></returns>
        public static List<CategoryCache> GetNextLevelSubCategoriesIsSiteMapDetailPopularOrderByClicks(int categoryId, int countryId)
        {
            List<CategoryCache> ccList = GetNextLevelSubCategoriesIsSiteMapDetailPopular(categoryId, countryId);
            return ccList.OrderByDescending(c => c.Clicks).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryid"></param>
        /// <returns></returns>
        public static List<CategoryCache> GetNextLevelSubCategoriesIsNotSiteMapDetailPopular(int categoryId, int countryId)
        {
            List<CategoryCache> list = GetNextLevelSubCategories(categoryId, countryId);
            return list.Where(cat => !cat.IsSiteMapDetailPopular).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<CategoryCache> GetBreadCrumbCategoryList(CategoryCache category, int countryId)
        {
            List<CategoryCache> categoryList = new List<CategoryCache>();
            GetBreadCrumbCategoryList(category, categoryList, countryId);
            return categoryList;
        }

        static void GetBreadCrumbCategoryList(CategoryCache category, List<CategoryCache> categoryList, int countryId)
        {
            categoryList.Add(category);
            if (category.ParentID != 0)
            {
                CategoryCache parentCategory = GetCategoryByCategoryID(category.ParentID, countryId);
                GetBreadCrumbCategoryList(parentCategory, categoryList, countryId);
            }
        }

        public static List<RelatedCategoryCache> GetRelatedCategory(int categoryID)
        {
            //if (relatedCategoryDictionary != null && relatedCategoryDictionary.ContainsKey(categoryID))
            //{
            //    return relatedCategoryDictionary[categoryID];
            //}
            return null;
        }

        public static bool IsSearchOnly(int categoryId, int countryId)
        {
            return false;

            if (MultiCountryCategoryIsSearchOnlyDic_Static.ContainsKey(countryId) && MultiCountryCategoryIsSearchOnlyDic_Static[countryId].Contains(categoryId))
            {
                return true;
            }
            return false;
        }

        public static List<CategoryCache> GetAllRootCategoriesOrderByName(int countryId)
        {
            if(AllRootCategoriesOrderByName_Static.ContainsKey(countryId))
            {
                return AllRootCategoriesOrderByName_Static[countryId];
            }
            
            return new List<CategoryCache>();
        }

        public static string[] GetAllSubCategoryIdString(int cid, int countryId)
        {
            List<int> cidList = GetAllSubCategoryId(cid, countryId);
            string[] categoryIDs = new string[cidList.Count];

            for (int i = 0; i < cidList.Count; i++)
            {
                categoryIDs[i] = cidList[i].ToString();
            }

            return categoryIDs;
        }

        public static List<int> GetAllSubCategoryId(int cId, int countryId)
        {
            List<int> categoryIDs = new List<int>();

            List<CategoryCache> categoryCaches = GetNextLevelSubCategories(cId, countryId);
            foreach (var cache in categoryCaches)
            {
                SetAllSubCategoryIds(categoryIDs, cache.CategoryID, countryId);
            }

            return categoryIDs;
        }

        private static void SetAllSubCategoryIds(List<int> categoryIDs, int cid, int countryId)
        {
            categoryIDs.Add(cid);

            List<CategoryCache> categoryCaches = GetNextLevelSubCategories(cid, countryId);
            foreach (var cache in categoryCaches)
            {
                SetAllSubCategoryIds(categoryIDs, cache.CategoryID, countryId);
            }
        }

        public static List<CategoryCache> GetCategoriesMenuList(int countryId)
        {
            if(MultiCountryCategoriesMenuListDic_Static.ContainsKey(countryId))
            {
                return MultiCountryCategoriesMenuListDic_Static[countryId];
            }

            return null;
        }

        public static CategoryCache GetCategoryByProductID(int productId, int countryId)
        {
            var product = ProductController.GetProductNew(productId, countryId);

            if (product != null)
            {
                return GetCategoryByCategoryID(product.CategoryID.Value, countryId);
            }
            return null;
        }

        public static List<int> GetAllNoWeightUnitCategoryIdList(int countryId)
        {
            if(MultiCountryCategoryWuDic_Static.ContainsKey(countryId))
            {
                return MultiCountryCategoryWuDic_Static[countryId];
            }

            return null;
        }

        public static bool IsNoWeightUnitCategory(int categoryId, int countryId)
        {
            if (MultiCountryCategoryWuDic_Static.ContainsKey(countryId))
            {
                return MultiCountryCategoryWuDic_Static[countryId].Contains(categoryId);
            }

            return false;
        }

        public static List<CategoryFilterData> GetAllCategoryFilter(int countryId)
        {
            if(MultiCountryCategoryFilterDataDic_Static.ContainsKey(countryId))
            {
                return MultiCountryCategoryFilterDataDic_Static[countryId];
            }

            return null;
        }

        public static List<CategoryCache> GetAllSubCategory(int cid, int countryId)
        {
            List<CategoryCache> categoryList = new List<CategoryCache>();
            string[] subCategoryIDs = SearchController.GetSubCategoryIDs(cid, countryId);
            foreach (string subCID in subCategoryIDs)
            {
                int _cid = int.Parse(subCID);
                if (_cid == cid) continue;
                CategoryCache category = GetCategoryByCategoryID(_cid, countryId);
                if (category != null)
                {
                    categoryList.Add(category);
                }
            }
            return categoryList;
        }

        public static List<CategoryCache> GetAllReviewCategories(int countryId)
        {
            if(MultiCountryReviewCategoriesDic_Static.ContainsKey(countryId))
            {
                return MultiCountryReviewCategoriesDic_Static[countryId];
            }

            return null;
        }

        public static List<CategoryCache> GetMoneyPlansList(int countryId)
        {
            if (MultiCountryMoneyPlansList_Static.ContainsKey(countryId))
            {
                return MultiCountryMoneyPlansList_Static[countryId];
            }

            return null;
        }

        public static List<CategoryCache> GetReviewCategoriesByCategoryId(int cid, int countryId)
        {
            var ccList = GetAllReviewCategories(countryId);

            if(ccList != null)
            {
                return ccList.Where(r => r.ParentID == cid).ToList();
            }

            return null;
        }

        public static List<CategoryCache> GetAllSiteMapRootCategoriesList(int countryId)
        {
            if(MultiCountrySiteMapRootCategoriesList_Static.ContainsKey(countryId))
            {
                return MultiCountrySiteMapRootCategoriesList_Static[countryId];
            }

            return null;
        }

        public static List<CategoryCache> GetAllCategoryOrderByNameList(int countryId)
        {
            if(MultiCountryCategoryOrderByNameList_Static.ContainsKey(countryId))
            {
                return MultiCountryCategoryOrderByNameList_Static[countryId];
            }

            return null;
        }
    }
}