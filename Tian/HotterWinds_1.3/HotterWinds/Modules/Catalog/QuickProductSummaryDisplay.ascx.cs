using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules.Catalog
{
    public partial class QuickProductSummaryDisplay : System.Web.UI.UserControl
    {
        public List<PriceMeCommon.Data.CatalogManufeaturerProduct> catalogManufeaturerProductList;
        public PriceMe.PageName PageTo = PriceMe.PageName.Catalog;
        public Dictionary<string, string> currentPs;
        public Dictionary<int, Dictionary<string, int>> ProductsAttributes;
        public bool IsFilterByBrand;

        public List<ProductCatalog> productCatalogArrayF;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsFilterByBrand && catalogManufeaturerProductList != null)
            {
                productCatalogArrayF = new List<ProductCatalog>();
                if (catalogManufeaturerProductList != null)
                {
                    foreach (PriceMeCommon.Data.CatalogManufeaturerProduct cmp in catalogManufeaturerProductList)
                    {
                        productCatalogArrayF.AddRange(cmp.ProductCatalogCollection);
                    }
                    productCatalogArrayF = productCatalogArrayF.OrderBy(pc => pc.ProductName).ToList();
                }
            }
        }
    }
}