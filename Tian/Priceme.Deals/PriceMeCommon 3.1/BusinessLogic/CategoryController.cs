using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using PriceMeCommon.Convert;
using PriceMeCommon.Data;
using SubSonic.Schema;
using PriceMeCache;

namespace PriceMeCommon
{
    public static class CategoryController
    {
        public static List<CategoryCache> SiteMapRootCategoriesList;
        public static List<CategoryCache> CategoriesManuList;
        public static List<CategoryCache> AllPopularParentCategoriesOrderByName;
        public static List<CategoryCache> AllActiveRootCategoriesOrderByName;
        public static List<int> SimpleSitemapCategory;

        static List<CategoryCache> catOrderByName;

        private static List<MostPopularCategory> mostPopularCategories;
        private static Dictionary<int, List<string>> relatedManufacturerCategories; //VelocityCacheKey.RelatedManufacturerCategories


        static Dictionary<int, CategoryCache> categoryCache;
        private static Dictionary<int, List<CategoryCache>> nextLevelActiveCategories;
        static List<int> accessoiesCategoryIDList;
        static Dictionary<int, int> categoryProductCount;
        static Dictionary<int, int> categoryProductClickCount;
        static Dictionary<int, List<PriceMeCache.RelatedCategoryCache>> relatedCategoryDictionary;

        //static PriceMeDBDB priceMeDB = PriceMeStatic.PriceMeDB;
        static object lockObj = new object();
        public static List<int> HiddenBrandsCategoryID;

        static float priceMeExchangeRate = 1;
        static Dictionary<int, Dictionary<int, bool>> categoryShortCutsDictionary;

        static List<CategoryCache> reviewCategoryList;
        private static List<FeaturedTabCache> featuredProducts;

        public static Dictionary<int, int> Category301Dictionary;
        static Dictionary<int, string> CategoryMaxPriceDic;

        static Dictionary<int, CategoryExtend> CategoryExtendDic;
        static Dictionary<int, List<CategorySynonym>> CategorySynonymDic;

        public static List<int> listIsSearchOnly { get; set; }
        public static List<int> viewList { get; set; }
        public static List<int> viewQuick { get; set; }
        public static List<int> categoryWu { get; set; }
        public static List<FavouritesPageData> listFavoritesCatalog { get; set; }
        public static List<FavouritesPageData> listFavoritesSearch { get; set; }
        public static List<CategoryFilterData> listCategoryFilter { get; set; }

        static CategoryController()
        {
            bool autoLoad = false;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["AutoLoadCategoryController"], out autoLoad);
            if (autoLoad)
            {
                try
                {
                    Load(null);
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
                    throw ex;
                }
            }
        }

        #region ruangang
        public static List<MostPopularCategory> MostPopularCategories{
            get{return mostPopularCategories;}
        }

        public static Dictionary<int, List<string>> RelatedManufacturerCategories {
            get { return relatedManufacturerCategories; }
        }
        #endregion

        public static List<CategoryCache> CategoryOrderByName
        {
            get { return catOrderByName; }
        }

        public static Dictionary<int, List<CategoryCache>> NextLevelActiveCategories
        {
            get { return nextLevelActiveCategories; }
        }
        public static List<FeaturedTabCache> FeaturedProducts
        {
            get { return featuredProducts; }
        }

        public static void Load()
        {
            Load(null);
        }

