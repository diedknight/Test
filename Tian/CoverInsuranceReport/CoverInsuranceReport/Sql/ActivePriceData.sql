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
		FROM CSK_Store_RetailerProduct AS o
		where ProductId=a.ProductId
		and RetailerProductStatus =1 
		and RetailerId in (
			select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and RetailerCountry = 3
		) 
		and RetailerPrice<10000
	) AS x
	WHERE rn IN ((c + 1)/2, (c + 2)/2)
	GROUP BY productid
)
from CSK_Store_RetailerProduct as a
where ProductId in @Pids
and RetailerProductStatus =1 
and RetailerId in (
	select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and RetailerCountry = 3
) 
and RetailerPrice<10000
group by productid  
order by productid desc