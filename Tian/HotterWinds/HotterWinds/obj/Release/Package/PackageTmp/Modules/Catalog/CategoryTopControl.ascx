<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryTopControl.ascx.cs" Inherits="HotterWinds.Modules.Catalog.CategoryTopControl" %>

<%if (category == null) return;%>

<div class="ctcTitleTd page-header">
    <h1 class="productInfo"><%=category.CategoryName%></h1>
    <%if(!string.IsNullOrEmpty(category.LocalDescription)){ %>
    <div class="categoryDec">
        <%= category.LocalDescription %>
        <%var categorySynonyms = PriceMeCommon.BusinessLogic.CategoryController.GetCategorySynonym(category.CategoryID, PriceMe.WebConfig.CountryId);
 
            if(categorySynonyms != null && categorySynonyms.Count > 0)
            {
                categorySynonyms = categorySynonyms.Where(cs => !string.IsNullOrEmpty(cs.LocalName)).ToList();
            }

            if(categorySynonyms != null && categorySynonyms.Count > 0) { %>
        <%=string.Format(Resources.Resource.TextString_CategoryIsAlsoKnow, "<b>" + string.Join("</b> " + Resources.Resource.TextString_And + " <b>", categorySynonyms.Select(cs => cs.LocalName).ToList()) + "</b>") %>
        <%} %>
    </div>
    <%} %>
</div>