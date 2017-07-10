<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RetailerCenterNavigation.ascx.cs" Inherits="HotterWinds.Modules.RetailerCenterNavigation" %>

<%
    string url = Request.Url.ToString().ToLower();
    //string other = "";
%>
<div style="padding:20px 20px 20px 15px;">
<ul class="nav nav-tabs" role="tablist">
<%--    <%other = url.Contains("retailercenter.aspx") ? "active" : "";%><li class="<%=other %>"><a href="../RetailerCenter.aspx"><%=Resources.Resource.TextString_RetailerCentre %> </a></li>
    <%other = url.Contains("listyourproducts.aspx") ? "active" : "";%><li class="<%=other %>"><a href="../RetailerCenter/ListYourProducts.aspx"><%=Resources.Resource.TextString_ListProducts %></a></li>
    <li ><a href="https://merchant.priceme.com" rel="nofollow"><%=Resources.Resource.TextString_MerchantPortal %></a></li>
    <%other = url.Contains("retailerlist.aspx") ? "active" : "";%><li class="<%=other %>"><a href="../RetailerList.aspx"><%=Resources.Resource.TextString_CurrentRetailers %> </a></li>
    <%other = url.Contains("retailerkit.aspx") ? "active" : "";%><li class="<%=other %>"><a href="<%=Page.ResolveUrl("~/RetailerCenter/RetailerKit.aspx")%>"><%=Resources.Resource.TextString_RetailerKit %></a></li>
    <%other = url.Contains("faq.aspx") ? "active" : "";%><li class="<%=other %>"><a href="../RetailerCenter/FAQ.aspx"><%=Resources.Resource.TextString_FAQs %></a></li>
    <%other = url.Contains("channelpartners.aspx") ? "active" : "";%><li class="<%=other %>"><a href="<%=Page.ResolveUrl("~/RetailerCenter/ChannelPartners.aspx")%>"><%=Resources.Resource.TextString_ChannelPartners %></a></li>--%>

    <li class="active"><a href="../RetailerCenter.aspx"><%=Resources.Resource.TextString_RetailerCentre %> </a></li>
    <li class=""><a href="../RetailerCenter.aspx#ListYourProds"><%=Resources.Resource.TextString_ListProducts %></a></li>
    <li class=""><a href="../RetailerCenter.aspx#RetailerKit"><%=Resources.Resource.TextString_RetailerKit %></a></li>
    <li class=""><a href="../RetailerCenter.aspx#Faq"><%=Resources.Resource.TextString_FAQs %></a></li>
    <li class=""><a href="../RetailerCenter.aspx#ChannelPartners"><%=Resources.Resource.TextString_ChannelPartners %></a></li>

</ul>
    </div>