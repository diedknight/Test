using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMe
{
    public class MostPopularCategory
    {
        private int _rn, _cnt, _categoryID, _rootID;

        public int RootID
        {
            get { return _rootID; }
            set { _rootID = value; }
        }

        public int CategoryID
        {
            get { return _categoryID; }
            set { _categoryID = value; }
        }

        public int Cnt
        {
            get { return _cnt; }
            set { _cnt = value; }
        }

        public int Rn
        {
            get { return _rn; }
            set { _rn = value; }
        }
    }
}