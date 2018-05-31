﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HWGridView.ascx.cs" Inherits="HotterWinds.Modules.Catalog.HWGridView" %>

<div class="item-inner">
    <div class="item-img">
        <div class="item-img-info">
            <div class="pimg">
                <a href="<%=linkUrl%>" class="product-image">
                    <img width="300" height="362" class="attachment-shop_catalog size-shop_catalog wp-post-image" id="productImage<%=ProductID %>" src="<%=DefaultImage%>" data-pm-src2="<%=DefaultImage%>" alt="<%=ImageAlt%>" title="<%=ImageAlt %>" onerror="onImgError(this)" />
                </a>
            </div>
        </div>
    </div>

    <div class="item-info">
        <div class="info-inner">

            <div class="item-title">
                <a href="<%=linkUrl%>"><%=DisplayName%></a>
            </div>

            <div class="desc std">
                <p><%=ShortDescriptionZH %></p>
            </div> 

            <div class="item-content">

                <div class="rating">
                    <div class="ratings">
                        <div class="rating-box">
                            <div style="width: <%=Score*2*10 %>%" class="rating"></div>                                   
                        </div>
                    </div>
                </div>
                
                <div class="item-price">
                    <div class="price-box">
                        <span class="amount"><%=PriceMe.Utility.FormatPrice(BestPrice)%></span>
                    </div>
                </div>

                <div class="action">
                    <a href="<%=ClickOutUrl %>" target="_blank" class="button add_to_cart_button btn-cart" onclick="dataLayer.push({'transactionId': '<%=Guid.NewGuid() %>','transactionProducts': [{ 'name': '<%=ProductName %>', 'sku': '<%=BestPPCRetailerID %>','category': <%=CategoryID %>, 'price': <%=BestPrice %>, 'quantity': 1, 'dimension1' : '<%=BestPPCRetailerID %>'}],'event': 'pmco_trans'});">
                        <span>Visit Shop</span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</div>
