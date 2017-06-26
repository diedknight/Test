using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public class AttributeTitleCache
    {
        private int _TypeID;

        public int TypeID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }
        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private int _LessThan;

        public int LessThan
        {
            get { return _LessThan; }
            set { _LessThan = value; }
        }
        private int _MoreThan;

        public int MoreThan
        {
            get { return _MoreThan; }
            set { _MoreThan = value; }
        }
        private string _IsNumeric;

        public string IsNumeric
        {
            get { return _IsNumeric; }
            set { _IsNumeric = value; }
        }
        private string _Unit;

        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }
        private string _ShortDescription;

        public string ShortDescription
        {
            get { return _ShortDescription; }
            set { _ShortDescription = value; }
        }

        public int AttributeGroupID
        {
            get; set;
        }

        public int AttributeTypeID
        {
            get; set;
        }
    }
}