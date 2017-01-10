using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public partial class MostPopularCategories
    {
        private int _Id;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        private int _CategoryId;

        public int CategoryId
        {
            get { return _CategoryId; }
            set { _CategoryId = value; }
        }
        private int _ListOrder;

        public int ListOrder
        {
            get { return _ListOrder; }
            set { _ListOrder = value; }
        }
        private string _CreatedBy;

        public string CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        private DateTime _CreatedOn;

        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        private string _ModifiedBy;

        public string ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
        private DateTime _ModifiedOn;

        public DateTime ModifiedOn
        {
            get { return _ModifiedOn; }
            set { _ModifiedOn = value; }
        }
    }
}