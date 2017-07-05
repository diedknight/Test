<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductItemPriceCompare.ascx.cs" Inherits="HotterWinds.Modules.Products.ProductItemPriceCompare" %>

<%@ Import Namespace="PriceMeCache" %>
<%@ Import Namespace="PriceMeCommon" %>
<%@ Import Namespace="PriceMeCommon.Extend" %>
<%@ Import Namespace="PriceMeCommon.BusinessLogic" %>
<%@ Import Namespace="PriceMe" %>

<% 
    string scriptFormat = "on_clickOut('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', " + PriceMe.WebConfig.Use_GoogleTrackConversion + ")";
    string VSOnclickScript = string.Format(scriptFormat, Guid.NewGuid(), ProductID, RetailerId, RetailerProductID,
        ProductName.Replace("'", " ").Replace("\"", "").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "),
        CategoryId, RetailerPrice.ToString("0.00"), "&amp;t=p" /*+ "&rpp=" + RetailerPrice.ToString("0.00") + "&pos=" + PricePosition + "&tpos=" + PricePositionCount + "&st=" + StockStatus + "&loc=" + Loc*/, WebConfig.CountryId, WebConfig.Environment == "prod");
    //  VSOnclickScript = "RecordabTesting('product price compare','');" + VSOnclickScript;

    string RetailerProductURL = Utility.GetRootUrl("/ResponseRedirect.aspx?aid=40&pid=" + ProductID + "&rid=" +
        RetailerId + "&rpid=" + RetailerProductID + "&countryID=" + WebConfig.CountryId + "&cid=" +
        CategoryId + "&t=p" + "&rpp=" + RetailerPrice.ToString("0.00") + "&pos=" + PricePosition + "&tpos=" + PricePositionCount + "&st=" + StockStatus + "&loc=" + Loc, WebConfig.CountryId);
    string uuid = Guid.NewGuid().ToString();
    RetailerProductURL += "&uuid=" + uuid;
    
    string rName = "";
    string rowClass = string.Empty;
    if (retailer != null)
    {
        int count = 0;
        Regex reg = new Regex(@"ctl(?<count>[\d]*)_ProductItemPriceCompare1");
        Match mat = reg.Match(this.ClientID);
        if (mat != null)
            int.TryParse(mat.Groups["count"].Value, out count);
        if ((count % 2) == 1)
            rowClass = " pricesDivRowBg";

        rName = retailer.RetailerName;

        string retailerLogo = retailer.LogoFile.GetSpecialSize("ms");
        if (!retailerLogo.Contains("http://") && !retailerLogo.Contains("https://"))
            retailerLogo = retailerLogo.FixUrl(Resources.Resource.ImageWebsite2);

        PriceMeCommon.Data.DBCountryInfo countryInfo;
        if (RetailerController.IsInternationalRetailer(retailer.RetailerId, WebConfig.CountryId))
            countryInfo = RetailerController.GetUtilCountry(retailer.RetailerCountry);
        else
            countryInfo = RetailerController.GetUtilCountry(WebConfig.CountryId);

        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("retailerId", retailer.RetailerId.ToString());
        string retailerUrl = UrlController.GetRewriterUrl(PageName.RetailerInfo, param);
        var singleTms = RetailerController.NZTradeMeSellers_Static.SingleOrDefault(w => w.id == TradeMeSellerId);
        var isTradeMe=TradeMeSellerId > 0 ;

        var isExpirDate = singleTms != null && singleTms.RetailerId <= 0 && rps[0].ExpirationDate >= DateTime.Now;

        bool delInfo = string.IsNullOrEmpty(retailer.DeliveryInfo);

        if (!IsNoLink)
        {%>

<% if (isTradeMe)
   {
        %>
            <%if (isExpirDate) { %>
                    <div id="pricesRow<%=RetailerId %>" class="productlist clr <%=featuredProductCSS %>">
                <div id="pricesDivRow<%=RetailerId %>" class="bg1 pricesDiv <%=rowClass %>" style="padding: 1px 3px 1px 5px;">
                    <div class="pricesDivRetailer">
                        <div class="<%=retailerInfoCSS %>" style="margin-top:16px;">
                            <label class="pricesDivRetailerNameNoLink" isNull="<%=singleTms != null %>" retailerid="<%=singleTms.RetailerId <= 0 %>" exDate="<%=rps[0].ExpirationDate >= DateTime.Now %>" testID="3">
                                Trade Me</label>
                        </div>
                        <div class="pricesDivInfo" style="padding-left: 0; padding-top: 11px;">
                            <div class="cursor ProductLink" onclick="<%= PriceMe.WebConfig.Environment == "prod" ? "javascript:ga('send', 'event', 'retailerinfo', 'product', '"+RetailerProductID+"');":""%>">
                                <a href="<%=retailerUrl %>">
                                    <div class="flag">
                                        <img width="16" height="11" title="<%=string.Format(Resources.Resource.TextString_Infoabout,retailer.RetailerName) %>" alt="<%=countryInfo.KeyName %>" src="<%=Resources.Resource.ImageWebsite + countryInfo.CountryFlag %>" /></div>
                                </a>
                                <div data-toggle="modal" data-target="#retailerInfo" class="pricesDivInfoRating" onclick="ShowMoreDetail(<%=RetailerProductID %>,'p', 'r')">
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
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", ".");%>
                                    <div style="margin-top: 6px;" class="<%=Utility.GetStarImage(retailer.AvRating) %>"
                                        title="<%=avRating%>">
                                    </div>
                                    <div class="String"><a><%=retailer.ReviewString%></a></div>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pricesDivPdName" style="height: 50px;">
                        <div class="vertical">
                              <%if (!IsNoLink)
                              { %>
                             <span style="color:#666666;font-weight:normal"> <%=shortProductName%></span>
                            <%  }
                              else
                              { %>
                            <label style="font-weight: normal;"><%=shortProductName%></label>
                            <%} %>

                            <%if (condition != null)
                              { %>
                            <span class="pricesCondition" title="<%=condition.ConditionDescription %>"><%=condition.ConditionName %></span>
                            <%}

                              if (IsFeaturedProduct)
                              { %>

                            <span class="pricesFeatured" title="Featured store">Featured store</span>

                            <%} %>

                            
                            <div class="retailerMSG" style="height: 14px;font-style: normal;color:#666666;">
                                Seller: <%=singleTms.MemberName%>, Buy Now
                            </div>
                        </div>
                    </div>
                    <div class="pricesDivPdShowMore" style="padding-left: 2px;">
                        <%if (rps.Count > 1)
                          { %><div id="more<%=RetailerId %>Btn" class="bg1 show countSpan"<%if(rps.Count<11){ %> style="padding-left:10px;"<%}else if(rps.Count < 101){ %> style="padding-left:7px;" <%} %> title="<%=Resources.Resource.TextString_View %> <%=(rps.Count - 1)%> <%if ((rps.Count - 1) == 1)
                                                                                                                                                                            { %><%=Resources.Resource.TextString_Variation%><%}
                                                                                                                                                                            else
                                                                                                                                                                            { %><%=Resources.Resource.TextString_Variations%><%} %>" onclick="DisplayShowDiv('more<%=RetailerId %>Btn', 'more<%=RetailerId %>', 'pricesRow<%=RetailerId %>', 'pricesDivRow<%=RetailerId %>');"><%=rps.Count - 1%></div>
                        <%}
                          else
                          { %>&nbsp;<%} %>
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
                        <div class="PriceLarge">
                            <%--<div class="bubble"><%= GetLastModify(LastModify) %></div>--%>
                            <a class="PriceLarge rplistPrice" style="color:#999 !important;font-weight:normal" href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" rel="nofollow" target="_blank">
                                 <% var largePrice = FormatDecimal(RetailerPrice);
                                    if (largePrice.Contains("."))
                                    {
                                        var lastIndex = largePrice.LastIndexOf(".") + 1;
                                        var np1 = largePrice.Substring(0, lastIndex);
                                        var np2 = largePrice.Substring(lastIndex, largePrice.Length - lastIndex);
                                        if ((largePrice.Length - lastIndex) == 9)
                                            largePrice = np1.Replace("priceSpan","") + "<span style='position: relative;top: -0.2em;font-size: 0.8em; font-weight:normal;'>" + np2 + "</span>";
                                    }
                            %>
                                <%= largePrice%>

                            </a>
                        </div>
                    </div>
                    <div class="pricesDivShopping" style="margin: 19px 5px 0 0; position:relative;" title="<%=!delInfo?"Indicative shipping: "+retailer.DeliveryInfo:"" %>" >

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
                       
                        <%if (!delInfo) { %>
                             <div class="" style="background-image:url('https://images.pricemestatic.com/Images/PriceMeNewDesign/PriceMe_bgnew_161117.png');background-position: 70px -4121px;height:20px;" ></div>
                            <span class="glyphicon glyphicon-info-sign" aria-hidden="true" style="color:rgb(108,211,250);font-size:10px;position:absolute;top:-1px;right:3px;"></span>
                          <%}else{%>
                                <div class="bg1 pricesDivPriceFeesUn" title="<%=Resources.Resource.TextString_ShippingUnknown %>">
                                    <%=shipping %>
                                </div>
                            <%} %>

                        <%}%>



                        
                       
                    </div>
                    <div class="pricesDivVS" style="padding-top: 16px;">
                       <div class="btnVS exDateTred" style="width: auto;margin-left: 0px;color:#666666;font-size:10px;">Closes <%=GetProductDate(rps[0].ExpirationDate) %> </div>
                    </div>
                </div>
                <%if (rps.Count > 1)
                  { %>
                <div id="more<%=RetailerId %>" style="display: none;">
                    <%
                              int countMore = 0;
                              for (int i = 1; i < rps.Count; i++)
                              {
                                  RetailerProductID = rps[i].RetailerProductId;
                                  ProductName = rps[i].RetailerProductName;
                                  RetailerPrice = rps[i].RetailerPrice;
                                  TotalPrice = rps[i].TotalPrice;
                                  RetailerProductDescription = rps[i].RetailerProductDescription;
                                  FreightValue = rps[i].Freight ?? 0m;
                                  ccfeeValue = rps[i].CCFeeAmount ?? 0m;
                                  IsNoLink = rps[i].IsNoLink;
                                  Stock = rps[i].Stock;
                                  StockStatus = rps[i].StockStatus;
                                  stockImg = string.Empty;
                                  itempropStock = string.Empty;
                                  OriginalPrice = rps[i].OriginalPrice ?? 0;
                                  OriginalString = string.Empty;
                                  //PriceLocalCurrency = rps[i].PriceLocalCurrency ?? 0;
                                  if (OriginalPrice != null && OriginalPrice > 0 && OriginalPrice > RetailerPrice)
                                      OriginalString = (decimal.Round(((OriginalPrice - RetailerPrice) / OriginalPrice) * 100, 0)) + "%";
                                  condition = ProductController.GetRetailerProductCondition(rps[i].RetailerProductCondition ?? 0, WebConfig.CountryId);
                                  if (condition == null)
                                      shortProductName = ProductName.Length > 108 ? ProductName.Substring(0, 105) + "..." : ProductName;
                                  else
                                      shortProductName = ProductName.Length > (108 - condition.ConditionName.Length) ? ProductName.Substring(0, (105 - condition.ConditionName.Length)) + "..." : ProductName;

                                  BindStockStatus();

                                  isChristmasDate = false;
                                  if (WebConfig.CountryId == 3 && StockStatus != "0" && retailer.LastChristmasDevDate != null && retailer.LastChristmasDevDate.Year > 2012)
                                  {
                                      if (retailer.LastChristmasDevDate > DateTime.Now)
                                      {
                                          isChristmasDate = true;
                                          christmasString = Utility.GetChristmasString(retailer.LastChristmasDevDate);
                                      }
                                  }

                                  VSOnclickScript = string.Format(scriptFormat, Guid.NewGuid(), ProductID, RetailerId, RetailerProductID,
                                      ProductName.Replace("'", " ").Replace("\"", "").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "),
                                      CategoryId, RetailerPrice.ToString("0.00"), "&t=p" /*+ "&rpp=" + RetailerPrice.ToString("0.00") + "&pos=" + PricePosition + "&tpos=" + PricePositionCount + "&st=" + StockStatus + "&loc=" + Loc*/, WebConfig.CountryId, WebConfig.Environment == "prod");

                                  RetailerProductURL = Utility.GetRootUrl("/ResponseRedirect.aspx?aid=40&pid=" + ProductID + "&rid=" +
                                      RetailerId + "&rpid=" + RetailerProductID + "&countryID=" + WebConfig.CountryId + "&cid=" +
                                      CategoryId + "&t=p" + "&rpp=" + RetailerPrice.ToString("0.00") + "&pos=" + PricePosition + "&tpos=" + PricePositionCount + "&st=" + StockStatus + "&loc=" + Loc, WebConfig.CountryId);
                                  string uuid1 = Guid.NewGuid().ToString();
                                  RetailerProductURL += "&uuid=" + uuid1;
                                  
                                  //LastModify = rps[i].Modifiedon.HasValue ? rps[i].Modifiedon.Value : DateTime.MinValue;

                                  string rowClassMore = string.Empty;
                                  countMore = count + i;
                                  if ((countMore % 2) == 1)
                                      rowClassMore = " pricesDivRowBg";
                    %>

        


                    <div class="bg1 pricesDivMore<%=rowClassMore %>">
                        <div class="pricesDivPdName pricesDivNameMore">

                            <%if (!IsNoLink)
                              { %><a href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" class="trackName font-Bold" rel="nofollow" target="_blank"><%=shortProductName%></a>
                            <% }
                              else
                              { %>
                            <label style="font-weight: normal;"><%=shortProductName%></label><%} %>

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
                        <div class="ppricesDivStock pricesDivStockNoLink">
                            <div class="stock">&nbsp;</div>
                        </div>
                        <div class="pricesDivPriceNoLink">
                            <div class="PriceLarge">
                                <%--<div class="bubble"><%= GetLastModify(LastModify) %></div>--%>
                                <a class="PriceLarge rplistPrice" href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" rel="nofollow" target="_blank">
                                    <% var mlargePrice = FormatDecimal(RetailerPrice);
                                       if (mlargePrice.Contains("."))
                                    {
                                        var lastIndex = mlargePrice.LastIndexOf(".") + 1;
                                        var np1 = mlargePrice.Substring(0, lastIndex);
                                        var np2 = mlargePrice.Substring(lastIndex, mlargePrice.Length - lastIndex);
                                        if ((mlargePrice.Length - lastIndex) == 9)
                                            mlargePrice = np1.Replace("priceSpan", "") + "<span style='position: relative;top: -0.2em;font-size: 0.8em; font-weight:normal;'>" + np2 + "</span>";
                                    }
                            %>
                                <%= mlargePrice%>
                                </a>
                            </div>
                        </div>
                        <div class="pricesDivShopping pricesDivShoppingNoLink" style="position:relative;" title="<%=!delInfo?"Indicative shipping: "+retailer.DeliveryInfo:"" %>">

                         
                            <%
                              string shippingMore = GetProductFees();
                              if (shippingMore != "&nbsp;")
                              { %>
                           <div class="bg1 pricesDivPriceFees" title="<%=Resources.Resource.TextString_Shipping %>">
                                <%=shippingMore %>
                            </div>
                            <%}
                                else
                                {%>
                            <%if (!delInfo) { %>

                                <div class="" style="background-image:url('https://images.pricemestatic.com/Images/PriceMeNewDesign/PriceMe_bgnew_161117.png');background-position: 70px -4121px;height:20px;" ></div>
                            <span class="glyphicon glyphicon-info-sign" aria-hidden="true" style="color:rgb(108,211,250);font-size:10px;position:absolute;top:-1px;right:3px;"></span>

                              <%}else{%>
                                <div class="bg1 pricesDivPriceFeesUn" title="<%=Resources.Resource.TextString_ShippingUnknown %>">
                                <%=shippingMore%>
                            </div>
                              <%} %>
                             
                            <%}%>


                        </div>
                        <div class="pricesDivVSMore">
                            <div class="align-center visitShop">
                                <%if (condition == null)
                                  { %>
                                <a href="<%=RetailerProductURL %>" rel="nofollow" target="_blank" class="vs" onclick="<%=VSOnclickScript %>"><%=Resources.Resource.TextString_VisitShop %></a><%}
                                  else
                                  { %>
                                <a href="<%=RetailerProductURL %>" rel="nofollow" target="_blank" class="cond" onclick="<%=VSOnclickScript %>"><%=Resources.Resource.TextString_VisitShop %></a>
                                <%} %>
                            </div>
                        </div>
                    </div>
                    <%} %>
                </div>
                <%}%>
            </div>
              <%} %>

            
      <% 
   }
   else {%> 
        <div id="pricesRow<%=RetailerId %>" class="productlist clr <%=featuredProductCSS %>">
                <div id="pricesDivRow<%=RetailerId %>" class="bg1 pricesDiv <%=rowClass %>" style="padding: 1px 3px 1px 5px;">
                    <div class="pricesDivRetailer">
                        <div class="<%=retailerInfoCSS %>" style="margin-top: 6px;">
                            <%if (!retailer.LogoFile.Contains("."))
                              { %>
                            <a href="<%=RetailerProductURL %>" class="addLabel" onclick="<%=VSOnclickScript %>" rel="nofollow" target="_blank"><%=retailer.RetailerName%></a>
                            <%}
                              else
                              { %>
                            <div class="divImgPadding">
                                <a href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" rel="nofollow" target="_blank">
                                    <img class="plimg" src="https://images.pricemestatic.com/Images/MobileSite/pix1.png" data-pm-src2='<%=retailerLogo%>' width="120" height="40" alt="<%=retailer.RetailerName%>" /></a>
                            </div>
                            <%}%>
                        </div>
                        <div class="pricesDivInfo" style="padding-left: 0; padding-top: 11px;">
                            <div class="cursor ProductLink" onclick="<%= PriceMe.WebConfig.Environment == "prod" ? "javascript:ga('send', 'event', 'retailerinfo', 'product', '"+RetailerProductID+"');":""%>">
                                <a href="<%=retailerUrl %>">
                                    <div class="flag">
                                        <img width="16" height="11" title="<%=string.Format(Resources.Resource.TextString_Infoabout,retailer.RetailerName) %>" alt="<%=countryInfo.KeyName %>" src="<%=Resources.Resource.ImageWebsite + countryInfo.CountryFlag %>" /></div>
                                </a>
                                <div data-toggle="modal" data-target="#retailerInfo" class="pricesDivInfoRating" onclick="ShowMoreDetail(<%=RetailerProductID %>,'p', 'r')">
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
                                          string avRating1 = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", ".");%>
                                    <div class="smallstar-rating reviewDiv" title="<%=avRating1%>">
                                        <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                    </div>
                                    <div class="String"><a><%=retailer.ReviewString%></a></div>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pricesDivPdName" style="height: 50px;">
                        <div class="vertical">

                            <%if (!IsNoLink)
                              { %> <a href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" class="trackName font-Bold" rel="nofollow" target="_blank"><%=shortProductName%></a>
                            <%  }
                              else
                              { %>
                            <label style="font-weight: normal;"><%=shortProductName%></label>
                            <%} %>

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
                            <div class="retailerMSG" style="height: 14px;">
                                <%=retailer.RetailerMessage%>
                            </div>
                            <%}%>
                        </div>
                    </div>
                    <div class="pricesDivPdShowMore" style="padding-left: 2px;">
                        <%if (rps.Count > 1)
                          { %><div id="more<%=RetailerId %>Btn" class="bg1 show countSpan"<%if(rps.Count<11){ %> style="padding-left:10px;"<%}else if(rps.Count < 101){ %> style="padding-left:7px;" <%} %> title="<%=Resources.Resource.TextString_View %> <%=(rps.Count - 1)%> <%if ((rps.Count - 1) == 1)
                                                                                                                                                                            { %><%=Resources.Resource.TextString_Variation%><%}
                                                                                                                                                                            else
                                                                                                                                                                            { %><%=Resources.Resource.TextString_Variations%><%} %>" onclick="DisplayShowDiv('more<%=RetailerId %>Btn', 'more<%=RetailerId %>', 'pricesRow<%=RetailerId %>', 'pricesDivRow<%=RetailerId %>');"><%=rps.Count - 1%></div>
                        <%}
                          else
                          { %>&nbsp;<%} %>
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
                        <div class="PriceLarge">
                            <%--<div class="bubble"><%= GetLastModify(LastModify) %></div>--%>
                            <a class="PriceLarge rplistPrice" href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" rel="nofollow" target="_blank">
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
                                <%= largePrice%>

                            </a>
                        </div>
                    </div>
                    <div class="pricesDivShopping" style="margin: 19px 5px 0 0;position:relative;" title="<%=!delInfo?"Indicative shipping: "+retailer.DeliveryInfo:"" %>">
                        

                          <%
                            string shipping = GetProductFees();
                          if (shipping != "&nbsp;")
                          { %><div class="bg1 pricesDivPriceFees" title="<%=Resources.Resource.TextString_Shipping %>">
                            <%=shipping %>
                        </div>
                        
                        <%}
                            else
                            {%>
                        
                        <%
                            if (!delInfo) {%> 
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
                <%if (rps.Count > 1)
                  { %>
                <div id="more<%=RetailerId %>" style="display: none;">
                    <%
                      bool isout = false;
                      if (ppc != null && ppc.PPCforInStockOnly)
                          isout = true;
                          
                              int countMore = 0;
                              for (int i = 1; i < rps.Count; i++)
                              {
                                  RetailerProductID = rps[i].RetailerProductId;
                                  ProductName = rps[i].RetailerProductName;
                                  RetailerPrice = rps[i].RetailerPrice;
                                  TotalPrice = rps[i].TotalPrice;
                                  RetailerProductDescription = rps[i].RetailerProductDescription;
                                  FreightValue = rps[i].Freight ?? 0m;
                                  ccfeeValue = rps[i].CCFeeAmount ?? 0m;
                                  IsNoLink = rps[i].IsNoLink;
                                  Stock = rps[i].Stock;
                                  StockStatus = rps[i].StockStatus;
                                  stockImg = string.Empty;
                                  itempropStock = string.Empty;
                                  OriginalPrice = rps[i].OriginalPrice ?? 0;
                                  OriginalString = string.Empty;

                                  if (OriginalPrice > 0 && OriginalPrice > RetailerPrice)
                                      OriginalString = (decimal.Round(((OriginalPrice - RetailerPrice) / OriginalPrice) * 100, 0)) + "%";
                                  condition = ProductController.GetRetailerProductCondition(rps[i].RetailerProductCondition ?? 0, WebConfig.CountryId);
                                  if (condition == null)
                                      shortProductName = ProductName.Length > 108 ? ProductName.Substring(0, 105) + "..." : ProductName;
                                  else
                                      shortProductName = ProductName.Length > (108 - condition.ConditionName.Length) ? ProductName.Substring(0, (105 - condition.ConditionName.Length)) + "..." : ProductName;

                                  BindStockStatus();

                                  isChristmasDate = false;
                                  if (WebConfig.CountryId == 3 && StockStatus != "0" && retailer.LastChristmasDevDate != null && retailer.LastChristmasDevDate.Year > 2012)
                                  {
                                      if (retailer.LastChristmasDevDate > DateTime.Now)
                                      {
                                          isChristmasDate = true;
                                          christmasString = Utility.GetChristmasString(retailer.LastChristmasDevDate);
                                      }
                                  }

                                  VSOnclickScript = string.Format(scriptFormat, Guid.NewGuid(), ProductID, RetailerId, RetailerProductID,
                                      ProductName.Replace("'", " ").Replace("\"", "").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "),
                                      CategoryId, RetailerPrice.ToString("0.00"), "&t=p" /*+ "&rpp=" + RetailerPrice.ToString("0.00") + "&pos=" + PricePosition + "&tpos=" + PricePositionCount + "&st=" + StockStatus + "&loc=" + Loc*/, WebConfig.CountryId, WebConfig.Environment == "prod");

                                  RetailerProductURL = Utility.GetRootUrl("/ResponseRedirect.aspx?aid=40&pid=" + ProductID + "&rid=" +
                                      RetailerId + "&rpid=" + RetailerProductID + "&countryID=" + WebConfig.CountryId + "&cid=" +
                                      CategoryId + "&t=p" + "&rpp=" + RetailerPrice.ToString("0.00") + "&pos=" + PricePosition + "&tpos=" + PricePositionCount + "&st=" + StockStatus + "&loc=" + Loc, WebConfig.CountryId);
                                  string uuid2 = Guid.NewGuid().ToString();
                                  RetailerProductURL += "&uuid=" + uuid2;
                                  
                                  bool isoutnolink = false;
                                  if (isout && StockStatus == "0")
                                      isoutnolink = true;
                                  //LastModify = rps[i].Modifiedon.HasValue ? rps[i].Modifiedon.Value : DateTime.MinValue;

                                  string rowClassMore = string.Empty;
                                  countMore = count + i;
                                  if ((countMore % 2) == 1)
                                      rowClassMore = " pricesDivRowBg";
                    %>

        


                    <div class="bg1 pricesDivMore<%=rowClassMore %>">
                        <div class="pricesDivPdName pricesDivNameMore">

                            <%if (!isoutnolink)
                              { %><a href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" class="trackName font-Bold" rel="nofollow" target="_blank"><%=shortProductName%></a>
                            <% }
                              else
                              { %>
                            <label class="trackNameout"><%=shortProductName%></label><%} %>

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
                        <div class="ppricesDivStock pricesDivStockNoLink">
                            <div class="stock">&nbsp;</div>
                        </div>
                        <div class="pricesDivPriceNoLink">
                            <%if(!isoutnolink){ %>
                            <div class="PriceLarge">
                                <%--<div class="bubble"><%= GetLastModify(LastModify) %></div>--%>
                                <a class="PriceLarge rplistPrice" href="<%=RetailerProductURL %>" onclick="<%=VSOnclickScript %>" rel="nofollow" target="_blank">
                                     <% var mlargePrice = FormatDecimal(RetailerPrice);
                                       if (mlargePrice.Contains("."))
                                    {
                                        var lastIndex = mlargePrice.LastIndexOf(".") + 1;
                                        var np1 = mlargePrice.Substring(0, lastIndex);
                                        var np2 = mlargePrice.Substring(lastIndex, mlargePrice.Length - lastIndex);
                                        if ((mlargePrice.Length - lastIndex) == 9)
                                            mlargePrice = np1.Replace("priceSpan", "") + "<span style='position: relative;top: -0.2em;font-size: 0.8em; font-weight:normal;'>" + np2 + "</span>";
                                    }
                            %>
                                    <%= mlargePrice%>
                                </a>
                            </div><%}else{ %>
                            <div class="PriceLarge nolinkPrice">
                                <span>
                                     <% var mlargePrice = FormatDecimal(RetailerPrice);
                                       if (mlargePrice.Contains("."))
                                    {
                                        var lastIndex = mlargePrice.LastIndexOf(".") + 1;
                                        var np1 = mlargePrice.Substring(0, lastIndex);
                                        var np2 = mlargePrice.Substring(lastIndex, mlargePrice.Length - lastIndex);
                                        if ((mlargePrice.Length - lastIndex) == 9)
                                            mlargePrice = np1 + "<span style='position: relative;top: -0.2em;font-size: 0.8em; font-weight:normal;'>" + np2 + "</span>";
                                    }
                            %>
                                    <%= mlargePrice%>
                                </span>
                            </div>
                            <%} %>
                        </div>
                        <div class="pricesDivShopping pricesDivShoppingNoLink" style="position:relative;" title="<%=!delInfo?"Indicative shipping: "+retailer.DeliveryInfo:"" %>">
                            <%
                              string shippingMore = GetProductFees();
                              if (shippingMore != "&nbsp;")
                              { %>
                           <div class="bg1 pricesDivPriceFees" title="<%=Resources.Resource.TextString_Shipping %>">
                                <%=shippingMore %>
                            </div>
                            <%}
                                else
                                {%>
                            <%if (!delInfo) { %>

                                 <div class="" style="background-image:url('https://images.pricemestatic.com/Images/PriceMeNewDesign/PriceMe_bgnew_161117.png');background-position: 70px -4121px;height:20px;" ></div>
                            <span class="glyphicon glyphicon-info-sign" aria-hidden="true" style="color:rgb(108,211,250);font-size:10px;position:absolute;top:-1px;right:3px;"></span>

                              <%}else{%>
                                <div class="bg1 pricesDivPriceFeesUn" title="<%=Resources.Resource.TextString_ShippingUnknown %>">
                                <%=shippingMore%>
                            </div>
                              <%} %>
                             
                            <%}%>

                        </div>
                        <div class="pricesDivVSMore">
                            <div class="align-center visitShop">
                                <%if (!isoutnolink)
                                  { %>
                                <%if (condition == null)
                                  { %>
                                <a href="<%=RetailerProductURL%>" rel="nofollow" target="_blank" class="vs" onclick="<%=VSOnclickScript%>"><%=Resources.Resource.TextString_VisitShop%></a><%}
                                  else
                                  { %>
                                <a href="<%=RetailerProductURL%>" rel="nofollow" target="_blank" class="cond" onclick="<%=VSOnclickScript%>"><%=Resources.Resource.TextString_VisitShop%></a>
                                <%}
                                  } %>
                            </div>
                        </div>
                    </div>
                    <%} %>
                </div>
                <%}%>
            </div>
   <%} %>
            
<%}
                else
                { %>
            <%if (isTradeMe)
              { %>
                        <%if (isExpirDate) { %>
                                <div class="bg1 pricesDivNoLink<%=rowClass %>" id="pricesRow<%=RetailerId %>">
                    <div class="pricesDivRetailer">
                        <div class="<%=retailerInfoCSS %>" style="margin-top:10px; ">
                            <label class="pricesDivRetailerNameNoLink" isNull="<%=singleTms != null %>" retailerid="<%=singleTms.RetailerId <= 0 %>" exDate="<%=rps[0].ExpirationDate >= DateTime.Now %>" testID="4">
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
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", "."); %>
                                    <div class="npStarsDiv">
                                        <div class="smallstar-rating reviewDiv" title="<%=avRating%>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                    </div>
                                    <%}
                                      else
                                      {
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", "."); %>
                                    <div class="npStarsDiv">
                                        <div class="smallstar-rating reviewDiv" title="<%=avRating%>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                    </div>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pricesDivPdNameNoLink">
                        <label style="font-weight: normal;color:#666666;">
                            <%=shortProductName %>
                        </label>
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
                    <div class="pricesDivPriceNoLink" style="height:36px; line-height:36px !important;vertical-align:middle;">
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

                            <span style="color:#666666 !important;font-weight:normal;line-height:36px;"><%=nolinkPrice%></span>
                        </div>
                    </div>
                    <div class="pricesDivShopping pricesDivShoppingNoLink" style="padding-top:4px; position:relative;" title="<%=!delInfo?"Indicative shipping: "+retailer.DeliveryInfo:"" %>">
                        


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
                    <div class="pricesDivVS" style="padding-top:10px;"><div class="btnVS exDateTred" style="width: auto;margin-left: 0px;color:#666666;font-size:10px;">Closes <%=GetProductDate(rps[0].ExpirationDate) %> </div></div>
                </div>
                          <%} %>
              <%}else{%>
                    <div class="bg1 pricesDivNoLink<%=rowClass %>" id="pricesRow<%=RetailerId %>">
                    <div class="pricesDivRetailer">
                        <div class="<%=retailerInfoCSS %>">
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
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", "."); %>
                                    <div class="npStarsDiv">
                                        <div class="smallstar-rating reviewDiv" title="<%=avRating%>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                    </div>
                                    <%}
                                      else
                                      {
                                          string avRating = retailer.AvRating == 0 ? Resources.Resource.TextString_NoRating : string.Format(Resources.Resource.TextString_OutOfRating, retailer.AvRating.ToString("0.0")).Replace(",", "."); %>
                                    <div class="npStarsDiv">
                                        <div class="smallstar-rating reviewDiv" title="<%=avRating%>">
                                            <span style="width:<%=PriceMe.Utility.GetStarRatingNew((double)retailer.AvRating)%>%;"></span>
                                        </div>
                                    </div>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="pricesDivPdNameNoLink">
                        <label style="font-weight: normal;">
                            <%=shortProductName %>
                        </label>
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

                            <span><%=nolinkPrice%></span>
                        </div>
                    </div>
                    <div class="pricesDivShopping pricesDivShoppingNoLink" style="position:relative;" title="<%=!delInfo?"Indicative shipping: "+retailer.DeliveryInfo:"" %>"  >
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
                          <div class="" style="background-image:url('https://images.pricemestatic.com/Images/PriceMeNewDesign/PriceMe_bgnew_161117.png');background-position: 70px -4121px;height:20px;"></div>
                            <span class="glyphicon glyphicon-info-sign" aria-hidden="true" style="color:rgb(108,211,250);font-size:10px;position:absolute;top:-1px;right:3px;"></span>

                          <%}else{%>
                                <div class="bg1 pricesDivPriceFeesUn" title="<%=Resources.Resource.TextString_ShippingUnknown %>">
                            <%=shipping %>
                        </div>
                            <%} %>

                       
                        <%}%>  

                        
                    </div>
                    <div class="pricesDivVSNoLink">&nbsp;</div>
                </div>
              <%} %>
                
        <%}
    } %>

