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
        margin-left: 15px;
    }

        .related-pro .action .visit_shop:hover {
            text-decoration: none;
        }
</style>

<!-- Related Slider -->
<div class="related-pro">
    <div class="slider-items-products">
        <div class="related-block">
            <%--            <div class="home-block-inner">
                <div class="block-title">
                    <h2>Related Products</h2>
                </div>
            </div>--%>
             
            <div id="related-products-slider" class="product-flexslider hidden-buttons">
                <div class="slider-items slider-width-col4 products-grid block-content">

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

                                        <div class="rating">
                                            <div class="ratings">
                                                <div class="rating-box">
                                                    <div style="width: <%=p.ProductRatingSum %>0%" class="rating"></div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="item-price">
                                            <div class="price-box">
                                                <ins><span class="amount"><%=Resources.Resource.PriceCurrency %><%=p.BestPrice %></span></ins>
                                            </div>
                                        </div>

                                        <div class="action">
                                            <a href="<%=GetLinkUrl(Convert.ToInt32(p.ProductID),Convert.ToInt32(p.BestPPCRetailerID),Convert.ToInt32(p.BestPPCRetailerProductID),p.CategoryID) %>" class="visit_shop">Visit Shop >
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
