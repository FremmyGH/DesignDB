SELECT 
FIO AS ���,
 COUNT (ID_Record) AS ������,
   SUM(Summa) AS �����
   FROM Photo   
   INNER JOIN Record 
   ON ID_Photo=Photo_ID 
   INNER JOIN Score 
   ON ID_Record=Record_ID 
   GROUP BY 
   FIO 
   ORDER BY ������ 
   DESC