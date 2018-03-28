using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleShoppingFeed
{
    /// <summary>
    /// <item>
    /// </summary>
    public class GoogleFeedProduct
    {
        /// <summary>
        /// <g:id>产品Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// <g:title>产品名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// <g:description>
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// <g:link>
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// <g:image_link>
        /// </summary>
        public string ImageLink { get; set; }
        /// <summary>
        /// <g:availability>库存
        /// </summary>
        public string Availability { get; set; }
        /// <summary>
        /// <g:condition>new
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// <g:google_product_category>google的分类Id
        /// </summary>
        public int GoogleProductCategory { get; set; }
        /// <summary>
        /// <g:product_type>
        /// 可选属性 
        /// 为自己的商品定义的商品类别
        ///
        /// 示例
        /// Home > Women > Dresses > Maxi Dresses[家居服 > 女 > 连衣裙 > 及地长裙]
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        /// <g:price>554.88 NZD
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// <g:brand>
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// <g:adult>no
        /// </summary>
        public bool Adult { get; set; }
        /// <summary>
        /// <g:shipping>
        /// </summary>
        public decimal Shipping { get; set; }

        public string ToXmlString()
        {
            string xmlFormat = @"<item>
                                <g:id>{0}</g:id><g:title>{1}</g:title>
                                <g:description>{2}</g:description><g:link>{3}</g:link>
                                <g:image_link>{4}</g:image_link><g:availability>{5}</g:availability>
                                <g:condition>{6}</g:condition>
                                <g:product_type>{7}</g:product_type><g:price>{8}</g:price><g:brand>{9}</g:brand>
                                <g:adult>No</g:adult><g:shipping>{10}</g:shipping>
                                </item>";

            string priceInfo = Price.ToString("0.00") + " NZD";
            string shippingInfo = Shipping < 0 ? "" : Shipping.ToString("0.00") + " NZD";

            return string.Format(xmlFormat, Id, SafeString(Title), SafeString(Description), Link, SafeString(ImageLink), Availability, Condition, SafeString(ProductType), priceInfo, SafeString(Brand), shippingInfo);
        }

        public string SafeString(string str)
        {
            return str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
        }
    }
}