using HotterWinds.DBQuery;
using HotterWinds.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds
{     
    public partial class Default : System.Web.UI.Page
    {        
        public HomeViewModel Model = new HomeViewModel();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = HttpUtility.HtmlDecode(System.Configuration.ConfigurationManager.AppSettings["HomePageTilte"]);

            List<int> cateIds = PriceMe.WebConfig.NPcategoryID.Split(',').Select(item => Convert.ToInt32(item)).ToList();

            Model.DressesName = PriceMeCommon.BusinessLogic.CategoryController.GetCategoryByCategoryID(cateIds[0], PriceMe.WebConfig.CountryId).CategoryName;
            Model.Dresses = HomeQuery.GetProducts(4, cateIds[0]);

            Model.SunGlassesName = PriceMeCommon.BusinessLogic.CategoryController.GetCategoryByCategoryID(cateIds[1], PriceMe.WebConfig.CountryId).CategoryName;
            Model.SunGlasses = HomeQuery.GetProducts(4, cateIds[1]);

            Model.TentsName = PriceMeCommon.BusinessLogic.CategoryController.GetCategoryByCategoryID(cateIds[2], PriceMe.WebConfig.CountryId).CategoryName;
            Model.Tents = HomeQuery.GetProducts(4, cateIds[2]);

            Model.BootsName = PriceMeCommon.BusinessLogic.CategoryController.GetCategoryByCategoryID(cateIds[3], PriceMe.WebConfig.CountryId).CategoryName;
            Model.Boots = HomeQuery.GetProducts(4, cateIds[3]);

            Model.BestSellerProducts = HomeQuery.GetBestSellerProducts();
            Model.FeaturesProducts = HomeQuery.GetFeatureProducts();
            //Model.HotDeal = HomeQuery.GetProducts(1, 806)[0];
            Model.HotDeal = Model.Dresses[0];
            Model.Blogs = HomeQuery.GetBlogs();
        }
    }
} 