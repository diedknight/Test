select pid,NewPrice,CreatedOn from CSK_Store_RetailerProductHistory
where PID in @Pids
and RID in @Rids