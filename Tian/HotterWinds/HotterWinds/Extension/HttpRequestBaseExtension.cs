using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.Extension
{
    public static class HttpRequestBaseExtension
    {
        public static decimal GetQuery(this HttpRequestBase request, string key, decimal defVal)
        {
            if (request.QueryString[key] == null || request.QueryString[key].ToString() == "") return defVal;

            decimal result = defVal;
            string val = request.QueryString[key].ToString();

            if (!decimal.TryParse(val, out result)) return defVal;

            return result;
        }

        public static int GetQuery(this HttpRequestBase request, string key, int defVal)
        {
            if (request.QueryString[key] == null || request.QueryString[key].ToString() == "") return defVal;

            int result = defVal;
            string val = request.QueryString[key].ToString();

            if (!int.TryParse(val, out result)) return defVal;

            return result;
        }

        public static string GetQuery(this HttpRequestBase request, string key, string defVal)
        {
            if (request.QueryString[key] == null || request.QueryString[key].ToString() == "") return defVal;

            return request.QueryString[key].ToString();
        }

        public static List<string> GetQuery(this HttpRequestBase request, string key, char splitChar)
        {
            if (request.QueryString[key] == null || request.QueryString[key].ToString() == "") return new List<string>();

            string str = request.QueryString[key].ToString();

            return str.Split(splitChar).ToList();
        }
    }
}