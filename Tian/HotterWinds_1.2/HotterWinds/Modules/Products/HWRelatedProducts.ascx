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
        width:176px;
        overflow:hidden;
    }

    .related-pro .retailer_logo {
        height:40px;
        width:120px;
        margin:0 auto;
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
                                                <ins><span class="amount">$ <%=p.BestPrice %></span></ins>
                                            </div>
                                        </div>

                                        <p class="retailer_logo"><img width="120" height="40" src="<%=PriceMe.Utility.GetImage(p.BestPPCLogoPath, "_ms") %>" /></p>

                                        <div class="action">
                                            <a href="<%=GetLinkUrl(Convert.ToInt32(p.ProductID),Convert.ToInt32(p.BestPPCRetailerID),Convert.ToInt32(p.BestPPCRetailerProductID),p.CategoryID) %>" class="visit_shop">Visit Shop ></a>
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
