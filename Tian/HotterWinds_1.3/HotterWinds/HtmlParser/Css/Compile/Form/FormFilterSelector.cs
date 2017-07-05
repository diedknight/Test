using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile.Form
{
    public class FormFilterSelector:AbsCompile
    {
        public override bool Vaild(Syntax.AbsSyntax token)
        {
            if (!(token is NotValueFilterSyntax)) return false;

            List<string> list = new List<string>();
            list.Add("button");
            list.Add("checkbox");
            list.Add("checked");
            list.Add("disabled");
            list.Add("enabled");
            list.Add("file");
            list.Add("image");
            list.Add("input");
            list.Add("password");
            list.Add("radio");
            list.Add("reset");
            list.Add("selected");
            list.Add("submit");
            list.Add("text");

            return list.Contains(((NotValueFilterSyntax)token).FilterName);
        }

        public override HtmlNodeList Run(HtmlNodeList nodeList, Tokens<Syntax.AbsSyntax> tokens)
        {
            NotValueFilterSyntax syntax = tokens.ReadAndMoveNext() as NotValueFilterSyntax;
            tokens.Commit();    

            switch (syntax.FilterName)
            {
                case "button": return nodeList.Filter(item => item.NodeName == "button" || item.GetAttr("type") == "button");
                case "checkbox": return nodeList.Filter(item => item.GetAttr("type") == "checkbox");
                case "checked": return nodeList.Filter(item => item.GetAttr("checked") != null);
                case "disabled": return nodeList.Filter(item => item.GetAttr("disabled") != null);
                case "enabled": return nodeList.Filter(item => item.GetAttr("disabled") == null);
                case "file": return nodeList.Filter(item => item.GetAttr("type") == "file");
                case "image": return nodeList.Filter(item => item.GetAttr("type") == "image");
                case "input": return nodeList.Filter(item => item.NodeName == "input");
                case "password": return nodeList.Filter(item => item.GetAttr("type") == "password");
                case "radio": return nodeList.Filter(item => item.GetAttr("type") == "radio");
                case "reset": return nodeList.Filter(item => item.GetAttr("type") == "reset");
                case "selected": return nodeList.Filter(item => item.GetAttr("type") == "selected");
                case "submit": return nodeList.Filter(item => item.GetAttr("type") == "submit");
                case "text": return nodeList.Filter(item => item.GetAttr("type") == "text");
            }

            return null;
        }

        public override HtmlNodeList FilterRun(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens)
        {
            return this.Run(nodeList, tokens);
        }
    }
}
