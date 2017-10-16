<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManCategoryList1.ascx.cs" Inherits="HotterWinds.Modules.ManCategoryList1" %>

<%@ Import Namespace="PriceMe" %>
<%@ Import Namespace="PriceMeCache" %>
<%@ Import Namespace="PriceMeCommon.BusinessLogic" %>

<ul id="menu-mainmenu-1" class="mobile-menu accordion-menu">
    <%foreach (PriceMeCache.CategoryCache rootCate in this.RootCategoryList) %>
    <%{ %>

    <%if (rootCate.ProductsCount == 0) continue; %>
    <%var subCateList = CategoryController.GetNextLevelSubCategories(rootCate.CategoryID, WebConfig.CountryId);%>
    <%subCateList = subCateList.OrderBy(item => item.ListOrder).ToList(); %>

    <%if (subCateList.Count == 0) %>
    <%{ %>
    <li id="nav-menu-item-<%=rootCate.CategoryID %>" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat">
        <a href="<%=PriceMe.UrlController.GetCatalogUrl(rootCate.CategoryID) %>" class="">
            <span><i class="fa fa-tag"></i><%=rootCate.CategoryName %></span>
        </a>
    </li>
    <%} %>
    <%else %>
    <%{ %>
    <%
        string widecss = "";
        if (subCateList.Count == 2) widecss = "has-sub wide col-2";
        if (subCateList.Count > 2) widecss = "has-sub wide col-3";

    %>
    <li id="nav-menu-item-<%=rootCate.CategoryID %>" class="menu-item menu-item-type-custom menu-item-object-custom menu-item-has-children has-sub">
        <a href="<%=PriceMe.UrlController.GetCatalogUrl(rootCate.CategoryID) %>" class=""><span><i class="fa fa-tag"></i><%=rootCate.CategoryName %></span></a>
        <span class="arrow"></span>

        <ul class="nav sub-menu">            
            <%foreach (var subCate in subCateList) %>
            <%{ %>

            <%if (subCate.CategoryName == "Golf") continue; %>
            <%if (subCate.ProductsCount == 0) continue; %>
            <%var subsubCateList = CategoryController.GetNextLevelSubCategories(subCate.CategoryID, WebConfig.CountryId);%>
            <%subsubCateList = subsubCateList.OrderBy(item => item.ListOrder).ToList(); %>

            <%if (subsubCateList.Count == 0) %>
            <%{ %>
                <li id="accordion-menu-item-<%=subCate.CategoryID %>" class="menu-item menu-item-type-custom menu-item-object-custom"><a href="<%=PriceMe.UrlController.GetCatalogUrl(subCate.CategoryID) %>"><%=subCate.CategoryName %></a></li>
            <%} %>
            <%else %>
            <%{ %>
                <li id="nav-menu-item-<%=subCate.CategoryID %>" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  sub" data-cols="1">
                    <a href="<%=PriceMe.UrlController.GetCatalogUrl(subCate.CategoryID) %>" class=""><span><%=subCate.CategoryName %></span></a>
                    <span class="arrow"></span>

                    <ul class="nav sub-menu">

                        <%foreach (var subsubCate in subsubCateList) %>
                        <%{ %>

                        <%if (subsubCate.CategoryName == "Cycling Apparel") continue; %>
                        <%if (subsubCate.ProductsCount == 0) continue; %>

                        <li id="nav-menu-item-<%=subsubCate.CategoryID %>" class="menu-item menu-item-type-post_type menu-item-object-product ">
                            <a href="<%=PriceMe.UrlController.GetCatalogUrl(subsubCate.CategoryID) %>" class=""><span><%=subsubCate.CategoryName %></span></a>
                        </li>
                        <%} %>
                    </ul>
                </li>
            <%} %>
            
            <%} %>
        </ul>

    </li>
    <%} %>

    <%} %>
</ul>



