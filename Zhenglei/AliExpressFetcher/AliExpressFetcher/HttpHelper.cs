using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AliExpressFetcher
{
    public static class HttpHelper
    {
        public static string GetHttpContent(string url)
        {
            return GetHttpContent(url, null);
        }

        public static string GetHttpContent(string url, CookieCollection cookies)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.96 Safari/537.36";
                request.KeepAlive = true;
                request.Accept = "text/html";

                request.CookieContainer = new CookieContainer();
                if (cookies != null && cookies.Count > 0)
                {
                    request.CookieContainer.Add(cookies);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string httpString = streamReader.ReadToEnd();

                    if (response.Cookies != null && response.Cookies.Count > 0)
                    {
                        cookies.Add(response.Cookies);
                    }

                    return httpString;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "";
        }

        public static string PostHttpContent(string url, string postContent, CookieCollection cookies)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

                request.CookieContainer = new CookieContainer();
                if (cookies != null && cookies.Count > 0)
                {
                    request.CookieContainer.Add(cookies);
                }

                Encoding encode = Encoding.GetEncoding("utf-8");
                byte[] bytes = encode.GetBytes(postContent);
                request.ContentLength = bytes.Length;
                request.GetRequestStream().Write(bytes, 0, bytes.Length);


                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string httpString = streamReader.ReadToEnd();

                    if (response.Cookies != null && response.Cookies.Count > 0)
                    {
                        cookies.Add(response.Cookies);
                    }

                    return httpString;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
    }
}
