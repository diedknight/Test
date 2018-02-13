select 
ProductID as pid,
DefaultImage  
from CSK_Store_Product 
where ProductID in @Pids 
order by ProductID desc