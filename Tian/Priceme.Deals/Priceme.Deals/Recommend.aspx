<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Recommend.aspx.cs" Inherits="Priceme.Deals.Recommend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="recommend_voucher" content="noindex" />

    <link href="css/voucher.min.css?v=1.1.1" rel="stylesheet" />
    <link href="css/default.min.css?v=1.1.1" rel="stylesheet" />

    <link href="//deals.priceme.co.nz/recommend" rel="canonical" />

    <title>Recommend Voucher & Deals</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="recommend_page voucher_page default_page">
        <div class="ctcTitleTd page-header">
            <h1 class="productInfo">Recommend Voucher & Deals</h1>
            <div class="categoryDec">
                Thank you for sharing a voucher or deal with the PriceMe community. The PriceMe team moderates all new offers before posted online.
            </div>
        </div>

        <dl class="rec_form">
            <dt>Name of voucher or deal:</dt>
            <dt>
                <asp:TextBox runat="server" ID="txtVoucherName" placeholder="Voucher name"></asp:TextBox><span id="voucherNameError" style="color: red; display: none;">*</span></dt>
            <dt>&nbsp;</dt>

            <dt>Voucher value in % or $:</dt>
            <dt>
                <asp:TextBox runat="server" ID="txtValue" placeholder="Value like 20% or $100"></asp:TextBox></dt>
            <dt>&nbsp;</dt>

            <dt>Voucher URL:</dt>
            <dt>
                <asp:TextBox runat="server" ID="txtVoucherUrl" placeholder="http"></asp:TextBox><span id="voucherUrlError" style="color: red; display: none;">*</span></dt>
            <dt>&nbsp;</dt>

            <dt>Coupon code (optional):</dt>
            <dt>
                <asp:TextBox runat="server" ID="txtCode"></asp:TextBox></dt>
            <dt>&nbsp;</dt>

            <dt>Your email address (optional):</dt>
            <dt>
                <asp:TextBox runat="server" ID="txtEmail" placeholder="Email Address"></asp:TextBox></dt>
            <dt>&nbsp;</dt>

            <dt>Comment (optional):</dt>
            <dt>
                <asp:TextBox runat="server" ID="txtDes" TextMode="MultiLine" CssClass="description"></asp:TextBox></dt>
            <dt>&nbsp;</dt>



            <%--<dt>Store name:</dt>
            <dt><asp:TextBox runat="server" ID="txtStoreName" placeholder="Store name" ></asp:TextBox></dt>
            <dt>&nbsp;</dt>--%>

            <%--<dt>Valid until (optional):</dt>
            <dt><asp:TextBox runat="server" ID="txtUntil" type="date"></asp:TextBox></dt>
            <dt>&nbsp;</dt>--%>

            <dt>
                <asp:Button runat="server" ID="btnSend" CssClass="btn btn-primary" Text="Send" OnClick="btnSend_Click" /></dt>
            <dt>&nbsp;</dt>

        </dl>

    </div>

    <script type="text/javascript">
        (function () {
            $("#<%=btnSend.ClientID %>").on("click", function () {
                var isOk = true;
                if ($("#<%=txtVoucherName.ClientID %>").val() == "") {
                    $("#voucherNameError").css("display", "inline");
                    isOk = false;
                }
                if ($("#<%=txtVoucherUrl.ClientID %>").val() == "") {
                    $("#voucherUrlError").css("display", "inline");
                    isOk = false;
                }

                return isOk;
            });            
        })();

        $(document).ready(function () {
            $("#ad").css("display", "none");
        });
    </script>
</asp:Content>
