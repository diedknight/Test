using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceMe;
using PriceMeCommon;
using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Extend;
using HotterWinds.DBQuery;
using Dapper;

namespace HotterWinds.Modules.Catalog
{
    public partial class HWGridView : System.Web.UI.UserControl
    {
        public string ProductName;
        public string ProductID;
        public string ShortDescriptionZH;
        public string MaxPrice;
        public string BestPrice;
        public string DefaultImage;
        public string PriceCount;
        public string CategoryID;
        public string AvRating;
        protected double Score;
        public string BestPPCRetailerName;
        public string CatalogProductURL;
        public string BestPPCRetailerProductID;
        public string CatalogDescription;
        public string DisplayName;
        public string BestPPCRetailerID;
        public string IsSearchProduct = "0";
        public int DayCount;
        public bool IsUpComing;
        public string ImageAlt;
        public string StarsImageAlt;
        public string ClickOutUrl;
        public string StarsScore;
        public bool IsSearchOnly;
        public double PrevPrice;
        public float Sale;
        public float RatingPercent;
        public double CurrentPrice;
        public bool IsTop3;

        protected string VSOnclickScript;

        protected string linkUrl;
        protected bool isBestPPc = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProductID == null)
                return;
            if (DefaultImage == null)
            {
                DefaultImage = "";
            }

            if (string.IsNullOrEmpty(DefaultImage))
            {
                ImageAlt = "No Image";
            }
            else
            {
                ImageAlt = HttpUtility.HtmlEncode(ProductName);
            }
            
            double avR = 0;
            if (double.TryParse(AvRating, out avR))
            {
                Score = double.Parse(AvRating, PriceMeCommon.PriceMeStatic.Provider);
            }
            else
            {
                Score = 0d;
            }

            CatalogProductURL = UrlController.GetProductUrl(int.Parse(ProductID), ProductName);

            StarsScore = Utility.GetStarImage(Score);

            ProductName = ProductName.CutOut(70);

            ClickOutUrl = Utility.GetRootUrl("/ResponseRedirect.aspx?aid=40&pid=" + ProductID + "&rid=" + BestPPCRetailerID + "&rpid=" + BestPPCRetailerProductID + "&countryID=" + WebConfig.CountryId + "&cid=" + CategoryID + "&t=c", WebConfig.CountryId).Replace("&", "&amp;");
            string uuid = Guid.NewGuid().ToString();
            ClickOutUrl += "&uuid=" + uuid;

            linkUrl = CatalogProductURL;
            if (CategoryController.IsSearchOnly(int.Parse(CategoryID), WebConfig.CountryId))
            { 
                if (!string.IsNullOrEmpty(BestPPCRetailerName))
                {
                    linkUrl = ClickOutUrl;
                    isBestPPc = true;
                }
                else
                    linkUrl = string.Empty;
            }


            ShortDescriptionZH = GetDes(ProductID);
        }

        public string GetDes(string pid)
        {
            string sql = "select top 1 ShortDescriptionZH from CSK_Store_ProductNew where ProductID=" + pid;

            string des = HotterWindsQuery.GetConnection().ExecuteScalar<string>(sql);

            return des;
        }


    }
}