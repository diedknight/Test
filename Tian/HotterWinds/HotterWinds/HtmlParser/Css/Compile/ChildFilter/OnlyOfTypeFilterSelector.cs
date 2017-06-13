﻿using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.ChildFilter
{
    public class OnlyOfTypeFilterSelector:AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (!(token is NotValueFilterSyntax)) return false;

            return ((NotValueFilterSyntax)token).FilterName == "only-of-type";
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            NotValueFilterSyntax syntax = tokens.ReadAndMoveNext() as NotValueFilterSyntax;
            tokens.Commit();

            return nodeList.Filter(item =>
            {
                if (item.ParentNode == null) return false;

                return item.ParentNode.ChildNodes.Filter(tempItem => tempItem.NodeName == item.NodeName).Count == 1 && item.IndexOfType == 0;
            });
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }
    }
}