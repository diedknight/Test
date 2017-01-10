using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Priceme.Deals.Code
{
    public static class PageExtension
    {
        public static Pagination CreatePagination(this System.Web.UI.Page page, int pageSize = 60)
        {
            return new Pagination(page, pageSize);
        }

        public static int GetQuery(this System.Web.UI.Page page, string key, int defVal)
        {
            if (page.Request.QueryString[key] == null || page.Request.QueryString[key].ToString() == "") return defVal;

            int result = defVal;
            string val = page.Request.QueryString[key].ToString();

            if (!int.TryParse(val, out result)) return defVal;

            return result;
        }

        public static string GetQuery(this System.Web.UI.Page page, string key, string defVal)
        {
            if (page.Request.QueryString[key] == null || page.Request.QueryString[key].ToString() == "") return defVal;

            return page.Request.QueryString[key].ToString();
        }

        public static List<string> GetQuery(this System.Web.UI.Page page, string key, char splitChar)
        {
            if (page.Request.QueryString[key] == null || page.Request.QueryString[key].ToString() == "") return new List<string>();

            string str = page.Request.QueryString[key].ToString();

            return str.Split(splitChar).ToList();
        }

    }
}