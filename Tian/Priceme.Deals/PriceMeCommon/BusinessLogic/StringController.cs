using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PriceMeCommon.BusinessLogic
{
    public class StringController
    {
        //不能用[^\w-]+ 因为 \w 包括字母，数字，下划线，汉字等其它非符号
        private static Regex illegalReg = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        public static string FilterInvalidUrlPathChar(string sourceString)
        {
            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");
            sourceString = illegalReg3.Replace(sourceString, "");
            return sourceString;
        }

        public static string GetPluralWord(string word, int countryid)
        {
            if (!ConfigAppString.ListVersionNoEnglishCountryId.Contains(countryid))
                word += "s";

            return word;
        }
    }
}