﻿using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.BasicFilter
{
    public class LastFilterSelector:AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (!(token is NotValueFilterSyntax)) return false;

            return ((NotValueFilterSyntax)token).FilterName == "last";
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            NotValueFilterSyntax syntax = tokens.ReadAndMoveNext() as NotValueFilterSyntax;
            tokens.Commit();

            return nodeList.Item(nodeList.Count - 1).Combine();
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }
    }
}