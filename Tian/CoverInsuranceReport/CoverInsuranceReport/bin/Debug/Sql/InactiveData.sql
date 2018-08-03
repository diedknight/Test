declare @tempRP table (
	productid int,
	RetailerProductStatus bit
)

insert into @tempRP(productid,RetailerProductStatus) 
(
	select Productid,RetailerProductStatus from CSK_Store_RetailerProduct where RetailerId in (
		select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and RetailerCountry = 3
	)	
)

insert into @tempRP(productid,RetailerProductStatus) 
(
	select Productid,RetailerProductStatus from PriceMe_D.dbo.Priceme_CSK_Store_RetailerProduct where RetailerId in (
		select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and RetailerCountry = 3
	) 	
)


select 
ProductID as pid,
categoryid as 'categoryname',
m.ManufacturerName as manufacturer,
replace(ProductName,m.ManufacturerName+' ','') as 'productfamily',
replace(ProductName,m.ManufacturerName+' ','') as 'series',
replace(ProductName,m.ManufacturerName+' ','') as 'model',
productname as 'productname',
0 as 'minprice',
0 as 'averageprice',
0 as 'maxprice',
0 as 'numberofprices',
DefaultImage as productImageURL,
'' as 'upcoming',
Convert(varchar(10),p.CreatedOn,111) as CreatedOn,
Convert(varchar(10),p.ModifiedOn,111) as UpdateOn,
'N' as 'Forsale' 
from CSK_Store_Product p 
inner join CSK_Store_Manufacturer m on p.ManufacturerID=m.ManufacturerID 
where ProductID in (select Productid from @tempRP where RetailerProductStatus = 0) 
and Productid not in (select Productid from @tempRP where RetailerProductStatus = 1) 
and CategoryID in @Cids 
and IsMerge = 1 
and p.ManufacturerID!=-1 
and p.modifiedon>'2015/1/1' 
and NZReplication=0 
order by ProductName