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
using System.Web;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Nodes;
using Winista.Text.HtmlParser.Tags;
using Winista.Text.HtmlParser.Util;

namespace Pricealyser.Crawler.HtmlParser
{
    public class WinistaHtmlNode : HtmlNode
    {
        private INode _node = null;
        public INode CoreNode { get { return this._node; } }

        public WinistaHtmlNode(INode node)
        {
            if (node == null) throw new Exception("node is Null");

            this._node = node;            
        }

        public override int Index
        {
            get
            {
                if (this._node.Parent == null) return -1;

                int tempIndex = 0;

                new WinistaHtmlNode(this._node.Parent).ChildNodes.ForEach((item, index) =>
                {
                    if (((WinistaHtmlNode)item).CoreNode == this._node)
                        tempIndex = index;
                });

                return tempIndex;
            }
        }

        public override int IndexOfType
        {
            get
            {
                if (this._node.Parent == null) return -1;

                int resultIndex = 0;
                int tempIndex = -1;

                new WinistaHtmlNode(this._node.Parent).ChildNodes.ForEach(item =>
                {
                    if (item.NodeName != this.NodeName) return;

                    tempIndex++;

                    if (((WinistaHtmlNode)item).CoreNode == this._node) resultIndex = tempIndex;
                });

                return resultIndex;
            }
        }

        public override int IndexOfIgnoreTextNode
        {
            get
            {
                if (this._node.Parent == null) return -1;

                int resultIndex = 0;
                int tempIndex = -1;

                new WinistaHtmlNode(this._node.Parent).ChildNodes.ForEach(item =>
                {
                    if (item.NodeName == "") return;

                    tempIndex++;

                    if (((WinistaHtmlNode)item).CoreNode == this._node) resultIndex = tempIndex;
                });

                return resultIndex;
            }
        }

        public override string NodeName
        {
            get
            {
                if (this._node is TextNode) return "";

                return ((ITag)this._node).TagName.ToLower();
            }
        }

        public override string Value
        {
            get
            {
                if (this._node is TextNode) return HttpUtility.HtmlDecode(this._node.GetText());

                string value = ((ITag)this._node).GetAttribute("VALUE");
                if (value != null) return HttpUtility.HtmlDecode(value);

                if (this._node.Children == null) return "";
                if (this._node.Children.Count != 1) return "";
                if (this._node.FirstChild == null) return "";
                if (!(this._node.FirstChild is TextNode)) return "";

                return HttpUtility.HtmlDecode(this._node.FirstChild.GetText());
            }
        }

        public override HtmlNode ParentNode
        {
            get { return this._node.Parent == null ? null : new WinistaHtmlNode(this._node.Parent); }
        }

        public override HtmlNodeList ChildNodes
        {
            get
            {
                NodeList nodeList = WinistaUtil.ClearRepeatAndNullNode(this._node.Children);

                if (nodeList == null || nodeList.Count == 0) return null;

                return new WinistaHtmlNodeList(this._node.Children);
            }
        }

        public override HtmlNode PreviousSibling
        {
            get
            {
                INode tempNode = this._node.PreviousSibling;

                if(tempNode==null) return null;

                while (WinistaUtil.IsTextNodeWhiteSpace(tempNode))
                {
                    tempNode = tempNode.PreviousSibling;
                }

                return tempNode == null ? null : new WinistaHtmlNode(tempNode);
            }
        }

        public override HtmlNode NextSibling
        {
            get
            {
                INode tempNode = this._node.NextSibling;

                if (tempNode == null) return null;

                while (WinistaUtil.IsTextNodeWhiteSpace(tempNode))
                {
                    tempNode = tempNode.NextSibling;
                }

                return tempNode == null ? null : new WinistaHtmlNode(tempNode);
            }
        }

        public override HtmlNode FirstChild
        {
            get
            {
                INode tempNode = this._node.FirstChild;

                if (tempNode == null) return null;

                while (WinistaUtil.IsTextNodeWhiteSpace(tempNode))
                {
                    tempNode = tempNode.NextSibling;
                }

                return tempNode == null ? null : new WinistaHtmlNode(tempNode);
            }
        }

        public override HtmlNode LastChild
        {
            get
            {
                INode tempNode = this._node.LastChild;

                if (tempNode == null) return null;

                while (WinistaUtil.IsTextNodeWhiteSpace(tempNode))
                {
                    tempNode = tempNode.PreviousSibling;
                }

                return tempNode == null ? null : new WinistaHtmlNode(tempNode);
            }
        }

