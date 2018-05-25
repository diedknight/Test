using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.App_Code_Not
{
    public class UserPage : System.Web.UI.Page
    {
        public UserData UserData = null;


        protected override void OnPreLoad(EventArgs e)
        {
            this.UserData = PriceMe.Utility.GetUserInfoFromCookie();

            base.OnPreLoad(e);
        }
    }
}