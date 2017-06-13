using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules
{
    public partial class Breadcrumbs : System.Web.UI.UserControl
    {
        public int CategoryId { get; set; }
        public string ProductName { get; set; }

        protected List<ViewModels.CategoryV> CategoryList = new List<ViewModels.CategoryV>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CategoryId > 0)
            {
                CategoryList = DBQuery.ProductQuery.GetCategoryList(CategoryId);
            }
        }
    }
}