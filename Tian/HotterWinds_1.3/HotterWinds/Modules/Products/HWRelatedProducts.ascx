<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HWRelatedProducts.ascx.cs" Inherits="HotterWinds.Modules.Products.HWRelatedProducts" %>

<style type="text/css">
    .related-pro .action .visit_shop {
        display: inline-block;
        text-align: center;
        width: 120px;
        height: 32px;
        line-height: 32px;
        font-weight: bold;
        background-color: #1fc0a0;
        color: white;
        font-size: 15px;
        border-radius: 5px;        
    }

        .related-pro .action .visit_shop:hover {
            text-decoration: none;
        }

    .related-pro .item {
        float:left;
        width: 194px;
        overflow:hidden;
    }

    .related-pro .retailer_logo {
        height:40px;
        width:120px;
        margin:0 auto;
    }

    .products-grid .item .item-inner .item-info .info-inner .item-title {
        white-space:normal;
        height:52px;
    }

    @media (max-width: 1349px) {
        .related-pro .item {
            width: 194px;
            height: 348px;
        }
    }

    @media (max-width: 1180px) {
        .related-pro .item {
            width: 161px;
            height: 308px;
        }
    }

    @media (max-width: 1025px) {
        .related-pro .item {
            width: 241px;
            height: 405px;
        }
    }

    @media (max-width: 975px) {
        .related-pro .item {
            width: 186px;
            height: 339px;
        }
    }

    @media (max-width: 900px) {
        .related-pro .item {
            width: 248px;
            height: 414px;
        }
    }

    @media (max-width: 750px) {
        .related-pro .item {
            width: 183px;
            height: 358px;
        }
    }

    @media (max-width: 600px) {
        .related-pro .item {
            width: 183px;
            height: 358px;
        }
    }

    @media (max-width: 415px) {
        .related-pro .item {
            width: 150px;
            height: 358px;
        }
    }

    @media (max-width: 360px) {
        .related-pro .item {
            width: 310px;
            height: 345px;
        }
    }


</style>

<!-- Related Slider -->
<div class="related-pro">
    <div class="slider-items-products">
        <div class="related-block">
            <div id="related-products-slider" class="product-flexslider hidden-buttons">
                <div class="products-grid">

                    <%foreach (var p in filtedRelatedProducts) %>
                    <%{ %>
                    <div class="item">
                        <div class="item-inner">
                            <div class="item-img">
                                <div class="item-img-info">
                                    <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductID,p.ProductName) %>" title="<%=p.ProductName %>" class="product-image">
                                        <figure class="img-responsive">
                                            <img alt="<%=p.ProductName %>" src="<%=GetImage(p.DefaultImage) %>" onerror="onImgError2(this)">
                                        </figure>
                                    </a>
                                </div>
                            </div>

                            <div class="item-info">
                                <div class="info-inner">

                                    <div class="item-title">
                                        <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductID,p.ProductName) %>" title="<%=p.ProductName %>"><%=p.ProductName %></a>
                                    </div>

                                    <div class="item-content">                                                                               

                                        <div class="item-price">
                                            <div class="price-box">
                                                <ins><span class="amount">$<%=p.BestPrice %></span></ins>
                                            </div>
                                        </div>

                                        <p class="retailer_logo"><img width="120" height="40" src="<%=PriceMe.Utility.GetImage(p.BestPPCLogoPath, "_ms") %>" /></p>

                                        <div class="action">
                                            <a href="<%=GetLinkUrl(Convert.ToInt32(p.ProductID),Convert.ToInt32(p.BestPPCRetailerID),Convert.ToInt32(p.BestPPCRetailerProductID),p.CategoryID) %>" 
                                                class="visit_shop"
                                                onclick="dataLayer.push({'transactionId': '<%=Guid.NewGuid() %>','transactionProducts': [{ 'name': '<%=p.ProductName %>', 'sku': '<%=p.BestPPCRetailerID %>','category': <%=p.CategoryID %>, 'price': <%=p.BestPrice %>, 'quantity': 1, 'dimension1' : '<%=p.BestPPCRetailerID %>'}],'event': 'pmco_trans'});">
                                                Visit Shop >
                                            </a>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%} %>
                </div>
            </div>

        </div>
    </div>
</div>

<!-- End related products Slider -->
