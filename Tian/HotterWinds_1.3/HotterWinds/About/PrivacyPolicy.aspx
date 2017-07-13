<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PrivacyPolicy.aspx.cs" Inherits="HotterWinds.About.PrivacyPolicy" %>

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
        .static-inner p {
            line-height: 22px !important;
        }
    </style>

    <div class="main-container col2-right-layout">
        <div class="main container">
            <div class="breadcrumbs">
                <ul>
                    <li>
                        <a href="/">Home</a>
                    </li>
                    <li>
                        <span>/</span>
                        <strong>Privacy Policy</strong>
                    </li>
                </ul>
            </div>
            <div class="row">
                <div id="content" class="col-sm-9 col-sm-push-3">
                    <div class="col-main">
                        <div class="static-inner">
                            <div class="page-title">
                                <h2>Privacy Policy</h2>
                            </div>
                            
                            <p>PRIVACY STATEMENT</p>
                            <p>Hotter Winds is a New Zealand travel, adventure and outdoor gear comparison website.  Hotter Winds lists and compares thousands of products from various retailers and distributors.  This information is presented in a familiar and simple way. This makes it easy to search and navigate with the aim to inform people when they are choosing their next equipment or apparel purchase. In order to do this effectively, often we need to collect some of your personal information.  We value your privacy and provide our services free of charge. If you do click through to an advertisers site then we will receive a commision on that but in no way does that  impact you. These commissions are what allows us to keep the site running, up to date and free of charge. We respect your personal data and we believe knowledge of our data privacy practices will enable you to make the best possible decisions regarding data privacy and your usage of the Hotter Winds website.</p>
                            <p>HOTTER WINDS PRIVACY AND PERSONAL DATA PROTECTION POLICY STATEMENT</p>
                            <p>1. General Information</p>
                            <p>Hotter Winds complies fully with the Privacy Act 1993. The operative legislation concerning personal data protection in New Zealand.  This policy statement details how Hotter Winds collects and uses your personal data as well as keeping you informed about the personal data protection measures we have put in place.  When using our Service, you consent to the collection, transfer, manipulation, storage, disclosure and other uses of your information only as described in this Privacy and Personal Data Protection Policy.</p>
                            <p>2. The Information We Collect</p>
                            <p>We do not collect personal information if you are only browsing this website.  Unless you enter your email address and subscribe to our newsletter you remain anonymous when you browse this website. </p>
                            <p>3. How We Collect Data</p>
                            <p>Hotter Winds collects your personal information only when you choose to register for and/ or use our services, for example in the process of signing up to our mailing list. This information is communicated to us when you voluntarily complete a registration form. By providing this information and volunteering your personal details, you undertake to communicate accurate information that does not prejudice the interest of any third parties.</p>
                            <p>4. Use of Information</p>
                            <p>Upon giving us consent, we may collect and use for the following purposes:</p>
                            <p>Refer you to the brands and retailers listed on our website and/or</p>
                            <p>To personalize and improve aspects of our overall service to you and our other users, as well as carrying out research such as analyzing market trends and customer demographics; and/or</p>
                            <p>To communicate with you, in order to verify the personal information provided, or to request any additional information that is vital; and/or</p>
                            <p>To send you information about products and services which we think may be of interest to you.</p>
                            <p>5. Passing on Information about You</p>
                            <p>You authorize Hotter Winds to disclose and transfer personal data to third parties, whose products are listed on the Hotter Winds website, and/ or their authorized representatives. These transactions will only be made with third parties that were consented by you, the user.  Hotter Winds will not sell, distribute, or lease your personal information to third parties unless we have your permission, or are required to do so under the law.</p>
                            <p>6. Retaining Your Information</p>
                            <p>Collected information is retained until it is destroyed.  We retain your personal information in order to carry out the data uses stated in section 4 above.  This includes retaining your data to contact you with products or services we believe may be of interest to you in the future.  During the period that Hotter Winds retains your data, we are committed to all reasonable efforts and practical steps to ensure that collected data is protected against any loss, misuse, modification, unauthorized or accidental access or disclosure, alteration or destruction. Hotter Winds undertakes to protect the confidentiality of your data to the purposes set out in section 4 only, and therefore to ensure your data privacy.  We will ensure that your personal data is permanently deleted once it is no longer required to fulfill its original purpose, or as is required by the law.</p>
                            <p>7. Accessing Data and Ensuring Accuracy</p>
                            <p>Hotter Winds takes all reasonable steps to ensure that your information is accurate, complete, not misleading and kept up-to-date, consistent with the purpose for which the information was collected.  If you would like to access or correct personal information that is inaccurate, incomplete, misleading or not up-to-date please contact us via email on info@hotterwinds.co.nz.  We will make all reasonable efforts to accommodate requests for access and information changes.  Similarly, please contact us should you wish to withdraw consent for the use of your personal information and/or unsubscribe from our emails.</p>
                            <p>8. Keeping You Informed</p>
                            <p>You agree that Hotter Winds shall be entitled to advise you via email regarding any offers we think may interest you.  Whenever you provide us with personal information, we will give you an opportunity to tell us you do not consent to us and others using and sharing your information for marketing purposes. Unless you tell us otherwise, you are consenting to such uses. Any electronic marketing communications we send you will include clear and concise instructions to follow should you wish to unsubscribe at any time. Should you no longer wish to be contacted by us, you can advise us at any time by emailing us at info@hotterwinds.co.nz and requesting that you are unsubscribed.  </p>
                            <p>9. Improving our Service to You</p>
                            <p>In order to ensure the Services we provide you continue to meet your needs, we may ask you for feedback on your experience of using the website. Any feedback you provide will only be used as part of our program of continuous improvement and will not be published on the website. Third party providers and data privacy. If you decide to enter into a contract with a third party provider through this Website, the information you have provided to us together with any further information requested by, and supplied by you or us to the third party provider will be held by the provider for the purposes set out in that provider's privacy policy. Therefore, you are strongly advised to read your chosen provider's privacy policy and satisfy yourself as to the purposes for which the provider will use your personal information for entering into the contract. We have no responsibility for the uses to which a provider puts your personal information.</p>
                            <p>10. Website Browsing and Cookies</p>
                            <p>A cookie is a very small text file placed on your computer. Cookies help websites like Hotter Winds to:</p>
                            <p>understand  the number of visitors and browsing habits on the Hotter Winds Website and the pages visited, and remember you when you return to the Hotter Winds Website so we can provide you with a better browsing experience.</p>
                            <p>Most cookies are deleted as soon as you close your browser, these are known as session cookies. Others, known as persistent cookies, are stored on your computer either until you delete them or they expire. By using this Service, you consent to us using cookies. This website uses a variety of different cookies including Google Analytics and others for a range of purposes.  If you wish to receive a complete list please contact us.</p>
                            <p>11. Changes to This Policy</p>
                            <p>We may revise this Privacy Policy from time to time. If we make a change to this policy that, in sole discretion, is material, we will post it clearly on the website. By continuing to access or use the Hotter Winds Website after those changes become effective, you agree to be bound by the revised Privacy Policy. This Privacy Policy was last updated on 29 June 2017.</p>

                        </div>
                    </div>
                </div>

                <aside id="column-left" class="col-left sidebar col-sm-3 col-xs-12 col-sm-pull-9">
                    <div>
                        <div class="side-banner">
                        </div>
                    </div>
                    <div class="block block-company">
                        <div class="block-title">
                            Information
                        </div>
                        <div class="block-content">
                            <ul>
                                <li><a href="/About/AboutUs.aspx">About Us</a></li>
                                <li><a href="/About/PrivacyPolicy.aspx">Privacy Policy</a></li>
                                <li><a href="/About/TermsCondition.aspx">Terms &amp; Conditions</a></li>
                                <li><a href="/About/ContactUs.aspx">Contact Us</a></li>
                            </ul>
                        </div>
                    </div>
                </aside>
            </div>
        </div>
    </div>
</asp:Content>
