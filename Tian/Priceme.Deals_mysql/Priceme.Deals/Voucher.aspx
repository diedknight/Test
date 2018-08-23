<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Voucher.aspx.cs" Inherits="Priceme.Deals.Voucher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/voucher.min.css?v=1.1.1" rel="stylesheet" />
    <link href="css/default.min.css?v=1.1.1" rel="stylesheet" />

    <link href="//deals.priceme.co.nz/voucher" rel="canonical" />

    <title>Vouchers, Coupons & Offers - PriceMe NZ Deals</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="voucher_page default_page">
        <div class="ctcTitleTd page-header">
            <h1 class="productInfo">Vouchers</h1>
            <div class="categoryDec">
                We collect  vouchers and coupons that help you save. Missing a voucher? <%--<a href="recommend_voucher">Recommend voucher!</a>--%>
            </div>
        </div>

        <div class="tab-holder">
            <ul class="nav nav-tabs simple">
                <li><a style="width: 140px; top: 6px;" href="/" onclick="javascript:document.location=this.href">Deals</a></li>
                <li class="active"><a style="width: 140px;">Vouchers</a></li>
            </ul>
        </div>

        <span class="glyphicon glyphicon-filter"></span>
        <h3 class="filterTitle">Filters</h3>
        <div id="filters">
            <div class="popular_category">
                <asp:Literal runat="server" ID="ltCategories"></asp:Literal>
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

                <%for (int i = 0; i < voucherList.Count; i++) %>
                <%{ %>
                <div style="height: 460px;" class="product-grid-item">

                    <%if (DateTime.Now > voucherList[i].ValidUntilDate) %>
                    <%{ %>
                    <div class="sale_info_right"><span>Expired</span></div>
                    <%} %>
                    <%else if (voucherList[i].ExpireSoon) %>
                    <%{ %>
                    <div class="sale_info_right_green"><span>Expires soon</span></div>
                    <%} %>

                    <a href="<%=voucherList[i].VoucherUrl %>">
                        <img src="<%=voucherList[i].Image %>" alt="<%=voucherList[i].VoucherName %>" title="<%=voucherList[i].VoucherName %>" onerror="onImgError(this)" />
                    </a>

                    <div class="middle-div">
                        <%--<a class="productNameLink" href="<%=voucherList[i].VoucherUrl %>"><%=voucherList[i].VoucherName %></a>--%>
                        <p style="text-align: center; height:54px; overflow:hidden;"><%=voucherList[i].VoucherName %> </p>

                        <br />
                        <h3 style="text-align: center; height: 18px;"><%=voucherList[i].Value %> </h3>
                        <br />

                        <p style="text-align: center; height: 36px; color: gray;">
                            <%if (voucherList[i].CouponCode != "") %>
                            <%{ %>
                            <span class="glyphicon glyphicon-bookmark iconGray"></span>Coupon code
                            
                            <br />
                            <span style="font-weight: bold;"><%=voucherList[i].CouponCode %></span>
                            <%} %>
                        </p>

                        <br />
                        <p style="text-align: center; height: 25px; line-height: 25px; color: gray; border-bottom: solid 1px gray; width: 150px; margin: 0 auto;">
                            <span class="glyphicon glyphicon-time iconGray"></span>Valid until <%=voucherList[i].ValidUntilDate.ToString("dd MMMM") %>
                        </p>

                        <p style="height: 35px; line-height: 35px; text-align: center; font-size: 15px; overflow: hidden;">
                            <%if (voucherList[i].RetailerLogo != "") %>
                            <%{ %>
                            <img alt="" src="<%=voucherList[i].RetailerLogo %>" style="height: 35px; width: 100px; margin: 0 auto; padding-top: 5px;" />
                            <%} %>
                            <%else %>
                            <%{ %>
                            <%=voucherList[i].StoreName %>
                            <%} %>
                        </p>

                        <br />
                    </div>

                    <div class="block_btn">
                        <a class="btn btnVisitShop btn-xs" href="<%=voucherList[i].VoucherUrl %>">Visit Shop</a>
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
            $("#sortBySelect").on("change", function () {

                var url = $(this).val();

                document.location.href = url;
            });

        })();
    </script>
</asp:Content>
