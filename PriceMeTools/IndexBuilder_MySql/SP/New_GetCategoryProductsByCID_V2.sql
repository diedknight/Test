SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[New_GetCategoryProductsByCID_V2]
@cid int,
@countryID int,
@websiteID int = 0,
@onlyPPC bit = 0
AS

IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects WHERE id = OBJECT_ID(N'tempdb..#tempproductinfo') AND type in (N'U'))
DROP TABLE #tempproductinfo

IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects WHERE id = OBJECT_ID(N'tempdb..#productvotessumt') AND type in (N'U'))
DROP TABLE #productvotessumt

IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects WHERE id = OBJECT_ID(N'tempdb..#productexpertrating') AND type in (N'U'))
DROP TABLE #productexpertrating

create table #tempproductinfo
(
ProductId int primary key clustered,
BestPPCRetailerID int,
BestPPCRetailerName nvarchar(100),
PriceCount int,
RetailerCount int,
BestPrice money,
BestPPCPrice money,
MaxPrice money,
BestPPCRetailerProductID int,
BestPPCLogo nvarchar(500),
BestPricePPCIndex decimal(18,2),
RetailerProductList text,
CategoryID int,
RPS bit
)

CREATE TABLE #productvotessumt
(
[Id] [int] NOT NULL,
[ProductID] [int] NOT NULL,
[ProductRatingSum] [int] NOT NULL,
[ProductRatingVotes] [int] NOT NULL
)

CREATE TABLE #productexpertrating
(
[ProductID] [int] NOT NULL,
[AverageRating] [float] NOT NULL,
[Votes] [int] NOT NULL,
[VotesHasScore] [int] NOT NULL,
[UserVotes] [int] NOT NULL,
[UserAverageRating] [float] NOT NULL
)

if @countryID = 3
begin
insert into #productvotessumt
select * from CSK_Store_ProductVotesSumNZ
end
else if @countryID = 1
begin
insert into #productvotessumt
select * from CSK_Store_ProductVotesSumAU
end
else if @countryID = 28
begin
insert into #productvotessumt
select * from CSK_Store_ProductVotesSumPH
end
else if @countryID = 36
begin
insert into #productvotessumt
select * from CSK_Store_ProductVotesSumSG
end
else if @countryID = 41
begin
insert into #productvotessumt
select * from CSK_Store_ProductVotesSumHK
end
else if @countryID = 45
begin
insert into #productvotessumt
select * from CSK_Store_ProductVotesSumMY
end
else if @countryID = 55
begin
insert into #productvotessumt
select * from CSK_Store_ProductVotesSumTH
end
else if @countryID = 56
begin
insert into #productvotessumt
select * from CSK_Store_ProductVotesSumVN
end
else if @countryID = 51
begin
insert into #productvotessumt
select * from CSK_Store_ProductVotesSumID
end


insert into #productexpertrating([ProductID],[AverageRating],[Votes],[VotesHasScore], [UserVotes],[UserAverageRating])
select [ProductID],[AverageRating],[Votes],[VotesHasScore], [UserVotes], [UserAverageRating] from PriceMe_ExpertAverageRatingTF


insert #tempproductinfo
select * from dbo.New_ProductInfo(@countryID, @websiteID, @onlyPPC, @cid)


SELECT CSK_Store_Product.ProductID,
CSK_Store_Product.ProductGuid,
CSK_Store_Product.SKU,
CSK_Store_Product.DefaultImage,
CSK_Store_Product.ShortDescriptionZH,
CSK_Store_Product.ManufacturerID,
#productvotessumt.ProductRatingSum,
#productvotessumt.ProductRatingVotes,
CSK_Store_Product.IsMerge,
CSK_Store_Category.ParentID,
CSK_Store_Category.IsAccessories,
CSK_Store_Category.IsActive,
CSK_Store_Category.CategoryID,
CSK_Store_Product.keywords,
CSK_Store_Product.MPN,
CSK_Store_Product.UPC,
CSK_Store_Manufacturer.ManufacturerName,
#tempproductinfo.BestPrice,
#tempproductinfo.BestPPCPrice,
#tempproductinfo.MaxPrice,
#tempproductinfo.BestPPCRetailerName,
#tempproductinfo.BestPPCRetailerID,
#tempproductinfo.BestPPCRetailerProductID,
CSK_Store_Product.ProductName, CSK_Store_Product.ManufacturerPartNumber,
#tempproductinfo.PriceCount,
#tempproductinfo.RetailerCount,
#tempproductinfo.RPS,
dbo.ProductClickTemp.clicks,
0 as PrevPrice,
[dbo].[New_GetRetailerProductList](#tempproductinfo.ProductId, @countryID, @websiteID, @onlyPPC) as RetailerProductList,
#tempproductinfo.BestPricePPCIndex,
#tempproductinfo.BestPPCLogo,
CSK_Store_Product.IsReNamed,
CSK_Store_Product.CatalogDescription,
CSK_Store_Category.IsDisplayIsMerged,
#productexpertrating.AverageRating,
#productexpertrating.VotesHasScore,
#productexpertrating.Votes,
#productexpertrating.UserVotes,
#productexpertrating.UserAverageRating,
CSK_Store_Product.Width,
CSK_Store_Product.Weight,
CSK_Store_Product.Height,
CSK_Store_Product.UnitOfLength,
CSK_Store_Product.UnitOfMeasure
FROM CSK_Store_Product
inner join #tempproductinfo on CSK_Store_Product.productid = #tempproductinfo.productid
INNER JOIN CSK_Store_Category ON #tempproductinfo.CategoryID = CSK_Store_Category.CategoryID
left join dbo.ProductClickTemp on dbo.ProductClickTemp.productid = CSK_Store_Product.ProductID and dbo.ProductClickTemp.CountryID = @countryID
left join #productvotessumt on #productvotessumt.ProductID = CSK_Store_Product.ProductID
left join #productexpertrating on #productexpertrating.ProductID = CSK_Store_Product.ProductID
inner join CSK_Store_Manufacturer on CSK_Store_Manufacturer.ManufacturerID = CSK_Store_Product.ManufacturerID
where CSK_Store_Category.CategoryID  not in (1771, 2585)
order by dbo.ProductClickTemp.clicks desc