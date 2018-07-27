select pid,NewPrice,CreatedOn from CSK_Store_RetailerProductHistory
where PID in @Pids
and RID in @Rids
union 
select pid,NewPrice,CreatedOn from PriceMe_D.dbo.EDW_CSK_Store_RetailerProductHistory
where PID in @Pids
and RID in @Rids