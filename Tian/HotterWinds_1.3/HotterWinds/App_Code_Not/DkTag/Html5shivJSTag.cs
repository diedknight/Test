using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Html5shivTag
/// </summary>
namespace PriceMe
{
    public class Html5shivJSTag: HtmlControl
    {
        public Html5shivJSTag()
            : base("Script")
        {
            //this.Attributes.Add("src", "http://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js");
            this.Attributes.Add("src", "https://www.jsdelivr.com/html5shiv/3.7.2/html5shiv.min.js");
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.Write("</Script>");
        }
    }
}