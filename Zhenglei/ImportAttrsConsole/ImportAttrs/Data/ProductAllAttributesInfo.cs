using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs.Data
{
    public class ProductAllAttributesInfo
    {
        public int ProductId { get; set; }
        public List<ProductAttributeInfo> ProductAttributeList { get; set; }
        public List<ProductCompareAttributeInfo> ProductCompareAttributeList { get; set; }
    }
}