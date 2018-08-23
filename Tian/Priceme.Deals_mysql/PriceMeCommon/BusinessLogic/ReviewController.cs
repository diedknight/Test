using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using PriceMeCommon.Data;
using PriceMeDBA;
using System.Globalization;
using PriceMeCommon.BusinessLogic;
using System.Data.SqlClient;

namespace PriceMeCommon.BusinessLogic
{
    public static class ReviewController
    {
        const int MAXDOCS = 100000;

        static Dictionary<int, List<ReviewerUserReview>> MultiCountryProductReviews_Static;
        static Dictionary<int, List<ReviewerUserReview>> MultiCountryTodayNewProductReviews_Static;

        public static void Load(Timer.DKTimer timer)
        {
            MultiCountryProductReviews_Static = GetMultiCountryProductReviews();
            MultiCountryTodayNewProductReviews_Static = new Dictionary<int, List<ReviewerUserReview>>();
        }

        private static Dictionary<int, List<ReviewerUserReview>> GetMultiCountryProductReviews()
        {
            Dictionary<int, List<ReviewerUserReview>> multiDic = new Dictionary<int, List<ReviewerUserReview>>();
            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                List<ReviewerUserReview> rrList = GetProductReviews(countryId);

                //去掉没有retailerproduct 的产品的评论
                //var productReviews = new List<ReviewerUserReview>();
                //foreach (var review in rrList)
                //{
                //    var pc = ProductController.GetProductNew(review.ProductID, countryId);
                //    if (pc != null)
                //    {
                //        productReviews.Add(review);
                //    }
                //}
                //multiDic.Add(countryId, productReviews);

                //如果要剔除没有retailerproduct 的产品的评论， 可以考虑修改存储过程：GetAllProductReview
                multiDic.Add(countryId, rrList);
            }
            return multiDic;
        }

