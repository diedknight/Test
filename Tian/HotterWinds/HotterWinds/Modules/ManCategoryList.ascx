<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManCategoryList.ascx.cs" Inherits="HotterWinds.Modules.ManCategoryList" %>

<%@ Import Namespace="PriceMe" %>
<%@ Import Namespace="PriceMeCache" %>
<%@ Import Namespace="PriceMeCommon.BusinessLogic" %>

<ul id="menu-mainmenu" class="mega-menu nav">    
    <%foreach (PriceMeCache.CategoryCache rootCate in this.RootCategoryList) %>
    <%{ %>

    <%var subCateList = CategoryController.GetNextLevelSubCategories(rootCate.CategoryID, WebConfig.CountryId);%>

    <%if (subCateList.Count == 0) %>
    <%{ %>
    <li id="nav-menu-item-<%=rootCate.CategoryID %>" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat nosub narrow">
        <a href="<%=PriceMe.UrlController.GetCatalogUrl(rootCate.CategoryID) %>" class="">
            <span><i class="fa fa-tag"></i><%=rootCate.CategoryName %></span>
        </a>
    </li>
    <%} %>
    <%else %>
    <%{ %>
    <%
        string widecss = "";
        if (subCateList.Count == 2) widecss = "has-sub wide col-2";
        if (subCateList.Count > 2) widecss = "has-sub wide col-3";

    %>
    <li id="nav-menu-item-<%=rootCate.CategoryID %>" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children <%=widecss %>">
        <a href="<%=PriceMe.UrlController.GetCatalogUrl(rootCate.CategoryID) %>" class=""><span><i class="fa fa-tag"></i><%=rootCate.CategoryName %></span></a>

        <div class="mgk-popup wrap-popup">
            <div class="inner popup" style="">
                <ul class="nav sub-menu">

                    <%foreach (var subCate in subCateList) %>
                    <%{ %>

                    <%var subsubCateList = CategoryController.GetNextLevelSubCategories(subCate.CategoryID, WebConfig.CountryId);%>

                    <li id="nav-menu-item-<%=subCate.CategoryID %>" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  sub" data-cols="1">
                        <a href="<%=PriceMe.UrlController.GetCatalogUrl(subCate.CategoryID) %>" class=""><span><%=subCate.CategoryName %></span></a>
                        <ul class="nav sub-menu">

                            <%foreach (var subsubCate in subsubCateList) %>
                            <%{ %>
                            <li id="nav-menu-item-<%=subsubCate.CategoryID %>" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat ">
                                <a href="<%=PriceMe.UrlController.GetCatalogUrl(subsubCate.CategoryID) %>" class=""><span><%=subsubCate.CategoryName %></span></a>
                            </li>
                            <%} %>                                                        
                        </ul>
                    </li>
                    <%} %>                   
                </ul>
            </div>
        </div>
    </li>
    <%} %>



    <%} %>

</ul>
