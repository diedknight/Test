using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fetcher;
using Common.Data;

namespace Crawl
{
    public class CrawlResults
    {
        public readonly BaseFetcher Fetcher;
        public readonly List<MobilePlanInfo> MobilePlanInfoList;

        public CrawlResults(BaseFetcher fetcher, List<MobilePlanInfo> mobilePlanInfoList)
        {
            this.Fetcher = fetcher;
            this.MobilePlanInfoList = mobilePlanInfoList;
        }

        private string mobilePlanInfoFilePath;
        public string MobilePlanInfoFilePath
        {
            get { return mobilePlanInfoFilePath; }
            set { mobilePlanInfoFilePath = value; }
        }
    }
}
