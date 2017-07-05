﻿using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class PopularSearcherController
    {
        public static void GetSuggestKeywords(string input, int pCount, int cCount, int bCount, out Dictionary<string, LinkInfo> products, out Dictionary<string, LinkInfo> categories, out Dictionary<string, LinkInfo> brands, out Dictionary<string, LinkInfo> brandAndCategory, out Dictionary<string, LinkInfo> retailers, int countryId)
        {
            List<LinkInfo> otherInfos;
            GetSuggestKeywords(null, input, pCount, cCount, bCount, false, out products, out categories, out brands, out brandAndCategory, out retailers, out otherInfos, "", countryId);
        }

        public static void GetSuggestKeywords(List<int> categoryIDList, string input, int pCount, int cCount, int bCount, bool ppcOnly, out Dictionary<string, LinkInfo> products, out Dictionary<string, LinkInfo> categories, out Dictionary<string, LinkInfo> brands, out Dictionary<string, LinkInfo> brandAndCategory, out Dictionary<string, LinkInfo> retailers, out List<LinkInfo> otherInfos, string excludePids, int countryId)
        {
            IndexSearcher searcher = MultiCountryController.GetPopularLuceneSearcher(countryId);

            products = new Dictionary<string, LinkInfo>();
            categories = new Dictionary<string, LinkInfo>();
            brands = new Dictionary<string, LinkInfo>();
            brandAndCategory = new Dictionary<string, LinkInfo>();
            retailers = new Dictionary<string, LinkInfo>();
            otherInfos = new List<LinkInfo>();
            List<string> excludePidList = new List<string>();
            if (!string.IsNullOrEmpty(excludePids))
            {
                excludePidList.AddRange(excludePids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }

            Sort sort = new Sort(new SortField("Clicks", Lucene.Net.Search.SortFieldType.INT32, true));

            TopDocs tds1 = null;
            Task task1 = new Task(() => {
                BooleanQuery bq = new BooleanQuery();

                BooleanQuery nameQuery = new BooleanQuery();
                Query query1 = new WildcardQuery(new Term("Name", input + "*"));
                Query query2 = new WildcardQuery(new Term("Name", "*" + input));
                nameQuery.Add(query1, Occur.SHOULD);
                nameQuery.Add(query2, Occur.SHOULD);
                bq.Add(nameQuery, Occur.MUST);

                BooleanQuery categoryQuery = SearchController.GetCategoryQuery(categoryIDList, true, countryId);
                if (categoryQuery != null)
                {
                    Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                    Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(-1, 0, btRef);
                    TermQuery termQuery = new TermQuery(new Term("CategoryID", btRef));
                    categoryQuery.Add(termQuery, Occur.SHOULD);
                    bq.Add(categoryQuery, Occur.MUST);
                }

                if (ppcOnly)
                {
                    BooleanQuery ppcQuery = new BooleanQuery();
                    TermQuery ppcTermQuery = new TermQuery(new Term("IncludePPC", "1"));
                    ppcQuery.Add(ppcTermQuery, Occur.SHOULD);
                    Lucene.Net.Util.BytesRef btRef = new Lucene.Net.Util.BytesRef(Lucene.Net.Util.NumericUtils.BUF_SIZE_INT32);
                    Lucene.Net.Util.NumericUtils.Int32ToPrefixCoded(-1, 0, btRef);
                    TermQuery categoryTermQuery = new TermQuery(new Term("CategoryID", btRef));
                    ppcQuery.Add(categoryTermQuery, Occur.SHOULD);
                    bq.Add(ppcQuery, Occur.MUST);
                }
                
                tds1 = searcher.Search(bq, null, 600, sort);
            });
            task1.Start();

            TopDocs tds2 = null;
            Task task2 = new Task(() =>
            {
                string[] keys = Regex.Split(input, @"\s+");
                BooleanQuery bq = new BooleanQuery();

                for (int i = 0; i < keys.Length; i++)
                {
                    WildcardQuery wq = new WildcardQuery(new Term("Name", "*" + keys[i] + "*"));
                    bq.Add(wq, Occur.MUST);
                }

                tds2 = searcher.Search(bq, null, 1000, sort);
            });
            task2.Start();

            task1.Wait();
            int hitCount = tds1.ScoreDocs.Length;

            if (hitCount == 0)
            {
                task2.Wait();
                hitCount = tds2.ScoreDocs.Length;
                tds1 = tds2;
            }

            Dictionary<string, string> result = new Dictionary<string, string>();
            string name, type, id, other, minPrice;
            Document doc;
            for (int i = 0; i < hitCount; i++)
            {
                doc = searcher.Doc(tds1.ScoreDocs[i].Doc);
                name = doc.Get("DisplayValue");
                type = doc.Get("Type");
                id = doc.Get("ID");
                other = doc.Get("Other");

                LinkInfo linkInfo = new LinkInfo();
                linkInfo.LinkText = name;
                linkInfo.Value = other;
                linkInfo.ListOrder = doc.Get("Clicks");

                if (type == "P" || type.ToLower() == "ucp")
                {
                    if (products.ContainsKey(id) || excludePidList.Contains(id) || products.Count >= pCount)
                        continue;
                    minPrice = doc.Get("MinPrice");
                    int CategoryID, PPC_RetailerProductID, PPC_RetailerID;
                    int.TryParse(doc.Get("CategoryID"), out CategoryID);
                    int.TryParse(doc.Get("PPC_RetailerProductID"), out PPC_RetailerProductID);
                    int.TryParse(doc.Get("PPC_RetailerID"), out PPC_RetailerID);
                    decimal ppc_minprice = 0;
                    decimal.TryParse(doc.Get("PPC_MinPrice"), out ppc_minprice);

                    linkInfo.Title = minPrice;
                    linkInfo.ImageUrl = doc.Get("DefaultImage");
                    linkInfo.CategoryID = CategoryID;
                    linkInfo.PPC_RetailerID = PPC_RetailerID;
                    linkInfo.PPC_RetailerProductID = PPC_RetailerProductID;
                    linkInfo.PPC_MinPrice = ppc_minprice;
                    products.Add(id, linkInfo);
                }
                else if (type == "C")
                {
                    if (categories.ContainsKey(id) || categories.Count >= cCount)
                        continue;

                    categories.Add(id, linkInfo);
                }
                else if (type == "M")
                {
                    if (brands.ContainsKey(id) || brands.Count >= bCount)
                        continue;

                    linkInfo.Value = "Brand";
                    brands.Add(id, linkInfo);
                }
                else if (type == "BAC")
                {
                    if (brandAndCategory.ContainsKey(id) || brandAndCategory.Count >= 20)
                        continue;

                    brandAndCategory.Add(id, linkInfo);
                }
                else if (type == "R")
                {
                    if (retailers.ContainsKey(id) || retailers.Count >= cCount)
                        continue;

                    retailers.Add(id, linkInfo);
                }
                else if (type == "C1")
                {
                    linkInfo.LinkURL = linkInfo.Value;
                    otherInfos.Add(linkInfo);
                }

                if (products.Count > pCount && categories.Count > cCount && brands.Count > bCount && retailers.Count > cCount)
                    break;
            }
        }

    }
}