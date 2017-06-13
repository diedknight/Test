<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewCatalogProducts.ascx.cs" Inherits="HotterWinds.Modules.Catalog.NewCatalogProducts" %>

<%@ Register Src="~/Modules/Catalog/NewGridView.ascx" TagName="NewGridView" TagPrefix="DK" %>
<%@ Register Src="~/Modules/Catalog/NewListView.ascx" TagName="NewListView" TagPrefix="DK" %>
<%@ Register Src="~/Modules/Catalog/QuickProductSummaryDisplay.ascx" TagName="QuickProductSummaryDisplay" TagPrefix="DK" %>
<%@ Register Src="~/Modules/Catalog/HWGridView.ascx" TagPrefix="DK" TagName="HWGridView" %>

<section>
    <div>
        <ul class="products-grid">
            <asp:Repeater ID="RepeaterGridHW" runat="server">
                <ItemTemplate>
                    <li class="item col-lg-3 col-md-3 col-sm-4 col-xs-6 post-106 product type-product status-publish has-post-thumbnail product_cat-anarkalis product_cat-chinese-collar product_cat-clothing product_cat-dress-material product_cat-dresses product_cat-embroidered-suit product_cat-ethnic-wear product_cat-floral-suit product_cat-kurtas product_cat-outerwear product_cat-peterpan-collar product_cat-printed-suit product_cat-regular-collar product_cat-round-neck product_cat-salwar-suit-sets product_cat-sarees product_cat-shirts product_cat-shrugs product_cat-sportswear product_cat-stoles-dupattas product_cat-stripes-suit product_cat-sweatshirts product_cat-swimwear product_cat-t-shirts-top product_cat-tees-polo product_cat-thermals product_cat-tops product_cat-tops-tunics product_cat-winter-wear notblog shipping-taxable purchasable product-type-simple product-cat-anarkalis product-cat-chinese-collar product-cat-clothing product-cat-dress-material product-cat-dresses product-cat-embroidered-suit product-cat-ethnic-wear product-cat-floral-suit product-cat-kurtas product-cat-outerwear product-cat-peterpan-collar product-cat-printed-suit product-cat-regular-collar product-cat-round-neck product-cat-salwar-suit-sets product-cat-sarees product-cat-shirts product-cat-shrugs product-cat-sportswear product-cat-stoles-dupattas product-cat-stripes-suit product-cat-sweatshirts product-cat-swimwear product-cat-t-shirts-top product-cat-tees-polo product-cat-thermals product-cat-tops product-cat-tops-tunics product-cat-winter-wear instock">
                        <DK:HWGridView runat="server" ID="HWGridView"
                            MaxPrice='<%#Eval("MaxPrice") %>'
                            BestPrice='<%#Eval("BestPrice") %>'
                            ProductRatingSum='<%#Eval("ProductRatingSum") %>'
                            ProductRatingVotes='<%#Eval("ProductRatingVotes") %>'
                            ProductName='<%#Eval("ProductName") %>'
                            DefaultImage='<%#Eval("DefaultImage") %>'
                            ProductID='<%#Eval("ProductID") %>'
                            PriceCount='<%#Eval("PriceCount") %>'
                            BestPPCRetailerName='<%#Eval("BestPPCRetailerName") %>'
                            CategoryID='<%#Eval("CategoryID") %>'
                            BestPPCRetailerProductID='<%#Eval("BestPPCRetailerProductID") %>'
                            CatalogDescription='<%#Eval("CatalogDescription")%>'
                            DisplayName='<%#Eval("DisplayName")%>'
                            BestPPCRetailerID='<%#Eval("BestPPCRetailerID")%>'
                            AvRating='<%#Eval("AvRating") %>'
                            IsAccessory='<%#Eval("IsAccessory")%>'
                            IsSearchProduct='<%#IsSearchProduct%>'
                            DayCount='<%#Eval("DayCount")%>'
                            RootCategoryID='<%#RootCategoryID%>'
                            ShowCompareBox="<%#ShowCompareBox %>"
                            IsUpComing='<%#Eval("IsUpComing")%>'
                            ClickOutUrl='<%#Eval("ClickOutUrl") %>'
                            ImageAlt='<%#Eval("ImageAlt") %>'
                            StarsImage='<%#Eval("StarsImage") %>'
                            StarsImageAlt='<%#Eval("StarsImageAlt") %>'
                            ComparePriceString='<%#Eval("ComparePriceString") %>'
                            IsSearchOnly='<%#Eval("IsSearchOnly") %>'
                            ReviewCount='<%#Eval("ReviewCount") %>'
                            AttrDescription='<%#Eval("AttrDescription") %>'
                            PrevPrice='<%#Eval("PrevPrice") %>'
                            Sale='<%#Eval("Sale") %>'
                            RatingPercent='<%#Eval("RatingPercent") %>'
                            CurrentPrice='<%#Eval("CurrentPrice") %>'
                            IsTop3='<%#Eval("IsTop3") %>' />
                    </li>
                </ItemTemplate>
            </asp:Repeater>

        </ul>
    </div>

</section>

