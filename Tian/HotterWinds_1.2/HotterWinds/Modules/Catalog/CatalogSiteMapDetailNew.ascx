<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogSiteMapDetailNew.ascx.cs" Inherits="HotterWinds.Modules.Catalog.CatalogSiteMapDetailNew" %>

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

</style>

<div id="CatalogSiteMapDetailNew" class="container">
    <div>

        <%if (Category == null) return;%>

        <div class="ctcTitleTd page-header">
            <h1 class="productInfo"><%=Category.CategoryName%></h1>
            <%if(!string.IsNullOrEmpty(Category.LocalDescription)){ %>
            <div class="categoryDec">
                <%= Category.LocalDescription %>
                <%var categorySynonyms = CategoryController.GetCategorySynonym(Category.CategoryID, WebConfig.CountryId);
                    if(categorySynonyms != null && categorySynonyms.Count > 0) { %>
                <%=string.Format(Resources.Resource.TextString_CategoryIsAlsoKnow, "<b>" + string.Join("</b> " + Resources.Resource.TextString_And + " <b>", categorySynonyms.Select(cs => cs.LocalName).ToList()) + "</b>") %>
                <%} %>
            </div>
            <%} %>
        </div>

        <div>
        <%List<PriceMeCache.CategoryCache> subSisteMapDetailCategoryCollection = CategoryController.GetNextLevelSubCategoriesIsSiteMapDetailPopularOrderByClicks(Category.CategoryID, PriceMe.WebConfig.CountryId);
          List<PriceMeCache.CategoryCache> notSubSisteMapDetailCategoryCollection = CategoryController.GetNextLevelSubCategoriesIsNotSiteMapDetailPopular(Category.CategoryID, PriceMe.WebConfig.CountryId);
          List<PriceMeCache.CategoryCache> shortCutsCategoryCollection = CategoryController.GetShortCutsCategories(Category.CategoryID, WebConfig.CountryId);%>

        <div class="sitemap">
            <%foreach (PriceMeCache.CategoryCache subc in subSisteMapDetailCategoryCollection)
                {
                    if (CategoryController.IsSearchOnly(subc.CategoryID, PriceMe.WebConfig.CountryId) || subc.CategoryID == 1283)
                        continue;

                    string url = PriceMe.UrlController.GetCatalogUrl(subc.CategoryID);
                    string imgicon = "";//subc.CategoryIconCode.Replace("512px", "90px");
                    string imgurl = subc.ImageFile.Replace("_s.", "_m.").Replace("_ms.", "_m.");
                    //if (!string.IsNullOrEmpty(subc.Categoryicon))
                    //    imgurl = subc.Categoryicon;

                    List<PriceMeCommon.Data.CategoryFilterData> filters = CategoryController.GetAllCategoryFilter(WebConfig.CountryId).Where(f => f.CategoryId == subc.CategoryID).ToList();
            %>
            <div class="ctcdiv">
                <div class="ctcdivhover">
                    <div class="ctcicon">
                        <a href="<%=url %>">
                            <%if(string.IsNullOrEmpty(imgicon)){ %>
                            <img src="<%=Resources.Resource.ImageWebsite + imgurl %>" alt="<%=subc.CategoryName %>" width="150" height="150" onerror="onImgError_catalog(this)" /><%}else{ %>
                            <%=imgicon.Replace("#3d3d3d", "#3498db").Replace("#3D3D3D", "#3498db") %>
                            <%} %>
                        </a>
                    </div>

                    <div class="ctcname">
                        <a href="<%=url %>"><%=subc.CategoryName %></a>
                    </div>

                    <%List<PriceMeCache.CategoryCache> subCatas = CategoryController.GetAllSubCategory(subc.CategoryID, WebConfig.CountryId);
                      if (subCatas.Count > 0)
                      { %>
                    <% 
                  bool ismore = false;
                  int count = 0;
                  bool isaccessories = false;
                  if (subCatas.Count > 9)
                      isaccessories = true;

                  foreach(PriceMeCache.CategoryCache subCate in subCatas)
                      {
                          if (CategoryController.IsSearchOnly(subCate.CategoryID, WebConfig.CountryId))
                              continue;
                          if (isaccessories)
                          {
                              if (subCate.IsAccessories)
                                  continue;
                          }
                      
                          if (count > 8) { ismore = true; continue; }
                          count++;
                          string subUrl = PriceMe.UrlController.GetCatalogUrl(subCate.CategoryID);
                    %>
                        <div><a href="<%=subUrl%>"><i class="glyphicon glyphicon-folder-open iconLight"></i><%=subCate.CategoryName.Length > 17 ? subCate.CategoryName.Substring(0, 17) + "..." : subCate.CategoryName %></a></div>
                        <%} %>
                    <%if(ismore){%>
                    <div><i class="glyphicon glyphicon-play iconLight"></i><a href="<%=url %>">More <%=subc.CategoryName %>...</a></div>
                    <%} %>

                    <%}else{
                          if (subc.IsFilterByBrand)
                          {
                              PriceMeCommon.Data.NarrowByInfo narrowByInfo = GetAllTopManufacturerCache(subc.CategoryID);

                              foreach (PriceMeCommon.Data.NarrowItem narrowItem in narrowByInfo.NarrowItemList)
                              { %>
                    <div><a href="<%=narrowItem.Url%>"><i class="glyphicon glyphicon-flag iconLight"></i><%=narrowItem.DisplayName%></a><label class="countSpan">(<%= narrowItem.ProductCount%>)</label></div>
                        <%}
                          } %>
                    <%foreach(PriceMeCommon.Data.CategoryFilterData filter in filters){ %>
                    <div><a href="<%=filter.FilterUrl %>"><i class="glyphicon glyphicon-filter iconLight"></i><%=filter.FilterName %></a></div>
                    <%} %>

                    <div><a href="<%=PriceMe.UrlController.GetCatalogUrl(subc.CategoryID) %>"><i class="glyphicon glyphicon-arrow-right iconLight"></i><%=Resources.Resource.Module_CatalogSiteMapDetail_SeeMore.Replace("{0}", subc.CategoryName) %></a></div>
                    <%} %>
                </div>

                <div class="ctcicon">
                    <%if(string.IsNullOrEmpty(imgicon)){ %>
                    <img src="<%=Resources.Resource.ImageWebsite + imgurl %>" alt="<%=subc.CategoryName %>" width="150" height="150" onerror="onImgError_catalog(this)" /><%}else{ %>
                    <%=imgicon %>
                    <%} %>
                </div>

                <div class="ctcdisplayname">
                    <a href="<%=url %>"><%=subc.CategoryName %></a>
                </div>
            </div>
            <%} %>

            <%if (Category.CategoryID == 355 && WebConfig.CountryId == 3)
              { %>
            <div class="ctcdiv">
                <div class="ctcdivhover">
                    <div class="ctcicon">
                        <a href="https://www.priceme.co.nz/plans/mobile-plans">
                            <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" id="Capa_1" x="0px" y="0px" width="90px" height="90px" viewBox="0 0 70.072 70.072" style="enable-background:new 0 0 70.072 70.072;" xml:space="preserve">
                                <g>
	                                <path d="M47.288,48.218c0-8.162-6.64-14.803-14.799-14.803v-2.494c9.537-0.002,17.297,7.757,17.297,17.292L47.288,48.218z    M45.471,48.213c-0.005-7.153-5.826-12.979-12.987-12.979l0.005,2.5c5.778,0,10.479,4.704,10.479,10.479H45.471L45.471,48.213z    M32.486,39.693l0.004,2.497c3.321-0.004,6.022,2.699,6.022,6.022h2.496C41.013,43.516,37.19,39.695,32.486,39.693z M65.097,32.19   h-6.984h-0.929c-2.069,0-3.743,1.674-3.743,3.745v0.406c0,2.076,1.674,3.748,3.743,3.748h6.987h0.926   c2.066,0,3.745-1.681,3.745-3.748l-0.004-0.41C68.842,33.865,67.163,32.19,65.097,32.19z M68.842,26.588v0.416   c0,2.07-1.674,3.745-3.743,3.745h-0.928h-6.059h-0.931c-2.062,0-3.741-1.67-3.741-3.745v-0.406c0-2.072,1.679-3.747,3.745-3.747   h0.927v-0.005h2.043V7.505L26.036,7.5v8.376c3.265-1.833,6.131-3.424,7.525-4.146c4.428-2.296,6.974,0.504,6.207,3.827   c-0.589,2.564-4.938,5.388-9.523,8.929c-0.906,0.704-2.49,1.747-4.208,2.836v33.99h34.12v-2.536h-2.974   c-2.067,0-3.741-1.681-3.741-3.743v-0.418c0-2.066,1.67-3.74,3.741-3.74h0.931h6.059h0.924c2.068,0,3.743,1.67,3.747,3.74   l-0.004,0.409c0.004,2.071-1.675,3.747-3.748,3.747h-0.924v0.005h-0.724v6.567c0,2.598-2.128,4.729-4.732,4.729H27.476   c-2.604,0-4.734-2.131-4.734-4.729V59.96h-5.688c0,0-8.404-0.031-12.961-6.918c-0.049-0.071-0.096-0.135-0.145-0.205   c-0.191-0.302-0.365-0.623-0.539-0.947c-0.147-0.273-0.292-0.565-0.433-0.862c-0.133-0.282-0.262-0.557-0.383-0.857   c-0.24-0.606-0.448-1.241-0.633-1.923c-0.051-0.203-0.089-0.422-0.14-0.632c-0.138-0.589-0.25-1.207-0.341-1.854   c-0.035-0.249-0.067-0.497-0.096-0.755c-0.089-0.881-0.152-1.796-0.152-2.781c0-11.063,5.169-15.748,8.697-17.199   c0.014-0.009,6.513-3.73,12.814-7.292V4.734c0-2.604,2.13-4.734,4.734-4.734h31.238c2.601,0,4.731,2.131,4.731,4.734v18.118h1.651   C67.168,22.847,68.842,24.526,68.842,26.588z M40.73,65.339c0,1.314,1.059,2.372,2.365,2.372c1.312,0,2.37-1.058,2.37-2.372   c0-1.31-1.059-2.367-2.37-2.367C41.789,62.972,40.73,64.034,40.73,65.339z M48.664,3.983c0-0.321-0.254-0.575-0.57-0.575H38.1   c-0.316,0-0.574,0.254-0.574,0.575c0,0.315,0.258,0.57,0.574,0.57h9.994C48.408,4.553,48.664,4.298,48.664,3.983z M65.097,41.534   h-0.926h-6.987c-2.064,0-3.743,1.679-3.743,3.745v0.41c0,2.071,1.674,3.746,3.743,3.746h0.929h6.059h0.926   c2.071,0,3.741-1.675,3.745-3.746l-0.004-0.41C68.838,43.213,67.168,41.534,65.097,41.534z" fill="#3498db"/>
                                </g>
                            </svg>
                        </a>
                    </div>

                    <div class="ctcname">
                        <a href="https://www.priceme.co.nz/plans/mobile-plans">Mobile Plans</a>
                    </div>
                    <div><a href="https://www.priceme.co.nz/plans/mobile-plans/spark"><i class="glyphicon glyphicon-flag iconLight"></i>Spark</a></div>
                    <div><a href="https://www.priceme.co.nz/plans/mobile-plans/vodafone"><i class="glyphicon glyphicon-flag iconLight"></i>Vodafone</a></div>
                    <div><a href="https://www.priceme.co.nz/plans/mobile-plans/2-degrees"><i class="glyphicon glyphicon-flag iconLight"></i>2 Degrees</a></div>
                </div>

                <div class="ctcicon">
                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" id="Capa_1" x="0px" y="0px" width="90px" height="90px" viewBox="0 0 70.072 70.072" style="enable-background:new 0 0 70.072 70.072;" xml:space="preserve">
                        <g>
	                        <path d="M47.288,48.218c0-8.162-6.64-14.803-14.799-14.803v-2.494c9.537-0.002,17.297,7.757,17.297,17.292L47.288,48.218z    M45.471,48.213c-0.005-7.153-5.826-12.979-12.987-12.979l0.005,2.5c5.778,0,10.479,4.704,10.479,10.479H45.471L45.471,48.213z    M32.486,39.693l0.004,2.497c3.321-0.004,6.022,2.699,6.022,6.022h2.496C41.013,43.516,37.19,39.695,32.486,39.693z M65.097,32.19   h-6.984h-0.929c-2.069,0-3.743,1.674-3.743,3.745v0.406c0,2.076,1.674,3.748,3.743,3.748h6.987h0.926   c2.066,0,3.745-1.681,3.745-3.748l-0.004-0.41C68.842,33.865,67.163,32.19,65.097,32.19z M68.842,26.588v0.416   c0,2.07-1.674,3.745-3.743,3.745h-0.928h-6.059h-0.931c-2.062,0-3.741-1.67-3.741-3.745v-0.406c0-2.072,1.679-3.747,3.745-3.747   h0.927v-0.005h2.043V7.505L26.036,7.5v8.376c3.265-1.833,6.131-3.424,7.525-4.146c4.428-2.296,6.974,0.504,6.207,3.827   c-0.589,2.564-4.938,5.388-9.523,8.929c-0.906,0.704-2.49,1.747-4.208,2.836v33.99h34.12v-2.536h-2.974   c-2.067,0-3.741-1.681-3.741-3.743v-0.418c0-2.066,1.67-3.74,3.741-3.74h0.931h6.059h0.924c2.068,0,3.743,1.67,3.747,3.74   l-0.004,0.409c0.004,2.071-1.675,3.747-3.748,3.747h-0.924v0.005h-0.724v6.567c0,2.598-2.128,4.729-4.732,4.729H27.476   c-2.604,0-4.734-2.131-4.734-4.729V59.96h-5.688c0,0-8.404-0.031-12.961-6.918c-0.049-0.071-0.096-0.135-0.145-0.205   c-0.191-0.302-0.365-0.623-0.539-0.947c-0.147-0.273-0.292-0.565-0.433-0.862c-0.133-0.282-0.262-0.557-0.383-0.857   c-0.24-0.606-0.448-1.241-0.633-1.923c-0.051-0.203-0.089-0.422-0.14-0.632c-0.138-0.589-0.25-1.207-0.341-1.854   c-0.035-0.249-0.067-0.497-0.096-0.755c-0.089-0.881-0.152-1.796-0.152-2.781c0-11.063,5.169-15.748,8.697-17.199   c0.014-0.009,6.513-3.73,12.814-7.292V4.734c0-2.604,2.13-4.734,4.734-4.734h31.238c2.601,0,4.731,2.131,4.731,4.734v18.118h1.651   C67.168,22.847,68.842,24.526,68.842,26.588z M40.73,65.339c0,1.314,1.059,2.372,2.365,2.372c1.312,0,2.37-1.058,2.37-2.372   c0-1.31-1.059-2.367-2.37-2.367C41.789,62.972,40.73,64.034,40.73,65.339z M48.664,3.983c0-0.321-0.254-0.575-0.57-0.575H38.1   c-0.316,0-0.574,0.254-0.574,0.575c0,0.315,0.258,0.57,0.574,0.57h9.994C48.408,4.553,48.664,4.298,48.664,3.983z M65.097,41.534   h-0.926h-6.987c-2.064,0-3.743,1.679-3.743,3.745v0.41c0,2.071,1.674,3.746,3.743,3.746h0.929h6.059h0.926   c2.071,0,3.741-1.675,3.745-3.746l-0.004-0.41C68.838,43.213,67.168,41.534,65.097,41.534z" fill="#3d3d3d"/>
                        </g>
                    </svg>
                </div>

                <div class="ctcdisplayname">
                    <a href="https://www.priceme.co.nz/plans/mobile-plans">Mobile Plans</a>
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
            <%foreach (PriceMeCache.CategoryCache subc in subSisteMapDetailCategoryCollection)
                {
                    if (CategoryController.IsSearchOnly(subc.CategoryID, WebConfig.CountryId))
                        continue;

                    string url = PriceMe.UrlController.GetCatalogUrl(subc.CategoryID);
                    string imgicon = "";//subc.CategoryIconCode.Replace("512px", "50px");
                    string imgurl = subc.ImageFile.Replace("_ms.", "_s.");                    

                    //if (!string.IsNullOrEmpty(subc.Categoryicon))
                    //    imgurl = subc.Categoryicon;
            %>
            <div class="line" onclick="openlink('<%=url%>');">
                <div class="icon">
                    <%if(string.IsNullOrEmpty(imgicon)){ %>
                    <img src="<%=Resources.Resource.ImageWebsite + imgurl %>" alt="<%=subc.CategoryName %>" width="50" height="50" onerror="onImgError(this)" /><%}else{ %>
                    <%=imgicon %>
                    <%} %>
                </div>
                <div class="name">
                    <div class="vertical"><a href="<%=url %>"><%=subc.CategoryName %></a></div></div>
                <div class="link">
                    <div class="vertical"><a href="<%=url %>"><span class="glyphicon glyphicon-chevron-right iconGray"></span></a></div></div>
            </div>
            <%} %>
            <%if (Category.CategoryID == 355 && WebConfig.CountryId == 3)
              { %>
            <div class="line" onclick="openlink('https://www.priceme.co.nz/plans/mobile-plans');">
                <div class="icon">
                    <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" id="Capa_1" x="0px" y="0px" width="50px" height="50px" viewBox="0 0 70.072 70.072" style="enable-background:new 0 0 70.072 70.072;" xml:space="preserve">
                        <g>
	                        <path d="M47.288,48.218c0-8.162-6.64-14.803-14.799-14.803v-2.494c9.537-0.002,17.297,7.757,17.297,17.292L47.288,48.218z    M45.471,48.213c-0.005-7.153-5.826-12.979-12.987-12.979l0.005,2.5c5.778,0,10.479,4.704,10.479,10.479H45.471L45.471,48.213z    M32.486,39.693l0.004,2.497c3.321-0.004,6.022,2.699,6.022,6.022h2.496C41.013,43.516,37.19,39.695,32.486,39.693z M65.097,32.19   h-6.984h-0.929c-2.069,0-3.743,1.674-3.743,3.745v0.406c0,2.076,1.674,3.748,3.743,3.748h6.987h0.926   c2.066,0,3.745-1.681,3.745-3.748l-0.004-0.41C68.842,33.865,67.163,32.19,65.097,32.19z M68.842,26.588v0.416   c0,2.07-1.674,3.745-3.743,3.745h-0.928h-6.059h-0.931c-2.062,0-3.741-1.67-3.741-3.745v-0.406c0-2.072,1.679-3.747,3.745-3.747   h0.927v-0.005h2.043V7.505L26.036,7.5v8.376c3.265-1.833,6.131-3.424,7.525-4.146c4.428-2.296,6.974,0.504,6.207,3.827   c-0.589,2.564-4.938,5.388-9.523,8.929c-0.906,0.704-2.49,1.747-4.208,2.836v33.99h34.12v-2.536h-2.974   c-2.067,0-3.741-1.681-3.741-3.743v-0.418c0-2.066,1.67-3.74,3.741-3.74h0.931h6.059h0.924c2.068,0,3.743,1.67,3.747,3.74   l-0.004,0.409c0.004,2.071-1.675,3.747-3.748,3.747h-0.924v0.005h-0.724v6.567c0,2.598-2.128,4.729-4.732,4.729H27.476   c-2.604,0-4.734-2.131-4.734-4.729V59.96h-5.688c0,0-8.404-0.031-12.961-6.918c-0.049-0.071-0.096-0.135-0.145-0.205   c-0.191-0.302-0.365-0.623-0.539-0.947c-0.147-0.273-0.292-0.565-0.433-0.862c-0.133-0.282-0.262-0.557-0.383-0.857   c-0.24-0.606-0.448-1.241-0.633-1.923c-0.051-0.203-0.089-0.422-0.14-0.632c-0.138-0.589-0.25-1.207-0.341-1.854   c-0.035-0.249-0.067-0.497-0.096-0.755c-0.089-0.881-0.152-1.796-0.152-2.781c0-11.063,5.169-15.748,8.697-17.199   c0.014-0.009,6.513-3.73,12.814-7.292V4.734c0-2.604,2.13-4.734,4.734-4.734h31.238c2.601,0,4.731,2.131,4.731,4.734v18.118h1.651   C67.168,22.847,68.842,24.526,68.842,26.588z M40.73,65.339c0,1.314,1.059,2.372,2.365,2.372c1.312,0,2.37-1.058,2.37-2.372   c0-1.31-1.059-2.367-2.37-2.367C41.789,62.972,40.73,64.034,40.73,65.339z M48.664,3.983c0-0.321-0.254-0.575-0.57-0.575H38.1   c-0.316,0-0.574,0.254-0.574,0.575c0,0.315,0.258,0.57,0.574,0.57h9.994C48.408,4.553,48.664,4.298,48.664,3.983z M65.097,41.534   h-0.926h-6.987c-2.064,0-3.743,1.679-3.743,3.745v0.41c0,2.071,1.674,3.746,3.743,3.746h0.929h6.059h0.926   c2.071,0,3.741-1.675,3.745-3.746l-0.004-0.41C68.838,43.213,67.168,41.534,65.097,41.534z" fill="#3d3d3d"/>
                        </g>
                    </svg>
                </div>
                <div class="name">
                    <div class="vertical"><a href="https://www.priceme.co.nz/plans/mobile-plans">Mobile Plans</a></div></div>
                <div class="link">
                    <div class="vertical"><a href="https://www.priceme.co.nz/plans/mobile-plans"><span class="glyphicon glyphicon-chevron-right iconGray"></span></a></div></div>
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
