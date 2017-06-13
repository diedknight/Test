<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductImagesDisplay.ascx.cs" Inherits="HotterWinds.Modules.Products.ProductImagesDisplay" %>

<div class="images product-image">
    <div class="product-full">
        <a href="<%=ImageUrl %>" itemprop="image" class="woocommerce-main-image zoom cloud-zoom">
            <img src="<%=ImageUrl %>" id="product-zoom" data-zoom-image="<%=ImageUrl %>" onerror="onImgError3(this)" />
        </a>
    </div>

    <%if (listImages.Count != 0) %>
    <%{ %>
    <div class="more-views">
        <div class="slider-items-products">
            <div id="gallery_01" class="product-flexslider hidden-buttons product-img-thumb">
                <div class="slider-items slider-width-col4 block-content">

                    <%foreach (var imgItem in listImages) %>
                    <%{ %>
                    <div class="more-views-items">
                        <a href="imgItem" class="cloud-zoom-gallery" rel="useZoom: 'zoom1', smallImage: '<%=imgItem %>'" data-image="<%=imgItem %>">
                            <img width="73" height="88" src="<%=imgItem %>" class="attachment-shop_thumbnail size-shop_thumbnail wp-post-image" onerror="onImgError(this)" />
                        </a>
                    </div>
                    <%} %>
                </div>
            </div>
        </div>
    </div>
    <%} %>
</div>