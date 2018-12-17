using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Data
{
    public class ResourcesData
    {
        public IFormatProvider CurrentCulture { get; set; }
        public string HomeUrl { get; set; }
        public string PriceSymbol { get; set; }
        public string String_Price { get; set; }
        public string PriceTrend { get; set; }
        public string PriceHistory { get; set; }
        public string NoHistory { get; set; }
        public string GoogleMapUrl { get; set; }
        public string TrackRootUrl { get; set; }
        public string GetDirections { get; set; }
        public DbInfo DbInfo { get; set; }
        public string RetailerMapDes { get; set; }
        public string RetailerMapNo { get; set; }
    }
}
