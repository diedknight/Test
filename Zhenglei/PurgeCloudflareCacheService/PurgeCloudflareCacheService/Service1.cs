using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace PurgeCloudflareCacheService
{
    public partial class Service1 : ServiceBase
    {
        Thread myWorkThread = null;
        CountriesNodeInfo myCountriesNodeInfo = null;
        string myApiEndPoint;
        string myApiKey;
        string myApiEmail;
        int myMaxCount = 0;

        bool isDebug = false;
        bool myWorking = false;
        string mySelectSql;
        string mySelectProductInfoSqlFormat = @"SELECT ProductID,ProductName,PT.CategoryID,CT.CategoryName FROM CSK_Store_ProductNew as PT
                                                left join CSK_Store_Category as CT on PT.CategoryID = CT.CategoryID
                                                where PT.ProductId in ({0})";
        string myUpdateSqlFormat = "Update PurgedProduct set ProductChecked = 1 where ProductId in ({0})";
        string myDeleteSqlFormat = "Delete PurgedProduct where IndexChecked = 1 and ProductChecked = 1";

        string myApiUrlPathFormat = "zones/{0}/purge_cache";

        public Service1()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string sourceName = "PriceMeLog";
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "PurgeCloudflareCacheService");
            }
            this.eventLog1.Source = sourceName;
            this.eventLog1.Log = "PurgeCloudflareCacheService";

            eventLog1.WriteEntry("PurgeCloudflareCacheService In InitData");
            myCountriesNodeInfo = (CountriesNodeInfo)ConfigurationManager.GetSection("countries");
            eventLog1.WriteEntry(myCountriesNodeInfo.ToString());

            myApiEndPoint = ConfigurationManager.AppSettings["ApiEndPoint"];
            myApiKey = ConfigurationManager.AppSettings["ApiKey"];
            myApiEmail = ConfigurationManager.AppSettings["ApiEmail"];
            myMaxCount = int.Parse(ConfigurationManager.AppSettings["MaxCount"]);

            string selectSqlFormat = "SELECT top {0} ProductId FROM PurgedProduct where ProductChecked = 0";
            mySelectSql = string.Format(selectSqlFormat, myMaxCount);
        }

        protected override void OnStart(string[] args)
        {
            if(args != null && args.Length > 0)
            {
                if(args[0].Equals("debug", StringComparison.InvariantCultureIgnoreCase))
                {
                    isDebug = true;
                }
            }

            if (isDebug)
            {
                eventLog1.WriteEntry("PurgeCloudflareCacheService In OnStart(Debug)");
            }
            else
            {
                eventLog1.WriteEntry("PurgeCloudflareCacheService In OnStart");
            }

            myWorkThread = new Thread(new ThreadStart(DoWork));

            myWorking = true;

            myWorkThread.Start();
        }

        public void DoWork()
        {
            while (myWorking)
            {
                foreach (int key in myCountriesNodeInfo.CountryInfoListDic.Keys)
                {
                    CountryInfo ci = myCountriesNodeInfo.CountryInfoListDic[key];
                    try
                    {
                        PurgeCaches(ci);
                    }
                    catch (Exception ex)
                    {
                        eventLog1.WriteEntry(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

                Thread.Sleep(myCountriesNodeInfo.Interval);
            }
        }

        private void PurgeCaches(CountryInfo ci)
        {
            using (var sqlConn = DBController.CreateDBConnection(ci.MyDbInfo))
            {
                sqlConn.Open();
                List<int> pidList = new List<int>();
                using (var sqlCmd1 = DBController.CreateDbCommand(mySelectSql, sqlConn))
                {
                    using (var sqlDr = sqlCmd1.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            pidList.Add(sqlDr.GetInt32(0));
                        }
                    }
                }

                if (pidList.Count > 0)
                {
                    string idString = string.Join(",", pidList);

                    List<ProductInfo> productInfoList = new List<ProductInfo>();
                    Dictionary<int, string> categoryDic = new Dictionary<int, string>();

                    string selectProductInfoSql = string.Format(mySelectProductInfoSqlFormat, idString);
                    if (isDebug)
                    {
                        eventLog1.WriteEntry("Country " + ci.CountryId + " selectProductInfoSql : " + selectProductInfoSql);
                    }

                    string updateSql = string.Format(myUpdateSqlFormat, idString);
                    using (var updateSqlCmd = DBController.CreateDbCommand(updateSql, sqlConn))
                    {
                        updateSqlCmd.ExecuteNonQuery();
                    }

                    using (var sqlCmd2 = DBController.CreateDbCommand(selectProductInfoSql, sqlConn))
                    {
                        using (var sqlDr = sqlCmd2.ExecuteReader())
                        {
                            while (sqlDr.Read())
                            {
                                ProductInfo productInfo = new ProductInfo();
                                productInfo.ProductId = sqlDr.GetInt32(0);
                                productInfo.ProductName = sqlDr.GetString(1);
                                productInfoList.Add(productInfo);

                                int categoryId = sqlDr.GetInt32(2);
                                string categoryName = sqlDr.GetString(3);
                                if (!categoryDic.ContainsKey(categoryId))
                                {
                                    categoryDic.Add(categoryId, categoryName);
                                }
                            }
                        }
                    }

                    if (productInfoList.Count > 0)
                    {
                        List<string> urls = GetPurgeUrls(productInfoList, ci);
                        PurgeCloudflareCaches(urls, ci);

                        urls = GetPurgeUrls(categoryDic, ci);
                        PurgeCloudflareCaches(urls, ci);
                    }
                    if (isDebug)
                    {
                        eventLog1.WriteEntry("Country " + ci.CountryId + " productInfoList count is 0");
                    }

                    string deleteSql = string.Format(myDeleteSqlFormat, idString);
                    if (isDebug)
                    {
                        eventLog1.WriteEntry("Country " + ci.CountryId + " deleteSql : " + deleteSql);
                    }
                    using (var sqlCmd = DBController.CreateDbCommand(deleteSql, sqlConn))
                    {
                        sqlCmd.ExecuteNonQuery();
                    }

                    eventLog1.WriteEntry("Country " + ci.CountryId + " deleted products : " + idString);
                }
            }
        }

        private List<string> GetPurgeUrls(Dictionary<int, string> categoryDic, CountryInfo ci)
        {
            List<string> urls = new List<string>();

            foreach (int key in categoryDic.Keys)
            {
                string partUrl = UrlController.GetCatalogUrl(categoryDic[key], key);
                string url = ci.WebSite + partUrl;

                urls.Add(url);

                if (!string.IsNullOrEmpty(ci.AMPSite))
                {
                    url = ci.AMPSite + partUrl;
                    urls.Add(url);
                }
            }

            return urls;
        }

        private void PurgeCloudflareCaches(List<string> urls, CountryInfo ci)
        {
            string apiUrl = myApiEndPoint + string.Format(myApiUrlPathFormat, ci.ZoneId);

            HttpWebRequest httpRequest = HttpWebRequest.CreateDefault(new Uri(apiUrl)) as HttpWebRequest;
            httpRequest.Headers.Add("X-Auth-Email", myApiEmail);
            httpRequest.Headers.Add("X-Auth-Key", myApiKey);
            httpRequest.ContentType = "application/json";
            httpRequest.Method = "DELETE";

            string data = "{ \"files\":[\"" + string.Join("\",\"", urls) + "\"] }";
            if (isDebug)
            {
                eventLog1.WriteEntry("Country " + ci.CountryId + " data urls : " + data);
            }
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            httpRequest.ContentLength = byteData.LongLength;
            using (Stream postStream = httpRequest.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            using (HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string rs = reader.ReadToEnd();
                eventLog1.WriteEntry("Country " + ci.CountryId + " PurgeCloudflareCaches : " + rs);
            }
        }

        private List<string> GetPurgeUrls(List<ProductInfo> productInfoList, CountryInfo ci)
        {
            List<string> urls = new List<string>();

            foreach(ProductInfo pi in productInfoList)
            {
                string partUrl = UrlController.GetProductUrl(pi.ProductId, pi.ProductName, ci.UrlSeo);
                string url = ci.WebSite + partUrl;

                urls.Add(url);

                if(!string.IsNullOrEmpty(ci.AMPSite))
                {
                    url = ci.AMPSite + partUrl;
                    urls.Add(url);
                }
            }

            return urls;
        }

        protected override void OnStop()
        {
            myWorking = false;
        }
    }
}