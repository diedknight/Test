using PriceMeCommon;
using PriceMeCommon.Deal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Priceme.Deals.Code;
using PriceMe;
using PriceMeCommon.Data;

namespace Priceme.Deals
{
    public partial class Default : System.Web.UI.Page
    {
        protected List<ProductCatalog> productList = new List<ProductCatalog>();
        protected List<Category> popularCategories = new List<Category>();
        //protected List<PriceMeCache.CategoryCache> allCategories = new List<PriceMeCache.CategoryCache>();

        protected List<int> cids = new List<int>();
        //protected int s_cid = 0;
        //protected string s_cidText = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Main)this.Master).Breadcrumb = "Deals";


            this.cids = this.GetQuery("cid", ',').Select(item => Convert.ToInt32(item.Trim())).ToList();
            //this.s_cid = this.GetQuery("s_cid", 0);

            //if (this.s_cid != 0)
            //{
            //    this.s_cidText = CategoryController.GetCategoryByCategoryID(this.s_cid).CategoryName;

            //    //var list = this.cids.ToList();
            //    //list.Add(this.s_cid);
            //    //this.cids = list.ToArray();
            //}

            //this.allCategories = CategoryController.CategoryOrderByName.Where(item => CategoryController.GetCategoryProductCount(item.CategoryID) > 0 && item.IsAccessories == false).ToList();

            //List<int> tempCIds = new List<int>();
            //tempCIds.AddRange(this.cids);
            //tempCIds.Add(this.s_cid);

            var result = Product.GetProducts(this.GetQuery("pg", 1), 50, this.GetQuery("sb", "Sale"), this.GetRecentCIdsFromCookie(), cids.ToArray());

            this.productList = result.Products;

            Utility.FixProductCatalogList(this.productList, "");

            this.productList.ForEach(item =>
            {
                item.ProductUrl = "http://www.priceme.co.nz" + UrlController.GetProductUrl(item.ProductID, item.ProductName);

                if (!string.IsNullOrEmpty(item.BestPPCLogoPath))
                {
                    item.BestPPCLogoPath = Resources.Resource.ImageWebsite + item.BestPPCLogoPath;
                }

                item.BestPPCLogoPath = item.BestPPCLogoPath.Replace("http:", "https:");
                
                item.DefaultImage = this.FixImgUrl(item.DefaultImage);
            });
            
            //pagination
            var pagination = this.CreatePagination(50);
            pagination.Init(result.Amount);
            ltPagination.Text = pagination.Render();

            this.BindCategoies();
            this.BindOrderBy();
        }

        public string GetRetailerName(int rid)
        {
            var r = PriceMeCommon.RetailerController.GetRetailerByID(rid);
            if (r != null) return r.RetailerName;

            return "";
        }

        private void BindOrderBy()
        {
            UrlParam p = new UrlParam(this.Request);
            string sb = this.GetQuery("sb", "").Trim().ToLower();
            string html = "";

            p.SetParam("sb", "Sale");
            if (sb == "" || sb == "sale") html += $"<option value=\"{UrlRoute.Encode(p.GetUrl())}\" selected=\"selected\" v=\"Sale\">Largest discount</option>";
            else html += $"<option value=\"{UrlRoute.Encode(p.GetUrl())}\" v=\"Sale\">Largest discount</option>";

            p.SetParam("sb", "Clicks");
            if (sb == "clicks") html += $"<option value=\"{UrlRoute.Encode(p.GetUrl())}\" selected=\"selected\" v=\"Clicks\">Most popular</option>";
            else html += $"<option value=\"{UrlRoute.Encode(p.GetUrl())}\" v=\"Clicks\">Most popular</option>";

            p.SetParam("sb", "BestPrice");
            if (sb == "bestprice") html += $"<option value=\"{UrlRoute.Encode(p.GetUrl())}\" selected=\"selected\" v=\"BestPrice\">Lowest prices</option>";
            else html += $"<option value=\"{UrlRoute.Encode(p.GetUrl())}\" v=\"BestPrice\">Lowest prices</option>";

            ltSortByItems.Text = html;
        }

