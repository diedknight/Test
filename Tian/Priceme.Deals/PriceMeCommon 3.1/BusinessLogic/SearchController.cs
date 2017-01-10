using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using PriceMeDBA;
using PriceMeCommon.Data;

namespace PriceMeCommon
{
    public static class SearchController
    {
        const int MAXDOCS = 100000;
        const int MAXClAUSECOUNT = 100000;
        const string SEARCHFIELDNAME = "SearchField";
        const string PRICERANGEFIELDNAME = "BestPrice";

        static List<string> InvalidKeywordsList = null;
        static List<string> InvalidStatementList = null;

        static System.Text.RegularExpressions.Regex regex = new Regex("");

        static SearchController()
        {
            BooleanQuery.MaxClauseCount = MAXClAUSECOUNT;
            SetInvalidKeywordsList();
        }

        public static void Init()
        {
            SetInvalidKeywordsList();
        }

        private static void SetInvalidKeywordsList()
        {
            InvalidKeywordsList = new List<string>();
            InvalidStatementList = new List<string>();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                string sql = @"select * from InvalidKeywords";
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = sql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = System.Data.CommandType.Text;
                    sqlCMD.Connection = sqlConn;

                    sqlConn.Open();

                    using (System.Data.SqlClient.SqlDataReader sdr = sqlCMD.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string invaildString = sdr["InvalidKeyword"].ToString();
                            if (invaildString.Contains(" "))
                            {
                                InvalidStatementList.Add(invaildString);
                            }
                            else
                            {
                                InvalidKeywordsList.Add(invaildString);
                            }
                        }
                    }
                }
            }
        }

        public static ProductCatalog SearchProducts(int productID)
        {
            return SearchProducts(productID.ToString());
        }

        public static ProductCatalog SearchProducts(string productID, int countryID)
        {
            Searcher searcher = GetSearcherByCategoryID(0, countryID);
            if (searcher == null)
            {
                return null;
            }

            TermQuery termQuery = new TermQuery(new Term("ProductID", productID));

            TopDocs topDocs = searcher.Search(termQuery, null, 1);

            if (topDocs.TotalHits > 0)
            {
                Document doc = searcher.Doc(topDocs.ScoreDocs[0].Doc);
                return GetProductCatalogFromDoc(doc);
            }

            return null;
        }

        public static ProductCatalog SearchProducts(string productID)
        {
            Searcher searcher = GetSearcherByCategoryID(0);
            if (searcher == null)
            {
                return null;
            }

            TermQuery termQuery = new TermQuery(new Term("ProductID", productID));

            TopDocs topDocs = searcher.Search(termQuery, null, 1);

            if (topDocs.TotalHits > 0)
            {
                Document doc = searcher.Doc(topDocs.ScoreDocs[0].Doc);
                return GetProductCatalogFromDoc(doc);
            }

            return null;
        }

        public static List<ProductCatalog> SearchProductsByPIDs(int[] productIDs, int countryID)
        {
            Searcher searcher = GetSearcherByCategoryID(0, countryID);
            if (searcher == null)
            {
                return null;
            }

            BooleanQuery productIDsQuery = new BooleanQuery();
            foreach (int pid in productIDs)
            {
                TermQuery termQuery = new TermQuery(new Term("ProductID", pid.ToString()));
                productIDsQuery.Add(termQuery, Occur.SHOULD);
            }

            TopDocs topDocs = searcher.Search(productIDsQuery, null, 100000);
            HitsInfo resultHitsInfo = new HitsInfo(searcher, topDocs);

            List<ProductCatalog> productCatalogList = new List<ProductCatalog>();
            for (int i = 0; i < topDocs.TotalHits; i++)
            {
                ProductCatalog productCatalog = SearchController.GetProductCatalog(resultHitsInfo, i);
                productCatalogList.Add(productCatalog);
            }

            return productCatalogList;
        }

        //public static HitsInfo SearchProducts(string keywords, int categoryID, int manufacturerID, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, int retailerID, bool useIsDisplayIsMerged, bool searchMerged, int maxDocCount, int countryID, bool isTest)
        //{
        //    Searcher searcher = GetSearcherByCategoryID(categoryID, countryID);
        //    if (searcher == null)
        //    {
        //        return null;
        //    }
        //    string[] kwList = GetKeyWordsList(keywords);
        //    List<int> cidList = new List<int>();
        //    cidList.Add(categoryID);
        //    Query query = GetQuery(cidList, manufacturerID, attributeValueIDList, attributeRangeIDList, retailerID, searchMerged, false, useIsDisplayIsMerged, countryID, Occur.MUST, true, false);
        //    Filter filter = GetPriceRangeFilter(priceRange);
        //    Sort sort = GetSort(sortby);

        //    TopFieldDocs topFieldDocs = null;

        //    if (kwList != null && kwList.Length > 0)
        //    {
        //        Query newQuery = GetQuery(kwList, Occur.MUST, query, false);
        //        topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
        //        if (topFieldDocs.ScoreDocs.Length == 0 && kwList.Length > 1)
        //        {
        //            newQuery = GetQuery(kwList, Occur.MUST, query, true);
        //            topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);

        //            if (topFieldDocs.ScoreDocs.Length == 0)
        //            {
        //                newQuery = GetQuery(kwList, Occur.SHOULD, query, true);
        //                topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);

        //                if (topFieldDocs.ScoreDocs.Length > 300)
        //                {
        //                    bool modified;
        //                    kwList = GetFixedKWList(kwList, out modified, true);
        //                    if (modified)
        //                    {
        //                        newQuery = GetQuery(kwList, Occur.MUST, query, true);
        //                        topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
        //                    }
        //                }
        //                else if (topFieldDocs.ScoreDocs.Length > 20)
        //                {
        //                    bool modified;
        //                    kwList = GetFixedKWList(kwList, out modified, false);
        //                    if (modified)
        //                    {
        //                        newQuery = GetQuery(kwList, Occur.MUST, query, true);
        //                        TopFieldDocs _topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
        //                        if (topFieldDocs.ScoreDocs.Length > _topFieldDocs.ScoreDocs.Length)
        //                        {
        //                            topFieldDocs = _topFieldDocs;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (query == null)
        //        {
        //            query = new TermQuery(new Term("IsDisplay", "true"));
        //        }
        //        topFieldDocs = searcher.Search(query, filter, maxDocCount, sort);
        //    }

        //    return new HitsInfo(searcher, topFieldDocs);
        //}

        //public static HitsInfo SearchProductsAdmin(string keywords, int categoryID, int manufacturerID, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, int retailerID, bool useIsDisplayIsMerged, bool searchMerged, int maxDocCount, int countryID)
        //{
        //    return SearchProducts(keywords, categoryID, manufacturerID, priceRange, attributeValueIDList,
        //                          attributeRangeIDList, sortby, retailerID, useIsDisplayIsMerged, searchMerged, false, maxDocCount,
        //                          countryID, false, true);
        //}

        public static HitsInfo SearchProducts(string keywords, int categoryID, List<int> manufacturerIDs, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, List<int> retailerIDs, bool useIsDisplayIsMerged, bool searchMerged, bool categoryMerged, int maxDocCount, int countryID, bool multiAttribute, bool includeAccessories, bool isSearchonly)
        {
            List<int> cidList = new List<int>();
            cidList.Add(categoryID);
            return SearchProducts(keywords, cidList, manufacturerIDs, priceRange, attributeValueIDList, attributeRangeIDList, sortby, retailerIDs, useIsDisplayIsMerged, searchMerged, categoryMerged, maxDocCount, countryID, multiAttribute, includeAccessories, false, null, isSearchonly, null);
        }
        public static HitsInfo SearchProducts(string keywords, int categoryID, List<int> manufacturerIDs, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, List<int> retailerIDs, bool useIsDisplayIsMerged, bool searchMerged, bool categoryMerged, int maxDocCount, int countryID, bool multiAttribute, bool includeAccessories,
            Dictionary<int, string> attrValueRange, bool isSearchonly, DaysRange daysRange)
        {
            List<int> cidList = new List<int>();
            cidList.Add(categoryID);
            return SearchProducts(keywords, cidList, manufacturerIDs, priceRange, attributeValueIDList, attributeRangeIDList, sortby, retailerIDs, useIsDisplayIsMerged, searchMerged, categoryMerged, maxDocCount, countryID, multiAttribute, includeAccessories, false, attrValueRange, isSearchonly, daysRange);
        }

        /// <summary>
        /// PriceMe WebSite
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="categoryIDList"></param>
        /// <param name="manufacturerIDs"></param>
        /// <param name="priceRange"></param>
        /// <param name="attributeValueIDList"></param>
        /// <param name="attributeRangeIDList"></param>
        /// <param name="sortby"></param>
        /// <param name="retailerID"></param>
        /// <param name="useIsDisplayIsMerged"></param>
        /// <param name="searchMerged"></param>
        /// <param name="categoryMerged"></param>
        /// <param name="maxDocCount"></param>
        /// <param name="countryID"></param>
        /// <param name="multiAttribute"></param>
        /// <param name="includeAccessories"></param>
        /// <param name="ppcOnly"></param>
        /// <param name="attrValueRange"></param>
        /// <returns></returns>
        public static HitsInfo SearchProducts(string keywords, List<int> categoryIDList, List<int> manufacturerIDs, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, List<int> retailerIDs, bool useIsDisplayIsMerged, bool searchMerged, bool categoryMerged, int maxDocCount, int countryID, bool multiAttribute, bool includeAccessories, bool ppcOnly,
            Dictionary<int, string> attrValueRange, bool isSearchonly, DaysRange daysRange, bool sale = false, double saleMinimumPrice = -0.01d)
        {
            if (maxDocCount > MAXDOCS)
            {
                maxDocCount = MAXDOCS;
            }

            Lucene.Net.Search.Occur occur;
            if (multiAttribute)
            {
                occur = Occur.SHOULD;
            }
            else
            {
                occur = Occur.MUST;
            }
            Searcher searcher = null;
            if (categoryIDList == null || categoryIDList.Count == 0 || categoryIDList.Count > 1)
            {
                searcher = GetSearcherByCategoryID(0, countryID);
            }
            else if (categoryIDList != null && categoryIDList.Count == 1)
            {
                searcher = GetSearcherByCategoryID(categoryIDList[0], countryID);
            }
            if (searcher == null)
            {
                return null;
            }

            string[] kwList = GetKeyWordsList(keywords);
            Query query = GetQuery(categoryIDList, manufacturerIDs, attributeValueIDList, attributeRangeIDList, retailerIDs, searchMerged, categoryMerged, useIsDisplayIsMerged, countryID, occur, includeAccessories, ppcOnly, attrValueRange, isSearchonly, daysRange, sale, saleMinimumPrice);
            Filter filter = GetPriceRangeFilter(priceRange);
            Sort sort = GetSort(sortby);

            TopFieldDocs topFieldDocs = null;

            if (kwList != null && kwList.Length > 0)
            {
                Query newQuery = GetQuery(kwList, Occur.MUST, query, false);
                topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
                if (topFieldDocs.TotalHits == 0 && kwList.Length > 1)
                {
                    newQuery = GetQuery(kwList, Occur.MUST, query, true);
                    topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);

                    if (topFieldDocs.TotalHits == 0)
                    {
                        newQuery = GetQuery(kwList, Occur.SHOULD, query, true);
                        topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);

                        if (topFieldDocs.TotalHits >= 1000)
                        {
                            bool modified;
                            kwList = GetFixedKWList(kwList, out modified, true);
                            if (modified)
                            {
                                newQuery = GetQuery(kwList, Occur.MUST, query, true);
                                topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
                            }
                        }
                        else if (topFieldDocs.TotalHits > 300 || topFieldDocs.TotalHits == 0)
                        {
                            bool modified;
                            kwList = GetFixedKWList(kwList, out modified, true);
                            if (modified)
                            {
                                newQuery = GetQuery(kwList, Occur.MUST, query, true);
                                TopFieldDocs _topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
                                if (_topFieldDocs.TotalHits != 0 && topFieldDocs.TotalHits > _topFieldDocs.TotalHits || topFieldDocs.TotalHits == 0)
                                {
                                    topFieldDocs = _topFieldDocs;
                                }
                            }
                        }
                        //如果没有搜索结果就查找RetailerProductIndex
                        else if (topFieldDocs.TotalHits == 0)
                        {
                            newQuery = GetRetailerProductQuery(GetKeyWordsList(keywords), query);
                            TopFieldDocs _topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
                            topFieldDocs = _topFieldDocs;
                        }
                    }
                }
                else if (topFieldDocs.TotalHits == 0)
                {
                    newQuery = GetQuery(kwList, Occur.MUST, query, true);
                    topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);

                    if (topFieldDocs.TotalHits == 0)
                    {
                        newQuery = GetRetailerProductQuery(GetKeyWordsList(keywords), query);
                        if (newQuery != null)
                        {
                            TopFieldDocs _topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
                            topFieldDocs = _topFieldDocs;
                        }
                    }
                }
            }
            else
            {
                if (query == null)
                {
                    query = new TermQuery(new Term("IsDisplay", "true"));
                }
                topFieldDocs = searcher.Search(query, filter, maxDocCount, sort);
            }

            return new HitsInfo(searcher, topFieldDocs);
        }

        private static Query GetRetailerProductQuery(string[] kwList, Query query)
        {
            BooleanQuery queries = new BooleanQuery();
            BooleanQuery keywordsQuery = new BooleanQuery();
            string queryStringFormat = "*{0}*";
            foreach (string kw in kwList)
            {
                Lucene.Net.Index.Term term = new Lucene.Net.Index.Term("SearchField", string.Format(queryStringFormat, kw.ToLower()));
                WildcardQuery wildcardQuery = new WildcardQuery(term);
                keywordsQuery.Add(wildcardQuery, Occur.SHOULD);
            }
            TopFieldDocs topFieldDocs = LuceneController.RetailerProductsIndexSearcher.Search(keywordsQuery, null, 1000, new Sort());
            BooleanQuery rpProductIDQuery = null;
            if (topFieldDocs.TotalHits > 0)
            {
                rpProductIDQuery = new BooleanQuery();
                for (int i = 0; i < topFieldDocs.ScoreDocs.Count(); i++)
                {
                    var docm = LuceneController.RetailerProductsIndexSearcher.Doc(topFieldDocs.ScoreDocs[i].Doc);
                    string pid = docm.Get("ProductID");
                    TermQuery pidTermQuery = new TermQuery(new Term("ProductID", pid));
                    rpProductIDQuery.Add(pidTermQuery, Occur.SHOULD);
                }
                queries.Add(rpProductIDQuery, Occur.MUST);
                return queries;
            }

            return null;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public static HitsInfo SearchProducts(string keywords, int categoryID, List<int> manufacturerIDs, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, int retailerID, bool useIsDisplayIsMerged, bool searchMerged, bool categoryMerged, int maxDocCount, int countryID, bool multiAttribute, bool includeAccessories, List<int> paCids)
        //{
        //    if (maxDocCount > MAXDOCS)
        //    {
        //        maxDocCount = MAXDOCS;
        //    }

        //    Lucene.Net.Search.Occur occur;
        //    if (multiAttribute)
        //    {
        //        occur = Occur.SHOULD;
        //    }
        //    else
        //    {
        //        occur = Occur.MUST;
        //    }
        //    Searcher searcher = GetSearcherByCategoryID(categoryID, countryID);
        //    if (searcher == null)
        //    {
        //        return null;
        //    }

        //    string[] kwList = GetKeyWordsList(keywords);
        //    Query query = GetQuery_CateIDs(categoryID, paCids, manufacturerIDs, attributeValueIDList, attributeRangeIDList, retailerID, searchMerged, categoryMerged, useIsDisplayIsMerged, countryID, occur, includeAccessories);

        //    Filter filter = GetPriceRangeFilter(priceRange);
        //    Sort sort = GetSort(sortby);

        //    TopFieldDocs topFieldDocs = null;

        //    if (kwList != null && kwList.Length > 0)
        //    {
        //        Query newQuery = GetQuery(kwList, Occur.MUST, query, false);
        //        topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
        //        if (topFieldDocs.TotalHits == 0 && kwList.Length > 1)
        //        {
        //            newQuery = GetQuery(kwList, Occur.MUST, query, true);
        //            topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);

        //            if (topFieldDocs.TotalHits == 0)
        //            {
        //                newQuery = GetQuery(kwList, Occur.SHOULD, query, true);
        //                topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);

        //                if (topFieldDocs.TotalHits >= 1000)
        //                {
        //                    bool modified;
        //                    kwList = GetFixedKWList(kwList, out modified, true);
        //                    if (modified)
        //                    {
        //                        newQuery = GetQuery(kwList, Occur.MUST, query, true);
        //                        topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
        //                    }
        //                }
        //                else if (topFieldDocs.TotalHits > 300 || topFieldDocs.TotalHits == 0)
        //                {
        //                    bool modified;
        //                    kwList = GetFixedKWList(kwList, out modified, true);
        //                    if (modified)
        //                    {
        //                        newQuery = GetQuery(kwList, Occur.MUST, query, true);
        //                        TopFieldDocs _topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
        //                        if (_topFieldDocs.TotalHits != 0 && topFieldDocs.TotalHits > _topFieldDocs.TotalHits || topFieldDocs.TotalHits == 0)
        //                        {
        //                            topFieldDocs = _topFieldDocs;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else if (topFieldDocs.TotalHits == 0)
        //        {
        //            newQuery = GetQuery(kwList, Occur.MUST, query, true);
        //            topFieldDocs = searcher.Search(newQuery, filter, maxDocCount, sort);
        //        }
        //    }
        //    else
        //    {
        //        if (query == null)
        //        {
        //            query = new TermQuery(new Term("IsDisplay", "true"));
        //        }
        //        topFieldDocs = searcher.Search(query, filter, maxDocCount, sort);
        //    }

        //    return new HitsInfo(searcher, topFieldDocs);
        //}

        public static HitsInfo SearchProducts(string keywords, int categoryID, List<int> manufacturerIDs, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, List<int> retailerIDs, bool useIsDisplayIsMerged, bool categoryMerged, int maxDocCount, bool isSearchonly)
        {
            return SearchProducts(keywords, categoryID, manufacturerIDs, priceRange, attributeValueIDList,
                                  attributeRangeIDList, sortby, retailerIDs, useIsDisplayIsMerged, true, false, maxDocCount,
                                  ConfigAppString.CountryID, false, true, isSearchonly);
        }

        public static HitsInfo SearchProducts(string keywords, int categoryID, List<int> manufacturerIDs, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, List<int> retailerIDs, bool useIsDisplayIsMerged, bool searchMerged, int maxDocCount, int countryID, bool isSearchonly)
        {
            return SearchProducts(keywords, categoryID, manufacturerIDs, priceRange, attributeValueIDList,
                                  attributeRangeIDList, sortby, retailerIDs, useIsDisplayIsMerged, searchMerged, false, maxDocCount,
                                  countryID, false, true, isSearchonly);
        }

        private static string[] GetFixedKWList(string[] kwList, out bool modified, bool cut)
        {
            modified = false;
            string[] fixedKws = new string[kwList.Length];
            for (int i = 0; i < kwList.Length; i++)
            {
                if (cut && i >= 2)
                    break;
                if (kwList[i].Length >= 3 && kwList[i].Length < 7)
                {
                    int count = (kwList[i].Length - 1) / 2;
                    fixedKws[i] = kwList[i].Substring(0, kwList[i].Length - count);
                    modified = true;
                }
                else if (kwList[i].Length >= 7)
                {
                    int count = (kwList[i].Length - 1) / 2;
                    fixedKws[i] = kwList[i].Substring(1, kwList[i].Length - 1 - count);
                    modified = true;
                }
                else
                {
                    fixedKws[i] = kwList[i];
                }
            }
            return fixedKws;
        }

        public static HitsInfo SearchProducts(int categoryID, string keywords, bool isSearchonly)
        {
            return SearchProducts(keywords, categoryID, null, null, null, null, null, null, true, false, MAXDOCS, ConfigAppString.CountryID, isSearchonly);
        }

        public static HitsInfo SearchAttributes(List<string> productIDList)
        {
            if (productIDList != null && productIDList.Count > 0 && LuceneController.AttributesIndexSearcher != null)
            {
                BooleanQuery booleanQuery = new BooleanQuery();
                foreach (string pid in productIDList)
                {
                    TermQuery productIDTermQuery = new TermQuery(new Term("ProductID", pid));
                    booleanQuery.Add(productIDTermQuery, Occur.SHOULD);
                }

                TopDocs topDocs = LuceneController.AttributesIndexSearcher.Search(booleanQuery, MAXDOCS);

                return new HitsInfo(LuceneController.AttributesIndexSearcher, topDocs);
            }

            return null;
        }

        #region ruangang
        public static HitsInfo SearchCategories(string keywords)
        {
            keywords = keywords.ToLower();
            Searcher searcher = LuceneController.CategoriesIndexSearcher;
            if (searcher == null) return null;

            TermQuery tq = new TermQuery(new Term("CategoryName", keywords));
            TopDocs docs = searcher.Search(tq, 1);

            if (docs.ScoreDocs.Length == 0)
            {
                keywords = Regex.Match(keywords, "s$").Success ? Regex.Replace(keywords, "s$", "") : keywords + "s";
                tq = new TermQuery(new Term("CategoryName", keywords));
                docs = searcher.Search(tq, 1);
            }

            if (docs.ScoreDocs.Length == 0)
                return null;

            return new HitsInfo(searcher, docs);
        }

        //public static List<int> GetAttributeValuesByProductID(int productid) {
        //    if (productid <= 0 || LuceneController.AttributesIndexSearcher == null)
        //    {
        //        return new List<int>();
        //    }

        //    BooleanQuery queries = new BooleanQuery();
        //    TermQuery pidTermQuery = new TermQuery(new Term("ProductID", productid.ToString()));
        //    queries.Add(pidTermQuery, Occur.MUST);

        //    List<int> avids = new List<int>();
        //    TopDocs tds = LuceneController.AttributesIndexSearcher.Search(queries, 20);
        //    if (tds.ScoreDocs.Length == 0)
        //    {
        //        return avids;
        //    }

        //    HitsInfo hi = new HitsInfo(LuceneController.AttributesIndexSearcher, tds);

        //    for (int i = 0; i < hi.ResultCount; i++) {
        //        string avidString = hi.GetDocument(i).Get("AttributeValueID");
        //        if (!string.IsNullOrEmpty(avidString)) {
        //            avids.Add(int.Parse(avidString));
        //        }
        //    }

        //    //for (int i = 0; i < hits.Length(); i++) {
        //    //    string avidString = hits.Doc(i).Get("AttributeValueID");
        //    //    if (!string.IsNullOrEmpty(avidString)) {
        //    //        int avid = int.Parse(avidString);
        //    //        avids.Add(avid);
        //    //    }
        //    //}

        //    return avids;
        //}


        #endregion

        public static HitsInfo SearchRetailer(List<string> productIDList, int countryID)
        {
            Searcher indexSearcher = null;

            indexSearcher = LuceneController.GetProductRetailerMapIndexSearcher(countryID);
            if (productIDList != null && productIDList.Count > 0)
            {
                BooleanQuery booleanQuery = new BooleanQuery();
                foreach (string pid in productIDList)
                {
                    TermQuery productIDTermQuery = new TermQuery(new Term("ProductID", pid));
                    booleanQuery.Add(productIDTermQuery, Occur.SHOULD);
                }

                TopDocs topDocs = indexSearcher.Search(booleanQuery, MAXDOCS);

                return new HitsInfo(indexSearcher, topDocs);
            }

            return null;
        }

        #region other

        static Filter GetPriceRangeFilter(PriceRange priceRange)
        {
            if (priceRange == null)
            {
                return null;
            }
            NumericRangeFilter<double> numericRangeFilter = NumericRangeFilter.NewDoubleRange(PRICERANGEFIELDNAME, priceRange.MinPrice, priceRange.MaxPrice, true, false);
            return numericRangeFilter;
        }

        private static BooleanQuery GetCategoryQuery(int categoryID, bool isSearchonly)
        {
            List<int> cidList = new List<int>();
            cidList.Add(categoryID);
            return GetCategoryQuery(cidList, isSearchonly);
        }

        public static BooleanQuery GetCategoryQuery(List<int> categoryIDList, bool isSearchonly)
        {
            if (categoryIDList == null || categoryIDList.Count == 0)
            {
                return null;
            }
            //当搜索所有的index且不需要指定分类时，不要生成category的搜索条件，不然会严重影响搜索速度
            BooleanQuery categoryQuery = new BooleanQuery();
            foreach (int categoryID in categoryIDList)
            {
                if (categoryID > 0)
                {
                    string[] cids = GetSubCategoryIDs(categoryID);
                    foreach (string cid in cids)
                    {
                        if (isSearchonly)
                        {
                            int categoryid = 0;
                            int.TryParse(cid, out categoryid);
                            if (CategoryController.listIsSearchOnly != null && CategoryController.listIsSearchOnly.Contains(categoryid))
                                continue;
                        }
                        TermQuery termQuery = new TermQuery(new Term("CategoryID", cid));
                        categoryQuery.Add(termQuery, Occur.SHOULD);
                    }
                }
            }

            if (categoryQuery.GetClauses().Length > 0)
            {
                return categoryQuery;
            }
            return null;
        }

        private static BooleanQuery GetCategoryQuerys(int cateID, List<int> allowPaIDs)
        {
            BooleanQuery categoryQuery = new BooleanQuery();
            if (allowPaIDs != null && allowPaIDs.Count > 0)
            {
                foreach (var categoryID in allowPaIDs)
                {
                    if (categoryID > 0)
                    {
                        //var cates = CategoryController.GetNextLevelSubCategoriesIsActive(categoryID);
                        //foreach (var cate in cates)
                        //{
                        //    TermQuery termQuery = new TermQuery(new Term("CategoryID", cate.CategoryID.ToString()));
                        //    categoryQuery.Add(termQuery, BooleanClause.Occur.SHOULD);
                        //}
                        string[] cids = GetSubCategoryIDs(categoryID);
                        foreach (string cid in cids)
                        {
                            TermQuery termQuery = new TermQuery(new Term("CategoryID", cid));
                            categoryQuery.Add(termQuery, Occur.SHOULD);
                        }
                    }
                }
                //当搜索所有的index且不需要指定分类时，不要生成category的搜索条件，不然会严重影响搜索速度
                //else if (categoryID == 0)
                //{
                //    foreach (CSK_Store_Category c in CategoryController.CategoryOrderByName)
                //    {
                //        TermQuery termQuery = new TermQuery(new Term("CategoryID", c.CategoryID.ToString()));
                //        categoryQuery.Add(termQuery, BooleanClause.Occur.SHOULD);
                //    }
                //}
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
            if (topDocs != null && topDocs.ScoreDocs.Length > 0)
            {
                Lucene.Net.Documents.MapFieldSelector mapFieldSelector = new Lucene.Net.Documents.MapFieldSelector(new string[] { "SubCategoriesString" });
                Lucene.Net.Documents.Document doc = LuceneController.CategoriesIndexSearcher.Doc(topDocs.ScoreDocs[0].Doc, mapFieldSelector);

                return doc.Get("SubCategoriesString");
            }
            return cid.ToString();
        }

        private static Sort GetSort(string sortBy)
        {
            Sort sort = null;

            if (string.IsNullOrEmpty(sortBy))
            {
                sort = new Sort();
            }
            else if (sortBy.Equals("accessories", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField[] { new SortField("IsAccessory", SortField.INT, false) });
            }
            else if (sortBy.Equals("accessories-rev", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField[] { new SortField("IsAccessory", SortField.INT, true) });
            }
            else if (sortBy.Equals("adminclicks", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField[] { new SortField("Clicks", SortField.INT, true) });
            }
            else if (sortBy.Equals("clicks", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField[] { new SortField("IsAccessory", SortField.INT, false), new SortField("Clicks", SortField.INT, true) });
            }
            else if (sortBy.Equals("clicks-rev", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField[] { new SortField("IsAccessory", SortField.INT, true), new SortField("Clicks", SortField.INT, false) });
            }
            else if (sortBy.Equals("bestprice", StringComparison.InvariantCultureIgnoreCase) || sortBy.Equals("ismerge", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField("BestPrice", SortField.DOUBLE));
            }
            else if (sortBy.Equals("bestprice-rev", StringComparison.InvariantCultureIgnoreCase) || sortBy.Equals("ismerge", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField("BestPrice", SortField.DOUBLE, true));
            }
            else if (sortBy.Equals("title", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField("Title", SortField.STRING));
            }
            else if (sortBy.Equals("title-rev", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField("Title", SortField.STRING, true));
            }
            else if (sortBy.Equals("rating", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField("AvRating", SortField.FLOAT, true));
            }
            else if (sortBy.Equals("rating-rev", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField("AvRating", SortField.FLOAT, false));
            }
            else if (sortBy.Equals("help", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField("HelpListOrder", SortField.INT, true));
            }
            else if (sortBy.Equals("newest", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField("DayCount", SortField.INT, false));
            }
            else if (sortBy.Equals("sale", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField("Sale", SortField.DOUBLE, false));
            }
            else
            {
                sort = new Sort(new SortField(sortBy, SortField.STRING));
            }

            return sort;
        }

        private static string[] GetKeyWordsList(string keyWords)
        {
            if (!string.IsNullOrEmpty(keyWords))
            {
                keyWords = FixKeyWords(keyWords);

                keyWords = keyWords.Length > 80 ? keyWords.Substring(0, 70).ToLower() : keyWords.ToLower();
                string[] kws = keyWords.Split(new char[] { ' ', '-', '~', '&', '!', '#', '@', '^', '?', '*', '\"', '[', ']', '{', '}', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> kwsList = new List<string>(kws);

                kwsList = FilterKws(kwsList);
                if (kwsList.Count > 0)
                {
                    if (kwsList.Count > 7)
                    {
                        kws = kwsList.Take(7).ToArray();
                    }
                    else
                    {
                        kws = kwsList.ToArray();
                    }
                }
                else
                {
                    if (kws.Length > 7)
                        kws = kws.Take(7).ToArray();
                }
                return kws;
            }

            return null;
        }

        private static string FixKeyWords(string keyWords)
        {
            foreach (string str in InvalidStatementList)
            {
                if (keyWords.StartsWith(str))
                {
                    keyWords = keyWords.Substring(str.Length);
                }
                else if (keyWords.EndsWith(str))
                {
                    keyWords = keyWords.Substring(0, keyWords.Length - str.Length);
                }
                if (keyWords.Contains(" " + str + " "))
                {
                    keyWords = keyWords.Replace(" " + str + " ", " ");
                }
            }
            return keyWords;
        }

        private static List<string> FilterKws(List<string> kwsList)
        {
            for (int i = 0; i < kwsList.Count;)
            {
                if (InvalidKeywordsList.Contains(kwsList[i]))
                {
                    kwsList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            return kwsList;
        }

        //static BooleanQuery GetQuery(int categoryID, Query otherQuery)
        //{
        //    BooleanQuery queries = new BooleanQuery();
        //    if (categoryID >= 0)
        //    {
        //        List<int> cidList = new List<int>();
        //        cidList.Add(categoryID);
        //        BooleanQuery categoryQuery = GetCategoryQuery(cidList);
        //        if (categoryQuery != null)
        //        {
        //            queries.Add(categoryQuery, Occur.MUST);
        //        }
        //    }
        //    if (otherQuery != null)
        //    {
        //        queries.Add(otherQuery, Occur.MUST);
        //    }

        //    if (queries.GetClauses().Length > 0)
        //    {
        //        return queries;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public static HitsInfo SearchRetailerProduct(int ratailerId, int categoryId, int count, bool isSearchonly)
        {
            BooleanQuery booleanQuery = new BooleanQuery();
            if (ratailerId > 0)
            {
                TermQuery mergeQuery = new TermQuery(new Term("RetailerId", ratailerId.ToString()));
                booleanQuery.Add(mergeQuery, Occur.MUST);
            }
            if (categoryId > 0)
            {
                Query categoryQueries = GetCategoryQuery(categoryId, isSearchonly);
                booleanQuery.Add(categoryQueries, Occur.MUST);
            }

            Sort sort = new Sort(new SortField[] { new SortField("Clicks", SortField.INT, true) });

            Searcher searcher = LuceneController.RetailerProductsIndexSearcher;

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, count, sort);

            return new HitsInfo(searcher, topFieldDocs);
        }

        public static string SearchRetailerProductMPN(string MPN, float similar)
        {
            string fieldName = "MPN";
            Lucene.Net.Analysis.WhitespaceAnalyzer whitespaceAnalyzer = new Lucene.Net.Analysis.WhitespaceAnalyzer();

            System.IO.TextReader textReader = new System.IO.StringReader(MPN.ToLower());
            Lucene.Net.Analysis.TokenStream tokenStream = whitespaceAnalyzer.TokenStream(fieldName, textReader);
            Lucene.Net.Analysis.Tokenattributes.ITermAttribute termAttribute = tokenStream.AddAttribute<Lucene.Net.Analysis.Tokenattributes.ITermAttribute>();

            BooleanQuery booleanQuery = new BooleanQuery();
            while (tokenStream.IncrementToken())
            {
                Query query = new FuzzyQuery(new Term(fieldName, termAttribute.Term), similar, 0);
                booleanQuery.Add(query, Occur.MUST);
            }

            Searcher searcher = LuceneController.RetailerProductsIndexSearcher;
            TopDocs topDocs = searcher.Search(booleanQuery, 100);

            if (topDocs.TotalHits > 0)
            {
                return searcher.Doc(topDocs.ScoreDocs[0].Doc, new Lucene.Net.Documents.MapFieldSelector(fieldName)).Get(fieldName);
            }
            return string.Empty;
        }

        public static HitsInfo SearchRetailerProduct(int ratailerId, int categoryId, bool isSearchonly)
        {
            return SearchRetailerProduct(ratailerId, categoryId, MAXDOCS, isSearchonly);
        }

        private static Query GetQuery(
            List<int> cidList,
            List<int> manufacturerIDs,
            List<int> attributeValueIDList,
            List<int> attributeRangeIDList,
            List<int> retailerIDs,
            bool searchMerged,
            bool categoryMerged, //是不是只查找需要显示的分类中没有合并的产品
            bool useIsDisplayIsMerged,
            int countryID,
            Occur occur,
            bool includeAccessories,
            bool ppcOnly, bool isSearchonly)
        {
            Query categoryQueries = GetCategoryQuery(cidList, isSearchonly);
            Query attributeQueries = null;
            if ((attributeRangeIDList != null && attributeRangeIDList.Count > 0) || (attributeValueIDList != null && attributeValueIDList.Count > 0))
            {
                attributeQueries = GetAttributeQuery(attributeValueIDList, attributeRangeIDList, occur);
            }
            Query retailerQueries = GetRetailerQuery(retailerIDs, countryID);

            BooleanQuery booleanQuery = new BooleanQuery();


            if (categoryMerged)
            {
                TermQuery mergeQuery = new TermQuery(new Term("IsDisplay", "false"));
                booleanQuery.Add(mergeQuery, Occur.MUST);
            }
            else
            {
                if (useIsDisplayIsMerged)
                {
                    //TermQuery mergeQuery = new TermQuery(new Term("IsDisplay", "true"));
                    //booleanQuery.Add(mergeQuery, Occur.MUST);
                }

                if (!searchMerged)
                {
                    TermQuery isMergeTermQuery = new TermQuery(new Term("IsMerge", "False"));
                    booleanQuery.Add(isMergeTermQuery, Occur.MUST);
                }
            }

            if (manufacturerIDs != null && manufacturerIDs.Count > 0)
            {
                BooleanQuery brandBooleanQuery = new BooleanQuery();
                foreach (int brandID in manufacturerIDs)
                {
                    TermQuery manufacturerQuery = new TermQuery(new Term("ManufacturerID", brandID.ToString()));
                    brandBooleanQuery.Add(manufacturerQuery, Occur.SHOULD);
                }
                booleanQuery.Add(brandBooleanQuery, Occur.MUST);
            }

            if (categoryQueries != null)
            {
                booleanQuery.Add(categoryQueries, Occur.MUST);
            }

            if (attributeQueries != null)
            {
                booleanQuery.Add(attributeQueries, Occur.MUST);
            }

            if (!includeAccessories)
            {
                TermQuery manufacturerQuery = new TermQuery(new Term("IsAccessory", "0"));
                booleanQuery.Add(manufacturerQuery, Occur.MUST);
            }

            if (ppcOnly)
            {
                TermQuery manufacturerQuery = new TermQuery(new Term("IncludePPC", "1"));
                booleanQuery.Add(manufacturerQuery, Occur.MUST);
            }

            //为webserivce改的位置？为啥要放到后面？
            if (retailerQueries != null)
            {
                booleanQuery.Add(retailerQueries, Occur.MUST);
            }

            if (booleanQuery.GetClauses().Length == 0)
            {
                return null;
            }

            return booleanQuery;
        }

        private static Query GetQuery(
            List<int> cidList,
            List<int> manufacturerIDs,
            List<int> attributeValueIDList,
            List<int> attributeRangeIDList,
            List<int> retailerIDs,
            bool searchMerged,
            bool categoryMerged, //是不是只查找需要显示的分类中没有合并的产品
            bool useIsDisplayIsMerged,
            int countryID,
            Occur occur,
            bool includeAccessories,
            bool ppcOnly,
            Dictionary<int, string> attrValueRange,
            bool isSearchonly,
            DaysRange daysRange, bool sale, double saleMinimumPrice)
        {
            Query categoryQueries = GetCategoryQuery(cidList, isSearchonly);
            Query attributeQueries = null;
            if ((attributeRangeIDList != null && attributeRangeIDList.Count > 0) || (attributeValueIDList != null && attributeValueIDList.Count > 0))
            {
                attributeQueries = GetAttributeQuery(attributeValueIDList, attributeRangeIDList, occur);
            }
            Query retailerQueries = GetRetailerQuery(retailerIDs, countryID);

            Query attrValueRangeQueries = null;
            if (attrValueRange != null && attrValueRange.Count > 0)
            {
                attrValueRangeQueries = GetAttributeValueRangeQuery(attrValueRange);
            }
            BooleanQuery booleanQuery = new BooleanQuery();
            if (attrValueRangeQueries != null)
            {
                booleanQuery.Add(attrValueRangeQueries, Occur.MUST);
            }

            if (daysRange != null && daysRange.MaxDays > 0)
            {
                NumericRangeQuery<int> daysQuery = NumericRangeQuery.NewIntRange("DayCount", daysRange.MinDays, daysRange.MaxDays, true, true);
                booleanQuery.Add(daysQuery, Occur.MUST);
            }

            //Sale
            if (sale)
            {

                booleanQuery.Add(NumericRangeQuery.NewDoubleRange("Sale", -0.85d, saleMinimumPrice, true, true), Occur.MUST);
            }

            if (categoryMerged)
            {
                TermQuery mergeQuery = new TermQuery(new Term("IsDisplay", "false"));
                booleanQuery.Add(mergeQuery, Occur.MUST);
            }
            else
            {
                if (useIsDisplayIsMerged)
                {
                    //TermQuery mergeQuery = new TermQuery(new Term("IsDisplay", "true"));
                    //booleanQuery.Add(mergeQuery, Occur.MUST);
                }

                if (!searchMerged)
                {
                    TermQuery isMergeTermQuery = new TermQuery(new Term("IsMerge", "False"));
                    booleanQuery.Add(isMergeTermQuery, Occur.MUST);
                }
            }

            if (manufacturerIDs != null && manufacturerIDs.Count > 0)
            {
                BooleanQuery brandBooleanQuery = new BooleanQuery();
                foreach (int brandID in manufacturerIDs)
                {
                    TermQuery manufacturerQuery = new TermQuery(new Term("ManufacturerID", brandID.ToString()));
                    brandBooleanQuery.Add(manufacturerQuery, Occur.SHOULD);
                }
                booleanQuery.Add(brandBooleanQuery, Occur.MUST);
            }

            if (categoryQueries != null)
            {
                booleanQuery.Add(categoryQueries, Occur.MUST);
            }

            if (attributeQueries != null)
            {
                booleanQuery.Add(attributeQueries, Occur.MUST);
            }

            if (!includeAccessories)
            {
                TermQuery manufacturerQuery = new TermQuery(new Term("IsAccessory", "0"));
                booleanQuery.Add(manufacturerQuery, Occur.MUST);
            }

            if (ppcOnly)
            {
                TermQuery manufacturerQuery = new TermQuery(new Term("IncludePPC", "1"));
                booleanQuery.Add(manufacturerQuery, Occur.MUST);
            }

            //为webserivce改的位置？为啥要放到后面？
            if (retailerQueries != null)
            {
                booleanQuery.Add(retailerQueries, Occur.MUST);
            }

            if (booleanQuery.GetClauses().Length == 0)
            {
                return null;
            }

            return booleanQuery;
        }
        /// <summary>
        /// 按多个分类ID获取query
        /// </summary>
        private static Query GetQuery_CateIDs(int categoryID,
            List<int> allowPaIDs,
            List<int> manufacturerIDs,
            List<int> attributeValueIDList,
            List<int> attributeRangeIDList,
            List<int> retailerIDs,
            bool searchMerged,
            bool categoryMerged, //是不是只查找需要显示的分类中没有合并的产品
            bool useIsDisplayIsMerged,
            int countryID,
            Occur occur,
            bool includeAccessories)
        {
            Query categoryQueries = GetCategoryQuerys(categoryID, allowPaIDs);
            Query attributeQueries = null;
            if ((attributeRangeIDList != null && attributeRangeIDList.Count > 0) || (attributeValueIDList != null && attributeValueIDList.Count > 0))
            {
                attributeQueries = GetAttributeQuery(attributeValueIDList, attributeRangeIDList, occur);
            }
            Query retailerQueries = GetRetailerQuery(retailerIDs, countryID);

            BooleanQuery booleanQuery = new BooleanQuery();

            if (categoryMerged)
            {
                TermQuery mergeQuery = new TermQuery(new Term("IsDisplay", "false"));
                booleanQuery.Add(mergeQuery, Occur.MUST);
            }
            else
            {
                if (useIsDisplayIsMerged)
                {
                    //TermQuery mergeQuery = new TermQuery(new Term("IsDisplay", "true"));
                    //booleanQuery.Add(mergeQuery, BooleanClause.Occur.MUST);
                }

                if (!searchMerged)
                {
                    TermQuery isMergeTermQuery = new TermQuery(new Term("IsMerge", "False"));
                    booleanQuery.Add(isMergeTermQuery, Occur.MUST);
                }
            }

            if (manufacturerIDs != null && manufacturerIDs.Count > 0)
            {
                BooleanQuery brandBooleanQuery = new BooleanQuery();
                foreach (int brandID in manufacturerIDs)
                {
                    TermQuery manufacturerQuery = new TermQuery(new Term("ManufacturerID", brandID.ToString()));
                    brandBooleanQuery.Add(manufacturerQuery, Occur.SHOULD);
                }
                booleanQuery.Add(brandBooleanQuery, Occur.MUST);
            }

            if (categoryQueries != null)
            {
                booleanQuery.Add(categoryQueries, Occur.MUST);
            }

            if (attributeQueries != null)
            {
                booleanQuery.Add(attributeQueries, Occur.MUST);
            }

            if (!includeAccessories)
            {
                TermQuery manufacturerQuery = new TermQuery(new Term("IsAccessory", "0"));
                booleanQuery.Add(manufacturerQuery, Occur.MUST);
            }

            //为webserivce改的位置？为啥要放到后面？
            if (retailerQueries != null)
            {
                booleanQuery.Add(retailerQueries, Occur.MUST);
            }

            if (booleanQuery.GetClauses().Length == 0)
            {
                return null;
            }

            return booleanQuery;
        }

        private static Query GetRetailerQuery(List<int> retailerIDs, int countryID)
        {
            if (retailerIDs == null || retailerIDs.Count == 0)
            {
                return null;
            }
            BooleanQuery retailerQuery = new BooleanQuery();
            foreach (int rid in retailerIDs)
            {
                TermQuery termQuery = new TermQuery(new Term("RetailerID", rid.ToString()));
                retailerQuery.Add(termQuery, Occur.SHOULD);
            }
            Searcher productRetailerMapIndexSearcher = LuceneController.GetProductRetailerMapIndexSearcher(countryID);
            TopDocs topDocs = productRetailerMapIndexSearcher.Search(retailerQuery, MAXDOCS);

            BooleanQuery queries = null;
            int resultsCount = topDocs.ScoreDocs.Length;
            if (resultsCount > 0)
            {
                queries = new BooleanQuery();
                for (int i = 0; i < resultsCount; i++)
                {
                    Lucene.Net.Documents.Document doc = productRetailerMapIndexSearcher.Doc(topDocs.ScoreDocs[i].Doc, new MapFieldSelector(new string[] { "ProductID" }));
                    string pid = doc.Get("ProductID");
                    TermQuery _termQuery = new TermQuery(new Term("ProductID", pid));
                    queries.Add(_termQuery, Occur.SHOULD);
                }
            }
            return queries;
        }

        private static Query GetQuery(string[] kwList, Occur keywordsOccur, Query otherQuery, bool reSearch)
        {
            BooleanQuery queries = new BooleanQuery();

            if (otherQuery != null)
            {
                queries.Add(otherQuery, Occur.MUST);
            }

            if (kwList != null)
            {
                if (kwList.Length == 1)
                {
                    if (reSearch)
                    {
                        BooleanQuery keywordsQuery = new BooleanQuery();
                        string queryStringFormat = "*{0}*";
                        foreach (string kw in kwList)
                        {
                            Lucene.Net.Index.Term term = new Lucene.Net.Index.Term(SEARCHFIELDNAME, string.Format(queryStringFormat, kw.ToLower()));
                            WildcardQuery wildcardQuery = new WildcardQuery(term);
                            keywordsQuery.Add(wildcardQuery, keywordsOccur);
                        }

                        queries.Add(keywordsQuery, Occur.MUST);
                    }
                    else
                    {
                        BooleanQuery keywordsQuery = new BooleanQuery();
                        string queryStringFormat = "{0}";
                        foreach (string kw in kwList)
                        {
                            Lucene.Net.Index.Term term = new Lucene.Net.Index.Term(SEARCHFIELDNAME, string.Format(queryStringFormat, kw.ToLower()));
                            WildcardQuery wildcardQuery = new WildcardQuery(term);
                            keywordsQuery.Add(wildcardQuery, keywordsOccur);
                        }

                        queries.Add(keywordsQuery, Occur.MUST);
                    }
                }
                else
                {
                    BooleanQuery keywordsQuery = new BooleanQuery();
                    if (reSearch)
                    {
                        string queryStringFormat = "*{0}*";
                        foreach (string kw in kwList)
                        {
                            if (string.IsNullOrEmpty(kw))
                                continue;

                            Lucene.Net.Index.Term term = new Lucene.Net.Index.Term(SEARCHFIELDNAME, string.Format(queryStringFormat, kw.ToLower()));
                            WildcardQuery wildcardQuery = new WildcardQuery(term);
                            keywordsQuery.Add(wildcardQuery, keywordsOccur);
                        }
                    }
                    else
                    {
                        foreach (string kw in kwList)
                        {
                            Lucene.Net.Index.Term term = new Lucene.Net.Index.Term(SEARCHFIELDNAME, kw.ToLower());
                            TermQuery wildcardQuery = new TermQuery(term);
                            keywordsQuery.Add(wildcardQuery, keywordsOccur);
                        }
                    }
                    queries.Add(keywordsQuery, Occur.MUST);
                }
            }

            if (queries.GetClauses().Length > 0)
            {
                return queries;
            }
            else
            {
                return null;
            }
        }

        private static BooleanQuery GetAttributeQuery(List<int> attributeValueIDList, List<int> attributeRangeIDList, Occur occur)
        {
            List<string> selectedProductIdList = new List<string>();
            bool fSearch = true;
            if (attributeValueIDList != null && attributeValueIDList.Count > 0 && LuceneController.AttributesIndexSearcher != null)
            {
                selectedProductIdList = SearchAttributes(attributeValueIDList, occur, fSearch);
                fSearch = false;
            }

            if (attributeRangeIDList != null && attributeRangeIDList.Count > 0 && (fSearch || selectedProductIdList.Count > 0))
            {
                selectedProductIdList = SearchAttributeRange(attributeRangeIDList, occur, fSearch, selectedProductIdList);
            }

            if (selectedProductIdList.Count == 0)
            {
                selectedProductIdList.Add("-1999");
            }
            BooleanQuery productIDQueries = new BooleanQuery();
            foreach (string pid in selectedProductIdList)
            {
                TermQuery termQuery = new TermQuery(new Term("ProductID", pid));
                productIDQueries.Add(termQuery, Occur.SHOULD);
            }

            return productIDQueries;
        }

        private static List<string> SearchAttributeRange(List<int> attributeRangeIDList, Occur occur, bool fSearch, List<string> sPIdList)
        {
            if (occur == Occur.SHOULD)
            {
                foreach (int avr in attributeRangeIDList)
                {
                    BooleanQuery rangeQueries = new BooleanQuery();

                    BooleanQuery attributeValueQueries = new BooleanQuery();
                    List<PriceMeCache.AttributeValueCache> attributeValueCollection = AttributesController.GetAttributeValuesByValueRangeID(avr);
                    foreach (PriceMeCache.AttributeValueCache av in attributeValueCollection)
                    {
                        TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", av.AttributeValueID.ToString()));
                        attributeValueQueries.Add(attributeValueQuery, Occur.SHOULD);
                    }
                    rangeQueries.Add(attributeValueQueries, Occur.SHOULD);

                    if (sPIdList.Count > 0)
                    {
                        BooleanQuery productidQuery = new BooleanQuery();
                        foreach (string pid in sPIdList)
                        {
                            TermQuery pidTermQuery = new TermQuery(new Term("ProductID", pid));
                            productidQuery.Add(pidTermQuery, Occur.SHOULD);
                        }
                        rangeQueries.Add(productidQuery, occur);
                    }

                    sPIdList.Clear();
                    TopDocs topDocs = LuceneController.AttributesIndexSearcher.Search(rangeQueries, MAXDOCS);
                    int resultsCount = topDocs.ScoreDocs.Length;
                    if (resultsCount > 0)
                    {
                        Lucene.Net.Documents.MapFieldSelector mapFieldSelector = new Lucene.Net.Documents.MapFieldSelector(new string[] { "ProductID" });
                        for (int i = 0; i < resultsCount; i++)
                        {
                            Lucene.Net.Documents.Document doc = LuceneController.AttributesIndexSearcher.Doc(topDocs.ScoreDocs[i].Doc, mapFieldSelector);
                            string pid = doc.Get("ProductID");
                            sPIdList.Add(pid);
                        }
                    }

                }
            }
            else
            {
                Dictionary<int, List<int>> attributeRangesDic = new Dictionary<int, List<int>>();
                foreach (int avr in attributeRangeIDList)
                {
                    var attributeRange = AttributesController.GetAttributeTitleByAttributeRangeID(avr);
                    if (attributeRange != null)
                    {
                        if (attributeRangesDic.ContainsKey(attributeRange.TypeID))
                        {
                            attributeRangesDic[attributeRange.TypeID].Add(avr);
                        }
                        else
                        {
                            List<int> list = new List<int>();
                            list.Add(avr);
                            attributeRangesDic.Add(attributeRange.TypeID, list);
                        }
                    }
                }

                foreach (int key in attributeRangesDic.Keys)
                {
                    List<int> avrIDs = attributeRangesDic[key];

                    BooleanQuery query = new BooleanQuery();
                    BooleanQuery rangeQueries = new BooleanQuery();
                    foreach (int avr in avrIDs)
                    {
                        BooleanQuery attributeValueQueries = new BooleanQuery();
                        List<PriceMeCache.AttributeValueCache> attributeValueCollection = AttributesController.GetAttributeValuesByValueRangeID(avr);
                        foreach (PriceMeCache.AttributeValueCache av in attributeValueCollection)
                        {
                            TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", av.AttributeValueID.ToString()));
                            attributeValueQueries.Add(attributeValueQuery, Occur.SHOULD);
                        }
                        rangeQueries.Add(attributeValueQueries, Occur.SHOULD);
                    }
                    query.Add(rangeQueries, Occur.MUST);

                    if (sPIdList.Count > 0)
                    {
                        BooleanQuery productidQuery = new BooleanQuery();
                        foreach (string pid in sPIdList)
                        {
                            TermQuery pidTermQuery = new TermQuery(new Term("ProductID", pid));
                            productidQuery.Add(pidTermQuery, Occur.SHOULD);
                        }
                        query.Add(productidQuery, Occur.MUST);
                        sPIdList.Clear();
                    }
                    else if (!fSearch)
                    {
                        return sPIdList;
                    }

                    fSearch = false;

                    TopDocs topDocs = LuceneController.AttributesIndexSearcher.Search(query, MAXDOCS);
                    int resultsCount = topDocs.ScoreDocs.Length;
                    if (resultsCount > 0)
                    {
                        Lucene.Net.Documents.MapFieldSelector mapFieldSelector = new Lucene.Net.Documents.MapFieldSelector(new string[] { "ProductID" });
                        for (int i = 0; i < resultsCount; i++)
                        {
                            Lucene.Net.Documents.Document doc = LuceneController.AttributesIndexSearcher.Doc(topDocs.ScoreDocs[i].Doc, mapFieldSelector);
                            string pid = doc.Get("ProductID");
                            sPIdList.Add(pid);
                        }
                    }
                }
            }

            return sPIdList;
        }

        private static List<string> SearchAttributes(List<int> attributeValueIDList, Occur occur, bool fSearch)
        {
            List<string> pIdList = new List<string>();

            if (occur == Occur.SHOULD)
            {
                foreach (int avID in attributeValueIDList)
                {
                    BooleanQuery queries = new BooleanQuery();
                    TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", avID.ToString()));
                    queries.Add(attributeValueQuery, occur);

                    if (pIdList.Count > 0)
                    {
                        BooleanQuery productidQuery = new BooleanQuery();
                        foreach (string pid in pIdList)
                        {
                            TermQuery pidTermQuery = new TermQuery(new Term("ProductID", pid));
                            productidQuery.Add(pidTermQuery, Occur.SHOULD);
                        }
                        queries.Add(productidQuery, occur);
                        pIdList.Clear();
                    }

                    TopDocs topDocs = LuceneController.AttributesIndexSearcher.Search(queries, MAXDOCS);
                    int resultsCount = topDocs.ScoreDocs.Length;
                    if (resultsCount > 0)
                    {
                        Lucene.Net.Documents.MapFieldSelector mapFieldSelector = new Lucene.Net.Documents.MapFieldSelector(new string[] { "ProductID" });
                        for (int i = 0; i < resultsCount; i++)
                        {
                            Lucene.Net.Documents.Document doc = LuceneController.AttributesIndexSearcher.Doc(topDocs.ScoreDocs[i].Doc, mapFieldSelector);
                            string pid = doc.Get("ProductID");
                            pIdList.Add(pid);
                        }
                    }
                }
            }
            else
            {
                Dictionary<int, List<int>> attributesDic = new Dictionary<int, List<int>>();
                foreach (int avID in attributeValueIDList)
                {
                    var attributeTitle = AttributesController.GetAttributeTitleByVauleID(avID);
                    if (attributeTitle != null)
                    {
                        if (attributesDic.ContainsKey(attributeTitle.TypeID))
                        {
                            attributesDic[attributeTitle.TypeID].Add(avID);
                        }
                        else
                        {
                            List<int> list = new List<int>();
                            list.Add(avID);
                            attributesDic.Add(attributeTitle.TypeID, list);
                        }
                    }
                }

                foreach (int key in attributesDic.Keys)
                {
                    List<int> attValueIDs = attributesDic[key];
                    BooleanQuery queries = new BooleanQuery();
                    BooleanQuery attrQueries = new BooleanQuery();
                    foreach (int attValueID in attValueIDs)
                    {
                        TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", attValueID.ToString()));
                        attrQueries.Add(attributeValueQuery, Occur.SHOULD);
                    }
                    queries.Add(attrQueries, Occur.MUST);

                    if (pIdList.Count > 0)
                    {
                        BooleanQuery productidQuery = new BooleanQuery();
                        foreach (string pid in pIdList)
                        {
                            TermQuery pidTermQuery = new TermQuery(new Term("ProductID", pid));
                            productidQuery.Add(pidTermQuery, Occur.SHOULD);
                        }
                        queries.Add(productidQuery, Occur.MUST);
                        pIdList.Clear();
                    }
                    else if (!fSearch)
                    {
                        return pIdList;
                    }

                    fSearch = false;

                    TopDocs topDocs = LuceneController.AttributesIndexSearcher.Search(queries, MAXDOCS);
                    int resultsCount = topDocs.ScoreDocs.Length;
                    if (resultsCount > 0)
                    {
                        Lucene.Net.Documents.MapFieldSelector mapFieldSelector = new Lucene.Net.Documents.MapFieldSelector(new string[] { "ProductID" });
                        for (int i = 0; i < resultsCount; i++)
                        {
                            Lucene.Net.Documents.Document doc = LuceneController.AttributesIndexSearcher.Doc(topDocs.ScoreDocs[i].Doc, mapFieldSelector);
                            string pid = doc.Get("ProductID");
                            pIdList.Add(pid);
                        }
                    }
                }
            }

            return pIdList;
        }

        private static BooleanQuery GetAttributeValueRangeQuery(Dictionary<int, string> attributeValueRange)
        {
            List<string> selectedProductIdList = new List<string>();

            if (attributeValueRange != null && attributeValueRange.Count > 0 && LuceneController.AttributesIndexSearcher != null)
            {
                foreach (var range in attributeValueRange)
                {
                    BooleanQuery queries = new BooleanQuery();
                    TermQuery attributeTitleQuery = new TermQuery(new Term("TypeID", range.Key.ToString()));
                    queries.Add(attributeTitleQuery, Occur.MUST);

                    var range_ = range.Value.Split('-');
                    NumericRangeQuery<float> attRange = NumericRangeQuery.NewFloatRange("AttributeValue",
                        float.Parse(range_[0]), float.Parse(range_[1]), true, true);
                    queries.Add(attRange, Occur.MUST);

                    if (selectedProductIdList.Count > 0)
                    {
                        BooleanQuery productidQuery = new BooleanQuery();
                        foreach (string pid in selectedProductIdList)
                        {
                            TermQuery pidTermQuery = new TermQuery(new Term("ProductID", pid));
                            productidQuery.Add(pidTermQuery, Occur.SHOULD);
                        }
                        queries.Add(productidQuery, Occur.MUST);
                        selectedProductIdList.Clear();
                    }

                    TopDocs topDocs = LuceneController.AttributesIndexSearcher.Search(queries, MAXDOCS);
                    int resultsCount = topDocs.ScoreDocs.Length;
                    if (resultsCount > 0)
                    {
                        Lucene.Net.Documents.MapFieldSelector mapFieldSelector = new Lucene.Net.Documents.MapFieldSelector(new string[] { "ProductID" });
                        for (int i = 0; i < resultsCount; i++)
                        {
                            Lucene.Net.Documents.Document doc = LuceneController.AttributesIndexSearcher.Doc(topDocs.ScoreDocs[i].Doc, mapFieldSelector);
                            string pid = doc.Get("ProductID");
                            selectedProductIdList.Add(pid);
                        }
                    }
                }
            }

            if (selectedProductIdList.Count == 0)
            {
                selectedProductIdList.Add("-1999");
            }
            BooleanQuery productIDQueries = new BooleanQuery();
            foreach (string pid in selectedProductIdList)
            {
                TermQuery termQuery = new TermQuery(new Term("ProductID", pid));
                productIDQueries.Add(termQuery, Occur.SHOULD);
            }

            return productIDQueries;
        }

        public static Searcher GetSearcherByCategoryID(int categoryID)
        {
            return GetSearcherByCategoryID(categoryID, ConfigAppString.CountryID);
        }

        public static Searcher GetSearcherByCategoryID(int categoryID, int countryID)
        {
            int rootCategoryID = 0;
            if (categoryID != 0)
            {
                PriceMeCache.CategoryCache rootCategory = CategoryController.GetRootCategory(categoryID);
                if (rootCategory == null)
                {
                    return null;
                }
                rootCategoryID = rootCategory.CategoryID;
            }

            LuceneInfo luceneInfo = LuceneController.GetLuceneInfo(countryID);
            if (luceneInfo != null)
            {
                if (luceneInfo.CategoriesProductLuceneIndex.ContainsKey(rootCategoryID))
                {
                    return luceneInfo.CategoriesProductLuceneIndex[rootCategoryID];
                }
            }

            return null;
        }

        public static ProductCatalog GetProductCatalogFromDoc(Lucene.Net.Documents.Document hitDoc)
        {
            ProductCatalog productCatalog = new ProductCatalog();
            productCatalog.ProductID = hitDoc.Get("ProductID");
            if (string.IsNullOrEmpty(productCatalog.ProductID))
                return null;
            productCatalog.DefaultImage = hitDoc.Get("DefaultImage");
            productCatalog.ShortDescriptionZH = "";// hitDoc.Get("ShortDescriptionZH");
            productCatalog.MaxPrice = hitDoc.Get("MaxPrice");

            int indexOfDot = productCatalog.MaxPrice.IndexOf('.');
            if (indexOfDot > 0)
            {
                int length = productCatalog.MaxPrice.Length;
                if (length - indexOfDot > 3)
                {
                    productCatalog.MaxPrice = productCatalog.MaxPrice.Substring(0, indexOfDot + 3);
                }
            }

            productCatalog.BestPrice = hitDoc.Get("BestPrice");
            indexOfDot = productCatalog.BestPrice.IndexOf('.');
            if (indexOfDot > 0)
            {
                int length = productCatalog.BestPrice.Length;
                if (length - indexOfDot > 3)
                {
                    productCatalog.BestPrice = productCatalog.BestPrice.Substring(0, indexOfDot + 3);
                }
            }

            productCatalog.BestPPCPrice = productCatalog.BestPrice;

            productCatalog.ProductRatingSum = string.IsNullOrEmpty(productCatalog.ProductRatingSum) ? "3" : productCatalog.ProductRatingSum;
            productCatalog.ProductRatingVotes = string.IsNullOrEmpty(productCatalog.ProductRatingVotes) ? "1" : productCatalog.ProductRatingVotes;
            productCatalog.Rating = hitDoc.Get("AvRating");
            productCatalog.PriceCount = hitDoc.Get("PriceCount");
            productCatalog.ProductName = hitDoc.Get("ProductName");

            int cid;
            int.TryParse(hitDoc.Get("CategoryID"), out cid);
            productCatalog.CategoryID = cid;
            productCatalog.AvRating = hitDoc.Get("AvRating");
            productCatalog.BestPPCRetailerName = hitDoc.Get("BestPPCRetailerName");
            productCatalog.ReviewCount = hitDoc.Get("ProductReviewCount");

            int ck;
            int.TryParse(hitDoc.Get("Clicks"), out ck);
            productCatalog.Click = ck;
            productCatalog.BestPPCLogoPath = hitDoc.Get("BestPPCLogoPath");
            productCatalog.BestPPCRetailerProductID = hitDoc.Get("BestPPCRetailerProductID");
            productCatalog.BestPPCRetailerID = hitDoc.Get("BestPPCRetailerID");
            productCatalog.CatalogDescription = hitDoc.Get("CatalogDescription");
            productCatalog.RetailerCount = hitDoc.Get("RetailerCount");
            productCatalog.ManufacturerID = hitDoc.Get("ManufacturerID");
            productCatalog.DisplayName = productCatalog.ProductName;
            productCatalog.IsAccessory = hitDoc.Get("IsAccessory");
            

            int dc;
            int.TryParse(hitDoc.Get("DayCount"), out dc);
            productCatalog.DayCount = dc;

            int upcoming = 0;
            int.TryParse(hitDoc.Get("IsUpComing"), out upcoming);
            bool isUpComing = upcoming == 1 ? true : false;
            productCatalog.IsUpComing = isUpComing;

            productCatalog.AttrDescription = hitDoc.Get("AttrDescription");

            double prevPrice;
            double.TryParse(hitDoc.Get("PrevPrice"), out prevPrice);
            productCatalog.PrevPrice = prevPrice;

            double currentPrice;
            double.TryParse(hitDoc.Get("BestPrice"), out currentPrice);
            productCatalog.CurrentPrice = currentPrice;

            float sale;
            float.TryParse(hitDoc.Get("Sale"), out sale);
            productCatalog.Sale = sale;

            int bprId = 0;
            int.TryParse(hitDoc.Get("BestPriceRetailerId"), out bprId);

            productCatalog.BestPriceRetailerId = bprId;

            return productCatalog;
        }
        #endregion

        internal static ProductCatalog GetProductCatalog(HitsInfo hitsInfo, int index)
        {
            Document doc = hitsInfo.GetDocument(index);
            ProductCatalog productCatalog = GetProductCatalogFromDoc(doc);
            return productCatalog;
        }

        /// <summary>
        /// 请注意:
        /// 这个方法要用到key:RetailerProductInfoPath 已经被删除
        /// 如要使用就重新添加
        /// e.g: <add key="RetailerProductInfoPath" value="E:\RPI\" /> 
        /// </summary>
        /// <param name="retailerID"></param>
        /// <returns></returns>
        public static IndexSearcher GetRetailerProductInfoSearcher(int retailerID)
        {
            string path = ConfigAppString.RetailerProductInfoPath + retailerID;
            if (System.IO.Directory.Exists(path))
            {
                IndexSearcher indexSearcher = LuceneController.GetIndexSearcher(path);
                return indexSearcher;
            }
            else
            {
                return null;
            }
        }

        public static RetailerProductCatalog GetRetailerProductCatalog(HitsInfo resultHitsInfo, int index)
        {
            Document doc = resultHitsInfo.GetDocument(index);
            RetailerProductCatalog retailerProductCatalog = GetRetailerProductCatalogFromDoc(doc);
            return retailerProductCatalog;
        }

        private static RetailerProductCatalog GetRetailerProductCatalogFromDoc(Document doc)
        {
            RetailerProductCatalog retailerProductCatalog = new RetailerProductCatalog();

            retailerProductCatalog.CatgoryId = doc.Get("CategoryID");
            retailerProductCatalog.ProductId = doc.Get("ProductID");
            retailerProductCatalog.RetailerId = doc.Get("RetailerId");
            retailerProductCatalog.RetailerPrice = doc.Get("RetailerPrice");
            retailerProductCatalog.RetailerProductDefaultImage = doc.Get("RetailerProductDefaultImage");
            retailerProductCatalog.RetailerProductDescription = doc.Get("RetailerProductDescription");
            retailerProductCatalog.RetailerProductId = doc.Get("RetailerProductId");
            retailerProductCatalog.RetailerProductName = doc.Get("RetailerProductName");
            retailerProductCatalog.ProductAvRating = doc.Get("AvRating");
            retailerProductCatalog.ProductReviewCount = doc.Get("ReviewCount");
            retailerProductCatalog.RetailerAvRating = doc.Get("RetailerAvRating");
            retailerProductCatalog.RetailerProductCondition = doc.Get("RetailerProductCondition");
            retailerProductCatalog.ProductName = doc.Get("ProductName");

            return retailerProductCatalog;
        }

        public static List<ProductCatalog> GetBiggestPriceDropByProductIDs(string pids)
        {
            List<ProductCatalog> pcc = new List<ProductCatalog>();

            BooleanQuery pidQuery = new BooleanQuery();
            string[] ids = pids.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length > 0)
            {
                Searcher searcher = GetSearcherByCategoryID(0, 3);
                Sort sort = new Sort();

                foreach (string id in ids)
                {
                    TermQuery idQuery = new TermQuery(new Term("ProductID", id));
                    pidQuery.Add(idQuery, Occur.SHOULD);
                }

                TopDocs topDocs = searcher.Search(pidQuery, null, ids.Length);

                for (int i = 0; i < topDocs.ScoreDocs.Length; i++)
                {
                    ProductCatalog pc = GetProductCatalogFromDoc(searcher.Doc(topDocs.ScoreDocs[i].Doc));
                    pcc.Add(pc);
                }
            }
            return pcc;
        }
    }
}