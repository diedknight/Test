<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogAjax.aspx.cs" Inherits="HotterWinds.CatalogAjax" %>

<%@ Register Src="~/Modules/Catalog/NewCatalogProducts.ascx" TagName="NewCatalogProducts" TagPrefix="DK" %>
<%@ Register Src="~/Modules/PrettyPager.ascx" TagName="PrettyPager" TagPrefix="DK" %>
<%@ Register Src="~/Modules/Catalog/NewFilters.ascx" TagName="NewFilters" TagPrefix="DK" %>



<div>
    <input type="hidden" id="c_url" value="<%=currentUrl %>" />
    <input type="hidden" id="c_pCount" value="<%=catalogPageInfo == null ? "0" : catalogPageInfo.CurrentProductCount.ToString("N0") %>" />

    <%
        string text = "0";
        if (selections != null && selections.Count > 0)
        {
            text = selections.Count.ToString();
            if (selections.Count > 1)
            {
                text += " " + Resources.Resource.TextString_Filters;
            }
            else
            {
                text += " " + Resources.Resource.TextString_Filter;
            }
        }
    %>
    <input type="hidden" id="f_Count" value="<%=text%>" />

    <div id="pcDiv">
        <DK:NewCatalogProducts ID="NewCatalogProducts1" runat="server" />
    </div>

    <div id="sortByDiv">
        <%if(!v.Equals("quick", StringComparison.InvariantCultureIgnoreCase)){ %>        
        <select id="sortBySelect">
            <%foreach(var lk in sortByInfoList) { %>
                <option value="<%=lk.Value %>" data-url="<%=lk.LinkURL %>" <%=sortBy.Equals(lk.Value, StringComparison.InvariantCultureIgnoreCase) ? "selected" : "" %>><%=lk.LinkText%></option>
            <%} %>
        </select>
        <%} %>
    </div>

    <div id="pagerDiv">
    <%if (catalogPageInfo != null)
    {%>
        <DK:PrettyPager ID="PrettyPager1" runat="server" />
    <%} %>
    </div>

    <DK:NewFilters ID="NewFilters1" runat="server" />
</div>
