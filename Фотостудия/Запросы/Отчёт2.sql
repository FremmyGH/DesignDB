SELECT 
CAST(DateTimePP AS time) AS �����,
 COUNT(ID_Record) AS ���������
  FROM Record 
  GROUP BY CAST(DateTimePP AS time) 
  ORDER BY ��������� DESC