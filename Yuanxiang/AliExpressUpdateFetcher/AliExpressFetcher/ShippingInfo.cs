using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliExpressFetcher
{
    public class ShippingInfo
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PriceCurrency { get; set; }
        public string EstimatedDeliveryTime { get; set; }

        public string ToXmlString()
        {
            string xmlFormat = "<Shipping><Name>{0}</Name><Price>{1}</Price><PriceCurrency>{2}</PriceCurrency><EstimatedDeliveryTime>{3}</EstimatedDeliveryTime></Shipping>";
            string xml = string.Format(xmlFormat, Name.ToXmlSafeString(), Price.ToString("0.00"), PriceCurrency, EstimatedDeliveryTime.ToXmlSafeString());
            return xml;
        }

        public string ToDeliveryTimeString()
        {
            return EstimatedDeliveryTime.ToCsvSafeString();
        }
    }
}