<%--<section>
    <div id="catalogProductsDiv" class="catalogProductsDiv row">
        <asp:Repeater ID="RepeaterList" runat="server">
            <ItemTemplate>
                <DK:NewListView ID="NewListView1" runat="server"
                    MaxPrice='<%#Eval("MaxPrice") %>'
                    BestPrice='<%#Eval("BestPrice") %>'
                    ProductRatingSum='<%#Eval("ProductRatingSum") %>'
                    ProductRatingVotes='<%#Eval("ProductRatingVotes") %>'
                    ProductName='<%#Eval("ProductName") %>'
                    DefaultImage='<%#Eval("DefaultImage") %>'
                    ProductID='<%#Eval("ProductID") %>'
                    PriceCount='<%#Eval("PriceCount") %>'
                    BestPPCRetailerName='<%#Eval("BestPPCRetailerName") %>'
                    CategoryID='<%#Eval("CategoryID") %>'
                    BestPPCRetailerProductID='<%#Eval("BestPPCRetailerProductID") %>'
                    CatalogDescription='<%#Eval("CatalogDescription")%>'
                    DisplayName='<%#Eval("DisplayName")%>'
                    BestPPCRetailerID='<%#Eval("BestPPCRetailerID")%>'
                    AvRating='<%#Eval("AvRating") %>'
                    IsAccessory='<%#Eval("IsAccessory")%>'
                    IsSearchProduct='<%#IsSearchProduct%>'
                    DayCount='<%#Eval("DayCount")%>'
                    RootCategoryID='<%#RootCategoryID%>'
                    ShowCompareBox="<%#ShowCompareBox %>"
                    IsUpComing='<%#Eval("IsUpComing")%>'
                    ClickOutUrl='<%#Eval("ClickOutUrl") %>'
                    ImageAlt='<%#Eval("ImageAlt") %>'
                    StarsImage='<%#Eval("StarsImage") %>'
                    StarsImageAlt='<%#Eval("StarsImageAlt") %>'
                    ComparePriceString='<%#Eval("ComparePriceString") %>'
                    IsSearchOnly='<%#Eval("IsSearchOnly") %>'
                    ReviewCount='<%#Eval("ReviewCount") %>'
                    AttrDescription='<%#Eval("AttrDescription") %>'
                    PrevPrice='<%#Eval("PrevPrice") %>'
                    Sale='<%#Eval("Sale") %>'
                    RatingPercent='<%#Eval("RatingPercent") %>'
                    CurrentPrice='<%#Eval("CurrentPrice") %>'
                    IsTop3='<%#Eval("IsTop3") %>' />
            </ItemTemplate>
        </asp:Repeater>

        <%if (View.Equals("grid", StringComparison.InvariantCultureIgnoreCase))
            { %>
        <div id="product-grid-view">
            <asp:Repeater ID="RepeaterGrid" runat="server">
                <ItemTemplate>
                    <DK:NewGridView ID="NewGridView1" runat="server"
                        MaxPrice='<%#Eval("MaxPrice") %>'
                        BestPrice='<%#Eval("BestPrice") %>'
                        ProductRatingSum='<%#Eval("ProductRatingSum") %>'
                        ProductRatingVotes='<%#Eval("ProductRatingVotes") %>'
                        ProductName='<%#Eval("ProductName") %>'
                        DefaultImage='<%#Eval("DefaultImage") %>'
                        ProductID='<%#Eval("ProductID") %>'
                        PriceCount='<%#Eval("PriceCount") %>'
                        AvRating='<%#Eval("AvRating") %>'
                        BestPPCRetailerName='<%#Eval("BestPPCRetailerName") %>'
                        CategoryID='<%#Eval("CategoryID") %>'
                        BestPPCRetailerProductID='<%#Eval("BestPPCRetailerProductID") %>'
                        DisplayName='<%#Eval("DisplayName")%>'
                        BestPPCRetailerID='<%#Eval("BestPPCRetailerID")%>'
                        IsSearchProduct='<%#IsSearchProduct%>'
                        DayCount='<%#Eval("DayCount")%>'
                        IsUpComing='<%#Eval("IsUpComing")%>'
                        ClickOutUrl='<%#Eval("ClickOutUrl") %>'
                        ImageAlt='<%#Eval("ImageAlt") %>'
                        StarsImage='<%#Eval("StarsImage") %>'
                        StarsImageAlt='<%#Eval("StarsImageAlt") %>'
                        ComparePriceString='<%#Eval("ComparePriceString") %>'
                        IsSearchOnly='<%#Eval("IsSearchOnly") %>'
                        PrevPrice='<%#Eval("PrevPrice") %>'
                        Sale='<%#Eval("Sale") %>'
                        RatingPercent='<%#Eval("RatingPercent") %>'
                        CurrentPrice='<%#Eval("CurrentPrice") %>'
                        IsTop3='<%#Eval("IsTop3") %>' />
                </ItemTemplate>
            </asp:Repeater>
            <div class="clearAndHidden"></div>
        </div>
        <%} %>

        <DK:QuickProductSummaryDisplay runat="server" ID="QuickProductSummaryDisplay1" />
    </div>
</section>--%>
