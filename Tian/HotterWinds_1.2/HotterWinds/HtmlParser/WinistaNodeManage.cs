using Pricealyser.Crawler.HtmlParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Nodes;
using Winista.Text.HtmlParser.Util;

namespace Pricealyser.Crawler.NewZealand.HtmlParser
{
    public class WinistaNodeManage
    {
        private HashSet<INode> _removedNodeSet = null;

        public WinistaUtil Util { get; private set; }

        public WinistaNodeManage()
        {
            this.Util = new WinistaUtil(this);
            this._removedNodeSet = new HashSet<INode>();
        }

        public HtmlNode Create(INode node)
        {
            if (node == null) return null;

            return new WinistaHtmlNode(node, this);
        }

        public HtmlNodeList Create(IEnumerable<INode> nodeList)
        {
            if (nodeList == null) return null;

            var nodes = this.Util.ClearUselessNode(nodeList);

            if (nodes.Count() == 0) return null;

            return new WinistaHtmlNodeList(this, nodes);            
        }

        public HtmlNodeList Create(IEnumerable<HtmlNode> nodeList)
        {
            if (nodeList == null) return null;

            List<INode> list = new List<INode>();

            foreach (HtmlNode node in nodeList)
            {
                if (node == null) continue;

                list.Add(((WinistaHtmlNode)node).CoreNode);
            }

            return this.Create(list);
        }
      
        public HtmlNodeList Create(NodeList nodeList)
        {
            if (nodeList == null) return null;

            var nodes = this.Util.ClearUselessNode(nodeList);

            if (nodes.Count() == 0) return null;

            return new WinistaHtmlNodeList(this, nodes);     
        }

        public HtmlNodeList Create(IEnumerable<NodeList> nodeList)
        {
            if (nodeList == null) return null;

            NodeList tempList = new NodeList();

            foreach (NodeList list in nodeList)
            {
                if (list == null || list.Count == 0) continue;

                tempList.Add(list);
            }

            return this.Create(tempList);
        }

        public HtmlNodeList Create(IEnumerable<HtmlNodeList> nodeList)
        {
            if (nodeList == null) return null;

            List<INode> list = new List<INode>();

            foreach (HtmlNodeList nodes in nodeList)
            {
                if (nodes == null) continue;

                list.AddRange(((WinistaHtmlNodeList)nodes).CoreNodeList);
            }

            return this.Create(list);
        }

        public bool NodeRemoved(HtmlNode node)
        {
            return this.NodeRemoved(((WinistaHtmlNode)node).CoreNode);
        }

        public bool NodeRemoved(INode node)
        {
            return this._removedNodeSet.Contains(node);
        }

        public void RemoveNode(INode node)
        {
            if (this._removedNodeSet.Contains(node)) return;

            this._removedNodeSet.Add(node);
        }       

        public void RemoveNode(HtmlNode node)
        {
            this.RemoveNode(((WinistaHtmlNode)node).CoreNode);
        }

        public void RemoveNode(HtmlNodeList nodeList)
        {
            ((WinistaHtmlNodeList)nodeList).ForEach(item => this.RemoveNode(item));
        }

    }
}
