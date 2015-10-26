using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class SelectorFilterSyntax : FilterSyntax
    {
        public string FilterName { get; private set; }
        public string CssSelector { get; private set; }

        public SelectorFilterSyntax(string filterName, string cssSelector)
        {
            this.FilterName = filterName;
            this.CssSelector = cssSelector;
        }

        public override string ToString()
        {
            return ":" + FilterName + "(" + CssSelector + ")";
        }

    }
}
