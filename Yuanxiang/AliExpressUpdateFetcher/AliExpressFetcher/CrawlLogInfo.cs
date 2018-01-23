using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliExpressFetcher
{
    public class CrawlLogInfo
    {
        public string FeedPath { get; set; }
        public bool IsAllFinished { get; set; } = false;
        public List<string> FinishedUrls { get; set; } = new List<string>();

        public override string ToString()
        {
            string str = FeedPath + "\r\n" + IsAllFinished + "\r\n" + string.Join("\r\n", FinishedUrls);
            return str;
        }
    }
}