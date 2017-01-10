using PriceMe;
using PriceMeCommon;
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
            CategoryController.CategoryOrderByName.ForEach(item =>
            {
                string name = UrlController.FilterInvalidNameChar(item.CategoryName.ToLower());

                if (!categoryCache.ContainsKey(name))
                {
                    categoryCache.Add(name, item.CategoryID);
                }
            });
        }


        public static string Decode(string url)
        {
            return Decode(new Uri(url));
        }

        public static string Decode(Uri uri)
        {
            if (uri.ToString().ToLower().Contains("ajaxpage.aspx")) return uri.AbsolutePath;
            if (uri.ToString().ToLower().Contains("default.aspx")) return uri.AbsolutePath;
            if (uri.AbsolutePath.Replace("/", "") == "") return uri.AbsolutePath;

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
            //var queryString = new UrlParam(uri);
            //int s_cid = queryString.GetParam("s_cid", 0);
            //string path = "/";

            //if (s_cid != 0)
            //{
            //    queryString.RemoveParam("s_cid");
            //    var cate = CategoryController.GetCategoryByCategoryID(s_cid);
            //    if (cate.IsSiteMap == true || cate.IsSiteMapDetail == true)
            //    {
            //        path += UrlController.FilterInvalidNameChar(cate.CategoryName);
            //    }
            //    else
            //    {
            //        path += "cid=" + cate.CategoryID;
            //    }
            //}

            //return uri.GetBaseUrl() + path + queryString.GetUrlParams();

            var queryString = new UrlParam(uri);
            string[] cids = queryString.GetParam("cid").Split(',');
            int cid = 0;
            string path = "/";

            if (cids.Length == 1 && (!string.IsNullOrEmpty(cids[0])))
            {
                cid = Convert.ToInt32(cids[0]);

                var cate = CategoryController.GetCategoryByCategoryID(cid);
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