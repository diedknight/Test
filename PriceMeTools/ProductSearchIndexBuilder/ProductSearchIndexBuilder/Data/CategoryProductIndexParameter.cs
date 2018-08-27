using System;
using System.Collections.Generic;

namespace ProductSearchIndexBuilder.Data
{
    public class CategoryProductIndexParameter
    {
        public string Path;
        public CategoryCache Category;
        public int CountryID;
        public bool UseCreatedOn;
        public Dictionary<int, List<ProductCatalog>> UpComingProductDic;
        public Dictionary<int, decimal> PrevPriceDic;
        public DbInfo PamUserDbInfo;
        public DbInfo PriceMe205DbInfo;
    }
}