SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[CSK_Store_12RMB_Index_GetProductsDescriptor_V2]
@countryID int
AS
SELECT CSK_Store_ProductDescriptor.ProductID,
CSK_Store_ProductDescriptor.AttributeValueID,
CSK_Store_ProductDescriptor.TypeID,
CSK_Store_Product.CategoryID

FROM CSK_Store_ProductDescriptor
INNER JOIN dbo.CSK_Store_Product
ON CSK_Store_Product.ProductID = dbo.CSK_Store_ProductDescriptor.ProductID
INNER JOIN CSK_Store_AttributeValue
on CSK_Store_AttributeValue.AttributeValueID = CSK_Store_ProductDescriptor.AttributeValueID

WHERE (CSK_Store_Product.ProductID IN
(SELECT Vaild_RetailerProduct.ProductId
FROM Vaild_RetailerProduct INNER JOIN CSK_Store_Retailer
ON Vaild_RetailerProduct.RetailerId = CSK_Store_Retailer.RetailerId
WHERE CSK_Store_Retailer.RetailerCountry = @countryID))

RETURN