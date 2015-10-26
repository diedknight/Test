using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawl
{
    public class CrawlFinishedEventArgs : EventArgs
    {
        public delegate void OnCrawlFinished(object sender, CrawlFinishedEventArgs crawlFinishedEvent);

        public readonly CrawlResults EventCrawlResults;

        public CrawlFinishedEventArgs(CrawlResults crawlResults)
        {
            EventCrawlResults = crawlResults;
        }
    }
}
