using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using PriceMeCommon.Data;
using PriceMeCache;
using System.Data;
using SubSonic.Schema;
using PriceMeCommon.Extend;
using Lucene.Net.Documents;
using System.Collections;
using PriceMeCommon.DBTableInfo;
using System.Data.SqlClient;

namespace PriceMeCommon.BusinessLogic
{
    public class ProductController
    {

        private static PriceMeDBDB db = PriceMeStatic.PriceMeDB;
        private static Dictionary<int, ReviewAverage> ras;
        public static List<NewExpertReview> newERList;
        public static Dictionary<int, ExpertReviewSource> ra;
        public static Dictionary<int, string> energyImgs;
        public static Dictionary<int, List<ProductVideo>> proVideoDic;
        public static Dictionary<int, RetailerProductCondition> rpConditionDic;

        private static List<PriceMeCache.MostPopularProduct> _top5RetailerTrackProduct;
        public static List<PriceMeCache.MostPopularProduct> Top5RetailerTrackProduct
        {
            get { return _top5RetailerTrackProduct; }
        }
        public static List<PriceMeCache.TreepodiaVideo> TreepodiaVideoList;
        public static List<HotProduct> HotProducts;
        public static List<FavouriteProductData> listFavouriteProduct { get; set; }

        //SubSonic Columns
        private static readonly string[] productCol = new string[] { "ProductId", "ProductName", "ManufacturerID", "CategoryID", "DefaultImage", "ProductAttributeText", "ShoppingProductID", "Length", "Width", "Height", "Weight", "UnitOfMeasure", "CreatedOn" };
        private static readonly string[] productDesCol = new string[] { "ProductId", "ProductName", "ManufacturerID", "CategoryID", "DefaultImage", "ProductAttributeText", "ShortDescriptionZH", "ShoppingProductID", "Length", "Width", "Height", "Weight", "UnitOfMeasure", "LongDescriptionEN", "CreatedOn" };

        public static void Load()
        {
            Load(null);
        }

        public static void Load(Timer.DKTimer dkTimer)
        {
            if (dkTimer != null)
            {
                dkTimer.Set("ProductController.Load() --- Befor load GetAllReviewAverage");
            }
            ras = ReviewsSearch.GetAllReviewAverage();

            if (dkTimer != null)
                dkTimer.Set("ProductController.Load() --- Befor load GetAllProductVideo");
            proVideoDic = ReviewsSearch.GetAllProductVideo();

            //if (dkTimer != null)
            //{
            //    dkTimer.Set("ProductController.Load() --- Befor load GetNewExpertReview");
            //}
            //GetNewExpertReview();
            if (dkTimer != null)
            {
                dkTimer.Set("ProductController.Load() --- Befor load GetExpertReviewSources");
            }
            GetExpertReviewSources();
            if (dkTimer != null)
            {
                dkTimer.Set("ProductController.Load() --- Befor load GetEnergyImgs");
            }
            GetEnergyImgs();

            if (dkTimer != null)
            {
                dkTimer.Set("ProductController.Load() --- Befor load TopNRetailerTrackProducts");
            }
            //_top5RetailerTrackProduct = VelocityController.GetCache<List<PriceMeCache.MostPopularProduct>>(VelocityCacheKey.TopNRetailerTrackProducts);
            //if (_top5RetailerTrackProduct == null)
            //{
            _top5RetailerTrackProduct = GetTopNRetailerTrackProducts();
            HotProducts = GetTopNHotProducts();
            //    LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "Top5RetailerTrackProduct no velocity");
            //}
            //Retailer Product Condition
            BindRetailerProductCondition();

            if (dkTimer != null)
            {
                dkTimer.Set("ProductController.Load() --- Befor load GetTreepodiaVideos");
            }
            GetTreepodiaVideos();

            BindFavouriteProduct();
        }

        private static void BindFavouriteProduct()
        {
            listFavouriteProduct = new List<FavouriteProductData>();
            string sql = "select * from ProductFavourites where productId > 0 order by id desc";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text; 
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int pid = 0;
                int.TryParse(dr["productID"].ToString(), out pid);
                string tokenID = dr["tokenID"].ToString();
                FavouriteProductData fp = new FavouriteProductData();
                fp.ProductId = pid;
                fp.TokenId = tokenID;
                listFavouriteProduct.Add(fp);
            }
            dr.Close();
        }

        public static bool GetTreepodiaVideoBypid(int pid)
        {
            List<TreepodiaVideo> tvs = TreepodiaVideoList.Where(tv => tv.ProductID == pid).ToList();
            if (tvs != null && tvs.Count > 0 && ConfigAppString.CountryID==3)
                return true;
            else
                return false;
        }

        //public static bool HasGmap(List<int> retailerIDs)
        //{
        //    bool has = false;
            
        //    foreach (int rid in retailerIDs)
        //    {
        //        PriceMeCache.RetailerCache retailer = RetailerController.GetRetailerByID(rid);
        //        if (retailer == null || retailer.RetailerId == 0) continue;
        //        string isShow = "true";

        //        if (retailer.StoreType == 2)
        //            isShow = "false";
        //        if (isShow == "false") continue;

        //        List<GLatLngCache> glats;
        //        RetailerController.GLatLngCacheDictionary.TryGetValue(rid, out glats);
        //        if (glats != null && glats.Count > 0) has = true;
        //    }
        //    return has;
        //}