        public static void Load(Timer.DKTimer dkTimer)
        {
            CategoryMaxPriceDic = new Dictionary<int, string>();

            Category301Dictionary = GetCategory301Dictionary();
            SimpleSitemapCategory = GetSimpleSitemapCategory();
            CategoryExtendDic = GetCategoryExtendDic();
            CategorySynonymDic = GetCategorySynonymDic();
            BindIsSearchOnly();

            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load priceMeExchangeRate");
            }
            CSK_Util_Country _country = CSK_Util_Country.SingleOrDefault(country => country.countryID == ConfigAppString.CountryID);
            if (_country != null)
            {
                priceMeExchangeRate = (float)_country.PriceMeExchangeRate;
            }
            LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "priceMeExchangeRate : " + priceMeExchangeRate);

            //

            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load catOrderByName");
            }
            catOrderByName = VelocityController.GetCache<List<CategoryCache>>(VelocityCacheKey.CategoryByName);
            if (catOrderByName == null)
            {
                catOrderByName = GetCatOrderByNameOrderByName();
                //List<CSK_Store_Category> catOrderByNameList = CSK_Store_Category.All().Where(cat => cat.IsActive).OrderBy(cat => cat.CategoryName).ToList();
                //catOrderByName = ConvertController<CategoryCache, CSK_Store_Category>.ConvertData(catOrderByNameList);
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "CategoryByName no velocity");
            }
            else
            {
                HiddenBrandsCategoryID = VelocityController.GetCache<List<int>>(VelocityCacheKey.HiddenBrandsCategoryID);
                if (HiddenBrandsCategoryID == null)
                {
                    HiddenBrandsCategoryID = catOrderByName.Where(cat => !cat.IsFilterByBrand).Select(cat => cat.CategoryID).ToList();
                    LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "HiddenBrandsCategoryID no velocity");
                }
            }
            LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "Category cache count : " + catOrderByName.Count);


            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load AllPopularParentCategoriesOrderByName");
            }
            //
            AllPopularParentCategoriesOrderByName = VelocityController.GetCache<List<CategoryCache>>(VelocityCacheKey.AllPopularParentCategoriesOrderByName);
            if(AllPopularParentCategoriesOrderByName == null)
            {
                AllPopularParentCategoriesOrderByName = catOrderByName.FindAll(cat => cat.ParentID == 0 && cat.IsActive && cat.PopularCategory == true).OrderBy(cat => cat.CategoryName).ToList();
                //IOrderedEnumerable<CSK_Store_Category> allPopularParentCategoriesOrderByName = CSK_Store_Category.Find(cat => cat.ParentID == new int?(0) && cat.IsActive && cat.PopularCategory == true).OrderBy(cat => cat.CategoryName);
                //AllPopularParentCategoriesOrderByName = ConvertController<CategoryCache, CSK_Store_Category>.ConvertData(allPopularParentCategoriesOrderByName);
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AllPopularParentCategoriesOrderByName no velocity");
            }
            CategoriesManuList = new List<CategoryCache>(AllPopularParentCategoriesOrderByName);
            if (ConfigAppString.FinanceWebsite)
            {
                CategoryCache cc = new CategoryCache();
                cc.CategoryID = -99;
                cc.CategoryName = "Money & Plans";
                cc.PopularCategory = true;
                CategoriesManuList.Add(cc);
                CategoriesManuList = CategoriesManuList.OrderBy(c => c.CategoryName).ToList();
            }

            SiteMapRootCategoriesList = catOrderByName.FindAll(cat => cat.ParentID == 0 && cat.IsActive).ToList();
            if (ConfigAppString.FinanceWebsite)
            {
                CategoryCache cc = new CategoryCache();
                cc.CategoryID = -99;
                cc.CategoryName = "Money";
                cc.PopularCategory = true;
                SiteMapRootCategoriesList.Add(cc);
            }
            SiteMapRootCategoriesList = SiteMapRootCategoriesList.OrderBy(c => c.CategoryName).ToList();

            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load AllActiveRootCategoriesOrderByName");
            }
            AllActiveRootCategoriesOrderByName = VelocityController.GetCache<List<CategoryCache>>(VelocityCacheKey.AllActiveRootCategoriesOrderByName);
            if (AllActiveRootCategoriesOrderByName == null)
            {
                AllActiveRootCategoriesOrderByName = catOrderByName.FindAll(cat => cat.ParentID == 0 && cat.IsActive).OrderBy(cat => cat.CategoryName).ToList();
                //IOrderedEnumerable<CSK_Store_Category> allActiveRootCategoriesOrderByName = CSK_Store_Category.Find(cat => cat.ParentID == new int?(0) && cat.IsActive).OrderBy(cat => cat.CategoryName);
                //AllActiveRootCategoriesOrderByName = ConvertController<CategoryCache, CSK_Store_Category>.ConvertData(allActiveRootCategoriesOrderByName);
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AllActiveRootCategoriesOrderByName no velocity");
            }

            //
            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load categoryProductCount");
            }
            categoryProductCount = VelocityController.GetCache<Dictionary<int, int>>(VelocityCacheKey.CategoryProductCount);
            if (categoryProductCount == null)
            {
                categoryProductCount = new Dictionary<int, int>();
                foreach (CategoryCache category in catOrderByName)
                {
                    PriceMeCommon.Data.HitsInfo hitsInfo = SearchController.SearchProducts("", category.CategoryID, null, null, null, null, "", null, true, true, false, 10000, ConfigAppString.CountryID, false, true, true);
                    if (hitsInfo != null && hitsInfo.ResultCount > 0)
                    {
                        categoryProductCount.Add(category.CategoryID, hitsInfo.ResultCount);
                    }
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "CategoryProductCount no velocity");
            }
            categoryProductClickCount = VelocityController.GetCache<Dictionary<int, int>>(VelocityCacheKey.CategoryProductClickCount);
            if (categoryProductClickCount == null)
            {
                categoryProductClickCount = new Dictionary<int, int>();
                foreach (CategoryCache category in catOrderByName)
                {
                    PriceMeCommon.Data.HitsInfo hitsInfo = SearchController.SearchProducts("", category.CategoryID, null, null, null, null, "", null, true, true, false, 10000, ConfigAppString.CountryID, false, true, true);
                    if (hitsInfo != null && hitsInfo.ResultCount > 0)
                    {
                        int totalClicks = 0;
                        for(int i = 0; i < hitsInfo.ResultCount; i++ )
                        {
                            var doc = hitsInfo.GetDocument(i, new string[] { "Clicks" });
                            string clicks = doc.Get("Clicks");
                            totalClicks += int.Parse(clicks);
                        }
                        categoryProductClickCount.Add(category.CategoryID, totalClicks);
                    }
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "CategoryProductClickCount no velocity");
            }
            

            //
            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load DoNotRemoveNoProductCategory");
            }
            bool doNotRemoveNoProductCategory = true;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["DoNotRemoveNoProductCategory"], out doNotRemoveNoProductCategory);

            if (!doNotRemoveNoProductCategory)
            {
                if (categoryProductCount.Count > 0)
                {
                    for (int i = 0; i < catOrderByName.Count; )
                    {
                        if (!categoryProductCount.ContainsKey(catOrderByName[i].CategoryID))
                        {
                            catOrderByName.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                    for (int i = 0; i < AllPopularParentCategoriesOrderByName.Count; )
                    {
                        if (!categoryProductCount.ContainsKey(AllPopularParentCategoriesOrderByName[i].CategoryID))
                        {
                            AllPopularParentCategoriesOrderByName.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                    for (int i = 0; i < AllActiveRootCategoriesOrderByName.Count; )
                    {
                        if (!categoryProductCount.ContainsKey(AllActiveRootCategoriesOrderByName[i].CategoryID))
                        {
                            AllActiveRootCategoriesOrderByName.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
            }
            //
            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load accessoiesCategoryIDList");
            }
            accessoiesCategoryIDList = new List<int>();
            categoryCache = new Dictionary<int, CategoryCache>();

            foreach (CategoryCache c in catOrderByName)
            {
                categoryCache.Add(c.CategoryID, c);
                if (c.IsAccessories)
                {
                    accessoiesCategoryIDList.Add(c.CategoryID);
                }
            }

            //
            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load nextLevelActiveCategories");
            }
            nextLevelActiveCategories = VelocityController.GetCache<Dictionary<int, List<CategoryCache>>>(VelocityCacheKey.NextLevelActiveCategories);
            if (nextLevelActiveCategories == null)
            {
                nextLevelActiveCategories = new Dictionary<int, List<CategoryCache>>();
                foreach (CategoryCache category in catOrderByName)
                {
                    NextLevelActiveCategories.Add(category.CategoryID, GetNextLevelSubCategoriesIsActiveCache(category.CategoryID));
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "NextLevelActiveCategories no velocity");
            }

            relatedCategoryDictionary = VelocityController.GetCache<Dictionary<int, List<PriceMeCache.RelatedCategoryCache>>>(VelocityCacheKey.RelatedCategorys);

            //
            //Dictionary<int, CSK_Store_Category> categoryDictionary = CSK_Store_Category.All().ToDictionary(c => c.CategoryID, c => c);

            //
            //if (dkTimer != null)
            //{
            //    dkTimer.Set("CategoryController.Load() --- Befor load hiddenBrandsCategoryID");
            //}
            //hiddenBrandsCategoryID = GetHiddenBrandsCategoryID();

            #region
            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load MostPopularCategories");
            }
            mostPopularCategories = VelocityController.GetCache<List<MostPopularCategory>>(VelocityCacheKey.MostPopularCategories);
            if (mostPopularCategories == null)
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "mostPopularCategories no velocity");
            }
            else
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "mostPopularCategories count : " + mostPopularCategories.Count);
            }

            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load RelatedManufacturerCategories");
            }
            relatedManufacturerCategories = VelocityController.GetCache<Dictionary<int, List<string>>>(VelocityCacheKey.RelatedManufacturerCategories);
            if (relatedManufacturerCategories == null)
            {
                relatedManufacturerCategories = new Dictionary<int, List<string>>();
                //List<int> cid = new List<int>();
                //cid.Add(1178);
                //cid.Add(1254);
                //cid.Add(1255);
                //cid.Add(1337);
                //cid.Add(1392);
                //cid.Add(1393);
                //cid.Add(1427);
                //cid.Add(145);
                //cid.Add(15);
                //relatedManufacturerCategories.Add(1, cid);
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "RelatedManufacturerCategories no velocity");
            }

            #endregion

            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load BindReviewCategorys");
            }
            BindReviewCategorys();// reviewCategoryList = VelocityController.GetCache(VelocityCacheKey.ReviewCatgory) as List<CategoryCache>;

            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load FeaturedProduct");
            }
            featuredProducts = VelocityController.GetCache<List<PriceMeCache.FeaturedTabCache>>(VelocityCacheKey.FeaturedProducts);
            if (featuredProducts == null)
            {
                featuredProducts = new List<FeaturedTabCache>();
                List<CSK_Store_FeaturedTab> featuredTabList = CSK_Store_FeaturedTab.All().OrderBy(f => f.ListOrder).ToList();
                foreach (CSK_Store_FeaturedTab ft in featuredTabList)
                {
                    StoredProcedure sp = PriceMeStatic.PriceMeDB.CSK_Store_FeatureProducts();
                    sp.Command.CommandTimeout = 0;
                    sp.Command.AddParameter("@CATID", ft.CategoryID, DbType.Int32);
                    sp.Command.AddParameter("@COUNTRYID", ConfigAppString.CountryID, DbType.Int32);
                    IDataReader dr = sp.ExecuteReader();
                    List<PriceMeCache.FeaturedProduct> featuredProductList = new List<FeaturedProduct>();
                    while (dr.Read())
                    {
                        PriceMeCache.FeaturedProduct featuredProduct = new FeaturedProduct();
                        featuredProduct.ProductID = int.Parse(dr["ProductID"].ToString());
                        featuredProduct.ProductName = dr["ProductName"].ToString();
                        featuredProduct.DefaultImage = dr["DefaultImage"].ToString();
                        featuredProduct.CategoryID = int.Parse(dr["CategoryID"].ToString());
                        featuredProduct.RootID = int.Parse(dr["RootID"].ToString());
                        featuredProduct.MaxPrice = double.Parse(dr["MaxPrice"].ToString());
                        featuredProduct.MinPrice = double.Parse(dr["MinPrice"].ToString());
                        featuredProduct.ProductGUID = dr["ProductGUID"].ToString();
                        featuredProductList.Add(featuredProduct);
                    }
                    dr.Close();

                    PriceMeCache.FeaturedTabCache featuredTabCache = new FeaturedTabCache();
                    featuredTabCache.CategoryID = ft.CategoryID;
                    featuredTabCache.Label = categoryCache.ContainsKey(ft.CategoryID) ? categoryCache[ft.CategoryID].CategoryName : ft.Label;
                    featuredTabCache.Title = ft.Title;
                    featuredTabCache.ListOrder = int.Parse(ft.ListOrder.ToString());
                    featuredTabCache.FeaturedProductList = featuredProductList;
                    featuredProducts.Add(featuredTabCache);
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "AllFeaturedProducts no velocity");
            }

            categoryShortCutsDictionary = GetCategoryShortCutsDictionary();

            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load BindIsSearchOnly");
            }
            
            BindCategoryView();
            BindCategoryWeightUnit();
            BindFavourite();
            BindCategoryFilter();

            if (dkTimer != null)
            {
                dkTimer.Set("CategoryController.Load() --- Befor load 301 table");
            }
        }

        private static void BindCategoryFilter()
        {
            listCategoryFilter = new List<CategoryFilterData>();
            string selectSql = "Select * From CategoryFilter";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader dr = sqlCMD.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int id = 0, cid = 0;
                            int.TryParse(dr["Id"].ToString(), out id);
                            int.TryParse(dr["CategoryID"].ToString(), out cid);
                            string name = dr["FilterName"].ToString();
                            string url = dr["FilterUrl"].ToString();
                            CategoryFilterData data = new CategoryFilterData();
                            data.Id = id;
                            data.CategoryId = cid;
                            data.FilterName = name;
                            data.FilterUrl = url;
                            listCategoryFilter.Add(data);
                        }
                        dr.Close();
                    }
                }
            }
        }

        private static void BindFavourite()
        {
            listFavoritesCatalog = new List<FavouritesPageData>();
            listFavoritesSearch = new List<FavouritesPageData>();
            string sql = "select * from PageFavourites where PageId != '' And (PageName = 'catalog' or PageName = 'search') order by id desc ";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                string pageid = dr["PageId"].ToString();
                string tokenID = dr["tokenID"].ToString();
                string pagename = dr["PageName"].ToString();
                FavouritesPageData fp = new FavouritesPageData();
                fp.PageId = pageid.ToLower();
                fp.TokenId = tokenID;
                fp.PageName = pagename;
                if (pagename == "catalog")
                    listFavoritesCatalog.Add(fp);
                else
                    listFavoritesSearch.Add(fp);
            }
            dr.Close();
        }

        private static void BindCategoryWeightUnit()
        {
            categoryWu = new List<int>();
            string sql = "select CategoryID from CSK_Store_Category Where WeightUnit = 0";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int cid = 0;
                int.TryParse(dr["CategoryID"].ToString(), out cid);
                categoryWu.Add(cid);
            }
            dr.Close();
        }

        private static void BindCategoryView()
        {
            viewList = new List<int>();
            string sql = "select CategoryID from CSK_Store_Category Where CategoryViewType = 2";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int cid = 0;
                int.TryParse(dr["CategoryID"].ToString(), out cid);
                viewList.Add(cid);
            }
            dr.Close();

            viewQuick = new List<int>();
            sql = "select CategoryID from CSK_Store_Category Where CategoryViewType = 3";
            StoredProcedure qsp = new StoredProcedure("");
            qsp.Command.CommandSql = sql;
            qsp.Command.CommandTimeout = 0;
            qsp.Command.CommandType = CommandType.Text;
            IDataReader qdr = qsp.ExecuteReader();
            while (qdr.Read())
            {
                int cid = 0;
                int.TryParse(qdr["CategoryID"].ToString(), out cid);
                viewQuick.Add(cid);
            }
            qdr.Close();
        }

        private static void BindIsSearchOnly()
        {
            listIsSearchOnly = new List<int>();
            string sql = "select CategoryID from CSK_Store_Category where isSearchOnly = 1";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int cid = 0;
                int.TryParse(dr["CategoryID"].ToString(), out cid);
                listIsSearchOnly.Add(cid);
            }
            dr.Close();
        }

        public static List<CategorySynonym> GetCategorySynonym(int categoryID)
        {
            if (CategorySynonymDic.ContainsKey(categoryID))
            {
                return CategorySynonymDic[categoryID];
            }
            return null;
        }

        private static Dictionary<int, List<CategorySynonym>> GetCategorySynonymDic()
        {
            Dictionary<int, List<CategorySynonym>> dic = new Dictionary<int, List<CategorySynonym>>();

            string selectSql = "SELECT CategoryID,[Synonym],LocalName FROM [dbo].[CategorySynonym_New] where CountryID = " + PriceMeCommon.ConfigAppString.CountryID;
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
                            int cid = sqlDR.GetInt32(0);
                            string synonym = sqlDR.GetString(1);
                            string localName = sqlDR.GetString(2);
                            CategorySynonym categorySynonym = new CategorySynonym();
                            categorySynonym.CategoryID = cid;
                            categorySynonym.Synonym = synonym;
                            categorySynonym.LocalName = localName;
                            categorySynonym.CountryID = PriceMeCommon.ConfigAppString.CountryID;

                            if (dic.ContainsKey(cid))
                            {
                                dic[cid].Add(categorySynonym);
                            }
                            else
                            {
                                List<CategorySynonym> list = new List<CategorySynonym>();
                                list.Add(categorySynonym);
                                dic.Add(cid, list);
                            }
                        }
                    }
                }
            }

            return dic;
        }

        public static bool IsComareTop3(int categoryID)
        {
            if(CategoryExtendDic.ContainsKey(categoryID))
            {
                return CategoryExtendDic[categoryID].IsCompareTop3;
            }
            else
            {
                return false;
            }
        }

        private static Dictionary<int, CategoryExtend> GetCategoryExtendDic()
        {
            Dictionary<int, CategoryExtend> dic = new Dictionary<int, CategoryExtend>();
            string selectSql = "select CategoryID, IsCompareTop3 from CategoryExtend";
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
                            int cid = sqlDR.GetInt32(0);
                            bool isComareTop3 = sqlDR.GetBoolean(1);
                            CategoryExtend categoryExtend = new CategoryExtend();
                            categoryExtend.CategoryID = cid;
                            categoryExtend.IsCompareTop3 = isComareTop3;
                            dic.Add(cid, categoryExtend);
                        }
                    }
                }
            }

            return dic;
        }

        private static Dictionary<int, int> GetCategory301Dictionary()
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            string selectSql = "select * from [Category301Redirect]";
            string connString = PriceMeCommon.ConfigAppString.CommerceTemplateCommon;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int fcid = int.Parse(sqlDR["FromCategoryid"].ToString());
                            int tcid = int.Parse(sqlDR["ToCategoryid"].ToString());
                            dictionary.Add(fcid, tcid);
                        }
                    }
                }
            }

            return dictionary;
        }

        public static List<CategoryCache> GetShortCutsCategories(int categoryID)
        {
            List<CategoryCache> categoryCache = new List<CategoryCache>();
            if (categoryShortCutsDictionary.ContainsKey(categoryID))
            {
                Dictionary<int, bool> categoryIDs = categoryShortCutsDictionary[categoryID];
                foreach (int cid in categoryIDs.Keys)
                {
                    if (categoryProductCount.ContainsKey(cid) && categoryProductCount[cid] > 0)
                    {
                        CategoryCache cc = GetCategoryByCategoryID(cid);
                        cc.IsShortCutPopular = categoryIDs[cid];
                        categoryCache.Add(cc);
                    }
                }
            }

            return categoryCache;
        }

        private static List<int> GetSimpleSitemapCategory()
        {
            List<int> simpleSitemapCategoryID = new List<int>();
            string selectSql = "select CategoryID from SimpleSitemapCategory";
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
                            int cid = sqlDR.GetInt32(0);
                            simpleSitemapCategoryID.Add(cid);
                        }
                    }
                }
            }

            return simpleSitemapCategoryID;
        }

        private static Dictionary<int, Dictionary<int, bool>> GetCategoryShortCutsDictionary()
        {
            Dictionary<int, Dictionary<int, bool>> dictionary = new Dictionary<int, Dictionary<int, bool>>();
            string selectSql = "select * from ShortCutCategory_Map";
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

        private static List<CategoryCache> GetCatOrderByNameOrderByName()
        {
            HiddenBrandsCategoryID = new List<int>();
            List<CategoryCache> categoryCacheList = new List<CategoryCache>();
            string selectSql = "select CT.*,LCT.CategoryName as LocalName,LCT.Description as LocalDesc from CSK_Store_Category as CT left join Local_CategoryName as LCT on CT.CategoryID = LCT.CategoryID and LCT.CountryID = " + ConfigAppString.CountryID + " where CT.IsActive = 1 order by CT.CategoryName";
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
                            //categoryCache.IsAutomatic = bool.Parse(sqlDR["IsAutomatic"].ToString());
                            categoryCache.IsDisplayIsMerged = bool.Parse(sqlDR["IsDisplayIsMerged"].ToString());
                            categoryCache.IsFilterByBrand = bool.Parse(sqlDR["IsFilterByBrand"].ToString());
                            //categoryCache.IsFooterCategory = bool.Parse(sqlDR["IsFooterCategory"].ToString());
                            categoryCache.IsSiteMap = bool.Parse(sqlDR["IsSiteMap"].ToString());
                            categoryCache.IsSiteMapDetail = bool.Parse(sqlDR["IsSiteMapDetail"].ToString());
                            categoryCache.IsSiteMapDetailPopular = bool.Parse(sqlDR["IsSiteMapDetailPopular"].ToString());
                            categoryCache.IsSiteMapPopular = bool.Parse(sqlDR["IsSiteMapPopular"].ToString());
                            //categoryCache.ListOrder = int.Parse(sqlDR["ListOrder"].ToString());
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

                            categoryCacheList.Add(categoryCache);

                            if (!categoryCache.IsFilterByBrand)
                            {
                                HiddenBrandsCategoryID.Add(categoryCache.CategoryID);
                            }
                        }
                    }
                }
            }

            return categoryCacheList;
        }

        public static List<int> GetFooterCategoryIDList()
        {
            List<int> idList = new List<int>();
            List<CategoryFooterMap> categoryFooterMapList = CategoryFooterMap.All().ToList();
            foreach (var cfm in categoryFooterMapList)
            {
                idList.Add(cfm.CategoryID);
            }
            return idList;
        }

        private static void BindReviewCategorys()
        {
            reviewCategoryList = (List<CategoryCache>)VelocityController.GetCache(VelocityCacheKey.ReviewCatgory);
            if (reviewCategoryList == null)
            {
                try
                {
                    reviewCategoryList = new List<CategoryCache>();
                    SubSonic.Schema.StoredProcedure sp = PriceMeDBStatic.PriceMeDB.GetReviewsCatgory();
                    sp.Command.CommandTimeout = 0;
                    IDataReader dr = sp.ExecuteReader();

                    while (dr.Read())
                    {
                        CategoryCache c = new CategoryCache();
                        c.CategoryID = int.Parse(dr["CategoryID"].ToString());
                        c.CategoryName = dr["CategoryName"].ToString();
                        c.ParentID = int.Parse(dr["ParentID"].ToString());

                        reviewCategoryList.Add(c);
                    }

                    dr.Close();
                }
                catch (Exception ex) { LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, "ReviewCatgory Error: " + ex.Message); }

                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "ReviewCatgory no velocity");
            }
        }

        public static List<CategoryCache> GetReviewCategorys(int parentID)
        {
            List<CategoryCache> cateList = new List<CategoryCache>();
            if (reviewCategoryList != null)
                cateList = reviewCategoryList.Where(r => r.ParentID == parentID).ToList();
            
            return cateList;
        }

        public static bool IsHiddenBrandsCategoryID(int cid)
        {
            return HiddenBrandsCategoryID.Contains(cid);
        }

        public static List<int> GetAllSubCategoryId()
        {
            return CSK_Store_Category.Find(c => !CSK_Store_Category.All().Select(_c => _c.ParentID).Contains(c.CategoryID)).
                Select(cat => cat.CategoryID).ToList();
        }

        public static List<CatalogSitemapCategory> GetCatalogSitemapCategories(int categoryID)
        {
            return GetCatalogSitemapCategories(categoryID, true);
        }

        public static List<CatalogSitemapCategory> GetCatalogSitemapPopularCategories(int categoryID)
        {
            if (categoryID == 0)
                return null;

            List<CatalogSitemapCategory> catalogSitemapCategoryList = new List<CatalogSitemapCategory>();

            List<PriceMeCache.CategoryCache> categories = CategoryController.GetAllSubSiteMapPopularCategoriesByParentID(categoryID);

            foreach (PriceMeCache.CategoryCache c in categories)
            {
                CatalogSitemapCategory csc = new CatalogSitemapCategory();
                csc.Link.LinkText = c.CategoryName;
                csc.Link.Value = c.CategoryID.ToString();
                csc.ImageURL = c.ImageFile;
                catalogSitemapCategoryList.Add(csc);
            }
            return catalogSitemapCategoryList;
        }

        private static List<CategoryCache> GetAllSubSiteMapPopularCategoriesByParentID(int categoryID)
        {
            List<CategoryCache> resultCategory = new List<CategoryCache>();
            List<CategoryCache> categoryCollection = GetAllSubCategory(categoryID);
            foreach (CategoryCache category in categoryCollection)
            {
                if (category.IsSiteMapPopular && category.CategoryID != categoryID)
                {
                    resultCategory.Add(category);
                }
            }
            return resultCategory;
        }

        public static List<HotSearch> GetHotSearchsByCategoryID(int CategoryID)
        {
            List<HotSearch> hotSearchs = new List<HotSearch>();

            SubSonic.Schema.StoredProcedure storeProcedure = PriceMeDBStatic.PriceMeDB.GetHotSearch();

            storeProcedure.Command.AddParameter("@days", ConfigAppString.CategoryHotSearchDays, DbType.Int32);
            storeProcedure.Command.AddParameter("@count", ConfigAppString.CategoryHotSearchCount, DbType.Int32);
            storeProcedure.Command.AddParameter("@categoryID", CategoryID, DbType.Int32);

            using (IDataReader idr = storeProcedure.ExecuteReader())
            {
                while (idr.Read())
                {
                    string keywords = idr["KeyWords"].ToString();
                    int searchCount = int.Parse(idr["SearchCount"].ToString());
                    HotSearch hotSearch = new HotSearch();
                    hotSearch.Keywords = keywords;
                    hotSearch.Count = searchCount;

                    ProductSearcher productSearcher = new ProductSearcher(CategoryID, null, null, null, null, null, keywords, null, false, int.MaxValue, ConfigAppString.CountryID, true);
                    if (productSearcher.GetSearchResult(1, 1).ProductCount > 0)
                    {
                        hotSearch.HasResult = true;
                    }
                    else
                    {
                        hotSearch.HasResult = false;
                    }
                    hotSearchs.Add(hotSearch);
                }
            }

            if (hotSearchs.Count > 3)
            {
                int stepPoint = (hotSearchs[0].Count - hotSearchs[hotSearchs.Count - 1].Count) / 4;
                int maxCount = hotSearchs[0].Count;
                if (stepPoint < 1)
                {
                    stepPoint = 1;
                }

                foreach (HotSearch hs in hotSearchs)
                {
                    int size = (maxCount - hs.Count) / stepPoint;
                    if ((maxCount - hs.Count) % stepPoint > stepPoint / 2)
                        size++;
                    if (hs.Count > 3)
                    {
                        hs.Number = 5 - size;
                    }
                    else
                    {
                        hs.Number = 1;
                    }
                }
            }
            hotSearchs.Sort();
            return hotSearchs;
        }

        public static List<PriceMeCache.RelatedCategoryCache> GetRelatedCategory(int categoryID)
        {
            if (relatedCategoryDictionary != null && relatedCategoryDictionary.ContainsKey(categoryID))
            {
                return relatedCategoryDictionary[categoryID];
            }
            return null;
        }

        public static bool IsAccessoriesCategory(int categoryID)
        {
            return accessoiesCategoryIDList.Contains(categoryID);
        }

        public static CategoryCache GetParentCategoryByCategoryID(int categoryID)
        {
            CategoryCache category = GetCategoryByCategoryID(categoryID);

            if (category == null || category.ParentID == 0)
                return category;

            return GetParentCategoryByCategoryID(category.ParentID);
        }

        public static CategoryCache GetCategoryByProductID(int productID)
        {
            var product = PriceMeCommon.BusinessLogic.ProductController.GetProduct(productID);
            
            if (product != null)
            {
                return CategoryController.GetCategoryByCategoryID(product.CategoryID.Value);
            }
            return null;
        }

        public static CategoryCache GetCategoryByCategoryID(int categoryID)
        {
            if (categoryCache != null && categoryCache.ContainsKey(categoryID))
            {
                return categoryCache[categoryID];
            }
            CategoryCache category = GetCategoryCacheFromDB(categoryID); 
            lock (lockObj)
            {
                if (categoryCache == null)
                {
                    categoryCache = new Dictionary<int, CategoryCache>();
                    categoryCache.Add(categoryID, category);
                }
                else if (!categoryCache.ContainsKey(categoryID))
                {
                    categoryCache.Add(categoryID, category);
                }
            }

            return category;
        }

        private static CategoryCache GetCategoryCacheFromDB(int categoryID)
        {
            CSK_Store_Category category = CSK_Store_Category.SingleOrDefault(cat => cat.CategoryID == categoryID);
            if (category != null)
            {
                CategoryCache cc = ConvertController<CategoryCache, CSK_Store_Category>.ConvertData(category);
                cc.CategoryNameEN = cc.CategoryName;

                return cc;
            }
            else
            {
                if (PriceMeCommon.CategoryController.Category301Dictionary.ContainsKey(categoryID))
                {
                    int _301Cid = PriceMeCommon.CategoryController.Category301Dictionary[categoryID];
                    category = CSK_Store_Category.SingleOrDefault(cat => cat.CategoryID == _301Cid);

                    if (category != null)
                    {
                        CategoryCache cc = ConvertController<CategoryCache, CSK_Store_Category>.ConvertData(category);
                        cc.CategoryNameEN = cc.CategoryName;

                        return cc;
                    }
                }
            }
            return null;
        }

        public static string GetCategoryNameByCategoryID(int categoryID)
        {
            CategoryCache category = GetCategoryByCategoryID(categoryID);
            if (category != null)
            {
                return category.CategoryName;
            }
            return "";
        }

        public static int GetCategoryProductCount(int categoryID)
        {
            if (categoryProductCount.ContainsKey(categoryID))
            {
                return categoryProductCount[categoryID];
            }
            return 0;
        }

        public static List<CategoryCache> GetNextLevelSubCategoriesIsActive(int parentCategoryID)
        {
            if (NextLevelActiveCategories.ContainsKey(parentCategoryID))
            {
                return NextLevelActiveCategories[parentCategoryID];
            }
            else
            {
                if (parentCategoryID == -99)
                {
                    return GetAllFinanceCategory();
                }
                return GetNextLevelSubCategoriesIsActiveCache(parentCategoryID);
            }
        }

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

        static List<CategoryCache> GetNextLevelSubCategoriesIsActiveCache(int parentCategoryID)
        {
            List<CategoryCache> categories = catOrderByName.Where(cat => cat.ParentID == parentCategoryID).ToList();
            return categories;
        }

        public static List<CategoryCache> GetNextLevelSubCategoriesIsSiteMapDetailPopular(int categoryid)
        {
            return catOrderByName.Where(cat => cat.ParentID == categoryid && cat.IsSiteMapDetailPopular).ToList();
        }

        public static List<CategoryCache> GetNextLevelSubCategoriesIsSiteMapDetailPopularOrderByClicks(int categoryid)
        {
            List<CategoryCache> ccList = GetNextLevelSubCategoriesIsSiteMapDetailPopular(categoryid);
            foreach(var cc in ccList)
            { 
                if(categoryProductClickCount.ContainsKey(cc.CategoryID))
                {
                    cc.Clicks = categoryProductClickCount[cc.CategoryID];
                }
            }
            return ccList.OrderByDescending(c => c.Clicks).ToList();
        }

        public static List<CategoryCache> GetNextLevelSubCategoriesWithoutAccessoiesOrderByClicks(int categoryid)
        {
            List<CategoryCache> ccList = GetNextLevelSubCategoriesIsActiveCache(categoryid);
            foreach (var cc in ccList)
            {
                if (categoryProductClickCount.ContainsKey(cc.CategoryID))
                {
                    cc.Clicks = categoryProductClickCount[cc.CategoryID];
                }
            }
            return ccList.Where(c=>c.IsAccessories == false).OrderByDescending(c => c.Clicks).ToList();
        }

        public static List<CategoryCache> GetNextLevelSubCategoriesIsSiteMapPopular(int categoryid)
        {
            return catOrderByName.Where(cat => cat.ParentID == categoryid && cat.IsSiteMapPopular).ToList();
        }

        public static List<CategoryCache> GetNextLevelSubCategoriesIsNotSiteMapDetailPopular(int categoryid)
        {
            return catOrderByName.Where(cat => cat.ParentID == categoryid && !cat.IsSiteMapDetailPopular).ToList();
        }

        public static CategoryCache GetRootCategory(int categoryid)
        {
            CategoryCache category = GetCategoryByCategoryID(categoryid);
            if (category == null || category.ParentID == 0)
                return category;
            return GetRootCategory(category.ParentID);
        }

        public static PriceMeCommon.Data.PriceRange GetCategoryPriceRange(int categoryID, int priceRangeNumber)
        {
            PriceMeCommon.Data.PriceRange priceRange = null;
            CategoryCache category = GetCategoryByCategoryID(categoryID);

            if (category != null && priceRangeNumber > 0 && priceRangeNumber < 7 && category.MaxPrice != category.MinPrice && category.MaxPrice != 0)
            {
                switch (priceRangeNumber)
                {
                    case 1:
                        priceRange = new Data.PriceRange(0f, category.MinPrice * priceMeExchangeRate);
                        break;
                    case 6:
                        priceRange = new Data.PriceRange(category.MaxPrice * priceMeExchangeRate, float.MaxValue * priceMeExchangeRate);
                        break;
                    default:
                        float step = (category.MaxPrice * priceMeExchangeRate - category.MinPrice * priceMeExchangeRate) / 4f;
                        float min = category.MinPrice * priceMeExchangeRate + (priceRangeNumber - 2) * step;
                        float max = min + step;
                        priceRange = new Data.PriceRange(min, max);
                        break;
                }
            }
            return priceRange;
        }

        public static string[] GetAllSubCategoryIdString(int cid)
        {
            List<int> cidList = GetAllSubCategoryId(cid);
            string[] categoryIDs = new string[cidList.Count];

            for (int i = 0; i < cidList.Count; i++ )
            {
                categoryIDs[i] = cidList[i].ToString();
            }

            return categoryIDs;
        }

        public static List<int> GetAllSubCategoryId(int cid)
        {
            List<int> categoryIDs = new List<int>();

            List<CategoryCache> categoryCaches = GetNextLevelSubCategoriesIsActiveCache(cid);
            foreach (var cache in categoryCaches)
            {
                SetAllSubCategoryIds(categoryIDs, cache.CategoryID);
            }

            return categoryIDs;
        }

        private static void SetAllSubCategoryIds(List<int> categoryIDs, int cid)
        {
            categoryIDs.Add(cid);

            List<CategoryCache> categoryCaches = GetNextLevelSubCategoriesIsActiveCache(cid);
            foreach (var cache in categoryCaches)
            {
                SetAllSubCategoryIds(categoryIDs, cache.CategoryID);
            }
        }

        /// <summary>
        /// use lucene index
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static List<CategoryCache> GetAllSubCategory(int cid)
        {
            List<CategoryCache> categoryList = new List<CategoryCache>();
            string[] subCategoryIDs = SearchController.GetSubCategoryIDs(cid);
            foreach (string subCID in subCategoryIDs)
            {
                int _cid = int.Parse(subCID);
                if (_cid == cid) continue;
                CategoryCache category = GetCategoryByCategoryID(_cid);
                if (category != null)
                {
                    categoryList.Add(category);
                }
            }
            return categoryList;
        }

        public static List<CategoryCache> GetBreadCrumbCategoryList(CategoryCache category)
        {
            List<CategoryCache> categoryList = new List<CategoryCache>();
            GetBreadCrumbCategoryList(category, categoryList);
            return categoryList;
        }

        static void GetBreadCrumbCategoryList(CategoryCache category, List<CategoryCache> categoryList)
        {
            categoryList.Add(category);
            if (category.ParentID != 0)
            {
                CategoryCache parentCategory = PriceMeCommon.CategoryController.GetCategoryByCategoryID(category.ParentID);
                GetBreadCrumbCategoryList(parentCategory, categoryList);
            }
        }

        public static List<CatalogSitemapCategory> GetCatalogSitemapCategories(int categoryID, bool includeAccessories)
        {
            if (categoryID == 0)
                return null;

            List<CatalogSitemapCategory> CatalogSitemapCategories = new List<CatalogSitemapCategory>();
            //CategoryCache category = CategoryController.GetCategoryByCategoryID(categoryID);
            List<CategoryCache> categories = CategoryController.GetNextLevelSubCategoriesIsActive(categoryID);
            categories = categories.OrderByDescending(c => c.Clicks).ToList();

            foreach (CategoryCache c in categories)
            {
                if (!categoryProductCount.ContainsKey(c.CategoryID) || categoryProductCount[c.CategoryID] == 0)
                {
                    continue;
                }
                List<CategoryCache> subCategories = CategoryController.GetNextLevelSubCategoriesIsActive(c.CategoryID);
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
                    if (categoryProductCount.ContainsKey(_c.CategoryID) && categoryProductCount[_c.CategoryID] > 0)
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

        static object GetMaxPriceStringLock = new object();

        public static string GetMaxPriceString(int categoryID)
        {
            lock (GetMaxPriceStringLock)
            {
                if (!CategoryMaxPriceDic.ContainsKey(categoryID))
                {
                    ProductSearcher ps = new ProductSearcher(categoryID, null, null, new List<int>(), new List<int>(), "bestprice-rev", "", null, true, ConfigAppString.CountryID, false, true);
                    if (ps.GetProductCount() > 0)
                    {
                        SearchResult sr = ps.GetSearchResult(1, 1);
                        CategoryMaxPriceDic.Add(categoryID, sr.ProductCatalogList[0].BestPrice);
                    }
                    else
                    {
                        CategoryMaxPriceDic.Add(categoryID, "0");
                    }
                }
                return CategoryMaxPriceDic[categoryID];
            }
        }

        public static bool IsDisplayBrands(CategoryCache category)
        {
            return !CategoryController.IsHiddenBrandsCategoryID(category.CategoryID) || category.IsFilterByBrand;
        }
    }
}