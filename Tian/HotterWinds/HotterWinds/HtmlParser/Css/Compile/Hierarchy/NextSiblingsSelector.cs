using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.Hierarchy
{
    public class NextSiblingsSelector:AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (!(token is RelationSyntax)) return false;

            return ((RelationSyntax)token).Relation == "~";
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

            HtmlNodeList list = null;

            nodeList.ForEach(item =>
            {
                var node = item.NextSibling;

                while (node != null)
                {
                    list = list == null ? node.Combine() : list.Combine(node);

                    node = node.NextSibling;
                }
            });

            //HtmlNodeList tempNodeList = nodeList.NextSibling;

            //while (tempNodeList != null)
            //{
            //    list = list == null ? tempNodeList : list.Combine(tempNodeList);

            //    tempNodeList = tempNodeList.NextSibling;
            //}

            if (list == null) return null;
            return list.Filter(value);
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }
    }
}
