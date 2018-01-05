using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlertFCM
{
    //1:{"ProductId":894634543,"ProductName":"Canon Test 5D","ProductBestPrice":700.60,"AlertMsg":"Canon Test 5D dropped to $87.60","ProductImage":"https://images.pricemestatic.com/Images/RetailerProductImages/StRetailer461/0042765488_ms.jpg","CountryId":3}
    public class PriceAlertInfo
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductBestPrice { get; set; }
        public string ProductImage { get; set; }
        public int CountryId { get; set; }
        public string AlertMsg { get; set; }
    }
}
