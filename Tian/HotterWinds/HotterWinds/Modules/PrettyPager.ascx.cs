using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;


namespace HotterWinds.Modules
{
    public partial class PrettyPager : System.Web.UI.UserControl
    {
        public int currentPage = 0;
        public int totalPages = 0;
        public int displayPages = 5;//要在页面上显示多少个页码
        public Dictionary<string, string> ps;
        public PriceMe.PageName PageTo;
        public int PageSize;
        public int ItemCount;

        protected bool showPreview = false;
        protected bool showNext = false;
        protected bool showFirst = false;
        protected bool showLast = false;
        protected int startPageIndex = 1;
        protected int endPageIndex = 0;

        public bool IsDisplay = true;
        public bool DisplayItemCountInfo = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsDisplay) return;
            if (currentPage < 1) currentPage = 1;

            if (currentPage > displayPages / 2)
            {
                startPageIndex = currentPage - (displayPages / 2);
            }
            else
            {
                startPageIndex = 1;
            }

            if (totalPages - startPageIndex > (displayPages - 1))
            {
                endPageIndex = startPageIndex + (displayPages - 1);
            }
            else
            {
                endPageIndex = totalPages;
            }

            if (totalPages > 1)
            {
                if (currentPage != 1)
                {
                    showPreview = true;
                    showFirst = true;
                }
                if (currentPage != totalPages)
                {
                    showNext = true;
                    showLast = true;
                }
            }
        }

        public void SetPaginationHeader()
        {
            Dictionary<string, string> _ps = new Dictionary<string, string>(ps);
            if (_ps.ContainsKey("v"))
                _ps.Remove("v");
            if (_ps.ContainsKey("sb"))
                _ps.Remove("sb");
            string prevURL = "";
            if (currentPage > 1)
            {
                if (_ps.ContainsKey("pg"))
                {
                    _ps.Remove("pg");
                }
                _ps.Add("pg", (currentPage - 1).ToString());
                prevURL = PriceMe.UrlController.GetRewriterUrl(PageTo, _ps);
            }
            string nextURL = "";
            int cp = currentPage;
            if (cp == 0)
                cp = 1;
            if (cp < totalPages)
            {
                _ps.Remove("pg");
                _ps.Add("pg", (cp + 1).ToString());
                nextURL = PriceMe.UrlController.GetRewriterUrl(PageTo, _ps);
            }
            DynamicHtmlHeader.SetPaginationHeader(prevURL, nextURL, this.Page);
        }
    }
}