using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliExpressFetcher
{
    public class ProductInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string PriceCurrency { get; set; }
        public decimal OldPrice { get; set; }
        public string OldPriceCurrency { get; set; }
        public string ProductPriceUnit { get; set; }
        public string BulkPriceStr { get; set; }
        public string SKU { get; set; }
        public string Vender { get; set; }
        public string FullDescription { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Unit { get; set; }
        public string UnitType { get; set; }
        public float Weight { get; set; }
        public List<string> Images { get; set; }
        public int StockNum { get; set; }
        public decimal Shipping { get; set; }
        public List<ShippingInfo> ShippingInfos { get; set; }
        public Dictionary<string, string> Attributes { get; set; }

        public string ToXmlString()
        {
            string imagesXml = "";
            foreach (string imgUrl in Images)
            {
                imagesXml += "<Image>" + imgUrl.ToXmlSafeString() + "</Image>";
            }

            string shippingXml = "";
            foreach (var si in ShippingInfos)
            {
                shippingXml += si.ToXmlString();
            }

            string attrXml = "";
            foreach (string attr in Attributes.Keys)
            {
                attrXml += "<Attr><Title>" + attr.ToXmlSafeString() + "</Title><Value>" + Attributes[attr].ToXmlSafeString() + "</Value></Attr>";
            }
            string xmlFormat = "<Product><Name>{0}</Name><Url>{1}</Url><Category>{2}</Category><Price>{3}</Price><PriceCurrency>{4}</PriceCurrency><OldPrice>{5}</OldPrice><OldPriceCurrency>{6}</OldPriceCurrency><ProductPriceUnit>{7}</ProductPriceUnit><SKU>{8}</SKU><Vender>{9}</Vender><BulkPriceStr>{10}</BulkPriceStr><FullDescription>{11}</FullDescription><Stock>{12}</Stock><Images>{13}</Images><ShippingInfos>{14}</ShippingInfos><Attributes>{15}</Attributes></Product>";

            string xml = string.Format(xmlFormat, Name.ToXmlSafeString(), Url.ToXmlSafeString(), Category.ToXmlSafeString(), Price.ToString("0.00"), PriceCurrency.ToXmlSafeString(), OldPrice.ToString("0.00"), OldPriceCurrency.ToXmlSafeString(), ProductPriceUnit.ToXmlSafeString(), SKU.ToXmlSafeString(), Vender.ToXmlSafeString(), BulkPriceStr.ToXmlSafeString(), FullDescription.ToXmlSafeString(), StockNum, imagesXml, shippingXml, attrXml);
            return xml;
        }

        //Name  Sku	FullDescription	Vendor	DeliveryDate PriceCurrency	Price OldPriceCurrency	OldPrice	ProductCost	Categories	Picture1	Picture2	Picture3
        public string ToCsvString()
        {
            string csvFormat = "\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"";
            string csv = string.Format(csvFormat, Name.ToCsvSafeString(), SKU.ToCsvSafeString(), Price.ToString("0.00"), Category.ToCsvSafeString(), Url.ToCsvSafeString(), Shipping.ToString("0.00"));
            
            string deliveryDate = "";
            if (ShippingInfos.Count > 0)
            {
                deliveryDate = ShippingInfos[0].ToDeliveryTimeString();
            }

            string lengthStr;
            string widthStr;
            string heightStr;

            if(Unit.Equals("cm", StringComparison.InvariantCultureIgnoreCase))
            {
                lengthStr = (Length / 100).ToString("0.00");
                widthStr = (Width / 100).ToString("0.00");
                heightStr = (Height / 100).ToString("0.00");
            }
            else
            {
                lengthStr = Length.ToString("0.00");
                widthStr = Width.ToString("0.00");
                heightStr = Height.ToString("0.00");
            }

            return csv;
        }

        public static string ToCsvHeaderString()
        {
            string headerString = "Name,Sku,Price,Category,AdminComment,Shipping";

            return headerString;
        }
    }
}