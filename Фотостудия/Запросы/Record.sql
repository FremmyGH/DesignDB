SELECT [ID_Record]
      ,[DateRecord]
      ,[DateTimePP]
      ,[DateTimePF]
      ,[DateTimeUP]
      ,[DateTimeUF]
      ,[Client].[FIO] AS "Client"
      ,[LocationName]
      ,[Photo].[FIO] AS "Photogrpapher"
  FROM [dbo].[Record]
INNER JOIN [dbo].[Photo]
	ON [Photo_ID]=[ID_Photo]
INNER JOIN [dbo].[Location]
	ON [Location_ID]=[ID_Location]
INNER JOIN [dbo].[Client]
	ON [Client_ID]=[ID_Client]
	ORDER BY DateRecord

