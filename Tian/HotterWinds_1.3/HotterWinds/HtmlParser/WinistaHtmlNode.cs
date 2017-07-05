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
using Pricealyser.Crawler.NewZealand.HtmlParser.WinistaTags;
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
        private WinistaNodeManage _nodeManage = null;

        private INode _node = null;
        public INode CoreNode { get { return this._node; } }

        public WinistaHtmlNode(INode node, WinistaNodeManage nodeManage)
        {
            if (node == null) throw new Exception("node is Null");
            if (nodeManage == null) throw new Exception("nodeManage is Null");

            //if (node is CompositeTag)
            //    this._node = new ProxyCompositeTag((CompositeTag)node, nodeManage);
            //else
            this._node = node;

            this._nodeManage = nodeManage;
        }

        public override int Index
        {
            get
            {
                if (this._node.Parent == null) return -1;

                int tempIndex = 0;

                this._nodeManage.Create(this._node.Parent).ChildNodes.ForEach((item, index) =>
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

                this._nodeManage.Create(this._node.Parent).ChildNodes.ForEach(item =>
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

                this._nodeManage.Create(this._node.Parent).ChildNodes.ForEach(item =>
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

                //if (this._node.Children == null) return "";
                //if (this._node.Children.Count != 1) return "";
                //if (this._node.FirstChild == null) return "";
                //if (!(this._node.FirstChild is TextNode)) return "";

                if (this._node.Children == null) return "";

                var childList = this._nodeManage.Util.ClearUselessNode(this._node.Children).ToList();

                if (childList == null) return "";
                if (childList.Count != 1) return "";
                if (!(childList[0] is TextNode)) return "";

                return HttpUtility.HtmlDecode(childList[0].GetText());
            }
        }

        public override HtmlNode ParentNode
        {
            get { return this._nodeManage.Create(this._node.Parent); }
        }

        public override HtmlNodeList ChildNodes
        {
            get { return this._nodeManage.Create(this._node.Children); }
        }

        public override HtmlNode PreviousSibling
        {
            get
            {
                INode tempNode = this._node.PreviousSibling;

                while (true)
                {
                    if (tempNode == null) return null;
                    if (!this._nodeManage.Util.IsUselessNode(tempNode)) break;

                    tempNode = tempNode.PreviousSibling;
                }

                return this._nodeManage.Create(tempNode);
            }
        }

        public override HtmlNode NextSibling
        {
            get
            {
                INode tempNode = this._node.NextSibling;

                while (true)
                {
                    if (tempNode == null) return null;
                    if (!this._nodeManage.Util.IsUselessNode(tempNode)) break;

                    tempNode = tempNode.NextSibling;
                }

                return this._nodeManage.Create(tempNode);
            }
        }

        public override HtmlNode FirstChild
        {
            get
            {
                INode tempNode = this._node.FirstChild;

                while (true)
                {
                    if (tempNode == null) return null;
                    if (!this._nodeManage.Util.IsUselessNode(tempNode)) break;

                    tempNode = tempNode.NextSibling;
                }

                return this._nodeManage.Create(tempNode);
            }
        }

        public override HtmlNode LastChild
        {
            get
            {
                INode tempNode = this._node.LastChild;

                while (true)
                {
                    if (tempNode == null) return null;
                    if (!this._nodeManage.Util.IsUselessNode(tempNode)) break;

                    tempNode = tempNode.PreviousSibling;
                }

                return this._nodeManage.Create(tempNode);
            }
        }

        public override string OuterHtml
        {
            get
            {
                var node = this._node;

                //if (node is CompositeTag) node = new ProxyCompositeTag((CompositeTag)node, this._nodeManage);

                return HttpUtility.HtmlDecode(node.ToHtml(true));
            }
        }

        public override string InnerHtml
        {
            get
            {
                if (this._node.Children == null) return "";

                var node = this._node;

                // if (node is CompositeTag) node = new ProxyCompositeTag((CompositeTag)node, this._nodeManage);

                return HttpUtility.HtmlDecode(node.Children.ToHtml(true));
            }
        }

        public override string InnerText
        {
            get
            {
                var node = this._node;

                //if (node is CompositeTag) node = new ProxyCompositeTag((CompositeTag)node, this._nodeManage);

                return HttpUtility.HtmlDecode(node.ToPlainTextString());
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

            return this._nodeManage.Create(tempList);
        }

        public override HtmlNodeList GetElementByClassName(string className)
        {
            NodeList tempList = this._nodeManage.Util.MatchNodeByClassName(className, this._node.Children);

            return this._nodeManage.Create(tempList);
        }

        public override HtmlNodeList GetElementByAttrName(string attrName, string value)
        {
            if (this._node.Children == null) return null;

            NodeList tempList = this._node.Children.ExtractAllNodesThatMatch(new HasAttributeFilter(attrName, value), true);

            return this._nodeManage.Create(tempList);
        }

        public override HtmlNodeList GetElementByAttrName(string attrName)
        {
            NodeList tempList = this._nodeManage.Util.ExistNodeByAttrName(attrName, this._node);

            return this._nodeManage.Create(tempList);
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
            else if (this._node is ImageTag) return HttpUtility.HtmlDecode(((ImageTag)this._node).ImageURL);
            else if (this._node is IFrameTag) return HttpUtility.HtmlDecode(((IFrameTag)this._node).FrameLocation);
            else if (this._node is LinkTag) return HttpUtility.HtmlDecode(((LinkTag)this._node).ExtractLink());
            else if (this._node is FormTag) return HttpUtility.HtmlDecode(((FormTag)this._node).FormLocation);
            else
            {
                if (this._node is ITag)
                {
                    string result = ((ITag)this._node).GetAttribute("src");
                    if (result == null) return "";

                    return HttpUtility.HtmlDecode(result);
                }
            }

            return "";
        }

        public override HtmlNodeList Combine()
        {
            return this._nodeManage.Create(new NodeList(this._node));
        }

        public override HtmlNodeList Combine(HtmlNode node)
        {
            if (node == null) return this.Combine();

            INode targetCoreNode = ((WinistaHtmlNode)node).CoreNode;

            return this._nodeManage.Create(new INode[2] { this._node, targetCoreNode });
        }

        public override HtmlNodeList Combine(HtmlNodeList nodeList)
        {
            if (nodeList == null) return this.Combine();

            List<INode> tempNodeList = new List<INode>();
            tempNodeList.Add(this._node);
            tempNodeList.AddRange(((WinistaHtmlNodeList)nodeList).CoreNodeList);

            return this._nodeManage.Create(tempNodeList);
        }

        public override HtmlNode RemoveChild(HtmlNode node)
        {
            if (this._node.Children == null) return this;

            if (this._node.Children.Remove(((WinistaHtmlNode)node).CoreNode))
            {
                this._nodeManage.RemoveNode(node);
            }

            return node;
        }

        public override HtmlNodeList Find(string cssSelector)
        {
            return new CssSelector(this).Find(cssSelector);
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
