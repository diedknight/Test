using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.Basic
{
    public class ClassSelector : AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            return token is ClassSyntax;
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            ClassSyntax syntax = tokens.ReadAndMoveNext() as ClassSyntax;
            tokens.Commit();

            return nodeList.Filter(item => HtmlUtil.IsCssClassMatch(syntax.ClassName, item.GetAttr("class")));
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            ClassSyntax syntax = tokens.ReadAndMoveNext() as ClassSyntax;
            tokens.Commit();

            return nodeList.GetElementByClassName(syntax.ClassName);
        }
    }
}
