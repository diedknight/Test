using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using PriceMeCommon.Data;
using PriceMeDBA;
using SubSonic;
using SubSonic.Schema;
using System.Globalization;

namespace PriceMeCommon
{
    public static class ReviewController
    {
        const int MAXDOCS = 100000;
        const int MAXClAUSECOUNT = 100000;

        public static List<ReviewerUserReview> productReviews;
        public static List<ReviewerUserReview> todayNewProductReviews;

        public static void Load()
        {
            productReviews = new List<ReviewerUserReview>();
            if (VelocityController.GetCache(VelocityCacheKey.AllUserReviews) != null)
                productReviews = (List<ReviewerUserReview>)VelocityController.GetCache(VelocityCacheKey.AllUserReviews);
            else//没有productUserReview velocity 时，从db获取
            {
                StoredProcedure sp = PriceMeStatic.PriceMeDB.GetAllProductReview();
                IDataReader dr = sp.ExecuteReader();

                while (dr.Read())
                {
                    ReviewerUserReview rur = new ReviewerUserReview();
                    rur.ProductID = int.Parse(dr["ProductID"].ToString());
                    rur.CategoryID = int.Parse(dr["CategoryID"].ToString());
                    rur.Rating = int.Parse(dr["Rating"].ToString());
                    rur.Title = dr["Title"].ToString();
                    rur.AuthorName = dr["AuthorName"].ToString();
                    rur.Body = dr["Body"].ToString();
                    rur.ProductName = dr["ProductName"].ToString();
                    rur.DefaultImage = dr["DefaultImage"].ToString();
                    rur.PostDate = dr["PostDate"].ToString();
                    rur.UserName = dr["UserName"].ToString();
                    rur.ManufacturerID = int.Parse(dr["ManufacturerID"].ToString());

                    productReviews.Add(rur);
                }
                dr.Close();
            }

            //去掉没有retailerproduct 的产品的评论
            var productReviews_ = new List<ReviewerUserReview>();
            foreach (var review in productReviews)
            {
                ProductCatalog pc = RetailerProductController.GetRetailerProductCountByProductId(review.ProductID);
                if (pc != null)
                { 
                    productReviews_.Add(review); 
                }
            }
            productReviews = productReviews_;

            todayNewProductReviews = new List<ReviewerUserReview>();
        }

        static ReviewController()
        {
            BooleanQuery.MaxClauseCount = MAXClAUSECOUNT;
        }

        public static List<ReviewerUserReview> SearchUserReview(int categoryId, int count)
        {
            List<ReviewerUserReview> rurs = new List<ReviewerUserReview>();

            int c = 0;
            foreach (ReviewerUserReview r in productReviews)
            {
                if (c >= count) break;
                if (r.CategoryID == categoryId)
                {
                    rurs.Add(r);
                    c++;
                }

                if (categoryId == 0)
                {
                    rurs.Add(r);
                    c++;
                }
                else if (r.CategoryID == categoryId)
                {
                    rurs.Add(r);
                    c++;
                }
            }

            //StoredProcedure sp = PriceMeStatic.PriceMeDB.CSK_Store_ProductReview_GetAllUserReview();
            //sp.Command.AddParameter("@cId", categoryId, DbType.Int32);
            //IDataReader dr = sp.ExecuteReader();

            //int c = 0;
            //while (dr.Read())
            //{
            //    if (c >= count) break;

            //    ReviewerUserReview rur = new ReviewerUserReview();
            //    rur.ProductID = int.Parse(dr["ProductID"].ToString());
            //    rur.CategoryID = int.Parse(dr["CategoryID"].ToString());
            //    rur.Rating = int.Parse(dr["Rating"].ToString());
            //    rur.Title = dr["Title"].ToString();
            //    rur.AuthorName = dr["AuthorName"].ToString();
            //    rur.Body = dr["Body"].ToString();
            //    rur.ProductName = dr["ProductName"].ToString();
            //    rur.DefaultImage = dr["DefaultImage"].ToString();
            //    rur.PostDate = dr["PostDate"].ToString();
            //    rur.UserName = dr["UserName"].ToString();

            //    rurs.Add(rur);

            //    c++;
            //}
            //dr.Close();

            return rurs;
        }

