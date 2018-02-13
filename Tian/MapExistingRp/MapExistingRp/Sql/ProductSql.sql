select ProductID,ProductName,CategoryID ,
Price=(select top 1 RetailerPrice from csk_store_retailerproduct as b where b.productid=a.ProductID)
from CSK_Store_Product as a
where ismerge=0 
and CategoryID in @CIds 
and productid in(
	select productid from csk_store_retailerproduct where retailerproductstatus=1 and isdeleted=0 and retailerid in(
		select retailerid from csk_store_retailer where retailerstatus=1
		)
)