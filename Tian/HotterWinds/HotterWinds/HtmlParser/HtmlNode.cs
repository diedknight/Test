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
    public abstract class HtmlNode : IDomable
    {
        public abstract int Index { get; }
        public abstract int IndexOfIgnoreTextNode { get; }
        public abstract int IndexOfType { get; }

        public abstract string NodeName { get; }
        public abstract string Value { get; }
        public abstract HtmlNode ParentNode { get; }
        public abstract HtmlNodeList ChildNodes { get; }
        public abstract HtmlNode PreviousSibling { get; }
        public abstract HtmlNode NextSibling { get; }
        public abstract HtmlNode FirstChild { get; }
        public abstract HtmlNode LastChild { get; }
        public abstract string OuterHtml { get; }
        public abstract string InnerHtml { get; }
        public abstract string InnerText { get; }

        public abstract string GetAttr(string attr);
        public abstract string GetLink();

        public abstract HtmlNode GetElementById(string id);
        public abstract HtmlNodeList GetElementByTagName(string tagName);
        public abstract HtmlNodeList GetElementByClassName(string className);
        public abstract HtmlNodeList GetElementByAttrName(string attrName, string value);
        public abstract HtmlNodeList GetElementByAttrName(string attrName);

        public abstract HtmlNode RemoveChild(HtmlNode node);        

        public abstract HtmlNodeList Combine();
        public abstract HtmlNodeList Combine(HtmlNode node);
        public abstract HtmlNodeList Combine(HtmlNodeList nodeList);

        public abstract HtmlNodeList Find(string cssSelector);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator ==(HtmlNode lHtmlNode, HtmlNode rHtmlNode)
        {
            if ((Object)lHtmlNode == (Object)rHtmlNode)
            {
                return true;
            }

            if ((Object)lHtmlNode == null || (Object)rHtmlNode == null)
            {
                return false;
            }

            return lHtmlNode.Equals(rHtmlNode);
        }

        public static bool operator !=(HtmlNode lHtmlNode, HtmlNode rHtmlNode)
        {
            return !(lHtmlNode == rHtmlNode);
        }

        
    }
}
