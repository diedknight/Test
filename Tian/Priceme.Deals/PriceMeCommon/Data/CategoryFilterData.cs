using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class CategoryFilterData
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string FilterName { get; set; }
        public string FilterUrl { get; set; }
        public int CountryId { get; set; }
    }
}
