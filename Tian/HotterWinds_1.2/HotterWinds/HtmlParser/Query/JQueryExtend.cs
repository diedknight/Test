using Pricealyser.Crawler.HtmlParser.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Query
{
    public static class JQueryExtend
    {
        public static JQuery ToJQuery(this HtmlNode node)
        {
            return new JQuery(node);
        }

        public static JQuery ToJQuery(this HtmlNodeList nodeList)
        {
            return new JQuery(nodeList);
        }

    }
}
