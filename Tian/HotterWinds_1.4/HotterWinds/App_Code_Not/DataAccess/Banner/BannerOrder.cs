using System;
using System.Collections.Generic;
using System.Web;

namespace Commerce.Common.Banner
{
    /// <summary>
    ///BannerOrder 的摘要说明
    /// </summary>
    public class BannerOrder
    {
        public int BannerOrderID;
        public int BannerTypeID;
        public int AdvertiserID;
        public string OrderName;
        public DateTime StartDate;
        public DateTime EndDate;
        public bool Unlimited;
        public int NumberDaily;
        public string BannerImageUrl;
        public string BannerClickUrl;
        public int NumberHourly;
        public int CategoryID;
    }
}