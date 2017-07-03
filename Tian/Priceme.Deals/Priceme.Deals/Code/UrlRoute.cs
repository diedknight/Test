//#undef NoDebug
#define NoDebug

using PriceMe;
using PriceMeCommon.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Priceme.Deals.Code
{
    public class UrlRoute
    {
        private static Dictionary<string, int> categoryCache = new Dictionary<string, int>();

        static UrlRoute()
        {
#if (NoDebug)
            CategoryController.GetAllCategoryOrderByNameList(PriceMe.WebConfig.CountryId).ForEach(item =>
            {
                string name = UrlController.FilterInvalidNameChar(item.CategoryName.ToLower());

                if (!categoryCache.ContainsKey(name))
                {
                    categoryCache.Add(name, item.CategoryID);
                }
            });
#endif
        }


        public static string Decode(string url)
        {
            return Decode(new Uri(url));
        }

        public static string Decode(Uri uri)
        {
            string url = uri.ToString();
            if (uri.AbsolutePath.Split('/').Length > 2)
            {
                url = url.Substring(0, url.LastIndexOf('/'));

                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", url);
                HttpContext.Current.Response.End();
            }
            

            if (uri.ToString().ToLower().Contains("ajaxpage.aspx")) return uri.AbsolutePath;
            if (uri.ToString().ToLower().Contains("default.aspx")) return uri.AbsolutePath;
            if (uri.AbsolutePath.Replace("/", "") == "") return uri.AbsolutePath;

            if (uri.ToString().ToLower().Contains("/voucher"))
            {
                if (!uri.ToString().ToLower().Contains(".aspx")) uri = new Uri(uri.ToString().ToLower().Replace("voucher", "voucher.aspx"));                
            }
            if (uri.ToString().ToLower().Contains("voucher.aspx")) return uri.AbsolutePath;

            if (uri.ToString().ToLower().Contains("recommend_voucher"))
            {
                if (!uri.ToString().ToLower().Contains(".aspx")) uri = new Uri(uri.ToString().ToLower().Replace("recommend_voucher", "recommend.aspx"));                
            }
            if (uri.ToString().ToLower().Contains("recommend.aspx")) return uri.AbsolutePath;


            if (uri.ToString().ToLower().Contains("tracker.aspx")) return uri.AbsolutePath;

            string keyword = HttpUtility.UrlDecode(uri.AbsolutePath).Replace("/", "").ToLower();

            string cid = "";
            if (keyword.Contains("cid="))
            {
                cid = keyword.Replace("cid=", "").Trim();
            }
            else
            {
                //var cate = CategoryController.CategoryOrderByName.SingleOrDefault(item => item.CategoryName.ToLower() == keyword);
                //if (cate != null) cid = cate.CategoryID;

                if (categoryCache.ContainsKey(keyword))
                {
                    cid = categoryCache[keyword].ToString();
                }
            }

            string query = HttpUtility.UrlDecode(uri.Query);
            if (string.IsNullOrEmpty(query)) query = "?cid=" + cid;
            else query += "&cid=" + cid;

            return "/default.aspx" + query;
        }

        public static string Encode(string url)
        {
            return Encode(new Uri(url));
        }

        public static string Encode(Uri uri)
        {
            if (uri.ToString().ToLower().Contains("voucher.aspx"))
            {
                uri = new Uri(uri.ToString().ToLower().Replace(".aspx", ""));
                return uri.ToString();
            }

            if (uri.ToString().ToLower().Contains("recommend.aspx"))
            {
                uri = new Uri(uri.ToString().ToLower().Replace("recommend.aspx", "recommend_voucher"));
                return uri.ToString();
            }

            if (uri.ToString().ToLower().Contains("tracker.aspx")) return uri.ToString();

            var queryString = new UrlParam(uri);
            string[] cids = queryString.GetParam("cid").Split(',');
            int cid = 0;
            string path = "/";

            if (cids.Length == 1 && (!string.IsNullOrEmpty(cids[0])))
            {
                cid = Convert.ToInt32(cids[0]);

                var cate = CategoryController.GetCategoryByCategoryID(cid, PriceMe.WebConfig.CountryId);
                if (cate.IsSiteMap == true || cate.IsSiteMapDetail == true)
                {
                    path += UrlController.FilterInvalidNameChar(cate.CategoryName);
                }
                else
                {
                    path += "cid=" + cate.CategoryID;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(queryString.GetParam("cid")))
                {
                    path += "cid=" + queryString.GetParam("cid");
                }
            }

            queryString.RemoveParam("cid");
            return uri.GetBaseUrl() + path + queryString.GetUrlParams();
        }

    }
}