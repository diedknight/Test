using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace PriceMeCommon.Extend {
    public static class StringExtend {
        public static string ParseString(this string s, string key, bool ignoreCase) {
            Dictionary<string, string> kvs = s.ParseString(ignoreCase);
            if (kvs.ContainsKey(key)) {
                return kvs[key];
            }
            return "";
        }

        public static Dictionary<string, string> ParseString(this string s, bool ignoreCase) {

            if (s.IndexOf('?') != -1) {
                s = s.Remove(0, s.IndexOf('?'));
            }

            Dictionary<string, string> kvs = new Dictionary<string, string>();
            Regex reg = new Regex(@"[\?&]?(?<key>[^=]+)=(?<value>[^\&]*)", RegexOptions.Compiled | RegexOptions.Multiline);
            MatchCollection ms = reg.Matches(s);
            string key;
            foreach (Match ma in ms) {
                key = ignoreCase ? ma.Groups["key"].Value.ToLower() : ma.Groups["key"].Value;
                if (kvs.ContainsKey(key)) {
                    kvs[key] += "," + ma.Groups["value"].Value;
                } else {
                    kvs[key] = ma.Groups["value"].Value;
                }
            }

            return kvs;
        }

        public static string SetUrlKeyValue(this string url, string key, string value, Encoding encode) {
            if (url.ParseString(key, true).Trim() != "") {
                Regex reg = new Regex(@"([\?\&])(" + key + @"=)([^\&]*)(\&?)");

                return reg.Replace(url, "$1$2" + HttpUtility.UrlEncode(value, encode) + "$4");
            } else {
                return url + (url.IndexOf('?') > -1 ? "&" : "?") + key + "=" + HttpUtility.UrlEncode(value, encode);
            }
        }

        public static string FixUrl(this string url) {
            return url.FixUrl("");
        }

        public static string FixUrl(this string url, string defaultPrefix) {
            string tmp = url.Trim();
            if (!Regex.Match(tmp, "^(http|https):").Success) {
                tmp = string.Format("{0}/{1}", defaultPrefix, tmp);
            }
            tmp = Regex.Replace(tmp, @"(?<!(http|https):)[\\/]+", "/").Trim();
            return tmp;
        }

        public static int ToInt( this string str, int defaultValue ) {
            int v = defaultValue;
            int.TryParse(str, out v);
            return v;
        }

        public static decimal ToDecimal(this string str, decimal defaultValue) {
            decimal v = defaultValue;
            decimal.TryParse(str, out v);
            return v;
        }

        public static string CutOut( this string str, int leng ) {
            return str.CutOut(leng, "...");
        }

        public static string CutOut( this string str, int leng, string suffix ) {
            if (string.IsNullOrEmpty(str) || leng <= 0)
                return str;

            if (str.Length > leng)
                return str.Substring(0, leng) + suffix;
            else
                return str;
        }

        public static string SafeString( this string txt ) {
            if (string.IsNullOrEmpty(txt))
                return "";
            txt = txt.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace(@"\", @"\\").Replace("\"", "&quote;").Replace("'", @"\'");
            return txt;            
        }
    }
}
