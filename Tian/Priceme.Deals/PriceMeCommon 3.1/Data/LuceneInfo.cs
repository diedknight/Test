using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;

namespace PriceMeCommon.Data
{
    public class LuceneInfo
    {
        public int CountryID;
        public Dictionary<int, Searcher> CategoriesProductLuceneIndex;
        public Searcher CategoriesIndexSearcher;
        public Searcher AttributesIndexSearcher;
        public Searcher ProductRetailerMapIndexSearcher;
        public Searcher RetailerProductsIndexSearcher;
        public Searcher AllBrandsIndexSearcher;
        public Searcher PopularIndexSearcher;
    }
}
