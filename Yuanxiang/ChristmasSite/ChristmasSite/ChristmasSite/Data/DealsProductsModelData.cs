using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasSite.Data
{
    public class DealsProductsModelData
    {
        public List<ProductCatalog> datas { get; set; }
        public string CategoreSelect { get; set; }
        public string SortByItems { get; set; }
        public string Pagination { get; set; }
        public string Description { get; set; }
    }
}
