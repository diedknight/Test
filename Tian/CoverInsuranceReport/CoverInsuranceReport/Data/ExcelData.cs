using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverInsuranceReport.Data
{
    public class ExcelData
    {
        public int RowIndex { get; set; }
        public int PId { get; set; }
        public string CategoryName { get; set; }
        public string Manufacturer { get; set; }
        public string ProductFamily { get; set; }
        public string Series { get; set; }
        public string Model { get; set; }
        public string ProductName { get; set; }
        public decimal MinPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MedianPrice { get; set; }
        public int NumberOfPrices { get; set; }
        public string ProductImageUrl { get; set; }
        public string Upcoming { get; set; }
        public string CreatedOn { get; set; }
        public string UpdateOn { get; set; }
        public string ForSale { get; set; }

        public decimal _30DaysMinPriceDiff { get; set; }
        public decimal _30DaysMaxPriceDiff { get; set; }
        public decimal _30DaysMedianPriceDiff { get; set; }

        public decimal _180DaysMinPriceDiff { get; set; }
        public decimal _180DaysMaxPriceDiff { get; set; }
        public decimal _180DaysMedianPriceDiff { get; set; }
        public decimal _180DaysAvgPriceDiff { get; set; }

    }
}
