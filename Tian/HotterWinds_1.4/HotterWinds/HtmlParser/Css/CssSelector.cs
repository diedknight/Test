using Pricealyser.Crawler.HtmlParser.Css.Compile;
using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css
{
    public class CssSelector
    {
        private HtmlNodeList _nodeList = null;

        public CssSelector(HtmlNodeList nodeList)
        {
            if (nodeList == null) return;

            this._nodeList = nodeList.Filter(item => item.NodeName != "");
        }

        public CssSelector(HtmlNode node)
        {
            if (node == null) return;
            if (node.NodeName == "") return;

            this._nodeList = node.Combine();
        }

        private HtmlNodeList Find(string cssSelector, bool isFilterAction)
        {
            if (this._nodeList == null) return null;
            if (string.IsNullOrWhiteSpace(cssSelector)) return this._nodeList;

            var tokens = LexerBuilder.Create(cssSelector);
            var syntaxs = SyntaxBuilder.Create(tokens);

            HtmlNodeList tempNodeList = null;
            foreach (Tokens<AbsSyntax> item in syntaxs)
            {
                if (item.Count == 0) continue;

                if (tempNodeList == null)
                {
                    tempNodeList = CompileBuilder.Create(this._nodeList, item, isFilterAction);
                }
                else
                {
                    tempNodeList = tempNodeList.Combine(CompileBuilder.Create(this._nodeList, item, isFilterAction));
                }
            }

            return tempNodeList;
        }

        public HtmlNodeList Find(string cssSelector)
        {
            return this.Find(cssSelector, false);
        }

        public HtmlNodeList Filter(string cssSelector)
        {
            return this.Find(cssSelector, true);
        }
    }
}