        public static List<ReviewerUserReview> SearchUserReviewByManufacturerID(int manufacturerID, int count)
        {
            List<ReviewerUserReview> rurs = new List<ReviewerUserReview>();

            int c = 0;
            foreach (ReviewerUserReview r in productReviews)
            {
                if (c >= count) break;
                if (r.ManufacturerID == manufacturerID)
                {
                    rurs.Add(r);
                    c++;
                }
            }

            //StoredProcedure sp = PriceMeStatic.PriceMeDB.CSK_Store_ProductReview_GetAllUserReviewByManufacturerID();
            //sp.Command.AddParameter("@mid", manufacturerID, DbType.Int32);
            //IDataReader dr = sp.ExecuteReader();

            //int c = 0;
            //while (dr.Read())
            //{
            //    if (c >= count) break;

            //    ReviewerUserReview rur = new ReviewerUserReview();
            //    rur.ProductID = int.Parse(dr["ProductID"].ToString());
            //    rur.CategoryID = int.Parse(dr["CategoryID"].ToString());
            //    rur.Rating = int.Parse(dr["Rating"].ToString());
            //    rur.Title = dr["Title"].ToString();
            //    rur.AuthorName = dr["AuthorName"].ToString();
            //    rur.Body = dr["Body"].ToString();
            //    rur.ProductName = dr["ProductName"].ToString();
            //    rur.DefaultImage = dr["DefaultImage"].ToString();
            //    rur.PostDate = dr["PostDate"].ToString();
            //    rur.ManufacturerID = int.Parse(dr["ManufacturerID"].ToString());
            //    rur.UserName = dr["UserName"].ToString();

            //    rurs.Add(rur);

            //    c++;
            //}
            //dr.Close();

            return rurs;
        }

        public static List<ReviewerUserReview> SearchUserReviewByProductID(int ProductID)
        {
            List<ReviewerUserReview> rurs = new List<ReviewerUserReview>();
            foreach (ReviewerUserReview r in productReviews)
            {
                if (r.ProductID == ProductID)
                    rurs.Add(r);
            }

            //StoredProcedure sp = PriceMeStatic.PriceMeDB.CSK_Store_ProductReview_GetAllUserReviewByProductID();
            //sp.Command.AddParameter("@pid", ProductID, DbType.Int32);
            //IDataReader dr = sp.ExecuteReader();

            //while (dr.Read())
            //{
            //    ReviewerUserReview rur = new ReviewerUserReview();
            //    rur.ProductID = int.Parse(dr["ProductID"].ToString());
            //    rur.Rating = int.Parse(dr["Rating"].ToString());
            //    rur.Title = dr["Title"].ToString();
            //    rur.AuthorName = dr["AuthorName"].ToString();
            //    rur.Body = dr["Body"].ToString();
            //    rur.PostDate = dr["PostDate"].ToString();
            //    rur.UserName = dr["UserName"].ToString();

            //    rurs.Add(rur);
            //}
            //dr.Close();

            return rurs;
        }

        public static int GetUserReviewCountByProductId(int productId)
        {
            var u = from r in PriceMeDBStatic.PriceMeDB.CSK_Store_ProductReviews where r.ProductID == productId && r.IsApproved == true select r.ReviewID;

            if (u != null)
                return u.Count();
            else
                return 0;
        }

