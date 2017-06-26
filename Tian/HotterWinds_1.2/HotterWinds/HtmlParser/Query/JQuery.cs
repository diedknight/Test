using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Query
{
    public class JQuery
    {
        private HtmlNodeList _nodeList = null;

        public JQuery() { }

        public JQuery(HtmlNodeList nodeList)
        {
            if (nodeList == null) return;

            this._nodeList = nodeList.Filter(item => item.NodeName != "");
        }

        public JQuery(HtmlNode node)
        {            
            if (node == null) return;

            this._nodeList = node.Combine().Filter(item => item.NodeName != "");
        }

        public JQuery(string html, string url = "")
        {
            if (string.IsNullOrEmpty(html)) return;

            HtmlDocument doc = HtmlDocFactory.Create();
            doc.Load(html, url);

            this._nodeList = doc;
            if (this._nodeList == null) return;

            this._nodeList = this._nodeList.Filter(item => item.NodeName != "");
        }

        public int length
        {
            get
            {
                if (this._nodeList == null) return 0;

                return this._nodeList.Count;
            }
        }

        public HtmlNodeList ToHtmlNodeList()
        {
            return this._nodeList;
        }

        public JQuery each(Action<HtmlNode> Action)
        {
            if (this._nodeList != null) this._nodeList.ForEach(Action);
           
            return this;
        }

        public JQuery each(Action<HtmlNode, int> Action)
        {
            if (this._nodeList != null) this._nodeList.ForEach(Action);

            return this;
        }

        public HtmlNode get(int index)
        {
            if (this._nodeList == null) return null;

            index = index >= 0 ? index : this._nodeList.Count + index;

            return this._nodeList.Item(index);
        }

        public JQuery children()
        {
            if (this._nodeList == null) return new JQuery();
            if (this._nodeList.ChildNodes == null) return new JQuery();

            return new JQuery(this._nodeList.ChildNodes.Filter(item => item.NodeName != ""));
        }

        public JQuery children(string selector)
        {
            if (this._nodeList == null) return this;
            if (this._nodeList.ChildNodes == null) return this;

            return new JQuery(this._nodeList.ChildNodes.Filter(selector));
        }

        public JQuery eq(int index)
        {
            if (this._nodeList == null) return this;

            index = index >= 0 ? index : this._nodeList.Count + index;

            return new JQuery(this._nodeList.Item(index));
        }

        public JQuery filter(string selector)
        {
            if (this._nodeList == null) return this;

            return new JQuery(this._nodeList.Filter(selector));
        }

        public JQuery filter(Func<HtmlNode, bool> Match)
        {
            if (this._nodeList == null) return this;

            return new JQuery(this._nodeList.Filter(Match));
        }

        public JQuery filter(Func<HtmlNode, int, bool> Match)
        {
            if (this._nodeList == null) return this;

            return new JQuery(this._nodeList.Filter(Match));
        }

        public JQuery find(string selector)
        {
            if (this._nodeList == null) return this;

            return new JQuery(this._nodeList.Find(selector));
        }

        public JQuery first()
        {
            if (this._nodeList == null) return this;

            return new JQuery(this._nodeList.Item(0));
        }

        public JQuery has(HtmlNode node)
        {
            if (this._nodeList == null) return this;
            if (node == null) return new JQuery();

            return this.has(node.Combine());
        }

        public JQuery has(HtmlNodeList nodeList)
        {
            if (this._nodeList == null) return this;
            if (nodeList == null) return new JQuery();

            Stack<HtmlNode> stack = new Stack<HtmlNode>();
            var compareList = nodeList.ToList();

            return new JQuery(this._nodeList.Filter(item =>
            {
                var nodes = item.ChildNodes;

                while (nodes != null)
                {
                    if (nodes.Filter(node => node.NodeName != "" && compareList.Contains(node)) != null) return true;

                    nodes = nodes.ChildNodes;
                }

                return false;
            }));
        }

        public JQuery has(string selector)
        {
            if (this._nodeList == null) return this;
           
            return new JQuery(this._nodeList.Find(":has(" + selector + ")"));
        }

        public bool Is(HtmlNode node)
        {
            if (this._nodeList == null) return false;
            if (node == null) return false;

            return this.Is(node.Combine());
        }

        public bool Is(HtmlNodeList nodeList)
        {
            if (this._nodeList == null) return false;
            if (nodeList == null) return false;

            var list = nodeList.ToList();

            return this._nodeList.Filter(item => list.Contains(item)) != null;
        }

        public bool Is(string selector)
        {
            if (this._nodeList == null) return false;            

            return this._nodeList.Filter(selector) != null;
        }

        public JQuery last()
        { 
            if (this._nodeList == null) return this;

            return new JQuery(this._nodeList.Item(this._nodeList.Count - 1));
        }

        public JQuery next()
        {
            if (this._nodeList == null) return this;

            HtmlNodeList resultNodes = null;
            
            this._nodeList.ForEach(item => {
                
                while (true)
                {
                    HtmlNode node = item.NextSibling;

                    if (node == null) break;
                    if (node.NodeName == "") continue;

                    resultNodes = resultNodes == null ? node.Combine() : resultNodes.Combine(node);
                    return;
                }
            });

            return new JQuery(resultNodes);
        }

        public JQuery next(string selector)
        {
            if (this._nodeList == null) return this;

            var query = this.next();
            if (query.length == 0) return query;

            return query.filter(selector);
        }

        public JQuery nextAll()
        {
            if (this._nodeList == null) return this;

            HtmlNodeList resultNodes = null;

            this._nodeList.ForEach(item =>
            {
                HtmlNode node = item;

                while (true)
                {
                    node = node.NextSibling;

                    if (node == null) break;
                    if (node.NodeName == "") continue;

                    resultNodes = resultNodes == null ? node.Combine() : resultNodes.Combine(node);
                }
            });

            return new JQuery(resultNodes);
        }

        public JQuery nextAll(string selector)
        {
            if (this._nodeList == null) return this;

            var query = this.nextAll();
            if (query.length == 0) return query;

            return query.filter(selector);
        }

        public JQuery nextUntil(string selector)
        {
            if (this._nodeList == null) return this;

            HtmlNodeList list = null;

            this._nodeList.ForEach(item =>
            {
                var query = item.ToJQuery().nextAll();
                var filterQuery = query.filter(selector);

                if (filterQuery.length != 0)
                {
                    var filterQuery2List = filterQuery.ToHtmlNodeList().ToList();

                    foreach (var a in query.ToHtmlNodeList().ToList())
                    {
                        if (filterQuery2List.Contains(a)) break;

                        list = list == null ? a.Combine() : list.Combine(a);
                    }
                }
                else
                {
                    list = list == null ? query.ToHtmlNodeList() : list.Combine(query.ToHtmlNodeList());
                }
            });

            return new JQuery(list);
        }

        public JQuery not(string selector)
        {
            if (this._nodeList == null) return this;

            var nodes = this._nodeList.Filter(selector);

            if (nodes == null) return this;

            Dictionary<HtmlNode, bool> dic = new Dictionary<HtmlNode, bool>();
            nodes.ForEach(item => dic.Add(item, true));

            return new JQuery(this._nodeList.Filter(item => !dic.ContainsKey(item)));
        }

        public JQuery parent()
        {
            if (this._nodeList == null) return this;

            return new JQuery(this._nodeList.ParentNode);
        }

        public JQuery parent(string selector)
        {
            if (this._nodeList == null) return this;

            var query = this.parent();
            if (query.length == 0) return query;

            return query.filter(selector);
        }

        public JQuery parents()
        {
            if (this._nodeList == null) return this;

            HtmlNodeList list = null;
            Queue<HtmlNode> queue = new Queue<HtmlNode>();

            this._nodeList.ForEach(item => { queue.Enqueue(item); });

            while (queue.Count > 0)
            {
                var node = queue.Dequeue().ParentNode;
                if (node == null) continue;

                list = list == null ? node.Combine() : list.Combine(node);

                queue.Enqueue(node);
            }

            return new JQuery(list);
        }

        public JQuery parents(string selector)
        {
            if (this._nodeList == null) return this;

            var query = this.parents();
            if (query.length == 0) return query;

            return query.filter(selector);
        }

        public JQuery parentsUntil(string selector)
        {
            if (this._nodeList == null) return this;

            HtmlNodeList list = null;

            this._nodeList.ForEach(item =>
            {
                var query = item.ToJQuery().parents();
                var filterQuery = query.filter(selector);

                if (filterQuery.length != 0)
                {
                    var filterQuery2List = filterQuery.ToHtmlNodeList().ToList();

                    foreach (var a in query.ToHtmlNodeList().ToList())
                    {
                        if (filterQuery2List.Contains(a)) break;

                        list = list == null ? a.Combine() : list.Combine(a);
                    }
                }
                else
                {
                    list = list == null ? query.ToHtmlNodeList() : list.Combine(query.ToHtmlNodeList());
                }
            });

            return new JQuery(list);
        }

        public JQuery prev()
        {
            if (this._nodeList == null) return this;

            HtmlNodeList resultNodes = null;

            this._nodeList.ForEach(item =>
            {

                while (true)
                {
                    HtmlNode node = item.PreviousSibling;

                    if (node == null) break;
                    if (node.NodeName == "") continue;

                    resultNodes = resultNodes == null ? node.Combine() : resultNodes.Combine(node);
                    return;
                }
            });

            return new JQuery(resultNodes);
        }

        public JQuery prev(string selector)
        {
            if (this._nodeList == null) return this;

            var query = this.prev();
            if (query.length == 0) return query;

            return query.filter(selector);
        }

        public JQuery prevAll()
        {
            if (this._nodeList == null) return this;

            HtmlNodeList resultNodes = null;

            this._nodeList.ForEach(item =>
            {
                HtmlNode node = item.PreviousSibling;

                while (true)
                {
                    node = node.PreviousSibling;

                    if (node == null) break;
                    if (node.NodeName == "") continue;

                    resultNodes = resultNodes == null ? node.Combine() : resultNodes.Combine(node);
                }
            });

            return new JQuery(resultNodes);
        }

        public JQuery prevAll(string selector)
        {
            if (this._nodeList == null) return this;

            var query = this.prevAll();
            if (query.length == 0) return query;

            return query.filter(selector);
        }

        public JQuery prevUntil(string selector)
        {
            if (this._nodeList == null) return this;

            HtmlNodeList list = null;

            this._nodeList.ForEach(item =>
            {
                var query = item.ToJQuery().prevAll();
                var filterQuery = query.filter(selector);

                if (filterQuery.length != 0)
                {
                    var filterQuery2List = filterQuery.ToHtmlNodeList().ToList();

                    foreach (var a in query.ToHtmlNodeList().ToList())
                    {
                        if (filterQuery2List.Contains(a)) break;

                        list = list == null ? a.Combine() : list.Combine(a);
                    }
                }
                else
                {
                    list = list == null ? query.ToHtmlNodeList() : list.Combine(query.ToHtmlNodeList());
                }
            });

            return new JQuery(list);
        }

        public JQuery siblings()
        {
            if (this._nodeList == null) return this;

            HtmlNodeList nodes = null;

            this._nodeList.ForEach(item => {
                var prevQuery = item.ToJQuery().prevAll();
                var nextQuery = item.ToJQuery().nextAll();

                Stack<HtmlNode> stack = new Stack<HtmlNode>();
                prevQuery.each(node => { stack.Push(node); });

                while (stack.Count > 0)
                {
                    nodes = nodes == null ? stack.Pop().Combine() : nodes.Combine(stack.Pop());
                }

                nodes = nodes == null ? nextQuery.ToHtmlNodeList() : nodes.Combine(nextQuery.ToHtmlNodeList());
            });

            return new JQuery(nodes);
        }

        public JQuery siblings(string selector)
        {
            if (this._nodeList == null) return this;

            var query = this.siblings();
            if (query.length == 0) return query;

            return query.filter(selector);
        }

        public JQuery slice(int startIndex)
        {
            if (this._nodeList == null) return this;

            startIndex = startIndex >= 0 ? startIndex : this._nodeList.Count + startIndex;

            return new JQuery(this._nodeList.Filter((item, index) => index >= startIndex));
        }

        public JQuery slice(int startIndex, int endIndex)
        {
            if (this._nodeList == null) return this;

            startIndex = startIndex >= 0 ? startIndex : this._nodeList.Count + startIndex;
            endIndex = endIndex >= 0 ? endIndex : this._nodeList.Count + endIndex;

            if (startIndex >= endIndex) return new JQuery();

            return new JQuery(this._nodeList.Filter((item, index) => index >= startIndex && index < endIndex));
        }

        public string attr(string attrName)
        {
            if (this._nodeList == null) return "";

            return this._nodeList.Item(0).GetAttr(attrName);
        }

        public JQuery clone()
        {
            return new JQuery(this._nodeList);
        }

        public bool hasClass(string className)
        {
            if (this._nodeList == null) return false;
            if (string.IsNullOrEmpty(className)) return true;

            return this._nodeList.Filter(item => HtmlUtil.IsCssClassMatch(className, item.GetAttr("class"))) != null;
        }

        public string text()
        {
            if (this._nodeList == null) return "";

            return this._nodeList.InnerText;
        }

        public string html()
        {
            if (this._nodeList == null) return "";

            return this._nodeList.Item(0).InnerHtml;
        }

        public string val()
        {
            if (this._nodeList == null) return "";

            return this._nodeList.Item(0).Value;
        }

        public string getLink()
        {
            if (this._nodeList == null) return "";

            return this._nodeList.Item(0).GetLink();
        }

        public JQuery remove()
        {
            if (this._nodeList == null) return this;

            this._nodeList = this._nodeList.RemoveAll();

            return this;
        }

        public JQuery remove(string selector)
        {
            if (this._nodeList == null) return this;

            this.filter(selector).each(item => { this._nodeList = this._nodeList.Remove(item); });

            return this;
        }

        public JQuery empty()
        {
            if (this._nodeList == null) return this;
            if (this._nodeList.ChildNodes == null) return this;

            this._nodeList.ChildNodes.RemoveAll();

            return this;
        }


    }
}
