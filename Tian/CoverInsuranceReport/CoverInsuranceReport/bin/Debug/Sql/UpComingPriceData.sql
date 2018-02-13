select 
ProductID as pid,
PriceNZ as price,
Convert(varchar(10),ReleaseDate,111) as Upcoming 
from UpcomingProduct 
where ProductID in @Pids 
order by productid desc