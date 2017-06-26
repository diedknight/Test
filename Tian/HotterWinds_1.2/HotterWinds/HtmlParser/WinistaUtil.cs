using Pricealyser.Crawler.HtmlParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Nodes;
using Winista.Text.HtmlParser.Util;

namespace Pricealyser.Crawler.NewZealand.HtmlParser
{
    public class WinistaUtil
    {
        private WinistaNodeManage _nodeManage = null;

        public WinistaUtil(WinistaNodeManage nodeManage)
        {
            this._nodeManage = nodeManage;
        }


        public bool IsTextNodeWhiteSpace(INode node)
        {
            //if (node == null) return false;
            if (node is RemarkNode) return true;

            if (node is TextNode)
            {
                TextNode tempTextNode = (TextNode)node;

                //if (tempTextNode.WhiteSpace) return true;
                if (tempTextNode.GetText().Trim().Equals("")) return true;
            }

            return false;
        }
        
        /// <summary>
        /// 递归匹配节点Class属性
        /// </summary>
        /// <param name="node"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public NodeList MatchNodeByClassName(string className, INode node)
        {
            if (node == null) return null;
            if (!(node is TagNode)) return null;

            NodeList nodeList = new NodeList();
            Queue<HtmlNode> queue = new Queue<HtmlNode>();

            queue.Enqueue(this._nodeManage.Create(node));

            while (queue.Count > 0)
            {
                HtmlNode curNode = queue.Dequeue();

                //匹配ClassName
                if (HtmlUtil.IsCssClassMatch(className, curNode.GetAttr("class")))
                {
                    nodeList.Add(((WinistaHtmlNode)curNode).CoreNode);
                }

                if (curNode.ChildNodes == null) continue;

                //将子节点压入队列
                curNode.ChildNodes.ForEach(item => { queue.Enqueue(item); });
            }

            return nodeList;
        }

        public NodeList MatchNodeByClassName(string className, NodeList nodeList)
        {
            if (nodeList == null) return null;
            if (nodeList.Count == 0) return new NodeList();

            NodeList list = new NodeList();

            INodeIterator nodeIterator = nodeList.Elements();
            while (nodeIterator.HasMoreNodes())
            {
                INode tempNode = nodeIterator.NextNode();

                if (tempNode == null) continue;
                if (IsTextNodeWhiteSpace(tempNode)) continue;

                NodeList tempNodeList = MatchNodeByClassName(className, tempNode);

                if (tempNodeList == null || tempNodeList.Count == 0) continue;

                list.Add(tempNodeList);
            }

            return list;
        }

        /// <summary>
        /// Removed, WhiteSpace, Null Node
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public bool IsUselessNode(INode node)
        {
            if (node == null) return true;
            if (IsTextNodeWhiteSpace(node)) return true;
            if (this._nodeManage.NodeRemoved(node)) return true;

            return false;
        }

        /// <summary>
        /// Removed, Repeat, WhiteSpace, Null Node
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public IEnumerable<INode> ClearUselessNode(IEnumerable<INode> nodes)
        {
            //if (nodes == null) return null;            

            List<INode> tempList = new List<INode>();
            HashSet<INode> set = new HashSet<INode>();

            foreach (INode node in nodes)
            {
                if (node == null) continue;
                if (set.Contains(node)) continue;
                if (IsTextNodeWhiteSpace(node)) continue;
                if (this._nodeManage.NodeRemoved(node)) continue;

                set.Add(node);
                tempList.Add(node);
            }

            return tempList;
        }

        public IEnumerable<INode> ClearUselessNode(NodeList nodeList)
        {
            //if (nodeList == null) return null;            

            List<INode> tempList = new List<INode>();         
            HashSet<INode> set = new HashSet<INode>();

            INodeIterator nodeIterator = nodeList.Elements();
            while (nodeIterator.HasMoreNodes())
            {
                INode tempNode = nodeIterator.NextNode();

                if (tempNode == null) continue;
                if (set.Contains(tempNode)) continue;
                if (IsTextNodeWhiteSpace(tempNode)) continue;
                if (this._nodeManage.NodeRemoved(tempNode)) continue;

                set.Add(tempNode);
                tempList.Add(tempNode);
            }

            return tempList;
        }

        public NodeList ExistNodeByAttrName(string attrName, INode node)
        {
            if (node == null) return null;
            if (!(node is TagNode)) return null;

            NodeList nodeList = new NodeList();
            Queue<HtmlNode> queue = new Queue<HtmlNode>();

            queue.Enqueue(this._nodeManage.Create(node));

            while (queue.Count > 0)
            {
                HtmlNode curNode = queue.Dequeue();

                //匹配AttrName
                if (curNode.GetAttr(attrName) != null)
                {
                    nodeList.Add(((WinistaHtmlNode)curNode).CoreNode);
                }

                if (curNode.ChildNodes == null) continue;

                //将子节点压入队列
                curNode.ChildNodes.ForEach(item => { queue.Enqueue(item); });
            }

            return nodeList;
        }

        public NodeList ExistNodeByAttrName(string attrName, NodeList nodeList)
        {
            if (nodeList == null) return null;
            if (nodeList.Count == 0) return new NodeList();

            NodeList list = new NodeList();

            INodeIterator nodeIterator = nodeList.Elements();
            while (nodeIterator.HasMoreNodes())
            {
                INode tempNode = nodeIterator.NextNode();

                if (tempNode == null) continue;
                if (IsTextNodeWhiteSpace(tempNode)) continue;

                NodeList tempNodeList = ExistNodeByAttrName(attrName, tempNode);

                if (tempNodeList == null || tempNodeList.Count == 0) continue;

                list.Add(tempNodeList);
            }

            return list;
        }

        public NodeList ExistNodeByAttrName(string attrName, INode[] nodeList)
        {
            if (nodeList == null) return null;

            NodeList list = new NodeList();

            for (int i = 0; i < nodeList.Length; i++)
            {
                NodeList tempList = ExistNodeByAttrName(attrName, nodeList[i]);

                if (tempList == null && tempList.Count == 0) continue;

                list.Add(tempList);
            }

            return list;
        }

    }
}
