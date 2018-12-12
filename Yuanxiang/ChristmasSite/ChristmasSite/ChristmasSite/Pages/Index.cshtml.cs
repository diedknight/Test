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
using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Data;

namespace ChristmasSite.Pages
{
    public class IndexModel : PageModel
    {
        static TimeSpan CacheTimeSpan_Static = new TimeSpan(0, 10, 0);
        static int maxValue = 10000;

        public DealsProductsModelData data;

        public int type;
        public int pagesize = 50;
        public int pageindex;
        public string pricerange;

        public void OnGet()
        {
            type = Utility.GetIntParameter("tp", this.Request);
            pageindex = Utility.GetIntParameter("pg", this.Request);
            pricerange = Utility.GetParameter("pr", this.Request);

            if (pageindex == 0)
                pageindex = 1;

            BindData();
        }

        private void BindData()
        {
            string key = "tp=" + type + "&pr=" + pricerange + "&pg=" + pageindex;

            DealsProductsModelData cmd = MemoryCacheController.Get<DealsProductsModelData>(key);
            if (cmd != null)
            {
                data = cmd;
            }
            else
            {
                List<int> cidList = new List<int>();
                if(type == 1)
                    cidList.AddRange(SiteConfig.Forher);
                else if(type == 2)
                    cidList.AddRange(SiteConfig.Forhim);
                else if(type == 3)
                    cidList.AddRange(SiteConfig.Forkids);
                else
                {
                    cidList.AddRange(SiteConfig.Forher);
                    cidList.AddRange(SiteConfig.Forhim);
                    cidList.AddRange(SiteConfig.Forkids);
                }

                PriceRange pr = null;
                if (!string.IsNullOrEmpty(pricerange))
                {
                    string[] temps = pricerange.Split('-');
                    double minp = 0, maxp = 0;
                    double.TryParse(temps[0], out minp);
                    double.TryParse(temps[1], out maxp);
                    pr = new PriceRange(minp, maxp);
                }
                else
                    pr = new PriceRange(0, 300);

                PriceRange prinfo = new PriceRange(0, 300);
                ProductSearcher pSearcher = new ProductSearcher("", cidList, null, prinfo, null, "clicks", null, maxValue, 3, false, true, false, true, null, "", null, false);
                NarrowByInfo info = pSearcher.GetCatalogPriceRangeResulte_New(new System.Globalization.CultureInfo("en-nz"), "$", 10, -1);
                info.ProductCountListWithoutP = info.NarrowItemList.Select(ni => ni.ProductCount).ToList();

                ProductSearcher productSearcher = new ProductSearcher("", cidList, null, pr, null, "clicks", null, maxValue, 3, false, true, false, true, null, "", null, false);
                int total = productSearcher.GetProductCount();
                SearchResult result = productSearcher.GetSearchResult(pageindex, pagesize);
                List<DbEntity.ProductCatalog> datas = result.ProductCatalogList;

                string homeurl = SiteConfig.ChristmasUrl;
                if (type > 0)
                {
                    if (!string.IsNullOrEmpty(pricerange))
                        homeurl += "?tp=" + type + "&pr=" + pricerange;
                    else
                        homeurl += "?tp=" + type;
                }
                else
                {
                    if (!string.IsNullOrEmpty(pricerange))
                        homeurl += "?pr=" + pricerange;
                }

                var pagination = PageExtension.CreatePagination(homeurl, pageindex, pagesize);
                pagination.Init(total);
                string ltPagination = pagination.Render();
                
                string categorySelect = BindCategorySelect();

                data = new DealsProductsModelData();
                data.datas = datas;
                data.Nb = info;
                data.Pagination = ltPagination;
                data.CategoreSelect = categorySelect;
                data.PriceRange = pr;
                data.PageIndex = pageindex;
                data.Type = type;

                MemoryCacheController.Set<DealsProductsModelData>(key, data, CacheTimeSpan_Static);
            }
        }

        private string BindCategorySelect()
        {
            string html = string.Empty;
            string clsHer = string.Empty;
            string clsHim = string.Empty;
            string clsKids = string.Empty;
            if (type == 1)
                clsHer = " active";
            else if (type == 2)
                clsHim = " active";
            else if (type == 3)
                clsKids = " active";

            html = "<a class=\"btn btn-xs btnClass" + clsHer + "\" href=\"" + SiteConfig.ChristmasUrl + "?tp=1\">For her</a>";
            html += "<a class=\"btn btn-xs btnClass" + clsHim + "\" href=\"" + SiteConfig.ChristmasUrl + "?tp=2\">For him</a>";
            html += "<a class=\"btn btn-xs btnClass" + clsKids + "\" href=\"" + SiteConfig.ChristmasUrl + "?tp=3\">For kids</a>";

            return html;
        }
    }
}
