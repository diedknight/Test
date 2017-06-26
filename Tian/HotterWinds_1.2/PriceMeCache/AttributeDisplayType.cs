using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCache
{
    public class AttributeDisplayType
    {
        public int ID { get; set; }
        public int AttributeID { get; set; }

        public int TypeID { get; set; }
        public bool IsComparison { get; set; }
        public string DisplayAdjectiveBetter { get; set; }

        public string DisplayAdjectiveWorse { get; set; }
        public bool IsCompareAttribute { get; set; }
    }
}
