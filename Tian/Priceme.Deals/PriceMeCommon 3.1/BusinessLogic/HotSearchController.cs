using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using System.Data;
using SubSonic.Schema;
using SubSonic.Extensions;
using PriceMeCommon.Data;
using PriceMeCommon.Extend;

namespace PriceMeCommon.BusinessLogic {
    public class HotSearchController {
        public static List<HotSearchResult> Load(int count) {
            #region
            //PriceMeDBDB db = new PriceMeDBDB();
            //StoredProcedure sp = db.GetHotSearch2();
            //sp.Command.AddParameter("@COUNT", count);
            //IDataReader idr = sp.ExecuteReader();

            //List<string> columns = new List<string>();
            //columns.Add("KeyWords");
            //columns.Add("SearchCount");

            //while (idr.Read()) { 
            //    HotSearch hs = new HotSearch();
            //    idr.Load<HotSearch>(hs,columns);
            //}
            #endregion

            PriceMeDBDB db = PriceMeStatic.PriceMeDB;
            var gs = from s in db.CSK_Store_UserSearch_Googles.Take(count)
                     orderby s.CNT descending
                     select s;

            List<HotSearchResult> tmp = new List<HotSearchResult>();
            foreach (var usg in gs)
            {
                HotSearchResult hotSearchResult = new HotSearchResult();
                hotSearchResult.Keywords = usg.KeyWords;
                hotSearchResult.Count = usg.CNT;
                tmp.Add(hotSearchResult);
            }

            var ts = from s in tmp select s.Count;

            if (ts.Count() > 0) {
                int max = ts.Max();
                int min = ts.Min();
                int step = (max - min) / 5;

                tmp.ForEach(t => t.Level = (int)Math.Round((double)t.Count / step));
            }

            return tmp;
        }
    }

    public class HotSearchResult : IComparable<HotSearchResult> {
        string keywords;
        int count;
        int categoryID = 0;
        //string categoryName = "";
        int level;

        public int Count {
            get { return count; }
            set { count = value; }
        }

        public string Keywords {
            get { return keywords; }
            set { keywords = value.Trim(); }
        }

        public int CategoryID {
            get { return categoryID; }
            set { categoryID = value; }
        }

        //public string CategoryName {
        //    get { return categoryName; }
        //    set { categoryName = value; }
        //}

        public int Level {
            get { return level; }
            set { level = value; }
        }

        #region IComparable<HotSearch1> 成员

        public int CompareTo(HotSearchResult other) {
            return this.keywords.CompareTo(other.Keywords);
        }

        #endregion
    }
}
