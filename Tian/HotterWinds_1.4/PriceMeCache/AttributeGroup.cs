using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    public class AttributeGroup
    {
        private string _attributeGroupName;

        public string AttributeGroupName
        {
            get { return _attributeGroupName; }
            set { _attributeGroupName = value; }
        }

        private List<AttributeGroupList> _attributeGroupList;

        public List<AttributeGroupList> AttributeGroupList
        {
            get { return _attributeGroupList; }
            set { _attributeGroupList = value; }
        }

        private int _orderID;

        public int OrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }
    }
}
