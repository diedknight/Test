using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class NotValueFilterSyntax : FilterSyntax
    {
        public string FilterName { get; private set; }

        public NotValueFilterSyntax(string filterName)
        {
            this.FilterName = filterName;
        }

        public override string ToString()
        {
            return ":" + FilterName;
        }
    }
}
