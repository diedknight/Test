using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class CommonPPCMember
    {
        public int PPCMemberTypeID { get; set; }
        public int RetailerId { get; set; }
        public string PPCLogo { get; set; }
        public int RetailerCountry { get; set; }
        public bool PPCforInStockOnly { get; set; }
        public decimal PPCIndex { get; set; }
    }
}