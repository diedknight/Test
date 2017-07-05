using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile
{
    public abstract class AbsCompile
    {
        public abstract bool Vaild(AbsSyntax token);

        public abstract HtmlNodeList Run(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens);
        public abstract HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens);

        public HtmlNodeList Action(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            if (!tokens.HaveMoreToken()) return nodeList;
            if (nodeList == null) return null;
            if (nodeList.Count == 0) return null;

            if (tokens.Read(0).IsFilter) return this.FilterAction(nodeList, tokens);

            if (!this.Vaild(tokens.Read(0))) return nodeList;            

            return this.Run(nodeList, tokens);
        }

        public HtmlNodeList FilterAction(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            if (!tokens.HaveMoreToken()) return nodeList;
            if (nodeList == null) return null;
            if (nodeList.Count == 0) return null;

            if (!this.Vaild(tokens.Read(0))) return nodeList;

            return this.FilterRun(nodeList, tokens);
        }

    }
}
