using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PriceMeCache;
using PriceMeCommon.Data;

namespace HotterWinds.Modules.Catalog
{
    public partial class CatalogSiteMapNew : System.Web.UI.UserControl
    {
        public List<CatalogSitemapCategory> catalogSitemapCategories;
        public List<CatalogSitemapCategory> catalogSitemapPopularCategories;
        public CategoryCache Category;
        public ManufacturerInfo Manufacturer;
        public bool HasDealsProduct = false;

        protected string titleString;
        protected string description;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Category == null) return;

            titleString = string.Format(Resources.Resource.Module_CatalogSiteMap_Title, Category.CategoryName);

            description = Category.ShortDescription.ToString().Replace("New Zealand", Resources.Resource.Country).Trim();
            if (string.IsNullOrEmpty(description))
            {
                string descriptionFormat = Resources.Resource.String_DefaultCategoryDescription;
                description = string.Format(descriptionFormat, Category.CategoryName, "");
            }

            //if (Consumer1 != null)
            //{
            //    Consumer1.categoryid = Category.CategoryID;
            //    Consumer1.categoryname = Category.CategoryName;
            //    Consumer1.paddingsize = 7;
            //}
        }
    }
}