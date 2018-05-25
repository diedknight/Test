using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules.Products
{
    public partial class HWProductReview : System.Web.UI.UserControl
    {
        public PriceMeDBA.CSK_Store_ProductNew Product = null;

        public UserData UserData = null;

        public List<HotterWindsDBA.CSK_Store_ProductReview> ProductReviewList = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.UserData = PriceMe.Utility.GetUserInfoFromCookie();



            this.UserData = new UserData();
            this.UserData.countryid = 0;
            this.UserData.createon = DateTime.Now;
            this.UserData.email = "";
            this.UserData.firstname = "";
            this.UserData.isapproveemail = true;
            this.UserData.lastname = "";
            this.UserData.logintype = 0;
            this.UserData.name = "";
            this.UserData.parseid = "";
            this.UserData.pass = "";
            this.UserData.sex = true;
            this.UserData.userid = "";

        }
    }
}