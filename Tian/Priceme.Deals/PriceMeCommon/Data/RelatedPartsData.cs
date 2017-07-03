using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class RelatedPartsData
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ShopProductId { get; set; }
        public DateTime CreteaOn { get; set; }
    }
}
