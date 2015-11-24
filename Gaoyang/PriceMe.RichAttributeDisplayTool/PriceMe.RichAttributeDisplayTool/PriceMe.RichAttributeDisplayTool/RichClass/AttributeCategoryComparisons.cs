using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMe.RichAttributeDisplayTool.RichClass
{
   public class AttributeCategoryComparisons:IClass
    {
        public int Aid { get; set; }
        public bool IsHigherBetter { get; set; }
        public bool IsCompareAttribute { get; set; }
        public string Top10 { get; set; }
        public string Top20 { get; set; }
        public string Top30 { get; set; }
        public string Average { get; set; }
        public string Bottom30 { get; set; }
        public string Bottom20 { get; set; }
        public string Bottom10 { get; set; }
        public DateTime? Createon { get; set; }
        public DateTime? Modifiedon { get; set; }
       
    }
}
