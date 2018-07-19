using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndexBuildCommon.Data
{
    public class CategoryProductIndexParameter
    {
        public string Path;
        public PriceMeCache.CategoryCache Category;
        public int CountryID;
        public bool UseCreatedOn;
        public Dictionary<int, List<PriceMeCache.ProductCatalog>> UpComingProductDic;
        public Dictionary<int, decimal> PrevPriceDic;
    }
}
