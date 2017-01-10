using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class CatalogSitemapCategory
    {
        LinkInfo link = new LinkInfo();
        int categoryID;
        string imageURL;
        List<LinkInfo> subCategoryInfos;

        public LinkInfo Link
        {
            get { return link; }
            set { link = value; }
        }

        public string ImageURL
        {
            get { return imageURL; }
            set { imageURL = value; }
        }
        public string IconUrl { get; set; }
        public string IconCode { get; set; }
        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        public List<LinkInfo> SubCategoryInfos
        {
            get { return subCategoryInfos; }
            set { subCategoryInfos = value; }
        }
    }
}