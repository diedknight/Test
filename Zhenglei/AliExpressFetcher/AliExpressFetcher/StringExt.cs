using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliExpressFetcher
{
    public static class StringExt
    {
        public static string ToXmlSafeString(this string mStr)
        {
            if (string.IsNullOrEmpty(mStr))
                return string.Empty;

            return mStr.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;").Trim();
        }

        public static string ToCsvSafeString(this string mStr)
        {
            if (string.IsNullOrEmpty(mStr))
                return string.Empty;

            return mStr.Replace("\"", "\"\"").Trim();
        }
    }
}