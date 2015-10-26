using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class ValueFilterSyntax : FilterSyntax
    {
        public string FilterName { get; private set; }
        public string Value { get; private set; }

        public ValueFilterSyntax(string filterName, string value)
        {
            this.FilterName = filterName;
            this.Value = value;
        }

        public override string ToString()
        {
            return ":" + FilterName + "(" + Value + ")";
        }

    }
}
