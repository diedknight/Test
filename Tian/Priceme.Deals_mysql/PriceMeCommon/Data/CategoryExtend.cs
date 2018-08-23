using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class CategoryExtend
    {
        public int CategoryID { get; set; }
        public int CountryID { get; set; }
        public string Synonym { get; set; }
        public string LocalName { get; set; }
    }
}