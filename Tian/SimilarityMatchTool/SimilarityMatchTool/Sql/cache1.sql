select P.ProductID,ProductName,CategoryID,ManufacturerID,AVG(RP.RetailerPrice) as Price from CSK_Store_Product P inner join csk_store_retailerproduct RP
on P.ProductID=RP.ProductId where ManufacturerID>0 and ismerge=0 and CategoryID in @CIds and RP.isdeleted=0 and RP.RetailerProductStatus=1 and retailerid in
(select retailerid from csk_store_retailer where retailerstatus=1 and RetailerCountry=3)
group by P.ProductID,ProductName,CategoryID,ManufacturerID
order by ManufacturerID