<%--            <div class="menu-wrap">
                <ul id="menu-mainmenu-1" class="mobile-menu accordion-menu">
                    <li id="accordion-menu-item-3214" class="menu-item menu-item-type-custom menu-item-object-custom current-menu-item current_page_item menu-item-home current-menu-ancestor current-menu-parent menu-item-has-children active has-sub">
                        <a href="/linea/" class=" current "><i class="fa fa-home"></i>Home</a>
                        <span class="arrow"></span>
                        <ul class="sub-menu">
                            <li id="accordion-menu-item-3216" class="menu-item menu-item-type-custom menu-item-object-custom current-menu-item current_page_item menu-item-home active"><a href="/linea/" class="">Home Layout 1</a></li>
                            <li id="accordion-menu-item-3217" class="menu-item menu-item-type-custom menu-item-object-custom "><a href="/linea2" class="">Home Layout 2</a></li>
                            <li id="accordion-menu-item-3218" class="menu-item menu-item-type-custom menu-item-object-custom "><a href="/linea3" class="">Home Layout 3</a></li>
                            <li id="accordion-menu-item-3219" class="menu-item menu-item-type-custom menu-item-object-custom "><a href="/linea4" class="">Home Layout 4</a></li>
                        </ul>
                    </li>
                    <li id="accordion-menu-item-3099" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                        <a href="/linea/product-category/dresses/" class=""><i class="fa fa-male"></i>Dresses</a>
                        <span class="arrow"></span>
                        <ul class="sub-menu">
                            <li id="accordion-menu-item-3100" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/dresses/clothing/" class="">Clothing</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3224" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/clothing/designer-wear/" class="">Designer Wear</a></li>
                                    <li id="accordion-menu-item-3225" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/clothing/ethnic-wear-clothing/" class="">Ethnic Wear</a></li>
                                    <li id="accordion-menu-item-3226" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/clothing/night-wear/" class="">Night Wear</a></li>
                                    <li id="accordion-menu-item-3227" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/clothing/western-wear/" class="">Western Wear</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3101" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/dresses/sportswear/" class="">Sportswear</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3228" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/sportswear/t-shirts/" class="">T Shirts</a></li>
                                    <li id="accordion-menu-item-3207" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/sportswear/skirts/" class="">Skirts</a></li>
                                    <li id="accordion-menu-item-3229" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/sportswear/jackets/" class="">Jackets</a></li>
                                    <li id="accordion-menu-item-3208" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/sportswear/shorts/" class="">Shorts</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3220" class="women-img menu-item menu-item-type-custom menu-item-object-custom "></li>
                            <li id="accordion-menu-item-3102" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/dresses/winter-wear/" class="">Winter Wear</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3195" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/winter-wear/shrugs/" class="">Shrugs</a></li>
                                    <li id="accordion-menu-item-3206" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/winter-wear/sweatshirts/" class="">Sweatshirts</a></li>
                                    <li id="accordion-menu-item-3230" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/winter-wear/thermals/" class="">Thermals</a></li>
                                    <li id="accordion-menu-item-3231" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/winter-wear/outerwear/" class="">Outerwear</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3103" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/dresses/swimwear/" class="">Swimwear</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3232" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/swimwear/swimsuits/" class="">Swimsuits</a></li>
                                    <li id="accordion-menu-item-3233" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/swimwear/beach-clothing/" class="">Beach Clothing</a></li>
                                    <li id="accordion-menu-item-3234" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/swimwear/bikinis/" class="">Bikinis</a></li>
                                    <li id="accordion-menu-item-3235" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/dresses/swimwear/clothing-swimwear/" class="">Clothing</a></li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                    <li id="accordion-menu-item-3196" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                        <a href="/linea/product-category/tops/" class=""><i class="fa fa-female"></i>Tops</a>
                        <span class="arrow"></span>
                        <ul class="sub-menu">
                            <li id="accordion-menu-item-3197" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/tops/tops-tunics/" class="">Tops &#038; Tunics</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3198" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/tops-tunics/polyester/" class="">Polyester</a></li>
                                    <li id="accordion-menu-item-3236" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/tops-tunics/poly-cotton/" class="">Poly Cotton</a></li>
                                    <li id="accordion-menu-item-3237" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/tops-tunics/poly-crepe/" class="">Poly Crepe</a></li>
                                    <li id="accordion-menu-item-3238" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/tops-tunics/poly-georgette/" class="">Poly Georgette</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3199" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/tops/t-shirts-top/" class="">T Shirts</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3239" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/t-shirts-top/embroidered/" class="">Embroidered</a></li>
                                    <li id="accordion-menu-item-3240" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/t-shirts-top/floral/" class="">Floral</a></li>
                                    <li id="accordion-menu-item-3241" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/t-shirts-top/stripes/" class="">Stripes</a></li>
                                    <li id="accordion-menu-item-3200" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/t-shirts-top/printed/" class="">Printed</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3222" class="men-img menu-item menu-item-type-custom menu-item-object-custom "></li>
                            <li id="accordion-menu-item-3201" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/tops/shirts/" class="">Shirts</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3202" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/shirts/chinese-collar/" class="">Chinese Collar</a></li>
                                    <li id="accordion-menu-item-3242" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/shirts/peterpan-collar/" class="">Peterpan Collar</a></li>
                                    <li id="accordion-menu-item-3243" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/shirts/regular-collar/" class="">Regular Collar</a></li>
                                    <li id="accordion-menu-item-3244" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/shirts/round-neck/" class="">Round Neck</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3203" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/tops/tees-polo/" class="">Tees &#038; Polo</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3246" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/tees-polo/halter-neck/" class="">Halter Neck</a></li>
                                    <li id="accordion-menu-item-3245" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/tees-polo/boat-neck/" class="">Boat Neck</a></li>
                                    <li id="accordion-menu-item-3247" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/tees-polo/hood/" class="">Hood</a></li>
                                    <li id="accordion-menu-item-3205" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/tops/tees-polo/peterpan/" class="">Peterpan</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3223" class="men-img menu-item menu-item-type-custom menu-item-object-custom "></li>
                        </ul>
                    </li>
                    <li id="accordion-menu-item-3212" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                        <a href="/linea/product-category/ethnic-wear/" class=""><i class="fa fa-cogs"></i>Ethnic Wear</a>
                        <span class="arrow"></span>
                        <ul class="sub-menu">
                            <li id="accordion-menu-item-3204" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/ethnic-wear/sarees/" class="">Sarees</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3210" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/sarees/silk-sarees/" class="">Silk sarees</a></li>
                                    <li id="accordion-menu-item-3248" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/sarees/cotton-sarees/" class="">Cotton sarees</a></li>
                                    <li id="accordion-menu-item-3249" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/sarees/designer-sarees/" class="">Designer sarees</a></li>
                                    <li id="accordion-menu-item-3250" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/sarees/printed-sarees/" class="">Printed sarees</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3209" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/ethnic-wear/kurtas/" class="">Kurtas</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3211" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/kurtas/34-sleeve/" class="">3/4 Sleeve</a></li>
                                    <li id="accordion-menu-item-3251" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/kurtas/half-sleeve/" class="">Half Sleeve</a></li>
                                    <li id="accordion-menu-item-3252" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/kurtas/long-sleeve/" class="">Long Sleeve</a></li>
                                    <li id="accordion-menu-item-3253" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/kurtas/sleeveless/" class="">Sleeveless</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3258" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/ethnic-wear/dress-material/" class="">Dress Material</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3254" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/dress-material/chiffon-dress/" class="">Chiffon</a></li>
                                    <li id="accordion-menu-item-3255" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/dress-material/cotton-dress/" class="">Cotton</a></li>
                                    <li id="accordion-menu-item-3256" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/dress-material/silk-dress/" class="">Silk</a></li>
                                    <li id="accordion-menu-item-3257" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/dress-material/synthetic-dress/" class="">Synthetic</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3269" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/ethnic-wear/stoles-dupattas/" class="">Stoles &#038; Dupattas</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3270" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/stoles-dupattas/wool-stoles/" class="">Wool Stoles</a></li>
                                    <li id="accordion-menu-item-3271" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/stoles-dupattas/art-silk/" class="">Art Silk</a></li>
                                    <li id="accordion-menu-item-3272" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/stoles-dupattas/chiffon/" class="">Chiffon</a></li>
                                    <li id="accordion-menu-item-3273" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/stoles-dupattas/cottonstoles/" class="">Cotton-Stoles</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3259" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/ethnic-wear/anarkalis/" class="">Anarkalis</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3260" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/anarkalis/cotton-anarkalis/" class="">Cotton Anarkalis</a></li>
                                    <li id="accordion-menu-item-3261" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/anarkalis/net-anarkalis/" class="">Net Anarkalis</a></li>
                                    <li id="accordion-menu-item-3262" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/anarkalis/silk-anarkalis/" class="">Silk Anarkalis</a></li>
                                    <li id="accordion-menu-item-3263" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/anarkalis/synthetic-anarkalis/" class="">Synthetic Anarkalis</a></li>
                                </ul>
                            </li>
                            <li id="accordion-menu-item-3264" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat menu-item-has-children  has-sub">
                                <a href="/linea/product-category/ethnic-wear/salwar-suit-sets/" class="">Salwar Suit Sets</a>
                                <span class="arrow"></span>
                                <ul class="sub-menu">
                                    <li id="accordion-menu-item-3265" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/salwar-suit-sets/embroidered-suit/" class="">Embroidered Suit</a></li>
                                    <li id="accordion-menu-item-3266" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/salwar-suit-sets/floral-suit/" class="">Floral Suit</a></li>
                                    <li id="accordion-menu-item-3267" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/salwar-suit-sets/printed-suit/" class="">Printed Suit</a></li>
                                    <li id="accordion-menu-item-3268" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/ethnic-wear/salwar-suit-sets/stripes-suit/" class="">Stripes Suit</a></li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                    <li id="accordion-menu-item-3215" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/partywear/" class=""><i class="fa fa-tag"></i>Partywear</a></li>
                    <li id="accordion-menu-item-3309" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/baby-kids/" class=""><i class="fa fa-child"></i>Baby &#038; Kids</a></li>
                    <li id="accordion-menu-item-3310" class="menu-item menu-item-type-taxonomy menu-item-object-product_cat "><a href="/linea/product-category/game-sport/" class=""><i class="fa fa-soccer-ball-o"></i>Game &#038; Sport</a></li>
                    <li id="accordion-menu-item-3285" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/blog/" class=""><i class="fa fa-rss"></i>Blog</a></li>
                    <li id="accordion-menu-item-3308" class="menu-item menu-item-type-custom menu-item-object-custom current-menu-item current_page_item menu-item-home menu-item-has-children active has-sub">
                        <a href="/linea/" class=" current "><i class="fa fa-file-text"></i>Pages</a>
                        <span class="arrow"></span>
                        <ul class="sub-menu">
                            <li id="accordion-menu-item-3302" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/blog/" class="">Blog</a></li>
                            <li id="accordion-menu-item-3307" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/page-markup-and-formatting/" class="">Typography</a></li>
                            <li id="accordion-menu-item-3303" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/full-width-page/" class="">Full Width Page</a></li>
                            <li id="accordion-menu-item-3305" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/page-left-sidbar/" class="">Page &#8211; Left Sidebar</a></li>
                            <li id="accordion-menu-item-3306" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/page-right-sidebar/" class="">Page &#8211; Right Sidebar</a></li>
                            <li id="accordion-menu-item-3304" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/page-3-column/" class="">Page &#8211; 3 Column</a></li>
                        </ul>
                    </li>
                    <li id="accordion-menu-item-3213" class="menu-item menu-item-type-custom menu-item-object-custom menu-item-has-children  has-sub">
                        <a href="/bolt4/shop/" class=""><i class="fa fa-star"></i>Products</a>
                        <span class="arrow"></span>
                        <ul class="sub-menu">
                            <li id="accordion-menu-item-3287" class="menu-item menu-item-type-post_type menu-item-object-product "><a href="/linea/product/riot-jeans-casual-roll-up-sleeve-printed-womens-top/" class="">Single Product</a></li>
                            <li id="accordion-menu-item-3286" class="menu-item menu-item-type-post_type menu-item-object-product "><a href="/linea/product/united-colors-of-benetton-womens-top-lime/" class="">Variable Product</a></li>
                            <li id="accordion-menu-item-3290" class="menu-item menu-item-type-post_type menu-item-object-product "><a href="/linea/product/today-fashion-casual-full-sleeve-solid-womens-top-black/" class="">Grouped Product</a></li>
                            <li id="accordion-menu-item-3288" class="menu-item menu-item-type-post_type menu-item-object-product "><a href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/" class="">External Product</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
            <div class="menu-wrap">
                <ul id="menu-toplinks-1" class="top-links1 accordion-menu">
                    <li id="accordion-menu-item-3311" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/my-account/" class="">My Account</a></li>
                    <li id="accordion-menu-item-3282" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/shop/" class="">Shop</a></li>
                    <li id="accordion-menu-item-3284" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/blog/" class="">Blog</a></li>
                    <li id="accordion-menu-item-3283" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/checkout/" class="">Checkout</a></li>
                    <li id="accordion-menu-item-3289" class="menu-item menu-item-type-post_type menu-item-object-page "><a href="/linea/wishlist-2-6/" class="">Wishlist</a></li>
                    <li class="menu-item"><a href="/linea/my-account/"><span>Login</span></a></li>
                    <li class="menu-item"><a href="/linea/my-account/"><span>Register</span></a></li>
                </ul>
            </div>--%>