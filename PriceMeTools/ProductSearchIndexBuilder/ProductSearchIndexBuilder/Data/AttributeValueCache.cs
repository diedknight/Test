using System;
using System.Collections.Generic;
using System.Text;

namespace ProductSearchIndexBuilder.Data
{
    public class AttributeValueCache
    {
        public int AttributeValueID { get; set; }
        public int AttributeTitleID { get; set; }
        public string Value { get; set; }
        public int ListOrder { get; set; }
        public bool PopularAttribute { get; set; }
        public float NumericValue { get; set; }
    }
}