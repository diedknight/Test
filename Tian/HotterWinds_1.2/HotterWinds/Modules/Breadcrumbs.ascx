<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Breadcrumbs.ascx.cs" Inherits="HotterWinds.Modules.Breadcrumbs" %>

<div class="breadcrumbs">
    <ul>
        <li><a class="home" href="/">Home</a></li>
        <% foreach (var cate in CategoryList)%>
        <%{ %>
        <span>⁄ </span>
        <li><a href="<%=PriceMe.UrlController.GetCatalogUrl(cate.CategoryId) %>"><%=cate.CategoryName %></a></li>
        <%}%>

        <%if (!string.IsNullOrEmpty(ProductName)) %>
        <%{ %>
        <span>⁄ </span>
        <li><strong><%=ProductName %></strong></li>
        <%} %>        
    </ul>
</div>
