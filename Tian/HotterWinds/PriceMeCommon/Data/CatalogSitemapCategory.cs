using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class CatalogSitemapCategory
    {
        public LinkInfo Link { get; set; }

        public string ImageURL { get; set; }
        public string IconUrl { get; set; }
        public string IconCode { get; set; }
        public int CategoryID { get; set; }

        public List<LinkInfo> SubCategoryInfos { get; set; }

        public CatalogSitemapCategory()
        {
            Link = new LinkInfo();
        }
    }
}