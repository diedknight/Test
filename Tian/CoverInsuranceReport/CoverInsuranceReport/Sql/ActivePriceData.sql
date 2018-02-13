select 
productid as pid,
ROUND(min(RetailerPrice),0) as [min],
ROUND(avg(RetailerPrice),0) as [avg],
ROUND(max(RetailerPrice),0) as [max],
count(productid) as num 
from CSK_Store_RetailerProduct 
where ProductId in @Pids
and RetailerProductStatus =1 
and RetailerId in (
	select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and RetailerCountry = 3
) 
group by productid  
order by productid desc