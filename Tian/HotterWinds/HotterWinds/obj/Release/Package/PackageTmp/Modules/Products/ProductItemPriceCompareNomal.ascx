<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductItemPriceCompareNomal.ascx.cs" Inherits="HotterWinds.Modules.Products.ProductItemPriceCompareNomal" %>

<%@ Register Src="~/Modules/Products/ProductItemPriceCompare.ascx" TagPrefix="uc1" TagName="ProductItemPriceCompare" %>


<asp:Repeater ID="dtProducts" runat="server">
    <ItemTemplate>        
            <uc1:ProductItemPriceCompare id="ProductItemPriceCompare1" runat="server" 
            RetailerId='<%#Eval("RetailerId") %>'
            rps='<%#Eval("RpList") %>'
            categoryId = '<%#CategoryId %>'
            pricesCount = '<%#PricesCount %>'
            isSinglePrice = '<%#isSinglePrice %>'
            />  
    </ItemTemplate>
</asp:Repeater>