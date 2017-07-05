using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Tags;
using Winista.Text.HtmlParser.Util;

namespace Pricealyser.Crawler.NewZealand.HtmlParser.WinistaTags
{
    public class ProxyCompositeTag : CompositeTag
    {
        private WinistaNodeManage _nodeManage = null;

        private CompositeTag _tag = new CompositeTag();                

        public ProxyCompositeTag(CompositeTag tag, WinistaNodeManage nodeManage)
            : base()
        {            
            this._tag = tag;
            this._nodeManage = nodeManage;
        }

        public override void Accept(Winista.Text.HtmlParser.Visitors.NodeVisitor visitor)
        {
            this._tag.Accept(visitor);
        }

        public override System.Collections.Hashtable Attributes
        {
            get
            {
                return this._tag.Attributes;
            }
            set
            {
                this._tag.Attributes = value;
            }
        }

        public override System.Collections.ArrayList AttributesEx
        {
            get
            {
                return this._tag.AttributesEx;
            }
            set
            {
                this._tag.AttributesEx = value;
            }
        }

        public override bool BreaksFlow()
        {
            return this._tag.BreaksFlow();
        }

        public override INode ChildAt(int index)
        {
            return this._tag.ChildAt(index);
        }

        public override int ChildCount
        {
            get
            {
                return this._tag.ChildCount;
            }
        }

        public override NodeList Children
        {
            get
            {
                return this._tag.Children;
            }
            set
            {
                this._tag.Children = value;
            }
        }

        public override INode[] ChildrenAsNodeArray
        {
            get
            {
                return this._tag.ChildrenAsNodeArray;
            }
        }

        public override string ChildrenHTML
        {
            get
            {
                return this._tag.ChildrenHTML;
            }
        }

        public override object Clone()
        {
            return this._tag.Clone();
        }

        public override void CollectInto(NodeList list, NodeFilter filter)
        {
            this._tag.CollectInto(list, filter);
        }

        public override IText[] DigupStringNode(string searchText)
        {
            return this._tag.DigupStringNode(searchText);
        }

        public override void DoSemanticAction()
        {
            this._tag.DoSemanticAction();
        }

        public override ISimpleNodeIterator Elements()
        {
            return this._tag.Elements();
        }

        public override bool EmptyXmlTag
        {
            get
            {
                return this._tag.EmptyXmlTag;
            }
            set
            {
                this._tag.EmptyXmlTag = value;
            }
        }

        public override string[] Enders
        {
            get
            {
                return this._tag.Enders;
            }
        }

        public override int EndingLineNumber
        {
            get
            {
                return this._tag.EndingLineNumber;
            }
        }

        public override int EndPosition
        {
            get
            {
                return this._tag.EndPosition;
            }
            set
            {
                this._tag.EndPosition = value;
            }
        }

        public override string[] EndTagEnders
        {
            get
            {
                return this._tag.EndTagEnders;
            }
        }

        public override bool Equals(object obj)
        {
            return this._tag.Equals(obj);
        }

        public override int FindPositionOf(INode searchNode)
        {
            return this._tag.FindPositionOf(searchNode);
        }

        public override int FindPositionOf(string text)
        {
            return this._tag.FindPositionOf(text);
        }

        public override int FindPositionOf(string text, System.Globalization.CultureInfo locale)
        {
            return this._tag.FindPositionOf(text, locale);
        }

        public override INode FirstChild
        {
            get
            {
                return this._tag.FirstChild;
            }
        }

        public override string GetAttribute(string name)
        {
            return this._tag.GetAttribute(name);
        }

        public override TagAttribute GetAttributeEx(string name)
        {
            return this._tag.GetAttributeEx(name);
        }

        public override INode GetChild(int index)
        {
            return this._tag.GetChild(index);
        }

        public override ITag GetEndTag()
        {
            return this._tag.GetEndTag();
        }

        public override int GetHashCode()
        {
            return this._tag.GetHashCode();
        }

        public override string GetText()
        {
            return this._tag.GetText();
        }

        public override string[] Ids
        {
            get
            {
                return this._tag.Ids;
            }
        }

        public override bool IsEndTag()
        {
            return this._tag.IsEndTag();
        }

        public override INode LastChild
        {
            get
            {
                return this._tag.LastChild;
            }
        }

        public override INode NextSibling
        {
            get
            {
                return this._tag.NextSibling;
            }
        }

        public override Winista.Text.HtmlParser.Lex.Page Page
        {
            get
            {
                return this._tag.Page;
            }
            set
            {
                this._tag.Page = value;
            }
        }

        public override INode Parent
        {
            get
            {
                return this._tag.Parent;
            }
            set
            {
                this._tag.Parent = value;
            }
        }

        public override System.Collections.Hashtable Parsed
        {
            get
            {
                return this._tag.Parsed;
            }
        }

        public override INode PreviousSibling
        {
            get
            {
                return this._tag.PreviousSibling;
            }
        }

        //protected override void PutChildrenInto(StringBuilder sb, bool verbatim)
        //{
        //    base.PutChildrenInto(sb, verbatim);
        //}

        //protected override void PutEndTagInto(StringBuilder sb, bool verbatim)
        //{
        //    base.PutEndTagInto(sb, verbatim);
        //}

        public override string RawTagName
        {
            get
            {
                return this._tag.RawTagName;
            }
        }

