declare @tempRP table (
	productid int,
	RetailerPrice money
)

insert into @tempRP(productid,RetailerPrice) 
(
	select ProductId,RetailerPrice from CSK_Store_RetailerProduct
	where ProductId in @Pids
	and RetailerProductStatus =1 
	and RetailerId in (
		select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and RetailerCountry = 3
	) 
	and RetailerPrice<9999
)

insert into @tempRP(productid,RetailerPrice) 
(
	select ProductId,RetailerPrice from PriceMe_D.dbo.Priceme_CSK_Store_RetailerProduct
	where ProductId in @Pids
	and RetailerProductStatus =1 
	and RetailerId in (
		select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and RetailerCountry = 3
	) 
	and RetailerPrice<9999
)

select 
productid as pid,
ROUND(min(RetailerPrice),0) as [min],
ROUND(avg(RetailerPrice),0) as [avg],
ROUND(max(RetailerPrice),0) as [max],
count(productid) as num,
median=(
	select Median = AVG(1.0 * rpc)
	from(
		SELECT productid, o.RetailerPrice as rpc, rn=ROW_NUMBER() OVER (PARTITION BY productid ORDER BY o.RetailerPrice), c=COUNT(0)OVER(PARTITION BY productid)
		FROM @tempRP AS o
		where ProductId=a.ProductId
	) AS x
	WHERE rn IN ((c + 1)/2, (c + 2)/2)
	GROUP BY productid
)
from @tempRP as a
group by productid  
order by productid desc