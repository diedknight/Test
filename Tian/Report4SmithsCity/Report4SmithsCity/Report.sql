declare @RportRId int set @RportRId=1540

declare @CompareRId1 int set @RportRId=833
declare @CompareRId2 int set @RportRId=51
declare @CompareRId3 int set @RportRId=746
declare @CompareRId4 int set @RportRId=189
declare @CompareRId5 int set @RportRId=69
declare @CompareRId6 int set @RportRId=830
declare @CompareRId7 int set @RportRId=1885
declare @CompareRId8 int set @RportRId=298

declare @report table(
	ProductId int,
	RetailerPrice money,
	RetailerId int,
	RetailerProductSKU nvarchar(max),
	ProductName nvarchar(max),
	CategoryId int,
	CategoryName nvarchar(max)
)

insert @report(ProductId,RetailerPrice,RetailerId,RetailerProductSKU,ProductName,CategoryId,CategoryName)
(
	select *,
	CategoryName=(select CategoryName from CSK_Store_Category where CSK_Store_Category.CategoryID=b.CategoryId)
	from (
		select *,
		ProductName=(select ProductName from CSK_Store_Product as b where b.ProductId=a.ProductId),
		CategoryId=(select CategoryID from CSK_Store_Product as b where b.ProductId=a.ProductId)
		from (
			select ProductId,RetailerPrice,RetailerId,RetailerProductSKU 
			from CSK_Store_RetailerProduct 
			where productid in (select ProductId from csk_store_retailerproduct where RetailerId=@RportRId) and RetailerProductStatus = 1
			and IsDeleted = 0 and RetailerPrice > 0.2
			and RetailerId in (select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and IsSetupComplete = 1 and  RetailerCountry = 3)
			and ProductId in (select ProductId from CSK_Store_Product where CategoryID in (select CategoryID from CSK_Store_Category where IsDisplayIsMerged = 1 and IsActive = 1))
			union
			select ProductId,RetailerPrice,RetailerId,RetailerProductSKU 
			from CSK_Store_RetailerProduct 
			where productid in (select ProductId from csk_store_retailerproduct where RetailerId=@RportRId) and RetailerProductStatus = 1 and IsDeleted = 0 and RetailerPrice > 0.2
			and RetailerId in (select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and IsSetupComplete = 1 and  RetailerCountry = 3)
			and ProductId in (select ProductId from CSK_Store_Product where CategoryID in (select CategoryID from CSK_Store_Category where IsDisplayIsMerged = 0 and IsActive = 1) and IsMerge = 1)
		) as a
	) as b
)

select 
ProductId,
RetailerPrice=min(RetailerPrice),
RetailerId=min(RetailerId),
RetailerProductSKU=min(RetailerProductSKU),
ProductName=min(ProductName),
CategoryId=min(CategoryId),
CategoryName=min(CategoryName),
Lowest=isnull((select min(RetailerPrice) from @report as b where b.ProductId=a.ProductId),0),
Price1=isnull((select min(RetailerPrice) from @report as b where b.ProductId=a.ProductId and b.RetailerId=@CompareRId1),0),
Price2=isnull((select min(RetailerPrice) from @report as b where b.ProductId=a.ProductId and b.RetailerId=@CompareRId2),0),
Price3=isnull((select min(RetailerPrice) from @report as b where b.ProductId=a.ProductId and b.RetailerId=@CompareRId3),0),
Price4=isnull((select min(RetailerPrice) from @report as b where b.ProductId=a.ProductId and b.RetailerId=@CompareRId4),0),
Price5=isnull((select min(RetailerPrice) from @report as b where b.ProductId=a.ProductId and b.RetailerId=@CompareRId5),0),
Price6=isnull((select min(RetailerPrice) from @report as b where b.ProductId=a.ProductId and b.RetailerId=@CompareRId6),0),
Price7=isnull((select min(RetailerPrice) from @report as b where b.ProductId=a.ProductId and b.RetailerId=@CompareRId7),0),
Price8=isnull((select min(RetailerPrice) from @report as b where b.ProductId=a.ProductId and b.RetailerId=@CompareRId8),0)
from @report as a where RetailerId=@RportRId
group by ProductId
