using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class PriceDropItem
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
        public string ImageAlt { get; set; }
        public bool HasAlert { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal LastChange { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}