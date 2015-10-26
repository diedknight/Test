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
        public static bool IsTextNodeWhiteSpace(INode node)
        {
            if (node == null) return false;
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
        public static NodeList MatchNodeByClassName(string className, INode node)
        {
            if (node == null) return null;
            if (!(node is TagNode)) return null;

            NodeList nodeList = new NodeList();
            Queue<HtmlNode> queue = new Queue<HtmlNode>();

            queue.Enqueue(new WinistaHtmlNode(node));

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

        public static NodeList MatchNodeByClassName(string className, NodeList nodeList)
        {
            if (nodeList == null) return null;
            if (nodeList.Count == 0) return new NodeList();

            NodeList list = new NodeList();

            INodeIterator nodeIterator = nodeList.Elements();
            while (nodeIterator.HasMoreNodes())
            {
                INode tempNode = nodeIterator.NextNode();

                if (IsTextNodeWhiteSpace(tempNode)) continue;

                NodeList tempNodeList = MatchNodeByClassName(className, tempNode);

                if (tempNodeList == null || tempNodeList.Count == 0) continue;

                list.Add(tempNodeList);
            }

            return list;
        }


        public static INode[] ClearRepeatAndNullNode(INode[] nodes)
        {
            if (nodes == null) return null;
            if (nodes.Length == 0) return new INode[0];

            List<INode> tempList = new List<INode>();
            Dictionary<INode, bool> dic = new Dictionary<INode, bool>();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] == null) continue;
                if (IsTextNodeWhiteSpace(nodes[i])) continue;

                if (dic.ContainsKey(nodes[i])) continue;

                dic.Add(nodes[i], true);
                tempList.Add(nodes[i]);
            }

            return tempList.ToArray();
        }

        public static NodeList ClearRepeatAndNullNode(NodeList nodeList)
        {
            if (nodeList == null) return null;
            if (nodeList.Count == 0) return new NodeList();

            NodeList list = new NodeList();
            Dictionary<INode, bool> dic = new Dictionary<INode, bool>();

            INodeIterator nodeIterator = nodeList.Elements();
            while (nodeIterator.HasMoreNodes())
            {
                INode tempNode = nodeIterator.NextNode();

                if (IsTextNodeWhiteSpace(tempNode)) continue;

                if (dic.ContainsKey(tempNode)) continue;

                dic.Add(tempNode, true);
                list.Add(tempNode);
            }

            return list;
        }

        public static NodeList ExistNodeByAttrName(string attrName, INode node)
        {
            if (node == null) return null;
            if (!(node is TagNode)) return null;

            NodeList nodeList = new NodeList();
            Queue<HtmlNode> queue = new Queue<HtmlNode>();

            queue.Enqueue(new WinistaHtmlNode(node));

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

        public static NodeList ExistNodeByAttrName(string attrName, NodeList nodeList)
        {
            if (nodeList == null) return null;
            if (nodeList.Count == 0) return new NodeList();

            NodeList list = new NodeList();

            INodeIterator nodeIterator = nodeList.Elements();
            while (nodeIterator.HasMoreNodes())
            {
                INode tempNode = nodeIterator.NextNode();

                if (IsTextNodeWhiteSpace(tempNode)) continue;

                NodeList tempNodeList = ExistNodeByAttrName(attrName, tempNode);

                if (tempNodeList == null || tempNodeList.Count == 0) continue;

                list.Add(tempNodeList);
            }

            return list;
        }

        public static NodeList ExistNodeByAttrName(string attrName, INode[] nodeList)
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
