using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.ChildFilter
{
    public class OnlyChildFilterSelector:AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (!(token is NotValueFilterSyntax)) return false;

            return ((NotValueFilterSyntax)token).FilterName == "only-child";
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            NotValueFilterSyntax syntax = tokens.ReadAndMoveNext() as NotValueFilterSyntax;
            tokens.Commit();

            return nodeList.Filter(item =>
            {
                if (item.ParentNode == null) return false;

                var tempNodeList = CompileUtil.ClearTextNode(item.ParentNode.ChildNodes);
                if (tempNodeList == null) return false;

                return tempNodeList.Count == 1 && item.IndexOfIgnoreTextNode == 0;
            });
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }
    }
}
