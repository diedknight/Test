using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs.Data
{
    public class CompareAttributeInfo
    {
        public int CompareAttributeId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsNumeric { get; set; } = false;
        public string Unit { get; set; }
        public int AttributeTypeID { get; set; }
    }
}