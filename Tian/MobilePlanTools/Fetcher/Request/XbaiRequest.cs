//===============================================================================
// Pricealyser Crawler
// 
// Copyright (c) 2012 12RMB Ltd. All Rights Reserved.
//
// Author:          TianBJ  
// Date Created:    2015-03-27  (yyyy-MM-dd)
//
// Description:
// 
//===============================================================================
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
    public class XbaiRequest : IWebRequest
    {
        private Dictionary<string, string> _textDic = null;
        private Dictionary<string, byte[]> _fileDic = null;        
        private CookieCollection _cookieCollection = new CookieCollection();
        private string _referer = "";

        private Uri _uri = null;

        public Uri Uri
        {
            get { return this._uri; }
            set { this._uri = value; }
        }

        public XbaiRequest()
        {
            this._textDic = new Dictionary<string, string>();
            this._fileDic = new Dictionary<string, byte[]>();
        }

        public XbaiRequest(string url)
            : this(new Uri(url))
        { }

        public XbaiRequest(Uri uri)
        {
            this._textDic = new Dictionary<string, string>();
            this._fileDic = new Dictionary<string, byte[]>();
            this._uri = uri;
        }

        public void AddOrSetPara(string name, string value)
        {
            if (this._fileDic.ContainsKey(name)) this._fileDic.Remove(name);

            if (this._textDic.ContainsKey(name))
            {
                this._textDic[name] = value;
            }
            else
            {
                this._textDic.Add(name, value);
            }
        }

        public void AddOrSetPara(string name, byte[] value)
        {
            if (this._textDic.ContainsKey(name)) this._textDic.Remove(name);

            if (this._fileDic.ContainsKey(name))
            {
                this._fileDic[name] = value;
            }
            else
            {
                this._fileDic.Add(name, value);
            }
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
                    httpWebRequest.Referer = this._referer;                    

                    using (Stream requestStream = httpWebRequest.GetRequestStream())
                    {
                        using (StreamWriter streamWriter = new StreamWriter(requestStream))
                        {
                            partManage.Write(streamWriter, text2);
                            text = this.GetHtml(httpWebRequest);
                        }
                    }
                    result = text;
                    this._referer = this.Uri.ToString();
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
                    httpWebRequest.Referer = this._referer;

                    using (Stream requestStream = httpWebRequest.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        text = this.GetHtml(httpWebRequest);
                    }
                    result = text;
                    this._referer = this.Uri.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
                    httpWebRequest.Referer = this._referer;

                    result = this.GetHtml(httpWebRequest);
                    this._referer = this.Uri.ToString();
                }
                catch(Exception ex)
                {
                    result = "";
                    Console.WriteLine(ex.Message);
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
                    httpWebRequest.Referer = this._referer;

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
                    this._referer = this.Uri.ToString();
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
                foreach (Cookie cookie in httpWebResponse.Cookies)
                {
                    if (this._cookieCollection[cookie.Name] == null)
                    {
                        this._cookieCollection.Add(cookie);
                    }
                    else
                    {                        
                        var tempCookie = this._cookieCollection[cookie.Name];
                        tempCookie.Comment = cookie.Comment;
                        tempCookie.CommentUri = cookie.CommentUri;
                        tempCookie.Discard = cookie.Discard;
                        tempCookie.Domain = cookie.Domain;
                        tempCookie.Expired = cookie.Expired;
                        tempCookie.Expires = cookie.Expires;
                        tempCookie.HttpOnly = cookie.HttpOnly;
                        tempCookie.Path = cookie.Path;
                        tempCookie.Port = cookie.Port;
                        tempCookie.Secure = cookie.Secure;
                        
                        tempCookie.Value = cookie.Value;
                        tempCookie.Version = cookie.Version;
                    }
                }               

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
                    if (string.IsNullOrEmpty(httpWebResponse.CharacterSet))
                    {
                        encoding = Encoding.GetEncoding(text.Trim().Replace("\"", "").Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk"));
                    }
                    else
                    {
                        encoding = Encoding.GetEncoding(httpWebResponse.CharacterSet);
                    }

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

            this._cookieCollection.Add(new Cookie(name, value, "/", this.Uri.Host));
        }

    }
}
