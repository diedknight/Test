using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Data;
using System.Data;
using SubSonic;
using SubSonic.Schema;
using SubSonic.Extensions;
using PriceMeCache;
using ProductCatalog = PriceMeCommon.Data.ProductCatalog;
using System.Data.SqlClient;

namespace IndexBuildCommon
{
    public static class CacheBuilder
    {
        static VelocityController Static_VelocityController;

        public static void BuildCache()
        {
            Static_VelocityController = MultiCountryController.GetVelocityController(AppValue.CountryId);
            Static_VelocityController.TryCreateRegion();
            
            if (Static_VelocityController != null && Static_VelocityController.IsCacheServerOK)
            {
                LogController.WriteLog("Start Build Cache...... At :" + DateTime.Now.ToString());

                try
                {
                    CategoryController.LoadForBuildIndex();
                    BuildCategoryCache();
                }
                catch (Exception ex)
                {
                    LogController.WriteException(ex.Message + "---" + ex.StackTrace + " At :" + DateTime.Now.ToString());
                }

                try
                {
                    AttributesController.LoadForBuildIndex();
                    BuildAttributeCache();
                }
                catch (Exception ex)
                {
                    LogController.WriteException(ex.Message + "---" + ex.StackTrace + " At :" + DateTime.Now.ToString());
                }

                try
                {
                    BuildRetailerCache();
                }
                catch (Exception ex)
                {
                    LogController.WriteException(ex.Message + "---" + ex.StackTrace + " At :" + DateTime.Now.ToString());
                }

                try
                {
                    LogController.WriteLog("Start BuildOtherCache()...... At :" + DateTime.Now.ToString());
                    BuildOtherCache();
                }
                catch (Exception ex)
                {
                    LogController.WriteException(ex.Message + "---" + ex.StackTrace + " At :" + DateTime.Now.ToString());
                }

                LogController.WriteLog("Build Cache finished...... At :" + DateTime.Now.ToString());
            }
        }

        private static void BuildCategoryCache()
        {
            LogController.WriteLog("Index path：" + MultiCountryController.GetCurrentIndexPath(AppValue.CountryId));
            LogController.WriteLog("Start Build Category Cache At :" + DateTime.Now.ToString());

            LogController.WriteLog("---Build " + VelocityCacheKey.CategoryByNameWithClicks + " At :" + DateTime.Now.ToString());
            List<CategoryCache> ccList = CategoryController.GetCatOrderByNameOrderByNameWithClicks(AppValue.CountryId, null);
            if (ccList.Count > 0)
            {
                PutCache(VelocityCacheKey.CategoryByNameWithClicks, ccList);
            }
            LogController.WriteLog("VelocityCacheKey.CategoryByNameWithClicks : " + ccList.Count);

            LogController.WriteLog("---Build " + VelocityCacheKey.MostPopularCategories + " At :" + DateTime.Now.ToString());
            List<MostPopularCategory> mostCategoriesListSort = GetMostPopularCategories();
            if (mostCategoriesListSort.Count > 0)
            {
                PutCache(VelocityCacheKey.MostPopularCategories, mostCategoriesListSort);
            }
            LogController.WriteLog("MostCategoriesListSort count : " + mostCategoriesListSort.Count);

            LogController.WriteLog("End Build Category Cache At :" + DateTime.Now.ToString());
        }

        private static void BuildAttributeCache()
        {
            LogController.WriteLog("Start Build Attributes Cache At :" + DateTime.Now.ToString());

            Dictionary<int, List<AttributeGroup>> attGroupDic = AttributesController.GetAttGroupDic(AppValue.CountryId, null);
            if (attGroupDic.Count > 0)
            {
                PutCache(VelocityCacheKey.AllCategoryAttributeGroup, attGroupDic);
            }
            LogController.WriteLog("AllCategoryAttributeGroup count : " + attGroupDic.Count);

            LogController.WriteLog("End Build Attributes Cache At :" + DateTime.Now.ToString());
        }

        private static void BuildRetailerCache()
        {
            LogController.WriteLog("Start BuildRetailerCache()...... At :" + DateTime.Now.ToString());

            List<RetailerCache> rcList = RetailerController.GetRetailersWithVotesSumOrderByClicksFromDB(AppValue.CountryId);
            if (rcList.Count > 0)
            {
                PutCache(VelocityCacheKey.RetailerListOrderByNameWithClicks, rcList);
            }
            LogController.WriteLog("RetailerListOrderByNameWithClicks count : " + rcList.Count);

            LogController.WriteLog("End Build BuildRetailerCache() Cache At :" + DateTime.Now.ToString());
        }

