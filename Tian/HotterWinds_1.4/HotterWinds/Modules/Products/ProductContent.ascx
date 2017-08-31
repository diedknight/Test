<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductContent.ascx.cs" Inherits="HotterWinds.Modules.Products.ProductContent" %>
<%@ Register Src="~/Modules/Products/ProductImagesDisplay.ascx" TagPrefix="uc1" TagName="ProductImagesDisplay" %>
<%@ Register Src="~/Modules/Products/ProductTopDisplay.ascx" TagPrefix="uc1" TagName="ProductTopDisplay" %>
<%@ Register Src="~/Modules/Products/MDProductPriceCompareNomal.ascx" TagPrefix="uc37" TagName="ProductPriceCompareNomal" %>
<%@ Register Src="~/Modules/Products/ProductItemPriceCompareNomal.ascx" TagPrefix="uc37" TagName="ProductItemPriceCompareNomal" %>
<%@ Register Src="~/Modules/Products/ProductDescription.ascx" TagName="ProductDescription" TagPrefix="PD" %>
<%@ Register Src="~/Modules/Products/HWRelatedProducts.ascx" TagPrefix="PD" TagName="HWRelatedProducts" %>




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
                                <a href="#tab-compare">Compare</a>
                            </li>
                            <li class="description_tab">
                                <a href="#tab-description">Description</a>
                            </li>
                            <%--<li class="additional_tab">
                                <a href="#tab-additional">Additional Information</a>
                            </li>--%>
                            <li class="reviews_tab" style="display: none;">
                                <a href="#tab-reviews">Reviews (0)</a>
                            </li>
                        </ul>

                    </div>

                    <div id="productTabContent" class="tab-content">
                        <div class="panel entry-content" id="tab-compare">
                            <%--<div class="productCenter">
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
                                </div>
                            </div>--%>

                            <PD:HWRelatedProducts runat="server" ID="SimilarLinks1" />

                        </div>
                        <div class="panel entry-content" id="tab-description">
                            <div><%=ProductDesc %></div>
                            <div class="clr"></div>

                            <div class="productCenter">
                                <div class="pricesTD clearboth">
                                    <section>
                                        <PD:ProductDescription ID="ProductDescription1" runat="server" />
                                    </section>
                                </div>
                            </div>
                        </div>

                        <%--<div class="panel entry-content" id="tab-additional">
                            
                        </div>--%>

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
                                            <p class="must-log-in">You must be <a href="https://wordpress.magikthemes.com/linea/my-account/">logged in</a> to post a review.</p>
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

        <!--google ad-->
        <div>
            <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
            <!-- Hotter Winds Horisontal -->
            <ins class="adsbygoogle"
                style="display: block"
                data-ad-client="ca-pub-6992217816861590"
                data-ad-slot="9110394807"
                data-ad-format="auto"></ins>
            <script>
                $(document).ready(function () {
                    setTimeout(function () {
                        (adsbygoogle = window.adsbygoogle || []).push({});
                    }, 1);
                });
            </script>

        </div>


        <meta itemprop="url" content="https://wordpress.magikthemes.com/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/" />

    </div>
    <!--itemscope-->
</div>
<!--product-view-->
