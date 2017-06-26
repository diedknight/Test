using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PriceMeCommon.Extend {
    public static class ImageExtend {
        public static string GetSpecialSize(this string img, string size)
        {
            if (string.IsNullOrEmpty(img) || !img.Contains("."))
                return "";
            img = img.Insert(img.LastIndexOf('.'), "_" + size);
            string str = img.Replace("\\", "/");
            if (!str.StartsWith("/"))
            {
                str = "/" + str;
            }

            return str;
            //return Regex.Replace(img, @"([\s\S]*)(\.(jpg|gif|png|jpeg))([\s\S]*)", string.Format("$1_{0}$2$4", size), RegexOptions.IgnoreCase);
        }
    }
}
