using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndexBuildCommon.Data
{
    public class FixPriceInfo
    {
        public int RetailerProductID;
        public int RetailerID;
        public double Price;

        public override string ToString()
        {
            return "RetailerID : " + RetailerID + "\t RetailerProductID : " + RetailerProductID + "\tPrice : " + Price.ToString("0.00");
        }
    }
}
