using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Priceme.Deals.Code.BusinessLogic
{
    public class DealProduct
    {
        public static bool ExistProduct(int cid)
        {
            return GetProducts(1, 20, "Sale", null, cid).Amount > 0;
        }

        public static ProductResult GetProducts(int curPage, int pageSize, string orderBy, List<int> recentViewCids, params int[] cids)
        {
            //if (cids.Length == 0) cids = CategoryController.GetAllRootCategoriesOrderByName(PriceMe.WebConfig.CountryId).Select(item => item.CategoryID).ToArray();

            List<PriceMeCommon.Data.ProductCatalog> list = new List<PriceMeCommon.Data.ProductCatalog>();

            float saleRate = -0.01f;
            string saleRateStr = System.Configuration.ConfigurationManager.AppSettings["SaleRate"];
            if (!string.IsNullOrEmpty(saleRateStr))
            {
                saleRateStr = saleRateStr.Replace("%", "");
                if (!float.TryParse(saleRateStr, out saleRate))
                {
                    saleRate = -0.01f;
                }

                if (saleRate > 1) saleRate = saleRate / 100f;
                if (saleRate > 0) saleRate = -saleRate;
            }

            double minimunPrice = 0.01d;
            if (!double.TryParse(System.Configuration.ConfigurationManager.AppSettings["minimumPrice"], out minimunPrice))
            {
                minimunPrice = 0.01d;
            }

            var hitInfo = SearchController.SearchProducts("", cids.ToList(), null, new PriceRange(minimunPrice, 0d), null, null, orderBy, null, 100000, PriceMe.WebConfig.CountryId, false, true, false, null, false, null, "", true, saleRate);

            int amount = hitInfo.ResultCount;
            int pageCount = amount / pageSize;
            if (amount % pageSize > 0) pageCount += 1;

            NumRange range = new NumRange(curPage, pageSize, pageCount, amount);

            if (recentViewCids == null || recentViewCids.Count == 0)
            {
                range.ForEach(index =>
                {
                    list.Add(SearchController.GetProductCatalog(hitInfo, index));
                });
            }
            else
            {
                List<PriceMeCommon.Data.ProductCatalog> tempList = new List<PriceMeCommon.Data.ProductCatalog>();
                List<PriceMeCommon.Data.ProductCatalog> tempResultList = new List<PriceMeCommon.Data.ProductCatalog>();

                for (int i = 0; i < amount; i++)
                {
                    var product = SearchController.GetProductCatalog(hitInfo, i);

                    if (recentViewCids.Contains(product.CategoryID))
                        tempResultList.Add(product);
                    else
                        tempList.Add(product);
                }
                tempResultList.AddRange(tempList);

                range.ForEach(index =>
                {
                    list.Add(tempResultList[index]);
                });
            }

            ProductResult result = new ProductResult();
            result.Amount = amount;
            result.Products = list;

            return result;
        }


        //class
        public class ProductResult
        {
            public int Amount { get; set; }

            public List<PriceMeCommon.Data.ProductCatalog> Products { get; set; }
        }

        private class NumRange
        {
            public int Start { get; private set; }
            public int End { get; private set; }
            public int Amount { get; private set; }

            public NumRange(int curPage, int pageSize, int pageCount, int amount)
            {
                this.Amount = amount;
                if (pageCount == 0) pageCount = 1;
                if (curPage > pageCount) curPage = pageCount;


                if (curPage == 1)
                {
                    this.Start = 1;
                    this.End = amount > pageSize ? pageSize : amount;
                    return;
                }

                if (curPage == pageCount)
                {
                    this.Start = (curPage - 1) * pageSize + 1;
                    this.End = amount;
                    return;
                }

                this.Start = (curPage - 1) * pageSize + 1;
                this.End = curPage * pageSize;
            }

            public void ForEach(Action<int> action)
            {
                if (action == null) return;

                for (int i = this.Start - 1; i < this.End; i++)
                {
                    action(i);
                }
            }
        }
    }
}