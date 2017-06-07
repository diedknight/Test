using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtensionWebsite.Data
{
    public class Retailer
    {
        public int RetailerId { get; set; }
        public string RetailerUrl { get; set; }
        public string RetailerLog { get; set; }
        public string RetailerName { get; set; }
        public bool IsNolink { get; set; }
        public int RetailerCountry { get; set; }
    }
}