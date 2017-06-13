<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductTopDisplay.ascx.cs" Inherits="HotterWinds.Modules.Products.ProductTopDisplay" %>

<div class="product-shop col-lg-8 col-sm-7 col-xs-12">
    <div class="product-name">
        <h1 itemprop="name" class="product_title entry-title"><%=Product.ProductName %></h1>
    </div>

    <div itemprop="offers" itemscope itemtype="http://schema.org/Offer" class="price-block">
        <div class="price-box price"><span class="amount"><%=Resources.Resource.PriceCurrency %> <%=GetPriceRange()%></span></div>

        <%--<p class="availability in-stock pull-right"><span>In Stock</span></p>--%>        
    </div>

    <div itemprop="description" class="short-description">
        <h2>Quick Overview</h2>
        <p><%=GetShorDescription() %></p>
    </div>

    <div class="yith-wcwl-add-to-wishlist add-to-wishlist-799">
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
    <div class="product_meta"></div>

    <div class="social">
        <ul>
            <li class="fb pull-left">
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
            </li>
        </ul>
    </div>


</div>

