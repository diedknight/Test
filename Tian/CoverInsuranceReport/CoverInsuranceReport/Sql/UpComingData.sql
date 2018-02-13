select 
ProductID as pid,
categoryid as 'categoryname',
SUBSTRING(ProductName,0,charindex(' ',productname,1)+1) as manufacturer,
SUBSTRING(ProductName,charindex(' ',productname,1)+1,len(ProductName)) as 'productfamily',
SUBSTRING(ProductName,charindex(' ',productname,1)+1,len(ProductName)) as 'series',
SUBSTRING(ProductName,charindex(' ',productname,1)+1,len(ProductName)) as 'model',
productname as 'productname',
0 as 'minprice',
0 as 'average price',
0 as 'max price',
0 as 'numberofprices',
DefaultImage as productImageURL,
'' as 'upcoming',
Convert(varchar(10),CreatedOn,111) as CreatedOn,
Convert(varchar(10),CreatedOn,111) as UpdateOn,
'N' as 'Forsale'  
from CSK_Store_Product 
where CategoryID in @Cids 
and ProductStatus = 1 
and IsMerge = 1 
and ProductId not in (
	select productid from CSK_Store_RetailerProduct where retailerid in (
		select retailerid from CSK_Store_Retailer where RetailerCountry = 3
	) 
	and RetailerProductStatus=1
) 
and productid in (select ProductID  from UpcomingProduct) 
order by ProductName 