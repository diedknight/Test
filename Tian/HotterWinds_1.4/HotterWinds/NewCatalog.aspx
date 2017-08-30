<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="NewCatalog.aspx.cs" Inherits="HotterWinds.NewCatalog" %>

<%@ MasterType VirtualPath="~/Main.master" %>

<%@ Register Src="~/Modules/Catalog/CatalogSiteMapNew.ascx" TagName="CatalogSiteMap" TagPrefix="DK" %>
<%@ Register Src="~/Modules/Catalog/CatalogSiteMapDetailNew.ascx" TagName="CatalogSiteMapDetail" TagPrefix="DK" %>
<%@ Register Src="~/Modules/Catalog/NewFilters.ascx" TagName="NewFilters" TagPrefix="DK" %>
<%@ Register Src="~/Modules/PrettyPager.ascx" TagName="PrettyPager" TagPrefix="DK" %>
<%@ Register Src="~/Modules/Catalog/CategoryTopControl.ascx" TagName="CategoryTopControl" TagPrefix="DK" %>
<%@ Register Src="~/Modules/Catalog/NewCatalogProducts.ascx" TagName="NewCatalogProducts" TagPrefix="DK" %>
<%@ Register Src="~/Modules/Breadcrumbs.ascx" TagPrefix="DK" TagName="Breadcrumbs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">document.documentElement.className = document.documentElement.className + ' yes-js js_active js'</script>
    <style>
        #yith-quick-view-modal .yith-wcqv-main {
            background: #ffffff;
        }

        #yith-quick-view-close {
            color: #cdcdcd;
        }

            #yith-quick-view-close:hover {
                color: #ff0000;
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

    <link rel="alternate" type="application/rss+xml" title="linea demo &raquo; Feed" href="/linea/feed/" />
    <link rel="alternate" type="application/rss+xml" title="linea demo &raquo; Comments Feed" href="/linea/comments/feed/" />
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
    <link rel="alternate" type="application/rss+xml" title="New products added to Clothing" href="/linea/shop/feed/?product_cat=clothing" />
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
        $("body").attr("class", "archive tax-product_cat term-clothing term-51 woocommerce woocommerce-page cms-index-index cms-linea-home");
    </script>

    <script type="text/javascript">

        var winHeight = $(window).height();
        var winWidth = $(window).width();

        var node = $("<div id=\"wrap\"></div>");
        node.css({ "height": winHeight, "width": winWidth, "position": "fixed", "background": "white", "z-index": "10000" });

        $("body").append(node);

        $(document).ready(function () {
            node.remove();
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style type="text/css">
        #sortByDiv {
            min-width: inherit;
            margin-right: 0px;
        }

        .sortByDiv select {
            width: auto !important;
            height: auto !important;
            color: black;
            padding: 6px 12px;
            padding-right: 25px;
            font-size: 14px;
            font-weight: 400;
        }

            .sortByDiv select:focus {
                padding: 6px 12px;
                padding-right: 25px;
            }

        @media (max-width: 1024px) {
            .sortByDiv_des {
                display: none;
            }
        }

        @media (min-width: 768px) {
            #leftFilters {
                width: 100%;
            }
        }

        @media(max-width:767px) {
            .clr_max768 {
                clear: both;
            }
        }

        @media only screen and (max-width: 479px) and (min-width: 320px) {
            button.button, .btn, button.button:hover {
                padding: 1px 5px;
            }
        }

        #dropdownMenuSortBy {
            border-radius: 0px;
        }

        .dropdown-menu {
            left: -48px;
        }

        .products-grid .item .item-inner .item-info .info-inner .item-title {
            height: 50px;
            white-space: normal;
        }


        @media (max-width: 768px) {
            .hideMax768 {
                display: none;
            }
        }
    </style>

    <script src="<%=PriceMe.WebConfig.CssJsPath %>/Scripts/new_catalog.js?ver=<%=PriceMe.WebConfig.WEB_cssVersion %>"></script>
    <div class="main-container col2-left-layout">
        <main>
            <%if (category.IsSiteMap)
                { %>
            <DK:CatalogSiteMap ID="CatalogSiteMap1" runat="server" />
            <%}
                else if (category.IsSiteMapDetail)
                {%>
            <DK:CatalogSiteMapDetail ID="CatalogSiteMapDetail1" runat="server" />
            <%
                }
                else
                {%>

            <%--<div class="container">
                <DK:CategoryTopControl ID="CategoryTopControl1" runat="server" />

                <div class="newBody">
                    <div id="rightProducts">
                        <div id="loadDiv">
                            <div id="catalogBar">
                                <%if (!v.Equals("quick", StringComparison.InvariantCultureIgnoreCase))
                                    { %>
                                <div id="compareDiv">
                                    <div class="compareContainer">
                                        <div id="compareSelected">
                                            <span class="clickable" onclick="compareProducts()"><span class="glyphicon glyphicon-stats"></span><%=Resources.Resource.TextString_CompareFeatures %> <span class="glyphicon glyphicon-chevron-right"></span></span><span id="imagesList"></span>
                                        </div>
                                    </div>
                                </div>

                                <%} %>

                                <div id="sortByDiv" class="sortByDiv">
                                    <%if (catalogPageInfo.CurrentProductCount > 1 && !v.Equals("quick", StringComparison.InvariantCultureIgnoreCase))
                                        { %>
                                    <label title="<%=Resources.Resource.TextString_CatalogSortby%>"><span class="glyphicon glyphicon-sort"></span>&nbsp;</label>

                                    <div class="dropdown">
                                        <button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenuSortBy" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                            <%=GetSortByTitle(sortBy, sortByInfoList, sortByInfoList[0].LinkText)%>
                                            <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu" aria-labelledby="dropdownMenuSortBy">
                                            <%foreach (var lk in sortByInfoList)
                                                { %>
                                            <li><a href="<%=lk.LinkURL %>"><%=lk.LinkText%></a></li>
                                            <%} %>
                                        </ul>
                                    </div>
                                    <%} %>
                                </div>

                                <%string view = v;
                                    if (string.IsNullOrEmpty(view))
                                        view = DefaultView; %>
                                <div id="viewByDiv" class="viewByDiv btn-group<%= view.Equals("quick", StringComparison.InvariantCultureIgnoreCase)?" MarginR0":"" %>">
                                    <label title="<%=Resources.Resource.TextString_CatalogView%>"><span class="glyphicon glyphicon-eye-open"></span>&nbsp;</label>
                                    <a class="btn btnViewByGray btn-sm <%= view.Equals("Grid", StringComparison.InvariantCultureIgnoreCase)?"disabled":"" %>" href="<%=GridViewUrl%>">
                                        <span class="glyphicon glyphicon-th-large"></span><%=Resources.Resource.TextString_Gallery %>
                                    </a>
                                    <a class="btn btnViewByGray btn-sm <%= view.Equals("List", StringComparison.InvariantCultureIgnoreCase)?"disabled":"" %>" href="<%=ListViewUrl%>">
                                        <span class="glyphicon glyphicon-th-list"></span><%= Resources.Resource.TextString_List %>
                                    </a>
                                    <a class="btn btnViewByGray btn-sm <%= view.Equals("Quick", StringComparison.InvariantCultureIgnoreCase)?"disabled":"" %>" href="<%=QuickViewUrl%>">
                                        <span class="glyphicon glyphicon-align-justify"></span><%=Resources.Resource.TextString_Quick %>
                                    </a>
                                </div>


                            </div>

                            <div id="pcDiv">
                                <%if (catalogPageInfo.CurrentProductCount == 0)
                                    { %>
                                <div id="statsbar2">
                                    <div class="alert alert-warning" style="margin: 10px;">
                                        <h4><span class="glyphicon glyphicon-info-sign"></span><%=string.IsNullOrEmpty(swi)? Resources.Resource.TextString_NoSearchResult1: string.Format(Resources.Resource.TextString_NoSearchResult,swi)%></h4>
                                    </div>
                                </div>
                                <%}%>
                                <DK:NewCatalogProducts ID="NewCatalogProducts1" runat="server" />
                            </div>

                            <div id="pagerDiv">
                                <DK:PrettyPager ID="PrettyPager1" runat="server" />
                            </div>
                        </div>

                    </div>


                </div>
            </div>--%>

            <div class="container">

                <div class="row">
                    <div class="newBody">

                        <div class="col-left sidebar col-sm-4 col-xs-12 col-md-3">
                            <div id="leftFilters">
                                <DK:NewFilters ID="NewFilters1" runat="server" />
                            </div>
                            <div class="clr"></div>

                            <!--google ad-->
                            <div class="hideMax768" style="text-align: center;">
                                <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
                                <!-- Hotter Winds Skyscraper -->
                                <ins class="adsbygoogle"
                                    style="display: inline-block; width: 160px; height: 600px"
                                    data-ad-client="ca-pub-6992217816861590"
                                    data-ad-slot="4006064329"></ins>
                                <script>
                                    (adsbygoogle = window.adsbygoogle || []).push({});
                                </script>

                            </div>

                        </div>
                        <!-- col-sm-3   -->
                        <div class="clr_max768"></div>
                        <div id="loadDiv">
                            <div class="col-sm-8 col-md-9">
                                <DK:Breadcrumbs runat="server" ID="Breadcrumbs" />

                                <div class="page-title">
                                    <h2 class="page-heading">
                                        <span class="page-heading-title"><%=category.CategoryName %></span>
                                    </h2>
                                </div>

                                <div class="col-main pro-grid">

                                    <div class="toolbar">
                                        <div class="display-product-option">

                                            <div class="sorter">
                                                <div class="view-mode">
                                                    <a id="gridBtn" href="#" class="grid-trigger button-active button-grid"></a>
                                                    <a id="listBtn" href="#" title="List" class="list-trigger  button-list"></a>
                                                </div>
                                            </div>

                                            <div style="float: right;">
                                                <%if (catalogPageInfo.CurrentProductCount > 1 && !v.Equals("quick", StringComparison.InvariantCultureIgnoreCase))
                                                    { %>
                                                <label class="sortByDiv_des left">Sort By: </label>
                                                <div id="sortByDiv" class="sortByDiv" style="display: inline-block">

                                                    <div class="dropdown">
                                                        <button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenuSortBy" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                            <%=GetSortByTitle(sortBy, sortByInfoList, sortByInfoList[0].LinkText)%>
                                                            <span class="caret"></span>
                                                        </button>
                                                        <ul class="dropdown-menu" aria-labelledby="dropdownMenuSortBy">
                                                            <%foreach (var lk in sortByInfoList)
                                                                { %>
                                                            <li><a href="<%=lk.LinkURL %>"><%=lk.LinkText%></a></li>
                                                            <%} %>
                                                        </ul>
                                                    </div>
                                                </div>
                                                <%} %>

                                                <div class="clear"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="pcDiv" class="category-products">
                                        <DK:NewCatalogProducts ID="NewCatalogProducts1" runat="server" />
                                        <div class="clr"></div>
                                    </div>

                                    <div class="toolbar bottom">

                                        <div id="pagerDiv">
                                            <DK:PrettyPager ID="PrettyPager1" runat="server" />
                                        </div>
                                        <%--                                        <div class="woocommerce-pagination pager pages">
                                            <ul class='page-numbers'>
                                                <li><span class='page-numbers current'>1</span></li>
                                                <li><a class='page-numbers' href='http://wordpress.magikthemes.com/linea/product-category/dresses/clothing/page/2/'>2</a></li>
                                                <li><a class='page-numbers' href='http://wordpress.magikthemes.com/linea/product-category/dresses/clothing/page/3/'>3</a></li>
                                                <li><a class="next page-numbers" href="http://wordpress.magikthemes.com/linea/product-category/dresses/clothing/page/2/">
                                                    <div class="page-separator-next">&raquo;</div>
                                                </a></li>
                                            </ul>
                                        </div>

                                        <p class="woocommerce-result-count">
                                            Showing 1&ndash;8 of 20 results
                                        </p>--%>
                                    </div>
                                </div>
                                <!--  col-main pro-grid    -->
                            </div>
                            <!-- col-sm-9   -->

                            <div class="clr"></div>
                        </div>


                        <script>
                                    initFilter();
                        </script>
                    </div>
                </div>
                <!-- row -->

                <!--google ad-->
                <div style="height: 90px; margin-top:15px;">
                    <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
                    <!-- Hotter Winds Horisontal -->
                    <ins class="adsbygoogle"
                        style="display: block"
                        data-ad-client="ca-pub-6992217816861590"
                        data-ad-slot="9110394807"
                        data-ad-format="auto"></ins>
                    <script>
                                    (adsbygoogle = window.adsbygoogle || []).push({});
                    </script>

                </div>
            </div>
            <!-- container -->
            <%}%>
        </main>



    </div>
    <!-- maincontainer -->

    <script type="text/javascript">
                            //$(".mega-menu-category").css("display", "none");



    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="foot" runat="server">
    <script type='text/javascript'>
                                    /* <![CDATA[ */
                                    var wc_add_to_cart_params = { "ajax_url": "\/linea\/wp-admin\/admin-ajax.php", "wc_ajax_url": "\/linea\/product-category\/dresses\/clothing\/?wc-ajax=%%endpoint%%", "i18n_view_cart": "View Cart", "cart_url": "https:\/\/wordpress.magikthemes.com\/linea\/cart\/", "is_cart": "", "cart_redirect_after_add": "no" };
        /* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/frontend/add-to-cart.min.js?ver=2.4.12'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/jquery-blockui/jquery.blockUI.min.js?ver=2.70'></script>
    <script type='text/javascript'>
                                        /* <![CDATA[ */
                                        var woocommerce_params = { "ajax_url": "\/linea\/wp-admin\/admin-ajax.php", "wc_ajax_url": "\/linea\/product-category\/dresses\/clothing\/?wc-ajax=%%endpoint%%" };
        /* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/frontend/woocommerce.min.js?ver=2.4.12'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/jquery-cookie/jquery.cookie.min.js?ver=1.4.1'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var wc_cart_fragments_params = { "ajax_url": "\/linea\/wp-admin\/admin-ajax.php", "wc_ajax_url": "\/linea\/product-category\/dresses\/clothing\/?wc-ajax=%%endpoint%%", "fragment_name": "wc_fragments" };
        /* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/frontend/cart-fragments.min.js?ver=2.4.12'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var yith_woocompare = { "nonceadd": "3eada33b0d", "nonceremove": "88f09ee1cf", "nonceview": "86a1f613bf", "ajaxurl": "https:\/\/wordpress.magikthemes.com\/linea\/wp-admin\/admin-ajax.php", "actionadd": "yith-woocompare-add-product", "actionremove": "yith-woocompare-remove-product", "actionview": "yith-woocompare-view-table", "added_label": "Added", "table_title": "Product Comparison", "auto_open": "yes", "loader": "https:\/\/wordpress.magikthemes.com\/linea\/wp-content\/plugins\/yith-woocommerce-compare\/assets\/images\/loader.gif", "button_text": "Compare" };
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
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/bootstrap.min.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/jquery.cookie.min.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/countdown.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/parallax.js?ver=4.4.1'></script>
    <script type='text/javascript' src='/linea/wp-content/themes/linea/js/revslider.js?ver=4.4.1'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var js_linea_wishvar = { "MGK_ADD_TO_WISHLIST_SUCCESS_TEXT": "Product successfully added to wishlist <a href=\"https:\/\/wordpress.magikthemes.com\/linea\/wishlist\/view\/\">Browse Wishlist.<\/a>", "MGK_ADD_TO_WISHLIST_EXISTS_TEXT": "The product is already in the wishlist! <a href=\"https:\/\/wordpress.magikthemes.com\/linea\/wishlist\/view\/\">Browse Wishlist.<\/a>", "IMAGEURL": "http:\/\/wordpress.magikthemes.com\/linea\/wp-content\/themes\/linea\/images", "WOO_EXIST": "1", "SITEURL": "https:\/\/wordpress.magikthemes.com\/linea" };
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
    <script type='text/javascript' src='/linea/wp-includes/js/jquery/ui/core.min.js?ver=1.11.4'></script>
    <script type='text/javascript' src='/linea/wp-includes/js/jquery/ui/widget.min.js?ver=1.11.4'></script>
    <script type='text/javascript' src='/linea/wp-includes/js/jquery/ui/mouse.min.js?ver=1.11.4'></script>
    <script type='text/javascript' src='/linea/wp-includes/js/jquery/ui/slider.min.js?ver=1.11.4'></script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/frontend/jquery-ui-touch-punch.min.js?ver=2.4.12'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var woocommerce_price_slider_params = { "currency_symbol": "$", "currency_pos": "left", "min_price": "", "max_price": "" };
        /* ]]> */
    </script>
    <script type='text/javascript' src='/linea/wp-content/plugins/woocommerce/assets/js/frontend/price-slider.min.js?ver=2.4.12'></script>
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
    <!-- WooCommerce JavaScript -->
    <script type="text/javascript">
        jQuery(function ($) {

            jQuery('.dropdown_layered_nav_size').change(function () {
                var term_id = parseInt(jQuery(this).val(), 10);
                location.href = '/linea/product-category/dresses/clothing/?filtering=1&filter_size=' + (isNaN(term_id) ? '' : term_id);
            });

        });
    </script>
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
