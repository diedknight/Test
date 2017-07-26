using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class SearchController
    {
        public readonly static int MaxSearchCount_Static = 10000;

        const int MAXDOCS = 100000;
        const int MAXClAUSECOUNT = 100000;
        const string PRICERANGEFIELDNAME = "BestPrice";
        const string SEARCHFIELDNAME = "SearchField2";

        static Dictionary<int, List<string>> InvalidKeywordsListDic_Static;
        static Dictionary<int, List<string>> InvalidStatementListDic_Static;

        static Dictionary<int, string> RootCategoryNameDic_Static;
        static Dictionary<int, int> CategoryRootMapDic_Static;
        static List<int> IsSearchOnlyCateogryIdList_Static;

        static SearchController()
        {
            BooleanQuery.MaxClauseCount = MAXClAUSECOUNT;
            SetInvalidKeywordsList();
            SetCategoryInfo();
        }

        private static void SetCategoryInfo()
        {
            RootCategoryNameDic_Static = new Dictionary<int, string>();
            CategoryRootMapDic_Static = new Dictionary<int, int>();
            IsSearchOnlyCateogryIdList_Static = new List<int>();

            string connectionString = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                string sql = @"GetCategoryRootInfo";
                using (SqlCommand sqlCMD = new SqlCommand())
                {
                    sqlCMD.CommandText = sql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCMD.Connection = sqlConn;

                    sqlConn.Open();

                    using (SqlDataReader sdr = sqlCMD.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            int rootId = sdr.GetInt32(0);
                            string rootCategoryName = sdr.GetString(1);
                            int cId = sdr.GetInt32(2);
                            bool isSearchOnly = sdr.GetBoolean(4);
                            CategoryRootMapDic_Static.Add(cId, rootId);

                            if(!RootCategoryNameDic_Static.ContainsKey(rootId))
                            {
                                RootCategoryNameDic_Static.Add(rootId, rootCategoryName);
                            }

                            if (isSearchOnly)
                                IsSearchOnlyCateogryIdList_Static.Add(cId);
                        }
                    }
                }
            }
        }

        private static void SetInvalidKeywordsList()
        {
            InvalidKeywordsListDic_Static = new Dictionary<int, List<string>>();
            InvalidStatementListDic_Static = new Dictionary<int, List<string>>();

            foreach(int countryId in MultiCountryController.CountryIdList)
            { 
                List<string> invalidKeywordsList = new List<string>();
                List<string> invalidStatementList = new List<string>();
                string connectionString = MultiCountryController.GetDBConnectionString(countryId);
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    string sql = @"select InvalidKeyword from InvalidKeywords";
                    using (SqlCommand sqlCMD = new SqlCommand())
                    {
                        sqlCMD.CommandText = sql;
                        sqlCMD.CommandTimeout = 0;
                        sqlCMD.CommandType = System.Data.CommandType.Text;
                        sqlCMD.Connection = sqlConn;

                        sqlConn.Open();

                        using (SqlDataReader sdr = sqlCMD.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                string invaildString = sdr["InvalidKeyword"].ToString();
                                if (invaildString.Contains(" "))
                                {
                                    invalidStatementList.Add(invaildString);
                                }
                                else
                                {
                                    invalidKeywordsList.Add(invaildString);
                                }
                            }
                        }
                    }
                }

                InvalidKeywordsListDic_Static.Add(countryId, invalidKeywordsList);
                InvalidStatementListDic_Static.Add(countryId, invalidStatementList);
            }
        }

        /// <summary>
        /// PriceMe WebSite(All)
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="categoryIDList"></param>
        /// <param name="manufacturerIDs"></param>
        /// <param name="priceRange"></param>
        /// <param name="attributeValueIDList"></param>
        /// <param name="attributeRangeIDList"></param>
        /// <param name="sortby"></param>
        /// <param name="retailerID"></param>
        /// <param name="maxDocCount"></param>
        /// <param name="countryID"></param>
        /// <param name="multiAttribute"></param>
        /// <param name="includeAccessories"></param>
        /// <param name="ppcOnly"></param>
        /// <param name="attrValueRange"></param>
        /// <returns></returns>
        public static HitsInfo SearchProducts(string keywords, 
            List<int> categoryIDList, 
            List<int> manufacturerIDs, 
            PriceRange priceRange, 
            List<int> attributeValueIDList, 
            List<int> attributeRangeIDList, 
            string sortby, 
            List<int> retailerIDs,
            int maxDocCount, 
            int countryId, 
            bool multiAttribute, 
            bool includeAccessories, 
            bool ppcOnly,
            Dictionary<int, string> attrValueRange, 
            bool useSearchonly, 
            DaysRange daysRange, 
            string onSaleOnly, bool sale = false, float saleMinimumPrice = -0.01f)
        {
            /// <param name="searchMerged">如果categoryMerged为false，searchMerged也为false，则只搜索IsMerged为false的产品</param>
            /// <param name="categoryMerged">如果categoryMerged为true，则只搜索IsDisplay为false的产品</param>
            /// 
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
            IndexSearcher searcher = null;
            if (categoryIDList == null || categoryIDList.Count == 0 || categoryIDList.Count > 1)
            {
                searcher = GetSearcherByCategoryID(0, countryId);
            }
            else if (categoryIDList != null && categoryIDList.Count == 1)
            {
                searcher = GetSearcherByCategoryID(categoryIDList[0], countryId);
            }
            if (searcher == null)
            {
                return null;
            }

            string[] kwList = GetKeyWordsList(keywords, countryId);
            Query query = GetQuery(categoryIDList, manufacturerIDs, attributeValueIDList, attributeRangeIDList, retailerIDs, countryId, occur, includeAccessories, ppcOnly, attrValueRange, useSearchonly, daysRange, onSaleOnly, sale, saleMinimumPrice);
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
                            newQuery = GetRetailerProductQuery(GetKeyWordsList(keywords, countryId), query, countryId);
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
                        newQuery = GetRetailerProductQuery(GetKeyWordsList(keywords, countryId), query, countryId);
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

        private static IndexSearcher GetSearcherByCategoryID(int categoryID, int countryID)
        {
            if (categoryID != 0)
            {
                if(CategoryRootMapDic_Static.ContainsKey(categoryID))
                {
                    int rootId = CategoryRootMapDic_Static[categoryID];

                    if(RootCategoryNameDic_Static.ContainsKey(rootId))
                    {
                        string rootCategoryName = RootCategoryNameDic_Static[rootId];
                        return MultiCountryController.GetRootCategoryProductsLuceneSearcher(rootCategoryName, countryID);
                    }
                    
                }
            }

            return MultiCountryController.GetAllCategoryProductsIndexSearcher(countryID);
        }

        private static string[] GetKeyWordsList(string keyWords, int countryId)
        {
            if (!string.IsNullOrEmpty(keyWords))
            {
                keyWords = FixKeyWords(keyWords, countryId);

                keyWords = keyWords.Length > 80 ? keyWords.Substring(0, 70).ToLower() : keyWords.ToLower();
                string[] kws = keyWords.Split(new char[] { ' ', '-', '~', '&', '!', '#', '@', '^', '?', '*', '\"', '[', ']', '{', '}', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> kwsList = new List<string>(kws);

                kwsList = FilterKws(kwsList, countryId);
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

        private static string FixKeyWords(string keyWords, int countryId)
        {
            if (InvalidStatementListDic_Static.ContainsKey(countryId))
            {
                List<string> invalidStatementList = InvalidStatementListDic_Static[countryId];
                foreach (string str in invalidStatementList)
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
            }
            return keyWords;
        }

        private static List<string> FilterKws(List<string> kwsList, int countryId)
        {
            if (InvalidKeywordsListDic_Static.ContainsKey(countryId))
            {
                List<string> invalidKeywordsList = InvalidKeywordsListDic_Static[countryId];
                for (int i = 0; i < kwsList.Count;)
                {
                    if (invalidKeywordsList.Contains(kwsList[i]))
                    {
                        kwsList.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return kwsList;
        }

        private static Query GetQuery(
            List<int> cidList,
            List<int> manufacturerIDs,
            List<int> attributeValueIDList,
            List<int> attributeRangeIDList,
            List<int> retailerIDs,
            int countryId,
            Occur occur,
            bool includeAccessories,
            bool ppcOnly,
            Dictionary<int, string> attrValueRange,
            bool useSearchonly,
            DaysRange daysRange,
            string onSaleOnly, bool sale, float saleMinimumPrice)
        {
            Query categoryQueries = GetCategoryQuery(cidList, useSearchonly, countryId);
            Query attributeQueries = null;
            if ((attributeRangeIDList != null && attributeRangeIDList.Count > 0) || (attributeValueIDList != null && attributeValueIDList.Count > 0))
            {
                attributeQueries = GetAttributeQuery(attributeValueIDList, attributeRangeIDList, occur, countryId);
            }
            Query retailerQueries = GetRetailerQuery(retailerIDs, countryId);

            Query attrValueRangeQueries = null;
            if (attrValueRange != null && attrValueRange.Count > 0)
            {
                attrValueRangeQueries = GetAttributeValueRangeQuery(attrValueRange, countryId);
            }
            BooleanQuery booleanQuery = new BooleanQuery();
            if (attrValueRangeQueries != null)
            {
                booleanQuery.Add(attrValueRangeQueries, Occur.MUST);
            }

            if (daysRange != null && daysRange.MaxDays > 0)
            {
                //NumericRangeQuery<int> daysQuery = NumericRangeQuery.NewInt32Range("DayCount", daysRange.MinDays, daysRange.MaxDays, true, true);
                NumericRangeQuery<int> daysQuery = NumericRangeQuery.NewIntRange("DayCount", daysRange.MinDays, daysRange.MaxDays, true, true);
                booleanQuery.Add(daysQuery, Occur.MUST);
            }

            if (!string.IsNullOrEmpty(onSaleOnly))
            {
                TermQuery onSaleOnlyQuery = new TermQuery(new Term("DisplaySaleTag", onSaleOnly));
                booleanQuery.Add(onSaleOnlyQuery, Occur.MUST);
            }

            if (manufacturerIDs != null && manufacturerIDs.Count > 0)
            {
                BooleanQuery brandBooleanQuery = new BooleanQuery();
                foreach (int brandID in manufacturerIDs)
                {
                    //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                    //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(brandID, 0, btRef);
                    //TermQuery manufacturerQuery = new TermQuery(new Term("ManufacturerID", btRef));
                    TermQuery manufacturerQuery = new TermQuery(new Term("ManufacturerID", brandID.ToString()));
                    brandBooleanQuery.Add(manufacturerQuery, Occur.SHOULD);
                }
                booleanQuery.Add(brandBooleanQuery, Occur.MUST);
            }

            if (sale)
            {
                booleanQuery.Add(NumericRangeQuery.NewFloatRange("Sale", -0.85f, saleMinimumPrice, true, true), Occur.MUST);
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

        public static BooleanQuery GetCategoryQuery(List<int> categoryIDList, bool useSearchOnly, int countryID)
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
                    string[] cids = GetSubCategoryIDs(categoryID, countryID);
                    foreach (string cid in cids)
                    {
                        int categoryid = 0;
                        int.TryParse(cid, out categoryid);

                        if (useSearchOnly)
                        {
                            //去掉isSearchOnly的分类
                            if (IsSearchOnlyCateogryIdList_Static.Contains(categoryid))
                                continue;
                        }
                        //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                        //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(categoryid, 0, btRef);
                        //TermQuery termQuery = new TermQuery(new Term("CategoryID", btRef));
                        TermQuery termQuery = new TermQuery(new Term("CategoryID", categoryid.ToString()));
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
                            Term term = new Term(SEARCHFIELDNAME, string.Format(queryStringFormat, kw.ToLower()));
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
                            Term term = new Term(SEARCHFIELDNAME, string.Format(queryStringFormat, kw.ToLower()));
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

                            Term term = new Term(SEARCHFIELDNAME, string.Format(queryStringFormat, kw.ToLower()));
                            WildcardQuery wildcardQuery = new WildcardQuery(term);
                            keywordsQuery.Add(wildcardQuery, keywordsOccur);
                        }
                    }
                    else
                    {
                        foreach (string kw in kwList)
                        {
                            Term term = new Term(SEARCHFIELDNAME, kw.ToLower());
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

        public static string[] GetSubCategoryIDs(int categoryID, int countryID)
        {
            string allSubCategoriesString = "";
            if (categoryID != 0)
            {
                allSubCategoriesString = GetAllSubCategoriesString(categoryID, countryID);
            }

            if (allSubCategoriesString.Length > 0)
            {
                return allSubCategoriesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            return new string[0];
        }

        private static string GetAllSubCategoriesString(int cid, int countryId)
        {
            //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(cid, 0, btRef);
            //TermQuery categoryQuery = new TermQuery(new Term("CategoryID", btRef));
            TermQuery categoryQuery = new TermQuery(new Term("CategoryID", cid.ToString()));

            TopDocs topDocs = MultiCountryController.GetCategoriesLuceneSearcher(countryId).Search(categoryQuery, null, 1);
            if (topDocs != null && topDocs.ScoreDocs.Length > 0)
            {
                //ISet<string> s = new HashSet<string>();
                //s.Add("SubCategoriesString");
                //Document doc = MultiCountryController.GetCategoriesLuceneSearcher(countryId).Doc(topDocs.ScoreDocs[0].Doc, s);
                Document doc = MultiCountryController.GetCategoriesLuceneSearcher(countryId).Doc(topDocs.ScoreDocs[0].Doc, new MapFieldSelector("SubCategoriesString"));

                return doc.Get("SubCategoriesString");
            }
            return cid.ToString();
        }

        private static BooleanQuery GetAttributeQuery(List<int> attributeValueIDList, List<int> attributeRangeIDList, Occur occur, int countryId)
        {
            List<string> selectedProductIdList = new List<string>();
            bool fSearch = true;
            if (attributeValueIDList != null && attributeValueIDList.Count > 0)
            {
                selectedProductIdList = SearchAttributes(attributeValueIDList, occur, fSearch, countryId);
                fSearch = false;
            }

            if (attributeRangeIDList != null && attributeRangeIDList.Count > 0 && (fSearch || selectedProductIdList.Count > 0))
            {
                selectedProductIdList = SearchAttributeRange(attributeRangeIDList, occur, fSearch, selectedProductIdList, countryId);
            }

            if (selectedProductIdList.Count == 0)
            {
                selectedProductIdList.Add("-1999");
            }
            BooleanQuery productIDQueries = new BooleanQuery();
            foreach (string pid in selectedProductIdList)
            {
                //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef);
                //TermQuery termQuery = new TermQuery(new Term("ProductID", btRef));
                TermQuery termQuery = new TermQuery(new Term("ProductID", pid));
                productIDQueries.Add(termQuery, Occur.SHOULD);
            }

            return productIDQueries;
        }

        private static List<string> SearchAttributeRange(List<int> attributeRangeIDList, Occur occur, bool fSearch, List<string> sPIdList, int countryId)
        {
            IndexSearcher attributeSearcher = MultiCountryController.GetAttributesLuceneSearcher(countryId);

            if (occur == Occur.SHOULD)
            {
                foreach (int avr in attributeRangeIDList)
                {
                    BooleanQuery rangeQueries = new BooleanQuery();

                    BooleanQuery attributeValueQueries = new BooleanQuery();
                    List<PriceMeCache.AttributeValueCache> attributeValueCollection = AttributesController.GetAttributeValuesByValueRangeID(avr);
                    foreach (PriceMeCache.AttributeValueCache av in attributeValueCollection)
                    {
                        //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                        //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(av.AttributeValueID, 0, btRef);
                        //TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", btRef));
                        TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", av.AttributeValueID.ToString()));
                        attributeValueQueries.Add(attributeValueQuery, Occur.SHOULD);
                    }
                    rangeQueries.Add(attributeValueQueries, Occur.SHOULD);

                    if (sPIdList.Count > 0)
                    {
                        BooleanQuery productidQuery = new BooleanQuery();
                        foreach (string pid in sPIdList)
                        {
                            //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef);
                            //TermQuery pidTermQuery = new TermQuery(new Term("ProductID", btRef));
                            TermQuery pidTermQuery = new TermQuery(new Term("ProductID", pid));
                            productidQuery.Add(pidTermQuery, Occur.SHOULD);
                        }
                        rangeQueries.Add(productidQuery, occur);
                    }

                    sPIdList.Clear();
                    TopDocs topDocs = attributeSearcher.Search(rangeQueries, MAXDOCS);
                    int resultsCount = topDocs.ScoreDocs.Length;
                    if (resultsCount > 0)
                    {
                        //ISet<string> s = new HashSet<string>();
                        //s.Add("ProductID");
                        for (int i = 0; i < resultsCount; i++)
                        {
                            //Document doc = attributeSearcher.Doc(topDocs.ScoreDocs[i].Doc, s);
                            Document doc = attributeSearcher.Doc(topDocs.ScoreDocs[i].Doc, new MapFieldSelector("ProductID"));
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
                            //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(av.AttributeValueID, 0, btRef);
                            //TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", btRef));
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
                            //Lucene.Net.Util.BytesRef btRef2 = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef2);
                            //TermQuery pidTermQuery = new TermQuery(new Term("ProductID", btRef2));
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

                    TopDocs topDocs = attributeSearcher.Search(query, MAXDOCS);
                    int resultsCount = topDocs.ScoreDocs.Length;
                    if (resultsCount > 0)
                    {
                        //ISet<string> s = new HashSet<string>();
                        //s.Add("ProductID");
                        for (int i = 0; i < resultsCount; i++)
                        {
                            //Document doc = attributeSearcher.Doc(topDocs.ScoreDocs[i].Doc, s);
                            Document doc = attributeSearcher.Doc(topDocs.ScoreDocs[i].Doc, new MapFieldSelector("ProductID"));
                            string pid = doc.Get("ProductID");
                            sPIdList.Add(pid);
                        }
                    }
                }
            }

            return sPIdList;
        }

        private static List<string> SearchAttributes(List<int> attributeValueIDList, Occur occur, bool fSearch, int countryId)
        {
            List<string> pIdList = new List<string>();
            IndexSearcher attributeSearcher = MultiCountryController.GetAttributesLuceneSearcher(countryId);
            if (occur == Occur.SHOULD)
            {
                foreach (int avID in attributeValueIDList)
                {
                    BooleanQuery queries = new BooleanQuery();
                    //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                    //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(avID, 0, btRef);
                    //TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", btRef));
                    TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", avID.ToString()));
                    queries.Add(attributeValueQuery, occur);

                    if (pIdList.Count > 0)
                    {
                        BooleanQuery productidQuery = new BooleanQuery();
                        foreach (string pid in pIdList)
                        {
                            //Lucene.Net.Util.BytesRef btRef2 = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef2);
                            //TermQuery pidTermQuery = new TermQuery(new Term("ProductID", btRef2));
                            TermQuery pidTermQuery = new TermQuery(new Term("ProductID", pid));
                            productidQuery.Add(pidTermQuery, Occur.SHOULD);
                        }
                        queries.Add(productidQuery, occur);
                        pIdList.Clear();
                    }

                    TopDocs topDocs = attributeSearcher.Search(queries, MAXDOCS);
                    int resultsCount = topDocs.ScoreDocs.Length;
                    if (resultsCount > 0)
                    {
                        //ISet<string> s = new HashSet<string>();
                        //s.Add("ProductID");
                        for (int i = 0; i < resultsCount; i++)
                        {
                            //Document doc = attributeSearcher.Doc(topDocs.ScoreDocs[i].Doc, s);
                            Document doc = attributeSearcher.Doc(topDocs.ScoreDocs[i].Doc, new MapFieldSelector("ProductID"));
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
                        //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                        //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(attValueID, 0, btRef);
                        //TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", btRef));
                        TermQuery attributeValueQuery = new TermQuery(new Term("AttributeValueID", attValueID.ToString()));
                        attrQueries.Add(attributeValueQuery, Occur.SHOULD);
                    }
                    queries.Add(attrQueries, Occur.MUST);

                    if (pIdList.Count > 0)
                    {
                        BooleanQuery productidQuery = new BooleanQuery();
                        foreach (string pid in pIdList)
                        {
                            //Lucene.Net.Util.BytesRef btRef2 = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef2);
                            //TermQuery pidTermQuery = new TermQuery(new Term("ProductID", btRef2));
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

                    TopDocs topDocs = attributeSearcher.Search(queries, MAXDOCS);
                    int resultsCount = topDocs.ScoreDocs.Length;
                    if (resultsCount > 0)
                    {
                        //ISet<string> s = new HashSet<string>();
                        //s.Add("ProductID");
                        for (int i = 0; i < resultsCount; i++)
                        {
                            //Document doc = attributeSearcher.Doc(topDocs.ScoreDocs[i].Doc, s);
                            Document doc = attributeSearcher.Doc(topDocs.ScoreDocs[i].Doc, new MapFieldSelector("ProductID"));
                            string pid = doc.Get("ProductID");
                            pIdList.Add(pid);
                        }
                    }
                }
            }

            return pIdList;
        }

        /// <summary>
        /// 搜索Retailer对应的Product
        /// </summary>
        /// <param name="retailerIDs"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        private static Query GetRetailerQuery(List<int> retailerIDs, int countryId)
        {
            if (retailerIDs == null || retailerIDs.Count == 0)
            {
                return null;
            }
            BooleanQuery retailerQuery = new BooleanQuery();
            foreach (int rid in retailerIDs)
            {
                //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(rid, 0, btRef);
                //TermQuery termQuery = new TermQuery(new Term("RetailerID", btRef));
                TermQuery termQuery = new TermQuery(new Term("RetailerID", rid.ToString()));
                retailerQuery.Add(termQuery, Occur.SHOULD);
            }
            IndexSearcher productRetailerMapIndexSearcher = MultiCountryController.GetProductRetailerMapLuceneSearcher(countryId);
            TopDocs topDocs = productRetailerMapIndexSearcher.Search(retailerQuery, MAXDOCS);

            BooleanQuery queries = null;
            int resultsCount = topDocs.ScoreDocs.Length;
            if (resultsCount > 0)
            {
                queries = new BooleanQuery();
                for (int i = 0; i < resultsCount; i++)
                {
                    //ISet<string> s = new HashSet<string>();
                    //s.Add("ProductID");
                    //Document doc = productRetailerMapIndexSearcher.Doc(topDocs.ScoreDocs[i].Doc, s);
                    Document doc = productRetailerMapIndexSearcher.Doc(topDocs.ScoreDocs[i].Doc, new MapFieldSelector("ProductID"));
                    string pid = doc.Get("ProductID");
                    //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                    //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef);
                    //TermQuery _termQuery = new TermQuery(new Term("ProductID", btRef));
                    TermQuery _termQuery = new TermQuery(new Term("ProductID", pid));
                    queries.Add(_termQuery, Occur.SHOULD);
                }
            }
            return queries;
        }

        private static BooleanQuery GetAttributeValueRangeQuery(Dictionary<int, string> attributeValueRange, int countryId)
        {
            List<string> selectedProductIdList = new List<string>();

            IndexSearcher attributeIndexSearcher = MultiCountryController.GetAttributesLuceneSearcher(countryId);
            if (attributeValueRange != null && attributeValueRange.Count > 0 && attributeIndexSearcher != null)
            {
                foreach (var range in attributeValueRange)
                {
                    BooleanQuery queries = new BooleanQuery();
                    //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                    //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(range.Key, 0, btRef);
                    //TermQuery attributeTitleQuery = new TermQuery(new Term("TypeID", btRef));
                    TermQuery attributeTitleQuery = new TermQuery(new Term("TypeID", range.Key.ToString()));
                    queries.Add(attributeTitleQuery, Occur.MUST);

                    var range_ = range.Value.Split('-');
                    NumericRangeQuery<float> attRange = NumericRangeQuery.NewFloatRange("AttributeValue", float.Parse(range_[0]), float.Parse(range_[1]), true, true);
                    queries.Add(attRange, Occur.MUST);

                    if (selectedProductIdList.Count > 0)
                    {
                        BooleanQuery productidQuery = new BooleanQuery();
                        foreach (string pid in selectedProductIdList)
                        {
                            //Lucene.Net.Util.BytesRef btRef2 = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                            //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef2);
                            //TermQuery pidTermQuery = new TermQuery(new Term("ProductID", btRef2));
                            TermQuery pidTermQuery = new TermQuery(new Term("ProductID", pid));
                            productidQuery.Add(pidTermQuery, Occur.SHOULD);
                        }
                        queries.Add(productidQuery, Occur.MUST);
                        selectedProductIdList.Clear();
                    }

                    TopDocs topDocs = attributeIndexSearcher.Search(queries, MAXDOCS);
                    int resultsCount = topDocs.ScoreDocs.Length;
                    if (resultsCount > 0)
                    {
                        //ISet<string> s = new HashSet<string>();
                        //s.Add("ProductID");
                        for (int i = 0; i < resultsCount; i++)
                        {
                            //Document doc = attributeIndexSearcher.Doc(topDocs.ScoreDocs[i].Doc, s);
                            Document doc = attributeIndexSearcher.Doc(topDocs.ScoreDocs[i].Doc, new MapFieldSelector("ProductID"));
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
                //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef);
                //TermQuery termQuery = new TermQuery(new Term("ProductID", btRef));
                TermQuery termQuery = new TermQuery(new Term("ProductID", pid));
                productIDQueries.Add(termQuery, Occur.SHOULD);
            }

            return productIDQueries;
        }

        private static Filter GetPriceRangeFilter(PriceRange priceRange)
        {
            if (priceRange == null)
            {
                return null;
            }
            NumericRangeFilter<double> numericRangeFilter = NumericRangeFilter.NewDoubleRange(PRICERANGEFIELDNAME, priceRange.MinPrice, priceRange.MaxPrice, true, false);
            return numericRangeFilter;
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
                sort = new Sort(new SortField[] { new SortField("IsAccessory", SortField.STRING, false) });
            }
            else if (sortBy.Equals("accessories-rev", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField[] { new SortField("IsAccessory", SortField.STRING, true) });
            }
            else if (sortBy.Equals("adminclicks", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField[] { new SortField("Clicks", SortField.INT, true) });
            }
            else if (sortBy.Equals("clicks", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField[] { new SortField("IsAccessory", SortField.STRING, false), new SortField("Clicks", SortField.INT, true) });
                //sort = new Sort(new SortField[] { new SortField("Clicks", SortFieldType.INT32, true) });
            }
            else if (sortBy.Equals("clicks-rev", StringComparison.InvariantCultureIgnoreCase))
            {
                sort = new Sort(new SortField[] { new SortField("IsAccessory", SortField.STRING, true), new SortField("Clicks", SortField.INT, false) });
                //sort = new Sort(new SortField[] { new SortField("Clicks", SortFieldType.INT32, false) });
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
                sort = new Sort(new SortField("Sale", SortField.FLOAT, false));
            }
            else
            {
                sort = new Sort(new SortField(sortBy, SortField.STRING));
            }

            return sort;
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

        private static Query GetRetailerProductQuery(string[] kwList, Query query, int countryId)
        {
            BooleanQuery queries = new BooleanQuery();
            BooleanQuery keywordsQuery = new BooleanQuery();
            IndexSearcher searcher = MultiCountryController.GetRetailerProductsLuceneSearcher(countryId);
            string queryStringFormat = "*{0}*";
            foreach (string kw in kwList)
            {
                Lucene.Net.Index.Term term = new Lucene.Net.Index.Term("SearchField", string.Format(queryStringFormat, kw.ToLower()));
                WildcardQuery wildcardQuery = new WildcardQuery(term);
                keywordsQuery.Add(wildcardQuery, Occur.SHOULD);
            }
            TopFieldDocs topFieldDocs = searcher.Search(keywordsQuery, null, 1000, new Sort());
            BooleanQuery rpProductIDQuery = null;
            if (topFieldDocs.TotalHits > 0)
            {
                rpProductIDQuery = new BooleanQuery();
                for (int i = 0; i < topFieldDocs.ScoreDocs.Count(); i++)
                {
                    var docm = searcher.Doc(topFieldDocs.ScoreDocs[i].Doc);
                    string pid = docm.Get("ProductID");
                    //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                    //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef);
                    //TermQuery pidTermQuery = new TermQuery(new Term("ProductID", btRef));
                    TermQuery pidTermQuery = new TermQuery(new Term("ProductID", pid));
                    rpProductIDQuery.Add(pidTermQuery, Occur.SHOULD);
                }
                queries.Add(rpProductIDQuery, Occur.MUST);
                if (query != null)
                {
                    queries.Add(query, Occur.MUST);
                }
                return queries;
            }

            return null;
        }

        public static ProductCatalog GetProductCatalogFromDoc(Document hitDoc)
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

            return productCatalog;
        }

        public static ProductCatalog GetProductCatalog(HitsInfo hitsInfo, int index)
        {
            Document doc = hitsInfo.GetDocument(index);
            ProductCatalog productCatalog = GetProductCatalogFromDoc(doc);
            return productCatalog;
        }

        /// <summary>
        /// 获取分类的根类Id
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static int GetRootCategoryId(int cid)
        {
            if(CategoryRootMapDic_Static.ContainsKey(cid))
            {
                return CategoryRootMapDic_Static[cid];
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static ProductCatalog SearchProduct(string productId, int countryId)
        {
            int pid = int.Parse(productId);

            int[] pidArray = new int[1];
            pidArray[0] = pid;

            List<ProductCatalog> list = SearchProductsByPIDs(pidArray, countryId);
            if(list != null && list.Count > 0)
            {
                return list[0];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pids"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<ProductCatalog> GetBiggestPriceDropByProductIDs(string pids, int countryId)
        {
            BooleanQuery pidQuery = new BooleanQuery();
            string[] ids = pids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if(ids.Length > 0)
            {
                int[] pidArray = new int[ids.Length];
                for(int i = 0; i < ids.Length; i++)
                {
                    pidArray[i] = int.Parse(ids[i]);
                }
                return SearchProductsByPIDs(pidArray, countryId);
            }
            
            return new List<ProductCatalog>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productIDs"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<ProductCatalog> SearchProductsByPIDs(int[] productIDs, int countryId)
        {
            IndexSearcher searcher = GetSearcherByCategoryID(0, countryId);
            if (searcher == null)
            {
                return null;
            }

            BooleanQuery productIDsQuery = new BooleanQuery();
            foreach (int pid in productIDs)
            {
                //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(pid, 0, btRef);
                //TermQuery termQuery = new TermQuery(new Term("ProductID", btRef));
                TermQuery termQuery = new TermQuery(new Term("ProductID", pid.ToString()));
                productIDsQuery.Add(termQuery, Occur.SHOULD);
            }

            TopDocs topDocs = searcher.Search(productIDsQuery, null, 100000);
            HitsInfo resultHitsInfo = new HitsInfo(searcher, topDocs);

            List<ProductCatalog> productCatalogList = new List<ProductCatalog>();
            for (int i = 0; i < topDocs.TotalHits; i++)
            {
                ProductCatalog productCatalog = GetProductCatalog(resultHitsInfo, i);
                productCatalogList.Add(productCatalog);
            }

            return productCatalogList;
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

        public static HitsInfo SearchCategories(string keywords, int countryId)
        {
            keywords = keywords.ToLower();
            IndexSearcher searcher = MultiCountryController.GetCategoriesLuceneSearcher(countryId);
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

        public static HitsInfo SearchRetailer(List<string> productIDList, int countryId)
        {
            if (productIDList != null && productIDList.Count > 0)
            {
                IndexSearcher indexSearcher = MultiCountryController.GetProductRetailerMapLuceneSearcher(countryId);

                BooleanQuery booleanQuery = new BooleanQuery();
                foreach (string pid in productIDList)
                {
                    //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                    //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef);
                    //TermQuery productIDTermQuery = new TermQuery(new Term("ProductID", btRef));
                    TermQuery productIDTermQuery = new TermQuery(new Term("ProductID", pid));
                    booleanQuery.Add(productIDTermQuery, Occur.SHOULD);
                }

                TopDocs topDocs = indexSearcher.Search(booleanQuery, MAXDOCS);

                return new HitsInfo(indexSearcher, topDocs);
            }

            return null;
        }

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="productIDList"></param>
        /// <returns></returns>
        public static HitsInfo SearchAttributes(List<string> productIdList, int countryId)
        {
            if (productIdList != null && productIdList.Count > 0)
            {
                IndexSearcher searcher = MultiCountryController.GetAttributesLuceneSearcher(countryId);
                BooleanQuery booleanQuery = new BooleanQuery();
                foreach (string pid in productIdList)
                {
                    //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                    //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(int.Parse(pid), 0, btRef);
                    //TermQuery productIDTermQuery = new TermQuery(new Term("ProductID", btRef));
                    TermQuery productIDTermQuery = new TermQuery(new Term("ProductID", pid));
                    booleanQuery.Add(productIDTermQuery, Occur.SHOULD);
                }

                TopDocs topDocs = searcher.Search(booleanQuery, MAXDOCS);

                return new HitsInfo(searcher, topDocs);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MPN"></param>
        /// <param name="similar"></param>
        /// <returns></returns>
        public static string SearchRetailerProductMPN(string MPN, float similar, int countryId)
        {
            string fieldName = "MPN";
            //Lucene.Net.Analysis.Core.WhitespaceAnalyzer whitespaceAnalyzer = new Lucene.Net.Analysis.Core.WhitespaceAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            Lucene.Net.Analysis.WhitespaceAnalyzer whitespaceAnalyzer = new Lucene.Net.Analysis.WhitespaceAnalyzer();

            BooleanQuery booleanQuery = new BooleanQuery();
            Query query = new FuzzyQuery(new Term(fieldName, MPN.ToLower()), (int)similar, 0);
            booleanQuery.Add(query, Occur.MUST);

            IndexSearcher searcher = MultiCountryController.GetRetailerProductsLuceneSearcher(countryId);
            TopDocs topDocs = searcher.Search(booleanQuery, 100);

            if (topDocs.TotalHits > 0)
            {
                //ISet<string> s = new HashSet<string>();
                //s.Add(fieldName);
                //return searcher.Doc(topDocs.ScoreDocs[0].Doc, s).Get(fieldName);
                return searcher.Doc(topDocs.ScoreDocs[0].Doc, new MapFieldSelector(fieldName)).Get(fieldName);
            }
            return string.Empty;
        }

        public static HitsInfo SearchRetailerProduct(int retailerId, int categoryId, int count, bool isSearchonly, int countryId)
        {
            BooleanQuery booleanQuery = new BooleanQuery();
            if (retailerId > 0)
            {
                //Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                //Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(retailerId, 0, btRef);
                //TermQuery mergeQuery = new TermQuery(new Term("RetailerId", btRef));
                TermQuery mergeQuery = new TermQuery(new Term("RetailerId", retailerId.ToString()));
                booleanQuery.Add(mergeQuery, Occur.MUST);
            }
            if (categoryId > 0)
            {
                List<int> cList = new List<int>();
                cList.Add(categoryId);
                Query categoryQueries = GetCategoryQuery(cList, isSearchonly, countryId);
                booleanQuery.Add(categoryQueries, Occur.MUST);
            }

            Sort sort = new Sort(new SortField[] { new SortField("Clicks", SortField.INT, true) });

            IndexSearcher searcher = MultiCountryController.GetRetailerProductsLuceneSearcher(countryId);

            TopFieldDocs topFieldDocs = searcher.Search(booleanQuery, null, count, sort);

            return new HitsInfo(searcher, topFieldDocs);
        }

        public static List<RetailerProductCatalog> SearchTopRetailerProduct(int retailerId, int countryId)
        {
            RetailerProductSearcher retailerProductSearcher = new RetailerProductSearcher(retailerId, 0, countryId);
            SearchResult<RetailerProductCatalog> searchResult = retailerProductSearcher.GetSearchResult(1, 1000);

            return searchResult.ResultList;
        }

        public static void UpdateIndex(int countryId)
        {
            Dictionary<int, decimal> priceDic = GetProductPriceDic(countryId);
            if (priceDic.Count > 0)
            {
                int[] pids = priceDic.Keys.ToArray();
                List<ProductCatalog> pcList = SearchProductsByPIDs(pids, countryId);
                List<ProductCatalog> newPcList = new List<ProductCatalog>();
                foreach (var pc in pcList)
                {
                    int productId = int.Parse(pc.ProductID);
                    decimal bp = decimal.Parse(pc.BestPrice);
                    if(bp != priceDic[productId])
                    {
                        pc.BestPrice = priceDic[productId].ToString("0.00");
                        newPcList.Add(pc);
                    }
                }

                if (newPcList.Count > 0)
                {
                    string productIndexRootPath = MultiCountryController.GetAllCategoryProductsIndexRootPath(countryId);
                    ProductIndexUpdater productIndexUpdater = new ProductIndexUpdater(newPcList, productIndexRootPath);
                    productIndexUpdater.Update();
                    if(productIndexUpdater.UpdatedCount > 0)
                    {
                        MultiCountryController.ReopenProductIndex(countryId);
                    }
                }
            }
        }

        private static Dictionary<int, decimal> GetProductPriceDic(int countryId)
        {
            Dictionary<int, decimal> dic = new Dictionary<int, decimal>();

            string connectionString = MultiCountryController.GetDBConnectionString(countryId);
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                List<int> list = new List<int>();

                string querySql = @"SELECT [ProductId] FROM [dbo].[PurgedProduct] where IndexChecked = 0";
                sqlConn.Open();

                using (SqlCommand sqlCMD = new SqlCommand(querySql, sqlConn))
                {
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader sdr = sqlCMD.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            int productId = sdr.GetInt32(0);
                            if (!list.Contains(productId))
                            {
                                list.Add(productId);
                            }
                        }
                    }
                }

                if (list.Count > 0)
                {
                    string pidStr = string.Join(",", list);
                    string updateSql = "Update [dbo].[PurgedProduct] set IndexChecked = 1 where productId in (" + pidStr + ")";

                    using (SqlCommand sqlCMD = new SqlCommand(updateSql, sqlConn))
                    {
                        sqlCMD.CommandTimeout = 0;
                        sqlCMD.CommandType = System.Data.CommandType.Text;

                        sqlCMD.ExecuteNonQuery();
                    }

                    string priceSql = "select ProductId, min(RetailerPrice) as minp from CSK_Store_RetailerProductNew where ProductId in (" + pidStr + ") group by ProductId";
                    using (SqlCommand sqlCMD = new SqlCommand(priceSql, sqlConn))
                    {
                        sqlCMD.CommandTimeout = 0;
                        sqlCMD.CommandType = System.Data.CommandType.Text;

                        using (SqlDataReader sdr = sqlCMD.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                int productId = sdr.GetInt32(0);
                                decimal price = sdr.GetDecimal(1);

                                dic.Add(productId, price);
                            }
                        }
                    }
                }
            }

            return dic;
        }
    }
}