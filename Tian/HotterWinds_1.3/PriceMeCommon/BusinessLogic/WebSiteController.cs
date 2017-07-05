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
    /// <summary>
    /// 依赖CategoryController.Load()
    /// </summary>
    public static class WebSiteController
    {
        public static List<FeaturedTabCache> FeaturedProducts_Static { get; private set; }
        public static List<MostPopularCategory> MostPopularCategories_Static { get; private set; }

        public static void Load(int countryId)
        {
            VelocityController vc = MultiCountryController.GetVelocityController(countryId);
            FeaturedProducts_Static = GetFeaturedProducts(countryId, vc);

            MostPopularCategories_Static = vc.GetCache<List<MostPopularCategory>>(VelocityCacheKey.MostPopularCategories);
            if (MostPopularCategories_Static == null)
            {
                LogController.WriteLog("mostPopularCategories no velocity");
            }
            else
            {
                LogController.WriteLog("mostPopularCategories count : " + MostPopularCategories_Static.Count);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="vc"></param>
        /// <returns></returns>
        public static List<FeaturedTabCache> GetFeaturedProducts(int countryId, VelocityController vc)
        {
            List<FeaturedTabCache> featuredProducts = null;

            if (vc != null)
            {
                featuredProducts = vc.GetCache<List<FeaturedTabCache>>(VelocityCacheKey.FeaturedProducts);
            }

            if (featuredProducts == null)
            {
                featuredProducts = new List<FeaturedTabCache>();

                string selectSql = "SELECT [CategoryID],[Label],[Title],[ListOrder] FROM [dbo].[CSK_Store_FeaturedTab]";
                string connString = MultiCountryController.GetDBConnectionString(countryId);
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    sqlConn.Open();
                    using (SqlCommand sqlCMD = new SqlCommand(selectSql, sqlConn))
                    {
                        using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
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
                        string selectFeaturedProductsSql = "CSK_Store_FeatureProducts";
                        List<FeaturedProduct> featuredProductList = new List<FeaturedProduct>();
                        using (SqlCommand sqlCMD = new SqlCommand(selectFeaturedProductsSql, sqlConn))
                        {
                            sqlCMD.CommandTimeout = 0;
                            sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                            SqlParameter cIdParamter = new SqlParameter("@CATID", ft.CategoryID);
                            SqlParameter countryIdParamter = new SqlParameter("@COUNTRYID", countryId);
                            sqlCMD.Parameters.Add(cIdParamter);
                            sqlCMD.Parameters.Add(countryIdParamter);

                            using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                            {
                                while (sqlDR.Read())
                                {
                                    FeaturedProduct featuredProduct = new FeaturedProduct();
                                    featuredProduct.ProductID = int.Parse(sqlDR["ProductID"].ToString());
                                    featuredProduct.ProductName = sqlDR["ProductName"].ToString();
                                    featuredProduct.DefaultImage = sqlDR["DefaultImage"].ToString();
                                    featuredProduct.CategoryID = int.Parse(sqlDR["CategoryID"].ToString());
                                    featuredProduct.RootID = int.Parse(sqlDR["RootID"].ToString());
                                    featuredProduct.MaxPrice = double.Parse(sqlDR["MaxPrice"].ToString());
                                    featuredProduct.MinPrice = double.Parse(sqlDR["MinPrice"].ToString());
                                    //featuredProduct.ProductGUID = sqlDR["ProductGUID"].ToString();
                                    featuredProductList.Add(featuredProduct);
                                }
                            }

                            var cate = CategoryController.GetCategoryByCategoryID(ft.CategoryID, countryId);
                            if (cate != null)
                            {
                                ft.Label = cate.CategoryName;
                            }

                            ft.FeaturedProductList = featuredProductList;
                        }
                    }
                }

                LogController.WriteLog("Country: " + countryId + " FeaturedProducts no velocity");
            }

            return featuredProducts;
        }

        public static Dictionary<int, List<MostPopularCategory>> GetDatas(int countryId)
        {
            Dictionary<int, List<MostPopularCategory>> datas = new Dictionary<int, List<MostPopularCategory>>();
            if (MostPopularCategories_Static != null)
            {
                MostPopularCategories_Static.ForEach(mc =>
                {
                    if (!datas.ContainsKey(mc.RootID))
                    {
                        datas.Add(mc.RootID, new List<MostPopularCategory>());
                    }
                    datas[mc.RootID].Add(mc);
                });
            }
            else
            {
                int[] popularCategoriesIDList = new int[] { 1, 365, 189, 6, 355, 1284, 184, 616 };

                foreach (int cid in popularCategoriesIDList)
                {
                    List<MostPopularCategory> mostPopularCategoryList = new List<MostPopularCategory>();
                    var category = CategoryController.GetCategoryByCategoryID(cid, countryId);
                    if (category != null)
                    {
                        ProductSearcher productSearcher = new ProductSearcher("", cid, null, null, null, null, "clicks", null, SearchController.MaxSearchCount_Static, countryId, false, false, false, true, null, "", null);
                        var searchResult = productSearcher.GetCatalogCategoryResulte();

                        if (searchResult.NarrowItemList.Count > 0)
                        {
                            foreach (var ni in searchResult.NarrowItemList)
                            {
                                MostPopularCategory mostPopularCategory = new MostPopularCategory();
                                mostPopularCategory.CategoryID = int.Parse(ni.Value);
                                mostPopularCategoryList.Add(mostPopularCategory);
                                if (mostPopularCategoryList.Count >= 5)
                                {
                                    break;
                                }
                            }
                        }
                        datas.Add(cid, mostPopularCategoryList);
                    }
                }
            }
            return datas;
        }
    }
}