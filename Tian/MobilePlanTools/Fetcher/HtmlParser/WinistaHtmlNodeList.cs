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

using Pricealyser.Crawler.HtmlParser.Css;
using Pricealyser.Crawler.NewZealand.HtmlParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Nodes;
using Winista.Text.HtmlParser.Util;

namespace Pricealyser.Crawler.HtmlParser
{
    public class WinistaHtmlNodeList : HtmlNodeList
    {
        private List<INode> _htmlNodelist = null;   //因为有多余的空白标签，这个对象保存过滤后的节点集合
        private NodeList _nodeList = null;

        #region 属性

        public List<INode> CoreNodeList { get { return this._htmlNodelist; } }

        public override int Count { get { return this._htmlNodelist == null ? 0 : this._htmlNodelist.Count; } }

        public override HtmlNodeList ParentNode
        {
            get
            {
                List<INode> list = new List<INode>();

                this._htmlNodelist.ForEach(item =>
                {
                    if (item.Parent == null) return;

                    list.Add(item.Parent);
                });

                if (list.Count == 0) return null;

                return new WinistaHtmlNodeList(list.ToArray());
            }
        }

        public override HtmlNodeList ChildNodes
        {
            get
            {
                List<NodeList> list = new List<NodeList>();

                this._htmlNodelist.ForEach(item =>
                {
                    NodeList nodeList = WinistaUtil.ClearRepeatAndNullNode(item.Children);

                    if (nodeList == null || nodeList.Count == 0) return;

                    list.Add(nodeList);
                });

                if (list.Count == 0) return null;

                return new WinistaHtmlNodeList(list.ToArray());
            }
        }

        public override HtmlNodeList PreviousSibling
        {
            get
            {
                List<INode> list = new List<INode>();

                this._htmlNodelist.ForEach(item => {
                    INode tempNode = item.PreviousSibling;

                    if (tempNode == null) return ;

                    while (WinistaUtil.IsTextNodeWhiteSpace(tempNode))
                    {
                        tempNode = tempNode.PreviousSibling;
                    }

                    if (tempNode == null) return;

                    list.Add(tempNode);
                });

                if (list.Count == 0) return null;

                return new WinistaHtmlNodeList(list.ToArray());
            }
        }

        public override HtmlNodeList NextSibling
        {
            get
            {
                List<INode> list = new List<INode>();

                this._htmlNodelist.ForEach(item =>
                {
                    INode tempNode = item.NextSibling;

                    if (tempNode == null) return;

                    while (WinistaUtil.IsTextNodeWhiteSpace(tempNode))
                    {
                        tempNode = tempNode.NextSibling;
                    }

                    if (tempNode == null) return;

                    list.Add(tempNode);
                });

                if (list.Count == 0) return null;

                return new WinistaHtmlNodeList(list.ToArray());
            }
        }

        public override HtmlNodeList FirstChild
        {
            get
            {
                List<INode> list = new List<INode>();

                this._htmlNodelist.ForEach(item =>
                {
                    INode tempNode = item.FirstChild;

                    if (tempNode == null) return;

                    while (WinistaUtil.IsTextNodeWhiteSpace(tempNode))
                    {
                        tempNode = tempNode.NextSibling;
                    }

                    if (tempNode == null) return;

                    list.Add(tempNode);
                });

                if (list.Count == 0) return null;

                return new WinistaHtmlNodeList(list.ToArray());
            }
        }

        public override HtmlNodeList LastChild
        {
            get
            {
                List<INode> list = new List<INode>();

                this._htmlNodelist.ForEach(item =>
                {
                    INode tempNode = item.LastChild;

                    if (tempNode == null) return;

                    while (WinistaUtil.IsTextNodeWhiteSpace(tempNode))
                    {
                        tempNode = tempNode.PreviousSibling;
                    }

                    if (tempNode == null) return;

                    list.Add(tempNode);
                });

                if (list.Count == 0) return null;

                return new WinistaHtmlNodeList(list.ToArray());
            }
        }

