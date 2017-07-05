<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ContactUsSuccess.aspx.cs" Inherits="HotterWinds.About.ContactUsSuccess" %>

<%@ MasterType VirtualPath="~/Main.Master" %>



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
    <%--<script type="text/javascript">
        window._wpemojiSettings = { "baseUrl": "https:\/\/s.w.org\/images\/core\/emoji\/72x72\/", "ext": ".png", "source": { "concatemoji": "https:\/\/wordpress.magikthemes.com\/linea\/wp-includes\/js\/wp-emoji-release.min.js?ver=4.4.1" } };
        !function (a, b, c) { function d(a) { var c, d = b.createElement("canvas"), e = d.getContext && d.getContext("2d"); return e && e.fillText ? (e.textBaseline = "top", e.font = "600 32px Arial", "flag" === a ? (e.fillText(String.fromCharCode(55356, 56806, 55356, 56826), 0, 0), d.toDataURL().length > 3e3) : "diversity" === a ? (e.fillText(String.fromCharCode(55356, 57221), 0, 0), c = e.getImageData(16, 16, 1, 1).data.toString(), e.fillText(String.fromCharCode(55356, 57221, 55356, 57343), 0, 0), c !== e.getImageData(16, 16, 1, 1).data.toString()) : ("simple" === a ? e.fillText(String.fromCharCode(55357, 56835), 0, 0) : e.fillText(String.fromCharCode(55356, 57135), 0, 0), 0 !== e.getImageData(16, 16, 1, 1).data[0])) : !1 } function e(a) { var c = b.createElement("script"); c.src = a, c.type = "text/javascript", b.getElementsByTagName("head")[0].appendChild(c) } var f, g; c.supports = { simple: d("simple"), flag: d("flag"), unicode8: d("unicode8"), diversity: d("diversity") }, c.DOMReady = !1, c.readyCallback = function () { c.DOMReady = !0 }, c.supports.simple && c.supports.flag && c.supports.unicode8 && c.supports.diversity || (g = function () { c.readyCallback() }, b.addEventListener ? (b.addEventListener("DOMContentLoaded", g, !1), a.addEventListener("load", g, !1)) : (a.attachEvent("onload", g), b.attachEvent("onreadystatechange", function () { "complete" === b.readyState && c.readyCallback() })), f = c.source || {}, f.concatemoji ? e(f.concatemoji) : f.wpemoji && f.twemoji && (e(f.twemoji), e(f.wpemoji))) }(window, document, window._wpemojiSettings);
    </script>--%>
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
    <script type="text/javascript">
        $("body").attr("class", "information-information-4");
        $("#page").attr("class", "category_page");
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="foot" runat="server">
    <script type='text/javascript'>
        /* <![CDATA[ */
        var wc_add_to_cart_params = { "ajax_url": "\/linea\/wp-admin\/admin-ajax.php", "wc_ajax_url": "\/linea\/?wc-ajax=%%endpoint%%", "i18n_view_cart": "View Cart", "cart_url": "https:\/\/wordpress.magikthemes.com\/linea\/cart\/", "is_cart": "", "cart_redirect_after_add": "no" };
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
        var yith_woocompare = { "nonceadd": "62a378ef6d", "nonceremove": "13b5943f4b", "nonceview": "193493de98", "ajaxurl": "https:\/\/wordpress.magikthemes.com\/linea\/wp-admin\/admin-ajax.php", "actionadd": "yith-woocompare-add-product", "actionremove": "yith-woocompare-remove-product", "actionview": "yith-woocompare-view-table", "added_label": "Added", "table_title": "Product Comparison", "auto_open": "yes", "loader": "https:\/\/wordpress.magikthemes.com\/linea\/wp-content\/plugins\/yith-woocommerce-compare\/assets\/images\/loader.gif", "button_text": "Compare" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/yith-woocommerce-compare/assets/js/woocompare.js?ver=2.0.5'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/yith-woocommerce-compare/assets/js/jquery.colorbox-min.js?ver=1.4.21'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var yith_qv = { "ajaxurl": "https:\/\/wordpress.magikthemes.com\/linea\/wp-admin\/admin-ajax.php", "is2_2": "", "loader": "https:\/\/wordpress.magikthemes.com\/linea\/wp-content\/plugins\/yith-woocommerce-quick-view\/assets\/image\/qv-loader.gif" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/yith-woocommerce-quick-view/assets/js/frontend.js?ver=1.0'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/prettyPhoto/jquery.prettyPhoto.min.js?ver=3.1.5'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/prettyPhoto/jquery.prettyPhoto.init.min.js?ver=2.4.12'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/yith-woocommerce-wishlist/assets/js/jquery.selectBox.min.js?ver=1.2.0'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var yith_wcwl_l10n = { "ajax_url": "https:\/\/wordpress.magikthemes.com\/linea\/wp-admin\/admin-ajax.php", "redirect_to_cart": "no", "multi_wishlist": "", "hide_add_button": "1", "is_user_logged_in": "", "ajax_loader_url": "https:\/\/wordpress.magikthemes.com\/linea\/wp-content\/plugins\/yith-woocommerce-wishlist\/assets\/images\/ajax-loader.gif", "remove_from_wishlist_after_add_to_cart": "yes", "labels": { "cookie_disabled": "We are sorry, but this feature is available only if cookies are enabled on your browser.", "added_to_cart_message": "<div class=\"woocommerce-message\">Product correctly added to cart<\/div>" }, "actions": { "add_to_wishlist_action": "add_to_wishlist", "remove_from_wishlist_action": "remove_from_wishlist", "move_to_another_wishlist_action": "move_to_another_wishlsit", "reload_wishlist_and_adding_elem_action": "reload_wishlist_and_adding_elem" } };
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
        var js_linea_wishvar = { "MGK_ADD_TO_WISHLIST_SUCCESS_TEXT": "Product successfully added to wishlist <a href=\"https:\/\/wordpress.magikthemes.com\/linea\/wishlist\/view\/\">Browse Wishlist.<\/a>", "MGK_ADD_TO_WISHLIST_EXISTS_TEXT": "The product is already in the wishlist! <a href=\"https:\/\/wordpress.magikthemes.com\/linea\/wishlist\/view\/\">Browse Wishlist.<\/a>", "IMAGEURL": "https:\/\/wordpress.magikthemes.com\/linea\/wp-content\/themes\/linea\/images", "WOO_EXIST": "1", "SITEURL": "https:\/\/wordpress.magikthemes.com\/linea" };
/* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/common.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/jquery.mobile-menu.min.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/owl.carousel.min.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/cloud-zoom.js?ver=4.4.1'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var js_linea_vars = { "ajax_url": "https:\/\/wordpress.magikthemes.com\/linea\/wp-admin\/admin-ajax.php", "container_width": "760", "grid_layout_width": "20" };
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
    <style type="text/css">
        .button.continue:before {
            content: "\f061";
            font-family: FontAwesome;
            font-size: 12px;
            padding-right: 8px;
        }

        .col2-right-layout .col-main {
            padding: 5px 16px 5px 0;
        }
    </style>

    <div class="main-container col2-right-layout">
        <div class="main container">
            <div class="row">
                <div id="content" class="col-sm-12">
                    <div class="col-main">
                        <h1 style="font-size: 36px;">Contact Us</h1>
                        <p>Your enquiry has been successfully sent to the store owner!</p>
                        <div class="buttons">
                            <div class="pull-right">
                                <!-- <a href="" class="btn btn-primary"></a> -->
                                <button onclick="window.location='/About/ContactUs.aspx';" type="submit" class="button continue">Continue </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
