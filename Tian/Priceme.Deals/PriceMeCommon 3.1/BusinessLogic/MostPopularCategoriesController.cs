using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeCommon.Data;

namespace PriceMeCommon.BusinessLogic
{
    public class MostPopularCategoriesController
    {
        public static Dictionary<int, List<MostPopularCategory>> GetDatas()
        {
            Dictionary<int, List<MostPopularCategory>> datas = new Dictionary<int, List<MostPopularCategory>>();
            if (CategoryController.MostPopularCategories != null)
            {
                CategoryController.MostPopularCategories.ForEach(mc =>
                {
                    if (!datas.ContainsKey(mc.RootID))
                    {
                        datas.Add(mc.RootID, new List<MostPopularCategory>());
                    }
                    datas[mc.RootID].Add(mc);
                });
            }
            else
            {
                int[] popularCategoriesIDList = new int[]{1,365,189,6,355,1284,184,616};

                foreach (int cid in popularCategoriesIDList)
                {
                    List<MostPopularCategory> mostPopularCategoryList = new List<MostPopularCategory>();
                    var category = PriceMeCommon.CategoryController.GetCategoryByCategoryID(cid);
                    if (category != null)
                    {
                        PriceMeCommon.ProductSearcher productSearcher = new PriceMeCommon.ProductSearcher(cid, null, null, null, null, "clicks", "", null, true, PriceMeCommon.ConfigAppString.CountryID, false, false);
                        var searchResult = productSearcher.GetCatalogCategoryResulte();

                        if (searchResult.NarrowItemList.Count > 0)
                        {
                            foreach (var ni in searchResult.NarrowItemList)
                            {
                                MostPopularCategory mostPopularCategory = new MostPopularCategory();
                                mostPopularCategory.CategoryID = int.Parse(ni.Value);
                                mostPopularCategoryList.Add(mostPopularCategory);
                                if (mostPopularCategoryList.Count >= 5)
                                {
                                    break;
                                }
                            }
                        }
                        datas.Add(cid, mostPopularCategoryList);
                    }
                }
            }
            return datas;
        }
    }
}
