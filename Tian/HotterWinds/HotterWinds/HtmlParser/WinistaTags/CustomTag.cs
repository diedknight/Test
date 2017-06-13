using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Winista.Text.HtmlParser.Tags;

namespace Pricealyser.Crawler.HtmlParser.WinistaTags
{
    public class CustomTag : CompositeTag
    {
        private string[] _ids = null;

        public CustomTag(string tagName)
        {
            this._ids = new string[] { tagName.ToUpper(new CultureInfo("en")) };
        }

        public override string[] Ids
        {
            get
            {
                return this._ids;
            }
        }
    }
}
