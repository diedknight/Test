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

namespace Pricealyser.Crawler.HtmlParser
{
    public abstract class HtmlNodeList : IDomable
    {

        public abstract int Count { get; }

        public abstract HtmlNodeList ParentNode { get; }
        public abstract HtmlNodeList ChildNodes { get; }
        public abstract HtmlNodeList PreviousSibling { get; }
        public abstract HtmlNodeList NextSibling { get; }
        public abstract HtmlNodeList FirstChild { get; }
        public abstract HtmlNodeList LastChild { get; }
        public abstract HtmlNode First { get; }
        public abstract HtmlNode Last { get; }
        public abstract string OuterHtml { get; }
        public abstract string InnerHtml { get; }
        public abstract string InnerText { get; }

        public abstract HtmlNode Item(int index);
        public abstract List<HtmlNode> ToList();
        public abstract HtmlNode GetElementById(string id);
        public abstract HtmlNodeList GetElementByTagName(string tagName);
        public abstract HtmlNodeList GetElementByClassName(string className);
        public abstract HtmlNodeList GetElementByAttrName(string attrName, string value);
        public abstract HtmlNodeList GetElementByAttrName(string attrName);

        /// <summary>
        /// 过滤集合中的节点
        /// </summary>
        /// <param name="Match">匹配表达式</param>
        /// <returns>返回匹配成功的节点集合</returns>
        public abstract HtmlNodeList Filter(Func<HtmlNode, bool> Match);
        public abstract HtmlNodeList Filter(Func<HtmlNode, int, bool> Match);
        public abstract HtmlNodeList Filter(string cssSelector);


        public abstract HtmlNodeList ForEach(Action<HtmlNode> Action);
        public abstract HtmlNodeList ForEach(Action<HtmlNode, int> Action);

        public abstract HtmlNodeList Combine(HtmlNode node);
        public abstract HtmlNodeList Combine(HtmlNodeList nodeList);

        public abstract HtmlNodeList Find(string cssSelector);

        //public static bool operator ==(HtmlNodeList lHtmlNodeList, HtmlNodeList rHtmlNodeList)
        //{
        //    if ((Object)lHtmlNodeList == (Object)rHtmlNodeList)
        //    {
        //        return true;
        //    }

        //    if ((Object)lHtmlNodeList == null || (Object)rHtmlNodeList == null)
        //    {
        //        return false;
        //    }

        //    return lHtmlNodeList.Equals(rHtmlNodeList);
        //}

        //public static bool operator !=(HtmlNodeList lHtmlNodeList, HtmlNodeList rHtmlNodeList)
        //{
        //    return !(lHtmlNodeList == rHtmlNodeList);
        //}


       
    }
}
