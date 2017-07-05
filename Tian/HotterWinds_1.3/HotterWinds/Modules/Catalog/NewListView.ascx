<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewListView.ascx.cs" Inherits="HotterWinds.Modules.Catalog.NewListView" %>

<%if (ProductID == null) return; %>

<div class="productInfoDiv_List <%=ClassName %>" id="item_<%=ProductID %>">
    <%if(ShowCompareBox) { %>
    <div class="checkDiv">
        <input type="checkbox" id="cb_<%=ProductID %>" onchange="addToCompareList(<%=ProductID %>, '<%=Resources.Resource.TextString_MaximumProductsToCompare %>')" value="<%=ProductID %>" />
    </div>
    <%} %>

    <%int priceCount = int.Parse(PriceCount);%>

<%if (priceCount <= 1 && !String.IsNullOrEmpty(BestPPCRetailerName) || ParentPage == "retailerpage")
    {
        string fromPage = "&ctlgList";
        if(ParentPage == "retailerpage")
        {
            fromPage = "&rList";
        }
        int bppcRPID = 0;
        int.TryParse(BestPPCRetailerProductID, out bppcRPID);
        string scriptFormat = "on_clickOut('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', " + PriceMe.WebConfig.Use_GoogleTrackConversion + ")";
        VSOnclickScript = string.Format(scriptFormat, Guid.NewGuid(), ProductID, BestPPCRetailerID, BestPPCRetailerProductID,
            ProductName.Replace("'", " ").Replace("\"", "").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "),
            CategoryID, BestPrice, fromPage, PriceMe.WebConfig.CountryId, PriceMe.WebConfig.Environment == "prod");

    } %>
        <img id="productImage<%=ProductID %>" class="sImage" src="https://images.pricemestatic.com/Images/MobileSite/pix1.png" data-pm-src2="<%=DefaultImage%>" 
            alt="<%=ImageAlt%>" title="<%=ImageAlt %>" width="55" height="55" onerror="onImgError(this)" />

    <div class="psdMiddleDiv">
        <%
            if (PriceMeCommon.BusinessLogic.CategoryController.IsSearchOnly(int.Parse(CategoryID), PriceMe.WebConfig.CountryId))
            {
                if (!string.IsNullOrEmpty(BestPPCRetailerName))
                {%>
        <a class="pNameA" href="<%=linkUrl%>" target="_blank" rel="nofollow"><label><%=DisplayName%></label></a>
        <span class="glyphicon glyphicon-share iconGray" style="font-size: 12px;"></span>
            <%if (IsUpComing)
              { %>
            <span class="listNewestDiv"><%=Resources.Resource.TextString_ComingSoonTag%></span>
            <%}
              else
              { %>
            <%if (Sale <= -0.1 && (PrevPrice - CurrentPrice) >= 10)
                { %>
            <span class="listNewestDiv sale"><%=Resources.Resource.TextString_SaleTag%></span>
            <%} else if(IsTop3 && IsSearchProduct=="0")
                {
            %>
            <span class="listNewestDiv bSeller"><%=Resources.Resource.TextString_BestsellerTag%></span>
            <%
                }
                else if (DayCount <= PriceMe.WebConfig.NewDayCount)
                {
                    %>
            <span class="listNewestDiv"><%=Resources.Resource.TextString_NewTag%></span>
            <span class="glyphicon glyphicon-flash"></span>
                    
            <%
                      }
                  } %>
        <%}
                else
                { %>
        <span class="pNameA iconGray"><%=DisplayName%>
            <%if (IsUpComing)
              { %>
            <span class="listNewestDiv"><%=Resources.Resource.TextString_ComingSoonTag%></span>
            <span class="glyphicon glyphicon-flash"></span>
            <%}
              else
              { %>
            <%if (DayCount <= PriceMe.WebConfig.NewDayCount)
              { %><span class="listNewestDiv">
                <%=Resources.Resource.TextString_NewTag%>
	        </span>
            <span class="glyphicon glyphicon-flash"></span>
            <%} %>
        <%}%>
            </span>
            <%
                }
            }
            else
            { %>
        <a class="pNameA" href="<%=CatalogProductURL%>"><label><%=DisplayName%></label></a>
        <%if (IsUpComing)
              { %>
            <span class="listNewestDiv"><%=Resources.Resource.TextString_ComingSoonTag%></span>
            <span class="glyphicon glyphicon-flash"></span>
            <%}
              else
              { %>
            <%if (Sale <= -0.1 && (PrevPrice - CurrentPrice) >= 10)
                { %>
            <span class="listNewestDiv sale"><%=Resources.Resource.TextString_SaleTag%></span>
            <%} else if(IsTop3 && IsSearchProduct=="0")
                {
            %>
            <span class="listNewestDiv bSeller"><%=Resources.Resource.TextString_BestsellerTag%></span>
            <%
                }
                else if (DayCount <= PriceMe.WebConfig.NewDayCount)
                {
                    %>
            <span class="listNewestDiv"><%=Resources.Resource.TextString_NewTag%></span>
            <span class="glyphicon glyphicon-flash"></span>
                    
            <%
                      }
                  } %>
        <%} %>

        <div class="attrDesc"><%=AttrDescription %></div>
    </div>
    <div class="comparePriceDiv">
        
            <%if (!IsUpComing)
              { %>
        <div class="btnVS">
            <%
                  if (ParentPage == "c" || (ParentPage == "retailerpage" && PriceMeCommon.BusinessLogic.RetailerController.IsPPcRetailer(int.Parse(BestPPCRetailerID), PriceMe.WebConfig.CountryId)))
                  {
                      if (PriceMeCommon.BusinessLogic.CategoryController.IsSearchOnly(int.Parse(CategoryID), PriceMe.WebConfig.CountryId))
                      {
                          if (!string.IsNullOrEmpty(BestPPCRetailerName))
                          { %>
            <a class="btn btn-warning btn-xs btn-vs" href="<%=linkUrl%>" rel="nofollow" target="_blank" onclick="<%=VSOnclickScript%>"><span class="glyphicon glyphicon-chevron-right"></span><span class="compareText"><%=Resources.Resource.TextString_VisitShop%></span></a>
                        <%}
                      }
                      else
                      {
                          if ((priceCount == 1 && !string.IsNullOrEmpty(BestPPCRetailerName)) || ParentPage == "retailerpage")
                          {
            %>
            <a class="btn btn-warning btn-xs btn-vs" href="<%=ClickOutUrl%>" rel="nofollow" target="_blank" onclick="<%=VSOnclickScript%>"><span class="glyphicon glyphicon-chevron-right"></span><span class="compareText"><%=Resources.Resource.TextString_VisitShop%></span></a>
            <%
                          }
                          else
                          {
            %>
            <a class="btn btn-warning btn-xs btn-cp" href="<%=CatalogProductURL%>"><span class="glyphicon glyphicon-chevron-right"></span><span class="compareText"><%=ComparePriceString%></span></a>
            <%
                          }
                      }
                  }%>
            </div>
            <%
              }
            %>
        

        <%if (ParentPage != "retailerpage")
       { %>
           <div class="bestPPCDiv" id="bestPPC<%=ProductID %>">
               <%if (IsUpComing)
                 { %>
               <span style="font-size:12px;">New release</span>
               <%}
                else
                {
                    if ((priceCount == 1 && !string.IsNullOrEmpty(BestPPCRetailerName)))
                    {
                    %>
                <span class="brn"><%=BestPPCRetailerName %></span>
                <%}
                else { %>

                <%=priceCount%> <span class="glyphicon glyphicon-shopping-cart"></span>

                <%}
                } %>
        </div>
        <%} %>
    </div>
    <div class="col-prices">
        <% if (!string.IsNullOrEmpty(BestPPCRetailerID) && !PriceMeCommon.BusinessLogic.RetailerController.IsPPcRetailer(int.Parse(BestPPCRetailerID), PriceMe.WebConfig.CountryId)){%>
        <div class="priceDiv nolinkPrice">
            <%} else { %>
        <div class="priceDiv">
            <%} %>
            <%if(IsUpComing){ %>
            <span style="font-size:12px;">Price unknown</span>
            <%}else{ %>
                <%=PriceMe.Utility.FormatPrice(BestPrice)%><%} %>
        </div>
    </div>
    <div class="col-review">
        <%if (StarsImageAlt != Resources.Resource.TextString_NoRating) { %>
        <div class="reviewDiv">
            <div class="star-rating reviewDiv" title="<%=StarsImageAlt %>">
                <span style="width:<%=(RatingPercent * 100 - 1).ToString("0.00")%>%;"></span>
            </div>
        </div>
        <div class="reviewCount"><%=ReviewCountString %></div>
        <%} %>
    </div>
</div>