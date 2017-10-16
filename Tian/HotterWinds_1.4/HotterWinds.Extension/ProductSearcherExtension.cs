using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotterWinds.Extension
{
    public static class ProductSearcherExtension
    {
        public static NarrowByInfo GetCatalogCategoryResulte(this ProductSearcher ps,int categoryId,int countryId)
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();

            List<PriceMeCache.CategoryCache> nextLevelSubCategories = CategoryController.GetNextLevelSubCategories(categoryId, countryId);
            if (nextLevelSubCategories == null || nextLevelSubCategories.Count == 0)
            {
                return narrowByInfo;
            }

            narrowByInfo.ListOrder = 1;
            narrowByInfo.Title = "Categories";
            narrowByInfo.Name = "Catalog Category";

            List<NarrowItem> narrowList = new List<NarrowItem>();

            foreach (PriceMeCache.CategoryCache category in nextLevelSubCategories)
            {
                if (CategoryController.IsSearchOnly(category.CategoryID, countryId))
                    continue;

                NarrowItem narrowItem = new NarrowItem();
                narrowItem.DisplayName = category.CategoryName;
                narrowItem.IsPopular = category.PopularCategory;
                narrowItem.Value = category.CategoryID.ToString();
                narrowItem.ProductCount = CategoryController.GetCategoryProductCount(category.CategoryID, countryId);
                if (narrowItem.ProductCount == 0)
                    continue;
                narrowList.Add(narrowItem);
            }
            narrowByInfo.NarrowItemList = narrowList;

            return narrowByInfo;
        }
    }
}
