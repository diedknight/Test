using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Data
{
    public class RetailerProductItem
    {
        private int _retailerId;

        public int RetailerId
        {
            get { return _retailerId; }
            set { _retailerId = value; }
        }

        private List<RetailerProductNew> _rpList;

        public List<RetailerProductNew> RpList
        {
            get { return _rpList; }
            set { _rpList = value; }
        }
    }
}
