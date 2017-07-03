using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class TradeMeSeller
    {
        public int id { get; set; }
        public string MemberId { get; set; }
        public int RetailerId { get; set; }
        public bool Status { get; set; }
        public string MemberName { get; set; }
    }
}
