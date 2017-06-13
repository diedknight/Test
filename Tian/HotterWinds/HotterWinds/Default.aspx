<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HotterWinds.Default" %>

<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">document.documentElement.className = document.documentElement.className + ' yes-js js_active js'</script>
    <style>
        #yith-quick-view-modal .yith-wcqv-main {
            background: #ffffff
        }

        #yith-quick-view-close {
            color: #cdcdcd
        }

            #yith-quick-view-close:hover {
                color: #ff0000
            }
    </style>

    <style>
        .wishlist_table .add_to_cart, a.add_to_wishlist.button.alt {
            border-radius: 16px;
            -moz-border-radius: 16px;
            -webkit-border-radius: 16px;
        }
    </style>
    <script type="text/javascript">
        var yith_wcwl_plugin_ajax_web_url = '/linea/wp-admin/admin-ajax.php';
    </script>

    <link rel="alternate" type="application/rss+xml" title="Linea Demo &raquo; Feed" href="/linea/feed/" />
    <link rel="alternate" type="application/rss+xml" title="Linea Demo &raquo; Comments Feed" href="/linea/comments/feed/" />
    <link rel="alternate" type="application/rss+xml" title="Linea Demo &raquo; Home Comments Feed" href="/linea/home/feed/" />
    <script type="text/javascript">
        window._wpemojiSettings = { "baseUrl": "http:\/\/s.w.org\/images\/core\/emoji\/72x72\/", "ext": ".png", "source": { "concatemoji": "http:\/\/wordpress.magikthemes.com\/linea\/wp-includes\/js\/wp-emoji-release.min.js?ver=4.4.1" } };
        !function (a, b, c) { function d(a) { var c, d = b.createElement("canvas"), e = d.getContext && d.getContext("2d"); return e && e.fillText ? (e.textBaseline = "top", e.font = "600 32px Arial", "flag" === a ? (e.fillText(String.fromCharCode(55356, 56806, 55356, 56826), 0, 0), d.toDataURL().length > 3e3) : "diversity" === a ? (e.fillText(String.fromCharCode(55356, 57221), 0, 0), c = e.getImageData(16, 16, 1, 1).data.toString(), e.fillText(String.fromCharCode(55356, 57221, 55356, 57343), 0, 0), c !== e.getImageData(16, 16, 1, 1).data.toString()) : ("simple" === a ? e.fillText(String.fromCharCode(55357, 56835), 0, 0) : e.fillText(String.fromCharCode(55356, 57135), 0, 0), 0 !== e.getImageData(16, 16, 1, 1).data[0])) : !1 } function e(a) { var c = b.createElement("script"); c.src = a, c.type = "text/javascript", b.getElementsByTagName("head")[0].appendChild(c) } var f, g; c.supports = { simple: d("simple"), flag: d("flag"), unicode8: d("unicode8"), diversity: d("diversity") }, c.DOMReady = !1, c.readyCallback = function () { c.DOMReady = !0 }, c.supports.simple && c.supports.flag && c.supports.unicode8 && c.supports.diversity || (g = function () { c.readyCallback() }, b.addEventListener ? (b.addEventListener("DOMContentLoaded", g, !1), a.addEventListener("load", g, !1)) : (a.attachEvent("onload", g), b.attachEvent("onreadystatechange", function () { "complete" === b.readyState && c.readyCallback() })), f = c.source || {}, f.concatemoji ? e(f.concatemoji) : f.wpemoji && f.twemoji && (e(f.twemoji), e(f.wpemoji))) }(window, document, window._wpemojiSettings);
    </script>
    <style type="text/css">
        img.wp-smiley,
        img.emoji {
            display: inline !important;
            border: none !important;
            box-shadow: none !important;
            height: 1em !important;
            width: 1em !important;
            margin: 0 .07em !important;
            vertical-align: -0.1em !important;
            background: none !important;
            padding: 0 !important;
        }
    </style>
    <link rel='stylesheet' id='siteorigin-panels-front-css' href='/linea/wp-content/plugins/siteorigin-panels/css/front.css?ver=2.1.2' type='text/css' media='all' />
    <link rel='stylesheet' id='woocommerce-layout-css' href='/linea/wp-content/plugins/woocommerce/assets/css/woocommerce-layout.css?ver=2.4.12' type='text/css' media='all' />
    <link rel='stylesheet' id='woocommerce-smallscreen-css' href='/linea/wp-content/plugins/woocommerce/assets/css/woocommerce-smallscreen.css?ver=2.4.12' type='text/css' media='only screen and (max-width: 768px)' />
    <link rel='stylesheet' id='woocommerce-general-css' href='/linea/wp-content/plugins/woocommerce/assets/css/woocommerce.css?ver=2.4.12' type='text/css' media='all' />
    <link rel='stylesheet' id='megamenu-css' href='/linea/wp-admin/admin-ajax.php?action=megamenu_css&#038;ver=1.7.3.1' type='text/css' media='all' />
    <link rel='stylesheet' id='dashicons-css' href='/linea/wp-includes/css/dashicons.min.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='jquery-colorbox-css' href='/linea/wp-content/plugins/yith-woocommerce-compare/assets/css/colorbox.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='yith-woocompare-widget-css' href='/linea/wp-content/plugins/yith-woocommerce-compare/assets/css/widget.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='yith-quick-view-css' href='/linea/wp-content/plugins/yith-woocommerce-quick-view/assets/css/yith-quick-view.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='woocommerce_prettyPhoto_css-css' href='/linea/wp-content/plugins/woocommerce/assets/css/prettyPhoto.css?ver=3.1.6' type='text/css' media='all' />
    <link rel='stylesheet' id='jquery-selectBox-css' href='/linea/wp-content/plugins/yith-woocommerce-wishlist/assets/css/jquery.selectBox.css?ver=1.2.0' type='text/css' media='all' />
    <link rel='stylesheet' id='yith-wcwl-main-css' href='/linea/wp-content/plugins/yith-woocommerce-wishlist/assets/css/style.css?ver=2.0.13' type='text/css' media='all' />
    <link rel='stylesheet' id='yith-wcwl-font-awesome-css' href='/linea/wp-content/plugins/yith-woocommerce-wishlist/assets/css/font-awesome.min.css?ver=4.3.0' type='text/css' media='all' />
    <link rel='stylesheet' id='linea-Fonts-css' href='https://fonts.googleapis.com/css?family=Open+Sans%3A700%2C600%2C800%2C400%7CPoppins%3A400%2C300%2C500%2C600%2C700&#038;subset=latin%2Clatin-ext&#038;ver=1.0.0' type='text/css' media='all' />
    <link rel='stylesheet' id='bootstrap-css' href='/linea/wp-content/themes/linea/css/bootstrap.min.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='font-awesome-css' href='/linea/wp-content/themes/linea/css/font-awesome.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='simple-line-icons-css' href='/linea/wp-content/themes/linea/css/simple-line-icons.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='owl.carousel-css' href='/linea/wp-content/themes/linea/css/owl.carousel.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='owl.theme-css' href='/linea/wp-content/themes/linea/css/owl.theme.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='flexslider-css' href='/linea/wp-content/themes/linea/css/flexslider.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='jquery.bxslider-css' href='/linea/wp-content/themes/linea/css/jquery.bxslider.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='linea-style-css' href='/linea/wp-content/themes/linea/style.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='linea-blog-css' href='/linea/wp-content/themes/linea/skins/default/emerald/blogs.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='linea-revslider-css' href='/linea/wp-content/themes/linea/skins/default/emerald/revslider.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='linea-layout-css' href='/linea/wp-content/themes/linea/skins/default/emerald/style.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='linea-mgk_menu-css' href='/linea/wp-content/themes/linea/skins/default/emerald/mgk_menu.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='linea-jquery.mobile-menu-css' href='/linea/wp-content/themes/linea/skins/default/emerald/jquery.mobile-menu.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='linea-custom-style-css' href='/linea/wp-content/themes/linea/css/custom.css?ver=4.4.1' type='text/css' media='all' />
    <link rel='stylesheet' id='mc4wp-form-basic-css' href='/linea/wp-content/plugins/mailchimp-for-wp/assets/css/form-basic.min.css?ver=3.0.9' type='text/css' media='all' />
    <script type='text/javascript' src='/linea/wp-includes/js/jquery/jquery.js?ver=1.11.3'></script>
    <script type='text/javascript' src='/linea/wp-includes/js/jquery/jquery-migrate.min.js?ver=1.2.1'></script>
    <script type='text/javascript' src='/linea/wp-includes/js/hoverIntent.min.js?ver=1.8.1'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var megamenu = { "effect": { "fade": { "in": { "animate": { "opacity": "show" } }, "out": { "animate": { "opacity": "hide" } } }, "slide": { "in": { "animate": { "height": "show" }, "css": { "display": "none" } }, "out": { "animate": { "height": "hide" } } } }, "fade_speed": "fast", "slide_speed": "fast", "timeout": "300" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/megamenu/js/maxmegamenu.js?ver=1.7.3.1'></script>
    <link rel='https://api.w.org/' href='/linea/wp-json/' />
    <link rel="EditURI" type="application/rsd+xml" title="RSD" href="/linea/xmlrpc.php?rsd" />
    <link rel="wlwmanifest" type="application/wlwmanifest+xml" href="/linea/wp-includes/wlwmanifest.xml" />
    <meta name="generator" content="WordPress 4.4.1" />
    <meta name="generator" content="WooCommerce 2.4.12" />
    <link rel="canonical" href="/linea/" />
    <link rel='shortlink' href='/linea/' />
    <style type="text/css">
        .recentcomments a {
            display: inline !important;
            padding: 0 !important;
            margin: 0 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyStyle" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="foot" runat="server">
    <script type='text/javascript'>
        /* <![CDATA[ */
        var wc_add_to_cart_params = { "ajax_url": "\/linea\/wp-admin\/admin-ajax.php", "wc_ajax_url": "\/linea\/?wc-ajax=%%endpoint%%", "i18n_view_cart": "View Cart", "cart_url": "http:\/\/wordpress.magikthemes.com\/linea\/cart\/", "is_cart": "", "cart_redirect_after_add": "no" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/frontend/add-to-cart.min.js?ver=2.4.12'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/jquery-blockui/jquery.blockUI.min.js?ver=2.70'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var woocommerce_params = { "ajax_url": "\/linea\/wp-admin\/admin-ajax.php", "wc_ajax_url": "\/linea\/?wc-ajax=%%endpoint%%" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/frontend/woocommerce.min.js?ver=2.4.12'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/jquery-cookie/jquery.cookie.min.js?ver=1.4.1'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var wc_cart_fragments_params = { "ajax_url": "\/linea\/wp-admin\/admin-ajax.php", "wc_ajax_url": "\/linea\/?wc-ajax=%%endpoint%%", "fragment_name": "wc_fragments" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/frontend/cart-fragments.min.js?ver=2.4.12'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var yith_woocompare = { "nonceadd": "62a378ef6d", "nonceremove": "13b5943f4b", "nonceview": "193493de98", "ajaxurl": "http:\/\/wordpress.magikthemes.com\/linea\/wp-admin\/admin-ajax.php", "actionadd": "yith-woocompare-add-product", "actionremove": "yith-woocompare-remove-product", "actionview": "yith-woocompare-view-table", "added_label": "Added", "table_title": "Product Comparison", "auto_open": "yes", "loader": "http:\/\/wordpress.magikthemes.com\/linea\/wp-content\/plugins\/yith-woocommerce-compare\/assets\/images\/loader.gif", "button_text": "Compare" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/yith-woocommerce-compare/assets/js/woocompare.js?ver=2.0.5'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/yith-woocommerce-compare/assets/js/jquery.colorbox-min.js?ver=1.4.21'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var yith_qv = { "ajaxurl": "http:\/\/wordpress.magikthemes.com\/linea\/wp-admin\/admin-ajax.php", "is2_2": "", "loader": "http:\/\/wordpress.magikthemes.com\/linea\/wp-content\/plugins\/yith-woocommerce-quick-view\/assets\/image\/qv-loader.gif" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/yith-woocommerce-quick-view/assets/js/frontend.js?ver=1.0'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/prettyPhoto/jquery.prettyPhoto.min.js?ver=3.1.5'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/prettyPhoto/jquery.prettyPhoto.init.min.js?ver=2.4.12'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/yith-woocommerce-wishlist/assets/js/jquery.selectBox.min.js?ver=1.2.0'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var yith_wcwl_l10n = { "ajax_url": "http:\/\/wordpress.magikthemes.com\/linea\/wp-admin\/admin-ajax.php", "redirect_to_cart": "no", "multi_wishlist": "", "hide_add_button": "1", "is_user_logged_in": "", "ajax_loader_url": "http:\/\/wordpress.magikthemes.com\/linea\/wp-content\/plugins\/yith-woocommerce-wishlist\/assets\/images\/ajax-loader.gif", "remove_from_wishlist_after_add_to_cart": "yes", "labels": { "cookie_disabled": "We are sorry, but this feature is available only if cookies are enabled on your browser.", "added_to_cart_message": "<div class=\"woocommerce-message\">Product correctly added to cart<\/div>" }, "actions": { "add_to_wishlist_action": "add_to_wishlist", "remove_from_wishlist_action": "remove_from_wishlist", "move_to_another_wishlist_action": "move_to_another_wishlsit", "reload_wishlist_and_adding_elem_action": "reload_wishlist_and_adding_elem" } };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/yith-woocommerce-wishlist/assets/js/jquery.yith-wcwl.js?ver=2.0.13'></script>
    <script type='text/javascript' src='/linea/wp-includes/js/comment-reply.min.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/bootstrap.min.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/jquery.cookie.min.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/countdown.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/parallax.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/revslider.js?ver=4.4.1'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var js_linea_wishvar = { "MGK_ADD_TO_WISHLIST_SUCCESS_TEXT": "Product successfully added to wishlist <a href=\"http:\/\/wordpress.magikthemes.com\/linea\/wishlist\/view\/\">Browse Wishlist.<\/a>", "MGK_ADD_TO_WISHLIST_EXISTS_TEXT": "The product is already in the wishlist! <a href=\"http:\/\/wordpress.magikthemes.com\/linea\/wishlist\/view\/\">Browse Wishlist.<\/a>", "IMAGEURL": "http:\/\/wordpress.magikthemes.com\/linea\/wp-content\/themes\/linea\/images", "WOO_EXIST": "1", "SITEURL": "http:\/\/wordpress.magikthemes.com\/linea" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/common.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/jquery.mobile-menu.min.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/owl.carousel.min.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/cloud-zoom.js?ver=4.4.1'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var js_linea_vars = { "ajax_url": "http:\/\/wordpress.magikthemes.com\/linea\/wp-admin\/admin-ajax.php", "container_width": "760", "grid_layout_width": "20" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/mgk_menu.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-includes/js/wp-embed.min.js?ver=4.4.1'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var mc4wp_forms_config = [];
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/mailchimp-for-wp/assets/js/forms-api.min.js?ver=3.0.9'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var wc_add_to_cart_variation_params = { "i18n_no_matching_variations_text": "Sorry, no products matched your selection. Please choose a different combination.", "i18n_unavailable_text": "Sorry, this product is unavailable. Please choose a different combination." };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/frontend/add-to-cart-variation.min.js?ver=2.4.12'></script>
    <script type="text/javascript">
        jQuery('.timer-grid').each(function () {
            var countTime = jQuery(this).attr('data-time'); jQuery(this).countdown(countTime, function (event) { jQuery(this).html('<div class="day box-time-date"><span class="number">' + event.strftime('%D') + ' </span>days</div> <div class="hour box-time-date"><span class="number">' + event.strftime('%H') + '</span>Hrs</div><div class="min box-time-date"><span class="number">' + event.strftime('%M') + '</span>MINS</div> <div class="sec box-time-date"><span class="number">' + event.strftime('%S') + ' </span>SEC</div>'); });
        });
    </script>
    <script type="text/javascript">
        (function () {
            function addEventListener(element, event, handler) {
                if (element.addEventListener) {
                    element.addEventListener(event, handler, false);
                } else if (element.attachEvent) {
                    element.attachEvent('on' + event, handler);
                }
            }
        })();
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="row">
            <div class="col-md-3 col-md-4 col-sm-3 hidden-xs"></div>

            <div class="col-md-9 col-sm-9 col-xs-12 home-slider">
                <div id="magik-slideshow" class="magik-slideshow slider-block">



                    <div id='rev_slider_4_wrapper' class='rev_slider_wrapper fullwidthbanner-container'>
                        <div id='rev_slider_4' class='rev_slider fullwidthabanner'>
                            <ul>

                                <li data-transition='random' data-slotamount='7' data-masterspeed='1000' data-thumb='https://s3.pricemestatic.com/Images/HotterWindsVersion/slide-img1-150x150.jpg'>
                                    <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/slide-img1.jpg" data-data-bgposition='left top' data-bgfit='cover' data-bgrepeat='no-repeat' alt="https://s3.pricemestatic.com/Images/HotterWindsVersion/slide-img1.jpg" />
                                    <div class="info">
                                        <div class='tp-caption ExtraLargeTitle sft  tp-resizeme ' data-endspeed='500' data-speed='500' data-start='1100' data-easing='Linear.easeNone' data-splitin='none' data-splitout='none' data-elementdelay='0.1' data-endelementdelay='0.1' style='z-index: 2; max-width: auto; max-height: auto; white-space: nowrap;'><span>Womens style</span> </div>
                                        <div class='tp-caption LargeTitle sfl  tp-resizeme ' data-endspeed='500' data-speed='500' data-start='1300' data-easing='Linear.easeNone' data-splitin='none' data-splitout='none' data-elementdelay='0.1' data-endelementdelay='0.1' style='z-index: 3; max-width: auto; max-height: auto; white-space: nowrap;'><span>Season Sale</span> </div>
                                        <div class='tp-caption Title sft  tp-resizeme ' data-endspeed='500' data-speed='500' data-start='1450' data-easing='Power2.easeInOut' data-splitin='none' data-splitout='none' data-elementdelay='0.1' data-endelementdelay='0.1' style='z-index: 4; max-width: auto; max-height: auto; white-space: nowrap;'>In augue urna, nunc, tincidunt, augue, augue facilisis facilisis.</div>
                                        <div class='tp-caption sfb  tp-resizeme ' data-endspeed='500' data-speed='500' data-start='1500' data-easing='Linear.easeNone' data-splitin='none' data-splitout='none' data-elementdelay='0.1' data-endelementdelay='0.1' style='z-index: 4; max-width: auto; max-height: auto; white-space: nowrap;'><a href='#' class="buy-btn">Shop Now</a> </div>
                                    </div>
                                    <a class="s-link" href="http://wordpress.magikthemes.com/linea/"></a>



                                </li>


                                <li data-transition='random' data-slotamount='7' data-masterspeed='1000' data-thumb='https://s3.pricemestatic.com/Images/HotterWindsVersion/slide-img2-150x150.jpg'>
                                    <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/slide-img2.jpg" data-data-bgposition='left top' data-bgfit='cover' data-bgrepeat='no-repeat' alt="https://s3.pricemestatic.com/Images/HotterWindsVersion/slide-img2.jpg" />
                                    <div class="info">
                                        <div class='tp-caption ExtraLargeTitle sft slide2  tp-resizeme ' data-endspeed='500' data-speed='500' data-start='1100' data-easing='Linear.easeNone' data-splitin='none' data-splitout='none' data-elementdelay='0.1' data-endelementdelay='0.1' style='z-index: 2; max-width: auto; max-height: auto; white-space: nowrap; padding-right: 0px'><span>Super Sale</span> </div>
                                        <div class='tp-caption LargeTitle sfl  tp-resizeme ' data-endspeed='500' data-speed='500' data-start='1300' data-easing='Linear.easeNone' data-splitin='none' data-splitout='none' data-elementdelay='0.1' data-endelementdelay='0.1' style='z-index: 3; max-width: auto; max-height: auto; white-space: nowrap;'>digital life</div>
                                        <div class='tp-caption Title sft  tp-resizeme ' data-endspeed='500' data-speed='500' data-start='1500' data-easing='Power2.easeInOut' data-splitin='none' data-splitout='none' data-elementdelay='0.1' data-endelementdelay='0.1' style='z-index: 4; max-width: auto; max-height: auto; white-space: nowrap;'>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</div>
                                        <div class='tp-caption sfb  tp-resizeme ' data-endspeed='500' data-speed='500' data-start='1500' data-easing='Linear.easeNone' data-splitin='none' data-splitout='none' data-elementdelay='0.1' data-endelementdelay='0.1' style='z-index: 4; max-width: auto; max-height: auto; white-space: nowrap;'><a href='#' class="buy-btn">Buy Now</a> </div>
                                    </div>
                                    <a class="s-link" href="http://wordpress.magikthemes.com/linea/"></a>



                                </li>


                            </ul>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>


    <div class="main-container col2-left-layout">
        <div class="container">
            <div class="row">
                <div class="col-sm-9 col-sm-push-3">


                    <div class="promotion-banner">
                        <div class="row">



                            <div class="col-lg-5 col-sm-5">
                                <a href="http://wordpress.magikthemes.com/linea/" title=" Banner 1">
                                    <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/home-banner1.jpg" alt=" Banner 1">
                                </a>
                            </div>




                            <div class="col-lg-7 col-sm-7">
                                <a href="http://wordpress.magikthemes.com/linea/" title=" Banner 2">
                                    <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/home-banner2.jpg" alt=" Banner 2">
                                </a>
                            </div>

                        </div>
                    </div>



                    <div class="category-product">
                        <div class="navbar nav-menu">
                            <div class="navbar-collapse">
                                <div class="new_title">
                                    <h2>New Products</h2>
                                </div>

                                <ul class="nav navbar-nav">
                                    <li class=" active ">
                                        <a href="#cat-982" data-toggle="tab"><%=Model.DressesName %></a>
                                    </li>

                                    <li class="">
                                        <a href="#cat-1191" data-toggle="tab"><%=Model.SunGlassesName %></a>
                                    </li>

                                    <li class="">
                                        <a href="#cat-1177" data-toggle="tab"><%=Model.TentsName %></a>
                                    </li>

                                    <li class="">
                                        <a href="#cat-963" data-toggle="tab"><%=Model.BootsName %></a>
                                    </li>

                                </ul>
                            </div>
                        </div>

                        <!-- Tab panes -->
                        <div class="product-bestseller">
                            <div class="product-bestseller-content">
                                <div class="product-bestseller-list">
                                    <div class="tab-container">
                                        <div class="tab-panel  active " id="cat-982">
                                            <div class="category-products">
                                                <ul class="products-grid">

                                                    <% foreach (Product p in Model.Dresses)%>
                                                    <%{ %>
                                                    <li class="item col-lg-3 col-md-3 col-sm-4 col-xs-6">
                                                        <div class="item-inner">
                                                            <div class="item-img">
                                                                <div class="item-img-info">
                                                                    <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>" class="product-image">
                                                                        <figure class="img-responsive">
                                                                            <img alt="<%=p.ProductName%>" src="<%=p.ImgUrl%>" onerror="onImgError2(this)">
                                                                        </figure>
                                                                    </a>
                                                                </div>
                                                            </div>

                                                            <div class="item-info">
                                                                <div class="info-inner">

                                                                    <div class="item-title">
                                                                        <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>"><%=p.ProductName%></a>
                                                                    </div>

                                                                    <div class="item-content">

                                                                        <div class="rating">
                                                                            <div class="ratings">
                                                                                <div class="rating-box">
                                                                                    <div style="width: <%=p.Stars%>0%" class="rating"></div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="item-price">
                                                                            <div class="price-box">
                                                                                <span class="amount">&#36;<%=p.Price.ToString("0.00")%></span>
                                                                            </div>
                                                                        </div>

                                                                        <div class="action">
                                                                            <a target="_blank" class="button btn-cart" href="<%=p.PurchaseUrl%>">
                                                                                <span>Visit Shop</span>
                                                                            </a>

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <%} %>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="tab-panel " id="cat-1191">
                                            <div class="category-products">
                                                <ul class="products-grid">

                                                    <% foreach (Product p in Model.SunGlasses)%>
                                                    <% {%>
                                                    <li class="item col-lg-3 col-md-3 col-sm-4 col-xs-6">
                                                        <div class="item-inner">
                                                            <div class="item-img">
                                                                <div class="item-img-info">
                                                                    <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>" class="product-image">
                                                                        <figure class="img-responsive">
                                                                            <img alt="<%=p.ProductName%>" src="<%=p.ImgUrl%>" onerror="onImgError2(this)">
                                                                        </figure>
                                                                    </a>
                                                                </div>
                                                            </div>

                                                            <div class="item-info">
                                                                <div class="info-inner">

                                                                    <div class="item-title">
                                                                        <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>"><%=p.ProductName%></a>
                                                                    </div>

                                                                    <div class="item-content">

                                                                        <div class="rating">
                                                                            <div class="ratings">
                                                                                <div class="rating-box">
                                                                                    <div style="width: <%=p.Stars%>0%" class="rating"></div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="item-price">
                                                                            <div class="price-box">
                                                                                <span class="amount">&#36;<%=p.Price.ToString("0.00")%></span>
                                                                            </div>
                                                                        </div>

                                                                        <div class="action">
                                                                            <a target="_blank" class="button btn-cart" href="<%=p.PurchaseUrl%>">
                                                                                <span>Visit Shop</span>
                                                                            </a>

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <%} %>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="tab-panel " id="cat-1177">
                                            <div class="category-products">
                                                <ul class="products-grid">
                                                    <%foreach (Product p in Model.Tents) %>
                                                    <%{ %>
                                                    <li class="item col-lg-3 col-md-3 col-sm-4 col-xs-6">
                                                        <div class="item-inner">
                                                            <div class="item-img">
                                                                <div class="item-img-info">
                                                                    <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>" class="product-image">
                                                                        <figure class="img-responsive">
                                                                            <img alt="<%=p.ProductName%>" src="<%=p.ImgUrl%>" onerror="onImgError2(this)">
                                                                        </figure>
                                                                    </a>
                                                                </div>
                                                            </div>

                                                            <div class="item-info">
                                                                <div class="info-inner">

                                                                    <div class="item-title">
                                                                        <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>"><%=p.ProductName%></a>
                                                                    </div>

                                                                    <div class="item-content">

                                                                        <div class="rating">
                                                                            <div class="ratings">
                                                                                <div class="rating-box">
                                                                                    <div style="width: <%=p.Stars%>0%" class="rating"></div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="item-price">
                                                                            <div class="price-box">
                                                                                <span class="amount">&#36;<%=p.Price.ToString("0.00")%></span>
                                                                            </div>
                                                                        </div>

                                                                        <div class="action">
                                                                            <a target="_blank" class="button btn-cart" href="<%=p.PurchaseUrl%>">
                                                                                <span>Visit Shop</span>
                                                                            </a>

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <%} %>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="tab-panel " id="cat-963">
                                            <div class="category-products">
                                                <ul class="products-grid">
                                                    <%foreach (Product p in Model.Boots) %>
                                                    <%{ %>
                                                    <li class="item col-lg-3 col-md-3 col-sm-4 col-xs-6">
                                                        <div class="item-inner">
                                                            <div class="item-img">
                                                                <div class="item-img-info">
                                                                    <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>" class="product-image">
                                                                        <figure class="img-responsive">
                                                                            <img alt="<%=p.ProductName%>" src="<%=p.ImgUrl%>" onerror="onImgError2(this)">
                                                                        </figure>
                                                                    </a>
                                                                </div>
                                                            </div>

                                                            <div class="item-info">
                                                                <div class="info-inner">

                                                                    <div class="item-title">
                                                                        <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>"><%=p.ProductName%></a>
                                                                    </div>

                                                                    <div class="item-content">

                                                                        <div class="rating">
                                                                            <div class="ratings">
                                                                                <div class="rating-box">
                                                                                    <div style="width: <%=p.Stars%>0%" class="rating"></div>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="item-price">
                                                                            <div class="price-box">
                                                                                <span class="amount">&#36;<%=p.Price.ToString("0.00")%></span>
                                                                            </div>
                                                                        </div>

                                                                        <div class="action">
                                                                            <a target="_blank" class="button btn-cart" href="<%=p.PurchaseUrl%>">
                                                                                <span>Visit Shop</span>
                                                                            </a>

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <%} %>
                                                </ul>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>


                    <div class="bestsell-pro">
                        <div>
                            <div class="slider-items-products">
                                <div class="bestsell-block">

                                    <div class="block-title">
                                        <h2>Best Sellers </h2>
                                    </div>

                                    <div id="bestsell-slider" class="product-flexslider hidden-buttons">
                                        <div class="slider-items slider-width-col4 products-grid block-content">
                                            <%foreach (Product p in Model.BestSellerProducts) %>
                                            <%{ %>
                                            <div class="item">
                                                <div class="item-inner">
                                                    <div class="item-img">
                                                        <div class="item-img-info">
                                                            <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>" class="product-image">
                                                                <figure class="img-responsive">
                                                                    <img alt="<%=p.ProductName%>" src="<%=p.ImgUrl%>" onerror="onImgError2(this)" />
                                                                </figure>
                                                            </a>

                                                        </div>
                                                    </div>

                                                    <div class="item-info">
                                                        <div class="info-inner">

                                                            <div class="item-title">
                                                                <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>"><%=p.ProductName%></a>
                                                            </div>

                                                            <div class="item-content">

                                                                <div class="rating">
                                                                    <div class="ratings">
                                                                        <div class="rating-box">
                                                                            <div style="width: <%=p.Stars%>0%" class="rating"></div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="item-price">
                                                                    <div class="price-box">
                                                                        <ins><span class="amount">&#36;<%=p.Price.ToString("0.00")%></span></ins>
                                                                    </div>
                                                                </div>

                                                                <div class="action">
                                                                    <a target="_blank" class="button btn-cart" href="<%=p.PurchaseUrl%>">
                                                                        <span>Visit Shop</span>
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
                    </div>



                    <div class="featured-pro-block">
                        <div class="slider-items-products">

                            <div class="new-arrivals-block">
                                <div class="block-title">
                                    <h2>Featured Product</h2>
                                </div>
                            </div>

                            <div id="new-arrivals-slider" class="product-flexslider hidden-buttons">
                                <div class="home-block-inner"></div>
                                <div class="slider-items slider-width-col4 products-grid block-content">
                                    <%foreach (Product p in Model.FeaturesProducts) %>
                                    <%{ %>
                                    <div class="item">
                                        <div class="item-inner">
                                            <div class="item-img">
                                                <div class="item-img-info">
                                                    <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>" class="product-image">
                                                        <figure class="img-responsive">
                                                            <img alt="<%=p.ProductName%>" src="<%=p.ImgUrl%>" onerror="onImgError2(this)" />
                                                        </figure>
                                                    </a>

                                                </div>
                                            </div>

                                            <div class="item-info">
                                                <div class="info-inner">

                                                    <div class="item-title">
                                                        <a href="<%=PriceMe.UrlController.GetProductUrl(p.ProductId,p.ProductName) %>" title="<%=p.ProductName%>"><%=p.ProductName%></a>
                                                    </div>

                                                    <div class="item-content">

                                                        <div class="rating">
                                                            <div class="ratings">
                                                                <div class="rating-box">
                                                                    <div style="width: <%=p.Stars%>0%" class="rating"></div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="item-price">
                                                            <div class="price-box">
                                                                <ins><span class="amount">&#36;<%=p.Price.ToString("0.00")%></span></ins>
                                                            </div>
                                                        </div>

                                                        <div class="action">
                                                            <a target="_blank" class="button btn-cart" href="<%=p.PurchaseUrl %>">
                                                                <span>Visit Shop</span>
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

                <aside class="col-left sidebar col-sm-3 col-xs-12 col-sm-pull-9">
                    <div class="custom-slider-wrap">
                        <div class="custom-slider-inner">
                            <div class="home-custom-slider">
                                <div>
                                    <div id="carousel-example-generic" class="carousel slide" data-ride="carousel">
                                        <ol class="carousel-indicators">
                                            <li class=" active " data-target="#carousel-example-generic" data-slide-to="0"></li>
                                            <li class="" data-target="#carousel-example-generic" data-slide-to="1"></li>
                                            <li class="" data-target="#carousel-example-generic" data-slide-to="2"></li>
                                        </ol>

                                        <div class="carousel-inner">

                                            <div class="item  active ">
                                                <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/custom-slide1.jpg" alt="slider1" />

                                                <div class="carousel-caption">
                                                    <span>Mega Deal</span>
                                                    <p>Save up to <strong>70% OFF</strong> Fahion collection</p>
                                                </div>


                                            </div>

                                            <div class="item ">
                                                <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/custom-slide2.jpg" alt="slider2" />

                                                <div class="carousel-caption">
                                                    <span>Huge <strong>sale</strong></span>
                                                    <p>Save up to <strong>70% OFF</strong> Fahion collection</p>
                                                </div>


                                            </div>

                                            <div class="item ">
                                                <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/custom-slide3.jpg" alt="slider3" />

                                                <div class="carousel-caption">
                                                    <span>Hot <strong>Deal</strong></span>
                                                    <p>Save up to <strong>70% OFF</strong> Fahion collection</p>
                                                </div>


                                            </div>

                                        </div>

                                        <a class="left carousel-control" href="#" data-slide="prev"><span class="sr-only">Previous</span> </a><a class="right carousel-control" href="#" data-slide="next"><span class="sr-only">Next</span> </a>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="hot-deal">
                        <ul class="products-grid">
                            <li class="right-space two-height item">
                                <div class="item-inner">
                                    <div class="item-img">
                                        <div class="item-img-info">
                                            <a href="<%=PriceMe.UrlController.GetProductUrl(Model.HotDeal.ProductId,Model.HotDeal.ProductName) %>" title="<%=Model.HotDeal.ProductName%>" class="product-image">
                                                <figure class="img-responsive">
                                                    <img width="260" height="315" src="<%=Model.HotDeal.ImgUrl%>" class="attachment-260x315 size-260x315 wp-post-image" alt="product14" onerror="onImgError2(this)" />
                                                </figure>
                                            </a>
                                        </div>
                                    </div>

                                    <div class="item-info">
                                        <div class="info-inner">

                                            <div class="item-title">
                                                <a href="<%=Model.HotDeal.PurchaseUrl%>" title="<%=Model.HotDeal.ProductName%>"><%=Model.HotDeal.ProductName%></a>
                                            </div>

                                            <div class="item-content">

                                                <div class="rating">
                                                    <div class="ratings">
                                                        <div class="rating-box">
                                                            <div style="width: <%=Model.HotDeal.Stars %>0%" class="rating"></div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="item-price">
                                                    <div class="price-box">
                                                        <ins><span class="amount">&#36;<%=Model.HotDeal.Price.ToString("0.00") %></span></ins>
                                                    </div>
                                                </div>

                                                <div class="action">
                                                    <a class="single_add_to_cart_button add_to_cart_button  product_type_simple ajax_add_to_cart button btn-cart" title='Add to cart' data-quantity="1" data-product_id="116"
                                                        href='@Model.HotDeal.PurchaseUrl'>
                                                        <span>Visit Shop</span>
                                                    </a>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>

                        </ul>
                    </div>



                    <div class="testimonials">
                        <div class="ts-testimonial-widget">
                            <div class="slider-items-products">
                                <div id="testimonials-slider" class="product-flexslider hidden-buttons home-testimonials">
                                    <div class="slider-items slider-width-col4 fadeInUp">


                                        <div class="holder">
                                            <div class="thumb">
                                                <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/member1.jpg" data-bgposition='left top' data-bgfit='cover' data-bgrepeat='no-repeat'
                                                    alt="Saraha Smith" />
                                            </div>
                                            <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit, Lid est laborum dolo rumes fugats untras. dolore magna aliquam erat volutpat. Aenean est auctorwisiet urna. Aliquam erat volutpat.</p>

                                            <div class="line"></div>

                                            <strong class="name">
                                                <a href="#" target="_blank">Saraha Smith
                                                </a>
                                            </strong>
                                        </div>


                                        <div class="holder">
                                            <div class="thumb">
                                                <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/member2.jpg" data-bgposition='left top' data-bgfit='cover' data-bgrepeat='no-repeat'
                                                    alt="Mark doe" />
                                            </div>
                                            <p>Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nam sapien nunc, convallis in sollicitudin in, ullamcorper ad eulibero. Etiam cursus eu ipsum egestas.</p>

                                            <div class="line"></div>

                                            <strong class="name">
                                                <a href="#" target="_blank">Mark doe
                                                </a>
                                            </strong>
                                        </div>


                                        <div class="holder">
                                            <div class="thumb">
                                                <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/member3.jpg" data-bgposition='left top' data-bgfit='cover' data-bgrepeat='no-repeat'
                                                    alt="John Doe" />
                                            </div>
                                            <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Duis ac turpis. Donec sit amet eros.</p>

                                            <div class="line"></div>

                                            <strong class="name">
                                                <a href="#" target="_blank">John Doe
                                                </a>
                                            </strong>
                                        </div>


                                        <div class="holder">
                                            <div class="thumb">
                                                <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/member4.jpg" data-bgposition='left top' data-bgfit='cover' data-bgrepeat='no-repeat'
                                                    alt="Stephen Doe" />
                                            </div>
                                            <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci.</p>

                                            <div class="line"></div>

                                            <strong class="name">
                                                <a href="#" target="_blank">Stephen Doe
                                                </a>
                                            </strong>
                                        </div>


                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="featured-add-box">
                        <div class="featured-add-inner">

                            <a href="http://wordpress.magikthemes.com/linea/" title="bottom Banner 1">

                                <img src="https://s3.pricemestatic.com/Images/HotterWindsVersion/ads-07.jpg" alt="bottom Banner 1">
                            </a>


                            <div class="banner-content">
                                <div class="banner-text">Women's</div>
                                <div class="banner-text1">49% off</div>
                                <p>on selected products</p>
                                <a href="#" class="view-bnt">Shop now</a>
                            </div>


                        </div>
                    </div>
                </aside>
            </div>
        </div>
    </div>


    <div class="container">
        <div class="row">
            <div class="blog-outer-container">

                <div class="block-title">
                    <h2>Latest Blog</h2>
                </div>
                <div class="blog-inner">

                    <%foreach (var blog in Model.Blogs) %>
                    <%{ %>
                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <div class="entry-thumb image-hover2">
                            <a href="<%=blog.Link %>">
                                <img src="<%=blog.ImgUrl %>" alt="Pellentesque habitant morbi" onerror="onImgError_blog(this)">
                            </a>
                        </div>

                        <div class="blog-preview_info">

                            <h4 class="blog-preview_title">
                                <a href="<%=blog.Link %>"><%=blog.Title %></a>
                            </h4>

                            <ul class="post-meta">
                                <li>
                                    <i class="fa fa-user"></i>posted by <%=blog.Creator %>
                                </li>

                                <li>
                                    <i class="fa fa-comments"></i><%=blog.Comments %> Comments
                                </li>

                                <li>
                                    <i class="fa fa-clock-o"></i>
                                    <time datetime="<%=blog.PubDate %>" class="entry-date published"><%=blog.PubDate.ToString("MMM dd yyyy") %></time>
                                </li>
                            </ul>



                            <div class="blog-preview_desc">
                                <p><%=blog.Description %></p>
                            </div>
                            <a class="blog-preview_btn" href="<%=blog.Link %>">READ MORE</a>
                        </div>
                    </div>
                    <%} %>
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        $(".mega-menu-category").css("display", "");
    </script>
</asp:Content>