        public static void GetReviewSourceId(List<int> sidList, int pid)
        {
            BooleanQuery booleanQuery = new BooleanQuery();

            TermQuery termQuery = new TermQuery(new Term("ProductID", pid.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            Sort sort = new Sort();

            Searcher searcher = LuceneController.ExpertReviewIndexSearcher;

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, MAXDOCS, sort);

            for (int i = 0; i < topFieldDocs.ScoreDocs.Count(); i++)
            {
                Document hitDoc = searcher.Doc(topFieldDocs.ScoreDocs[i].Doc);
                int sourceId = int.Parse(hitDoc.Get("SourceID"));

                if (!sidList.Contains(sourceId))
                    sidList.Add(sourceId);
            }
        }

        public static bool GetReviewsCountByProductIDAndSourceId(int pid, int sid, out int count)
        {
            bool isScore = false;
            count = 0;

            BooleanQuery booleanQuery = new BooleanQuery();

            TermQuery termQuery = new TermQuery(new Term("ProductID", pid.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            termQuery = new TermQuery(new Term("SourceID", sid.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            Sort sort = new Sort();

            Searcher searcher = LuceneController.ExpertReviewIndexSearcher;

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, MAXDOCS, sort);
            for (int i = 0; i < topFieldDocs.ScoreDocs.Count(); i++)
            {
                Document hitDoc = searcher.Doc(topFieldDocs.ScoreDocs[i].Doc);

                double priceMeScore = 0;
                double.TryParse(hitDoc.Get("PriceMeScore"), NumberStyles.Float, PriceMeStatic.Provider, out priceMeScore);
                if (priceMeScore > 0)
                    isScore = true;
                break;
            }
            count = topFieldDocs.ScoreDocs.Count();
            return isScore;
        }

        public static ReviewerExpertReview GetCompareReviewsByProductIDAndSourceId(int pid, int sid)
        {
            Dictionary<int, ReviewerExpertReview> ersDic = new Dictionary<int, ReviewerExpertReview>();

            BooleanQuery booleanQuery = new BooleanQuery();

            TermQuery termQuery = new TermQuery(new Term("ProductID", pid.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            termQuery = new TermQuery(new Term("SourceID", sid.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            Sort sort = new Sort();

            Searcher searcher = LuceneController.ExpertReviewIndexSearcher;

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, MAXDOCS, sort);

            ReviewerExpertReview ner = new ReviewerExpertReview();

            for (int i = 0; i < topFieldDocs.ScoreDocs.Count(); i++)
            {
                Document hitDoc = searcher.Doc(topFieldDocs.ScoreDocs[i].Doc);
                int sourceId = int.Parse(hitDoc.Get("SourceID"));

                if (ersDic.ContainsKey(sourceId)) continue;

                ner.SourceID = sourceId.ToString();
                ner.ProductID = int.Parse(hitDoc.Get("ProductID"));
                double score = 0;
                double.TryParse(hitDoc.Get("PriceMeScore"), NumberStyles.Float, PriceMeStatic.Provider, out score);
                ner.PriceMeScore = score;
                ner.ReviewURL = hitDoc.Get("ReviewURL");
                ner.ReviewDate = hitDoc.Get("ReviewDate");
                string[] temps = ner.ReviewDate.Split('/');
                if (temps.Length > 2 && temps[0].Length < 4)
                {
                    string year = temps[2].Split(' ')[0];

                    ner.ReviewDate = year + "-" + temps[0] + "-" + temps[1];
                }
                break;
            }

            return ner;
        }

        public static List<ReviewerExpertReview> SearchExpertReview(int categoryId, int count)
        {
            List<int> productList = new List<int>();
            BooleanQuery booleanQuery = new BooleanQuery();
            if (categoryId > 0)
            {
                Query categoryQueries = GetCategoryQuery(categoryId);
                booleanQuery.Add(categoryQueries, Occur.MUST);
            }

            Sort sort = new Sort(new SortField("SortField", SortField.INT, false));

            Searcher searcher = LuceneController.ExpertReviewIndexSearcher;

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, MAXDOCS, sort);

            List<ReviewerExpertReview> ners = new List<ReviewerExpertReview>();

            for (int i = 0; i < topFieldDocs.ScoreDocs.Count(); i++)
            {
                ReviewerExpertReview ner = new ReviewerExpertReview();
                Document hitDoc = searcher.Doc(topFieldDocs.ScoreDocs[i].Doc);
                ner.SourceID = hitDoc.Get("SourceID");
                ner.ProductID = int.Parse(hitDoc.Get("ProductID"));

                if (productList.Contains(ner.ProductID)) continue;

                if (ners.Count >= count) break;

                ner.CategoryID = int.Parse(hitDoc.Get("CategoryID"));
                double score = 0;
                double.TryParse(hitDoc.Get("PriceMeScore"), NumberStyles.Float, PriceMeStatic.Provider, out score);
                ner.PriceMeScore = score;
                ner.Description = hitDoc.Get("Description");
                ner.Title = hitDoc.Get("Title");
                ner.Pros = hitDoc.Get("Pros");
                ner.Cons = hitDoc.Get("Cons");
                ner.Verdict = hitDoc.Get("Verdict");
                ner.ReviewURL = hitDoc.Get("ReviewURL");
                ner.ReviewDate = hitDoc.Get("ReviewDate");
                string[] temps = ner.ReviewDate.Split('/');
                if (temps.Length > 2 && temps[0].Length < 4)
                {
                    string year = temps[2].Split(' ')[0];

                    ner.ReviewDate = year + "-" + temps[0] + "-" + temps[1];
                }

                productList.Add(ner.ProductID);
                ners.Add(ner);
            }

            return ners;
        }

        public static List<ReviewerExpertReview> SearchExpertReviewByManufacturerID(int manufacturerID, int count)
        {
            List<int> productList = new List<int>();
            BooleanQuery booleanQuery = new BooleanQuery();

            TermQuery termQuery = new TermQuery(new Term("ManufacturerID", manufacturerID.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            Sort sort = new Sort(new SortField("SortField", SortField.INT, false));

            Searcher searcher = LuceneController.ExpertReviewIndexSearcher;

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, MAXDOCS, sort);

            List<ReviewerExpertReview> ners = new List<ReviewerExpertReview>();

            for (int i = 0; i < topFieldDocs.ScoreDocs.Count(); i++)
            {
                ReviewerExpertReview ner = new ReviewerExpertReview();
                Document hitDoc = searcher.Doc(topFieldDocs.ScoreDocs[i].Doc);
                ner.SourceID = hitDoc.Get("SourceID");
                ner.ProductID = int.Parse(hitDoc.Get("ProductID"));
                bool isExpertReview = true;
                bool.TryParse(hitDoc.Get("IsExpertReview"), out isExpertReview);

                if (productList.Contains(ner.ProductID) || !isExpertReview) continue;

                if (ners.Count >= count) break;

                ner.CategoryID = int.Parse(hitDoc.Get("CategoryID"));
                double score = 0;
                double.TryParse(hitDoc.Get("PriceMeScore"), NumberStyles.Float, PriceMeStatic.Provider, out score);
                ner.PriceMeScore = score;
                ner.Description = hitDoc.Get("Description");
                ner.Title = hitDoc.Get("Title");
                ner.Pros = hitDoc.Get("Pros");
                ner.Cons = hitDoc.Get("Cons");
                ner.Verdict = hitDoc.Get("Verdict");
                ner.ReviewURL = hitDoc.Get("ReviewURL");
                ner.ReviewDate = hitDoc.Get("ReviewDate");
                string[] temps = ner.ReviewDate.Split('/');
                if (temps.Length > 2 && temps[0].Length < 4)
                {
                    string year = temps[2].Split(' ')[0];

                    ner.ReviewDate = year + "-" + temps[0] + "-" + temps[1];
                }

                productList.Add(ner.ProductID);
                ners.Add(ner);
            }

            return ners;
        }

        public static List<ReviewerExpertReview> SearchExpertReview(int count)
        {
            Searcher searcher = LuceneController.ExpertReviewIndexSearcher;
            List<ReviewerExpertReview> ners = new List<ReviewerExpertReview>();
            List<int> productList = new List<int>();

            BooleanQuery booleanQuery = new BooleanQuery();
            Lucene.Net.Search.NumericRangeQuery<int> rangeQuery = Lucene.Net.Search.NumericRangeQuery.NewIntRange("SortField", 0, int.MaxValue, true, true);
            booleanQuery.Add(rangeQuery, Occur.MUST);

            Sort sort = new Sort(new SortField("SortField", SortField.INT, false));

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, MAXDOCS, sort);

            for (int i = 0; i < searcher.MaxDoc; i++)
            {
                ReviewerExpertReview ner = new ReviewerExpertReview();
                Document hitDoc = searcher.Doc(i);
                ner.SourceID = hitDoc.Get("SourceID");
                ner.ProductID = int.Parse(hitDoc.Get("ProductID"));

                if (productList.Contains(ner.ProductID)) continue;

                if (ners.Count >= count) break;

                ner.CategoryID = int.Parse(hitDoc.Get("CategoryID"));
                double score = 0;
                double.TryParse(hitDoc.Get("PriceMeScore"), NumberStyles.Float, PriceMeStatic.Provider, out score);
                ner.PriceMeScore = score;
                ner.Description = hitDoc.Get("Description");
                ner.Title = hitDoc.Get("Title");
                ner.Pros = hitDoc.Get("Pros");
                ner.Cons = hitDoc.Get("Cons");
                ner.Verdict = hitDoc.Get("Verdict");
                ner.SourceID = hitDoc.Get("SourceID");
                ner.ReviewURL = hitDoc.Get("ReviewURL");
                ner.ReviewDate = hitDoc.Get("ReviewDate");

                string[] temps = ner.ReviewDate.Split('/');
                if (temps.Length > 2 && temps[0].Length < 4)
                {
                    string year = temps[2].Split(' ')[0];

                    ner.ReviewDate = year + "-" + temps[0] + "-" + temps[1];
                }
                productList.Add(ner.ProductID);
                ners.Add(ner);
            }

            return ners;
        }

        public static int GetExpertReviewCountByProductId(int productId)
        {
            BooleanQuery booleanQuery = new BooleanQuery();
            TermQuery termQuery = new TermQuery(new Term("ProductID", productId.ToString()));
            booleanQuery.Add(termQuery, Occur.SHOULD);

            Sort sort = new Sort();

            Searcher searcher = LuceneController.ExpertReviewIndexSearcher;

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, MAXDOCS, sort);

            if (topFieldDocs != null)
                return topFieldDocs.ScoreDocs.Count();
            else
                return 0;
        }

        private static BooleanQuery GetCategoryQuery(int categoryID)
        {
            BooleanQuery categoryQuery = new BooleanQuery();
            if (categoryID > 0)
            {
                string[] cids = GetSubCategoryIDs(categoryID);
                foreach (string cid in cids)
                {
                    TermQuery termQuery = new TermQuery(new Term("CategoryID", cid));
                    categoryQuery.Add(termQuery, Occur.SHOULD);
                }
            }
            else
            {
                categoryQuery = null;
            }

            return categoryQuery;
        }

        public static string[] GetSubCategoryIDs(int categoryID)
        {
            string allSubCategoriesString = "";
            if (categoryID != 0)
            {
                allSubCategoriesString = GetAllSubCategoriesString(categoryID);
            }

            if (allSubCategoriesString.Length > 0)
            {
                return allSubCategoriesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            return new string[0];
        }

        static string GetAllSubCategoriesString(int cid)
        {
            TermQuery categoryQuery = new TermQuery(new Term("CategoryID", cid.ToString()));

            TopDocs topDocs = LuceneController.CategoriesIndexSearcher.Search(categoryQuery, null, 1);
            if (topDocs.ScoreDocs.Length > 0)
            {
                Lucene.Net.Documents.MapFieldSelector mapFieldSelector = new Lucene.Net.Documents.MapFieldSelector(new string[] { "SubCategoriesString" });
                Lucene.Net.Documents.Document doc = LuceneController.CategoriesIndexSearcher.Doc(topDocs.ScoreDocs[0].Doc, mapFieldSelector);

                return doc.Get("SubCategoriesString");
            }
            return cid.ToString();
        }

        public static List<ReviewerUserReview> GetTodayUserReviewsByProductAndAuthor(int product, string author)
        {
            if (todayNewProductReviews != null && todayNewProductReviews.Count > 0)
            {
                List<ReviewerUserReview> reviews =
                    (from r in todayNewProductReviews
                     where r.ProductID == product && r.AuthorName.ToLower() == author.ToLower()
                     select r).ToList();

                return reviews;
            }
            return new List<ReviewerUserReview>();
        }
        
        public static bool InsertReviewToTodayUserReviews(ReviewerUserReview review)
        {
            if (review != null)
            {
                if (todayNewProductReviews == null)
                    todayNewProductReviews = new List<ReviewerUserReview>();
                todayNewProductReviews.Add(review);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取 user 的ProductReview and RetailerReview
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public static List<ReviewerUserReview> GetUserProductAndRetailerReviewsByAuthor(string author)
        {
            List<ReviewerUserReview> list = new List<ReviewerUserReview>();
            //从velocity读取productreview
            if (productReviews != null && productReviews.Count > 0)
            {
                List<ReviewerUserReview> reviews =
                    (from r in productReviews
                     where r.AuthorName.ToLower() == author.ToLower()
                     select r).ToList();
                list.AddRange(reviews);
            }            

            //从今天的新评论列表中读取productreview
            if (todayNewProductReviews != null && todayNewProductReviews.Count > 0)
            {
                List<ReviewerUserReview> reviews =
                    (from r in todayNewProductReviews
                     where r.AuthorName.ToLower() == author.ToLower()
                     select r).ToList();

                list.AddRange(reviews);
            }

            //从velocity中读取retailerreview
            if (RetailerController.RetailerReviews != null && RetailerController.RetailerReviews.Count > 0)
            {
                foreach (var item in RetailerController.RetailerReviews)
                {
                    if (!string.IsNullOrEmpty(item.CreatedBy) && item.CreatedBy.ToLower() == author.ToLower())
                    {
                        var retailer = RetailerController.GetRetailerByID(item.RetailerID);
                        if (retailer == null) continue;

                        ReviewerUserReview review = new ReviewerUserReview();
                        review.AuthorName = item.CreatedBy;
                        review.Body = item.Body;
                        review.CategoryID = item.RetailerID;
                        review.DefaultImage = retailer.LogoFile;
                        review.PostDate = item.CreatedOn.ToString();
                        review.ProductID = item.RetailerID;
                        review.ProductName = retailer.RetailerName;
                        review.Rating = item.SourceType == "web" ? item.OverallStoreRating : (int)item.OverallRating;
                        review.Title = retailer.RetailerName;
                        review.UserName = item.CreatedBy;
                        review.ManufacturerID = 999999;

                        list.Add(review);
                    }
                }
            }

            var date = DateTime.Now;
            var date2 = DateTime.Now.AddYears(-10);
            list.ForEach(r => r.PostDate2 = DateTime.TryParse(r.PostDate, out date) ? date : date2);
            list = list.OrderByDescending(p => p.PostDate2).ToList();
            return list;
        }

        public static string GetReviewDateTime(string dateStr)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

            string date = dateStr;
            DateTime dt;
            if (DateTime.TryParse(dateStr, out dt))
                date = dt.ToString("d MMMM yyyy");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            return date;
        }
    }
}
