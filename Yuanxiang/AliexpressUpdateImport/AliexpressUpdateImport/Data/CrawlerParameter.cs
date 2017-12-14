using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliexpressDBA;

namespace AliexpressImport.Data
{
    public class CrawlerParameter
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string FilePath { get; set; }
        public DateTime CrawlFinishTime { get; set; }
    }
}
