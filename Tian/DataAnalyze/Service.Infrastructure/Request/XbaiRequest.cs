using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Infrastructure.Request
{
    public class XbaiRequest : IWebRequest
    {
        private Dictionary<string, string> _textDic = null;
        private Dictionary<string, byte[]> _fileDic = null;
        private Uri _uri = null;
        private ContentType _contentType = ContentType.Html;
        private CookieCollection _cookieCollection = new CookieCollection();

        public XbaiRequest(string url)
            : this(new Uri(url))
        { }

        public XbaiRequest(Uri uri)
        {
            this._textDic = new Dictionary<string, string>();
            this._fileDic = new Dictionary<string, byte[]>();
            this._uri = uri;
        }

        public void AddPara(string name, string value)
        {
            if (this.IsExistPara(name)) throw new Exception("name:已存在");

            this._textDic.Add(name, value);
        }

        public void AddPara(string name, byte[] value)
        {
            if (this.IsExistPara(name)) throw new Exception("name:已存在");

            this._fileDic.Add(name, value);
        }

        public bool IsExistPara(string name)
        {
            return this._textDic.ContainsKey(name) || this._fileDic.ContainsKey(name);
        }

        public void RemovePara(string name)
        {
            if (this._textDic.ContainsKey(name)) this._textDic.Remove(name);
            if (this._fileDic.ContainsKey(name)) this._fileDic.Remove(name);
        }

        public void ClearPara()
        {
            this._textDic.Clear();
            this._fileDic.Clear();
        }

        public string GetTextPara(string name)
        {
            return this._textDic.ContainsKey(name) ? this._textDic[name] : "";
        }

        public byte[] GetDataPara(string name)
        {
            return this._fileDic.ContainsKey(name) ? this._fileDic[name] : null;
        }

        public string Upload()
        {
            string result;
            if (this._uri.OriginalString.Trim().Length == 0)
            {
                result = "";
            }
            else
            {
                try
                {
                    PartManage partManage = this.CreatePartManage(false);
                    string text = "";
                    string text2 = "----------------" + Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + "Xbai";
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this._uri);
                    httpWebRequest.Method = "POST";
                    httpWebRequest.AllowWriteStreamBuffering = false;
                    httpWebRequest.Timeout = 300000;
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";
                    httpWebRequest.ContentType = "multipart/form-data; boundary=" + text2;
                    httpWebRequest.ContentLength = partManage.GetByteLength(text2);
                    httpWebRequest.CookieContainer = new CookieContainer();
                    httpWebRequest.CookieContainer.Add(this._cookieCollection);

                    using (Stream requestStream = httpWebRequest.GetRequestStream())
                    {
                        using (StreamWriter streamWriter = new StreamWriter(requestStream))
                        {
                            partManage.Write(streamWriter, text2);
                            text = this.GetHtml(httpWebRequest);
                        }
                    }
                    result = text;
                }
                catch
                {
                    result = "";
                }
            }
            return result;
        }

        public string Post()
        {
            string result;
            if (this._uri.OriginalString.Trim().Length == 0)
            {
                result = "";
            }
            else
            {
                try
                {
                    PartManage partManage = this.CreatePartManage(true);
                    string s = partManage.Read(true);
                    string text = "";
                    byte[] bytes = Encoding.UTF8.GetBytes(s);
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this._uri);
                    httpWebRequest.Method = "POST";
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.ContentLength = (long)bytes.Length;
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";
                    httpWebRequest.CookieContainer = new CookieContainer();
                    httpWebRequest.CookieContainer.Add(this._cookieCollection);

                    using (Stream requestStream = httpWebRequest.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        text = this.GetHtml(httpWebRequest);
                    }
                    result = text;
                }
                catch
                {
                    result = "";
                }
            }
            return result;
        }

        public string Get()
        {
            return this.Get("text/html");
        }

        public string Get(string contentType)
        {
            string result;
            if (this._uri.OriginalString.Trim().Length == 0)
            {
                result = "";
            }
            else
            {
                try
                {
                    PartManage partManage = this.CreatePartManage(true);
                    string requestUriString = this._uri.OriginalString + partManage.Read(false);
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                    httpWebRequest.Method = "GET";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.ContentType = contentType;
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";
                    httpWebRequest.CookieContainer = new CookieContainer();
                    httpWebRequest.CookieContainer.Add(this._cookieCollection);
                    
                    result = this.GetHtml(httpWebRequest);
                }
                catch
                {
                    result = "";
                }
            }
            return result;
        }

        public byte[] GetFile()
        {
            byte[] result;
            if (this._uri.OriginalString.Trim().Length == 0)
            {
                result = null;
            }
            else
            {
                try
                {
                    byte[] array = null;
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this._uri);
                    httpWebRequest.Method = "GET";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";

                    using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (Stream responseStream = httpWebResponse.GetResponseStream())
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                responseStream.CopyTo(memoryStream);
                                array = memoryStream.GetBuffer();
                            }
                        }
                    }
                    result = array;
                }
                catch
                {
                    result = null;
                }
            }
            return result;
        }

        private PartManage CreatePartManage(bool isTextPart = true)
        {
            PartManage partManage = new PartManage();
            if (isTextPart)
            {
                foreach (KeyValuePair<string, string> current in this._textDic)
                {
                    partManage.PartList.Add(new TextPart(current.Key, current.Value));
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> current in this._textDic)
                {
                    partManage.PartList.Add(new NormalPart(current.Key, current.Value));
                }
                foreach (KeyValuePair<string, byte[]> current2 in this._fileDic)
                {
                    partManage.PartList.Add(new FilePart(current2.Key, "Xbai_" + partManage.PartList.Count, current2.Value));
                }
            }
            return partManage;
        }

        private string GetHtml(HttpWebRequest request)
        {
            string result;
            using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
            {
                this._cookieCollection = httpWebResponse.Cookies;

                if (this._contentType == ContentType.Html)
                {
                    if (httpWebResponse.ContentType.ToUpper().IndexOf("TEXT/HTML") == -1)
                    {
                        result = "";
                        return result;
                    }
                }
                MemoryStream memoryStream = new MemoryStream();
                if (httpWebResponse.ContentEncoding != null && httpWebResponse.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                {
                    new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress).CopyTo(memoryStream, 10240);
                }
                else
                {
                    httpWebResponse.GetResponseStream().CopyTo(memoryStream, 10240);
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


        public string GetCookie(string name)
        {
            foreach (Cookie cookie in this._cookieCollection)
            {
                if (cookie.Name == name) return cookie.Value;
            }

            return "";
        }

        public void SetCookie(string name, string value)
        {
            foreach (Cookie cookie in this._cookieCollection)
            {
                if (cookie.Name == name)
                {
                    cookie.Value = value;
                    return;
                }
            }

            this._cookieCollection.Add(new Cookie(name, value));
        }

    }
}
