using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs.Data
{
    public class ImportProductInfo
    {
        public int ProductId { get; set; }
        public int RetailerId { get; set; }
        public int CategoryId { get; set; }
        public Dictionary<string, string> AllAttributesDic { get; set; }
    }
}