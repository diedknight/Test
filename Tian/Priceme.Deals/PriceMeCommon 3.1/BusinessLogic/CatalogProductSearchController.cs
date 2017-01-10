using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class CatalogProductSearchController
    {
        public static CatalogPageInfo SearchProducts(
            int categoryID,
            List<int> manufeaturerIDs,
            PriceRange priceRange,
            List<int> selectedAttributeIDs,
            List<int> selectedAttributeRangeIDs,
            string sortBy,
            string keywords,
            List<int> selectedRetailers,
            bool useIsDisplayIsMerged,
            int countryID,
            bool MultiAttributes,
            Dictionary<int, string> selectedAttrRangeValues,
            bool isSearchonly,
            int pageIndex,
            int pageSize,
            DaysRange daysRange)
        {
            CatalogPageInfo catalogPageInfo = new CatalogPageInfo();

            ProductSearcher productSearcher = new ProductSearcher(categoryID, manufeaturerIDs, priceRange, selectedAttributeIDs, selectedAttributeRangeIDs, sortBy, keywords, selectedRetailers, true, ConfigAppString.CountryID, false, selectedAttrRangeValues, isSearchonly, daysRange);
            SearchResult searchResult = productSearcher.GetSearchResult(pageIndex, pageSize);

            catalogPageInfo.CurrentProductCount = productSearcher.GetProductCount();
            catalogPageInfo.PageCount = searchResult.PageCount;
            catalogPageInfo.ProductCatalogList = searchResult.ProductCatalogList;
            catalogPageInfo.MyProductSearcher = productSearcher;

            return catalogPageInfo;
        }

        public static void FixFiltersProductCount(NarrowByInfo narrowByInfo, NarrowByInfo narrowByInfoWithP)
        {
            if(narrowByInfo.NarrowItemList != null && narrowByInfo.NarrowItemList.Count > 0)
            {
                if (narrowByInfo.NarrowItemList != null && narrowByInfo.NarrowItemList.Count > 0)
                {
                    foreach (var ni in narrowByInfo.NarrowItemList)
                    {
                        ni.ProductCount = 0;
                    }
                }

                if (narrowByInfoWithP.NarrowItemList != null && narrowByInfoWithP.NarrowItemList.Count > 0)
                {
                    foreach (var ni in narrowByInfoWithP.NarrowItemList)
                    {
                        FixItemProductCount(narrowByInfo.NarrowItemList, ni);
                    }
                }
            }
        }

        private static void FixItemProductCount(List<NarrowItem> narrowItemList, NarrowItem ni)
        {
            foreach(var item in narrowItemList)
            {
                if (item.Value == ni.Value)
                {
                    item.ProductCount = ni.ProductCount;
                    return;
                }
            }
        }

        public static void FixAttributesNarrowByInfoProductCount(List<NarrowByInfo> attributesNarrowByInfoList, List<NarrowByInfo> attributesNarrowByInfoListWithP)
        {
            foreach (var ani in attributesNarrowByInfoList)
            {
                if (ani.NarrowItemList != null && ani.NarrowItemList.Count > 0)
                {
                    foreach (var ni in ani.NarrowItemList)
                    {
                        ni.ProductCount = 0;
                    }
                }

                foreach (var aniWithP in attributesNarrowByInfoListWithP)
                {
                    if (ani.Title == aniWithP.Title)
                    {
                        CatalogProductSearchController.FixFiltersProductCount(ani, aniWithP);
                        break;
                    }
                }
            }
        }
    }
}