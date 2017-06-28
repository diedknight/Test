using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliexpressTool.Data
{
    public class ShopOrderData
    {
        public int Id { get; set; }
        public long AliOrderId { get; set; }
        public int ShippingStatusId { get; set; }
    }
}
