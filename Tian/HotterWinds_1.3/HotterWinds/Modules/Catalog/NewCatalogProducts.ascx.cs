using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules.Catalog
{
    public partial class NewCatalogProducts : System.Web.UI.UserControl
    {
        public string IsSearchProduct = "0";
        public int RootCategoryID;
        public List<ProductCatalog> ProductCatalogList;
        public bool ShowCompareBox = false;
        public string View = "";

        public bool IsFilterByBrand;
        public List<PriceMeCommon.Data.CatalogManufeaturerProduct> QuickCatalogManufeaturerProductList;
        public bool IsAdmin = false;
        public List<NarrowByInfo> AttributesInfo;
        public Dictionary<string, string> CurrentPS;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (View.Equals("list", StringComparison.InvariantCultureIgnoreCase))
            {
                //RepeaterList.DataSource = ProductCatalogList;
                //RepeaterList.DataBind();

                RepeaterGridHW.DataSource = ProductCatalogList;
                RepeaterGridHW.DataBind();
            }
            else if (View.Equals("grid", StringComparison.InvariantCultureIgnoreCase))
            {
                //RepeaterGrid.DataSource = ProductCatalogList;
                //RepeaterGrid.DataBind();

                RepeaterGridHW.DataSource = ProductCatalogList;
                RepeaterGridHW.DataBind();
            }
            //else
            //{
            //    if (IsAdmin)
            //    {
            //        int maxCount = 2;
            //        List<int> attTypes = new List<int>();
            //        for (int i = 0; i < AttributesInfo.Count; i++)
            //        {
            //            if (i == maxCount)
            //                break;

            //            attTypes.Add(AttributesInfo[i].ID);
            //        }

            //        List<string> productIDs = new List<string>();
            //        foreach (var cmp in QuickCatalogManufeaturerProductList)
            //        {
            //            productIDs.AddRange(cmp.ProductCatalogCollection.Select(pc => pc.ProductID).ToList());
            //        }

            //        Dictionary<int, Dictionary<string, int>> dic = AttributesController.GetProductsAttributes(attTypes, productIDs, PriceMe.WebConfig.CountryId);
            //        this.QuickProductSummaryDisplay1.ProductsAttributes = dic;
            //    }
            //    this.QuickProductSummaryDisplay1.catalogManufeaturerProductList = QuickCatalogManufeaturerProductList;
            //    this.QuickProductSummaryDisplay1.currentPs = CurrentPS;
            //    this.QuickProductSummaryDisplay1.IsFilterByBrand = IsFilterByBrand;

            //    if (IsSearchProduct != "0")
            //    {
            //        this.QuickProductSummaryDisplay1.PageTo = PriceMe.PageName.Search;
            //    }
            //}
        }
    }
}