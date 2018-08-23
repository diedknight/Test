using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCache
{
   public class AttributeCategoryComparison
    {
        public int Aid { get; set; }
        public bool IsHigherBetter { get; set; }
        public string Top10 { get; set; }
        public string Top20 { get; set; }
        public string Top30 { get; set; }
        public string Average { get; set; }
        public string Bottom10 { get; set; }
        public string Bottom20 { get; set; }
        public string Bottom30 { get; set; }
        public DateTime? Createdon {get;set;}
        public DateTime? Modifiedon { get; set; }
        public bool IsCompareAttribute { get; set; }
    }
}
