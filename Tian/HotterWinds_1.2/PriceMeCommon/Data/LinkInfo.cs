using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMeCommon.Data
{
    public class LinkInfo : IComparable<LinkInfo>
    {
        public string LinkURL { get; set; }

        public string LinkText { get; set; }

        public string Title { get; set; }

        public string Value { get; set; }

        public string ListOrder { get; set; }
        public string ImageUrl { get; set; }

        public int CompareTo(LinkInfo other)
        {
            return this.LinkText.CompareTo(other.LinkText);
        }

        public int CategoryID { get; set; }
        public decimal PPC_MinPrice { get; set; }
        public int PPC_RetailerProductID { get; set; }
        public int PPC_RetailerID { get; set; }
        public string IconUrl { get; set; }
    }
}