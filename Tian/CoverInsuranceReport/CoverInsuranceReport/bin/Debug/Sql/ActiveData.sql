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
'Y' as 'Forsale' 
from CSK_Store_Product p 
inner join CSK_Store_Manufacturer m on p.ManufacturerID=m.ManufacturerID  
where ProductID in (
	select Productid from CSK_Store_RetailerProduct where RetailerId in (
		select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and RetailerCountry = 3
	) 
	and RetailerProductStatus = 1
)
and CategoryID in @Cids
and ProductStatus = 1 
and IsMerge = 1 
and NZReplication=1 
order by ProductName 