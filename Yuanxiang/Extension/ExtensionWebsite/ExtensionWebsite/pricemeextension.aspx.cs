using ExtensionWebsite.Code;
using ExtensionWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExtensionWebsite
{
    public partial class pricemeextension : System.Web.UI.Page
    {
        protected string key;
        protected bool isReturn = false;
        protected int countryid;
        protected List<RetailerProduct> datas;
        protected string rpurl;
        protected string stringsave;
        protected string track;
        protected bool islongprice = false;
        protected string homeurl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            key = Utility.GetParameter("key");
            key = System.Web.HttpUtility.UrlDecode(key);

            if (!string.IsNullOrEmpty(key))
            {
                List<Retailer> retailers = SiteConfig.ListRetailer.Where(r => key.Contains(r.RetailerUrl)).ToList();
                if (retailers != null && retailers.Count > 0)
                {
                    countryid = retailers[0].RetailerCountry;
                    GetData(retailers[0]);
                    isReturn = true;
                    homeurl = Utility.GetPriceMeHomeUrl(countryid);
                    track = Utility.GetTrackUrl(countryid);
                    if (countryid == 51 || countryid == 56 || countryid == 55)
                        islongprice = true;
                }
            }

            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST");
        }

        private void GetData(Retailer retailer)
        {
            datas = BusinessLogic.GetRetailerProducts(retailer, key);
            if (datas.Count > 0)
            {
                rpurl = Utility.GetPriceMeProductUrl(datas[0].RetailerProductName, datas[0].ProductId, countryid);
                if (datas[0].DiffPrice > 0)
                    stringsave = "Save " + Utility.FormatPrice(datas[0].DiffPrice, countryid) + " at " + datas[0].RetailerName;
                else
                    stringsave = "Next best offer " + Utility.FormatPrice(datas[0].RetailerPrice, countryid) + " at " + datas[0].RetailerName;
            }
        }

        protected string GetDelivery(decimal freight)
        {
            string DeliveryInfo = "";

            if (freight > 0)
                DeliveryInfo = Utility.FormatPrice(freight, countryid);
            else if (freight == 0)
                DeliveryInfo = "Free";
            else
                DeliveryInfo = "&nbsp;";

            return DeliveryInfo;
        }
    }
}