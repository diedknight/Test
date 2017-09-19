using PriceMe;
using PriceMeCommon.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules
{
    public partial class ManCategoryList : System.Web.UI.UserControl
    {
        public List<PriceMeCache.CategoryCache> RootCategoryList = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //RootCategoryList = CategoryController.GetAllRootCategoriesOrderByName(WebConfig.CountryId);

            RootCategoryList = new List<PriceMeCache.CategoryCache>();

            System.Configuration.ConfigurationManager.AppSettings["RuleCategoryID"]
                .Split(',')
                .Select(item => Convert.ToInt32(item))
                .ToList()
                .ForEach(cid =>
                {
                    RootCategoryList.Add(CategoryController.GetRootCategory(cid, WebConfig.CountryId));
                });
        }
    }
}