using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    [Serializable]
    public class RetailerPaymentCache
    {
        int retailerID;
        public int RetailerID
        {
            get { return retailerID; }
            set { retailerID = value; }
        }
        int paymentID;
        public int PaymentID
        {
            get { return paymentID; }
            set { paymentID = value; }
        }
        string country;
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        string paymentName;
        public string PaymentName
        {
            get { return paymentName; }
            set { paymentName = value; }
        }
        string paymentImage;
        public string PaymentImage
        {
            get { return paymentImage; }
            set { paymentImage = value; }
        }
    }
}
