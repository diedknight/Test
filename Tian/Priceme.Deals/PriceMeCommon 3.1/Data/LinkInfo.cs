using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMeCommon.Data
{
    [Serializable]
    public class LinkInfo : IComparable<LinkInfo>
    {
        string linkURL;

        public string LinkURL
        {
            get { return linkURL; }
            set { linkURL = value; }
        }
        string linkText;

        public string LinkText
        {
            get { return linkText; }
            set { linkText = value; }
        }
        string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        string listOrder;

        public string ListOrder
        {
            get { return listOrder; }
            set { listOrder = value; }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

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