        private static void BindRetailerProductCondition()
        {
            rpConditionDic = new Dictionary<int, RetailerProductCondition>();
            string sql = "Select RetailerProductConditionId, ConditionName, ConditionDescription From CSK_Store_RetailerProductCondition";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int id = 0;
                int.TryParse(dr["RetailerProductConditionId"].ToString(), out id);
                string name = dr["ConditionName"].ToString();
                string description = dr["ConditionDescription"].ToString();

                RetailerProductCondition cond = new RetailerProductCondition();
                cond.Id = id;
                cond.ConditionName = name;
                cond.ConditionDescription = description;

                if (!rpConditionDic.ContainsKey(id))
                    rpConditionDic.Add(id, cond);
            }
        }

        public static RetailerProductCondition GetRetailerProductCondition(int key)
        {
            if (key != 0 && rpConditionDic.ContainsKey(key))
                return rpConditionDic[key];
            else
                return null;
        }

        private static void GetTreepodiaVideos()
        {
            //TreepodiaVideoList = VelocityController.GetCache<List<TreepodiaVideo>>(VelocityCacheKey.TreepodiaVideos);
            //if (TreepodiaVideoList == null)
            //{
                TreepodiaVideoList = new List<TreepodiaVideo>();
                string sql = "select * from CSK_Store_TreepodiaVideo";
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandTimeout = 0;
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    TreepodiaVideo tv = new TreepodiaVideo();
                    tv.ID = int.Parse(dr["id"].ToString());
                    tv.ProductID = int.Parse(dr["productid"].ToString());
                    TreepodiaVideoList.Add(tv);
                }
                dr.Close();
                //LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "TreepodiaVideo no velocity");
          //  }
        }

        private static List<PriceMeCache.MostPopularProduct> GetTopNRetailerTrackProducts()
        {
            List<PriceMeCache.MostPopularProduct> pcs = new List<PriceMeCache.MostPopularProduct>();
            //DateTime ds = DateTime.Now.AddDays(-30), de = DateTime.Now.AddDays(-1);

            //StoredProcedure sp = PriceMeStatic.PriceMeDB.CSK_Store_TopNRetailerTrackProducts();
            //sp.Command.Parameters.Add("@BEGIN_DATE", ds, System.Data.DbType.DateTime);
            //sp.Command.Parameters.Add("@END_DATE", de, System.Data.DbType.DateTime);
            //sp.Command.Parameters.Add("@COUNT", 5, System.Data.DbType.Int32);
            //sp.Command.Parameters.Add("@COUNTRY", ConfigAppString.CountryID, System.Data.DbType.Int32);

            //pcs = sp.ExecuteTypedList<PriceMeCache.MostPopularProduct>();
            //pcs = pcs.Take(5).Where(l => l.BestPrice > 0).ToList();

            ProductSearcher productSeacher = new ProductSearcher(0, null, null, null, null, "Clicks", null, null, true, ConfigAppString.CountryID, false, true);
            SearchResult sr = productSeacher.GetSearchResult(1, 5);

            foreach (var pc in sr.ProductCatalogList)
            {
                PriceMeCache.MostPopularProduct mpp = new PriceMeCache.MostPopularProduct();
                mpp.BestPrice = double.Parse(pc.BestPrice);
                mpp.DefaultImage = pc.DefaultImage;
                mpp.ProductID = int.Parse(pc.ProductID);
                mpp.ProductName = pc.ProductName;

                pcs.Add(mpp);
            }

            return pcs;
        }


        public static List<HotProduct> GetTopNHotProducts()
        {
            List<HotProduct> hotProducts = new List<HotProduct>();
            try
            {
                var pids = new List<int>();

                #region get hotProducts from [dbo].[HotProducts]

                List<HotProduct> hotProducts1 = new List<HotProduct>();
                string connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
                var sql = string.Format("SELECT * FROM [dbo].[HotProducts] ORDER BY DisplayOrder");
                using (SqlConnection conn = new SqlConnection(connectionStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HotProduct p = new HotProduct();
                        p.ProductID = int.Parse(reader["ProductId"].ToString());
                        p.DisplayName = reader["DisplayName"].ToString();
                        p.DisplayOrder = int.Parse(reader["DisplayOrder"].ToString());

                        pids.Add(p.ProductID);
                        hotProducts1.Add(p);
                    }
                    reader.Close();

                    conn.Close();
                }

                #endregion

                #region hotProducts from priceme.CSK_Store_RetailerProduct

                List<HotProduct> hotProducts2 = new List<HotProduct>();

                var sql2 = @"SELECT p.ProductId,p.ProductName, MIN(RetailerPrice) Price, p.DefaultImage FROM CSK_Store_Product p
INNER JOIN CSK_Store_RetailerProduct rp  ON p.ProductID = rp.ProductId WHERE p.ProductId IN({0}) 
AND rp.RetailerProductStatus = 1 GROUP BY p.ProductId,p.ProductName, p.DefaultImage";
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = string.Format(sql2, string.Join(",", pids));
                //sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;

                var reader2 = sp.ExecuteReader();
                while (reader2.Read())
                {
                    HotProduct p = new HotProduct();
                    p.ProductID = int.Parse(reader2["ProductId"].ToString());
                    p.BestPrice = double.Parse(reader2["Price"].ToString());
                    p.ProductName = reader2["ProductName"].ToString();
                    p.DefaultImage = reader2["DefaultImage"].ToString();

                    hotProducts2.Add(p);
                }
                reader2.Close();

                #endregion

                #region filter have no rp product 
                foreach (var p2 in hotProducts2)
                {
                    var p1 = hotProducts1.SingleOrDefault(p => p.ProductID == p2.ProductID);
                    if (p1 != null)
                    {
                        HotProduct hp = new HotProduct();
                        hp.ProductID = p1.ProductID;
                        hp.DisplayName = p1.DisplayName;
                        hp.DisplayOrder = p1.DisplayOrder;
                        hp.BestPrice = p2.BestPrice;
                        hp.ProductName = p2.ProductName;
                        hp.DefaultImage = p2.DefaultImage;

                        hotProducts.Add(hp);
                    }
                }
                hotProducts = hotProducts.OrderBy(p => p.DisplayOrder).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            return hotProducts;
        }

        private static void GetEnergyImgs()
        {
            energyImgs = VelocityController.GetCache<Dictionary<int, string>>(VelocityCacheKey.EnergyImgs);
            if (energyImgs == null)
            {
                energyImgs = new Dictionary<int, string>();
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
                            if (!energyImgs.ContainsKey(pid))
                                energyImgs.Add(pid, image);
                        }
                    }
                    else
                    {
                        int pid = 0;
                        int.TryParse(pidString, out pid);
                        if (!energyImgs.ContainsKey(pid))
                            energyImgs.Add(pid, image);
                    }
                }
                dr.Close();
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "EnergyImgs no velocity");
            }
        }

        public static CSK_Store_Product GetProduct(int productID)
        {
            return CSK_Store_Product.SingleOrDefault(p => p.ProductID == productID);
        }

        public static CSK_Store_ProductNew GetProductHistory(int productID)
        {
            CSK_Store_Product pn = (new SubSonic.Query.Select(productCol).From("CSK_Store_Product").Where("ProductID").In(productID)).ExecuteSingle<CSK_Store_Product>();
            if (pn != null)
            {
                CSK_Store_ProductNew product = new CSK_Store_ProductNew();
                product.ProductID = pn.ProductID;
                product.ProductName = pn.ProductName;
                product.CategoryID = pn.CategoryID;
                product.ManufacturerID = pn.ManufacturerID;
                product.DefaultImage = pn.DefaultImage;
                product.ShortDescriptionZH = pn.ShortDescriptionZH;
                product.ProductAttributeText = pn.ProductAttributeText;
                product.CreatedOn = pn.CreatedOn;
                return product;
            }
            return null;
        }

        public static CSK_Store_ProductNew GetProductDesHistory(int productID)
        {
            CSK_Store_Product pn = (new SubSonic.Query.Select(productDesCol).From("CSK_Store_Product").Where("ProductID").In(productID)).ExecuteSingle<CSK_Store_Product>();
            if (pn != null)
            {
                CSK_Store_ProductNew product = new CSK_Store_ProductNew();
                product.ProductID = pn.ProductID;
                product.ProductName = pn.ProductName;
                product.CategoryID = pn.CategoryID;
                product.ManufacturerID = pn.ManufacturerID;
                product.DefaultImage = pn.DefaultImage;
                product.ShortDescriptionZH = pn.ShortDescriptionZH;
                product.ProductAttributeText = pn.ProductAttributeText;
                product.Width = pn.Width;
                product.Weight = pn.Weight;
                product.Height = pn.Height;
                product.Length = pn.Length;
                product.UnitOfMeasure = pn.UnitOfMeasure;

                return product;
            }
            return null;
        }

        public static CSK_Store_ProductNew GetProductDesNew(int productID)
        {
            CSK_Store_ProductNew pn = (new SubSonic.Query.Select(productDesCol).From("CSK_Store_ProductNew").Where("ProductID").In(productID)).ExecuteSingle<CSK_Store_ProductNew>();

            return pn;
        }

        public static CSK_Store_ProductNew GetProductNew(int productID)
        {
            //CSK_Store_ProductNew pn = CSK_Store_ProductNew.SingleOrDefault(p => p.ProductID == productID);
            CSK_Store_ProductNew pn = (new SubSonic.Query.Select(productCol).From("CSK_Store_ProductNew").Where("ProductID").In(productID)).ExecuteSingle<CSK_Store_ProductNew>();

            return pn;
        }

        public static CSK_Store_Product GetProductIndex(int productID)
        {
            Data.ProductCatalog pc = SearchController.SearchProducts(productID);
            if (pc != null)
            {
                CSK_Store_Product product = new CSK_Store_Product();
                product.ProductID = int.Parse(pc.ProductID);
                product.ProductName = pc.ProductName;
                product.CategoryID = pc.CategoryID;
                product.ManufacturerID = int.Parse(pc.ManufacturerID);
                product.DefaultImage = pc.DefaultImage;
                product.ShortDescriptionZH = pc.ShortDescriptionZH;

                return product;
            }

            return null;
        }

        public static List<ExpertReview> GetExpertReviews(int productId, int sort, int pg, int starts)
        {
            return GetExpertReviews(productId, sort, pg, starts, 50);
        }

        public static List<ExpertReview> GetExpertReviews(int productId, int sort, int pg, int starts, int pageSize)
        {
            List<ExpertReview> ers = ReviewsSearch.GetSearchExpertReviewResult(sort, starts, pg, pageSize, productId);
            
            return ers;
        }

        public static List<ExpertReview> GetExpertReviewsForYahooDeals(int productId, int sort, int pg, int starts)
        {
            List<ExpertReview> ers = ReviewsSearch.GetSearchExpertReviewResultForYahooDeals(sort, starts, pg, 50, productId);

            return ers;
        }

        public static ExpertReview GetTestFreaksExpertReviews(int productId)
        {
            ExpertReview er = ReviewsSearch.GetExpertReviewsByProductIdSourceID(productId, 156);
            return er;
        }

        public static List<ExpertReview> GetExpertReviewsRatingCount(int productId)
        {
            List<ExpertReview> ers = ReviewsSearch.GetExpertReviewsRatingByProductId(productId);
            
            return ers;
        }

        public static ReviewAverage GetReviewAverage(int productId)
        {
            ReviewAverage ra = null;

            if (ras != null)
            {
                if (ras.ContainsKey(productId))
                    ra = ras[productId];
            }

            return ra;
        }

        public static void GetExpertReviewSources()
        {
            ra = (Dictionary<int, ExpertReviewSource>)VelocityController.GetCache(VelocityCacheKey.ExpertReviewSource);
            if (ra == null)
            {
                ra = new Dictionary<int, ExpertReviewSource>();

                SubSonic.Schema.StoredProcedure storedProcedure = PriceMeStatic.PriceMeDB.GetAllExpertReviewSourceTF();

                using (IDataReader idr = storedProcedure.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        ExpertReviewSource expertReviewSource = new ExpertReviewSource();
                        expertReviewSource.SourceID = int.Parse(idr["SourceID"].ToString());
                        expertReviewSource.WebSiteName = idr["WebSiteName"].ToString();
                        expertReviewSource.LogoFile = idr["LogoFile"].ToString();
                        expertReviewSource.CountryID = int.Parse(idr["CountryID"].ToString());
                        expertReviewSource.HomePage = idr["HomePage"].ToString();

                        if (!ra.ContainsKey(expertReviewSource.SourceID))
                            ra.Add(expertReviewSource.SourceID, expertReviewSource);
                    }
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "ExpertReviewSource no velocity");
            }
        }

        public static bool IsConsumer(int productID)
        {
            return ConsumerController.All.Contains(productID);
        }

        public static ProductRatingEntity GetAverageRating(int productID)
        {
            ProductRatingEntity pr = new ProductRatingEntity();
            ReviewAverage ra = GetReviewAverage(productID);

            if (ra != null)
            {
                pr.ReviewCount = ra.ReviewCount;
                pr.UserReviewCount = ra.UserReviewCount;
                pr.ExpertReviewCount = ra.ExpertReviewCount;
                if (ra.ProductRating > 0)
                    pr.ProductRating = ra.ProductRating;
                pr.FeatureScore = ra.FeatureScore;
                pr.TFERating = ra.TFEAverageRating;
                pr.TFURating = ra.TFUAverageRating;
            }

            return pr;
        }

        public static void GetNewExpertReview()
        {
            newERList = new List<NewExpertReview>();
            newERList = (List<NewExpertReview>)VelocityController.GetCache(VelocityCacheKey.NewExpertReview);
        }

        public static List<CSK_Store_ProductReview> GetUserReviews(int productID)
        {
            return CSK_Store_ProductReview.Find(r => r.ProductID == productID && r.IsApproved == true).ToList();
        }

        public static CSK_Store_ProductReview GetProductReview(int reviewID)
        {
            return CSK_Store_ProductReview.SingleOrDefault(pr => pr.ReviewID == reviewID);
        }

        public static List<CSK_Store_ProductReview> GetProductReviewByProductAndAuthor(int productID, string author)
        {
            return CSK_Store_ProductReview.Find(r => r.ProductID == productID && r.AuthorName == author).ToList();
        }

        public static List<CSK_Store_ProductList> GetProductList(int listID)
        {
            return CSK_Store_ProductList.Find(pl => pl.ListID == listID).ToList();
        }

        public static List<CSK_Store_ProductList> GetProductListByProductID(int productID)
        {
            return CSK_Store_ProductList.Find(pl => pl.ProductID == productID).ToList();
        }

        public static List<CSK_Store_ProductList> GetProductListByListIDAndProductID(int listID, int productID)
        {
            return CSK_Store_ProductList.Find(pl => pl.ListID == listID && pl.ProductID == productID).ToList();
        }

        public static CSK_Store_ReviewSource GetReviewSource(int sourceID)
        {
            return CSK_Store_ReviewSource.SingleOrDefault(s => s.SourceID == sourceID);
        }

        public static CSK_Util_Country GetReviewSourceCountry(int countryID)
        {
            return CSK_Util_Country.SingleOrDefault(c => c.countryID == countryID);
        }

        public static int GetNumberStores(int productID)
        {
            //CSK_Store_12RMB_Product_GetRetailerProductCount
            return CSK_Store_RetailerProduct.Find(rp => rp.ProductId == productID && rp.RetailerProductStatus == true).Count();
        }

        public static List<PriceMeCommon.Data.ProductCatalog> GetRelatedProductsByCategoryAndBrand(int cid, int mid)
        {

            //TODO 把内容从 Application 移到 静态变量里
            Dictionary<string, List<PriceMeCommon.Data.ProductCatalog>> similarCaches = new Dictionary<string, List<Data.ProductCatalog>>();
            //if (System.Web.HttpContext.Current.Application["SimilarCache"] == null) {
            //    similarCaches = new Dictionary<string, List<Data.ProductCatalog>>();
            //    System.Web.HttpContext.Current.Application["SimilarCache"] = similarCaches;
            //} else {
            //    similarCaches = System.Web.HttpContext.Current.Application["SimilarCache"] as Dictionary<string, List<Data.ProductCatalog>>;
            //}
            //string key = cid + "-" + mid;
            //if (similarCaches.ContainsKey(key)) {
            //    return similarCaches[key];
            //}

            CategoryCache category = CategoryController.GetCategoryByCategoryID(cid);
            List<Data.ProductCatalog> pcc = new List<Data.ProductCatalog>();
            List<int> brandIDs = new List<int>();
            if(mid > 0)
            {
                brandIDs.Add(mid);
            }
            HitsInfo hi = SearchController.SearchProducts("", cid, brandIDs, null, null, null, "", null, true, true, false, 1000, ConfigAppString.CountryID, false, true, true);
            if (hi == null) return pcc;

            int count = hi.ResultCount > 30 ? 30 : hi.ResultCount;

            for (int i = 0; i < count; i++)
            {
                Data.ProductCatalog pc = SearchController.GetProductCatalog(hi, i);
                pcc.Add(pc);
            }

            //if (!similarCaches.ContainsKey(key)) {
            //    similarCaches.Add(key, pcc);
            //}
            return pcc;

            #region old
            //Analyzer analyzer = new StandardAnalyzer();
            //BooleanQuery query = new BooleanQuery();
            //Query categoryQuery = new TermQuery(new Term("CategoryID", CategoryID.ToString()));
            //Query manuyQuery = new TermQuery(new Term("ManufacturerID", ManufacturerID.ToString()));
            //if (!category.IsDisplayIsMerged) {
            //    TermQuery mergeQuery = new TermQuery(new Term("IsMerge", "True"));
            //    query.Add(mergeQuery, BooleanClause.Occur.MUST);
            //}

            //query.Add(categoryQuery, BooleanClause.Occur.MUST);
            //query.Add(manuyQuery, BooleanClause.Occur.MUST);

            //Hits hits = null;
            //MultiSearcher multiSearcher = GetMultiSearcher(CategoryID);
            //if (multiSearcher == null) {
            //    return pcc;
            //}

            //hits = multiSearcher.Search(query);

            //for (int i = 0; i < hits.Length(); i++) {
            //    if (pcc.Count > 25)
            //        break;

            //    PriceMeCommon.Data.ProductCatalog pc = new Data.ProductCatalog();
            //    string bestP = hits.Doc(i).Get("BestPrice");
            //    bestP = string.IsNullOrEmpty(bestP) ? NumberTools.LongToString(0L) : bestP;
            //    pc.BestPrice = (NumberTools.StringToLong(bestP) / 10000D).ToString("0.00");
            //    pc.ProductName = hits.Doc(i).Get("ProductName");
            //    pc.ProductID = hits.Doc(i).Get("ProductID");
            //    pc.DefaultImage = hits.Doc(i).Get("DefaultImage");
            //    pcc.Add(pc);
            //}
            //if (!similarCaches.ContainsKey(key)) {
            //    similarCaches.Add(key, pcc);
            //}
            //return pcc;
            #endregion

        }

        public static void GetRetailerProducts(int productID, out decimal bestPrice, out decimal maxPrice, out bool singlePrice, out bool showFeatured, out List<CSK_Store_RetailerProductNew> retailerProducts, out int allprice, int flag, out bool isInternational, out decimal overseasPices)
        {
            bestPrice = 0;
            maxPrice = 0;
            singlePrice = false;
            showFeatured = false;

            retailerProducts = new List<CSK_Store_RetailerProductNew>();

            Hashtable allPPCMember = RetailerController.AllPPCMember;
            Hashtable allPPCNoLink = RetailerController.AllNoLinkPPCMember;
            List<int> isComplete = RetailerController.IsCompleteId;

            List<CSK_Store_RetailerProductNew> rps = RetailerProductController.GetRetailerProductsByProductId(productID);

            isInternational = RetailerProductController.CheckInternationalRetailerProducts(rps, out overseasPices);
            
            List<int> restrictedRetailer = new List<int>();

            int hour = DateTime.Now.Hour;
            for (int i = 0; i < rps.Count; i++)
            {
                CSK_Store_RetailerProductNew rp = rps[i];

                if (isComplete.Contains(rp.RetailerId))
                {
                    rps.RemoveAt(i);
                    i--;
                    continue;
                }
                else if (rp.RetailerId == 531 && rp.ExpirationDate < DateTime.Now)
                { 
                    rps.RemoveAt(i);
                    i--;
                    continue;
                }

                rp.ProductId = productID;
                rp.IsFeaturedProduct = false;

                if (allPPCMember.ContainsKey(rp.RetailerId))
                {
                    //CSK_Store_PPCMember ppcm = allPPCMember[rp.RetailerId] as CSK_Store_PPCMember;
                    rp.PPCMemberType = 2;
                    rp.IsNoLink = false;
                    //if (ppcm.PPCDropOff ?? false)
                    //{
                    //    if (ppcm.PPCDropOnTime <= hour && hour < ppcm.PPCTime)
                    //        rp.IsNoLink = false;
                    //    else
                    //        rp.IsNoLink = true;
                    //}
                }
                else
                {
                    rp.PPCMemberType = 5;
                    rp.IsNoLink = true;
                }
                //如果价格相同PPCMemberType = 2的在前面
                if (!rp.IsNoLink)
                {
                    rp.OrderbyProduct = rp.RetailerPrice - 0.01m;
                    rp.PPCOrderbyProduct = rp.RetailerPrice - 0.01m;
                }
                else
                {
                    rp.OrderbyProduct = rp.RetailerPrice;
                    rp.PPCOrderbyProduct = rp.RetailerPrice;
                }

                //临时推广合作
                //if (rp.RetailerId == 857)
                //{
                //    rp.IsFeaturedProduct = true;
                //}
            }

            rps = rps.OrderBy(rp => rp.OrderbyProduct).ToList();
           
            rps = rps.Where(rp => !RetailerController.IsCompleteId.Contains(rp.RetailerId) && !RetailerController.RetailerStatusList.Contains(rp.RetailerId))
                .Where(rp => RetailerController.AllActiveRetailers.ContainsKey(rp.RetailerId))
                //.OrderByDescending(rp => rp.IsFeaturedProduct)//临时推广合作
                .ToList();

            allprice = rps.Count;

            if (rps.Count > 0)
            {
                singlePrice = rps.Count == 1;
        
                bestPrice = rps.First().RetailerPrice;
                if (!singlePrice && rps[1].RetailerPrice < bestPrice)
                    bestPrice = rps[1].RetailerPrice;

                maxPrice = rps.Last().RetailerPrice;
                if (!singlePrice && rps[0].RetailerPrice > maxPrice)
                    maxPrice = rps[0].RetailerPrice;
            }

            retailerProducts = rps;
        }

        public static void GetRetailerProducts(int productID, out decimal bestPrice, out decimal maxPrice, out int singleRetailerID, out bool showFeatured, out int allprice, out int retailerCount)
        {
            bestPrice = 0;
            maxPrice = 0;
            singleRetailerID = 0;
            showFeatured = false;

            Hashtable allPPCMember = RetailerController.AllPPCMember;
            Hashtable allPPCNoLink = RetailerController.AllNoLinkPPCMember;
            List<int> isComplete = RetailerController.IsCompleteId;

            List<CSK_Store_RetailerProductNew> rps = RetailerProductController.GetRetailerProductsByProductId(productID);

            for (int i = 0; i < rps.Count; i++)
            {
                CSK_Store_RetailerProductNew rp = rps[i];

                if (isComplete.Contains(rp.RetailerId))
                {
                    rps.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            rps = rps.Where(rp => !RetailerController.IsCompleteId.Contains(rp.RetailerId) && !RetailerController.RetailerStatusList.Contains(rp.RetailerId)).ToList();
            rps = rps.Where(rp => RetailerController.AllActiveRetailers.ContainsKey(rp.RetailerId)).ToList();

            allprice = rps.Count;
            retailerCount = rps.Select(rp => rp.RetailerId).Distinct().Count();

            if (allprice > 0)
            {
                singleRetailerID = retailerCount == 1 ? rps[0].RetailerId : 0;

                bestPrice = rps.First().RetailerPrice;
                if (allprice > 1 && rps[1].RetailerPrice < bestPrice)
                    bestPrice = rps[1].RetailerPrice;

                maxPrice = rps.Last().RetailerPrice;
                if (allprice > 1 && rps[0].RetailerPrice > maxPrice)
                    maxPrice = rps[0].RetailerPrice;
            }

        }

        /// <summary>
        /// 不考虑 drop off 条件
        /// </summary>
        public static void GetRetailerProductsIgnoreDropOffCondition(int productID, out decimal bestPrice, out decimal maxPrice, out bool singlePrice, out bool showFeatured, out List<CSK_Store_RetailerProductNew> retailerProducts, out List<string> viewTrackingRetailers, out int allprice, int flag)
        {
            bestPrice = 0;
            maxPrice = 0;
            singlePrice = false;
            showFeatured = false;

            retailerProducts = new List<CSK_Store_RetailerProductNew>();
            viewTrackingRetailers = new List<string>();

            Hashtable allPPCMember = RetailerController.AllPPCMember;
            Hashtable allPPCNoLink = RetailerController.AllNoLinkPPCMember;
            List<int> isComplete = RetailerController.IsCompleteId;

            List<CSK_Store_RetailerProductNew> rps = RetailerProductController.GetRetailerProductsByProductId(productID);

            List<int> restrictedRetailer = new List<int>();

            int hour = DateTime.Now.Hour;
            for (int i = 0; i < rps.Count; i++)
            {
                CSK_Store_RetailerProductNew rp = rps[i];

                if (isComplete.Contains(rp.RetailerId))
                {
                    rps.RemoveAt(i);
                    i--;
                    continue;
                }
                if (!viewTrackingRetailers.Contains(rp.RetailerId.ToString()))
                    viewTrackingRetailers.Add(rp.RetailerId.ToString());

                rp.ProductId = productID;
                rp.IsFeaturedProduct = false;

                if (allPPCMember.ContainsKey(rp.RetailerId))
                {
                    CSK_Store_PPCMember ppcm = allPPCMember[rp.RetailerId] as CSK_Store_PPCMember;
                    
                    rp.PPCMemberType = 2;
                    rp.IsNoLink = false;
                    
                }
                else
                {
                    rp.PPCMemberType = 5;
                    rp.IsNoLink = true;
                }
                //如果价格相同PPCMemberType = 2的在前面
                if (!rp.IsNoLink)
                {
                    rp.OrderbyProduct = rp.RetailerPrice - 0.01m;
                    rp.PPCOrderbyProduct = rp.RetailerPrice - 0.01m;
                }
                else
                {
                    rp.OrderbyProduct = rp.RetailerPrice;
                    rp.PPCOrderbyProduct = rp.RetailerPrice;
                }
            }

            rps = rps.OrderBy(rp => rp.OrderbyProduct).ToList();
           
            rps = rps.Where(rp => !RetailerController.IsCompleteId.Contains(rp.RetailerId) && !RetailerController.RetailerStatusList.Contains(rp.RetailerId)).ToList();
            rps = rps.Where(rp => RetailerController.AllActiveRetailers.ContainsKey(rp.RetailerId)).ToList();

            allprice = rps.Count;

            if (rps.Count > 0)
            {
                singlePrice = rps.Count == 1;

                bestPrice = rps.First().RetailerPrice;
                if (!singlePrice && rps[1].RetailerPrice < bestPrice)
                    bestPrice = rps[1].RetailerPrice;

                maxPrice = rps.Last().RetailerPrice;
                if (!singlePrice && rps[0].RetailerPrice > maxPrice)
                    maxPrice = rps[0].RetailerPrice;
            }

            retailerProducts = rps;
        }

        public static void GetRetailerProducts(int productID, out decimal bestPrice, out decimal maxPrice, out int count, out int RetailerId, out int RetailerProductId, out CSK_Store_RetailerProductNew lowestrp)
        {
            bestPrice = 0;
            maxPrice = 0;
            count = 0;
            RetailerId = 0;
            RetailerProductId = 0;
            lowestrp = null;

            List<CSK_Store_RetailerProductNew> rps = RetailerProductController.GetRetailerProductsByProductId(productID);
            if (rps == null || rps.Count == 0) return;

            for (int i = 0; i < rps.Count; i++)
            {
                CSK_Store_RetailerProductNew rp = rps[i];

                if (RetailerController.AllPPCMember.ContainsKey(rp.RetailerId))
                {
                    CSK_Store_PPCMember ppcm = RetailerController.AllPPCMember[rp.RetailerId] as CSK_Store_PPCMember;

                    rp.IsNoLink = false;
                    //if (ppcm.PPCDropOff ?? false)
                    //{
                    //    if (ppcm.PPCDropOnTime <= DateTime.Now.Hour && DateTime.Now.Hour < ppcm.PPCTime)
                    //        rp.IsNoLink = false;
                    //    else
                    //        rp.IsNoLink = true;
                    //}
                }
                else
                {
                    rp.PPCMemberType = 5;
                    rp.IsNoLink = true;
                }
            }

            rps = rps.OrderBy(rp => rp.RetailerPrice).ToList();
            List<CSK_Store_RetailerProductNew> ppclowestrps = rps.Where(rp => !rp.IsNoLink).OrderBy(rp => rp.RetailerPrice).ToList();
            if (ppclowestrps != null && ppclowestrps.Count > 0 && ppclowestrps[0].RetailerPrice == rps[0].RetailerPrice)
                lowestrp = ppclowestrps[0];
            else
                lowestrp = rps[0];

            bestPrice = rps.First().RetailerPrice;
            maxPrice = rps.Last().RetailerPrice;
            count = rps.Select(rp => rp.RetailerId).Distinct().Count();
            if (count == 1)
            {
                RetailerId = rps[0].RetailerId;
                RetailerProductId = rps[0].RetailerProductId;
            }
        }

        public static void GetRetailerProductItems(int productID, out decimal bestPrice, out decimal maxPrice, out bool singlePrice, out bool showFeatured, out List<RetailerProductItem> rpis, out int allprice, int flag, out bool isInternational, out decimal overseasPices)
        {
            List<CSK_Store_RetailerProductNew> rps;
            ProductController.GetRetailerProducts(productID, out bestPrice, out maxPrice, out singlePrice, out showFeatured, out rps, out allprice, flag, out isInternational, out overseasPices);
            if (isInternational)
            {
                List<CSK_Store_RetailerProductNew> priceRps = rps.Where(r => !RetailerController.InternationalRetailers.ContainsKey(r.RetailerId)).ToList();
                singlePrice = priceRps.Count == 1;

                //bestPrice = priceRps.First().RetailerPrice;
                //if (!singlePrice && priceRps[1].RetailerPrice < bestPrice)
                //    bestPrice = priceRps[1].RetailerPrice;

                //maxPrice = priceRps.Last().RetailerPrice;
                //if (!singlePrice && priceRps[0].RetailerPrice > maxPrice)
                //    maxPrice = priceRps[0].RetailerPrice;


                if (priceRps.Count > 0)
                {
                    bestPrice = priceRps.First().RetailerPrice;
                    if (priceRps.Count > 1)
                    {
                        if (!singlePrice && priceRps[1].RetailerPrice < bestPrice)
                            bestPrice = priceRps[1].RetailerPrice;
                    }

                    maxPrice = priceRps.Last().RetailerPrice;
                    if (!singlePrice && priceRps[0].RetailerPrice > maxPrice)
                        maxPrice = priceRps[0].RetailerPrice;
                }
            }

            rpis = new List<RetailerProductItem>();
            Dictionary<int, List<CSK_Store_RetailerProductNew>> rpDic = new Dictionary<int, List<CSK_Store_RetailerProductNew>>();

            foreach (CSK_Store_RetailerProductNew rp in rps)
            {
                List<CSK_Store_RetailerProductNew> temps = new List<CSK_Store_RetailerProductNew>();

                if (rpDic.ContainsKey(rp.RetailerId))
                {
                    temps = rpDic[rp.RetailerId];
                    temps.Add(rp);
                    rpDic[rp.RetailerId] = temps;
                }
                else
                {
                    temps.Add(rp);
                    rpDic.Add(rp.RetailerId, temps);
                }
            }

            foreach (KeyValuePair<int, List<CSK_Store_RetailerProductNew>> pair in rpDic)
            {
                RetailerProductItem rpi = new RetailerProductItem();
                rpi.RetailerId = pair.Key;
                rpi.RpList = pair.Value;

                rpis.Add(rpi);
            }
        }

        /// <summary>
        /// 获取所有的网店价格
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="bestPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="singlePrice"></param>
        /// <param name="showFeatured"></param>
        /// <param name="rpis"></param>
        /// <param name="allprice"></param>
        /// <param name="flag"></param>
        /// <param name="lowestrp"></param>
        /// <param name="isInternational"></param>
        /// <param name="overseasPices"></param>
        public static void GetRetailerProductItems(int productID, out decimal bestPrice, out decimal maxPrice, out bool singlePrice, out bool showFeatured, out List<RetailerProductItem> rpis, out int allprice, int flag, out bool isInternational, out decimal overseasPices, List<RetailerProductItem> rpisInt, out List<int> retailerids, out List<CSK_Store_RetailerProductNew> rps)
        {
            rps = new List<CSK_Store_RetailerProductNew>();
            retailerids = new List<int>();

            ProductController.GetRetailerProducts(productID, out bestPrice, out maxPrice, out singlePrice, out showFeatured, out rps, out allprice, flag, out isInternational, out overseasPices);

            if (isInternational)
            {
                List<CSK_Store_RetailerProductNew> priceRps = rps.Where(r => !RetailerController.InternationalRetailers.ContainsKey(r.RetailerId)).ToList();
                if (priceRps.Count > 0)
                {
                    singlePrice = priceRps.Count == 1;

                    bestPrice = priceRps.First().RetailerPrice;
                    if (!singlePrice && priceRps[1].RetailerPrice < bestPrice)
                        bestPrice = priceRps[1].RetailerPrice;

                    maxPrice = priceRps.Last().RetailerPrice;
                    if (!singlePrice && priceRps[0].RetailerPrice > maxPrice)
                        maxPrice = priceRps[0].RetailerPrice;
                }
            }

            //int tempID = -1;
            //foreach (var item in rps) {
            //    if (item.RetailerId == 531) {
            //        item.RetailerId = tempID;
            //        tempID--;
            //    }
            //}
            

            var count531 = rps.Where(w => w.RetailerId == 531).ToList();
            if (count531.Count() > 1) {
                //var single531 = count531.Min(m => m.RetailerPrice);
                rps.RemoveAll(r => r.RetailerId == 531);
                rps.Add(count531.FirstOrDefault());
                rps = rps.OrderBy(o => o.RetailerPrice).ToList();
            }
            
           
            rpis = new List<RetailerProductItem>();
            Dictionary<string, List<CSK_Store_RetailerProductNew>> rpDic = new Dictionary<string, List<CSK_Store_RetailerProductNew>>();

            int i = 0;
            foreach (CSK_Store_RetailerProductNew rp in rps)
            {
                List<CSK_Store_RetailerProductNew> temps = new List<CSK_Store_RetailerProductNew>();
                string key = rp.RetailerId.ToString();
                if (rp.RetailerId == 344)
                {
                    key = rp.RetailerId + "_" + i;
                    i++;
                }

                if (rpDic.ContainsKey(key))
                {
                    temps = rpDic[key];
                    temps.Add(rp);
                    rpDic[key] = temps;
                }
                else
                {
                    temps.Add(rp);
                    rpDic.Add(key, temps);
                }
            }


            //foreach (var item in rpDic) {
            //    int rids = 0;
            //    if (item.Key.Contains("_"))
            //        int.TryParse(item.Key.Split('_')[0], out rids);
            //    else
            //        int.TryParse(item.Key, out rids);

            //    if (rids == 531) { 
                        
            //    }
            //}



            foreach (KeyValuePair<string, List<CSK_Store_RetailerProductNew>> pair in rpDic)
            {
                int rid = 0;
                if (pair.Key.Contains("_"))
                    int.TryParse(pair.Key.Split('_')[0], out rid);
                else
                    int.TryParse(pair.Key, out rid);

                RetailerProductItem rpi = new RetailerProductItem();
                rpi.RetailerId = rid;
                rpi.RpList = pair.Value;

                if (isInternational && RetailerController.InternationalRetailers.ContainsKey(rid))
                    rpisInt.Add(rpi);
                else
                {
                    rpis.Add(rpi);
                    retailerids.Add(rid);
                }
            }
        }

        

        public static void GetTop5PPCRetailerProducts(int productID, out List<CSK_Store_RetailerProductNew> retailerProducts, out decimal bestPrice, out decimal maxPrice, out int retailerCount, out int RetailerId, out int RetailerProductId, out CSK_Store_RetailerProductNew lowestrp, out bool isInternational, out decimal overseasPices)
        {
            bestPrice = 0;
            maxPrice = 0;
            retailerCount = 0;
            RetailerId = 0;
            RetailerProductId = 0;
            lowestrp = null;
            isInternational = false;
            overseasPices = 0;

            retailerProducts = new List<CSK_Store_RetailerProductNew>();

            Hashtable allPPCMember = RetailerController.AllPPCMember;
            Hashtable allPPCNoLink = RetailerController.AllNoLinkPPCMember;
            List<int> isComplete = RetailerController.IsCompleteId;

            List<CSK_Store_RetailerProductNew> rps = RetailerProductController.GetRetailerProductsByProductId(productID);

            if (rps != null && rps.Count > 0)
            {
                isInternational = RetailerProductController.CheckInternationalRetailerProducts(rps, out overseasPices);

                List<int> restrictedRetailer = new List<int>();

                int hour = DateTime.Now.Hour;
                for (int i = 0; i < rps.Count; i++)
                {
                    CSK_Store_RetailerProductNew rp = rps[i];

                    rp.ProductId = productID;
                    rp.IsFeaturedProduct = false;
                    if (allPPCMember.ContainsKey(rp.RetailerId))
                    {
                        CSK_Store_PPCMember ppcm = allPPCMember[rp.RetailerId] as CSK_Store_PPCMember;

                        rp.PPCMemberType = 2;
                        rp.IsNoLink = false;
                        //if (ppcm.PPCDropOff ?? false)
                        //{
                        //    if (ppcm.PPCDropOnTime <= hour && hour < ppcm.PPCTime)
                        //        rp.IsNoLink = false;
                        //    else
                        //        rp.IsNoLink = true;
                        //}
                    }
                    else
                    {
                        rp.PPCMemberType = 5;
                        rp.IsNoLink = true;
                    }

                    if (rp.PPCMemberType == 2)
                        rp.OrderbyProduct = rp.RetailerPrice - 0.01m;
                    else
                        rp.OrderbyProduct = rp.RetailerPrice;
                }

                if(isInternational)
                    rps = rps.Where(rp => !RetailerController.InternationalRetailers.ContainsKey(rp.RetailerId)).ToList();

                rps = rps.OrderBy(rp => rp.RetailerPrice).ToList();
                List<CSK_Store_RetailerProductNew> ppclowestrps = rps.Where(rp => !rp.IsNoLink).OrderBy(rp => rp.RetailerPrice).ToList();
                if (ppclowestrps != null && ppclowestrps.Count > 0 && ppclowestrps[0].RetailerPrice == rps[0].RetailerPrice)
                    lowestrp = ppclowestrps[0];
                else
                    lowestrp = rps[0];


                bestPrice = rps.First().RetailerPrice;
                maxPrice = rps.Last().RetailerPrice;
                retailerCount = rps.Select(rp => rp.RetailerId).Distinct().Count();
                if (retailerCount == 1)
                {
                    RetailerId = rps[0].RetailerId;
                    RetailerProductId = rps[0].RetailerProductId;
                }

                rps = rps.OrderBy(rp => rp.OrderbyProduct).Where(rp => rp.IsNoLink == false).Take(5).ToList();
            }

            retailerProducts = rps;
        }

        public static string GetPriceRangeFromDB(int productID, IFormatProvider currentCulture, string currentPriceSymbol)
        {
            string priceRange = "";
             SubSonic.Schema.StoredProcedure sp = db.CSK_Store_12RMB_GetBestPriceByProductID();
            //StoredProcedure sp = new StoredProcedure("CSK_Store_12RMB_GetBestPriceByProductID");
            sp.Command.AddParameter("@productID", productID, DbType.Int32);
            sp.Command.AddParameter("@CountryId", ConfigAppString.CountryID, DbType.Int32);
            using(IDataReader idr = sp.ExecuteReader())
            {
                if (idr.Read())
                {
                    string bestPrice = string.IsNullOrEmpty(idr[0].ToString()) ? "0.0" : idr[0].ToString();

                    string maxPrice = string.IsNullOrEmpty(idr[1].ToString()) ? "0.0" : idr[1].ToString();

                    bestPrice = PriceMeCommon.PriceMeStatic.PriceCultureString(double.Parse(bestPrice), currentCulture, currentPriceSymbol);
                    maxPrice = PriceMeCommon.PriceMeStatic.PriceCultureString(double.Parse(maxPrice), currentCulture, currentPriceSymbol);

                    if (bestPrice == maxPrice)
                    {
                        priceRange = bestPrice;
                    }
                    else
                    {
                        priceRange = bestPrice + " - " + maxPrice;
                    }
                }
                else
                    priceRange = "(null price)";

            }
            return priceRange;
        }

        public static string GetPriceRange(int productID, string formatString)
        {
            string result = "";
            string bestPrice = "";
            string maxPrice = "";

            SubSonic.Schema.StoredProcedure sp = db.CSK_Store_12RMB_GetBestPriceByProductID();
            //StoredProcedure sp = new StoredProcedure("CSK_Store_12RMB_GetBestPriceByProductID");
            sp.Command.AddParameter("@productID", productID, DbType.Int32);
            sp.Command.AddParameter("@CountryId", ConfigAppString.CountryID, DbType.Int32);
            IDataReader idr = sp.ExecuteReader();

            if (idr.Read())
            {
                bestPrice = string.IsNullOrEmpty(idr[0].ToString()) ? "0.0" : idr[0].ToString();
                result += GETSpecialCurrencyFormat(bestPrice, formatString);
                maxPrice = string.IsNullOrEmpty(idr[1].ToString()) ? "0.0" : idr[1].ToString();
                result += " - " + GETSpecialCurrencyFormat(maxPrice, formatString);
            }
            else
                result = "(null price)";

            idr.Close();

            if (bestPrice.Equals(maxPrice)) result = GETSpecialCurrencyFormat(bestPrice, formatString);

            return result;
        }


        public static string ConvertInt(double src)
        {
            string result = "";
            if (Math.Abs(src) < 1000)
                result = src.ToString();
            else
            {
                result = src.ToString().Substring(src.ToString().Length - 3, 3);
                long quotient = System.Convert.ToInt64(src) / 1000;
                if (Math.Abs(quotient) > 0)
                    result = ConvertInt(quotient) + "," + result;
            }

            return result;
        }

        public static string FormatPrice(decimal price, string PriceSymbol)
        {
            string dec = "";
            price = decimal.Round(price, 2);
            string result = "";
            //1. Default format for NZ, AU, HK, SG
            //1,234.00 (thousand separator, decimals)

            //2. PH format
            //12,588  (thousand separator, no decimals)
            if (PriceMeCommon.ConfigAppString.CountryID == 28 || PriceMeCommon.ConfigAppString.CountryID == 51)
            {
                //sg 36 my 45 au 1 nz 3 ph 28 hk 41
                price = decimal.Round(price, 0);
                dec = price.ToString();
            }
            //印度尼西亚 整数 00.000.000
            else
            {
                price = decimal.Round(price, 2);
                dec = price.ToString("0.00");
            }
            if (Math.Abs(price) < 1000)
                result = dec;
            else
            {
                int len = 3; string tail = string.Empty;
                if (dec.Contains('.')) { len = 6; tail = dec.Substring(dec.IndexOf('.')); }
                result = dec.Substring(dec.Length - len, len);
                long quotient = System.Convert.ToInt64(price) / 1000;
                if (Math.Abs(quotient) > 0)
                    result = ConvertInt(quotient) + "," + result;
                if (PriceMeCommon.ConfigAppString.CountryID == 51)
                {
                    result = result.Replace(",", ".");
                }
            }

            return PriceSymbol + result;
        }

        public static string GETSpecialCurrencyFormat(string strMoney, string formatString)
        {
            if (strMoney.Length == 0) strMoney = "0";

            return FormatPrice(decimal.Parse(strMoney), formatString);
        }

        public static string GETSpecialCurrencyFormat(decimal money, string formatString)
        {
            string cultureName = "en-NZ";
            string strMoney = "";

            IFormatProvider format = new System.Globalization.CultureInfo(cultureName);
            strMoney = money.ToString(formatString, format);
            return strMoney;
        }

        public static DataSet GetRelatedProducts(int manufacturerID, int categoryID, int productID)
        {
            //StoredProcedure sp = new StoredProcedure("CSK_Store_12RMB_GetRelatedProducts");
            StoredProcedure sp = db.CSK_Store_12RMB_GetRelatedProducts();
            sp.Command.AddParameter("@manufacturerID", manufacturerID, DbType.Int32);
            sp.Command.AddParameter("@categoryID", categoryID, DbType.Int32);
            sp.Command.AddParameter("@productID", productID, DbType.Int32);
            return sp.ExecuteDataSet();
        }

        public static DataSet GetRelatedProductsByParents(int categoryID)
        {
            //StoredProcedure sp = new StoredProcedure("CSK_Store_12RMB_GetRelatedProductsByParents");
            StoredProcedure sp = db.CSK_Store_12RMB_GetRelatedProductsByParents();
            sp.Command.AddParameter("@categoryID", categoryID, DbType.Int32);

            return sp.ExecuteDataSet();
        }

        public static decimal GetBestPrice(int productID)
        {
            decimal result = 0.0M;
            //StoredProcedure sp = new StoredProcedure("CSK_Store_12RMB_GetBestPriceByProductID");
            StoredProcedure sp = db.CSK_Store_12RMB_GetBestPriceByProductID();
            sp.Command.AddParameter("@productID", productID, DbType.Int32);
            sp.Command.AddParameter("@CountryId", PriceMeCommon.ConfigAppString.CountryID, DbType.Int32);
            IDataReader idr = sp.ExecuteReader();//SPs.Store12RMBGetBestPriceByProductID(productID).GetReader();
            idr.Read();
            result = string.IsNullOrEmpty(idr[0].ToString()) ? 0.0M : decimal.Parse(idr[0].ToString());
            idr.Close();

            return result;
        }

        public static List<PriceMeCommon.Data.ProductCatalog> GetMostPopular(int productCount)
        {
            ProductSearcher productSeacher = new ProductSearcher(0, null, null, null, null, "Clicks", null, null, true, productCount * 2, ConfigAppString.CountryID, true);
            SearchResult sr = productSeacher.GetSearchResult(1, productCount);
            return sr.ProductCatalogList;
        }

        public static List<ProductDescAndAttr> GetDescriptionAndAttribute(int pid)
        {
            List<ProductDescAndAttr> pdas = new List<ProductDescAndAttr>();
            string connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            var sql = string.Format("SELECT * FROM [dbo].Product_DescAndAttr where ProductID = " + pid);
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                var idr = cmd.ExecuteReader();
                while (idr.Read())
                {
                    ProductDescAndAttr pda = new ProductDescAndAttr();
                    string title = idr["title"] == null ? "" : idr["title"].ToString().Trim();
                    string sdesc = idr["ShortDescription"] == null ? "" : idr["ShortDescription"].ToString().Trim();
                    string value = idr["Value"] == null ? "" : idr["Value"].ToString().Trim();
                    string unit = idr["Unit"] == null ? "" : idr["Unit"].ToString().Trim();
                    int t, avs, cid, tid,sid;
                    int.TryParse(idr["T"].ToString(), out t);
                    int.TryParse(idr["AVS"].ToString(), out avs);
                    int.TryParse(idr["CID"].ToString(), out cid);
                    int.TryParse(idr["AttributeTypeID"].ToString(), out tid);
                    int.TryParse(idr["SID"].ToString(), out sid);

                    pda.Title = title;
                    pda.ShortDescription = sdesc;
                    pda.Value = value;
                    pda.Unit = unit;
                    pda.T = t;
                    pda.AVS = avs;
                    pda.CID = cid;
                    pda.AttributeTypeID = tid;
                    pda.AttributeTitleID = sid;

                    pdas.Add(pda);
                }
                idr.Close();

                conn.Close();
            }
            
            return pdas;
        }

        public static List<ProductDescAndAttr> GetAllDescriptionAndAttribute(int pid)
        {
            List<ProductDescAndAttr> pdas = new List<ProductDescAndAttr>();
            StoredProcedure sp = db.CSK_Product_GetAllDescAndAttr();
            sp.Command.Parameters.Add("@ProductID", pid);
            IDataReader idr = sp.ExecuteReader();
            while (idr.Read())
            {
                ProductDescAndAttr pda = new ProductDescAndAttr();
                string title = idr["Title"] == null ? "" : idr["Title"].ToString().Trim();
                int t, avs, cid;
                int.TryParse(idr["T"].ToString(), out t);
                int.TryParse(idr["AVS"].ToString(), out avs);
                int.TryParse(idr["CID"].ToString(), out cid);

                pda.Title = title;
                pda.T = t;
                pda.AVS = avs;
                pda.CID = cid;

                pdas.Add(pda);
            }
            idr.Close();

            return pdas;
        }

        public static bool HaveProductMap(int productid)
        {
            int count = 0;
            
            List<RetailerProductItem> rpis = new List<RetailerProductItem>();
            decimal bestPrice, maxPrice ; bool singlePrice ,featuredProduct ; int allprice ; bool isInternational ; decimal overseasPices ;
            ProductController.GetRetailerProductItems(productid, out bestPrice, out maxPrice, out singlePrice, out featuredProduct, out rpis, out allprice, 0, out isInternational, out overseasPices);
          //  List<int> retailerIDs = new List<int>();
            foreach (RetailerProductItem rp in rpis)
            {
                if (rp.RetailerId == 0) continue;


                PriceMeCache.RetailerCache retailer = RetailerController.GetRetailerByID(rp.RetailerId);
                if (retailer == null || retailer.RetailerId == 0) continue;
                bool isShow = true;

                //if (retailer.StoreType == 2)
                //    isShow = false;
                if (isShow == false) continue;

                List<GLatLngCache> glats;
                RetailerController.GLatLngCacheDictionary.TryGetValue(rp.RetailerId, out glats);
                if (glats == null || glats.Count == 0) continue;
                count++;
            }

           if (count > 0) return true;
           else return false;
        }

        public static bool HaveProductMap(int productid, List<RetailerProductItem> rpis)
        {
            int count = 0;
            if (rpis.Count <= 0)
                rpis = new List<RetailerProductItem>();
            //decimal bestPrice, maxPrice; bool singlePrice, featuredProduct; int allprice; bool isInternational; decimal overseasPices;
            //ProductController.GetRetailerProductItems(productid, out bestPrice, out maxPrice, out singlePrice, out featuredProduct, out rpis, out allprice, 0, out isInternational, out overseasPices);
            //  List<int> retailerIDs = new List<int>();
            foreach (RetailerProductItem rp in rpis)
            {
                if (rp.RetailerId == 0) continue;


                PriceMeCache.RetailerCache retailer = RetailerController.GetRetailerByID(rp.RetailerId);
                if (retailer == null || retailer.RetailerId == 0) continue;
                bool isShow = true;

                //if (retailer.StoreType == 2)
                //    isShow = false;
                if (isShow == false) continue;

                List<GLatLngCache> glats;
                RetailerController.GLatLngCacheDictionary.TryGetValue(rp.RetailerId, out glats);
                if (glats == null || glats.Count == 0) continue;
                count++;
            }

            if (count > 0) return true;
            else return false;
        }

        public static CSK_Store_ProductIsMerged GetProductIdInProductIsMergedByProductId(int pid)
        {
            //数据有问题   有些会有多条记录
            CSK_Store_ProductIsMerged pim = null;
            try { pim = db.CSK_Store_ProductIsMergeds.SingleOrDefault(pm => pm.ProductID == pid); }
            catch { pim = db.CSK_Store_ProductIsMergeds.Where(pm => pm.ProductID == pid).FirstOrDefault(); }

            return pim;
        }

        public static List<PriceMeCommon.Data.ProductCatalog> GetBiggestPriceDrop()
        {
            string categorys = System.Configuration.ConfigurationManager.AppSettings["PriceDropCategorys"].ToString();
            //decimal rate = 
            StoredProcedure sp = new StoredProcedure("CSK_Store_GetPriceDropInLastNDays2");

            int ndays = 7;

            sp.Command.AddParameter("@NDaysAgo", ndays, DbType.Int32);
            sp.Command.AddParameter("@TopNRows", 50, DbType.Int32);
            sp.Command.AddParameter("@MiniPrice", 100, DbType.Int32);
            sp.Command.AddParameter("@ExceptCatIDS", categorys, DbType.String);
            sp.Execute();
            IDataReader idr = sp.ExecuteReader();

            string ids = string.Empty;
            Dictionary<string, string[]> prices = new Dictionary<string, string[]>();
            while (idr.Read())
            {
                decimal dropRate = decimal.Parse(idr["DropRate"].ToString());
                //if (dropRate > rate) continue;

                ids += idr["ProductID"].ToString() + ",";
                if (!prices.ContainsKey(idr["ProductID"].ToString()))
                    prices.Add(idr["ProductID"].ToString(), new string[] { idr["TodayPrice"].ToString(), idr["HistoryPrice"].ToString(), idr["DropRate"].ToString() });
            }
            List<PriceMeCommon.Data.ProductCatalog> pcs = new List<Data.ProductCatalog>();
            List<PriceMeCommon.Data.ProductCatalog> searchPcs = PriceMeCommon.SearchController.GetBiggestPriceDropByProductIDs(ids);
            if (!string.IsNullOrEmpty(ids))
            {
                foreach (PriceMeCommon.Data.ProductCatalog pc in searchPcs)
                {
                    if (prices.ContainsKey(pc.ProductID) && pc.BestPPCPrice == pc.BestPrice)
                    {
                        pc.BestPrice = prices[pc.ProductID][0];
                        pc.PrevPrice = double.Parse(prices[pc.ProductID][1]);
                        pcs.Add(pc);
                    }
                }
            }

            return pcs;
        }

        public static CSK_Store_ProductNew GetRealProduct(int productID)
        {
            CSK_Store_ProductNew product = ProductController.GetProductNew(productID);

            if (product == null)
            {
                while (true)
                {
                    CSK_Store_ProductIsMerged pm = ProductController.GetProductIdInProductIsMergedByProductId(productID);
                    if (pm != null)
                    {
                        productID = pm.ToProductID;
                        product = ProductController.GetProductNew(pm.ToProductID);
                        if (product == null)
                            product = ProductController.GetProductHistory(pm.ToProductID);

                        if (product != null)
                        {
                            return product;
                        }
                    }
                    else
                    {
                        product = ProductController.GetProductHistory(productID);
                        return product;
                    }
                }
            }

            return product;
        }

        public static Dictionary<int, string> LoadHotProductImageDic()
        {
            Dictionary<int, string> hotProductImageDic = new Dictionary<int, string>();
            try
            {
                string connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
                var sql = string.Format("SELECT [ProductId],[Imagesurl] FROM [HotProducts]");
                using (SqlConnection conn = new SqlConnection(connectionStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    using (var idr = cmd.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            hotProductImageDic.Add(idr.GetInt32(0), idr.GetString(1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.ExceptionLogPath, ex.Message);
            }
            return hotProductImageDic;
        }

        public static string CheckUpcomingProduct(int pid)
        {
            string upcoming = string.Empty;
            string connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            var sql = string.Format("Select ReleaseDate, ReleaseDateIsEstimated From UpcomingProduct Where ProductId = " + pid + " And ReleaseDate > '" + DateTime.Today.ToString("yyyy-MM-dd") + "'");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime release = DateTime.Now;
                    DateTime.TryParse(reader["ReleaseDate"].ToString(), out release);
                    if (release.AddDays(60) < DateTime.Now)
                        return string.Empty;

                    bool isEstimated = false;
                    bool.TryParse(reader["ReleaseDateIsEstimated"].ToString(), out isEstimated);

                    string countryname = CSK_Util_Country.SingleOrDefault(c => c.countryID == ConfigAppString.CountryID).country;

                    string stringdate = release.ToString("dd MMM yyyy");
                    if (isEstimated)
                        upcoming = "This is a new release that's launching soon in " + countryname + ". The estimated release date is " + stringdate + ".";
                    else
                        upcoming = "This is a new release that's launching soon in " + countryname + ". The release date is " + stringdate + ".";
                }
                reader.Close();

                conn.Close();
            }

            return upcoming;
        }

        public static bool UpcomingProductSelect(int pid, string email)
        {
            bool isupcoming = false;
            string connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            var sql = string.Format("Select * from UpcomingProductAlter Where UpcomingProductID = " + pid + " And email = '" + email.Replace("'", "''") + "'");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();

                IDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    isupcoming = true;
                }
                conn.Close();
            }

            return isupcoming;
        }

        public static void UpcomingProductSave(int pid, string email)
        {
            string upcoming = string.Empty;
            string connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            var sql = string.Format("insert UpcomingProductAlter values(" + pid + ", '" + email.Replace("'", "''") + "', 0, " + ConfigAppString.CountryID + ")");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                cmd.ExecuteScalar();
                conn.Close();
            }
        }

        public static decimal GetPriceHistory(decimal newprice, int pid, out DateTime createdon)
        {
            createdon = DateTime.Now;
            decimal price = 0m;
            List<CSK_Store_PriceHistory> his = PriceMeDBStatic.PriceMeDB.CSK_Store_PriceHistories.Where(p => p.ProductID == pid).OrderByDescending(p => p.CreatedOn).ToList().Take(2).ToList();
            if (his != null && his.Count > 0)
            {
                if (newprice != his[0].Price)
                {
                    price = his[0].Price;
                    createdon = his[0].CreatedOn ?? DateTime.Now;
                }
                else if (his.Count > 1)
                {
                    price = his[1].Price;
                    createdon = his[1].CreatedOn ?? DateTime.Now;
                }
            }
            
            return price;
        }

        public static List<PriceDropItem> GetPriceDropItems(string parseID)
        {
            List<PriceDropItem> priceDropItems = new List<PriceDropItem>();

            List<int> productIdList = new List<int>();
            List<Data.ProductCatalog> pcList = new List<Data.ProductCatalog>();

            List<FavouriteProductData> fProducts = ProductController.listFavouriteProduct.Where(f => f.TokenId == parseID).ToList();
            List<FavouritesPageData> fPages = CategoryController.listFavoritesCatalog.Where(f => f.TokenId == parseID && f.PageName == "catalog").ToList();

            foreach (var fp in fPages)
            {
                int cid = int.Parse(fp.PageId);
                ProductSearcher productSeacher = new ProductSearcher(cid, null, null, null, null, "Clicks", null, null, true, ConfigAppString.CountryID, false, null, true, null);
                int pCount = productSeacher.GetProductCount();

                SearchResult searchResult = productSeacher.GetSearchResult(1, pCount);

                List<Data.ProductCatalog> rsList = searchResult.ProductCatalogList.Where(c => !CategoryController.listIsSearchOnly.Contains(c.CategoryID)).ToList();

                foreach (var pc in rsList)
                {
                    int pid = int.Parse(pc.ProductID);
                    if (!productIdList.Contains(pid))
                    {
                        productIdList.Add(pid);
                        pcList.Add(pc);
                    }
                }
            }

            int[] pidArray = fProducts.Select(fp => fp.ProductId).ToArray();
            foreach (var fp in fProducts)
            {
                List<Data.ProductCatalog> list = SearchController.SearchProductsByPIDs(pidArray, ConfigAppString.CountryID);
                foreach (var pc in list)
                {
                    int pid = int.Parse(pc.ProductID);
                    if (!productIdList.Contains(pid))
                    {
                        productIdList.Add(pid);

                        pcList.Add(pc);
                    }
                }
            }

            foreach (var pc in pcList)
            {
                int pid = int.Parse(pc.ProductID);
                decimal price = 0;
                decimal.TryParse(pc.BestPrice, out price);
                DateTime createdon = DateTime.Now;
                decimal oldprice = PriceMeCommon.BusinessLogic.ProductController.GetPriceHistory(price, pid, out createdon);
                if (oldprice == 0) continue;

                decimal changeprice = price - oldprice;
                if (changeprice == 0 || changeprice < ConfigAppString.PriceChange) continue;

                PriceDropItem pdi = new PriceDropItem();
                pdi.ProductId = pid;
                pdi.ProductName = pc.ProductName;
                pdi.ChangeDate = createdon;
                pdi.CurrentPrice = decimal.Parse(pc.BestPPCPrice);
                pdi.ImageUrl = pc.DefaultImage;
                pdi.HasAlert = false;
                pdi.LastChange = changeprice;
                priceDropItems.Add(pdi);

                if(priceDropItems.Count > 49)
                {
                    break;
                }
            }

            return priceDropItems;
        }
    }
        
    #region class ProductRatingEntity
    public class ProductRatingEntity
    {
        private int _ReviewCount;
        private int _UserReviewCount;
        private int _ExpertReviewCount;
        private double _productRating;
        private string _FeatureScore;
        private double _TFERating;

        public double TFERating
        {
            get { return _TFERating; }
            set { _TFERating = value; }
        }
        private double _TFURating;

        public double TFURating
        {
            get { return _TFURating; }
            set { _TFURating = value; }
        }

        public string FeatureScore
        {
            get { return _FeatureScore; }
            set { _FeatureScore = value; }
        }

        public int ReviewCount
        {
            get { return _ReviewCount; }
            set { _ReviewCount = value; }
        }

        public int UserReviewCount
        {
            get { return _UserReviewCount; }
            set { _UserReviewCount = value; }
        }

        public int ExpertReviewCount
        {
            get { return _ExpertReviewCount; }
            set { _ExpertReviewCount = value; }
        }

        public double ProductRating
        {
            get { return _productRating; }
            set { _productRating = value; }
        }

        public ProductRatingEntity()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    #endregion

    #region ProductAttributesATag
    /// <summary>
    ///  ProductController 在 PriceMeCommon 项目下面， UrlController 在网站项目下面，这里无法计算 Url 的值，所以，把 URL删除。
    /// </summary>
    public class ProductAttributesATag
    {
        int categoryID;
        string url;
        string attributeValueName;
        string title;
        int valueID;
        bool isManufacturer = false;
        bool isAttributeRange;

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        public int ValueID
        {
            get { return valueID; }
            set { valueID = value; }
        }

        public bool IsAttributeRange
        {
            get { return isAttributeRange; }
            set { isAttributeRange = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string AttributeValueName
        {
            get { return attributeValueName; }
            set { attributeValueName = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public bool IsManufacturer
        {
            get { return isManufacturer; }
            set { isManufacturer = value; }
        }
    }
    #endregion
}
