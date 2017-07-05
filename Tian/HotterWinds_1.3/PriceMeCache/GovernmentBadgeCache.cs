using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    [Serializable]
    public class GovernmentBadgeCache
    {
        public int ID { get; set; }

        public int RetailerID { get; set; }

        public string CompanyName { get; set; }

        public int CompanyID { get; set; }
    }
}
