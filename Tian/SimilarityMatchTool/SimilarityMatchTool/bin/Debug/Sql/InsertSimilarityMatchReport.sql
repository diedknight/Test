INSERT INTO [dbo].[SimilarityMatchReport]
           ([CID]
           ,[PID]
           ,[PName]
           ,[Price]
           ,[ToPID]
           ,[ToPName]
           ,[ToPrice]
           ,[Score]
           ,[CreatedBy]
           ,[Createdon])
     VALUES
           (@CID
           ,@PID
           ,@PName
           ,@Price
           ,@ToPID
           ,@ToPName
           ,@ToPrice
           ,@Score
           ,@CreatedBy
           ,GETDATE())


