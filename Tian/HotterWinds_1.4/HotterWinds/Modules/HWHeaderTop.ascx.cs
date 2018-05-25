﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules
{
    public partial class HWHeaderTop : System.Web.UI.UserControl
    {
        public UserData UserData = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.UserData = PriceMe.Utility.GetUserInfoFromCookie();
        }
    }
}