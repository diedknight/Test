using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.Basic
{
    public class AllSelector : AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            return token is AllElementSyntax;
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            AllElementSyntax syntax = tokens.ReadAndMoveNext() as AllElementSyntax;
            tokens.Commit();

            return nodeList;
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            AllElementSyntax syntax = tokens.ReadAndMoveNext() as AllElementSyntax;
            tokens.Commit();

            if (nodeList.ChildNodes == null) return null;

            HtmlNodeList resultList = null;
            Stack<HtmlNode> queue = new Stack<HtmlNode>();            

            nodeList.ChildNodes.ForEach(item =>
            {
                if (item.NodeName == "") return;

                HtmlNodeList tempNodeList = null;
                queue.Push(item);

                while (queue.Count > 0)
                {
                    HtmlNode node = queue.Pop();

                    resultList = resultList == null ? node.Combine() : resultList.Combine(node);

                    tempNodeList = CompileUtil.ClearTextNode(node.ChildNodes);
                    if (tempNodeList == null) continue;

                    tempNodeList.ForEach((tempItem, index) =>
                    {
                        queue.Push(tempNodeList.Item(tempNodeList.Count - 1 - index));
                    });
                }
            });

            return resultList;
        }
    }
}
