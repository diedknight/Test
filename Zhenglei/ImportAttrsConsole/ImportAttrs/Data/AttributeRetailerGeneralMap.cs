using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs.Data
{
    public class AttributeRetailerGeneralMap
    {
        public int RetailerId { get; set; } = 0;
        public int CategoryId { get; set; } = 0;
        public bool IsRemoveKeyword { get; set; }
        public string Keyword { get; set; }
    }
}
