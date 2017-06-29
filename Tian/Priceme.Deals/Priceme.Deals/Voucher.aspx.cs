using Priceme.Deals.Code;
using PriceMe;
using PriceMeCommon.Data;
using PriceMeCommon;
using PriceMeCommon.Deal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Priceme.Deals
{
    public partial class Voucher : System.Web.UI.Page
    {
        protected List<DealsVoucher> voucherList = new List<DealsVoucher>();

        protected List<VoucherCategory.Category> popularCategories = new List<VoucherCategory.Category>();
        protected List<int> cids = new List<int>();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Main)this.Master).Breadcrumb = "Vouchers";

            this.cids = this.GetQuery("cid", ',').Select(item => Convert.ToInt32(item.Trim())).ToList();

            voucherList = DealsVoucher.GetList(this.cids, this.GetQuery("sb", "newest"), this.GetQuery("pg", 1), 50);

            voucherList.ForEach(item => {
                item.VoucherUrl = "/Tracker.aspx?type=1&id=" + item.Id;

                item.RetailerLogo = this.FixImgUrl(item.RetailerLogo);
            });

            //pagination
            var pagination = this.CreatePagination(50);
            pagination.Init(DealsVoucher.GetCount(this.cids));
            ltPagination.Text = pagination.Render();

            this.BindCategoies();
            this.BindOrderBy();
        }

        private void BindCategoies()
        {
            this.popularCategories = VoucherCategory.GetList();          

            string nodes = "";

            this.popularCategories.ForEach(item =>
            {
                string ids = "";
                string css = "btn btn-xs btnShowDiff ";

                cids.ForEach(cid =>
                {
                    if (cid == item.Id) return;

                    if (ids.Length == 0) ids += cid;
                    else ids += "," + cid;
                });

                //if (cids.Contains(item.Id) || item.Id == s_cid)
                if (cids.Contains(item.Id))
                {
                    css += "btnClass";
                }
                else
                {
                    if (ids.Length == 0) ids += item.Id;
                    else ids += "," + item.Id;

                    css += "btnGrays";
                }

                if (ids.Length != 0) ids = "?cid=" + ids;                

                string url = UrlRoute.Encode(this.Request.Url.GetBaseUrl() + "/Voucher.aspx" + ids);

                nodes += $"<a class=\"{css}\" href=\"{url}\">{item.Name}</a>";
            });

            this.ltCategories.Text = nodes;
        }

        private void BindOrderBy()
        {
            UrlParam p = new UrlParam(this.Request);
            string sb = this.GetQuery("sb", "").Trim().ToLower();
            string html = "";

            p.SetParam("sb", "newest");
            if (sb == "" || sb == "newest") html += $"<option value=\"{UrlRoute.Encode(p.GetUrl())}\" selected=\"selected\" v=\"newest\">Newest</option>";
            else html += $"<option value=\"{UrlRoute.Encode(p.GetUrl())}\" v=\"newest\">Newest</option>";

            p.SetParam("sb", "soon");
            if (sb == "soon") html += $"<option value=\"{UrlRoute.Encode(p.GetUrl())}\" selected=\"selected\" v=\"soon\">Expires Soon</option>";
            else html += $"<option value=\"{UrlRoute.Encode(p.GetUrl())}\" v=\"soon\">Expires Soon</option>";

            ltSortByItems.Text = html;
        }


        private string FixImgUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";

            if (url.ToLower().Contains("://s3."))
            {
                url = url.Replace(Resources.Resource.ImageWebsite, "");
                if (url.FirstOrDefault() == '/')
                {
                    url = url.Substring(1);
                }
            }
            else
            {
                url = Resources.Resource.ImageWebsite + url;
            }

            url = url.Replace("http:", "https:");

            var c = new string[1] { "https://" };
            var b = url.Split(c, StringSplitOptions.RemoveEmptyEntries);

            url = "https://" + (b.Length == 1 ? b[0] : b[1]);

            return url;
        }

    }
}