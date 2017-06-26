<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MDProductPriceCompareNomal.ascx.cs" Inherits="HotterWinds.Modules.Products.MDProductPriceCompareNomal" %>

<%@ Register Src="~/Modules/Products/MDProductPriceCompare.ascx" TagPrefix="uc1" TagName="ProductPriceCompare" %>


<asp:Repeater ID="dtProducts" runat="server">
    <ItemTemplate>        
            <uc1:ProductPriceCompare id="ProductPriceCompare1" runat="server" 
            RetailerId='<%#Eval("RetailerId") %>'
            ProductName='<%#Eval("RetailerProductName") %>'
            PPCMemberType='<%#Eval("PPCMemberType") %>'                         
            ProductID='<%#Eval("ProductID") %>'
            RetailerProductID='<%#Eval("RetailerProductId") %>' 
            PurchaseURL='<%#Eval("purchaseURL") %>'
            Freight='<%#Eval("Freight") %>'
            CCFeeAmount='<%#Eval("CCFeeAmount") %>'
            TotalPrice='<%#Eval("TotalPrice") %>'
            RetailerPrice='<%#Eval("RetailerPrice") %>'
            Stock='<%#Eval("Stock") %>'
            IsNoLink='<%#Eval("IsNoLink") %>'
            IsFeaturedProduct='<%#Eval("IsFeaturedProduct") %>'
            RetailerProductDescription = '<%#Eval("RetailerProductDescription") %>'
            categoryId = '<%#CategoryId %>'
            pricesCount = '<%#PricesCount %>'
            StockStatus = '<%#Eval("StockStatus") %>'
            OriginalPrice = '<%#Eval("OriginalPrice") %>'
            RPCondition = '<%#Eval("RetailerProductCondition") %>'
            PricePosition = '<%#Eval("PricePosition") %>'
            PricePositionCount = '<%#Eval("PricePositionCount") %>'
            Loc = '<%#Eval("Loc") %>'
            TradeMeSellerId='<%#Eval("TradeMeSellerId") %>'
            ExpirationDate='<%#Eval("ExpirationDate") %>'
            />  
    </ItemTemplate>
</asp:Repeater>