        private static List<ReviewerUserReview> GetProductReviews(int countryId)
        {
            List<ReviewerUserReview> list = new List<ReviewerUserReview>();

            var connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            string sql = "GetAllProductReview";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        ReviewerUserReview rur = new ReviewerUserReview();
                        rur.ProductID = int.Parse(idr["ProductID"].ToString());
                        rur.CategoryID = int.Parse(idr["CategoryID"].ToString());
                        rur.Rating = int.Parse(idr["Rating"].ToString());
                        rur.Title = idr["Title"].ToString();
                        rur.AuthorName = idr["AuthorName"].ToString();
                        rur.Body = idr["Body"].ToString();
                        rur.ProductName = idr["ProductName"].ToString();
                        rur.DefaultImage = idr["DefaultImage"].ToString();
                        
                        rur.PostDate = idr.GetDateTime(idr.GetOrdinal("PostDate"));

                        rur.UserName = idr["UserName"].ToString();
                        rur.ManufacturerID = int.Parse(idr["ManufacturerID"].ToString());

                        list.Add(rur);
                    }
                }
            }

            return list;
        }

        public static List<ReviewerUserReview> SearchUserReview(int categoryId, int count, int countryId)
        {
            if (MultiCountryProductReviews_Static.ContainsKey(countryId))
            {
                var productReviews = MultiCountryProductReviews_Static[countryId];
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

                return rurs;
            }

            return new List<ReviewerUserReview>();
        }

        public static List<ReviewerUserReview> SearchUserReviewByManufacturerID(int manufacturerID, int count, int countryId)
        {
            if (MultiCountryProductReviews_Static.ContainsKey(countryId))
            {
                var productReviews = MultiCountryProductReviews_Static[countryId];
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

                return rurs;
            }

            return new List<ReviewerUserReview>();
        }

        public static List<ReviewerUserReview> SearchUserReviewByProductID(int productId, int countryId)
        {
            if (MultiCountryProductReviews_Static.ContainsKey(countryId))
            {
                var productReviews = MultiCountryProductReviews_Static[countryId];
                List<ReviewerUserReview> rurs = new List<ReviewerUserReview>();
                foreach (ReviewerUserReview r in productReviews)
                {
                    if (r.ProductID == productId)
                        rurs.Add(r);
                }

                return rurs;
            }
            return new List<ReviewerUserReview>();
        }

        public static int GetUserReviewCountByProductId(int productId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                var u = from r in PriceMeDBStatic.PriceMeDB.CSK_Store_ProductReviews where r.ProductID == productId && r.IsApproved == true select r.ReviewID;

                if (u != null)
                    return u.Count();
                else
                    return 0;
            }
        }

        public static void GetReviewSourceId(List<int> sidList, int pid, int countryId)
        {
            BooleanQuery booleanQuery = new BooleanQuery();

            //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(pid, 0, btRef);
            //TermQuery termQuery = new TermQuery(new Term("ProductID", btRef));
            TermQuery termQuery = new TermQuery(new Term("ProductID", pid.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            Sort sort = new Sort();

            IndexSearcher searcher = PriceMeCommon.BusinessLogic.MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, MAXDOCS, sort);

            for (int i = 0; i < topFieldDocs.ScoreDocs.Count(); i++)
            {
                Document hitDoc = searcher.Doc(topFieldDocs.ScoreDocs[i].Doc);
                int sourceId = int.Parse(hitDoc.Get("SourceID"));

                if (!sidList.Contains(sourceId))
                    sidList.Add(sourceId);
            }
        }

        public static bool GetReviewsCountByProductIDAndSourceId(int pid, int sid, out int count, int countryId)
        {
            bool isScore = false;
            count = 0;

            BooleanQuery booleanQuery = new BooleanQuery();

            //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(pid, 0, btRef);
            //TermQuery termQuery = new TermQuery(new Term("ProductID", btRef));
            TermQuery termQuery = new TermQuery(new Term("ProductID", pid.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);


            termQuery = new TermQuery(new Term("SourceID", sid.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            Sort sort = new Sort();

            IndexSearcher searcher = MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

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

        public static ReviewerExpertReview GetCompareReviewsByProductIDAndSourceId(int pid, int sid, int countryId)
        {
            Dictionary<int, ReviewerExpertReview> ersDic = new Dictionary<int, ReviewerExpertReview>();

            BooleanQuery booleanQuery = new BooleanQuery();

            //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(pid, 0, btRef);
            //TermQuery termQuery = new TermQuery(new Term("ProductID", btRef));
            TermQuery termQuery = new TermQuery(new Term("ProductID", pid.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            termQuery = new TermQuery(new Term("SourceID", sid.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            Sort sort = new Sort();

            IndexSearcher searcher = MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

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

        public static List<ReviewerExpertReview> SearchExpertReview(int categoryId, int count, int countryId)
        {
            List<int> productList = new List<int>();
            BooleanQuery booleanQuery = new BooleanQuery();
            if (categoryId > 0)
            {
                Query categoryQueries = GetCategoryQuery(categoryId, countryId);
                booleanQuery.Add(categoryQueries, Occur.MUST);
            }

            Sort sort = new Sort(new SortField("SortField", SortField.INT, false));

            IndexSearcher searcher = MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

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

        public static List<ReviewerExpertReview> SearchExpertReviewByManufacturerID(int manufacturerID, int count, int countryId)
        {
            List<int> productList = new List<int>();
            BooleanQuery booleanQuery = new BooleanQuery();

            //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(manufacturerID, 0, btRef);
            //TermQuery termQuery = new TermQuery(new Term("ManufacturerID", btRef));
            TermQuery termQuery = new TermQuery(new Term("ManufacturerID", manufacturerID.ToString()));
            booleanQuery.Add(termQuery, Occur.MUST);

            Sort sort = new Sort(new SortField("SortField", SortField.INT, false));

            IndexSearcher searcher = PriceMeCommon.BusinessLogic.MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

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

        public static List<ReviewerExpertReview> SearchExpertReview(int count, int countryId)
        {
            IndexSearcher searcher = MultiCountryController.GetExpertReviewLuceneSearcher(countryId);
            List<ReviewerExpertReview> ners = new List<ReviewerExpertReview>();
            List<int> productList = new List<int>();

            BooleanQuery booleanQuery = new BooleanQuery();
            NumericRangeQuery<int> rangeQuery = NumericRangeQuery.NewIntRange("SortField", 0, int.MaxValue, true, true);
            booleanQuery.Add(rangeQuery, Occur.MUST);

            Sort sort = new Sort(new SortField("SortField", SortField.INT, false));

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, MAXDOCS, sort);

            for (int i = 0; i < searcher.IndexReader.MaxDoc; i++)
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

        public static int GetExpertReviewCountByProductId(int productId, int countryId)
        {
            BooleanQuery booleanQuery = new BooleanQuery();

            //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(productId, 0, btRef);
            //TermQuery termQuery = new TermQuery(new Term("ProductID", btRef));
            TermQuery termQuery = new TermQuery(new Term("ProductID", productId.ToString()));
            booleanQuery.Add(termQuery, Occur.SHOULD);

            Sort sort = new Sort();

            IndexSearcher searcher = PriceMeCommon.BusinessLogic.MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, MAXDOCS, sort);

            if (topFieldDocs != null)
                return topFieldDocs.ScoreDocs.Count();
            else
                return 0;
        }

        private static BooleanQuery GetCategoryQuery(int categoryID, int countryId)
        {
            BooleanQuery categoryQuery = new BooleanQuery();
            if (categoryID > 0)
            {
                string[] cids = GetSubCategoryIDs(categoryID, countryId);
                foreach (string cid in cids)
                {
                    //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                    //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(cid), 0, btRef);
                    //TermQuery termQuery = new TermQuery(new Term("CategoryID", btRef));
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

        public static string[] GetSubCategoryIDs(int categoryID, int countryId)
        {
            string allSubCategoriesString = "";
            if (categoryID != 0)
            {
                allSubCategoriesString = GetAllSubCategoriesString(categoryID, countryId);
            }

            if (allSubCategoriesString.Length > 0)
            {
                return allSubCategoriesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            return new string[0];
        }

        static string GetAllSubCategoriesString(int cId, int countryId)
        {
            //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(cId, 0, btRef);
            //TermQuery categoryQuery = new TermQuery(new Term("CategoryID", btRef));
            TermQuery categoryQuery = new TermQuery(new Term("CategoryID", cId.ToString()));

            IndexSearcher searcher = MultiCountryController.GetCategoriesLuceneSearcher(countryId);

            TopDocs topDocs = searcher.Search(categoryQuery, null, 1);
            if (topDocs.ScoreDocs.Length > 0)
            {
                //ISet<string> s = new HashSet<string>();
                //s.Add("SubCategoriesString");
                //Lucene.Net.Documents.Document doc = searcher.Doc(topDocs.ScoreDocs[0].Doc, s);

                Lucene.Net.Documents.Document doc = searcher.Doc(topDocs.ScoreDocs[0].Doc, new MapFieldSelector("SubCategoriesString"));

                return doc.Get("SubCategoriesString");
            }
            return cId.ToString();
        }

        public static List<ReviewerUserReview> GetTodayUserReviewsByProductAndAuthor(int product, string author, int countryId)
        {
            if (MultiCountryTodayNewProductReviews_Static.ContainsKey(countryId))
            {
                var todayNewProductReviews = MultiCountryTodayNewProductReviews_Static[countryId];

                if (todayNewProductReviews.Count > 0)
                {
                    List<ReviewerUserReview> reviews =
                        (from r in todayNewProductReviews
                         where r.ProductID == product && r.AuthorName.ToLower() == author.ToLower()
                         select r).ToList();

                    return reviews;
                }
            }
            return new List<ReviewerUserReview>();
        }
        
        public static bool InsertReviewToTodayUserReviews(ReviewerUserReview review, int countryId)
        {
            if (review != null)
            {
                if (MultiCountryTodayNewProductReviews_Static.ContainsKey(countryId))
                {
                    MultiCountryTodayNewProductReviews_Static[countryId].Add(review);
                }
                else
                {
                    var todayNewProductReviews = new List<ReviewerUserReview>();
                    todayNewProductReviews.Add(review);
                    MultiCountryTodayNewProductReviews_Static.Add(countryId, todayNewProductReviews);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取 user 的ProductReview and RetailerReview
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public static List<ReviewerUserReview> GetUserProductAndRetailerReviewsByAuthor(string author, int countryId)
        {
            if (MultiCountryProductReviews_Static.ContainsKey(countryId))
            {
                var productReviews = MultiCountryProductReviews_Static[countryId];
                List<ReviewerUserReview> list = new List<ReviewerUserReview>();

                if (productReviews != null && productReviews.Count > 0)
                {
                    List<ReviewerUserReview> reviews =
                        (from r in productReviews
                         where r.AuthorName.ToLower() == author.ToLower()
                         select r).ToList();
                    list.AddRange(reviews);
                }

                //从今天的新评论列表中读取productreview
                if (MultiCountryTodayNewProductReviews_Static.ContainsKey(countryId))
                {
                    var todayNewProductReviews = MultiCountryTodayNewProductReviews_Static[countryId];
                    if (todayNewProductReviews != null && todayNewProductReviews.Count > 0)
                    {
                        List<ReviewerUserReview> reviews =
                            (from r in todayNewProductReviews
                             where r.AuthorName.ToLower() == author.ToLower()
                             select r).ToList();

                        list.AddRange(reviews);
                    }
                }

                //从velocity中读取retailerreview
                var retailerReviews = RetailerController.GetAllRetailerReviews(countryId);

                if(retailerReviews != null)
                {
                    foreach (var item in retailerReviews)
                    {
                        if (!string.IsNullOrEmpty(item.CreatedBy) && item.CreatedBy.ToLower() == author.ToLower())
                        {
                            var retailer = RetailerController.GetRetailerDeep(item.RetailerID, countryId);
                            if (retailer == null) continue;

                            ReviewerUserReview review = new ReviewerUserReview();
                            review.AuthorName = item.CreatedBy;
                            review.Body = item.Body;
                            review.CategoryID = item.RetailerID;
                            review.DefaultImage = retailer.LogoFile;
                            review.PostDate = item.CreatedOn;
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
                list.ForEach(r => r.PostDate2 = r.PostDate);
                list = list.OrderByDescending(p => p.PostDate2).ToList();
                return list;
            }

            return new List<ReviewerUserReview>();
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

        public static List<CSK_Store_ProductReview> GetRecentProductReviews(int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return CSK_Store_ProductReview.Find(pr => pr.IsApproved == true).OrderByDescending(pr => pr.PostDate).Take(5).ToList();
            }
        }

        public static object GetProductReviewByReviewID(int reviewId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("GetProductReviewByReviewID");
                sp.Command.AddParameter("reviewID", reviewId, DbType.Int32);

                return sp.ExecuteDataSet();
            }
        }

        public static CSK_Store_ProductReview GetProductReview(int reviewId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return CSK_Store_ProductReview.SingleOrDefault(pr => pr.ReviewID == reviewId);
            }
        }
    }
}
