using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using PriceMeDBA;
using PriceMe;
using PriceMeCommon.Data;

namespace HotterWinds.Modules.Catalog
{
    public partial class CategoryTopControl : System.Web.UI.UserControl
    {
        public PriceMeCache.CategoryCache category = null;
        public ManufacturerInfo manufacturer = null;
        public bool HasProduct = true;
        public AttributeParameterCollection attributeParameterList;

        public string priceTitleInfo = "";
        public string retailerTitleInfo = "";

        protected string endString = "";

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}