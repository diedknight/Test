using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceMe;
using PriceMeDBA;
using PriceMeCommon;
using PriceMeCommon.Data;
using PriceMeCommon.BusinessLogic;

namespace HotterWinds.Modules.Catalog
{
    public partial class CatalogSiteMapDetailNew : System.Web.UI.UserControl
    {
        const int TopCount = 5;

        public PriceMeCache.CategoryCache Category;
        public ManufacturerInfo Manufacturer;
        public bool HasDealsProduct = false;

        protected Timer.DKTimer dkTimer;

        protected void Page_Load(object sender, EventArgs e)
        {
            //dkTimer = new Timer.DKTimer();
            //Session["DKTimer"] = dkTimer;
            //dkTimer.Start();
            //dkTimer.Set("Begin Sitemap.Page_Load()");

            if (Category == null) return;

            //if (Consumer1 != null)
            //{
            //    Consumer1.categoryid = Category.CategoryID;
            //    Consumer1.categoryname = Category.CategoryName;
            //}

            ProductSearcher productSearcher = new ProductSearcher("", Category.CategoryID, null, null, null, null, "Clicks", null, SearchController.MaxSearchCount_Static, WebConfig.CountryId, false, true, false, true, null, "", null);
            SearchResult sr = productSearcher.GetSearchResult(1, 4);
            //DataList1.DataSource = sr.ProductCatalogList;
            //DataList1.DataBind();
        }

        public NarrowByInfo GetAllTopManufacturerCache(int cid)
        {
            Dictionary<string, string> ps = new Dictionary<string, string>();
            ps.Add("c", cid.ToString());
            ProductSearcher productSearcher = new ProductSearcher("", cid, null, null, null, null, "", null, SearchController.MaxSearchCount_Static, WebConfig.CountryId, false, true, false, true, null, "", null);
            NarrowByInfo narrowByInfo = productSearcher.GetTopManufacturerResulte(TopCount);
            UrlController.SetNarrowByInfoUrl(narrowByInfo, PageName.Catalog, ps, null);
            return narrowByInfo;
        }
    }
}