using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;

/// <summary>
///BottomViewStatePage 的摘要说明
/// </summary>
//public class BottomViewStatePage : Page
//{
//    static readonly Regex viewStateRegex = new Regex(@"(<input type=""hidden"" name=""__VIEWSTATE"" id=""__VIEWSTATE"" value=""[a-zA-Z0-9\+=\\/]+"" />)", RegexOptions.Multiline | RegexOptions.Compiled);
//    static readonly Regex endFormRegex = new Regex(@"</form>", RegexOptions.Multiline | RegexOptions.Compiled);
//    static readonly Regex spaceRegex = new Regex(@">\s+<", RegexOptions.Multiline | RegexOptions.Compiled);
//    static readonly Regex space2Regex = new Regex(@"\s+", RegexOptions.Multiline | RegexOptions.Compiled);
//    static readonly Regex scriptRegex = new Regex(@"<script src=""[^\s\""]+"" type=""text/javascript""></script>", RegexOptions.Multiline | RegexOptions.Compiled);

//    protected override void Render(HtmlTextWriter writer)
//    {    
//        System.IO.StringWriter stringWriter = new System.IO.StringWriter();  
//        HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);    
//        base.Render(htmlWriter);
//        string html = stringWriter.ToString();
//        Match viewStateMatch = viewStateRegex.Match(html);
//        if (viewStateMatch.Success)
//        {
//            string viewStateString = viewStateMatch.Captures[0].Value;
//            html = html.Remove(viewStateMatch.Index, viewStateMatch.Length);

//            Match endFormMatch = endFormRegex.Match(html, viewStateMatch.Index);
//            if (endFormMatch.Success)
//            {
//                html = html.Insert(endFormMatch.Index, viewStateString);

//                html = spaceRegex.Replace(html, "> <");

//                MatchCollection matches = scriptRegex.Matches(html);
//                foreach (Match m in matches)
//                {
//                    if (m.Value.Contains("main.js") || m.Value.Contains("http://maps.google.com/maps") || m.Value.Contains("jquery.js"))
//                    {
//                        continue;
//                    }
//                    html = html.Replace(m.Value, "");
//                    endFormMatch = endFormRegex.Match(html);
//                    html = html.Insert(endFormMatch.Index, m.Value);
//                }
//            }
//        }
//        writer.Write(html);
//    }
//}