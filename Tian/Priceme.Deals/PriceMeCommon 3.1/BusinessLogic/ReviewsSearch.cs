using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using PriceMeCache;
using PriceMeCommon.Data;

namespace PriceMeCommon.BusinessLogic
{
    public static class ReviewsSearch
    {
        const int MAXDOCS = 100000;

        public static List<ExpertReview> GetExpertReviewsRatingByProductId(int productId)
        {
            List<ExpertReview> ers = new List<ExpertReview>();

            TopFieldDocs docs = null;
            Sort sort = new Sort();
            BooleanQuery query = new BooleanQuery();

            TermQuery termQuery = new TermQuery(new Term("ProductID", productId.ToString()));
            query.Add(termQuery, Occur.MUST);
            
            termQuery = new TermQuery(new Term("SourceID", "156"));
            query.Add(termQuery, Occur.MUST_NOT);

            docs = LuceneController.ExpertReviewIndexSearcher.Search(query, null, MAXDOCS, sort);

            for (int i = 0; i < docs.ScoreDocs.Count(); i++)
            {
                Document hitDoc = LuceneController.ExpertReviewIndexSearcher.Doc(docs.ScoreDocs[i].Doc);

                int sid = 0;
                int.TryParse(hitDoc.Get("SourceID"), out sid);
                ////判断SourceID是否存在
                //if (!ProductController.ra.ContainsKey(sid)) continue;

                float score = 0;
                float.TryParse(hitDoc.Get("PriceMeScore"), System.Globalization.NumberStyles.Float, PriceMeStatic.Provider, out score);

                bool isExpertReview = true;
                bool.TryParse(hitDoc.Get("IsExpertReview"), out isExpertReview);

                ExpertReview er = new ExpertReview();
                er.ProductID = productId;
                er.SourceID = sid;
                er.PriceMeScore = score;
                er.IsExpertReview = isExpertReview;

                ers.Add(er);
            }

            return ers;
        }


        public static Dictionary<int, List<ProductVideo>> GetAllProductVideo()
        {
            Dictionary<int, List<ProductVideo>> pvs = new Dictionary<int, List<ProductVideo>>();
            

            if (LuceneController.ProductVideoIndexSearcher != null)
            {
                for (int i = 0; i < LuceneController.ProductVideoIndexSearcher.MaxDoc; i++)
                {
                    ProductVideo pv = new ProductVideo();
                    Document doc = LuceneController.ProductVideoIndexSearcher.Doc(i);
                    //pv.VideoID = int.Parse(doc.Get("VideoID"));
                    pv.ProductID = int.Parse(doc.Get("ProductID"));
                    pv.Url = doc.Get("Url").ToString();
                    pv.Thumbnail = doc.Get("Thumbnail").ToString();

                    if (!pvs.ContainsKey(pv.ProductID))
                    {
                        List<ProductVideo> pvList = new List<ProductVideo>();
                        pvList.Add(pv);
                        pvs.Add(pv.ProductID, pvList);
                    }
                    else
                    {
                        List<ProductVideo> pvList = pvs[pv.ProductID];
                        pvList.Add(pv);
                        pvs[pv.ProductID] = pvList;
                    }
                }
            }

            return pvs;
        }

        public static Dictionary<int, ReviewAverage> GetAllReviewAverage()
        {
            Dictionary<int, ReviewAverage>  ras = new Dictionary<int, ReviewAverage>();

            if (LuceneController.ReviewAverageIndexSearcher != null)
            {
                for (int i = 0; i < LuceneController.ReviewAverageIndexSearcher.MaxDoc; i++)
                {
                    ReviewAverage ra = new ReviewAverage();
                    Document doc = LuceneController.ReviewAverageIndexSearcher.Doc(i);
                    ra.ProductID = int.Parse(doc.Get("ProductID"));
                    ra.ExpertReviewCount = int.Parse(doc.Get("ExpertReviewCount"));
                    ra.UserReviewCount = int.Parse(doc.Get("UserReviewCount"));
                    ra.ReviewCount = ra.ExpertReviewCount + ra.UserReviewCount;

                    double pr = 0;
                    double.TryParse(doc.Get("ProductRating").ToString(), System.Globalization.NumberStyles.Float, PriceMeStatic.Provider, out pr);
                    ra.ProductRating = pr;
                    ra.FeatureScore = doc.Get("FeatureScore").ToString();

                    try
                    {
                        double erating = 0;
                        double.TryParse(doc.Get("TFEAverageRating").ToString(), System.Globalization.NumberStyles.Float, PriceMeStatic.Provider, out erating);
                        ra.TFEAverageRating = erating;
                        double urating = 0;
                        double.TryParse(doc.Get("TFUAverageRating").ToString(), System.Globalization.NumberStyles.Float, PriceMeStatic.Provider, out urating);
                        ra.TFUAverageRating = urating;
                    }
                    catch (Exception ex) { }

                    if (!ras.ContainsKey(ra.ProductID))
                        ras.Add(ra.ProductID, ra);
                }
            }

            return ras;
        }

