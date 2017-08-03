using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Contract
{
    public interface ICrawlerInfoFetcher : IContract
    {
        int RetailerId { get; set; }
        string RetailerName { get; set; }
        string RetailerUrl { get; set; }
        string CrawlName { get; set; }
        string CategoryName { get; set; }
        string FetcherType { get; set; }
        DateTime CrawlDateTime { get; set; }
        string StoreKey { get; set; }
    }
}
