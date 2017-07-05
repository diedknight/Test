using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    public class RetailerProductCondition
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _ConditionName;
        public string ConditionName
        {
            get { return _ConditionName; }
            set { _ConditionName = value; }
        }

        private string _ConditionDescription;
        public string ConditionDescription
        {
            get { return _ConditionDescription; }
            set { _ConditionDescription = value; }
        }
    }
}
