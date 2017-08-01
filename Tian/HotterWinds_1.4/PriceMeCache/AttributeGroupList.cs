using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    public class AttributeGroupList
    {
        private int _attributeId;

        public int AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        private string _attributeName;

        public string AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }

        private int _avs;

        public int Avs
        {
            get { return _avs; }
            set { _avs = value; }
        }

        private string _shortDescription;

        public string ShortDescription
        {
            get { return _shortDescription; }
            set { _shortDescription = value; }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _unit;

        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        private int _t;

        public int T
        {
            get { return _t; }
            set { _t = value; }
        }

        private int _AttributeTypeID;

        public int AttributeTypeID
        {
            get { return _AttributeTypeID; }
            set { _AttributeTypeID = value; }
        }

        public bool IsCategoryAttribute { get; set; }
    }
}