        public override string OuterHtml
        {
            get { return this._node == null ? "" : HttpUtility.HtmlDecode(this._node.ToHtml(true)); }
        }

        public override string InnerHtml
        {
            get
            {
                if (this._node == null) return "";
                if (this._node.Children == null) return "";

                return HttpUtility.HtmlDecode(this._node.Children.ToHtml(true));
            }
        }

        public override string InnerText
        {
            get
            {
                if (this._node == null) return "";

                return HttpUtility.HtmlDecode(this._node.ToPlainTextString());
            }
        }

        public override HtmlNode GetElementById(string id)
        {
            HtmlNodeList list = this.GetElementByAttrName("id", id);

            if (list == null || list.Count == 0) return null;

            return list.Item(0);
        }

        public override HtmlNodeList GetElementByTagName(string tagName)
        {
            if (this._node.Children == null) return null;

            NodeList tempList = this._node.Children.ExtractAllNodesThatMatch(new TagNameFilter(tagName), true);

            if (tempList == null || tempList.Count == 0) return null;

            return new WinistaHtmlNodeList(tempList);
        }

        public override HtmlNodeList GetElementByClassName(string className)
        {
            //return this.GetElementByAttrName("class", className);

            NodeList nodeList = WinistaUtil.MatchNodeByClassName(className, this._node.Children);

            if (nodeList == null || nodeList.Count == 0) return null;

            return new WinistaHtmlNodeList(nodeList);
        }

        public override HtmlNodeList GetElementByAttrName(string attrName, string value)
        {
            if (this._node.Children == null) return null;

            NodeList tempList = this._node.Children.ExtractAllNodesThatMatch(new HasAttributeFilter(attrName, value), true);

            if (tempList == null || tempList.Count == 0) return null;

            return new WinistaHtmlNodeList(tempList);
        }

        public override string GetAttr(string attr)
        {
            if (string.IsNullOrEmpty(attr)) return null;
            if (this._node is TextNode) return null;

            string result = ((ITag)this._node).GetAttribute(attr.ToUpper());

            if (result == null) return null;

            return HttpUtility.HtmlDecode(result);
        }

        public override string GetLink()
        {
            if (this._node is ATag) return HttpUtility.HtmlDecode(((ATag)this._node).Link);
            if (this._node is ImageTag) return HttpUtility.HtmlDecode(((ImageTag)this._node).ImageURL);
            if (this._node is IFrameTag) return HttpUtility.HtmlDecode(((IFrameTag)this._node).FrameLocation);
            if (this._node is LinkTag) return HttpUtility.HtmlDecode(((LinkTag)this._node).ExtractLink());
            if (this._node is FormTag) return HttpUtility.HtmlDecode(((FormTag)this._node).FormLocation);

            return "";
        }

        public override HtmlNodeList Combine()
        {
            return new WinistaHtmlNodeList(((WinistaHtmlNode)this).CoreNode);
        }

        public override HtmlNodeList Combine(HtmlNode node)
        {
            if (node == null) return new WinistaHtmlNodeList(this._node);

            INode targetCoreNode = ((WinistaHtmlNode)node).CoreNode;

            return new WinistaHtmlNodeList(this._node, targetCoreNode);
        }

        public override HtmlNodeList Combine(HtmlNodeList nodeList)
        {
            if (nodeList == null) return new WinistaHtmlNodeList(this._node);

            List<INode> tempNodeList = new List<INode>();
            tempNodeList.Add(this._node);
            tempNodeList.AddRange(((WinistaHtmlNodeList)nodeList).CoreNodeList);

            return new WinistaHtmlNodeList(tempNodeList.ToArray());
        }

        public override HtmlNodeList GetElementByAttrName(string attrName)
        {
            NodeList tempList = WinistaUtil.ExistNodeByAttrName(attrName, this._node);

            if (tempList == null || tempList.Count == 0) return null;

            return new WinistaHtmlNodeList(tempList);
        }

        public override HtmlNodeList Find(string cssSelector)
        {
            CssSelector css = new CssSelector(this);
            return css.Find(cssSelector);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is WinistaHtmlNode)) return false;

            return this._node == ((WinistaHtmlNode)obj).CoreNode;
        }
    }
}
