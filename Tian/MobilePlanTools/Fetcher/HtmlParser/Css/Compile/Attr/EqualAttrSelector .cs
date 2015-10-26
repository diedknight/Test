using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.Attr
{
    public class EqualAttrSelector:AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (!(token is AttrSyntax)) return false;

            return ((AttrSyntax)token).Operation == "=";
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            AttrSyntax syntax = tokens.ReadAndMoveNext() as AttrSyntax;
            tokens.Commit();

            return nodeList.Filter(item => item.GetAttr(syntax.AttrName) == syntax.Value);
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }

    }
}
