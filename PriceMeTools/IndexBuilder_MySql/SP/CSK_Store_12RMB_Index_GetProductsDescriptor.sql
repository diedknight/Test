USE [Priceme_test]
GO
/****** Object:  StoredProcedure [dbo].[CSK_Store_12RMB_Index_GetProductsDescriptor]    Script Date: 2015/1/7 13:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CSK_Store_12RMB_Index_GetProductsDescriptor]
@countryID int
AS
SELECT dbo.CSK_Store_ProductDescriptor.ProductID,
dbo.CSK_Store_ProductDescriptor.AttributeValueID,
dbo.CSK_Store_ProductDescriptor.TypeID,

dbo.CSK_Store_Product.ManufacturerID,
dbo.CSK_Store_Product.ProductRatingSum,
dbo.CSK_Store_Product.ProductRatingVotes,
dbo.CSK_Store_Product.CreatedOn,

dbo.CSK_Store_Product_Category_Map.CategoryID,
dbo.CSK_Store_Category.ParentID,

dbo.GetCategoryRootID(dbo.CSK_Store_Category.CategoryID) AS CategoryRootID,
dbo.GetRetailerAmountByProductId(dbo.CSK_Store_ProductDescriptor.ProductID, @countryID) AS RetailerAmount,
dbo.GetBestPrice(dbo.CSK_Store_ProductDescriptor.ProductID, @countryID) AS BestPrice,
CSK_Store_Manufacturer.ManufacturerName

FROM dbo.CSK_Store_ProductDescriptor INNER JOIN dbo.CSK_Store_Product_Category_Map
ON dbo.CSK_Store_ProductDescriptor.ProductID = dbo.CSK_Store_Product_Category_Map.ProductID
INNER JOIN dbo.CSK_Store_Category
ON dbo.CSK_Store_Category.CategoryID = dbo.CSK_Store_Product_Category_Map.CategoryID
INNER JOIN dbo.CSK_Store_Product
ON dbo.CSK_Store_Product.ProductID = dbo.CSK_Store_ProductDescriptor.ProductID
inner join dbo.CSK_Store_Manufacturer on CSK_Store_Manufacturer.ManufacturerID = CSK_Store_Product.ManufacturerID

WHERE (CSK_Store_Product.ProductID IN
(SELECT CSK_Store_RetailerProduct.ProductId
FROM CSK_Store_RetailerProduct INNER JOIN CSK_Store_Retailer
ON CSK_Store_RetailerProduct.RetailerId = CSK_Store_Retailer.RetailerId
WHERE (CSK_Store_RetailerProduct.RetailerProductStatus = 1)

AND (CSK_Store_Retailer.RetailerStatus < 99) and CSK_Store_Retailer.RetailerCountry = @countryID))

RETURN