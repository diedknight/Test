<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Priceme.Deals.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/default.min.css?v=1.1.1" rel="stylesheet" />

    <link href="//deals.priceme.co.nz" rel="canonical" />

    <title>Deals - Latest Price Drops in NZ</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="default_page">
        <div class="ctcTitleTd page-header">
            <h1 class="productInfo">Deals - VIP Deals & Insiders Hidden Price Drops</h1>
            <div class="categoryDec">
                Find the best deals in New Zealand with PriceMe Deals. We track price changes in the market and collate the best deals and discount for you based on this information. Missing a deal? Let us know!
            </div>
        </div>

        <div class="tab-holder">
            <ul class="nav nav-tabs simple">
                <li class="active"><a style="width:140px;">Deals</a></li>
                <li class=""><a style="width:140px; top:6px;" href="voucher" onclick="javascript:document.location=this.href">Vouchers</a></li>
            </ul>
        </div>

        <span class="glyphicon glyphicon-filter"></span>
        <h3 class="filterTitle">Filters</h3>
        <div id="filters">
            <div class="popular_category">
                <asp:Literal runat="server" ID="ltCategories"></asp:Literal>
            </div>

            <div class="input-group inputBox search" id="home-searchbar2" style="position: relative; margin-left: 0px;">
                <input class="form-control home-search-textbox homeBG" type="search" id="SearchTextBox2" placeholder="Search for categories" autocomplete="off" />
                <div class="drop">
                    <ul id="categoryDrop" class="list">
                        <%--<%for (int i = 0; i < allCategories.Count; i++) %>
                    <%{ %>
                    <li value="<%=allCategories[i].CategoryID %>"><%=allCategories[i].CategoryName %></li>
                    <%} %>--%>
                    </ul>
                </div>
            </div>

        </div>

        <div class="products">
            <div id="catalogBar">
                <div id="sortByDiv" class="sortByDiv">
                    <label title="Sort :"><span class="glyphicon glyphicon-sort"></span>&nbsp;</label>
                    <select id="sortBySelect">
                        <asp:Literal ID="ltSortByItems" runat="server"></asp:Literal>
                    </select>
                </div>
            </div>

            <div id="product-grid-view">

                <%for (int i = 0; i < productList.Count; i++) %>
                <%{ %>
                <div class="product-grid-item">
                    <div class="sale_info"><span><%=Convert.ToInt32(Math.Abs(productList[i].Sale*100)) %>% Off</span></div>

                    <a href="<%=productList[i].ProductUrl %>">
                        <img src="<%=productList[i].DefaultImage %>" alt="<%=productList[i].ProductName %>" title="<%=productList[i].ProductName %>" onerror="onImgError(this)" />
                    </a>

                    <div class="middle-div">
                        <a class="productNameLink" href="<%=productList[i].ProductUrl %>"><%=productList[i].ProductName %></a>

                        <div class="bestPPCDiv">
                            <div class="priceCountDiv"><%=productList[i].PriceCount %> <span class="glyphicon glyphicon-shopping-cart"></span></div>
                            <div class="old_price"><span class="priceSymbol">$</span><span class="priceSpan"><%=string.Format("{0:n}",Convert.ToDecimal(productList[i].PrevPrice)) %></span></div>
                            <div class="new_price"><span class="priceSymbol">$</span><span class="priceSpan"><%=string.Format("{0:n}",Convert.ToDecimal(productList[i].BestPrice)) %></span></div>
                        </div>
                    </div>

                    <div class="retailer_logo">
                        <%if (PriceMeCommon.BusinessLogic.RetailerController.IsPPcRetailer(int.Parse(productList[i].BestPPCRetailerID), PriceMe.WebConfig.CountryId)) %>
                        <%{ %>
                        <img alt="<%=productList[i].BestPPCRetailerName %>" src="<%=productList[i].BestPPCLogoPath %>" />
                        <%}%>
                        <%else%>
                        <%{%>
                        <label><%=GetRetailerName(int.Parse(productList[i].BestPPCRetailerID)) %></label>
                        <%} %>
                    </div>
                </div>
                <%} %>
            </div>

            <div id="pagerDiv">
                <asp:Literal ID="ltPagination" runat="server"></asp:Literal>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        (function () {
            var searchTextNode = $("#SearchTextBox2");
            var dropNode = $("#home-searchbar2 .drop");


            searchTextNode.on("keyup", function () {
                var str = searchTextNode.val().toLowerCase();

                if (str == "") {
                    dropNode.css("display", "none");
                    return;
                }

                GlobalAjax("AjaxController", "LoadCategory", { input: str }, function (msg) {

                    dropNode.find("#categoryDrop").html(msg);

                    dropNode.find("li").each(function () {
                        var node = $(this);
                        //node.on("click", function () { document.location.href = "/default.aspx?s_cid=" + node.attr("value"); });
                        node.on("click", function () { document.location.href = node.attr("value"); });
                    });

                    if (msg != "") dropNode.css("display", "block");
                    else dropNode.css("display", "none");
                });

            });

            //searchTextNode.on("keyup", function () {
            //    var str = searchTextNode.val().toLowerCase();

            //    if (str == "") {
            //        dropNode.css("display", "none");
            //        return;
            //    }

            //    var nodes = dropNode.find("li")

            //    nodes.css("display", "none");
            //    nodes = nodes.filter(function () {
            //        var text = $(this).text().toLocaleLowerCase();

            //        for (var i = 0; i < str.length; i++) {
            //            if (text.length <= i) return false;
            //            if (text[i] != str[i]) return false;
            //        }

            //        return true;
            //    });

            //    nodes.css("display", "block");

            //    if (nodes.length > 0) dropNode.css("display", "block");
            //    else dropNode.css("display", "none");
            //});

            //searchTextNode.on("blur", function () {
            //    dropNode.css("display", "none");
            //});

            //dropNode.find("li").each(function () {
            //    var node = $(this);
            //    node.on("click", function () { document.location.href = "/default.aspx?s_cid=" + node.attr("value"); });
            //});


            $("#sortBySelect").on("change", function () {

                var url = $(this).val();

                document.location.href = url;
            });

        })();

    </script>

</asp:Content>
