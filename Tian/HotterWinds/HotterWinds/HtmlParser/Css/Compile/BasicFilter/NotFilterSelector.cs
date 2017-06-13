using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.BasicFilter
{
    public class NotFilterSelector : AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (!(token is SelectorFilterSyntax)) return false;

            return ((SelectorFilterSyntax)token).FilterName == "not";
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            SelectorFilterSyntax syntax = tokens.ReadAndMoveNext() as SelectorFilterSyntax;
            tokens.Commit();

            CssSelector css = new CssSelector(nodeList);
            HtmlNodeList tempNodeList = css.Filter(syntax.CssSelector);

            if (tempNodeList == null) return nodeList;

            List<HtmlNode> templist = tempNodeList.ToList();

            return nodeList.Filter(item => !templist.Contains(item));
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }
    }
}
