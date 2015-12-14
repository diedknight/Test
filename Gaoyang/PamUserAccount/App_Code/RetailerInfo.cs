using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PamAccountGenerator
{
    public class RetailerInfo
    {
        public string fullCompanyName { get; set; }
        public string telPhoneNumber { get; set; }
        public string companyRegNumber { get; set; }
        public string retailerUrl { get; set; }
        public string storeType { get; set; }
        public string retailerCountry { get; set; }

        public string ppcType { get; set; }
        public string Ppc { get; set; }
        public string dailyBudget { get; set; }
        public string supportDirectPayment { get; set; }
        public string gstNumber { get; set; }
        public string billingCountry { get; set; }
    }
}