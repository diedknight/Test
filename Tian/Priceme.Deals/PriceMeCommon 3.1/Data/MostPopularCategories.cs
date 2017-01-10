using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data {
    public class MostPopularCategory {
        private int _rn, _cnt, _categoryID, _rootID;
        //private string _categoryName = "", _rootName = "";

        //public string RootName {
        //    get { return _rootName; }
        //    set { _rootName = value; }
        //}

        //public string CategoryName {
        //    get { return _categoryName; }
        //    set { _categoryName = value; }
        //}

        public int RootID {
            get { return _rootID; }
            set { _rootID = value; }
        }

        public int CategoryID {
            get { return _categoryID; }
            set { _categoryID = value; }
        }

        public int Cnt {
            get { return _cnt; }
            set { _cnt = value; }
        }

        public int Rn {
            get { return _rn; }
            set { _rn = value; }
        }
    }
}
