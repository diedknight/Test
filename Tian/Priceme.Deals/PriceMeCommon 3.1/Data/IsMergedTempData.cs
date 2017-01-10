using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MergeProductsSite
{
    public class IsMergedTempData
    {
        public int ProductID;
        public int ToProductID;
        public DateTime CreatedOn;
        public string CreatedBy;

        public override string ToString()
        {
            string format = "ProductID : {0} - ToProductID : {1} - CreatedOn : {2} - CreatedBy : {3}";
            return string.Format(format, ProductID, ToProductID, CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"), CreatedBy);
        }

        public string ToSqlString()
        {
            string format = "select {0}, {1}, '{2}', '{3}'";
            return string.Format(format, ProductID, ToProductID, CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"), CreatedBy);
        }
    }
}