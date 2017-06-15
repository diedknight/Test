<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrettyPager.ascx.cs" Inherits="HotterWinds.Modules.PrettyPager" %>

<%if (!IsDisplay)
        return; %>

<% string showingStr = ""; %>
<div>
    <style type="text/css">
        .pagination li a{
            border:none;
            margin-right:7px;
        }
        
        .pagination > .active a {
            border-top-left-radius: 0px !important;
            border-bottom-left-radius: 0px !important;
            background-color:#1fc0a0;
        }

            .pagination > .active a:hover {
                background-color:#1fc0a0 !important;
            }

    </style>
    <ul class="pagination PrettyPagerDiv">
        <%
            if (totalPages > 1)
            {
                string prevURL = "";
                string nextURL = "";
                if (currentPage > 1)
                {
                    if (ps.ContainsKey("pg"))
                    {
                        ps.Remove("pg");
                    }
                    ps.Add("pg", (currentPage - 1).ToString());
                    prevURL = PriceMe.UrlController.GetRewriterUrl(PageTo, ps);

                    ps.Remove("pg");
                    string firstPageURL = PriceMe.UrlController.GetRewriterUrl(PageTo, ps);
        %>
        <li><a href="<%=prevURL%>" rel="nofollow" class="pagerAtag"><div class="page-separator-prev">«</div></a></li>
        <%
            }
            for (int i = startPageIndex; i <= endPageIndex; i++)
            {
                if (i == currentPage)
                {%>
        <li class="active"><a><%=currentPage.ToString()%></a></li>
        <%
            }
            else
            {
                if (ps.ContainsKey("pg"))
                {
                    ps.Remove("pg");
                }
                ps.Add("pg", i.ToString());
        %>
        <li><a class="pagerAHidden" href="<%=PriceMe.UrlController.GetRewriterUrl(PageTo, ps)%>"><%=i.ToString()%></a></li>
        <%}
            }
            if (currentPage < totalPages)
            {
                ps.Remove("pg");
                ps.Add("pg", (currentPage + 1).ToString());
                nextURL = PriceMe.UrlController.GetRewriterUrl(PageTo, ps);

                ps.Remove("pg");
                ps.Add("pg", totalPages.ToString());
                string lastPageURL = PriceMe.UrlController.GetRewriterUrl(PageTo, ps);
        %>
        <li><a href="<%=nextURL%>" rel="nofollow" class="pagerAtag"><div class="page-separator-next">»</div></a></li>
        <%} %>
        <%} %>

        <%if (DisplayItemCountInfo && ItemCount > 0)
            { %>

        <%            
            if (totalPages > 1)
            {
                int startIndex = (currentPage - 1) * PageSize + 1;
                int endIndex = currentPage * PageSize;
                if (endIndex > ItemCount)
                {
                    endIndex = ItemCount;
                }
                if (startIndex > 0 && endIndex > 0)
                {
                    showingStr = string.Format(Resources.Resource.TextString_ShowResults_Format, startIndex.ToString("N0") + "-" + endIndex.ToString("N0"), ItemCount.ToString("N0"));
                }
            }
            else
            {
                showingStr = string.Format(Resources.Resource.TextString_ShowResults_Format2, ItemCount.ToString("N0"));
            }
        %>

        <li class="showingLI" style="float: left"></li>

        <%} %>
    </ul>

    <p style="text-align:right;"><%=showingStr %></p>
</div>
 