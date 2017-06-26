using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.Schema;

namespace PriceMeDBA {
    public partial class CSK_Store_RetailerProduct : IActiveRecord {
        public int PPCMemberType { get; set; }
        public bool IsNoLink { get; set; }
        public bool IsFeaturedProduct { get; set; }
        public decimal TotalPrice {
            get {
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
        public decimal OrderbyProduct { get; set; }
        public decimal PPCOrderbyProduct { get; set; }
        public int? TradeMeSellerId { get; set; }
        /// <summary>
        /// 当前价格排名
        /// </summary>
        public int PricePosition { get; set; }
        /// <summary>
        /// 价格排名总数
        /// </summary>
        public int PricePositionCount { get; set; }
        /// <summary>
        /// 0为国内，1为国外
        /// </summary>
        public int Loc { get; set; }
    }
}