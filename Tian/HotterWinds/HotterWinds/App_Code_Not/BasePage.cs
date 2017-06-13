using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
    protected override void Render(HtmlTextWriter writer)
    {
        StringWriter responceSW = new StringWriter();
        HtmlTextWriter responceHTW = new HtmlTextWriter(responceSW);
        base.Render(responceHTW);
        string html = responceSW.ToString();
        html = MoveSpecificHiddenFieldsToBottom(html, "__VIEWSTATE", "__EVENTTARGET", "__EVENTARGUMENT");
        writer.Write(html);
    }

    private string MoveSpecificHiddenFieldsToBottom(string html, params string[] name)
    {
        StringBuilder sbPattern = new StringBuilder();
        StringBuilder sbFounded = new StringBuilder();

        foreach (string nm in name)
            sbPattern.Append("<input.*.name=\"" + nm + "\".*/*>|<input.*.name=\"" + nm + "\"*></input>|");
        MatchCollection mc = Regex.Matches(html, sbPattern.ToString().Trim('|'), RegexOptions.IgnoreCase & RegexOptions.IgnorePatternWhitespace);

        foreach (Match m in mc)
        {
            sbFounded.AppendLine(m.Value);
            html = html.Replace(m.Value, string.Empty);
        }
        return html.Replace("</form>", sbFounded.ToString() + "</form>");
    }
}