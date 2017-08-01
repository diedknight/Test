using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    [Serializable]
    public class RetailerOperatingHours
    {
        private int _OperatingId;
        public int OperatingId
        {
            get { return _OperatingId; }
            set { _OperatingId = value; }
        }

        private int _RetailerId;
        public int RetailerId
        {
            get { return _RetailerId; }
            set { _RetailerId = value; }
        }

        private int _Days;
        public int Days
        {
            get { return _Days; }
            set { _Days = value; }
        }

        private string _OpenTime;
        public string OpenTime
        {
            get { return _OpenTime; }
            set { _OpenTime = value; }
        }

        private string _CloseTime;
        public string CloseTime
        {
            get { return _CloseTime; }
            set { _CloseTime = value; }
        }

    }
}