        public override HtmlNode First
        {
            get { return this.Item(0); }
        }

        public override HtmlNode Last
        {
            get { return this.Item(this.Count - 1); }
        }

        public override string OuterHtml
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                this._htmlNodelist.ForEach(item => { sb.AppendLine(new WinistaHtmlNode(item).OuterHtml); });

                return sb.ToString();
            }
        }

        public override string InnerHtml
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                this._htmlNodelist.ForEach(item => { sb.AppendLine(new WinistaHtmlNode(item).InnerHtml); });
                
                return sb.ToString();
            }
        }

        public override string InnerText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                this._htmlNodelist.ForEach(item => { sb.AppendLine(new WinistaHtmlNode(item).InnerText); });

                return sb.ToString();
            }
        }

        #endregion

        public WinistaHtmlNodeList(params NodeList[] nodeListArray)
        {
            if (nodeListArray == null || nodeListArray.Length == 0) throw new Exception("nodeListArray is Null");

            NodeList nodeList = new NodeList();

            for (int i = 0; i < nodeListArray.Length; i++)
            {
                if (nodeListArray[i] == null || nodeListArray[i].Count == 0) continue;

                nodeList.Add(nodeListArray[i]);
            }

            if (nodeList.Count == 0) throw new Exception("nodeList is Null");

            this._nodeList = nodeList;
            this._htmlNodelist = new List<INode>();

            INodeIterator nodeIterator = this._nodeList.Elements();
            while (nodeIterator.HasMoreNodes())
            {
                INode tempNode = nodeIterator.NextNode();

                if (tempNode == null) continue;
                if (WinistaUtil.IsTextNodeWhiteSpace(tempNode)) continue;

                this._htmlNodelist.Add(tempNode);
            }
        }

        public WinistaHtmlNodeList(params INode[] nodeList)
        {
            if (nodeList == null && nodeList.Length == 0) throw new Exception("nodeList is Null");

            var nodes = WinistaUtil.ClearRepeatAndNullNode(nodeList);

            this._htmlNodelist = new List<INode>();
            this._nodeList = new NodeList();

            for (int i = 0; i < nodes.Length; i++)
            {
                //if (nodes[i] == null) continue;
                //if (this.IsTextNodeWhiteSpace(nodes[i])) continue;

                this._htmlNodelist.Add(nodes[i]);
                this._nodeList.Add(nodes[i]);
            }

            if (this._nodeList.Count == 0) throw new Exception("nodeList is Null");
        }

        public override HtmlNode GetElementById(string id)
        {
            HtmlNodeList list = this.GetElementByAttrName("id", id);
            
            if (list == null || list.Count == 0) return null;

            return list.Item(0);
        }

        public override HtmlNodeList GetElementByTagName(string tagName)
        {
            NodeList tempList = new NodeList();

            this._htmlNodelist.ForEach(item =>
            {
                if (item.Children == null) return;

                var tempNodeList = item.Children.ExtractAllNodesThatMatch(new TagNameFilter(tagName), true);

                if (tempNodeList == null || tempNodeList.Count == 0) return;

                tempList.Add(tempNodeList);
            });

            //NodeList tempList = this._nodeList.ExtractAllNodesThatMatch(new TagNameFilter(tagName), true);

            if (tempList == null || tempList.Count == 0) return null;

            return new WinistaHtmlNodeList(tempList);
        }

        public override HtmlNodeList GetElementByClassName(string className)
        {
            //return this.GetElementByAttrName("class", className);
            NodeList nodeList = new NodeList();

            this._htmlNodelist.ForEach(item =>
            {
                var tempNodeList = WinistaUtil.MatchNodeByClassName(className, item.Children);

                if (tempNodeList == null || tempNodeList.Count == 0) return;

                nodeList.Add(tempNodeList);
            });

            if (nodeList == null || nodeList.Count == 0) return null;

            return new WinistaHtmlNodeList(nodeList);
        }

        public override HtmlNodeList GetElementByAttrName(string attrName, string value)
        {
            NodeList tempList = new NodeList();

            this._htmlNodelist.ForEach(item =>
            {
                if (item.Children == null) return;

                var tempNodeList = item.Children.ExtractAllNodesThatMatch(new HasAttributeFilter(attrName, value), true);

                if (tempNodeList == null || tempNodeList.Count == 0) return;

                tempList.Add(tempNodeList);
            });

            //NodeList tempList = this._nodeList.ExtractAllNodesThatMatch(new HasAttributeFilter(attrName, value), true);

            if (tempList == null || tempList.Count == 0) return null;

            return new WinistaHtmlNodeList(tempList);
        }

        public override HtmlNodeList GetElementByAttrName(string attrName)
        {
            NodeList nodeList = new NodeList();

            this._htmlNodelist.ForEach(item =>
            {
                if (item.Children == null) return;

                var tempNodeList = WinistaUtil.ExistNodeByAttrName(attrName, item.Children);

                if (tempNodeList == null || tempNodeList.Count == 0) return;

                nodeList.Add(tempNodeList);
            });

            if (nodeList == null || nodeList.Count == 0) return null;


            //NodeList tempList = WinistaUtil.ExistNodeByAttrName(attrName, this._nodeList);

            //if (tempList == null || tempList.Count == 0) return null;

            return new WinistaHtmlNodeList(nodeList);
        }

        public override List<HtmlNode> ToList()
        {
            List<HtmlNode> list = new List<HtmlNode>();

            this._htmlNodelist.ForEach(item => { list.Add(new WinistaHtmlNode(item)); });

            return list;
        }

        public override HtmlNode Item(int index)
        {
            if (index < 0) return null;
            if (index >= this._htmlNodelist.Count) return null;

            return new WinistaHtmlNode(this._htmlNodelist[index]);
        }

        public override HtmlNodeList Filter(Func<HtmlNode, bool> Match)
        {
            INode[] nodes = this._htmlNodelist.Where(item => Match(new WinistaHtmlNode(item))).ToArray();

            if (nodes.Length == 0) return null;

            return new WinistaHtmlNodeList(nodes);
        }

        public override HtmlNodeList Filter(Func<HtmlNode, int, bool> Match)
        {
            int index = 0;

            INode[] nodes = this._htmlNodelist.Where(item =>
            {
                var result = Match(new WinistaHtmlNode(item), index);
                index++;

                return result;
            }).ToArray();

            if (nodes.Length == 0) return null;

            return new WinistaHtmlNodeList(nodes);
        }

        public override HtmlNodeList Filter(string cssSelector)
        {
            CssSelector css = new CssSelector(this);
            return css.Filter(cssSelector);
        }

        public override HtmlNodeList ForEach(Action<HtmlNode> Action)
        {
            this._htmlNodelist.ForEach(item => { Action(new WinistaHtmlNode(item)); });

            return this;
        }

        public override HtmlNodeList ForEach(Action<HtmlNode, int> Action)
        {
            int index = 0;

            this._htmlNodelist.ForEach(item =>
            {
                Action(new WinistaHtmlNode(item), index);
                index++;
            });

            return this;
        }

        public override HtmlNodeList Combine(HtmlNode node)
        {
            if (node == null) return this;

            List<INode> tempNodeList = new List<INode>();
            tempNodeList.AddRange(this._htmlNodelist);
            tempNodeList.Add(((WinistaHtmlNode)node).CoreNode);

            return new WinistaHtmlNodeList(tempNodeList.ToArray());
        }

        public override HtmlNodeList Combine(HtmlNodeList nodeList)
        {
            if (nodeList == null) return this;

            List<INode> tempNodeList = new List<INode>();
            tempNodeList.AddRange(this._htmlNodelist);
            tempNodeList.AddRange(((WinistaHtmlNodeList)nodeList).CoreNodeList);

            return new WinistaHtmlNodeList(tempNodeList.ToArray());
        }

        public override HtmlNodeList Find(string cssSelector)
        {
            CssSelector css = new CssSelector(this);
            return css.Find(cssSelector);
        }
    }
}
