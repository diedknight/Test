using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.Basic
{
    public class ElementSelector : AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            return token is ElementSyntax;
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            ElementSyntax syntax = tokens.ReadAndMoveNext() as ElementSyntax;
            tokens.Commit();

            return nodeList.Filter(item => item.NodeName == syntax.TagName.ToLower());
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            ElementSyntax syntax = tokens.ReadAndMoveNext() as ElementSyntax;
            tokens.Commit();

            return nodeList.GetElementByTagName(syntax.TagName.ToLower());
        }
    }
}
