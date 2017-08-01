using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for IE8JqueryTag
/// </summary>

namespace PriceMe
{
    public class IE8JqueryTag : HtmlControl
    {
        public IE8JqueryTag() : base("Script")
        {
            this.Attributes.Add("src", "/Scripts/jquery-1.11.1.min.js?ver=" + PriceMe.WebConfig.WEB_cssVersion);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<!--[if lt IE 9]>");
            base.Render(writer);
            writer.Write("</Script>");
        }
    }
}