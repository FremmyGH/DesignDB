SELECT [ID_Student],[FIO] AS "���",[Phone] AS "�������",[NumSB] AS "����� �������������",[DateVidSB] AS "���� ������ �������������",[NumRB] AS "����� �������������",[DateVidRB] AS "���� ������ �������������",[Group].Name AS "������" FROM [dbo].[Student] INNER JOIN [dbo].[Group] ON ID_Group=Group_ID Order by FIO
