using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.ChildFilter
{
    public class NthLastOfTypeFilterSelector : AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (token is ValueFilterSyntax) return ((ValueFilterSyntax)token).FilterName == "nth-last-of-type";
            if (token is SelectorFilterSyntax) return ((SelectorFilterSyntax)token).FilterName == "nth-last-of-type";

            return false;
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            AbsSyntax tempSyntax = tokens.ReadAndMoveNext();
            tokens.Commit();

            if (tempSyntax is ValueFilterSyntax)
            {
                ValueFilterSyntax syntax = tempSyntax as ValueFilterSyntax;

                int value = Convert.ToInt32(syntax.Value) - 1;

                return nodeList.Filter(item =>
                {
                    if (item.ParentNode == null) return false;

                    int index = item.ParentNode.ChildNodes.Filter(tempItem => tempItem.NodeName == item.NodeName).Count - 1 - item.IndexOfType;

                    return index == value;
                });
            }
            else
            {
                SelectorFilterSyntax syntax = tempSyntax as SelectorFilterSyntax;

                if (syntax.CssSelector == "even")
                {
                    return nodeList.Filter(item =>
                    {
                        if (item.ParentNode == null) return false;

                        int index = item.ParentNode.ChildNodes.Filter(tempItem => tempItem.NodeName == item.NodeName).Count - 1 - item.IndexOfType;

                        return (index + 1) % 2 == 0;
                    });
                }

                if (syntax.CssSelector == "odd")
                {
                    return nodeList.Filter(item =>
                    {
                        if (item.ParentNode == null) return false;

                        int index = item.ParentNode.ChildNodes.Filter(tempItem => tempItem.NodeName == item.NodeName).Count - 1 - item.IndexOfType;

                        return (index + 1) % 2 != 0;
                    });
                }
            }

            return null;
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }
    }
}
