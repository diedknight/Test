using System;
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

            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            PrototypicalNodeFactory nodeFactory = (PrototypicalNodeFactory)parser.Lexer.NodeFactory;

            foreach (string tagName in nodeFactory.TagNames)
            {
                dic.Add(tagName, true);
            }
            
            while (true)
            {
                var node = parser.Lexer.NextNode();
                if (node == null) break;

                if (!(node is ITag)) continue;

                var tag = node as ITag;
                if (string.IsNullOrEmpty(tag.TagName)) continue;
                if (dic.ContainsKey(tag.TagName)) continue;

                dic.Add(tag.TagName, true);

                nodeFactory.RegisterTag(new CustomTag(tag.TagName));
            }

        }
    }
}