        private static void BuildOtherCache()
        {
            //status bar date
            LogController.WriteLog("Start Build statusBarDate At :" + DateTime.Now.ToString());
            Dictionary<string, string> statusBarDate = GetStatusBarData();
            PutCache(VelocityCacheKey.StatusBarInfo, statusBarDate);
            LogController.WriteLog("Start Build statusBarDate At :" + DateTime.Now.ToString());

            //by HuangRiLing，2013-01-10
            LogController.WriteLog("Start Build RetailerReviewList Cache At :" + DateTime.Now.ToString());
            List<RetailerReviewCache> RetailerReviewList = GetAllRetailerReviewList(AppValue.CountryId);
            PutCache(VelocityCacheKey.RetailerReviewList, RetailerReviewList);
            LogController.WriteLog("Velocity RetailerReviewList Count : " + RetailerReviewList.Count);
            LogController.WriteLog("End Build RetailerReviewList !");

            //TopNRetailerTrackProducts
            LogController.WriteLog("Start Build TopNRetailerTrackProducts Cache At :" + DateTime.Now.ToString());
            List<MostPopularProduct> TopNRetailerTrackProductsList = GetTopNRetailerTrackProducts(5);
            PutCache(VelocityCacheKey.TopNRetailerTrackProducts, TopNRetailerTrackProductsList);
            LogController.WriteLog("End Build TopNRetailerTrackProducts !");

            LogController.WriteLog("Start Build All FeaturedProduct Cache At :" + DateTime.Now.ToString());
            List<FeaturedTabCache> featuredProducts = GetAllFeaturedProducts();
            LogController.WriteLog("FeaturedProduct count : " + featuredProducts.Count);
            PutCache(VelocityCacheKey.FeaturedProducts, featuredProducts);
            LogController.WriteLog("End Build All FeaturedProduct!");

            //EnergyImgs
            LogController.WriteLog("Start Build All EnergyImgs Cache At :" + DateTime.Now.ToString());
            Dictionary<int, string> energyImgsDic = GetEnergyImgs();
            LogController.WriteLog("FeaturedProduct count : " + energyImgsDic.Count);
            PutCache(VelocityCacheKey.EnergyImgs, energyImgsDic);
            LogController.WriteLog("End Build All EnergyImgs!");

            //ProductVariants
            LogController.WriteLog("Start Build All ProductVariants Cache At :" + DateTime.Now.ToString());
            ProductController.LoadVariants(null, AppValue.CountryId);
            Dictionary<string, List<ProductVariants>> pvs = null;
            if (ProductController.dicVariants.TryGetValue(AppValue.CountryId, out pvs))
            {
                LogController.WriteLog("ProductVariants count : " + pvs.Count);
                PutCache(VelocityCacheKey.ProductVariants, pvs);
                LogController.WriteLog("End Build All ProductVariants!");
            }
            else
            {
                LogController.WriteLog("ProductVariants count : 0.");
            }
        }

        public static Dictionary<int, string> GetEnergyImgs()
        {
            Dictionary<int, string> energyImgsDic = new Dictionary<int, string>();
            string sql = "select ProductId, EnergyImage from CSK_Store_Energy "
                    + "where ProductId is not null And ProductId != ''";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandTimeout = 0;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                string pidString = dr["ProductId"].ToString();
                string image = dr["EnergyImage"].ToString();
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
            dr.Close();

            return energyImgsDic;
        }

        private static Dictionary<string, string> GetStatusBarData()
        {
            Dictionary<string, string> statusBarData = new Dictionary<string, string>();

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand("CSK_Store_12RMB_GetStatusBarData", sqlConn))
                {
                    sqlConn.Open();
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCMD.CommandTimeout = 0;
                    SqlParameter countryIdParam = new SqlParameter("@countryID", AppValue.CountryId);
                    sqlCMD.Parameters.Add(countryIdParam);

                    using (IDataReader idr = sqlCMD.ExecuteReader())
                    {
                        if (idr.Read())
                        {
                            statusBarData.Add(idr.GetName(0), idr[0].ToString());
                            statusBarData.Add(idr.GetName(1), idr[1].ToString());
                            statusBarData.Add(idr.GetName(2), idr[2].ToString());
                            statusBarData.Add(idr.GetName(3), idr[3].ToString());
                        }
                    }
                }
            }

