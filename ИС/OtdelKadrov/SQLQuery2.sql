USE [OtdelKadrov]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_showDolznostQuery]
AS
	SELECT ID_Dolznost AS '��� ���������', Name AS '��������', Oklad AS '����� (���.)' FROM Dolznost
