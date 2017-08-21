<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductTopDisplay.ascx.cs" Inherits="HotterWinds.Modules.Products.ProductTopDisplay" %>

<style type="text/css">
    .product-view .product-shop .ratings {
        border-bottom: none;
        padding-top: 10px;
        padding-bottom: 0px;
    }

    .product-view .product-shop .price-box {
        border-bottom: none;
        float: left;
    }

    .product-view .product-shop .shipping-box {
        float: left;
        padding: 18px 0;
    }

        .product-view .product-shop .shipping-box .shiping {
            padding: 10px 10px 0 10px;
            display: inline-block;
        }

        .product-view .product-shop .shipping-box .retailer_name {
            font-size: 19px;
            font-weight: bold;
        }

    .price-block {
        position: relative;
    }

        .price-block .retailer_logo {
            padding: 10px 0px;
            height: 40px;
            width: 120px;
        }

        .price-block .retailer_msg {
            padding: 10px 0px;
        }

    .product-view .product-shop .shipping-box .visit_shop {
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

        .product-view .product-shop .shipping-box .visit_shop:hover {
            text-decoration: none;
        }

    .availability.in-stock {
        position: inherit;
        top: inherit;
        left: inherit;
    }

    .google_ad_block {
        width: 300px;
        height: 300px;
        position: absolute;
        top: 15px;
        left: 450px;        
        display:none;
    }

    @media (min-width:1300px) {
        .google_ad_block {
            display:block;
        }

        .logo_block {
            width:450px;   
        }
    }

</style>

<div class="product-shop col-lg-8 col-sm-7 col-xs-12">
    <div class="product-name">
        <h1 itemprop="name" class="product_title entry-title"><%=Product.ProductName %></h1>
    </div>

    <div itemprop="offers" itemscope itemtype="http://schema.org/Offer" class="price-block">

        <div class="ratings">
            <div class="rating-box">
                <div style="width: <%=Product.ProductRatingSum %>0%" class="rating"></div>
            </div>
            <span>0 reviews</span>
        </div>

        <div>
            <div class="price-box price">
                <span class="amount"><%=GetPriceRange()%></span>
            </div>
            <div class="shipping-box">
                <span class="shiping">
                    <%if (RetailerProduct.Freight == 0) %>
                    <%{ %>
                    +&nbsp;Free&nbsp;shipping
                    <%} %>
                    <%else if (RetailerProduct.Freight < 0) %>
                    <%{ %>
                    <%} %>
                    <%else %>
                    <%{ %>
                    +&nbsp;$ <%=RetailerProduct.Freight %>&nbsp;shipping
                    <%} %>                    
                    at
                </span>

                <span class="retailer_name"><%=Retailer.RetailerName %></span>

                <a class="visit_shop" target="_blank" href="<%=VisitShopRetailerProductURL %>" onclick="dataLayer.push({'transactionId': '<%=Guid.NewGuid() %>','transactionProducts': [{ 'name': '<%=RetailerProduct.RetailerProductName %>', 'sku': '<%=RetailerProduct.RetailerId %>','category': <%=Product.CategoryID %>, 'price': <%=RetailerProduct.RetailerPrice %>, 'quantity': 1, 'dimension1' : '<%=RetailerProduct.RetailerId %>'}],'event': 'pmco_trans'});">Visit Shop ></a>
            </div>
            <div class="clr"></div>
        </div>

        <%if (InStockStatus == 1) %>
        <%{ %>
        <p class="availability in-stock"><span>In Stock</span></p>
        <%} %>
        <%else %>
        <%{ %>
        <p class="availability in-stock"><span>Out Of Stock</span></p>
        <%} %>

        <div class="clr"></div>

        <div class="logo_block" style="border-bottom: 1px #ddd dotted;">
            <p class="retailer_logo">
                <img width="120" height="40" src="<%=PriceMe.Utility.GetImage(Retailer.LogoFile, "_ms") %>" /></p>
            <p class="retailer_msg"><%=Retailer.RetailerMessage %></p>
        </div>

        <div class="clr"></div>

        <div class="google_ad_block">
            <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
            <!-- Hotter Winds Medium Rectangle -->
            <ins class="adsbygoogle"
                style="display: inline-block; width: 300px; height: 250px"
                data-ad-client="ca-pub-6992217816861590"
                data-ad-slot="6007033367"></ins>
            <script>
                (adsbygoogle = window.adsbygoogle || []).push({});
            </script>
        </div>
    </div>

    <%--    <div itemprop="description" class="short-description">
        <h2>Quick Overview</h2>
        <p><%=GetShorDescription() %></p>
    </div>--%>

    <%--    <div class="yith-wcwl-add-to-wishlist add-to-wishlist-799">
        <div class="yith-wcwl-add-button show" style="display: block">


            <a href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?add_to_wishlist=799" rel="nofollow" data-product-id="799" data-product-type="external" class="add_to_wishlist">Add to Wishlist
            </a>
            <img src="http://wordpress.magikthemes.com/linea/wp-content/plugins/yith-woocommerce-wishlist/assets/images/wpspin_light.gif" class="ajax-loading" alt="loading" width="16" height="16" style="visibility: hidden" />
        </div>

        <div class="yith-wcwl-wishlistaddedbrowse hide" style="display: none;">
            <span class="feedback">Product added!</span>
            <a href="http://wordpress.magikthemes.com/linea/wishlist/view/" rel="nofollow">Browse Wishlist
            </a>
        </div>

        <div class="yith-wcwl-wishlistexistsbrowse hide" style="display: none">
            <span class="feedback">The product is already in the wishlist!</span>
            <a href="http://wordpress.magikthemes.com/linea/wishlist/view/" rel="nofollow">Browse Wishlist
            </a>
        </div>

        <div style="clear: both"></div>
        <div class="yith-wcwl-wishlistaddresponse"></div>

    </div>

    <div class="clear"></div>
    <a href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/?action=yith-woocompare-add-product&amp;id=799&amp;_wpnonce=cd4491022c" class="compare button" data-product_id="799" rel="nofollow">Compare</a>
    <div class="product_meta"></div>--%>

    <div class="social">
        <ul>
            <li class="fb pull-left">
                <div class="fb-like" data-href="https://hotterwinds.co.nz" data-layout="standard" data-action="like" data-size="large" data-show-faces="false" data-share="false"></div>
            </li>
            <%--<li class="fb pull-left">
                <a onclick="window.open('https://www.facebook.com/sharer.php?s=100&amp;p[url]=http%3A%2F%2Fwordpress.magikthemes.com%2Flinea%2Fproduct%2Ftoday-fashion-casual-sleeveless-solid-womens-top-light-pink%2F','sharer', 'toolbar=0,status=0,width=620,height=280');" href="javascript:;"></a>
            </li>

            <li class="tw pull-left">
                <a onclick="popUp=window.open('http://twitter.com/home?status=Today+Fashion+Casual+Sleeveless+Solid+Women%26%238217%3Bs+Top+Light+Pink http%3A%2F%2Fwordpress.magikthemes.com%2Flinea%2Fproduct%2Ftoday-fashion-casual-sleeveless-solid-womens-top-light-pink%2F','sharer','scrollbars=yes,width=800,height=400');popUp.focus();return false;" href="javascript:;"></a>
            </li>

            <li class="googleplus pull-left">
                <a href="javascript:;" onclick="popUp=window.open('https://plus.google.com/share?url=http%3A%2F%2Fwordpress.magikthemes.com%2Flinea%2Fproduct%2Ftoday-fashion-casual-sleeveless-solid-womens-top-light-pink%2F','sharer','scrollbars=yes,width=800,height=400');popUp.focus();return false;"></a>
            </li>

            <li class="linkedin pull-left">
                <a onclick="popUp=window.open('http://linkedin.com/shareArticle?mini=true&amp;url=http%3A%2F%2Fwordpress.magikthemes.com%2Flinea%2Fproduct%2Ftoday-fashion-casual-sleeveless-solid-womens-top-light-pink%2F&amp;title=Today+Fashion+Casual+Sleeveless+Solid+Women%26%238217%3Bs+Top+Light+Pink','sharer','scrollbars=yes,width=800,height=400');popUp.focus();return false;" href="javascript:;"></a>
            </li>

            <li class="pintrest pull-left">
                <a onclick="popUp=window.open('http://pinterest.com/pin/create/button/?url=http%3A%2F%2Fwordpress.magikthemes.com%2Flinea%2Fproduct%2Ftoday-fashion-casual-sleeveless-solid-womens-top-light-pink%2F&amp;description=Today+Fashion+Casual+Sleeveless+Solid+Women%26%238217%3Bs+Top+Light+Pink&amp;media=http://wordpress.magikthemes.com/linea/wp-content/uploads/sites/2/2016/01/product3-2.jpg','sharer','scrollbars=yes,width=800,height=400');popUp.focus();return false;" href="javascript:;"></a>
            </li>--%>
        </ul>
    </div>


</div>

<div id="fb-root"></div>
<script>(function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_GB/sdk.js#xfbml=1&version=v2.9&appId=1202589909863312";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));</script>
