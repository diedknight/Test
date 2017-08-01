using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public partial class BuyingGuide
    {
        private int _BGID;

        public int BGID
        {
            get { return _BGID; }
            set { _BGID = value; }
        }
        private string _BGName;

        public string BGName
        {
            get { return _BGName; }
            set { _BGName = value; }
        }
        private string _WebSiteName;

        public string WebSiteName
        {
            get { return _WebSiteName; }
            set { _WebSiteName = value; }
        }
        private string _LinkCreguired;

        public string LinkCreguired
        {
            get { return _LinkCreguired; }
            set { _LinkCreguired = value; }
        }
        private string _ShortDescription;

        public string ShortDescription
        {
            get { return _ShortDescription; }
            set { _ShortDescription = value; }
        }
        private string _FullContent;

        public string FullContent
        {
            get { return _FullContent; }
            set { _FullContent = value; }
        }
        private int _BGTypeID;

        public int BGTypeID
        {
            get { return _BGTypeID; }
            set { _BGTypeID = value; }
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