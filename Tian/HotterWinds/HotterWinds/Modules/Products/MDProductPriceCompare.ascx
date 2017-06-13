<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MDProductPriceCompare.ascx.cs" Inherits="HotterWinds.Modules.Products.MDProductPriceCompare" %>

<%@ Import Namespace="PriceMeCommon.Extend" %>
<%@ Import Namespace="PriceMe" %>
<%@ Import Namespace="PriceMeCommon.BusinessLogic" %>

<% 
    string scriptFormat = "on_clickOut('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', " + PriceMe.WebConfig.Use_GoogleTrackConversion + ")";
    string VSOnclickScript = string.Format(scriptFormat, Guid.NewGuid(), ProductID, RetailerId, RetailerProductID,
        ProductName.Replace("'", " ").Replace("\"", "").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "),
        CategoryId, RetailerPrice.ToString("0.00"), "&t=" + type, WebConfig.CountryId, WebConfig.Environment == "prod");

    string RetailerProductURL = Utility.GetRootUrl("/ResponseRedirect.aspx?aid=40&pid=" + ProductID + "&rid=" + RetailerId + "&rpid=" + RetailerProductID + "&countryID=" + WebConfig.CountryId + "&cid=" + CategoryId + "&t=" + type + "&rpp=" + RetailerPrice.ToString("0.00") + "&pos=" + PricePosition + "&tpos=" + PricePositionCount + "&st=" + StockStatus + "&loc=" + Loc, WebConfig.CountryId).Replace("&", "&amp;");
    string uuid = Guid.NewGuid().ToString();
    RetailerProductURL += "&uuid=" + uuid;
    
    if (retailer != null)
    {
        int count = 0;
        string rowClass = string.Empty;
        Regex reg = new Regex(@"ctl(?<count>[\d]*)_ProductPriceCompare1");
        Match mat = reg.Match(this.ClientID);
        if (mat != null)
            int.TryParse(mat.Groups["count"].Value, out count);
        if ((count % 2) == 1)
            rowClass = " pricesDivRowBg";

        string retailerLogo = retailer.LogoFile.GetSpecialSize("ms");
        if(!retailerLogo.Contains("http://") && !retailerLogo.Contains("https://"))
            retailerLogo = retailerLogo.FixUrl(Resources.Resource.ImageWebsite2);

        PriceMeCommon.Data.DBCountryInfo countryInfo;
        if (RetailerController.IsCompleteRetailer(retailer.RetailerId, WebConfig.CountryId))
            countryInfo = RetailerController.GetUtilCountry(retailer.RetailerCountry);
        else
            countryInfo = RetailerController.GetUtilCountry(WebConfig.CountryId);

        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("retailerId", retailer.RetailerId.ToString());
        string retailerUrl = UrlController.GetRewriterUrl(PageName.RetailerInfo, param);
        var singleTms = RetailerController.NZTradeMeSellers_Static.SingleOrDefault(w => w.id == TradeMeSellerId);
        var isTradeMe = (TradeMeSellerId??0) > 0 ;
        var isExpirDate = singleTms != null && singleTms.RetailerId <= 0 && DateTime.Parse(ExpirationDate) >= DateTime.Now;
        bool delInfo = string.IsNullOrEmpty(retailer.DeliveryInfo);
        if (!IsNoLink)
        {
%>
            <%if (isTradeMe)
              {
              %>
                <%if (isExpirDate) { %>
                    <div class="productlist bg1 pricesDiv<%=rowClass %> <%=featuredProductCSS%>" id="pricesRow<%=RetailerProductID %>" style="padding: 1px 3px 1px 5px;">
                    <div class="pricesDivRetailer">
                        <div class="pricesDivRetailerInfo" style="margin-top:16px;">
                           <label class="pricesDivRetailerNameNoLink" isNull="<%=singleTms != null %>" retailerid="<%=singleTms.RetailerId <= 0 %>" exDate="<%=DateTime.Parse(ExpirationDate)  >= DateTime.Now %>" testID="1">
                                Trade Me</label>
                        </div>
                        <div class="pricesDivInfo" style="padding-left: 0; padding-top: 11px;">
                            <div class="cursor ProductLink" onclick="<%= PriceMe.WebConfig.Environment == "prod" ? "javascript:ga('send', 'event', 'retailerinfo', 'product', '"+RetailerProductID+"');":""%>">
                                <a href="<%=retailerUrl %>">
                                    <div class="flag">
                                        <img width="16" height="11" title="<%=string.Format(Resources.Resource.TextString_Infoabout,retailer.RetailerName) %>" alt="<%=countryInfo.KeyName %>" src="<%=Resources.Resource.ImageWebsite + countryInfo.CountryFlag %>" /></div>
                                </a>
                                <div data-toggle="modal" data-target="#retailerInfo" class="pricesDivInfoRating" onclick="ShowMoreDetail(<%=RetailerProductID %>,'p','r')">
                                    <%if (retailer.AvRating > 0)
                                      {
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", ".");%>
                                    <div class="npStarsDiv">
                                        <div class="smallstar-rating reviewDiv" title="<%=avRating %>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                        <div class="String"><a><%=retailer.ReviewString%></a></div>
                                    </div>
                                    <%}
                                      else
                                      {
                                          string aavRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", ".");%>
                                    <div class="npStarsDiv" style="margin-top: 6px;">
                                        <div class="<%=Utility.GetStarImage(retailer.AvRating) %>" title="<%=aavRating%>"></div>
                                        <div class="String"><a><%=retailer.ReviewString%></a></div>
                                    </div>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pricesDivPdName pricesDivPdNameMD" style="height: 50px;">
                        <div class="vertical">

                            <span style="font-weight: normal;color:#666666;"><%=ProductName.Length > 75 ? ProductName.Substring(0, 70) + "..." : ProductName%></span>

                            <%if (condition != null)
                              { %>
                            <span class="pricesCondition" title="<%=condition.ConditionDescription %>"><%=condition.ConditionName %></span>
                            <%}

                              if (IsFeaturedProduct)
                              { %>

                            <span class="pricesFeatured" title="Featured store">Featured store</span>

                            <%} %>

                           <div class="retailerMSG sourceMSG" style="height: 14px;font-weight: normal;color:#666666;">
                               Seller: <%=singleTms.MemberName%>, Buy Now
                            </div>
                        </div>
                    </div>
                    <div class="ppricesDivStock">
                        <%if (!string.IsNullOrEmpty(itempropStock))
                          { %>
                        <div title="<%=Stock%>" class="bg1 stock<%=stockImg %>"></div>
                        <%}
                          else
                          { %><div class="stock">&nbsp;</div>
                        <%} %>
                    </div>
                    <div class="ppricesDivCcfee">
                        <%--<%if (!string.IsNullOrEmpty(ccFeeString))
                                    { %>
                                <div class="bg1 stock ccfee" title="A <%=ccFeeString%> credit card charge may apply">&nbsp;</div><%}else{ %><div class="stock">&nbsp;</div><%} %>--%>

                        <%if (isChristmasDate)
                          { %>
                        <span class="glyphicon glyphicon-tree-conifer" title="<%=christmasString%>"></span>
                        <%}
                          else if (!string.IsNullOrEmpty(ccFeeString))
                          {%>
                        <div class="bg1 stock ccfee" title="A <%=ccFeeString%> credit card charge may apply">&nbsp;</div>
                        <% }
                          else
                          { %>
                        <div class="lastDev">&nbsp;</div>
                        <%} %>
                    </div>
                    <div class="pricesDivPrice">
                        <%--<div class="bubble"><%= GetLastModify(LastModify) %></div>--%>
                        <a class="PriceLarge rplistPrice" href="<%=RetailerProductURL %>" style="color:#999 !important;font-weight:normal" rel="nofollow" target="_blank">
                            <span class="ComparePrice" onclick="<%=VSOnclickScript %>">
                                <% var largePrice = FormatDecimal(RetailerPrice);
                                        if (largePrice.Contains("."))
                                        {
                                            var lastIndex = largePrice.LastIndexOf(".") + 1;
                                            var np1 = largePrice.Substring(0, lastIndex);
                                            var np2 = largePrice.Substring(lastIndex, largePrice.Length - lastIndex);
                                            if ((largePrice.Length - lastIndex) == 9)
                                                largePrice = np1.Replace("priceSpan", "") + "<span style='position: relative;top: -0.2em;font-size: 0.8em; font-weight:normal;'>" + np2 + "</span>";
                               }
                                %>
                                <%=largePrice%>

                            </span>
                        </a>
                    </div>
                    <div class="pricesDivShopping" style="margin: 19px 5px 0 0; position:relative;" title="<%=!delInfo?"Indicative shipping: "+retailer.DeliveryInfo:"" %>">

                       

                          <%
                      string shipping = GetProductFees();
                          if (shipping != "&nbsp;")
                          { %>
                         <div class="bg1 pricesDivPriceFees" title="<%=Resources.Resource.TextString_Shipping %>">
                            <%=shipping %>
                        </div>
                        
                        <%}
                                    else
                                    {%>
                         <%
                             
                            if (!delInfo) { %>
                            <div class="" style="background-image:url('https://images.pricemestatic.com/Images/PriceMeNewDesign/PriceMe_bgnew_161117.png');background-position: 70px -4121px;height:20px;" ></div>
                            <span class="glyphicon glyphicon-info-sign" aria-hidden="true" style="color:rgb(108,211,250);font-size:10px;position:absolute;top:-1px;right:3px"></span>
                          <%}else{%>
                                <div class="bg1 pricesDivPriceFeesUn" title="<%=Resources.Resource.TextString_ShippingUnknown %>">
                            <%=shipping %>
                        </div>
                            <%} %>
                       
                        <%}%>

                       
                    </div>
                    <div class="pricesDivVS" style="padding-top:16px;">
                        <div class="btnVS exDateTred" style="width: auto;margin-left: 0px;color:#666666;font-size:10px;">Closes <%=GetProductDate(DateTime.Parse(ExpirationDate)) %> </div>
                    </div>
                </div>
                  <%}%>
                
              <%}else{%>
              
                <div class="productlist bg1 pricesDiv<%=rowClass %> <%=featuredProductCSS%>" id="pricesRow<%=RetailerProductID %>" style="padding: 1px 3px 1px 5px;">
                    <div class="pricesDivRetailer">
                        <div class="pricesDivRetailerInfo" style="margin-top: 6px;">
                            <%if (!retailer.LogoFile.Contains("."))
                              { %>
                            <a href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" rel="nofollow" target="_blank" class="addLabel"><%=retailer.RetailerName%></a>
                            <%}
                              else
                              { %>
                            <div class="divImgPadding">
                                <a href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" rel="nofollow" target="_blank">
                                    <img class="plimg" src='<%=retailerLogo%>' width="90" height="90" alt="<%=retailer.RetailerName%>" /></a>
                            </div>
                            <%}%>
                        </div>
                        <div class="pricesDivInfo" style="padding-left: 0; padding-top: 11px;">
                            <div class="cursor ProductLink" onclick="<%= PriceMe.WebConfig.Environment == "prod" ? "javascript:ga('send', 'event', 'retailerinfo', 'product', '"+RetailerProductID+"');":""%>">
                                <a href="<%=retailerUrl %>">
                                    <div class="flag">
                                        <img width="16" height="11" title="<%=string.Format(Resources.Resource.TextString_Infoabout,retailer.RetailerName) %>" alt="<%=countryInfo.KeyName %>" src="<%=Resources.Resource.ImageWebsite + countryInfo.CountryFlag %>" /></div>
                                </a>
                                <div data-toggle="modal" data-target="#retailerInfo" class="pricesDivInfoRating" onclick="ShowMoreDetail(<%=RetailerProductID %>,'p','r')">
                                    <%if (retailer.AvRating > 0)
                                      {
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", ".");%>
                                    <div class="npStarsDiv">
                                        <div class="smallstar-rating reviewDiv" title="<%=avRating%>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                        <div class="String"><a><%=retailer.ReviewString%></a></div>
                                    </div>
                                    <%}
                                      else
                                      {
                                          string aavRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", ".");%>
                                    <div class="npStarsDiv" style="margin-top: 6px;">
                                        <div class="smallstar-rating reviewDiv" title="<%=aavRating%>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                        <div class="String"><a><%=retailer.ReviewString%></a></div>
                                    </div>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pricesDivPdName pricesDivPdNameMD" style="height: 50px;">
                        <div class="vertical">

                            <a href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" class="trackName font-Bold" rel="nofollow" target="_blank"><%=ProductName.Length > 75 ? ProductName.Substring(0, 70) + "..." : ProductName%></a>

                            <%if (condition != null)
                              { %>
                            <span class="pricesCondition" title="<%=condition.ConditionDescription %>"><%=condition.ConditionName %></span>
                            <%}

                              if (IsFeaturedProduct)
                              { %>

                            <span class="pricesFeatured" title="Featured store">Featured store</span>

                            <%} %>

                            <%if (!IsNoLink && !String.IsNullOrEmpty(retailer.RetailerMessage))
                              {%>
                            <div class="retailerMSG sourceMSG" style="height: 14px;">
                                <%=retailer.RetailerMessage%>
                            </div>
                            <%}%>
                        </div>
                    </div>
                    <div class="ppricesDivStock">
                        <%if (!string.IsNullOrEmpty(itempropStock))
                          { %>
                        <div title="<%=Stock%>" class="bg1 stock<%=stockImg %>"></div>
                        <%}
                          else
                          { %><div class="stock">&nbsp;</div>
                        <%} %>
                    </div>
                    <div class="ppricesDivCcfee">
                        <%--<%if (!string.IsNullOrEmpty(ccFeeString))
                                    { %>
                                <div class="bg1 stock ccfee" title="A <%=ccFeeString%> credit card charge may apply">&nbsp;</div><%}else{ %><div class="stock">&nbsp;</div><%} %>--%>

                        <%if (isChristmasDate)
                          { %>
                        <span class="glyphicon glyphicon-tree-conifer" title="<%=christmasString%>"></span>
                        <%}
                          else if (!string.IsNullOrEmpty(ccFeeString))
                          {%>
                        <div class="bg1 stock ccfee" title="A <%=ccFeeString%> credit card charge may apply">&nbsp;</div>
                        <% }
                          else
                          { %>
                        <div class="lastDev">&nbsp;</div>
                        <%} %>
                    </div>
                    <div class="pricesDivPrice">
                        <%--<div class="bubble"><%= GetLastModify(LastModify) %></div>--%>
                        <a class="PriceLarge rplistPrice" href="<%=RetailerProductURL %>" rel="nofollow" target="_blank">
                            <span class="ComparePrice" onclick="<%=VSOnclickScript %>">
                                <% var largePrice = FormatDecimal(RetailerPrice);
                                        if (largePrice.Contains("."))
                                        {
                                            var lastIndex = largePrice.LastIndexOf(".") + 1;
                                            var np1 = largePrice.Substring(0, lastIndex);
                                            var np2 = largePrice.Substring(lastIndex, largePrice.Length - lastIndex);
                                            if ((largePrice.Length - lastIndex) == 9)
                                                largePrice = np1 + "<span style='position: relative;top: -0.2em;font-size: 0.8em; font-weight:normal;'>" + np2 + "</span>";
                               }
                                %>
                                <%=largePrice%>

                            </span>
                        </a>
                    </div>
                    <div class="pricesDivShopping" style="margin: 19px 5px 0 0;position:relative;" title="<%=!delInfo?"Indicative shipping: "+retailer.DeliveryInfo:"" %>">
                       

                         <%
                    string shipping = GetProductFees();
                          if (shipping != "&nbsp;")
                          { %>
                        
                        <div class="bg1 pricesDivPriceFees" title="<%=Resources.Resource.TextString_Shipping %>">
                            <%=shipping %>
                        </div>
                        <%}
                                    else
                                    {%>

                         <%
                            
                            if (!delInfo) { %>
                             <div class="" style="background-image:url('https://images.pricemestatic.com/Images/PriceMeNewDesign/PriceMe_bgnew_161117.png');background-position: 70px -4121px;height:20px;" ></div>
                            <span class="glyphicon glyphicon-info-sign" aria-hidden="true" style="color:rgb(108,211,250);font-size:10px;position:absolute;top:-1px;right:3px;"></span>
                          <%}else{%>
                                <div class="bg1 pricesDivPriceFeesUn" title="<%=Resources.Resource.TextString_ShippingUnknown %>">
                            <%=shipping %>
                        </div>
                            <%} %>
                        
                        <%}%>
                        
                    </div>
                    <div class="pricesDivVS">
                        <%
                                    if (condition == null)
                                    { %>
                        <div class="btnVS">
                            <a class="btn btnProduct btn-xs" href="<%=RetailerProductURL %>" rel="nofollow" target="_blank" onclick="<%=VSOnclickScript %>"><%=Resources.Resource.TextString_VisitShop%></a>
                            <a class="btnsmall" href="<%=RetailerProductURL %>" rel="nofollow" target="_blank" onclick="<%=VSOnclickScript %>"><span class="glyphicon glyphicon-chevron-right"></span></a>
                        </div>
                        <%}
                                        else
                                        { %>
                        <div class="btnVSC">
                            <a class="btn btnCond btn-xs" href="<%=RetailerProductURL %>" rel="nofollow" target="_blank" onclick="<%=VSOnclickScript %>"><%=Resources.Resource.TextString_VisitShop%></a>
                            <a class="btncsmall" href="<%=RetailerProductURL %>" rel="nofollow" target="_blank" onclick="<%=VSOnclickScript %>"><span class="glyphicon glyphicon-chevron-right"></span></a>
                        </div>
                        <%} %>
                    </div>
                </div>
              <%} %>
                
<%
        }
        else
        { %>
                <%if (isTradeMe)
                  { %>
                        
                    <%if (isExpirDate) { %>
                        <div class="bg1 pricesDivNoLink<%=rowClass %>">
                    <div class="pricesDivRetailer">
                        <div class="pricesDivRetailerInfo" style="margin-top:10px;">
                            <label class="pricesDivRetailerNameNoLink" isNull="<%=singleTms != null %>" retailerid="<%=singleTms.RetailerId <= 0 %>" exDate="<%=DateTime.Parse(ExpirationDate)  >= DateTime.Now %>" testID="2">
                               Trade Me</label>
                        </div>
                        <div class="pricesDivInfo pricesDivInfoNoLink" style="padding-left: 0;padding-top:5px;">
                            <div class="cursor ProductLink" onclick="<%= PriceMe.WebConfig.Environment == "prod" ? "javascript:ga('send', 'event', 'retailerinfo', 'product', '"+RetailerProductID+"');":""%>">
                                <a href="<%=retailerUrl %>">
                                    <div class="flag">
                                        <img width="16" height="11" title="<%=string.Format(Resources.Resource.TextString_Infoabout,retailer.RetailerName) %>" alt="<%=countryInfo.KeyName %>" src="<%=Resources.Resource.ImageWebsite + countryInfo.CountryFlag %>" /></div>
                                </a>
                                <div data-toggle="modal" data-target="#retailerInfo" class="pricesDivInfoRatingNoLink" onclick="ShowMoreDetail(<%=RetailerProductID %>,'p','r')">
                                    <%if (retailer.AvRating > 0)
                                      {
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", ".");%>
                                    <div class="npStarsDiv">
                                        <div class="smallstar-rating reviewDiv" title="<%=avRating %>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                    </div>
                                    <%}
                                      else
                                      {
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", ".");%>
                                    <div class="npStarsDiv">
                                        <div class="smallstar-rating reviewDiv" title="<%=avRating %>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                    </div>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pricesDivPdNameNoLink pricesDivPdNameMDNL">
                        <label style="font-weight: normal;color:#666666;"><%=ProductName%></label>
                        <%if (condition != null)
                          { %>
                        <span class="pricesCondition" title="<%=condition.ConditionDescription %>"><%=condition.ConditionName %></span>
                        <%} %>

                        <div style="height: 14px;font-weight: normal;color:#666666;" class="retailerMSG">Seller: <%=singleTms.MemberName%>, Buy Now</div>
                    </div>
                    <div class="ppricesDivStock pricesDivStockNoLink">
                        <%if (!string.IsNullOrEmpty(itempropStock))
                          { %>
                        <div title="<%=Stock%>" class="bg1 stock<%=stockImg %>"></div>
                        <%}
                          else
                          { %><div class="stock">&nbsp;</div>
                        <%} %>
                    </div>
                    <div class="ppricesDivCcfee pricesDivStockNoLink">
                        <%if (!string.IsNullOrEmpty(ccFeeString))
                          { %>
                        <div class="bg1 stock ccfee" title="A <%=ccFeeString%> credit card charge may apply"></div>
                        <%}
                          else
                          { %><div class="stock">&nbsp;</div>
                        <%} %>
                    </div>
                    <div class="pricesDivPriceNoLink" style="padding-top:5px;">
                        <div class="PriceLarge nolinkPrice">
                            <%--<div class="bubble"><%= GetLastModify(LastModify) %></div>--%>
                              <% var nolinkPrice = FormatDecimal(RetailerPrice);
                                 if (nolinkPrice.Contains("."))
                                 {
                                     var lastIndex = nolinkPrice.LastIndexOf(".") + 1;
                                     var np1 = nolinkPrice.Substring(0, lastIndex);
                                     var np2 = nolinkPrice.Substring(lastIndex, nolinkPrice.Length - lastIndex);
                                     if ((nolinkPrice.Length - lastIndex) == 9)
                                         nolinkPrice = np1.Replace("priceSpan", "") + "<span style='position: relative;top: -0.2em;font-size: 0.8em; font-weight:normal;'>" + np2 + "</span>";
                                }
                        %>
                            <span style="color:#666666 !important;font-weight:normal"><%= nolinkPrice%></span>
                        </div>
                    </div>
                    <div class="pricesDivShopping pricesDivShoppingNoLink" style="padding-top:4px;position:relative;" title="<%=!delInfo?"Indicative shipping: "+retailer.DeliveryInfo:"" %>">
                      


                        <%  string shipping = GetProductFees();
                          if (shipping != "&nbsp;")
                          { %>
                      
                        <div class="bg1 pricesDivPriceFees" title="<%=Resources.Resource.TextString_Shipping %>">
                            <%=shipping %>
                        </div>
                        <%}
                                    else
                                    {%>

                          <%
                           
                            if (!delInfo) { %>
                              <div class="" style="background-image:url('https://images.pricemestatic.com/Images/PriceMeNewDesign/PriceMe_bgnew_161117.png');background-position: 70px -4121px;height:20px;" ></div>
                            <span class="glyphicon glyphicon-info-sign" aria-hidden="true" style="color:rgb(108,211,250);font-size:10px;position:absolute;top:-1px;right:3px;"></span>
                          <%}else{%>
                                  <div class="bg1 pricesDivPriceFeesUn" title="<%=Resources.Resource.TextString_ShippingUnknown %>">
                            <%=shipping %>
                        </div>
                            <%} %>

                        
                        <%}%>
                       
                    </div>
                    <div class="pricesDivVSNoLink" style=" padding-top:10px;"><div class="btnVS exDateTred" style="width: 150px;margin-left: 0px;color:#666666; font-size:10px;">Closes <%=GetProductDate(DateTime.Parse(ExpirationDate)) %> </div></div>
                </div>
                      <%} %>


                  <%}else{%>
                    <div class="bg1 pricesDivNoLink<%=rowClass %>">
                    <div class="pricesDivRetailer">
                        <div class="pricesDivRetailerInfo">
                            <label class="pricesDivRetailerNameNoLink">
                                <%=retailer.RetailerName%></label>
                        </div>
                        <div class="pricesDivInfo pricesDivInfoNoLink" style="padding-left: 0;">
                            <div class="cursor ProductLink" onclick="<%= PriceMe.WebConfig.Environment == "prod" ? "javascript:ga('send', 'event', 'retailerinfo', 'product', '"+RetailerProductID+"');":""%>">
                                <a href="<%=retailerUrl %>">
                                    <div class="flag">
                                        <img width="16" height="11" title="<%=string.Format(Resources.Resource.TextString_Infoabout,retailer.RetailerName) %>" alt="<%=countryInfo.KeyName %>" src="<%=Resources.Resource.ImageWebsite + countryInfo.CountryFlag %>" /></div>
                                </a>
                                <div data-toggle="modal" data-target="#retailerInfo" class="pricesDivInfoRatingNoLink" onclick="ShowMoreDetail(<%=RetailerProductID %>,'p','r')">
                                    <%if (retailer.AvRating > 0)
                                      {
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", ".");%>
                                    <div class="npStarsDiv">
                                        <div class="smallstar-rating reviewDiv" title="<%=avRating %>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                    </div>
                                    <%}
                                      else
                                      {
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", ".");%>
                                    <div class="npStarsDiv">
                                        <div class="smallstar-rating reviewDiv" title="<%=avRating %>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                    </div>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pricesDivPdNameNoLink pricesDivPdNameMDNL">
                        <label style="font-weight: normal;"><%=ProductName%></label>
                        <%if (condition != null)
                          { %>
                        <span class="pricesCondition" title="<%=condition.ConditionDescription %>"><%=condition.ConditionName %></span>
                        <%} %>
                    </div>
                    <div class="ppricesDivStock pricesDivStockNoLink">
                        <%if (!string.IsNullOrEmpty(itempropStock))
                          { %>
                        <div title="<%=Stock%>" class="bg1 stock<%=stockImg %>"></div>
                        <%}
                          else
                          { %><div class="stock">&nbsp;</div>
                        <%} %>
                    </div>
                    <div class="ppricesDivCcfee pricesDivStockNoLink">
                        <%if (!string.IsNullOrEmpty(ccFeeString))
                          { %>
                        <div class="bg1 stock ccfee" title="A <%=ccFeeString%> credit card charge may apply"></div>
                        <%}
                          else
                          { %><div class="stock">&nbsp;</div>
                        <%} %>
                    </div>
                    <div class="pricesDivPriceNoLink">
                        <div class="PriceLarge nolinkPrice">
                            <%--<div class="bubble"><%= GetLastModify(LastModify) %></div>--%>
                              <% var nolinkPrice = FormatDecimal(RetailerPrice);
                                 if (nolinkPrice.Contains("."))
                                 {
                                     var lastIndex = nolinkPrice.LastIndexOf(".") + 1;
                                     var np1 = nolinkPrice.Substring(0, lastIndex);
                                     var np2 = nolinkPrice.Substring(lastIndex, nolinkPrice.Length - lastIndex);
                                     if ((nolinkPrice.Length - lastIndex) == 9)
                                        nolinkPrice = np1 + "<span style='position: relative;top: -0.2em;font-size: 0.8em; font-weight:normal;'>" + np2 + "</span>";
                                }
                        %>
                            <span><%= nolinkPrice%></span>
                        </div>
                    </div>
                    <div class="pricesDivShopping pricesDivShoppingNoLink" style="position:relative;" title="<%=!delInfo?"Indicative shipping: "+retailer.DeliveryInfo:"" %>">
                      


                         <%string shipping = GetProductFees();
                          if (shipping != "&nbsp;")
                          { %>

                         <div class="bg1 pricesDivPriceFees" title="<%=Resources.Resource.TextString_Shipping %>">
                            <%=shipping %>
                        </div>

                        <%}
                                    else
                                    {%>
                          <%
                            
                            if (!delInfo) { %>
                             <div class="" style="background-image:url('https://images.pricemestatic.com/Images/PriceMeNewDesign/PriceMe_bgnew_161117.png');background-position: 70px -4121px;height:20px;" ></div>
                            <span class="glyphicon glyphicon-info-sign" aria-hidden="true" style="color:rgb(108,211,250);font-size:10px;position:absolute;top:-1px;right:3px"></span>
                          <%}else{%>
                                <div class="bg1 pricesDivPriceFeesUn" title="<%=Resources.Resource.TextString_ShippingUnknown %>">
                            <%=shipping %>
                        </div>
                            <%} %>

                        
                        <%}%>
                       
                    </div>
                    <div class="pricesDivVSNoLink"></div>
                </div>
                  <%} %>

        <%}
        } %>
