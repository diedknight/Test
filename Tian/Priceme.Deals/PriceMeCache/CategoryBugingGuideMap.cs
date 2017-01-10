using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public partial class CategoryBugingGuideMap
    {
        private int _BGMapID;

        public int BGMapID
        {
            get { return _BGMapID; }
            set { _BGMapID = value; }
        }
        private int _BGID;

        public int BGID
        {
            get { return _BGID; }
            set { _BGID = value; }
        }
        private int _Categoryid;

        public int Categoryid
        {
            get { return _Categoryid; }
            set { _Categoryid = value; }
        }
        private DateTime _CreatedOn;

        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        private string _CreatedBy;

        public string CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        private DateTime _ModifiedOn;

        public DateTime ModifiedOn
        {
            get { return _ModifiedOn; }
            set { _ModifiedOn = value; }
        }
        private string _ModifiedBy;

        public string ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
    }
}