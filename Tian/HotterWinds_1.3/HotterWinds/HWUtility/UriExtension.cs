using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.HWUtility
{
    public static class UriExtension
    {
        public static string GetBaseUrl(this Uri uri)
        {
            string port = uri.Port == 80 ? "" : ":" + uri.Port.ToString();

            return uri.Scheme + "://" + uri.Host + port;
        }
    }
}