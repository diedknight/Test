using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    [Serializable]
    public partial class ProductDescAndAttr
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
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

        private int _avs;
        public int AVS
        {
            get { return _avs; }
            set { _avs = value; }
        }

        private int _cid;
        public int CID
        {
            get { return _cid; }
            set { _cid = value; }
        }

        private int _AttributeTypeID;
        public int AttributeTypeID
        {
            get { return _AttributeTypeID; }
            set { _AttributeTypeID = value; }
        }

        public int AttributeTitleID
        {
            get; set;
        }

        public int productid { get; set; }
    }
}
