using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile
{
    public class CompileUtil
    {
        public static HtmlNodeList ClearTextNode(HtmlNodeList nodeList)
        {
            if (nodeList == null) return null;

            return nodeList.Filter(item => item.NodeName != "");
        }
    }
}
