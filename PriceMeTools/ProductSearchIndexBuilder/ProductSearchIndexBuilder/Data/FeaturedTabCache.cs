using System;
using System.Collections.Generic;

namespace ProductSearchIndexBuilder.Data
{
    public class FeaturedTabCache
    {
        public int CategoryID { get; set; }

        public string Label { get; set; }

        public string Title { get; set; }

        public int ListOrder { get; set; }
        public string TabName { get; set; }

        public List<FeaturedProduct> FeaturedProductList { get; set; }
    }

    public class FeaturedProduct
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string DefaultImage { get; set; }

        public int CategoryID { get; set; }

        public int RootID { get; set; }

        public double MinPrice { get; set; }

        public string ProductGUID { get; set; }

        public double MaxPrice { get; set; }
    }
}