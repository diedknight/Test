﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Winista.Text.HtmlParser;

namespace Pricealyser.Crawler.HtmlParser.WinistaTags
{
    public class RegisterCustomTag
    {
        public static void RegTags(Parser parser)
        {
            if (parser == null) return;
            
            HashSet<string> set = new HashSet<string>();
            PrototypicalNodeFactory nodeFactory = (PrototypicalNodeFactory)parser.Lexer.NodeFactory;

            foreach (string tagName in nodeFactory.TagNames)
            {
                set.Add(tagName);                
            }
            
            while (true)
            {
                var node = parser.Lexer.NextNode();
                if (node == null) break;

                if (!(node is ITag)) continue;

                var tag = node as ITag;

                if (string.IsNullOrEmpty(tag.TagName)) continue;
                if (set.Contains(tag.TagName)) continue;

                set.Add(tag.TagName);

                nodeFactory.RegisterTag(new CustomTag(tag.TagName));
            }

        }
    }
}