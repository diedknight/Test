using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChristmasSite.Code
{
    public class UrlParam
    {
        private Dictionary<string, string> urlDic = new Dictionary<string, string>();
        private Uri url = null;
        
        public UrlParam(string url)
        {
            this.url = new Uri(url);

            var queryString = HttpUtility.ParseQueryString(this.url.Query);

            string[] keys = queryString.AllKeys;

            for (int i = 0; i < keys.Length; i++)
            {
                if (string.IsNullOrEmpty(keys[i])) continue;

                string value = queryString[keys[i]];

                if (string.IsNullOrWhiteSpace(value)) continue;

                if (urlDic.ContainsKey(keys[i]))
                {
                    urlDic[keys[i]] = value;
                }
                else
                {
                    urlDic.Add(keys[i], value);
                }
            }
        }

        public void SetParam(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            if (urlDic.ContainsKey(key))
            {
                urlDic[key] = value;
            }
            else
            {
                urlDic.Add(key, value);
            }
        }

        public string GetParam(string key)
        {
            if (urlDic.ContainsKey(key)) return urlDic[key];

            return "";
        }

        public int GetParam(string key, int defVal = 0)
        {
            int val = defVal;

            if (!urlDic.ContainsKey(key)) return defVal;
            if (!int.TryParse(urlDic[key], out val)) return defVal;

            return val;
        }


        public void RemoveParam(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            if (urlDic.ContainsKey(key))
            {
                urlDic.Remove(key);
            }
        }

        public Dictionary<string, string> GetParams()
        {
            Dictionary<string, string> tempDic = new Dictionary<string, string>();

            foreach (var pair in urlDic)
            {
                tempDic.Add(pair.Key, pair.Value);
            }

            return tempDic;
        }

        public string GetUrlParams()
        {
            if (urlDic == null || urlDic.Count == 0) return "";

            string url = "";

            foreach (KeyValuePair<string, string> pair in urlDic)
            {
                url += pair.Key + "=" + pair.Value + "&";
            }

            url = url.Substring(0, url.LastIndexOf('&'));

            return "?" + url;
        }

        public string GetUrl()
        {
            return this.url.AbsoluteUri.Split('?')[0] + GetUrlParams();
        }
    }
}
