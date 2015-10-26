using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.BasicFilter
{
    public class EqFilterSelector:AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (!(token is ValueFilterSyntax)) return false;

            return ((ValueFilterSyntax)token).FilterName == "eq";
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            ValueFilterSyntax syntax = tokens.ReadAndMoveNext() as ValueFilterSyntax;
            tokens.Commit();

            int tempValue = Convert.ToInt32(syntax.Value);
            tempValue = tempValue >= 0 ? tempValue : nodeList.Count + tempValue;

            return nodeList.Item(tempValue).Combine();
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }

    }
}
