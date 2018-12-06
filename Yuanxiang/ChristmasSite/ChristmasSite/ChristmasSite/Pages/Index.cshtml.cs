using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChristmasSite.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using ChristmasSite.Logic;
using ChristmasSite.Data;
using ChristmasSite.Code;

namespace ChristmasSite.Pages
{
    public class IndexModel : PageModel
    {
        static TimeSpan CacheTimeSpan_Static = new TimeSpan(0, 10, 0);

        public int cid;
        public string Description { get; set; }
        public DealsProductsModelData data;

        public int pagesize = 50;
        public int pageindex;
        public string sb;

        public void OnGet()
        {
            if (SiteConfig.IsDispaly)
            {
                cid = Utility.GetIntParameter("cid", this.Request);
                pageindex = Utility.GetIntParameter("pg", this.Request);
                sb = Utility.GetParameter("sb", this.Request);

                if (pageindex == 0)
                    pageindex = 1;

                BindData();
            }
            else
                Description = DBController.GetBlackInfomation();
        }

        private void BindData()
        {
            string key = "cid=" + cid + "&sb=" + sb + "&pg=" + pageindex;

            DealsProductsModelData cmd = MemoryCacheController.Get<DealsProductsModelData>(key);
            if (cmd != null)
            {
                data = cmd;
            }
            else
            {
                DBController.LoadCategory();

                int total = DBController.GetBlackProductsCount(cid);
                List<ProductCatalog> datas = DBController.GetBlackProducts(cid, pageindex, pagesize, sb);

                string pageUrl = SiteConfig.BlackFridayUrl;
                if (!string.IsNullOrEmpty(sb))
                {
                    if (cid > 0)
                        pageUrl += "?cid=" + cid + "&sb=" + sb;
                    else
                        pageUrl += "?sb=" + sb;
                }
                else if (cid > 0)
                    pageUrl += "?cid=" + cid;

                var pagination = PageExtension.CreatePagination(pageUrl, pageindex, pagesize);
                pagination.Init(total);
                string ltPagination = pagination.Render();

                string ltSortByItems = BindOrderBy();

                string categorySelect = BindCategorySelect();

                data = new DealsProductsModelData();
                data.datas = datas;
                data.Pagination = ltPagination;
                data.SortByItems = ltSortByItems;
                data.CategoreSelect = categorySelect;
                data.Description = DBController.GetBlackInfomation();

                MemoryCacheController.Set<DealsProductsModelData>(key, data, CacheTimeSpan_Static);
            }

            Description = data.Description;
        }

        private string BindOrderBy()
        {
            string html = "";
            string rooturl = SiteConfig.BlackFridayUrl;
            if (pageindex > 1)
                rooturl += "?pg=" + pageindex + "&";
            else
                rooturl += "?";

            if (sb == "" || sb == "sale") html += $"<option value=\"{rooturl + "sb=sale"}\" selected=\"selected\" v=\"Sale\">Largest discount</option>";
            else html += $"<option value=\"{rooturl + "sb=sale"}\" v=\"Sale\">Largest discount</option>";
            
            if (sb == "Clicks") html += $"<option value=\"{rooturl + "sb=Clicks"}\" selected=\"selected\" v=\"Clicks\">Most popular</option>";
            else html += $"<option value=\"{rooturl + "sb=Clicks"}\" v=\"Clicks\">Most popular</option>";
            
            if (sb == "BestPrice") html += $"<option value=\"{rooturl + "sb=BestPrice"}\" selected=\"selected\" v=\"BestPrice\">Lowest prices</option>";
            else html += $"<option value=\"{rooturl + "sb=BestPrice"}\" v=\"BestPrice\">Lowest prices</option>";

            return html;
        }

        private string BindCategorySelect()
        {
            string html = string.Empty;
            if (cid > 0)
            {
                string categroyname = DBController.listCates.SingleOrDefault(c => c.CategoryId == cid).CategoryName;
                html = "<a class=\"btn btn-xs btnShowDiff btnClass\" href=\"" + SiteConfig.BlackFridayUrl + "\">" + categroyname
                    + " <span class=\"glyphicon glyphicon-remove\" style=\"color: #f00;right: 0px;position: absolute;\"></span></a>";
            }

            return html;
        }

        public void OnPost(string txtEmail)
        {
            DBController.NewsletterSignup(txtEmail);

            this.ViewData["signup"] = "Thank you for signing up";
            OnGet();
        }   
    }
}
