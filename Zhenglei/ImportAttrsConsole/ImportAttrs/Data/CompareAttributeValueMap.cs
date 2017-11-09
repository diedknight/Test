using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs.Data
{
    public class CompareAttributeValueMap
    {
        public int CompareAttributeID { get; set; }
        public string Value { get; set; }
        public string Skeywords { get; set; }
        public int CategoryID { get; set; }
        public List<string> SkeywordList { get; set; }
    }
}