        public override void RemoveAttribute(string key)
        {
            this._tag.RemoveAttribute(key);
        }

        public override void RemoveChild(int i)
        {
            this._tag.RemoveChild(i);
        }

        public override ITag SearchByName(string name)
        {
            return this._tag.SearchByName(name);
        }

        public override NodeList SearchFor(string searchString)
        {
            return this._tag.SearchFor(searchString);
        }

        public override NodeList SearchFor(string searchString, bool caseSensitive)
        {
            return this._tag.SearchFor(searchString, caseSensitive);
        }

        public override NodeList SearchFor(string searchString, bool caseSensitive, System.Globalization.CultureInfo locale)
        {
            return this._tag.SearchFor(searchString, caseSensitive, locale);
        }

        public override NodeList SearchFor(Type classType, bool recursive)
        {
            return this._tag.SearchFor(classType, recursive);
        }

        public override void SetAttribute(string key, string value_Renamed)
        {
            this._tag.SetAttribute(key, value_Renamed);
        }

        public override void SetAttribute(string key, string value_Renamed, char quote)
        {
            this._tag.SetAttribute(key, value_Renamed, quote);
        }

        public override void SetAttribute(TagAttribute attribute)
        {
            this._tag.SetAttribute(attribute);
        }

        public override void SetAttributeEx(TagAttribute attribute)
        {
            this._tag.SetAttributeEx(attribute);
        }

        public override void SetEndTag(ITag tag)
        {
            this._tag.SetEndTag(tag);
        }

        public override void SetText(string text)
        {
            this._tag.SetText(text);
        }

        public override int StartingLineNumber
        {
            get
            {
                return this._tag.StartingLineNumber;
            }
        }

        public override int StartPosition
        {
            get
            {
                return this._tag.StartPosition;
            }
            set
            {
                this._tag.StartPosition = value;
            }
        }

        public override string StringText
        {
            get
            {
                return this._tag.StringText;
            }
        }

        public override int TagBegin
        {
            get
            {
                return this._tag.TagBegin;
            }
            set
            {
                this._tag.TagBegin = value;
            }
        }

        public override int TagEnd
        {
            get
            {
                return this._tag.TagEnd;
            }
            set
            {
                this._tag.TagEnd = value;
            }
        }

        public override string TagName
        {
            get
            {
                return this._tag.TagName;
            }
            set
            {
                this._tag.TagName = value;
            }
        }

        public override Winista.Text.HtmlParser.Scanners.IScanner ThisScanner
        {
            get
            {
                return this._tag.ThisScanner;
            }
            set
            {
                this._tag.ThisScanner = value;
            }
        }

        public override string ToHtml()
        {
            return this.ToHtml(true);
        }

        public override string ToHtml(bool verbatim)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(this.BaseToHtml(verbatim));
            if (!this._tag.EmptyXmlTag)
            {
                this.PutChildrenInto(stringBuilder, verbatim);
                if (null != this._tag.GetEndTag())
                {
                    this.PutEndTagInto(stringBuilder, verbatim);
                }
            }
            return stringBuilder.ToString();
        }

        protected override void PutChildrenInto(StringBuilder sb, bool verbatim)
        {
            ISimpleNodeIterator children = this.GetChildren();
            while (children.HasMoreNodes())
            {
                INode node = children.NextNode();                
                if (!verbatim || node.StartPosition != node.EndPosition)
                {
                    sb.Append(node.ToHtml());
                }
            }
        }

        protected override void PutEndTagInto(StringBuilder sb, bool verbatim)
        {
            var mEndTag = this.GetmEndTag();

            if (!verbatim || mEndTag.StartPosition != mEndTag.EndPosition)
            {
                sb.Append(this._tag.GetEndTag().ToHtml());
            }
        }

        public override string ToPlainTextString()
        {
            return this._tag.ToPlainTextString();
        }

        public override string ToString()
        {
            return this._tag.ToString();
        }

        public override void ToString(int level, StringBuilder buffer)
        {
            this._tag.ToString(level, buffer);
        }

        public override Winista.Text.HtmlParser.Util.ISimpleNodeIterator GetChildren()
        {            
            NodeList nodeList = new NodeList();                        
             
            INodeIterator nodeIterator = this._tag.GetChildren();
            while (nodeIterator.HasMoreNodes())
            {
                INode tempNode = nodeIterator.NextNode();

                if (this._nodeManage.Util.IsUselessNode(tempNode)) continue;

                nodeList.Add(tempNode);
            }

            return nodeList.Elements();
        }

        private string BaseToHtml(bool verbatim)
        {
            int num = 2;
            ArrayList attributesEx = this._tag.AttributesEx;
            int count = attributesEx.Count;
            for (int i = 0; i < count; i++)
            {
                TagAttribute tagAttribute = (TagAttribute)attributesEx[i];
                num += tagAttribute.Length;
            }
            StringBuilder stringBuilder = new StringBuilder(num);
            stringBuilder.Append("<");
            for (int i = 0; i < count; i++)
            {
                TagAttribute tagAttribute = (TagAttribute)attributesEx[i];
                tagAttribute.ToString(stringBuilder);
            }
            stringBuilder.Append(">");
            return stringBuilder.ToString();
        }

        private ITag GetmEndTag()
        {
            return this._tag.GetType().GetField("mEndTag", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this._tag) as ITag;
        }

    }
}
