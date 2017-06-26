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
        private List<HtmlNode> _htmlNodelist = null;   //因为有多余的空白标签，这个对象保存过滤后的节点集合        
        private WinistaNodeManage _nodeManage = null;        

        public WinistaHtmlNodeList(WinistaNodeManage nodeManage, IEnumerable<INode> nodeList)
        {            
            this._nodeManage = nodeManage;
            this._htmlNodelist = new List<HtmlNode>();

            foreach (INode node in nodeList)
            {
                this._htmlNodelist.Add(this._nodeManage.Create(node));
            }
        }

        #region 属性

        public List<INode> CoreNodeList { get { return this._htmlNodelist.Select(item => ((WinistaHtmlNode)item).CoreNode).ToList(); } }

        public override int Count { get { return this._htmlNodelist == null ? 0 : this._htmlNodelist.Count; } }

        public override HtmlNodeList ParentNode
        {
            get
            {
                //List<INode> list = new List<INode>();

                //this._htmlNodelist.ForEach(item =>
                //{
                //    if (item.Parent == null) return;

                //    list.Add(item.Parent);
                //});

                //if (list.Count == 0) return null;

                //return new WinistaHtmlNodeList(list.ToArray());

                return this._nodeManage.Create(this._htmlNodelist.Select(item => item.ParentNode));
            }
        }

        public override HtmlNodeList ChildNodes
        {
            get
            {
                //List<NodeList> list = new List<NodeList>();

                //this._htmlNodelist.ForEach(item =>
                //{
                //    NodeList nodeList = this._nodeManage.Util.ClearRepeatAndNullNode(item.Children);

                //    if (nodeList == null || nodeList.Count == 0) return;

                //    list.Add(nodeList);
                //});

                //if (list.Count == 0) return null;

                //return new WinistaHtmlNodeList(list.ToArray());

                return this._nodeManage.Create(this._htmlNodelist.Select(item => item.ChildNodes));
            }
        }

        public override HtmlNodeList PreviousSibling
        {
            get
            {
                //List<INode> list = new List<INode>();

                //this._htmlNodelist.ForEach(item => {
                //    INode tempNode = item.PreviousSibling;

                //    if (tempNode == null) return;

                //    while (this._nodeManage.Util.IsTextNodeWhiteSpace(tempNode))
                //    {
                //        tempNode = tempNode.PreviousSibling;
                //    }

                //    if (tempNode == null) return;

                //    list.Add(tempNode);
                //});

                //if (list.Count == 0) return null;

                //return new WinistaHtmlNodeList(list.ToArray());

                return this._nodeManage.Create(this._htmlNodelist.Select(item => item.PreviousSibling));
            }
        }

        public override HtmlNodeList NextSibling
        {
            get
            {
                //List<INode> list = new List<INode>();

                //this._htmlNodelist.ForEach(item =>
                //{
                //    INode tempNode = item.NextSibling;

                //    if (tempNode == null) return;

                //    while (this._nodeManage.Util.IsTextNodeWhiteSpace(tempNode))
                //    {
                //        tempNode = tempNode.NextSibling;
                //    }

                //    if (tempNode == null) return;

                //    list.Add(tempNode);
                //});

                //if (list.Count == 0) return null;

                //return new WinistaHtmlNodeList(list.ToArray());

                return this._nodeManage.Create(this._htmlNodelist.Select(item => item.NextSibling));
            }
        }

        public override HtmlNodeList FirstChild
        {
            get
            {
                //List<INode> list = new List<INode>();

                //this._htmlNodelist.ForEach(item =>
                //{
                //    INode tempNode = item.FirstChild;

                //    if (tempNode == null) return;

                //    while (this._nodeManage.Util.IsTextNodeWhiteSpace(tempNode))
                //    {
                //        tempNode = tempNode.NextSibling;
                //    }

                //    if (tempNode == null) return;

                //    list.Add(tempNode);
                //});

                //if (list.Count == 0) return null;

                //return new WinistaHtmlNodeList(list.ToArray());

                return this._nodeManage.Create(this._htmlNodelist.Select(item => item.FirstChild));
            }
        }

        public override HtmlNodeList LastChild
        {
            get
            {
                //List<INode> list = new List<INode>();

                //this._htmlNodelist.ForEach(item =>
                //{
                //    INode tempNode = item.LastChild;

                //    if (tempNode == null) return;

                //    while (this._nodeManage.Util.IsTextNodeWhiteSpace(tempNode))
                //    {
                //        tempNode = tempNode.PreviousSibling;
                //    }

                //    if (tempNode == null) return;

                //    list.Add(tempNode);
                //});

                //if (list.Count == 0) return null;

                //return new WinistaHtmlNodeList(list.ToArray());

                return this._nodeManage.Create(this._htmlNodelist.Select(item => item.LastChild));
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

                //this._htmlNodelist.ForEach(item => { sb.AppendLine(new WinistaHtmlNode(item).OuterHtml); });

                this._htmlNodelist.ForEach(item => { sb.AppendLine(item.OuterHtml); });

                return sb.ToString();
            }
        }

        public override string InnerHtml
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                //this._htmlNodelist.ForEach(item => { sb.AppendLine(new WinistaHtmlNode(item).InnerHtml); });

                this._htmlNodelist.ForEach(item => { sb.AppendLine(item.InnerHtml); });

                return sb.ToString();
            }
        }

        public override string InnerText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                //this._htmlNodelist.ForEach(item => { sb.AppendLine(new WinistaHtmlNode(item).InnerText); });

                this._htmlNodelist.ForEach(item => { sb.AppendLine(item.InnerText); });

                return sb.ToString();
            }
        }

        #endregion        

        //public WinistaHtmlNodeList(WinistaNodeManage nodeManage, params NodeList[] nodeListArray)            
        //{
        //    if (nodeListArray == null || nodeListArray.Length == 0) throw new Exception("nodeListArray is Null");

        //    NodeList nodeList = new NodeList();

        //    for (int i = 0; i < nodeListArray.Length; i++)
        //    {
        //        if (nodeListArray[i] == null || nodeListArray[i].Count == 0) continue;

        //        nodeList.Add(nodeListArray[i]);
        //    }

        //    if (nodeList.Count == 0) throw new Exception("nodeList is Null");
            
        //    this._htmlNodelist = new List<INode>();

        //    INodeIterator nodeIterator = nodeList.Elements();
        //    while (nodeIterator.HasMoreNodes())
        //    {
        //        INode tempNode = nodeIterator.NextNode();

        //        if (tempNode == null) continue;
        //        if (this._nodeManage.Util.IsTextNodeWhiteSpace(tempNode)) continue;

        //        this._htmlNodelist.Add(tempNode);
        //    }
        //}

        //public WinistaHtmlNodeList(WinistaNodeManage nodeManage, params INode[] nodeList)            
        //{
        //    if (nodeList == null && nodeList.Length == 0) throw new Exception("nodeList is Null");

        //    var nodes = this._nodeManage.Util.ClearRepeatAndNullNode(nodeList);

        //    this._htmlNodelist = new List<INode>();

        //    this._htmlNodelist.AddRange(nodes);

        //    //for (int i = 0; i < nodes.Length; i++)
        //    //{
        //    //    //if (nodes[i] == null) continue;
        //    //    //if (this.IsTextNodeWhiteSpace(nodes[i])) continue;

        //    //    this._htmlNodelist.Add(nodes[i]);
        //    //}
        //}

        public override HtmlNode GetElementById(string id)
        {
            HtmlNodeList list = this.GetElementByAttrName("id", id);
            
            if (list == null || list.Count == 0) return null;

            return list.Item(0);
        }

        public override HtmlNodeList GetElementByTagName(string tagName)
        {
            //NodeList tempList = new NodeList();

            //this._htmlNodelist.ForEach(item =>
            //{
            //    if (item.Children == null) return;

            //    var tempNodeList = item.Children.ExtractAllNodesThatMatch(new TagNameFilter(tagName), true);

            //    if (tempNodeList == null || tempNodeList.Count == 0) return;

            //    tempList.Add(tempNodeList);
            //});

            ////NodeList tempList = this._nodeList.ExtractAllNodesThatMatch(new TagNameFilter(tagName), true);

            //if (tempList == null || tempList.Count == 0) return null;

            //return new WinistaHtmlNodeList(tempList);

            return this._nodeManage.Create(this._htmlNodelist.Select(item => item.GetElementByTagName(tagName)));
        }

        public override HtmlNodeList GetElementByClassName(string className)
        {
            ////return this.GetElementByAttrName("class", className);
            //NodeList nodeList = new NodeList();

            //this._htmlNodelist.ForEach(item =>
            //{
            //    var tempNodeList = this._nodeManage.Util.MatchNodeByClassName(className, item.Children);

            //    if (tempNodeList == null || tempNodeList.Count == 0) return;

            //    nodeList.Add(tempNodeList);
            //});

            //if (nodeList == null || nodeList.Count == 0) return null;

            //return new WinistaHtmlNodeList(nodeList);

            return this._nodeManage.Create(this._htmlNodelist.Select(item => item.GetElementByClassName(className)));
        }

        public override HtmlNodeList GetElementByAttrName(string attrName, string value)
        {
            //NodeList tempList = new NodeList();

            //this._htmlNodelist.ForEach(item =>
            //{
            //    if (item.Children == null) return;

            //    var tempNodeList = item.Children.ExtractAllNodesThatMatch(new HasAttributeFilter(attrName, value), true);

            //    if (tempNodeList == null || tempNodeList.Count == 0) return;

            //    tempList.Add(tempNodeList);
            //});

            ////NodeList tempList = this._nodeList.ExtractAllNodesThatMatch(new HasAttributeFilter(attrName, value), true);

            //if (tempList == null || tempList.Count == 0) return null;

            //return new WinistaHtmlNodeList(tempList);

            return this._nodeManage.Create(this._htmlNodelist.Select(item => item.GetElementByAttrName(attrName, value)));
        }

        public override HtmlNodeList GetElementByAttrName(string attrName)
        {
            //NodeList nodeList = new NodeList();

            //this._htmlNodelist.ForEach(item =>
            //{
            //    if (item.Children == null) return;

            //    var tempNodeList = this._nodeManage.Util.ExistNodeByAttrName(attrName, item.Children);

            //    if (tempNodeList == null || tempNodeList.Count == 0) return;

            //    nodeList.Add(tempNodeList);
            //});

            //if (nodeList == null || nodeList.Count == 0) return null;


            ////NodeList tempList = WinistaUtil.ExistNodeByAttrName(attrName, this._nodeList);

            ////if (tempList == null || tempList.Count == 0) return null;

            //return new WinistaHtmlNodeList(nodeList);

            return this._nodeManage.Create(this._htmlNodelist.Select(item => item.GetElementByAttrName(attrName)));
        }

        public override List<HtmlNode> ToList()
        {
            //List<HtmlNode> list = new List<HtmlNode>();

            //this._htmlNodelist.ForEach(item => { list.Add(new WinistaHtmlNode(item)); });

            //return list;

            return this._htmlNodelist;
        }

        public override HtmlNode Item(int index)
        {
            if (index < 0) return null;
            if (index >= this._htmlNodelist.Count) return null;

            //return new WinistaHtmlNode(this._htmlNodelist[index]);

            return this._htmlNodelist[index];
        }

        public override HtmlNodeList Filter(Func<HtmlNode, bool> Match)
        {
            //INode[] nodes = this._htmlNodelist.Where(item => Match(new WinistaHtmlNode(item))).ToArray();

            //if (nodes.Length == 0) return null;

            //return new WinistaHtmlNodeList(nodes);

            return this._nodeManage.Create(this._htmlNodelist.Where(Item => Match(Item)));
        }

        public override HtmlNodeList Filter(Func<HtmlNode, int, bool> Match)
        {
            //int index = 0;

            //INode[] nodes = this._htmlNodelist.Where(item =>
            //{
            //    var result = Match(new WinistaHtmlNode(item), index);
            //    index++;

            //    return result;
            //}).ToArray();

            //if (nodes.Length == 0) return null;

            //return new WinistaHtmlNodeList(nodes);

            int index = 0;

            return this._nodeManage.Create(this._htmlNodelist.Where(item => {
                var result = Match(item, index);

                index++;

                return result;
            }));

        }

        public override HtmlNodeList Filter(string cssSelector)
        {            
            return new CssSelector(this).Filter(cssSelector);
        }

        public override HtmlNodeList ForEach(Action<HtmlNode> Action)
        {
            //this._htmlNodelist.ForEach(item => { Action(new WinistaHtmlNode(item)); });
            this._htmlNodelist.ForEach(item => { Action(item); });

            return this;
        }

        public override HtmlNodeList ForEach(Action<HtmlNode, int> Action)
        {
            int index = 0;

            this._htmlNodelist.ForEach(item =>
            {
                //Action(new WinistaHtmlNode(item), index);
                Action(item, index);
                index++;
            });

            return this;
        }

        public override HtmlNodeList Combine(HtmlNode node)
        {
            if (node == null) return this;

            //List<INode> tempNodeList = new List<INode>();
            //tempNodeList.AddRange(this._htmlNodelist);
            //tempNodeList.Add(((WinistaHtmlNode)node).CoreNode);

            //return new WinistaHtmlNodeList(tempNodeList.ToArray());

            List<HtmlNode> tempNodeList = new List<HtmlNode>();
            tempNodeList.AddRange(this._htmlNodelist);
            tempNodeList.Add(node);

            return this._nodeManage.Create(tempNodeList);
        }

        public override HtmlNodeList Combine(HtmlNodeList nodeList)
        {
            if (nodeList == null) return this;

            //List<INode> tempNodeList = new List<INode>();
            //tempNodeList.AddRange(this._htmlNodelist);
            //tempNodeList.AddRange(((WinistaHtmlNodeList)nodeList).CoreNodeList);

            //return new WinistaHtmlNodeList(tempNodeList.ToArray());

            List<HtmlNode> tempNodeList = new List<HtmlNode>();
            tempNodeList.AddRange(this._htmlNodelist);
            tempNodeList.AddRange(nodeList.ToList());

            return this._nodeManage.Create(tempNodeList);
        }

        public override HtmlNodeList Find(string cssSelector)
        {            
            return new CssSelector(this).Find(cssSelector);
        }

        public override HtmlNodeList Remove(HtmlNode node)
        {
            this._nodeManage.RemoveNode(node);
            this._htmlNodelist.Remove(node);

            if (node.ParentNode != null) node.ParentNode.RemoveChild(node);

            if (this._htmlNodelist.Count == 0) return null;

            return this;
        }

        public override HtmlNodeList Remove(HtmlNodeList nodeList)
        {
            nodeList.ForEach(item => { this.Remove(item); });

            if (this._htmlNodelist.Count == 0) return null;

            return this;
        }

        public override HtmlNodeList RemoveAll()
        {
            this._nodeManage.RemoveNode(this);

            this._htmlNodelist.ForEach(item => {
                if (item.ParentNode != null) item.ParentNode.RemoveChild(item);
            });

            return null;
        }
    }
}