        private void BindCategoies()
        {
            if (Product.ExistProduct(2)) this.popularCategories.Add(new Category() { Id = 2, Name = "Digital Cameras" });
            if (Product.ExistProduct(500)) this.popularCategories.Add(new Category() { Id = 500, Name = "Headphones" });
            if (Product.ExistProduct(7)) this.popularCategories.Add(new Category() { Id = 7, Name = "Laptops" });
            if (Product.ExistProduct(11)) this.popularCategories.Add(new Category() { Id = 11, Name = "Mobile Phones" });
            if (Product.ExistProduct(1287)) this.popularCategories.Add(new Category() { Id = 1287, Name = "Perfumes" });
            if (Product.ExistProduct(3412)) this.popularCategories.Add(new Category() { Id = 3412, Name = "Portable & Mobile Speakers" });
            if (Product.ExistProduct(13)) this.popularCategories.Add(new Category() { Id = 13, Name = "Speakers" });
            if (Product.ExistProduct(2151)) this.popularCategories.Add(new Category() { Id = 2151, Name = "Tablets" });
            if (Product.ExistProduct(1753)) this.popularCategories.Add(new Category() { Id = 1753, Name = "TVs" });
            if (Product.ExistProduct(377)) this.popularCategories.Add(new Category() { Id = 377, Name = "Washing Machines" });

            List<int> popularCIds = new List<int>() { 2, 500, 7, 11, 1287, 3412, 13, 2151, 1753, 377 };

            cids.ForEach(cid => {
                if (popularCIds.Contains(cid)) return;

                var cate = CategoryController.GetCategoryByCategoryID(cid);
                if (cate != null)
                {
                    if (this.popularCategories.Count >= 10)
                        this.popularCategories[this.popularCategories.Count - 1] = new Category() { Id = cate.CategoryID, Name = cate.CategoryName };
                    else
                        this.popularCategories.Add(new Category() { Id = cate.CategoryID, Name = cate.CategoryName });
                }
            });

            //if (s_cid != 0 && (!this.popularCategories.Exists(item => item.Id == s_cid)))
            //{
            //    if (this.popularCategories.Count >= 10)
            //        this.popularCategories[this.popularCategories.Count - 1] = new Category() { Id = s_cid, Name = s_cidText };
            //    else
            //        this.popularCategories.Add(new Category() { Id = s_cid, Name = s_cidText });
            //}

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
                    if (ids.Length == 0)
                        ids += item.Id;
                    else
                        ids += "," + item.Id;
                    css += "btnGrays";
                }

                if (ids.Length != 0) ids = "?cid=" + ids;

                //if (this.s_cid != 0 && item.Id != this.s_cid)
                //{
                //    if (ids.Length == 0)
                //        ids = "?s_cid=" + this.s_cid;
                //    else
                //        ids += "&s_cid=" + this.s_cid;
                //}

                string url = UrlRoute.Encode(this.Request.Url.GetBaseUrl() + "/Default.aspx" + ids);

                nodes += $"<a class=\"{css}\" href=\"{url}\">{item.Name}</a>";
            });

            this.ltCategories.Text = nodes;
        }

        private List<int> GetRecentCIdsFromCookie()
        {
            List<int> list = new List<int>();

            if (this.Request.Cookies["myrecentlyviewCidCollection"] == null) return list;

            string val = this.Request.Cookies["myrecentlyviewCidCollection"].Value;

            if (!string.IsNullOrEmpty(val))
            {
                list = val.Split('|').Select(item => Convert.ToInt32(item)).ToList();
            }

            return list;
        }

        private string FixImgUrl(string url)
        {
            if (url.ToLower().Contains("://s3."))
            {
                url = url.Replace(Resources.Resource.ImageWebsite, "");
                if (url.FirstOrDefault() == '/')
                {
                    url = url.Substring(1);
                }
            }

            url = url.Replace("http:", "https:");

            var c = new string[1] { "https://" };
            var b = url.Split(c, StringSplitOptions.RemoveEmptyEntries);

            url = "https://" + (b.Length == 1 ? b[0] : b[1]);

            return url;
        }

        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }
}