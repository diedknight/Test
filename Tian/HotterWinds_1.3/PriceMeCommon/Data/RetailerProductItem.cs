using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;

namespace PriceMeCommon.Data
{
    public class RetailerProductItem
    {
        private int _retailerId;

        public int RetailerId
        {
            get { return _retailerId; }
            set { _retailerId = value; }
        }

        private List<CSK_Store_RetailerProductNew> _rpList;

        public List<CSK_Store_RetailerProductNew> RpList
        {
            get { return _rpList; }
            set { _rpList = value; }
        }
    }
}
