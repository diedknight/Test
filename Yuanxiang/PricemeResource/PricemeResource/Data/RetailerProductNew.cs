using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Data
{
    [Serializable]
    public class RetailerProductNew
    {
        public int RetailerProductId { get; set; }
        public int RetailerId { get; set; }
        public int ProductId { get; set; }
        public string RetailerProductName { get; set; }
        public decimal RetailerPrice { get; set; }
        public string PurchaseURL { get; set; }
        public string Stock { get; set; }
        public string RetailerProductMessage { get; set; }
        public int RetailerProductCondition { get; set; }
        public bool RetailerProductStatus { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Freight { get; set; }
        public decimal CCFeeAmount { get; set; }
        public string IsImageCheck { get; set; }
        public string ProductReference { get; set; }
        public string ProductModel { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string RetailerProductDescription { get; set; }
        public string DefaultImage { get; set; }
        public string LongDescriptionEN { get; set; }
        public string RetailerProductUIC { get; set; }
        public string RetailerProductSKU { get; set; }
        public string Barcode { get; set; }
        public string StockStatus { get; set; }
        public decimal OriginalPrice { get; set; }
        public string ShoppingRetailerProID { get; set; }
        public decimal PriceLocalCurrency { get; set; }
        public int YPReplication { get; set; }
        public int NZReplication { get; set; }
        public int VNReplication { get; set; }
        public int TradeMeSellerId { get; set; }

        public int PPCMemberType { get; set; }
        public bool IsNoLink { get; set; }
        public bool IsFeaturedProduct { get; set; }
        public decimal OrderbyProduct { get; set; }
        public decimal PPCOrderbyProduct { get; set; }

        /// <summary>
        /// 当前价格排名
        /// </summary>
        public int PricePosition { get; set; }
        /// <summary>
        /// 价格排名总数
        /// </summary>
        public int PricePositionCount { get; set; }
        public int Loc { get; set; }
        public decimal TotalPrice
        {
            get
            {
                if (this.Freight > -1 && this.CCFeeAmount > -1)
                    return (decimal)(RetailerPrice + Freight + CCFeeAmount);
                else if (Freight > -1)
                    return (decimal)(RetailerPrice + Freight);
                else if (CCFeeAmount > -1)
                    return (decimal)(RetailerPrice + CCFeeAmount);
                else
                    return RetailerPrice;
            }
        }
    }
}
