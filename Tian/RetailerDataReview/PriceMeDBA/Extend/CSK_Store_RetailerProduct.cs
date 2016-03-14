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

        public bool IsChanged { get; private set; }

        partial void OnLoaded()
        {
            this.IsChanged = false;
        }

        partial void OnChanged()
        {
            this.IsChanged = true;
        }
    }
}
