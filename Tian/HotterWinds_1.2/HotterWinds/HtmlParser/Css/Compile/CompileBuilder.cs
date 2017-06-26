using Pricealyser.Crawler.HtmlParser.Css.Compile.Attr;
using Pricealyser.Crawler.HtmlParser.Css.Compile.Basic;
using Pricealyser.Crawler.HtmlParser.Css.Compile.BasicFilter;
using Pricealyser.Crawler.HtmlParser.Css.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Compile
{
    public class CompileBuilder
    {
        private static List<AbsCompile> compileList = null;

        static CompileBuilder()
        {
            compileList = new List<AbsCompile>();
            List<Type> typeList = typeof(AbsCompile).Assembly.GetTypes()
                .Where(type =>
                    typeof(AbsCompile).IsAssignableFrom(type)
                    && (!type.IsInterface)
                    && (!type.IsAbstract)).ToList();

            typeList.ForEach(item => { compileList.Add((AbsCompile)Activator.CreateInstance(item)); });
        }

        public static HtmlNodeList Create(HtmlNodeList nodeList, Tokens<AbsSyntax> tokens, bool isFilterAction = false)
        {
            HtmlNodeList tempNodeList = nodeList;

            while (tokens.HaveMoreToken())
            {
                int tempCurIndex = tokens.CurIndex;

                compileList.ForEach(item =>
                {
                    if (tempNodeList != null) tempNodeList = isFilterAction ? item.FilterAction(tempNodeList, tokens) : item.Action(tempNodeList, tokens);
                });
                
                if (tempCurIndex == tokens.CurIndex) throw new Exception("Compile error \"" + tokens.Read(0).ToString() + "\"");

                if (tempNodeList == null) break;
            }

            return tempNodeList;

        }
    }
}
