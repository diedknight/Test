using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.Extension
{
    public static class StringExtension
    {
        public static string SQLWrapPageStr(this string str,int pageNum, int pageSize)
        {
            string sql1 = "select top " + pageSize + " * from (";
            sql1 += str;
            sql1 += " ) as wrap where rownum>" + (pageNum - 1) * pageSize;

            return sql1;
        }
    }
}