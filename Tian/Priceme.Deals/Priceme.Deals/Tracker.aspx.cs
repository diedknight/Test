using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Priceme.Deals
{
    public partial class Tracker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = HttpUtility.UrlDecode(this.Request.QueryString["id"]);
            string type = HttpUtility.UrlDecode(this.Request.QueryString["type"]);

            string url = PriceMeCommon.Data.DealsVoucher.GetUrl(Convert.ToInt32(id));

            HttpContext.Current.Response.Status = "301 Moved Permanently";
            HttpContext.Current.Response.AddHeader("Location", url);
            HttpContext.Current.Response.End();
        }
    }
}