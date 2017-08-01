using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for RespondJSTag
/// </summary>

namespace PriceMe
{
    public class RespondJSTag : HtmlControl
    {
        public RespondJSTag()
            : base("Script")
        {
            this.Attributes.Add("src", "https://oss.maxcdn.com/respond/1.4.2/respond.min.js");
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.Write("</Script>");
            writer.Write("<![endif]-->");
        }
    }
}