//===============================================================================
// Pricealyser Crawler
// 
// Copyright (c) 2012 12RMB Ltd. All Rights Reserved.
//
// Author:          TianBJ  
// Date Created:    2015-03-18  (yyyy-MM-dd)
//
// Description:
// 
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Util;
using Pricealyser.Crawler.NewZealand.HtmlParser;
using Pricealyser.Crawler.HtmlParser.WinistaTags;


namespace Pricealyser.Crawler.HtmlParser
{
    public class WinistaHtmlDocument : HtmlDocument
    {
        private Parser _parser = null;
        private HtmlNodeList _htmlNodeList = null;        

        public override string Url
        {
            get { return string.IsNullOrEmpty(this._parser.URL) ? "" : this._parser.URL; }
        }

        public override void Load(string html, string url = "")
        {
            if (string.IsNullOrEmpty(html)) throw new Exception("Document Html IsNullOrEmpty");

            var nodeManage = new WinistaNodeManage();

            //this._parser = new Parser(new Lexer(System.Web.HttpUtility.HtmlDecode(html)));
            this._parser = new Parser(new Lexer(html));
            this._parser.URL = url;
            RegisterCustomTag.RegTags(this._parser);
            
            this._parser.Reset();

            NodeList nodeList = new NodeList();

            //INodeIterator nodeIterator = this._parser.Elements();
            //while (nodeIterator.HasMoreNodes())
            //{
            //    INode tempNode = nodeIterator.NextNode();

            //    if (tempNode == null) continue;
            //    if (WinistaUtil.IsTextNodeWhiteSpace(tempNode)) continue;

            //    nodeList.Add(tempNode);
            //}

            //this._htmlNodeList = new WinistaHtmlNodeList(nodeList);

            INodeIterator nodeIterator = this._parser.Elements();
            while (nodeIterator.HasMoreNodes())
            {
                INode tempNode = nodeIterator.NextNode();

                nodeList.Add(tempNode);
            }

            this._htmlNodeList = nodeManage.Create(nodeList);
        }

        public override HtmlNodeList ChildNodes
        {
            get
            {
                return this._htmlNodeList.ChildNodes;
            }
        }

        public override int Count
        {
            get
            {
                return this._htmlNodeList.Count;
            }
        }

        public override HtmlNodeList FirstChild
        {
            get
            {
                return this._htmlNodeList.FirstChild;
            }
        }

        public override string InnerHtml
        {
            get
            {
                return this._htmlNodeList.InnerHtml;
            }
        }

        public override string InnerText
        {
            get
            {
                return this._htmlNodeList.InnerText;
            }
        }

        public override HtmlNodeList LastChild
        {
            get
            {
                return this._htmlNodeList.LastChild;
            }
        }

        public override HtmlNodeList NextSibling
        {
            get
            {
                return this._htmlNodeList.NextSibling;
            }
        }

        public override string OuterHtml
        {
            get
            {
                return this._htmlNodeList.OuterHtml;
            }
        }

        public override HtmlNodeList ParentNode
        {
            get
            {
                return this._htmlNodeList.ParentNode;
            }
        }

        public override HtmlNodeList PreviousSibling
        {
            get
            {
                return this._htmlNodeList.PreviousSibling;
            }
        }

        public override HtmlNode First
        {
            get { return this._htmlNodeList.First; }
        }

        public override HtmlNode Last
        {
            get { return this._htmlNodeList.Last; }
        }

        public override HtmlNode Item(int index)
        {
            return this._htmlNodeList.Item(index);
        }

        public override HtmlNodeList Filter(Func<HtmlNode, bool> Match)
        {
            return this._htmlNodeList.Filter(Match);
        }

        public override HtmlNodeList Filter(Func<HtmlNode, int, bool> Match)
        {
            return this._htmlNodeList.Filter(Match);
        }

        public override HtmlNodeList Filter(string cssSelector)
        {
            return this._htmlNodeList.Filter(cssSelector);
        }

        public override HtmlNodeList ForEach(Action<HtmlNode> Action)
        {
            return this._htmlNodeList.ForEach(Action);
        }

        public override HtmlNodeList ForEach(Action<HtmlNode, int> Action)
        {
            return this._htmlNodeList.ForEach(Action);
        }

        public override List<HtmlNode> ToList()
        {
            return this._htmlNodeList.ToList();
        }

        public override HtmlNode GetElementById(string id)
        {            
            //HtmlNodeList list = this.GetElementByAttrName("id", id);

            //if (list == null || list.Count == 0) return null;

            //return list.Item(0);
            
            return this._htmlNodeList.GetElementById(id);
        }

        public override HtmlNodeList GetElementByTagName(string tagName)
        {
            //this._parser.Reset();
            //NodeList nodeList = this._parser.Parse(new TagNameFilter(tagName));

            //if (nodeList == null || nodeList.Count == 0) return null;

            //return new WinistaHtmlNodeList(nodeList);

            return this._htmlNodeList.GetElementByTagName(tagName);
        }

        public override HtmlNodeList GetElementByClassName(string className)
        {
            //return this.GetElementByAttrName("class", className);
            //this._parser.Reset();

            //NodeList nodeList = new NodeList();

            //INodeIterator nodeIterator = this._parser.Elements();
            //while (nodeIterator.HasMoreNodes())
            //{
            //    INode tempNode = nodeIterator.NextNode();
            //    var tempNodeList = WinistaUtil.MatchNodeByClassName(className, tempNode);

            //    if (tempNodeList == null || tempNodeList.Count == 0) continue;

            //    nodeList.Add(tempNodeList);
            //}

            //if (nodeList == null || nodeList.Count == 0) return null;

            //return new WinistaHtmlNodeList(nodeList);

            return this._htmlNodeList.GetElementByClassName(className);
        }

        public override HtmlNodeList GetElementByAttrName(string attrName, string value)
        {
            //this._parser.Reset();
            //NodeList nodeList = this._parser.Parse(new HasAttributeFilter(attrName, value));

            //if (nodeList == null || nodeList.Count == 0) return null;

            //return new WinistaHtmlNodeList(nodeList);

            return this._htmlNodeList.GetElementByAttrName(attrName, value);
        }

        public override HtmlNodeList GetElementByAttrName(string attrName)
        {
            //NodeList tempList = WinistaUtil.ExistNodeByAttrName(attrName, ((WinistaHtmlNodeList)this._htmlNodeList).CoreNodeList.ToArray());

            //if (tempList == null || tempList.Count == 0) return null;

            //return new WinistaHtmlNodeList(tempList);

            return this._htmlNodeList.GetElementByAttrName(attrName);
        }

        public override HtmlNodeList Combine(HtmlNode node)
        {
            return this._htmlNodeList.Combine(node);
        }

        public override HtmlNodeList Combine(HtmlNodeList nodeList)
        {
            return this._htmlNodeList.Combine(nodeList);
        }

        public override HtmlNodeList Find(string cssSelector)
        {
            return this._htmlNodeList.Find(cssSelector);
        }

        public override HtmlNodeList Remove(HtmlNode node)
        {
            return this._htmlNodeList.Remove(node);
        }

        public override HtmlNodeList Remove(HtmlNodeList nodeList)
        {
            return this._htmlNodeList.Remove(nodeList);
        }

        public override HtmlNodeList RemoveAll()
        {
            return this._htmlNodeList.RemoveAll();
        }
    }
}