        public static ExpertReview GetExpertReviewsByProductIdSourceID(int productId, int sourceId)
        {
            ExpertReview er = new ExpertReview();

            TopFieldDocs docs = null;
            Sort sort = new Sort();
            BooleanQuery query = new BooleanQuery();

            TermQuery termQuery = new TermQuery(new Term("ProductID", productId.ToString()));
            query.Add(termQuery, Occur.MUST);

            termQuery = new TermQuery(new Term("SourceID", sourceId.ToString()));
            query.Add(termQuery, Occur.MUST);

            docs = LuceneController.ExpertReviewIndexSearcher.Search(query, null, MAXDOCS, sort);

            for (int i = 0; i < docs.ScoreDocs.Count(); i++)
            {
                Document hitDoc = LuceneController.ExpertReviewIndexSearcher.Doc(docs.ScoreDocs[i].Doc);
                er = GetExpertReviewsByDocument(hitDoc);
            }

            return er;
        }

        public static List<ExpertReview> GetSearchExpertReviewResult(int sortBy, int starts, int pageIndex, int pageSize, int productId)
        {
            List<ExpertReview> ers = new List<ExpertReview>();

            TopFieldDocs docs = null;
            Sort sort = GetSort(sortBy);
            BooleanQuery query = new BooleanQuery();
            Filter filter = null;

            TermQuery termQuery = new TermQuery(new Term("ProductID", productId.ToString()));
            query.Add(termQuery, Occur.MUST);

            termQuery = new TermQuery(new Term("SourceID", "156"));
            query.Add(termQuery, Occur.MUST_NOT);

            if (starts == -1)
            {
                termQuery = new TermQuery(new Term("IsExpertReview", "True"));
                query.Add(termQuery, Occur.MUST);
            }
            else if (starts > 0 && starts < 7)
            {
                termQuery = new TermQuery(new Term("IsExpertReview", "True"));
                query.Add(termQuery, Occur.MUST);

                if (starts == 1)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0.1f, 1.5f, true, true);
                else if (starts == 2)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 1.6f, 2.5f, true, true);
                else if (starts == 3)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 2.6f, 3.5f, true, true);
                else if (starts == 4)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 3.6f, 4.5f, true, true);
                else if (starts == 5)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 4.6f, 6, true, false);
                else if (starts == 6)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0, 0, true, true);
            }
            else if (starts == 7)
            {
                termQuery = new TermQuery(new Term("IsExpertReview", "False"));
                query.Add(termQuery, Occur.MUST);
            }
            else if (starts > 7)
            {
                termQuery = new TermQuery(new Term("IsExpertReview", "False"));
                query.Add(termQuery, Occur.MUST);

                if (starts == 8)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0.1f, 1.5f, true, true);
                else if (starts == 9)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 1.6f, 2.5f, true, true);
                else if (starts == 10)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 2.6f, 3.5f, true, true);
                else if (starts == 11)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 3.6f, 4.5f, true, true);
                else if (starts == 12)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 4.6f, 6f, true, false);
                else if (starts == 13)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0, 0, true, true);
            }

            docs = LuceneController.ExpertReviewIndexSearcher.Search(query, filter, MAXDOCS, sort);

            HitsInfo _resultHitsInfo = new HitsInfo(LuceneController.ExpertReviewIndexSearcher, docs);
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0 || pageSize == 0)
                return ers;

            PagerInfo pagerInfo = new PagerInfo(pageIndex, pageSize, _resultHitsInfo.ResultCount);

            List<ExpertReview> erNotRatings = new List<ExpertReview>();
            for (int i = pagerInfo.StartIndex; i < pagerInfo.EndIndex; i++)
            {
                ExpertReview er = GetExpertReview(_resultHitsInfo, i);
                if (er.ProductID > 0)
                {
                    if (sortBy == 5 && er.PriceMeScore == 0)
                        erNotRatings.Add(er);
                    else
                        ers.Add(er);
                }
            }

            if (sortBy == 5)
                ers.AddRange(erNotRatings);

            return ers;
        }

        /// <summary>
        /// for iphone app,return pagecount
        /// </summary>
        public static List<ExpertReview> GetSearchExpertReviewResult(int sortBy, int starts, int pageIndex, int pageSize, int productId, out int pageCount)
        {
            List<ExpertReview> ers = new List<ExpertReview>();

            TopFieldDocs docs = null;
            Sort sort = GetSort(sortBy);
            BooleanQuery query = new BooleanQuery();
            Filter filter = null;

            TermQuery termQuery = new TermQuery(new Term("ProductID", productId.ToString()));
            query.Add(termQuery, Occur.MUST);

            termQuery = new TermQuery(new Term("SourceID", "156"));
            query.Add(termQuery, Occur.MUST_NOT);

            if (starts == -1)
            {
                termQuery = new TermQuery(new Term("IsExpertReview", "True"));
                query.Add(termQuery, Occur.MUST);
            }
            else if (starts > 0 && starts < 7)
            {
                termQuery = new TermQuery(new Term("IsExpertReview", "True"));
                query.Add(termQuery, Occur.MUST);

                if (starts == 1)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0.1f, 1.5f, true, true);
                else if (starts == 2)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 1.6f, 2.5f, true, true);
                else if (starts == 3)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 2.6f, 3.5f, true, true);
                else if (starts == 4)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 3.6f, 4.5f, true, true);
                else if (starts == 5)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 4.6f, 6, true, false);
                else if (starts == 6)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0, 0, true, true);
            }
            else if (starts == 7)
            {
                termQuery = new TermQuery(new Term("IsExpertReview", "False"));
                query.Add(termQuery, Occur.MUST);
            }
            else if (starts > 7)
            {
                termQuery = new TermQuery(new Term("IsExpertReview", "False"));
                query.Add(termQuery, Occur.MUST);

                if (starts == 8)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0.1f, 1.5f, true, true);
                else if (starts == 9)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 1.6f, 2.5f, true, true);
                else if (starts == 10)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 2.6f, 3.5f, true, true);
                else if (starts == 11)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 3.6f, 4.5f, true, true);
                else if (starts == 12)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 4.6f, 6f, true, false);
                else if (starts == 13)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0, 0, true, true);
            }

            docs = LuceneController.ExpertReviewIndexSearcher.Search(query, filter, MAXDOCS, sort);

            HitsInfo _resultHitsInfo = new HitsInfo(LuceneController.ExpertReviewIndexSearcher, docs);
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0 || pageSize == 0)
            {
                pageCount = 0;
                return ers;
            }
            PagerInfo pagerInfo = new PagerInfo(pageIndex, pageSize, _resultHitsInfo.ResultCount);

            for (int i = pagerInfo.StartIndex; i < pagerInfo.EndIndex; i++)
            {
                ExpertReview er = GetExpertReview(_resultHitsInfo, i);
                if (er.ProductID > 0)
                    ers.Add(er);
            }

            var pCount = 0;
            pCount = _resultHitsInfo.ResultCount / pageSize;
            if (_resultHitsInfo.ResultCount % pageSize > 0)
                pCount = pCount + 1;

            pageCount = pCount;

            return ers;
        }

        /// <summary>
        /// For YahooDeals
        /// </summary>
        /// <returns></returns>
        public static List<ExpertReview> GetSearchExpertReviewResultForYahooDeals(int sortBy, int starts, int pageIndex, int pageSize, int productId)
        {
            List<ExpertReview> ers = new List<ExpertReview>();

            TopFieldDocs docs = null;
            Sort sort = GetSort(sortBy);
            BooleanQuery query = new BooleanQuery();
            Filter filter = null;

            TermQuery termQuery = new TermQuery(new Term("ProductID", productId.ToString()));
            query.Add(termQuery, Occur.MUST);

            termQuery = new TermQuery(new Term("SourceID", "156"));
            query.Add(termQuery, Occur.MUST_NOT);

            if (starts == -1)
            {
                //termQuery = new TermQuery(new Term("IsExpertReview", "True"));
                //query.Add(termQuery, Occur.MUST);
            }
            else if (starts > 0 && starts < 7)
            {
                //termQuery = new TermQuery(new Term("IsExpertReview", "True"));
                //query.Add(termQuery, Occur.MUST);

                if (starts == 1)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0.1f, 1.5f, true, true);
                else if (starts == 2)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 1.6f, 2.5f, true, true);
                else if (starts == 3)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 2.6f, 3.5f, true, true);
                else if (starts == 4)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 3.6f, 4.5f, true, true);
                else if (starts == 5)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 4.6f, 6, true, false);
                else if (starts == 6)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0, 0, true, true);
            }
            else if (starts == 7)
            {
                //termQuery = new TermQuery(new Term("IsExpertReview", "False"));
                //query.Add(termQuery, Occur.MUST);
            }
            else if (starts > 7)
            {
                //termQuery = new TermQuery(new Term("IsExpertReview", "False"));
                //query.Add(termQuery, Occur.MUST);

                if (starts == 8)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0.1f, 1.5f, true, true);
                else if (starts == 9)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 1.6f, 2.5f, true, true);
                else if (starts == 10)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 2.6f, 3.5f, true, true);
                else if (starts == 11)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 3.6f, 4.5f, true, true);
                else if (starts == 12)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 4.6f, 6f, true, false);
                else if (starts == 13)
                    filter = NumericRangeFilter.NewFloatRange("PriceMeScore", 0, 0, true, true);
            }

            docs = LuceneController.ExpertReviewIndexSearcher.Search(query, filter, MAXDOCS, sort);

            HitsInfo _resultHitsInfo = new HitsInfo(LuceneController.ExpertReviewIndexSearcher, docs);
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0 || pageSize == 0)
                return ers;

            PagerInfo pagerInfo = new PagerInfo(pageIndex, pageSize, _resultHitsInfo.ResultCount);

            for (int i = pagerInfo.StartIndex; i < pagerInfo.EndIndex; i++)
            {
                ExpertReview er = GetExpertReview(_resultHitsInfo, i);
                if (er.ProductID > 0)
                    ers.Add(er);
            }

            return ers;
        }
                
        internal static ExpertReview GetExpertReview(HitsInfo hitsInfo, int index)
        {
            Document doc = hitsInfo.GetDocument(index);
            ExpertReview er = GetExpertReviewsByDocument(doc);
            return er;
        }

        private static Sort GetSort(int sortBy)
        {
            Sort sort = null;

            if (sortBy == 1)
                sort = new Sort(new SortField[] { new SortField("IsExpertReview", SortField.STRING, true) });
            else if (sortBy == 2)
                sort = new Sort(new SortField[] { new SortField("ReviewDate", SortField.STRING, true) });
            else if(sortBy == 3)
                sort = new Sort(new SortField[] { new SortField("ReviewDate", SortField.STRING, false) });
            else if (sortBy == 4)
                sort = new Sort(new SortField[] { new SortField("PriceMeScore", SortField.FLOAT, true) });
            else if (sortBy == 5)
                sort = new Sort(new SortField[] { new SortField("PriceMeScore", SortField.FLOAT, false) });

            return sort;
        }

        private static ExpertReview GetExpertReviewsByDocument(Document hitDoc)
        {
            ExpertReview er = new ExpertReview();

            int sourceId = 0;
            int.TryParse(hitDoc.Get("SourceID"), out sourceId);
            ExpertReviewSource reviews = null;
            if (ProductController.ra.ContainsKey(sourceId))
                reviews = ProductController.ra[sourceId];
            else
                return er;
            er.LogoFile = reviews.LogoFile;
            er.WebSiteName = reviews.WebSiteName;

            int reviewID = 0;
            int.TryParse(hitDoc.Get("ReviewID"), out reviewID);
            int productId = 0;
            int.TryParse(hitDoc.Get("ProductID"), out productId);
            float score = 0;
            float.TryParse(hitDoc.Get("PriceMeScore"), System.Globalization.NumberStyles.Float, PriceMeStatic.Provider, out score);
            DateTime reviewDate;
            DateTime.TryParse(hitDoc.Get("ReviewDate"), out reviewDate);
            bool isExpertReview = true;
            bool.TryParse(hitDoc.Get("IsExpertReview"), out isExpertReview);

            er.ReviewID = reviewID;
            er.ProductID = productId;
            er.SourceID = sourceId;
            er.PriceMeScore = score;
            er.Title = hitDoc.Get("Title");
            er.Pros = hitDoc.Get("Pros");
            er.Cons = hitDoc.Get("Cons");
            er.Verdict = hitDoc.Get("Verdict");
            er.Description = hitDoc.Get("Description");
            er.ReviewURL = hitDoc.Get("ReviewURL");
            er.ReviewBy = hitDoc.Get("ReviewBy");
            er.ReviewDate = reviewDate;
            er.IsExpertReview = isExpertReview;

            return er;
        }
    }
}
