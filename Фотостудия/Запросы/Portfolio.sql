SELECT [ID_Portfolio]
      ,[link]
      ,[FIO]
  FROM [dbo].[Portfolio]
  INNER JOIN [dbo].[Client] 
  ON [Client_ID]=[ID_Client]
  ORDER BY FIO