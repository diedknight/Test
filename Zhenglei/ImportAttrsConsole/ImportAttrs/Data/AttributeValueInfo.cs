using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs.Data
{
    public class AttributeValueInfo
    {
        public int AttributeValueId { get; set; }
        public int TitleId { get; set; }
        public string Value { get; set; }
        public bool PopularAttribute { get; set; } = false;
        public int ListOrder { get; set; }
    }
}
