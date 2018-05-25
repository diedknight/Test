using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie hc = Request.Cookies["our_custom_session_cookienew_xxxx"];
            if (hc != null)
            {
                hc.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(hc);
            }

            Response.Redirect("/", false);
        }
    }
}