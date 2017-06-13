<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickProductSummaryDisplay.ascx.cs" Inherits="HotterWinds.Modules.Catalog.QuickProductSummaryDisplay" %>

<%@ Import Namespace="System.Linq" %>

<%if (catalogManufeaturerProductList != null)
  {
      %>
      <style type="text/css">
        .quickH2{background-color: #CFE4F4;padding:5px 10px;margin:0;}
        .quickListTable td{padding:5px;}
        .quickListTable .redirectLabel{color:#36c;}
        .quickListTable th {background-color:#F2F2F2;border-color:#FFF #BBB #BBB #FFF;border-style:solid;border-width:1px;text-align:center;}
        .quickListTable .thPN{}
        .quickListTable .thPc{width:110px;}
        .quickListTable .thRN{width:114px;}
        .quickListTable a{font-weight:bolder;font-size:13px;}
        .quickListTable .pT{background-color:#E2E2E2;}
        .quickListTable .glyphicon-shopping-cart{color:gray;}
        .quickListTable .attTD{width:85px;}
        @media (min-width: 1200px) {
            #catalogProductsDiv {
                width:1170px;
            }
        }
      </style>
      <%
          List<string> attributeTitles = new List<string>();
          if (ProductsAttributes != null && ProductsAttributes.Count > 0)
          {
              var attributeTitleIDs = ProductsAttributes.Keys.ToList();
              foreach(int atID in attributeTitleIDs)
              {
                  var attrTitle = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeTitleByID(atID);
                  string title = attrTitle.Title;
                  if(!string.IsNullOrEmpty(attrTitle.Unit))
                  {
                      title += "[" + attrTitle.Unit + "]";
                  }
                  attributeTitles.Add(title);
              }
          }
          catalogManufeaturerProductList.Sort();%>

<%if (attributeTitles.Count > 0)
                      { %>
<table id="adminQuick" class="quickListTable">
    <tr>
              <th class="thPN">
              </th>

              <%foreach(string at in attributeTitles) { 
                      %>
                <th class="attTD">
                    <%=at %>
                </th>
              <%} %>
            <td colspan="3"></td>
          </tr>
</table>
<%} %>

<%if (productCatalogArrayF != null && productCatalogArrayF.Count > 0)
  { %>
    <table class="quickListTable">

        <%int count = 0;
          string cssClass = "";
          foreach (PriceMeCommon.Data.ProductCatalog pc in productCatalogArrayF)
          {
              int urltype = 1;
              if (count % 2 == 0)
              {
                  cssClass = "sT";
              }
              else
              {
                  cssClass = "pT";
              }
              count++;
              string url = PriceMe.UrlController.GetProductUrl(int.Parse(pc.ProductID), pc.ProductName);
              if (PriceMeCommon.BusinessLogic.CategoryController.IsSearchOnly(pc.CategoryID, PriceMe.WebConfig.CountryId))
              {
                  if (!string.IsNullOrEmpty(pc.BestPPCRetailerName))
                  {
                      urltype = 2;
                      url = PriceMe.Utility.GetRootUrl("/ResponseRedirect.aspx?pid=" + pc.ProductID + "&rid=" + pc.BestPPCRetailerID + "&rpid=" + pc.BestPPCRetailerProductID + "&countryID=" + PriceMe.WebConfig.CountryId + "&cid=" + pc.CategoryID + "&t=c", PriceMe.WebConfig.CountryId).Replace("&", "&amp;");
                      string uuid = Guid.NewGuid().ToString();
                      url += "&uuid=" + uuid;
                  }
                  else
                      urltype = 3;
              }
              double bastPrice = 0;
              string bastPriceStr = "";
              try
              {
                  bastPrice = double.Parse(pc.BestPrice, System.Globalization.NumberStyles.Any, PriceMeCommon.PriceMeStatic.Provider);
                  if (bastPrice != 0)
                      bastPriceStr = PriceMe.Utility.FormatPrice(bastPrice);
              }
              catch { }

              double score;
              double avR = 0;
              if (double.TryParse(pc.AvRating, out avR))
              {
                  score = double.Parse(pc.AvRating, PriceMeCommon.PriceMeStatic.Provider);
              }
              else
              {
                  score = 0d;
              }

              string StarsScore = PriceMe.Utility.GetStarImage(score);
              string StarsImageAlt = "";
              if (score.ToString("0.0") != "0.0")
              {
                  StarsImageAlt = string.Format(Resources.Resource.TextString_OutOfRating, score.ToString("0.0")).Replace(",", ".");
              }
              else
              {
                  StarsImageAlt = Resources.Resource.TextString_NoRating;
              }


              %>
            <tr class="<%=cssClass%>">
                <td class="thPN">
                    <%if (urltype == 3)
                      { %>
                    <span style="padding-left:5px;"><%=pc.DisplayName%></span>
                    <%}
                      else
                      { %>
                    <a href="<%= url%>"<%if (urltype == 2)
                                         { %> target="_blank" rel="nofollow"<%} %>><%=pc.DisplayName%>
                        <%if (urltype == 2)
                          { %>
                        <span class="glyphicon glyphicon-share iconGray" style="font-size: 12px;"></span>
                        <%} %></a><%} %>

                    <%if (pc.DayCount <= PriceMe.WebConfig.NewDayCount)
                      { %>
                    <span class="glyphicon glyphicon-flash quickFlash"></span>
                    <%} %>
                </td>
                <%if (attributeTitles.Count > 0)
                  {%>
                
                <%foreach (var atID in ProductsAttributes.Keys)
                  {
                      if (ProductsAttributes[atID].ContainsKey(pc.ProductID))
                      {
                        %>
                <td class="attTD"><% var ac = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeValueByID(ProductsAttributes[atID][pc.ProductID]);

                                     if (ac != null)
                                     {
                                         var at = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeTitleByVauleID(ac.AttributeValueID);
                            %>
                    <%=ac.Value%>
                    <%} %>
                </td>
                        <%}
                      else
                      {%>
                <td class="attTD"></td>
                <%} %>

                <%} %>

                <%} %>
                <td class="thPrc"><%=bastPriceStr%></td>
                <td class="tdPc"><%=pc.PriceCount%> <span class="glyphicon glyphicon-shopping-cart"></span></td>
                <td class="thRT"><div class="<%=StarsScore%>P reviewDiv" title="<%=StarsImageAlt%>"></div></td>
            </tr>
          <%} %>
      </table>
<%}
  else
  { %>
<%
      foreach (PriceMeCommon.Data.CatalogManufeaturerProduct cmp in catalogManufeaturerProductList)
      {
      %>
      
      
    <h2 class="quickH2"><%=cmp.ManufacturerCache.LinkText%></h2>

      <table class="quickListTable">

        <%int count = 0;
          string cssClass = "";
          PriceMeCommon.Data.ProductCatalog[] productCatalogArray = cmp.ProductCatalogCollection.OrderBy(pc => pc.ProductName).ToArray();
          foreach (PriceMeCommon.Data.ProductCatalog pc in productCatalogArray)
          {
              int urltype = 1;
              if (count % 2 == 0)
              {
                  cssClass = "sT";
              }
              else
              {
                  cssClass = "pT";
              }
              count++;
              string url = PriceMe.UrlController.GetProductUrl(int.Parse(pc.ProductID), pc.ProductName);
              if (PriceMeCommon.BusinessLogic.CategoryController.IsSearchOnly(pc.CategoryID, PriceMe.WebConfig.CountryId))
              {
                  if (!string.IsNullOrEmpty(pc.BestPPCRetailerName))
                  {
                      urltype = 2;
                      url = PriceMe.Utility.GetRootUrl("/ResponseRedirect.aspx?pid=" + pc.ProductID + "&rid=" + pc.BestPPCRetailerID + "&rpid=" + pc.BestPPCRetailerProductID + "&countryID=" + PriceMe.WebConfig.CountryId + "&cid=" + pc.CategoryID + "&t=c", PriceMe.WebConfig.CountryId).Replace("&", "&amp;");
                      string uuid = Guid.NewGuid().ToString();
                      url += "&uuid=" + uuid;
                  }
                  else
                      urltype = 3;
              }
              double bastPrice = 0;
              string bastPriceStr = "";
              try
              {
                  bastPrice = double.Parse(pc.BestPrice, System.Globalization.NumberStyles.Any, PriceMeCommon.PriceMeStatic.Provider);
                  if (bastPrice != 0)
                      bastPriceStr = PriceMe.Utility.FormatPrice(bastPrice);
              }
              catch { }

              double score;
              double avR = 0;
              if (double.TryParse(pc.AvRating, out avR))
              {
                  score = double.Parse(pc.AvRating, PriceMeCommon.PriceMeStatic.Provider);
              }
              else
              {
                  score = 0d;
              }

              string StarsScore = PriceMe.Utility.GetStarImage(score);
              string StarsImageAlt = "";
              if (score.ToString("0.0") != "0.0")
              {
                  StarsImageAlt = string.Format(Resources.Resource.TextString_OutOfRating, score.ToString("0.0")).Replace(",", ".");
              }
              else
              {
                  StarsImageAlt = Resources.Resource.TextString_NoRating;
              }


              %>
            <tr class="<%=cssClass%>">
                <td class="thPN">
                    <%if (urltype == 3)
                      { %>
                    <span style="padding-left:5px;"><%=pc.DisplayName%></span>
                    <%}
                      else
                      { %>
                    <a href="<%= url%>"<%if (urltype == 2)
                                         { %> target="_blank" rel="nofollow"<%} %>><%=pc.DisplayName%>
                        <%if (urltype == 2)
                          { %>
                        <span class="glyphicon glyphicon-share iconGray" style="font-size: 12px;"></span>
                        <%} %></a><%} %>
                    <%if (pc.IsUpComing)
                      { %>
                    <span class="glyphicon glyphicon-time quickFlash"></span>
                    <%}
                      else
                      { %>
                    <%if (pc.DayCount <= PriceMe.WebConfig.NewDayCount)
                      { %>
                    <span class="glyphicon glyphicon-flash quickFlash"></span>
                    <%}
                      } %>
                </td>
                <%if (attributeTitles.Count > 0)
                  {%>
                
                <%foreach (var atID in ProductsAttributes.Keys)
                  {
                      if (ProductsAttributes[atID].ContainsKey(pc.ProductID))
                      {
                        %>
                <td class="attTD"><% var ac = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeValueByID(ProductsAttributes[atID][pc.ProductID]);

                                     if (ac != null)
                                     {
                                         var at = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeTitleByVauleID(ac.AttributeValueID);
                            %>
                    <%=ac.Value%>
                    <%} %>
                </td>
                        <%}
                      else
                      {%>
                <td class="attTD"></td>
                <%} %>

                <%} %>

                <%} %>
                <td class="thPrc"><%=bastPriceStr%></td>
                <td class="tdPc"><%=pc.PriceCount%> <span class="glyphicon glyphicon-shopping-cart"></span></td>
                <td class="thRT">
                    <%if (StarsImageAlt != Resources.Resource.TextString_NoRating)
                        { %>
                    <div class="star-rating reviewDiv" title="<%=StarsImageAlt %>">
                        <span style="width:<%=(PriceMe.Utility.GetStarRatingPercent(score) * 100 - 1).ToString("0.00")%>%;"></span>
                    </div>
                    <%} %>
                </td>
            </tr>

          <% if (PriceMe.WebConfig.QuickListCount == count)
             {%>
          <tr id="mh<%=cmp.ManufacturerCache.Value%>" style="height:1px;">
            <td></td>
              <%if (attributeTitles.Count > 0)
                {%>
                <td></td><td></td>
                <%} %>
            <td></td>
            <td></td>
            <td></td>
          </tr>
          <%}  %>
          <%}
          if (cmp.NeedDisplayMoreLink)
          {
              string ampStr = "";
              foreach (string mid in cmp.DisplayAllManufeaturerProducts)
              {
                  ampStr += mid + "_";
              }
              ampStr += cmp.ManufacturerCache.Value;

              Dictionary<string, string> _ps = new Dictionary<string, string>(currentPs);

              _ps.Remove("samp");
              _ps.Add("samp", ampStr);

              cmp.MoreLinkUrl = PriceMe.UrlController.GetRewriterUrl(PageTo, _ps);
          %>
          <tr>
            <td><a style="font-size:15px;" href="<%=cmp.MoreLinkUrl%>#mh<%=cmp.ManufacturerCache.Value%>">More <%=cmp.ManufacturerCache.LinkText%></a></td>
            <td></td>
            <td></td>
            <td></td>
          </tr>
          <%} 
           %>
      </table>
      <br />
  <%}
  }%>

  <%}%>