            return statusBarData;
        }

        private static List<int> GetProductUserReviewProductID(Dictionary<int, PriceMeCommon.Data.ProductVotesSum> productVotesSumDictionary)
        {
            List<int> productIDList = new List<int>();

            foreach (int pid in productVotesSumDictionary.Keys)
            {
                if (!productIDList.Contains(pid))
                    productIDList.Add(pid);
            }

            return productIDList;
        }

        private static List<MostPopularCategory> GetMostPopularCategories()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand("CSK_Store_MostPopularCategoriesByTime", sqlConn))
                {
                    sqlConn.Open();
                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlCMD.CommandTimeout = 0;
                    SqlParameter countryIdParam = new SqlParameter("@COUNTRY", AppValue.CountryId);
                    sqlCMD.Parameters.Add(countryIdParam);

                    using (IDataReader idr = sqlCMD.ExecuteReader())
                    {
                        List<MostPopularCategory> list = new List<MostPopularCategory>();
                        while (idr.Read())
                        {
                            MostPopularCategory mostPopularCategory = new MostPopularCategory();
                            string rn = idr["RN"].ToString();
                            string cnt = idr["CNT"].ToString();
                            string categoryId = idr["CategoryID"].ToString();
                            string rootId = idr["RootID"].ToString();

                            mostPopularCategory.Rn = int.Parse(rn);
                            mostPopularCategory.Cnt = int.Parse(cnt);
                            mostPopularCategory.CategoryID = int.Parse(categoryId);
                            mostPopularCategory.RootID = int.Parse(rootId);
                            list.Add(mostPopularCategory);
                        }
                        return list;
                    }
                }
            }
        }

        public static void PutCache(VelocityCacheKey key, object cache)
        {
            Static_VelocityController.PutCache(key, cache);
        }

        private static List<FeaturedTabCache> GetAllFeaturedProducts()
        {
            List<FeaturedTabCache> featuredTabCacheList = WebSiteController.GetFeaturedProducts(AppValue.CountryId, null);
            return featuredTabCacheList;
        }

        public static List<RetailerReviewCache> GetAllRetailerReviewList(int countryID)
        {
            List<RetailerReviewCache> _retailerReviewList = new List<RetailerReviewCache>();

            try
            {
                int ii = 0;
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand())
                    {
                        sqlCmd.Connection = conn;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandText = "GetRetailerReviewByCountryID";
                        sqlCmd.CommandTimeout = 0;

                        SqlParameter sqlParameter = new SqlParameter("@countryID", SqlDbType.Int);
                        sqlParameter.SqlValue = countryID;
                        sqlCmd.Parameters.Add(sqlParameter);

                        conn.Open();
                        using (SqlDataReader sqlDR = sqlCmd.ExecuteReader())
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
                }

                List<RetailerReviewCache> rrcList = new List<RetailerReviewCache>();
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand())
                    {
                        sqlCmd.Connection = conn;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandText = "GetRetailerReviewDetailByCountryID";
                        sqlCmd.CommandTimeout = 0;

                        SqlParameter sqlParameter = new SqlParameter("@countryID", SqlDbType.Int);
                        sqlParameter.SqlValue = countryID;
                        sqlCmd.Parameters.Add(sqlParameter);

                        conn.Open();
                        using (SqlDataReader sqlDR = sqlCmd.ExecuteReader())
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
                }
                _retailerReviewList.AddRange(rrcList);
                _retailerReviewList = _retailerReviewList.OrderByDescending(r => r.CreatedOn).ToList();

            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + ex.StackTrace);
            }

            return _retailerReviewList;
        }

        private static List<MostPopularProduct> GetTopNRetailerTrackProducts(int count)
        {
            List<MostPopularProduct> pcs = new List<MostPopularProduct>();
            DateTime ds = DateTime.Now.AddDays(-30), de = DateTime.Now.AddDays(-1);

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (var sqlConn = new SqlConnection(connString))
            {
                using (var sqlCMD = new SqlCommand("CSK_Store_TopNRetailerTrackProducts", sqlConn))
                {
                    sqlConn.Open();
                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlCMD.CommandTimeout = 0;
                    SqlParameter bdParam = new SqlParameter("@BEGIN_DATE", ds);
                    SqlParameter edParam = new SqlParameter("@END_DATE", de);
                    SqlParameter countParam = new SqlParameter("@COUNT", 15);
                    SqlParameter countryIdParam = new SqlParameter("@COUNTRY", AppValue.CountryId);
                    sqlCMD.Parameters.Add(bdParam);
                    sqlCMD.Parameters.Add(edParam);
                    sqlCMD.Parameters.Add(countParam);
                    sqlCMD.Parameters.Add(countryIdParam);

                    List<MostPopularProduct> mppList = new List<MostPopularProduct>();

                    using (var idr = sqlCMD.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            MostPopularProduct mostPopularProduct = new MostPopularProduct();

                            string cnt = idr["CNT"].ToString();
                            string productID = idr["ProductID"].ToString();
                            string defaultImage = idr["DefaultImage"].ToString();
                            string productName = idr["ProductName"].ToString();
                            string bestPrice = idr["BestPrice"].ToString();

                            double bp = 0;
                            if (double.TryParse(bestPrice, out bp))
                            {
                                if (bp > 0)
                                {
                                    mostPopularProduct.Cnt = int.Parse(cnt);
                                    mostPopularProduct.ProductID = int.Parse(productID);
                                    mostPopularProduct.DefaultImage = defaultImage;
                                    mostPopularProduct.ProductName = productName;
                                    mostPopularProduct.BestPrice = bp;
                                    mppList.Add(mostPopularProduct);
                                    if (mppList.Count == count)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return pcs;
        }
    }
}