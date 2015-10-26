using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Threading;
using Fetcher;
using Amib.Threading;
using Common.Data;

namespace Crawl
{
    public class FetcherCrawler
    {
        private static string FetcherFileRootPath = AppConfig.FetcherFileRootPath;
        private static string CrawlLogFileRootPath = AppConfig.CrawlLogFileRootPath;

        string mobilePlanInfoFileRootPath;

        string crawlLogFilePath;
        string crawlExceptionFilePath;

        FileLogWriter crawlLogWriter;
        FileLogWriter crawlExceptionWriter;

        public event Crawl.CrawlFinishedEventArgs.OnCrawlFinished CrawlFinishedEvent;

        public FetcherCrawler()
        {
            InitFilePath();
        }

        private void InitFilePath()
        {
            InitCrawlFilePath();
            InitLogFilePath();
        }

        private void InitLogFilePath()
        {
            string rootPath;
            if (string.IsNullOrEmpty(CrawlLogFileRootPath))
            {
                rootPath = "\\";
            }
            else
            {
                rootPath = CrawlLogFileRootPath;
            }

            rootPath += DateTime.Now.ToString("yyyyMMdd_HH") + "\\";
            if (!System.IO.Directory.Exists(rootPath))
            {
                System.IO.Directory.CreateDirectory(rootPath);
            }

            crawlLogFilePath = rootPath + "crawl_log.txt";
            crawlExceptionFilePath = rootPath + "crawl_exception.txt";
        }

        private void InitCrawlFilePath()
        {
            string rootPath;
            if (string.IsNullOrEmpty(FetcherFileRootPath))
            {
                rootPath = "\\";
            }
            else
            {
                rootPath = FetcherFileRootPath;
            }

            if (!rootPath.EndsWith("\\"))
            {
                rootPath += "\\";
            }

            rootPath += DateTime.Now.ToString("yyyyMMdd_HH") + "\\";
            mobilePlanInfoFileRootPath = rootPath;

            if (!System.IO.Directory.Exists(mobilePlanInfoFileRootPath))
            {
                System.IO.Directory.CreateDirectory(mobilePlanInfoFileRootPath);
            }
        }

        public void CrawlAll(int threadCount, ThreadPriority threadPriority)
        {
            List<BaseFetcher> fetcherList = BaseFetcher.GetAllFetcher();

            STPStartInfo stpStartInfo = new STPStartInfo();
            stpStartInfo.IdleTimeout = 8 * 60 * 60 * 1000 + 30 * 60 * 1000;
            stpStartInfo.MaxWorkerThreads = threadCount;
            stpStartInfo.MinWorkerThreads = 1;
            stpStartInfo.ThreadPriority = threadPriority;

            SmartThreadPool smartThreadPool = new SmartThreadPool(stpStartInfo);
            List<IWorkItemResult<bool>> itemresultList = new List<IWorkItemResult<bool>>();
            foreach (var fetcher in fetcherList)
            {
                IWorkItemResult<bool> iWorkItemResult = smartThreadPool.QueueWorkItem(new Amib.Threading.Func<BaseFetcher, bool>(AsynCrawl), fetcher);
                itemresultList.Add(iWorkItemResult);
            }
            smartThreadPool.Start();

            IWorkItemResult<bool>[] itemresult = new IWorkItemResult<bool>[itemresultList.Count];
            for (int i = 0; i < itemresultList.Count; i++)
                itemresult[i] = itemresultList[i];

            using (crawlLogWriter = new FileLogWriter(this.crawlLogFilePath, System.IO.FileMode.Append))
            {
                crawlExceptionWriter = new FileLogWriter(this.crawlExceptionFilePath, System.IO.FileMode.Append);
                string headerLine = "------------------------------" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss");
                crawlLogWriter.WriteLine(headerLine);
                crawlExceptionWriter.WriteLine(headerLine);
                crawlLogWriter.Flush();
                crawlExceptionWriter.Flush();
                bool success = SmartThreadPool.WaitAll(itemresult);
            }
        }

        private bool AsynCrawl(BaseFetcher fetcher)
        {
            bool success = true;

            string mobilePlanInfoFilePath = mobilePlanInfoFileRootPath + fetcher.ProviderName.Replace(" ", "_") + ".txt";

            fetcher.OnGenerateLogEvent += new GenerateLog(fetcher_OnGenerateLogEvent);

            List<MobilePlanInfo> mobilePlanInfoList = fetcher.GetMobilePlanInfoList();
            FetcherController.WriteFetcherCrawlUrl(mobilePlanInfoFilePath, fetcher.PlanUrls);
            FetcherController.WriteToFile<MobilePlanInfo>(mobilePlanInfoList, mobilePlanInfoFilePath);
            FetcherController.WriteFetcherCrawlUrl(mobilePlanInfoFilePath, fetcher.PhoneUrls);
            FetcherController.WriteToFile<MobilePlanInfo>(mobilePlanInfoList, mobilePlanInfoFilePath);
            crawlLogWriter.WriteLine("\t\tplans : " + mobilePlanInfoList.Count()+"\r\n--------------------------------------------------");

            if (CrawlFinishedEvent != null)
            {
                CrawlResults crawlResults = new CrawlResults(fetcher, mobilePlanInfoList);
                crawlResults.MobilePlanInfoFilePath = mobilePlanInfoFilePath;

                CrawlFinishedEventArgs crawlFinishedEvent = new Crawl.CrawlFinishedEventArgs(crawlResults);
                CrawlFinishedEvent(fetcher, crawlFinishedEvent);
            }

            return success;
        }

        void fetcher_OnGenerateLogEvent(object sender, LogEventArgs logEvent)
        {
            if (logEvent.LogInfo.LogType == LogType.CrawlLog || logEvent.LogInfo.LogType == LogType.MsgLog)
            {
                crawlLogWriter.WriteLine(logEvent.LogInfo.LogMsg + "\t" + logEvent.LogInfo.OtherInfo);
                crawlLogWriter.Flush();
            }
            else
            {
                crawlExceptionWriter.WriteLine(logEvent.LogInfo.LogMsg + "\t" + logEvent.LogInfo.OtherInfo);
                crawlExceptionWriter.Flush();
            }
        }
    }
}
