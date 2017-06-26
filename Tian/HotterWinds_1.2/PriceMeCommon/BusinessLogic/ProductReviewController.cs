using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using PriceMeCache;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceMeDBA;

namespace PriceMeCommon.BusinessLogic
{
    public static class ProductReviewController
    {
        const int MAXDOCS = 100000;

        public static List<ExpertReview> GetExpertReviewsRatingByProductId(int productId, int countryId)
        {
            IndexSearcher searcher = MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

            List<ExpertReview> ers = new List<ExpertReview>();

            TopFieldDocs docs = null;
            Sort sort = new Sort();
            BooleanQuery query = new BooleanQuery();

            TermQuery termQuery = new TermQuery(new Term("ProductID", productId.ToString()));
            query.Add(termQuery, Occur.MUST);

            termQuery = new TermQuery(new Term("SourceID", "156"));
            query.Add(termQuery, Occur.MUST_NOT);

            docs = searcher.Search(query, null, MAXDOCS, sort);

            for (int i = 0; i < docs.ScoreDocs.Count(); i++)
            {
                Document hitDoc = searcher.Doc(docs.ScoreDocs[i].Doc);

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

        public static ExpertReview GetExpertReviewsByProductIdSourceID(int productId, int sourceId, int countryId)
        {
            IndexSearcher searcher = MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

            ExpertReview er = new ExpertReview();

            TopFieldDocs docs = null;
            Sort sort = new Sort();
            BooleanQuery query = new BooleanQuery();

            TermQuery termQuery = new TermQuery(new Term("ProductID", productId.ToString()));
            query.Add(termQuery, Occur.MUST);

            termQuery = new TermQuery(new Term("SourceID", sourceId.ToString()));
            query.Add(termQuery, Occur.MUST);

            docs = searcher.Search(query, null, MAXDOCS, sort);

            for (int i = 0; i < docs.ScoreDocs.Count(); i++)
            {
                Document hitDoc = searcher.Doc(docs.ScoreDocs[i].Doc);
                er = GetExpertReviewsByDocument(hitDoc, countryId);
            }

            return er;
        }

        public static ExpertReview GetTestFreaksExpertReviews(int productId, int countryId)
        {
            ExpertReview er = GetExpertReviewsByProductIdSourceID(productId, 156, countryId);
            return er;
        }

        public static List<ExpertReview> GetSearchExpertReviewResult(int sortBy, int starts, int pageIndex, int pageSize, int productId, int countryId)
        {
            IndexSearcher searcher = MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

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

            docs = searcher.Search(query, filter, MAXDOCS, sort);

            HitsInfo _resultHitsInfo = new HitsInfo(searcher, docs);
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0 || pageSize == 0)
                return ers;

            PagerInfo pagerInfo = new PagerInfo(pageIndex, pageSize, _resultHitsInfo.ResultCount);

            List<ExpertReview> erNotRatings = new List<ExpertReview>();
            for (int i = pagerInfo.StartIndex; i < pagerInfo.EndIndex; i++)
            {
                ExpertReview er = GetExpertReview(_resultHitsInfo, i, countryId);
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
        public static List<ExpertReview> GetSearchExpertReviewResult(int sortBy, int starts, int pageIndex, int pageSize, int productId, out int pageCount, int countryId)
        {
            IndexSearcher searcher = MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

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

            docs = searcher.Search(query, filter, MAXDOCS, sort);

            HitsInfo _resultHitsInfo = new HitsInfo(searcher, docs);
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0 || pageSize == 0)
            {
                pageCount = 0;
                return ers;
            }
            PagerInfo pagerInfo = new PagerInfo(pageIndex, pageSize, _resultHitsInfo.ResultCount);

            for (int i = pagerInfo.StartIndex; i < pagerInfo.EndIndex; i++)
            {
                ExpertReview er = GetExpertReview(_resultHitsInfo, i, countryId);
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
        public static List<ExpertReview> GetSearchExpertReviewResultForYahooDeals(int sortBy, int starts, int pageIndex, int pageSize, int productId, int countryId)
        {
            IndexSearcher searcher = MultiCountryController.GetExpertReviewLuceneSearcher(countryId);

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

            }
            else if (starts > 0 && starts < 7)
            {
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

            }
            else if (starts > 7)
            {
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

            docs = searcher.Search(query, filter, MAXDOCS, sort);

            HitsInfo _resultHitsInfo = new HitsInfo(searcher, docs);
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0 || pageSize == 0)
                return ers;

            PagerInfo pagerInfo = new PagerInfo(pageIndex, pageSize, _resultHitsInfo.ResultCount);

            for (int i = pagerInfo.StartIndex; i < pagerInfo.EndIndex; i++)
            {
                ExpertReview er = GetExpertReview(_resultHitsInfo, i, countryId);
                if (er.ProductID > 0)
                    ers.Add(er);
            }

            return ers;
        }

        internal static ExpertReview GetExpertReview(HitsInfo hitsInfo, int index, int countryId)
        {
            Document doc = hitsInfo.GetDocument(index);
            ExpertReview er = GetExpertReviewsByDocument(doc, countryId);
            return er;
        }

        private static Sort GetSort(int sortBy)
        {
            Sort sort = new Sort();

            if (sortBy == 1)
                sort = new Sort(new SortField[] { new SortField("IsExpertReview", SortField.STRING, true) });
            else if (sortBy == 2)
                sort = new Sort(new SortField[] { new SortField("ReviewDate", SortField.STRING, true) });
            else if (sortBy == 3)
                sort = new Sort(new SortField[] { new SortField("ReviewDate", SortField.STRING, false) });
            else if (sortBy == 4)
                sort = new Sort(new SortField[] { new SortField("PriceMeScore", SortField.FLOAT, true) });
            else if (sortBy == 5)
                sort = new Sort(new SortField[] { new SortField("PriceMeScore", SortField.FLOAT, false) });

            return sort;
        }

        private static ExpertReview GetExpertReviewsByDocument(Document hitDoc, int countryId)
        {
            ExpertReview er = new ExpertReview();

            int sourceId = 0;
            int.TryParse(hitDoc.Get("SourceID"), out sourceId);
            ExpertReviewSource reviews = ProductController.GetReviewSource(sourceId, countryId);
            if (reviews == null)
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

        public static List<CSK_Store_ProductReview> GetProductReviewByProductAndAuthor(int productID, string author, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return CSK_Store_ProductReview.Find(r => r.ProductID == productID && r.AuthorName == author).ToList();
            }
        }
    }
}