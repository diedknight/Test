using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Contract
{
    public class CrawlerInfoFetcher : ICrawlerInfoFetcher
    {
        public int RetailerId { get; set; }

        public string RetailerName { get; set; }

        public string RetailerUrl { get; set; }
        public string CrawlName { get; set; }

        public string CategoryName { get; set; }

        public string FetcherType { get; set; }

        public DateTime CrawlDateTime { get; set; }

        public string StoreKey { get; set; }
    }
}
