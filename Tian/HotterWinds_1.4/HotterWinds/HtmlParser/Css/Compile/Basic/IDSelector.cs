using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.Basic
{
    public class IDSelector : AbsCompile
    {
        public override bool Vaild(AbsSyntax token)
        {
            return token is IdSyntax;
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            IdSyntax syntax = tokens.ReadAndMoveNext() as IdSyntax;
            tokens.Commit();

            return nodeList.Filter(item => item.GetAttr("id") == syntax.IdName);
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            IdSyntax syntax = tokens.ReadAndMoveNext() as IdSyntax;
            tokens.Commit();

            var node = nodeList.GetElementById(syntax.IdName);
            
            if (node == null) return null;            
            return node.Combine();
        }

        
    }
}
