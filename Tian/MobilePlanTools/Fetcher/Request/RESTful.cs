using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.Request
{
    public class RESTful
    {
        private CookieCollection _cookieCollection = new CookieCollection();

        public Dictionary<string, string> _header = null;
        public string Method { get; set; }
        public Dictionary<string, string> UrlParams { get; private set; }
        public Dictionary<string, string> FormParams { get; private set; }

        public RESTful()
        {

            this._header = new Dictionary<string, string>();
            this._header.Add("Content-Type", "text/html");
            this._header.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.124 Safari/537.36");

            this.Method = "GET";

            this.UrlParams = new Dictionary<string, string>();
            this.FormParams = new Dictionary<string, string>();
        }

        public void SetHeader(string key, string value)
        {
            if (this._header.ContainsKey(key))
            {
                this._header[key] = value;
            }
            else
            {
                this._header.Add(key, value);
            }
        }

        public string Request(string url)
        {
            PartManage urlManage = new PartManage();
            PartManage formManage = new PartManage();

            UrlParams.ToList().ForEach(item => urlManage.PartList.Add(new TextPart(item.Key, item.Value)));
            FormParams.ToList().ForEach(item => formManage.PartList.Add(new TextPart(item.Key, item.Value)));

            string requestUriString = url + urlManage.Read(false);
            string formValues = formManage.Read(true);
            byte[] formBytes = Encoding.UTF8.GetBytes(formValues);            

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            httpWebRequest.Method = this.Method;
            httpWebRequest.KeepAlive = false;
            httpWebRequest.Timeout = 1000000;
            httpWebRequest.ContentType = this._header["Content-Type"];
            httpWebRequest.UserAgent = this._header["User-Agent"];
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.CookieContainer.Add(this._cookieCollection);
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");

            string text = "";

            if (this.Method != "GET")
            {
                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(formBytes, 0, formBytes.Length);
                }
            }

            text = this.GetHtml(httpWebRequest);

            return text;
        }

        private string GetHtml(HttpWebRequest request)
        {
            string result;
            using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
            {
                this._cookieCollection = httpWebResponse.Cookies;

                MemoryStream memoryStream = new MemoryStream();
                if (httpWebResponse.ContentEncoding != null && httpWebResponse.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                {
                    var stream = new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                    stream.CopyTo(memoryStream, 10240);
                    stream.Close();
                }
                else
                {
                    var stream = httpWebResponse.GetResponseStream();
                    stream.CopyTo(memoryStream, 10240);
                    stream.Close();
                }
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                Match match = Regex.Match(Encoding.Default.GetString(bytes), "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                string text = (match.Groups.Count > 1) ? match.Groups[2].Value.ToLower() : string.Empty;
                Encoding encoding;
                if (text.Length > 2)
                {
                    encoding = Encoding.GetEncoding(text.Trim().Replace("\"", "").Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk"));
                }
                else
                {
                    if (string.IsNullOrEmpty(httpWebResponse.CharacterSet))
                    {
                        encoding = Encoding.UTF8;
                    }
                    else
                    {
                        encoding = Encoding.GetEncoding(httpWebResponse.CharacterSet);
                    }
                }
                result = encoding.GetString(bytes);
            }
            return result;
        }


    }
}
