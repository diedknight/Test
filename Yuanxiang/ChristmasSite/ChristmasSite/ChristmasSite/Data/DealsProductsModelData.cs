using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasSite.Data
{
    public class DealsProductsModelData
    {
        public List<DbEntity.ProductCatalog> datas { get; set; }
        public NarrowByInfo Nb { get; set; }
        public int Type { get; set; }
        public int PageIndex { get; set; }
        public PriceMeCommon.BusinessLogic.PriceRange PriceRange { get; set; }
        public string CategoreSelect { get; set; }
        public string Pagination { get; set; }
        public string Description { get; set; }
    }
}
