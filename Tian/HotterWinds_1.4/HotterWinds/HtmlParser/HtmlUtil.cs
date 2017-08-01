using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser
{
    public class HtmlUtil
    {
        public static bool IsCssClassMatch(string className, string compareClassName)
        {
            if (string.IsNullOrWhiteSpace(className) || string.IsNullOrWhiteSpace(compareClassName)) return false;

            var classlist = className.Split(' ').ToList();
            var compareList = compareClassName.Split(' ').ToList();

            bool result = true;

            classlist.ForEach(item =>
            {
                if (string.IsNullOrWhiteSpace(item)) return;

                result = result && compareList.Contains(item);
            });

            return result;
        }
    }
}
