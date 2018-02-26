using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Reflection;
using Common.Data;
using Common;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Util;
using Winista.Text.HtmlParser.Tags;
using Winista.Text.HtmlParser.Lex;

namespace Fetcher
{
    public abstract class BaseFetcher : IFetcher
    {
        private string providerName;
        public string ProviderName
        {
            get { return providerName; }
        }

        private int providerID;
        public int ProviderID
        {
            get { return providerID; }
        }

        private int priority;
        public int Priority
        {
            get { return priority; }
        }
        public List<string> PlanUrls = new List<string>();
        public List<string> PhoneUrls = new List<string>();

        abstract public List<MobilePlanInfo> GetMobilePlanInfoList();

        public event GenerateLog OnGenerateLogEvent;

        public BaseFetcher(int providerID, string providerName)
            : this(providerID, providerName, 1)
        {

        }

        public BaseFetcher(int providerID, string providerName, int priority)
        {
            this.providerID = providerID;
            this.providerName = providerName;
            this.priority = priority;
        }

        protected void GenerateLog(LogEventArgs logEvent)
        {
            if (OnGenerateLogEvent != null)
            {
                OnGenerateLogEvent(this, logEvent);
            }

            Console.WriteLine(logEvent.LogInfo.ToString());
        }


        protected void GenerateLog(string msg)
        {
            LogEventArgs logEvent = new LogEventArgs(LogType.MsgLog, msg, "");

            if (OnGenerateLogEvent != null)
            {
                OnGenerateLogEvent(this, logEvent);
            }

            Console.WriteLine(logEvent.LogInfo.ToString());
        }

        #region more log

        /// <summary>
        /// write the crawl plans url address
        /// </summary>
        /// <param name="link"></param>
        protected void StarlCrawlPlansLinkLog(string link)
        {
            GenerateLog(string.Format("\t\tCrawl Plans from -- {0}", link));
            if (!string.IsNullOrEmpty(link))
                PlanUrls.Add(link);
        }

        /// <summary>
        /// write the crawl phones url address
        /// </summary>
        /// <param name="link"></param>
        protected void StarlCrawlPhonesLinkLog(string link)
        {
            GenerateLog(string.Format("\t\tCrawl Phones from -- {0}", link));
            if (!string.IsNullOrEmpty(link))
                PhoneUrls.Add(link);
        }

        /// <summary>
        /// write when start to crawl the fethcer
        /// </summary>
        protected void StartCrawlingLog()
        {
            GenerateLog(ProviderName + ":\r\n\t\tstart crawling --- " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        }
        /// <summary>
        /// write when finish crawling the fethcer
        /// </summary>
        protected void FinishCrawlingLog()
        {
            GenerateLog("\t\tfinish crawling  --- " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        }
        #endregion

        protected string GetHttpContent(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.Timeout = 1000000;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string html = streamReader.ReadToEnd();
                    return html;
                }
            }
            catch (Exception ex)
            {
                LogEventArgs logEventArgs = new LogEventArgs(LogType.ExceptionLog, "url : " + url + "\n" + ex.Message, "GetHttpContent");
                GenerateLog(logEventArgs);
            }

            return "";
        }

        protected Parser GetParser(string url)
        {
            string html = GetHttpContent(url);

            if (!string.IsNullOrEmpty(html))
            {
                Lexer lexer = new Lexer(html);

                Parser parser = new Parser(lexer);
                parser.URL = url;

                return parser;
            }

            return null;
        }

        public static List<BaseFetcher> GetAllFetcher()
        {
            List<BaseFetcher> baseFetcherList = new List<BaseFetcher>();

            Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            Type[] allTypes = assembly.GetTypes();

            Type baseType = typeof(BaseFetcher);

            foreach (Type type in allTypes)
            {
                if (type.BaseType != null && type.BaseType == baseType)
                {
                    ConstructorInfo construcetorInfo = type.GetConstructor(new Type[]{});

                    BaseFetcher baseFetcher = construcetorInfo.Invoke(null) as BaseFetcher;

                    if (baseFetcher != null)
                    {
                        baseFetcherList.Add(baseFetcher);
                    }
                }
            }

            return baseFetcherList;
        }

        public static BaseFetcher GetFetcherByName(string fetcherClassName)
        {
            Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            Type[] allTypes = assembly.GetTypes();

            Type baseType = typeof(BaseFetcher);

            foreach (Type type in allTypes)
            {
                if (type.BaseType != null && type.BaseType == baseType && type.FullName.EndsWith("." + fetcherClassName, StringComparison.InvariantCultureIgnoreCase))
                {
                    ConstructorInfo construcetorInfo = type.GetConstructor(new Type[] { });

                    BaseFetcher baseFetcher = construcetorInfo.Invoke(null) as BaseFetcher;

                    return baseFetcher;
                }
            }

            return null;
        }
    }
}