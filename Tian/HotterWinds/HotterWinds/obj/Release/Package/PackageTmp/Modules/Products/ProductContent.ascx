<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductContent.ascx.cs" Inherits="HotterWinds.Modules.Products.ProductContent" %>
<%@ Register Src="~/Modules/Products/ProductImagesDisplay.ascx" TagPrefix="uc1" TagName="ProductImagesDisplay" %>
<%@ Register Src="~/Modules/Products/ProductTopDisplay.ascx" TagPrefix="uc1" TagName="ProductTopDisplay" %>
<%@ Register Src="~/Modules/Products/MDProductPriceCompareNomal.ascx" TagPrefix="uc37" TagName="ProductPriceCompareNomal" %>
<%@ Register Src="~/Modules/Products/ProductItemPriceCompareNomal.ascx" TagPrefix="uc37" TagName="ProductItemPriceCompareNomal" %>
<%@ Register Src="~/Modules/Products/ProductDescription.ascx" TagName="ProductDescription" TagPrefix="PD" %>



<style type="text/css">
    .productlist .btnVS .btnProduct {
        background-color: #1FC0A0;
        color: #FFFFFb;
    }

    #product-detail-tab {
        margin-bottom: 10px;
    }
</style>
<div class="product-view">

    <div itemscope itemtype="http://schema.org/Product" id="product-799" class="post-799 product type-product status-publish has-post-thumbnail product_cat-anarkalis product_cat-baby-kids product_cat-chiffon-dress product_cat-clothing product_cat-cotton-dress product_cat-cotton-anarkalis product_cat-dress-material product_cat-dresses product_cat-embroidered product_cat-ethnic-wear product_cat-floral product_cat-game-sport product_cat-kurtas product_cat-net-anarkalis product_cat-partywear product_cat-printed product_cat-salwar-suit-sets product_cat-sarees product_cat-shirts product_cat-silk-dress product_cat-silk-anarkalis product_cat-sportswear product_cat-stoles-dupattas product_cat-stripes product_cat-swimwear product_cat-synthetic-dress product_cat-synthetic-anarkalis product_cat-t-shirts-top product_cat-tees-polo product_cat-tops product_cat-tops-tunics product_cat-western product_cat-winter-wear notblog featured shipping-taxable product-type-external product-cat-anarkalis product-cat-baby-kids product-cat-chiffon-dress product-cat-clothing product-cat-cotton-dress product-cat-cotton-anarkalis product-cat-dress-material product-cat-dresses product-cat-embroidered product-cat-ethnic-wear product-cat-floral product-cat-game-sport product-cat-kurtas product-cat-net-anarkalis product-cat-partywear product-cat-printed product-cat-salwar-suit-sets product-cat-sarees product-cat-shirts product-cat-silk-dress product-cat-silk-anarkalis product-cat-sportswear product-cat-stoles-dupattas product-cat-stripes product-cat-swimwear product-cat-synthetic-dress product-cat-synthetic-anarkalis product-cat-t-shirts-top product-cat-tees-polo product-cat-tops product-cat-tops-tunics product-cat-western product-cat-winter-wear instock">
        <div class="product-essential">

            <div class="product-img-box col-lg-4 col-sm-5 col-xs-12">
                <uc1:ProductImagesDisplay runat="server" ID="ImagesDisplay1" />
            </div>

            <uc1:ProductTopDisplay runat="server" ID="ProductTopDisplay1" />

        </div>
        <!--product-essential-->


        <div class="product-collateral">

            <div class="add_info">
                <div class="woocommerce-tabs">
                    <div class="tabs">
                        <ul class="tabs nav nav-tabs product-tabs" id="product-detail-tab">
                            <li class="compare_tab">
                                <a href="#tab-compare">Compare Prices</a>
                            </li>
                            <li class="description_tab">
                                <a href="#tab-description">Description</a>
                            </li>
                            <li class="additional_tab">
                                <a href="#tab-additional">Additional Information</a>
                            </li>
                            <li class="reviews_tab">
                                <a href="#tab-reviews">Reviews (0)</a>
                            </li>
                        </ul>

                    </div>

                    <div id="productTabContent" class="tab-content">
                        <div class="panel entry-content" id="tab-compare">
                            <div class="productCenter">
                                <div class="pricesTD clearboth">
                                    <section id="pnlHistory" style="padding-bottom: 12px; overflow: hidden;">
                                        <div id="pnlPrices" runat="server">
                                            <%if (hasRetailerProducts)%>
                                            <%{%>
                                            <div>
                                                <%if (!isAllPrice)%>
                                                <%{ %>
                                                <uc37:ProductPriceCompareNomal ID="ProductPriceCompareNomal1" runat="server" />
                                                <%}%>
                                                <% else%>
                                                <%{ %>
                                                <uc37:ProductItemPriceCompareNomal ID="ProductItemPriceCompareNomal1" runat="server" />
                                                <%} %>
                                            </div>
                                            <%if (rpisInt.Count > 0 || rpsInt.Count > 0)
                                                { %>
                                            <div class="clr"></div>
                                            <div class="relatedControlTitle">
                                                <div class="ipTitle">
                                                    <span class="glyphicon glyphicon-globe"></span><span class="ipText">International prices</span>
                                                    <%if (PriceMe.WebConfig.CountryId == 3)
                                                        { %>
                                                    <div class="helpTopicDiv bg1" data-placement="top" data-original-title="International retailers not registered for GST in New Zealand. Their prices are exempt from GST below $400." data-toggle="tooltip" style="vertical-align: middle;">
                                                    </div>
                                                    <%} %>
                                                </div>

                                                <%if (!isAllPrice)
                                                    { %>
                                                <uc37:ProductPriceCompareNomal ID="ProductPriceCompareNomalInt" runat="server" />
                                                <%}
                                                    else
                                                    { %>
                                                <uc37:ProductItemPriceCompareNomal ID="ProductItemPriceCompareNomalInt" runat="server" />
                                                <%} %>
                                            </div>
                                            <%}
                                                }
                                                else
                                                {%>
                                            <div id="statsbar3">
                                                <div class="alert alert-info" role="alert">
                                                    &nbsp;
                            <%if (string.IsNullOrEmpty(upcoming))
                                { %><%=Resources.Resource.Product_NoProduct.Replace("{0}", "#relatedProduct")%><%}
                                                                                                                   else
                                                                                                                   { %>
                                                    <%=upcoming %>
                                                    <div id="upcomingmess" style="margin: 10px 0 0 5px;">
                                                        <%if (isLogin)
                                                            {
                                                                if (isUpcoming)
                                                                {%>
                                You will be notified when the product becomes available.
                                <%}
                                    else
                                    { %>
                                                        <div class="btn btn-default btn-xs proVisitShop" style="max-width: 220px;" onclick="ShowUpcoming(<%=product.ProductID%>, 1)">
                                                            Notify me when product available
                                                        </div>
                                                        <%}
                                                            }
                                                            else
                                                            { %>
                                                        <div class="btn btn-default btn-xs proVisitShop" style="max-width: 220px;" data-toggle="modal" data-target="#upcoming" onclick="ShowUpcoming(<%=product.ProductID%>, 0)">
                                                            Notify me when product available
                                                        </div>
                                                        <%} %>
                                                    </div>
                                                    <%} %>
                                                </div>
                                            </div>
                                            <%}%>
                                        </div>
                                    </section>
                                    <script type="text/javascript">
                                        $(function () {
                                            if ($("#relatedProduct").length <= 0) {
                                                $("#statsbar3 a").click(function () {
                                                    var cate_url = $("#new-breadcrumb li:last").prev().children().attr("href")
                                                    location.href = "https://" + location.host + cate_url;
                                                    return false;
                                                })
                                            }
                                            setTableWidthsP();
                                        })

                                        $(window).resize(function () {
                                            setTableWidthsP();
                                        });

                                    </script>
                                    <div class="relatedControl clearboth googleads" style="margin: 10px 0 12px 2px;">
                                        <%--<googlehorizontalads:googlehorizontalads id="AdsHorisontalMiddle" runat="server" />--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel entry-content" id="tab-description">
                            <div><%= ProductDesc %></div>
                        </div>

                        <div class="panel entry-content" id="tab-additional">
                            <div class="productCenter">
                                <div class="pricesTD clearboth">
                                    <section>
                                        <PD:ProductDescription ID="ProductDescription1" runat="server" />
                                    </section>
                                </div>
                            </div>
                        </div>

                        <div class="panel entry-content" id="tab-reviews">
                            <div id="reviews">
                                <div id="comments">
                                    <h2>Reviews</h2>


                                    <p class="woocommerce-noreviews">There are no reviews yet.</p>

                                </div>


                                <div id="review_form_wrapper">
                                    <div id="review_form">
                                        <div id="respond" class="comment-respond">
                                            <h3 id="reply-title" class="comment-reply-title">Be the first to review &ldquo;Today Fashion Casual Sleeveless Solid Women&#8217;s Top Light Pink&rdquo; <small><a rel="nofollow" id="cancel-comment-reply-link" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/#respond" style="display: none;">Cancel reply</a></small></h3>
                                            <p class="must-log-in">You must be <a href="http://wordpress.magikthemes.com/linea/my-account/">logged in</a> to post a review.</p>
                                        </div>
                                        <!-- #respond -->
                                    </div>
                                </div>


                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <!--product-collateral-->


        <!-- Related Slider -->
        <div class="related-pro">
            <div class="slider-items-products">
                <div class="related-block">
                    <div class="home-block-inner">
                        <div class="block-title">
                            <h2>Related Products</h2>
                        </div>
                    </div>

                    <div id="related-products-slider" class="product-flexslider hidden-buttons">
                        <div class="slider-items slider-width-col4 products-grid block-content">

                            <div class="item">
                                <div class="item-inner">
                                    <div class="item-img">
                                        <div class="item-img-info">
                                            <a href="http://wordpress.magikthemes.com/linea/product/united-colors-of-benetton-womens-top-lime/" title="United Colors of Benetton Women&#039;s Top lime" class="product-image">
                                                <figure class="img-responsive">
                                                    <img alt="United Colors of Benetton Women&#039;s Top lime" src="http://wordpress.magikthemes.com/linea/wp-content/uploads/sites/2/2016/01/product22-4-300x362.jpg">
                                                </figure>
                                            </a>
                                            <div class="sale-label sale-top-right">
                                                Sale
                                            </div>
                                            <div class="box-hover">
                                                <ul class="add-to-links">
                                                    <li>


                                                        <a class="detail-bnt yith-wcqv-button link-quickview"
                                                            data-product_id="103"></a>


                                                        <li>
                                                            <a title="Add to Wishlist" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add_to_wishlist=103" data-product-id="103" data-product-type="variable" class="link-wishlist"></a>

                                                        </li>
                                                    <li>


                                                        <a title="Add to Compare" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?action=yith-woocompare-add-product&#038;id=103&#038;_wpnonce=cd4491022c" class="link-compare add_to_compare compare " data-product_id="103"></a>


                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="item-info">
                                        <div class="info-inner">

                                            <div class="item-title">
                                                <a href="http://wordpress.magikthemes.com/linea/product/united-colors-of-benetton-womens-top-lime/"
                                                    title="United Colors of Benetton Women&#039;s Top lime">United Colors of Benetton Women&#039;s Top lime</a>
                                            </div>

                                            <div class="item-content">

                                                <div class="rating">
                                                    <div class="ratings">
                                                        <div class="rating-box">
                                                            <div style="width: 86.6%" class="rating"></div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="item-price">
                                                    <div class="price-box">
                                                        <del><span class="amount">&#36;20.00</span></del> <ins><span class="amount">&#36;16.00</span></ins>
                                                    </div>
                                                </div>

                                                <div class="action">
                                                    <a class="button btn-cart" title='Select options'
                                                        onclick='window.location.assign("http://wordpress.magikthemes.com/linea/product/united-colors-of-benetton-womens-top-lime/")'>
                                                        <span>Select options
                                                        </span>
                                                    </a>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="item">
                                <div class="item-inner">
                                    <div class="item-img">
                                        <div class="item-img-info">
                                            <a href="http://wordpress.magikthemes.com/linea/product/united-colors-of-benetton-casual-sleeveless-printed-womens-top/" title="United Colors of Benetton Casual Sleeveless Printed Women&#039;s Top" class="product-image">
                                                <figure class="img-responsive">
                                                    <img alt="United Colors of Benetton Casual Sleeveless Printed Women&#039;s Top" src="http://wordpress.magikthemes.com/linea/wp-content/uploads/sites/2/2016/01/product20-6-300x362.jpg">
                                                </figure>
                                            </a>
                                            <div class="box-hover">
                                                <ul class="add-to-links">
                                                    <li>


                                                        <a class="detail-bnt yith-wcqv-button link-quickview"
                                                            data-product_id="105"></a>


                                                        <li>
                                                            <a title="Add to Wishlist" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add_to_wishlist=105" data-product-id="105" data-product-type="simple" class="link-wishlist"></a>

                                                        </li>
                                                    <li>


                                                        <a title="Add to Compare" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?action=yith-woocompare-add-product&#038;id=105&#038;_wpnonce=cd4491022c" class="link-compare add_to_compare compare " data-product_id="105"></a>


                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="item-info">
                                        <div class="info-inner">

                                            <div class="item-title">
                                                <a href="http://wordpress.magikthemes.com/linea/product/united-colors-of-benetton-casual-sleeveless-printed-womens-top/"
                                                    title="United Colors of Benetton Casual Sleeveless Printed Women&#039;s Top">United Colors of Benetton Casual Sleeveless Printed Women&#039;s Top</a>
                                            </div>

                                            <div class="item-content">

                                                <div class="rating">
                                                    <div class="ratings">
                                                        <div class="rating-box">
                                                            <div style="width: 0%" class="rating"></div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="item-price">
                                                    <div class="price-box">
                                                        <span class="amount">&#36;20.00</span>
                                                    </div>
                                                </div>

                                                <div class="action">
                                                    <a class="single_add_to_cart_button add_to_cart_button  product_type_simple ajax_add_to_cart button btn-cart" title='Add to cart' data-quantity="1" data-product_id="105"
                                                        href='/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add-to-cart=105'>
                                                        <span>Add to cart </span>
                                                    </a>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="item">
                                <div class="item-inner">
                                    <div class="item-img">
                                        <div class="item-img-info">
                                            <a href="http://wordpress.magikthemes.com/linea/product/riot-jeans-casual-roll-up-sleeve-printed-womens-top/" title="Riot Jeans Casual Roll-up Sleeve Printed Women&#039;s Top" class="product-image">
                                                <figure class="img-responsive">
                                                    <img alt="Riot Jeans Casual Roll-up Sleeve Printed Women&#039;s Top" src="http://wordpress.magikthemes.com/linea/wp-content/uploads/sites/2/2016/01/product21-3-300x362.jpg">
                                                </figure>
                                            </a>
                                            <div class="sale-label sale-top-right">
                                                Sale
                                            </div>
                                            <div class="box-hover">
                                                <ul class="add-to-links">
                                                    <li>


                                                        <a class="detail-bnt yith-wcqv-button link-quickview"
                                                            data-product_id="102"></a>


                                                        <li>
                                                            <a title="Add to Wishlist" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add_to_wishlist=102" data-product-id="102" data-product-type="simple" class="link-wishlist"></a>

                                                        </li>
                                                    <li>


                                                        <a title="Add to Compare" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?action=yith-woocompare-add-product&#038;id=102&#038;_wpnonce=cd4491022c" class="link-compare add_to_compare compare " data-product_id="102"></a>


                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="item-info">
                                        <div class="info-inner">

                                            <div class="item-title">
                                                <a href="http://wordpress.magikthemes.com/linea/product/riot-jeans-casual-roll-up-sleeve-printed-womens-top/"
                                                    title="Riot Jeans Casual Roll-up Sleeve Printed Women&#039;s Top">Riot Jeans Casual Roll-up Sleeve Printed Women&#039;s Top</a>
                                            </div>

                                            <div class="item-content">

                                                <div class="rating">
                                                    <div class="ratings">
                                                        <div class="rating-box">
                                                            <div style="width: 80%" class="rating"></div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="item-price">
                                                    <div class="price-box">
                                                        <del><span class="amount">&#36;20.00</span></del> <ins><span class="amount">&#36;18.00</span></ins>
                                                    </div>
                                                </div>

                                                <div class="action">
                                                    <a class="single_add_to_cart_button add_to_cart_button  product_type_simple ajax_add_to_cart button btn-cart" title='Add to cart' data-quantity="1" data-product_id="102"
                                                        href='/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add-to-cart=102'>
                                                        <span>Add to cart </span>
                                                    </a>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="item">
                                <div class="item-inner">
                                    <div class="item-img">
                                        <div class="item-img-info">
                                            <a href="http://wordpress.magikthemes.com/linea/product/annabelle-by-pantaloons-formal-short-sleeve-solid-womens-top/" title="Annabelle by Pantaloons Formal Short Sleeve Solid Women&#039;s Top" class="product-image">
                                                <figure class="img-responsive">
                                                    <img alt="Annabelle by Pantaloons Formal Short Sleeve Solid Women&#039;s Top" src="http://wordpress.magikthemes.com/linea/wp-content/uploads/sites/2/2016/01/product19-1-300x362.jpg">
                                                </figure>
                                            </a>
                                            <div class="box-hover">
                                                <ul class="add-to-links">
                                                    <li>


                                                        <a class="detail-bnt yith-wcqv-button link-quickview"
                                                            data-product_id="106"></a>


                                                        <li>
                                                            <a title="Add to Wishlist" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add_to_wishlist=106" data-product-id="106" data-product-type="simple" class="link-wishlist"></a>

                                                        </li>
                                                    <li>


                                                        <a title="Add to Compare" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?action=yith-woocompare-add-product&#038;id=106&#038;_wpnonce=cd4491022c" class="link-compare add_to_compare compare " data-product_id="106"></a>


                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="item-info">
                                        <div class="info-inner">

                                            <div class="item-title">
                                                <a href="http://wordpress.magikthemes.com/linea/product/annabelle-by-pantaloons-formal-short-sleeve-solid-womens-top/"
                                                    title="Annabelle by Pantaloons Formal Short Sleeve Solid Women&#039;s Top">Annabelle by Pantaloons Formal Short Sleeve Solid Women&#039;s Top</a>
                                            </div>

                                            <div class="item-content">

                                                <div class="rating">
                                                    <div class="ratings">
                                                        <div class="rating-box">
                                                            <div style="width: 100%" class="rating"></div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="item-price">
                                                    <div class="price-box">
                                                        <span class="amount">&#36;47.99</span>
                                                    </div>
                                                </div>

                                                <div class="action">
                                                    <a class="single_add_to_cart_button add_to_cart_button  product_type_simple ajax_add_to_cart button btn-cart" title='Add to cart' data-quantity="1" data-product_id="106"
                                                        href='/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add-to-cart=106'>
                                                        <span>Add to cart </span>
                                                    </a>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="item">
                                <div class="item-inner">
                                    <div class="item-img">
                                        <div class="item-img-info">
                                            <a href="http://wordpress.magikthemes.com/linea/product/casual-sleeveless-solid-womens-top/" title="Casual Sleeveless Solid Women&#039;s Top" class="product-image">
                                                <figure class="img-responsive">
                                                    <img alt="Casual Sleeveless Solid Women&#039;s Top" src="http://wordpress.magikthemes.com/linea/wp-content/uploads/sites/2/2016/01/product17-6-300x362.jpg">
                                                </figure>
                                            </a>
                                            <div class="box-hover">
                                                <ul class="add-to-links">
                                                    <li>


                                                        <a class="detail-bnt yith-wcqv-button link-quickview"
                                                            data-product_id="2701"></a>


                                                        <li>
                                                            <a title="Add to Wishlist" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add_to_wishlist=2701" data-product-id="2701" data-product-type="simple" class="link-wishlist"></a>

                                                        </li>
                                                    <li>


                                                        <a title="Add to Compare" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?action=yith-woocompare-add-product&#038;id=2701&#038;_wpnonce=cd4491022c" class="link-compare add_to_compare compare " data-product_id="2701"></a>


                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="item-info">
                                        <div class="info-inner">

                                            <div class="item-title">
                                                <a href="http://wordpress.magikthemes.com/linea/product/casual-sleeveless-solid-womens-top/"
                                                    title="Casual Sleeveless Solid Women&#039;s Top">Casual Sleeveless Solid Women&#039;s Top</a>
                                            </div>

                                            <div class="item-content">

                                                <div class="rating">
                                                    <div class="ratings">
                                                        <div class="rating-box">
                                                            <div style="width: 0%" class="rating"></div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="item-price">
                                                    <div class="price-box">
                                                        <span class="amount">&#36;77.99</span>
                                                    </div>
                                                </div>

                                                <div class="action">
                                                    <a class="single_add_to_cart_button add_to_cart_button  product_type_simple ajax_add_to_cart button btn-cart" title='Add to cart' data-quantity="1" data-product_id="2701"
                                                        href='/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add-to-cart=2701'>
                                                        <span>Add to cart </span>
                                                    </a>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="item">
                                <div class="item-inner">
                                    <div class="item-img">
                                        <div class="item-img-info">
                                            <a href="http://wordpress.magikthemes.com/linea/product/united-colors-of-benetton-womens-top-orange/" title="United Colors of Benetton Women&#039;s Top Orange" class="product-image">
                                                <figure class="img-responsive">
                                                    <img alt="United Colors of Benetton Women&#039;s Top Orange" src="http://wordpress.magikthemes.com/linea/wp-content/uploads/sites/2/2016/01/product24-2-300x362.jpg">
                                                </figure>
                                            </a>
                                            <div class="box-hover">
                                                <ul class="add-to-links">
                                                    <li>


                                                        <a class="detail-bnt yith-wcqv-button link-quickview"
                                                            data-product_id="104"></a>


                                                        <li>
                                                            <a title="Add to Wishlist" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add_to_wishlist=104" data-product-id="104" data-product-type="simple" class="link-wishlist"></a>

                                                        </li>
                                                    <li>


                                                        <a title="Add to Compare" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?action=yith-woocompare-add-product&#038;id=104&#038;_wpnonce=cd4491022c" class="link-compare add_to_compare compare " data-product_id="104"></a>


                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="item-info">
                                        <div class="info-inner">

                                            <div class="item-title">
                                                <a href="http://wordpress.magikthemes.com/linea/product/united-colors-of-benetton-womens-top-orange/"
                                                    title="United Colors of Benetton Women&#039;s Top Orange">United Colors of Benetton Women&#039;s Top Orange</a>
                                            </div>

                                            <div class="item-content">

                                                <div class="rating">
                                                    <div class="ratings">
                                                        <div class="rating-box">
                                                            <div style="width: 100%" class="rating"></div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="item-price">
                                                    <div class="price-box">
                                                        <span class="amount">&#36;20.00</span>
                                                    </div>
                                                </div>

                                                <div class="action">
                                                    <a class="single_add_to_cart_button add_to_cart_button  product_type_simple ajax_add_to_cart button btn-cart" title='Add to cart' data-quantity="1" data-product_id="104"
                                                        href='/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add-to-cart=104'>
                                                        <span>Add to cart </span>
                                                    </a>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>

                </div>
            </div>
        </div>

        <!-- End related products Slider -->

        <meta itemprop="url" content="http://wordpress.magikthemes.com/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/" />

    </div>
    <!--itemscope-->
</div>
<!--product-view-->
