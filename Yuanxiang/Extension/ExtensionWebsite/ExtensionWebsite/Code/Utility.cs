using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ExtensionWebsite.Code
{
    public static class Utility
    {
        //不能用[^\w-]+ 因为 \w 包括字母，数字，下划线，汉字等其它非符号
        private static Regex illegalReg1 = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        public static string GetParameter(string sParam)
        {
            if (System.Web.HttpContext.Current.Request.QueryString[sParam] != null)
            {
                return System.Web.HttpContext.Current.Request.QueryString[sParam];
            }
            if (System.Web.HttpContext.Current.Request.Params[sParam] != null)
            {
                return System.Web.HttpContext.Current.Request.Params[sParam];
            }
            else
            {
                return "";
            }
        }

        public static string ProductListPrice(decimal price, int countryid)
        {
            string result = FormatPrice(price, countryid);
            if (result.Contains("."))
            {
                string[] temps = result.Split('.');
                result = temps[0] + ".<span class=\"decimal\">" + temps[1] + "</span>";
            }

            return result;
        }

        public static string FormatPrice(decimal price, int countryid)
        {
            string result = string.Empty;
            if (price < 1 && price > -1)
            {
                if (countryid == 28 || countryid == 45)
                    result = price.ToString("0");
                else
                    result = price.ToString("0.00");
            }
            else
            {
                if (countryid == 28 || countryid == 45)
                    result = price.ToString("#,###");
                else
                    result = price.ToString("#,###.00");
            }

            string spanb = "<span class=\"symbol\">";
            string spane = "</span>";
            if (countryid == 3 || countryid == 1)
                result = spanb + "$" + spane + result;
            else if (countryid == 28)
                result = spanb + "P" + spane + result;
            else if (countryid == 36)
                result = spanb + "S$" + spane + result;
            else if (countryid == 41)
                result = spanb + "HK$" + spane + result;
            else if (countryid == 45)
                result = spanb + "RM" + spane + result;
            else if (countryid == 51)
                result = spanb + "Rp" + spane + result;
            else if (countryid == 55)
                result = spanb + "฿" + spane + result;
            else if (countryid == 56)
                result = spanb + "Đ" + spane + result;

            return result;
        }

        public static string GetSpecialSize(string img, string size)
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
        }

        public static string GetPriceMeHomeUrl(int countryid)
        {
            string stringhttp = string.Empty;
            if (countryid == 1)
                stringhttp = "https://www.priceme.com.au";
            else if (countryid == 3)
                stringhttp = "https://www.priceme.co.nz";
            else if (countryid == 28)
                stringhttp = "https://www.priceme.com.ph";
            else if (countryid == 36)
                stringhttp = "https://www.priceme.com.sg";
            else if (countryid == 41)
                stringhttp = "https://www.priceme.com.hk";
            else if (countryid == 45)
                stringhttp = "https://www.priceme.com.my";
            else if (countryid == 51)
                stringhttp = "https://www.priceme.co.id";
            else if (countryid == 55)
                stringhttp = "https://www.priceme.com";
            else if (countryid == 56)
                stringhttp = "https://www.priceme.com.vn";

            return stringhttp;
        }

        public static string GetPriceMeProductUrl(string pname, int pid, int countryid)
        {
            string stringhttp = GetPriceMeHomeUrl(countryid);
            
            stringhttp = stringhttp + string.Format("/{0}/p-{1}.aspx", FilterInvalidNameChar(pname), pid);

            return stringhttp;
        }

        public static string GetTrackUrl(int countryid)
        {
            string stringhttp = string.Empty;
            if (countryid == 1)
                stringhttp = "https://track.priceme.com.au";
            else if (countryid == 3)
                stringhttp = "https://track.priceme.co.nz";
            else if (countryid == 28)
                stringhttp = "https://track.priceme.com.ph";
            else if (countryid == 36)
                stringhttp = "https://track.priceme.com.sg";
            else if (countryid == 41)
                stringhttp = "https://track.priceme.com.hk";
            else if (countryid == 45)
                stringhttp = "https://track.priceme.com.my";
            else if (countryid == 51)
                stringhttp = "https://track.priceme.co.id";
            else if (countryid == 55)
                stringhttp = "https://track.priceme.com";
            else if (countryid == 56)
                stringhttp = "https://track.priceme.com.vn";

            return stringhttp;
        }

        public static string FilterInvalidNameChar(string name)
        {
            name = illegalReg1.Replace(name, "-");
            name = illegalReg2.Replace(name, "-");
            name = illegalReg3.Replace(name, "");

            return name;
        }
    }
}