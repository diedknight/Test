<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewGridView.ascx.cs" Inherits="HotterWinds.Modules.Catalog.NewGridView" %>

<% if (ProductID == null) return; %>

<%int priceCount = int.Parse(PriceCount);

if (priceCount <= 1 && !string.IsNullOrEmpty(BestPPCRetailerName))
{
    int bppcRPID = 0;
    int.TryParse(BestPPCRetailerProductID, out bppcRPID);
    string scriptFormat = "on_clickAndOut('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')";
    VSOnclickScript = string.Format(scriptFormat, Guid.NewGuid(), ProductID, BestPPCRetailerID, BestPPCRetailerProductID,
        ProductName.Replace("'", " ").Replace("\"", "").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "),
        CategoryID, BestPrice, "&ctlgGrid", PriceMe.WebConfig.CountryId, PriceMe.WebConfig.Environment == "prod", ClickOutUrl);

} %>
<div class="product-grid-item" id="item_<%=ProductID %>">
    <div class="checkDiv">
        <input type="checkbox" id="cb_<%=ProductID %>" onchange="addToCompareList(<%=ProductID %>, '<%=Resources.Resource.TextString_MaximumProductsToCompare %>')" value="<%=ProductID %>" />
    </div>

    <%if (IsUpComing)
      { %>
    <div class="NewestDiv">
        <span class="NewTagSpan">Coming soon</span>
	</div>
    <%}
        else
        {
            int z_index = 0;
            if (Sale <= -0.1 && (PrevPrice - CurrentPrice) >= 10)
            {
                z_index++;
                %>
    <div class="NewestDiv sale tag<%=z_index %>">
        <span class="NewTagSpan"><%=Resources.Resource.TextString_SaleTag%></span>
	</div>
    <%
    }
    if (IsTop3 && IsSearchProduct=="0")
    {
            z_index++;
            %>
    <div class="NewestDiv bSeller tag<%=z_index %>">
        <span class="NewTagSpan"><%=Resources.Resource.TextString_BestsellerTag%></span>
	</div>
        <%}
    if (DayCount <= PriceMe.WebConfig.NewDayCount)
    {
                z_index++;
        %>
    <div class="NewestDiv tag<%=z_index %>">
        <span class="NewTagSpan"><%=Resources.Resource.TextString_NewTag%></span>
    </div>
    <%
        }
    } %>
    <%if(!string.IsNullOrEmpty(linkUrl)){ %>
    <a href="<%=linkUrl%>"<%if (isBestPPc){ %> target="_blank" rel="nofollow"<%} %>>
        <img id="productImage<%=ProductID %>" src="https://images.pricemestatic.com/Images/MobileSite/pix1.png" data-pm-src2="<%=DefaultImage%>" alt="<%=ImageAlt%>" title="<%=ImageAlt %>" onerror="onImgError(this)" />
    </a><%}else{ %>
    <div class="gpsh"><img id="productImage<%=ProductID %>" src="https://images.pricemestatic.com/Images/MobileSite/pix1.png" data-pm-src2="<%=DefaultImage%>" alt="<%=ImageAlt%>" title="<%=ImageAlt %>" onerror="onImgError(this)" /></div>
    <%} %>

    <div class="middle-div">
        <%if (!string.IsNullOrEmpty(linkUrl))
          { %>
        <a class="productNameLink" id="productName<%=ProductID%>" href="<%=linkUrl%>"<%if (isBestPPc){ %> target="_blank" rel="nofollow"<%} %>>
            <%=DisplayName%>
            <%if (isBestPPc)
              { %><span class="glyphicon glyphicon-share iconGray" style="font-size: 12px;"></span>
            <%} %> 
            <%if (DayCount <= PriceMe.WebConfig.NewDayCount)
            { %>
            <span class="glyphicon glyphicon-flash"></span>
             <%} %>

        </a><%}
          else
          { %>
        <div class="productNameLink iconGray">
        <%=DisplayName%> <%if (DayCount <= PriceMe.WebConfig.NewDayCount)
                           { %>
            <span class="glyphicon glyphicon-flash"></span>
        <%}%></div>
          <%} %>

        <div id="productReview<%=ProductID %>" class="productReviewDiv">
            <%if (StarsImageAlt != Resources.Resource.TextString_NoRating && !IsUpComing)
              { %>
            <div class="star-rating reviewDiv" title="<%=StarsImageAlt %>">
                <span style="width:<%=(RatingPercent * 100 - 1).ToString("0.00")%>%;"></span>
            </div>
            <%}
              else
              {%>
            &nbsp;
            <%} %>
        </div>

        <div class="bestPPCDiv" id="bestPPC<%=ProductID %>">

            <div id="productPrice<%=ProductID %>" class="priceDiv">
                <%if(IsUpComing){ %>
                <span style="font-size:12px;">Price unknown</span><%}else{ %>
                <%=PriceMe.Utility.FormatPrice(BestPrice)%><%} %>
            </div>

            <div class="priceCountDiv">
            <%if (IsUpComing)
            { %>
                <span style="font-size:12px;">New release</span>
            <%}
            else
            { %>
                <%if (priceCount == 1 && !string.IsNullOrEmpty(BestPPCRetailerName))
                {%>
                <span><%=BestPPCRetailerName%></span>
                <%}
                else
                {%>
                <%=PriceCount%> <span class="glyphicon glyphicon-shopping-cart"></span>

                <%}
            } %>
            </div>
            
        </div>
    </div>
</div>