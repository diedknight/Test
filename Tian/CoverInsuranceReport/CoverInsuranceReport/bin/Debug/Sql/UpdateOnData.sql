select 
productid as pid,
max(Convert(varchar(10),Modifiedon,111)) as updatedon  
from CSK_Store_RetailerProduct 
where ProductId in @Pids 
group by productid 
order by productid desc