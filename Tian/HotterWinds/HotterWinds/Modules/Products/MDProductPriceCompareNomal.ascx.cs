using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceMeDBA;
using PriceMe;

namespace HotterWinds.Modules.Products
{
    public partial class MDProductPriceCompareNomal : System.Web.UI.UserControl
    {
        public List<CSK_Store_RetailerProductNew> DataSource;
        public int CategoryId;
        public int PricesCount = 0;
        public int ProductID;
        public bool isSinglePrice;
        protected void Page_Load(object sender, EventArgs e)
        {
            dtProducts.DataSource = DataSource;
            dtProducts.DataBind();
        }
    }
}