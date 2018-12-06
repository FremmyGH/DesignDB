SELECT 
CAST(DateTimePP AS time) AS Время,
 COUNT(ID_Record) AS Посещения
  FROM Record 
  GROUP BY CAST(DateTimePP AS time) 
  ORDER BY Посещения DESC