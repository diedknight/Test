using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.ContentFilter
{
    public class HasFilterSelector:AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (!(token is SelectorFilterSyntax)) return false;

            return ((SelectorFilterSyntax)token).FilterName == "has";
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            SelectorFilterSyntax syntax = tokens.ReadAndMoveNext() as SelectorFilterSyntax;
            tokens.Commit();

            CssSelector css = new CssSelector(nodeList);
            HtmlNodeList tempNodeList = css.Find(syntax.CssSelector);

            if (tempNodeList == null) return null;

            Dictionary<HtmlNode, bool> tempDic = new Dictionary<HtmlNode, bool>();
            Stack<HtmlNode> stack = new Stack<HtmlNode>();

            tempNodeList.ParentNode.ForEach(item => stack.Push(item));

            while (stack.Count > 0)
            {
                HtmlNode tempNode = stack.Pop();

                if (!tempDic.ContainsKey(tempNode)) tempDic.Add(tempNode, true);

                if (tempNode.ParentNode != null) stack.Push(tempNode.ParentNode);
            }

            return nodeList.Filter(item => tempDic.ContainsKey(item));
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }
    }
}
