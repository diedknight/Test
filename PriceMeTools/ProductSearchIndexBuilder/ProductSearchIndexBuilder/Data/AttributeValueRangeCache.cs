using System;
using System.Collections.Generic;
using System.Text;

namespace ProductSearchIndexBuilder.Data
{
    public class AttributeValueRangeCache
    {
        public int ValueRangeID { get; set; }
        public int CategoryID { get; set; }
        public int AttributeTitleID { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }
}