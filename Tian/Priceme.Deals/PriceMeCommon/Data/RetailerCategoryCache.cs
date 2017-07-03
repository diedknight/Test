using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace PriceMeCommon.Data
{
    /// <summary>
    /// RetailerCategoryCache 的摘要说明
    /// </summary>
    public class RetailerCategoryCache
    {
        private int retailerCategoryTotal;
        public int RetailerCategoryTotal
        {
            get { return retailerCategoryTotal; }
            set { retailerCategoryTotal = value; }
        }

        private string retailerCategoryName;
        public string RetailerCategoryName
        {
            get { return retailerCategoryName; }
            set { retailerCategoryName = value; }
        }

        private string retailerCategoryID;
        public string RetailerCategoryID
        {
            get { return retailerCategoryID; }
            set { retailerCategoryID = value; }
        }

        public RetailerCategoryCache()
        {
        }
    }
}