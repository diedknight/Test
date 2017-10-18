<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogSiteMapNew.ascx.cs" Inherits="HotterWinds.Modules.Catalog.CatalogSiteMapNew" %>

<%@ Import Namespace="PriceMeCommon.Data" %>
<%@ Import Namespace="PriceMeCommon.BusinessLogic" %>
<%@ Import Namespace="PriceMe" %>

<%if (Category == null) return; %>

<style type="text/css">
    .ctcdiv {
        width:325px;
        height:400px;
    }

    .ctcdiv .ctcdivhover{
        width:325px;        
    }

    .sitemapphone .line {
        height:inherit;
        width:inherit;
        margin:0px;
        border:0px;
    }

    .sitemapphone .link {
        width:auto;
    }

    .sitemapphone .name {
        width:240px;
    }

</style>

<div id="CatalogSiteMapNew" class="container">
    <div id="catalogLeftDiv" style="margin-bottom:20px; width:100%">
        <div class="ctcTitleTd page-header">
            <h1 class="productInfo"><%=Category.CategoryName%></h1>
            <div class="categoryDec">
                <%=Category.LocalDescription %>
                <%var categorySynonyms = CategoryController.GetCategorySynonym(Category.CategoryID, WebConfig.CountryId);
                    if (categorySynonyms != null && categorySynonyms.Count > 0) { %>
                <%=string.Format(Resources.Resource.TextString_CategoryIsAlsoKnow, "<b>" + string.Join("</b> " + Resources.Resource.TextString_And + " <b>", categorySynonyms.Select(cs => cs.LocalName).ToList()) + "</b>") %>
                <%} %>
            </div>
        </div>

        <div>
        <div class="sitemap">
            <%foreach (CatalogSitemapCategory csc in catalogSitemapCategories)
                {
                    if (CategoryController.IsSearchOnly(csc.CategoryID, WebConfig.CountryId))
                        continue;

                    csc.Link.LinkURL = PriceMe.UrlController.GetCatalogUrl(int.Parse(csc.Link.Value));

                    string imgicon = "";//csc.IconCode.Replace("512px", "90px");
                    string imgurl = csc.ImageURL.Replace("_s.", "_m.").Replace("_ms.", "_m.");
                    //if (!string.IsNullOrEmpty(csc.IconUrl))

                    //    imgurl = csc.IconUrl;
            %>
            <div class="ctcdiv">
                <div class="ctcdivhover">
                    <div class="ctcicon">
                        <a href="<%=csc.Link.LinkURL%>">
                            <%if (string.IsNullOrEmpty(imgicon)) { %>
                            <img src="<%=/*Resources.Resource.ImageWebsite +*/ imgurl %>" alt="<%=csc.Link.LinkText%>" width="150" height="150" onerror="onImgError_catalog(this)" /><%} else { %>
                            <%=imgicon.Replace("#3d3d3d", "#3498db").Replace("#3D3D3D", "#3498db") %>
                            <%} %>
                        </a>
                    </div>

                    <div class="ctcname">
                        <a href="<%=csc.Link.LinkURL%>"><%=csc.Link.LinkText%></a>
                    </div>

                        <%
                            List<PriceMeCache.CategoryCache> shortCutsCategoryCollection = CategoryController.GetShortCutsCategories(csc.CategoryID, WebConfig.CountryId).OrderBy(item=>item.ListOrder).ToList();
                            bool ismore = false;
                            int count = 0;
                            bool isaccessories = false;
                            if ((csc.SubCategoryInfos.Count + shortCutsCategoryCollection.Count) > 9)
                                isaccessories = true;

                            for (int i = 0; i < csc.SubCategoryInfos.Count; i++)
                            {
                                if (CategoryController.IsSearchOnly(int.Parse(csc.SubCategoryInfos[i].Value), WebConfig.CountryId))
                                    continue;
                                if (isaccessories)
                                {
                                    PriceMeCache.CategoryCache cate = CategoryController.GetCategoryByCategoryID(int.Parse(csc.SubCategoryInfos[i].Value), WebConfig.CountryId);
                                    if (cate.IsAccessories)
                                        continue;
                                }

                                if (count > 8) { ismore = true; continue; }
                                count++;
                                string url = PriceMe.UrlController.GetCatalogUrl(int.Parse(csc.SubCategoryInfos[i].Value));
                    %>
                        <div><a href="<%=url%>"><i class="glyphicon glyphicon-folder-open iconLight"></i><%=csc.SubCategoryInfos[i].LinkText.Length > 17 ? csc.SubCategoryInfos[i].LinkText.Substring(0, 17) + "..." : csc.SubCategoryInfos[i].LinkText %></a></div>
                        <%} %>

                        <%
                            foreach (PriceMeCache.CategoryCache subc in shortCutsCategoryCollection)
                            {
                                if (CategoryController.IsSearchOnly(subc.CategoryID, WebConfig.CountryId))
                                    continue;
                                if (count > 8) { ismore = true; continue; }
                                count++;
                                string url = PriceMe.UrlController.GetCatalogUrl(subc.CategoryID);
                    %>
                    <div><i class="glyphicon glyphicon-share-alt iconLight"></i><a href="<%=url%>"><%=subc.CategoryName%></a></div>
                    <%} if (ismore) {%>
                    <div><i class="glyphicon glyphicon-play iconLight"></i><a href="<%=csc.Link.LinkURL%>">More <%=csc.Link.LinkText %>...</a></div>
                    <%} %>
                </div>

                <div class="ctcicon">
                    <%if (string.IsNullOrEmpty(imgicon)) { %>
                    <img src="<%=/*Resources.Resource.ImageWebsite +*/ imgurl %>" alt="<%=csc.Link.LinkText%>" width="150" height="150" onerror="onImgError_catalog(this)" /><%} else { %>
                    <%=imgicon %>
                    <%} %>
                </div>

                <div class="ctcdisplayname"> 
                    <a href="<%=csc.Link.LinkURL%>"><%=csc.Link.LinkText%></a>
                </div>
            </div>
            <%} %>

            <%
                if (WebConfig.CountryId == 3 && HasDealsProduct)
                {
                    string dealsUrl = "https://deals.priceme.co.nz/" + PriceMe.UrlController.FilterInvalidNameChar(Category.CategoryNameEN);
                    string dealsIcon = @"<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' version='1.1' id='Capa_1' x='0px' y='0px' width='90px' height='85px' viewBox='0 0 35.536 35.536' style='enable-background:new 0 0 35.536 35.536;' xml:space='preserve'>
<g>
	<path d='M26.641,10.99l-2.094-2.095L33.443,0l2.093,2.092L26.641,10.99z M20.398,19.688c-0.467,0-0.729,0.265-0.729,0.896v4.871   c0,0.631,0.265,0.896,0.729,0.896c0.447,0,0.73-0.267,0.73-0.896v-4.871C21.129,19.952,20.846,19.688,20.398,19.688z    M14.082,20.256v-4.873c0-0.629-0.285-0.896-0.73-0.896c-0.467,0-0.731,0.268-0.731,0.896v4.873c0,0.631,0.266,0.895,0.731,0.895   C13.797,21.15,14.082,20.887,14.082,20.256z M30.732,20.936L16.133,35.536L0,19.402L14.601,4.803   c3.715-3.715,9.354-4.33,13.709-1.848L26.034,5.23c-1.213-0.055-2.438,0.381-3.367,1.306c-1.748,1.748-1.746,4.585,0,6.332   c1.75,1.748,4.584,1.748,6.332,0c0.927-0.927,1.358-2.153,1.308-3.366l2.274-2.275C35.062,11.58,34.45,17.219,30.732,20.936z    M13.352,22.43c1.4,0,2.151-0.812,2.151-2.274v-4.67c0-1.463-0.751-2.271-2.151-2.271c-1.401,0-2.152,0.811-2.152,2.271v4.67   C11.2,21.618,11.951,22.43,13.352,22.43z M20.277,13.311h-1.319l-5.605,14.216h1.318L20.277,13.311z M22.553,20.684   c0-1.463-0.754-2.274-2.154-2.274c-1.399,0-2.152,0.813-2.152,2.274v4.67c0,1.463,0.753,2.273,2.152,2.273   c1.402,0,2.154-0.812,2.154-2.273V20.684z' fill='#3d3d3d'/>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
</svg>
";
                    string dealsLinkText = "Deals"; %>
            <div class="ctcdiv">
                <div class="ctcdivhover">
                    <div class="ctcicon">
                        <a href="<%=dealsUrl%>">
                            <%=dealsIcon.Replace("#3d3d3d", "#3498db").Replace("#3D3D3D", "#3498db") %>
                        </a>
                    </div>

                    <div class="ctcname">
                        <a href="<%=dealsUrl%>"><%=dealsLinkText%></a>
                    </div>
                </div>

                <div class="ctcicon">
                    <%=dealsIcon.Replace("#3d3d3d", "#f8484a").Replace("#3D3D3D", "#f8484a") %>
                </div>

                <div class="ctcdisplayname"> 
                    <a href="<%=dealsUrl%>"><%=dealsLinkText%></a>
                </div>
            </div>
            <%} %>
        </div>
        
        <div class="sitemapphone">
            <%foreach (CatalogSitemapCategory csc in catalogSitemapCategories)
                {
                    if (CategoryController.IsSearchOnly(csc.CategoryID, WebConfig.CountryId))
                        continue;

                    csc.Link.LinkURL = PriceMe.UrlController.GetCatalogUrl(int.Parse(csc.Link.Value));
                    string imgicon = "";//csc.IconCode.Replace("512px", "50px");
                    string imgurl = csc.ImageURL.Replace("_ms.", "_s.");

                    //if (!string.IsNullOrEmpty(csc.IconUrl))
                    //    imgurl = csc.IconUrl;
            %>
            <div class="line" onclick="openlink('<%=csc.Link.LinkURL%>');">
                <div class="icon">
                    <%if(string.IsNullOrEmpty(imgicon)){ %>
                    <img src="<%=/*Resources.Resource.ImageWebsite +*/ imgurl %>" alt="<%=csc.Link.LinkText%>" width="50" height="50" onerror="onImgError(this)" /><%}else{ %>
                    <%=imgicon %>
                    <%} %>
                </div>
                <div class="name">
                    <div class="vertical"><a href="<%=csc.Link.LinkURL%>"><%=csc.Link.LinkText%></a></div></div>
                <div class="link">
                    <div class="vertical"><a href="<%=csc.Link.LinkURL%>"><span class="glyphicon glyphicon-chevron-right iconGray"></span></a></div></div>
            </div>
            <%} %>

            <%
                if (WebConfig.CountryId == 3 && HasDealsProduct)
                {
                    string dealsUrl = "https://deals.priceme.co.nz/" + PriceMe.UrlController.FilterInvalidNameChar(Category.CategoryNameEN);
                    string dealsIcon = @"<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' version='1.1' id='Capa_1' x='0px' y='0px' width='50px' height='50px' viewBox='0 0 35.536 35.536' style='enable-background:new 0 0 35.536 35.536;' xml:space='preserve'>
<g>
	<path d='M26.641,10.99l-2.094-2.095L33.443,0l2.093,2.092L26.641,10.99z M20.398,19.688c-0.467,0-0.729,0.265-0.729,0.896v4.871   c0,0.631,0.265,0.896,0.729,0.896c0.447,0,0.73-0.267,0.73-0.896v-4.871C21.129,19.952,20.846,19.688,20.398,19.688z    M14.082,20.256v-4.873c0-0.629-0.285-0.896-0.73-0.896c-0.467,0-0.731,0.268-0.731,0.896v4.873c0,0.631,0.266,0.895,0.731,0.895   C13.797,21.15,14.082,20.887,14.082,20.256z M30.732,20.936L16.133,35.536L0,19.402L14.601,4.803   c3.715-3.715,9.354-4.33,13.709-1.848L26.034,5.23c-1.213-0.055-2.438,0.381-3.367,1.306c-1.748,1.748-1.746,4.585,0,6.332   c1.75,1.748,4.584,1.748,6.332,0c0.927-0.927,1.358-2.153,1.308-3.366l2.274-2.275C35.062,11.58,34.45,17.219,30.732,20.936z    M13.352,22.43c1.4,0,2.151-0.812,2.151-2.274v-4.67c0-1.463-0.751-2.271-2.151-2.271c-1.401,0-2.152,0.811-2.152,2.271v4.67   C11.2,21.618,11.951,22.43,13.352,22.43z M20.277,13.311h-1.319l-5.605,14.216h1.318L20.277,13.311z M22.553,20.684   c0-1.463-0.754-2.274-2.154-2.274c-1.399,0-2.152,0.813-2.152,2.274v4.67c0,1.463,0.753,2.273,2.152,2.273   c1.402,0,2.154-0.812,2.154-2.273V20.684z' fill='#f8484a'/>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
<g>
</g>
</svg>
";
                    string dealsLinkText = "Deals"; %>

            <div class="line" onclick="openlink('<%=dealsUrl%>');">
                <div class="icon">
                    
                    <%=dealsIcon %>

                </div>
                <div class="name">
                    <div class="vertical"><a href="<%=dealsUrl%>"><%=dealsLinkText%></a></div></div>
                <div class="link">
                    <div class="vertical"><a href="<%=dealsUrl%>"><span class="glyphicon glyphicon-chevron-right iconGray"></span></a></div></div>
            </div>

            <%} %>

        </div>
            </div>
    </div>
</div>