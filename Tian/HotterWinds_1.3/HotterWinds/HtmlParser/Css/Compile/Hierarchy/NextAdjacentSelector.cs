using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.Hierarchy
{
    public class NextAdjacentSelector:AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (!(token is RelationSyntax)) return false;

            return ((RelationSyntax)token).Relation == "+";
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            tokens.ReadAndMoveNext();

            if (!tokens.HaveMoreToken())
            {
                tokens.Rollback();
                return null;
            }

            string value = tokens.ReadAndMoveNext().ToString();
            tokens.Commit();

            var list = nodeList.NextSibling;
            if (list == null) return null;

            return list.Filter(value);
        }


        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }
    }
}
