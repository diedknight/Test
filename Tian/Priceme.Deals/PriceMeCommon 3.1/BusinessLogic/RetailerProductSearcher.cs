using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeCache;
using PriceMeCommon.Data;

namespace PriceMeCommon.BusinessLogic
{
    public class RetailerProductSearcher
    {
        private int _retailerId;
        private int _categoryId;

        public RetailerProductSearcher(int retailerId, int categoryId)
        {
            _retailerId = retailerId;
            _categoryId = categoryId;
        }

        public List<CategoryResultsInfo> GetCategoryInfo()
        {
            List<CategoryResultsInfo> categoryInfo = new List<CategoryResultsInfo>();

            HitsInfo hitsInfo = SearchController.SearchRetailerProduct(_retailerId, 0, true);

            Dictionary<string, int> cidDictionary = new Dictionary<string, int>();
            for (int i = 0; i < hitsInfo.ResultCount; i++)
            {
                string cidString = hitsInfo.GetDocument(i, new string[] { "CategoryID" }).Get("CategoryID");
                if (cidDictionary.ContainsKey(cidString))
                {
                    cidDictionary[cidString]++;
                    continue;
                }
                else
                {
                    cidDictionary.Add(cidString, 1);
                }
            }

            foreach(string key in cidDictionary.Keys)
            {
                int cid = int.Parse(key);
                CategoryCache category = CategoryController.GetCategoryByCategoryID(cid);

                if(category != null)
                {
                    CategoryResultsInfo categoryResultsInfo = new CategoryResultsInfo(cid, category.CategoryName, cidDictionary[key]);
                    categoryInfo.Add(categoryResultsInfo);
                }
            }

            return categoryInfo;
        }

        public SearchResult<RetailerProductCatalog> GetSearchResult(int pageIndex, int pageSize)
        {
            HitsInfo _resultHitsInfo = SearchController.SearchRetailerProduct(_retailerId, _categoryId, true);
            SearchResult<RetailerProductCatalog> searchResult = new SearchResult<RetailerProductCatalog>();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0 || pageSize == 0)
            {
                return searchResult;
            }

            PagerInfo pagerInfo = new PagerInfo(pageIndex, pageSize, _resultHitsInfo.ResultCount);

            List<RetailerProductCatalog> resultList = new List<RetailerProductCatalog>();
            for (int i = pagerInfo.StartIndex; i < pagerInfo.EndIndex; i++)
            {
                RetailerProductCatalog productCatalog = SearchController.GetRetailerProductCatalog(_resultHitsInfo, i);
                resultList.Add(productCatalog);
            }

            searchResult.CurrentPageIndex = pagerInfo.CurrentPageIndex;
            searchResult.ResultList = resultList;
            searchResult.PageCount = pagerInfo.PageCount;
            searchResult.ProductCount = _resultHitsInfo.ResultCount;

            return searchResult;
        }
    }
}