SELECT 
FIO AS Имя,
 COUNT (ID_Record) AS Заказы,
   SUM(Summa) AS Доход
   FROM Photo   
   INNER JOIN Record 
   ON ID_Photo=Photo_ID 
   INNER JOIN Score 
   ON ID_Record=Record_ID 
   GROUP BY 
   FIO 
   ORDER BY Заказы 
   DESC