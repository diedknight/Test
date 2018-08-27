using System;

namespace ProductSearchIndexBuilder.Data
{
    public class CategoryExtend
    {
        public int CategoryID { get; set; }
        public int CountryID { get; set; }
        public string Synonym { get; set; }
        public string LocalName { get; set; }
    }
}