using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules
{
    public partial class MobileSite : System.Web.UI.UserControl
    {
        public string mobileSiteUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Page.Title = "Mobile Site - Compare price on your mobile phone";
            mobileSiteUrl = Resources.Resource.Global_HomePageUrl + "/MobileSite.aspx";
        }
    }
}