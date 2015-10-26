using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.ContentFilter
{
    public class ContainsFilterSelector:AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (token is ValueFilterSyntax) return ((ValueFilterSyntax)token).FilterName == "contains";
            if (token is SelectorFilterSyntax) return ((SelectorFilterSyntax)token).FilterName == "contains";

            return false;
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            AbsSyntax tempSyntax = tokens.ReadAndMoveNext();
            tokens.Commit();

            string value = "";

            if (tempSyntax is ValueFilterSyntax) value = ((ValueFilterSyntax)tempSyntax).Value;
            if (tempSyntax is SelectorFilterSyntax) value = ((SelectorFilterSyntax)tempSyntax).CssSelector;

            return nodeList.Filter(item => item.InnerText.Contains(value) || item.InnerText.Contains(this.DropMarks(value)));
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }

        private string DropMarks(string value)
        {
            if (value.Length <= 2) return value;

            if (value[0] == '\'' && value[value.Length - 1] == '\'') return value.Substring(1, value.Length - 2);
            if (value[0] == '\"' && value[value.Length - 1] == '\"') return value.Substring(1, value.Length - 2);

            return value;
        }

    }
}
