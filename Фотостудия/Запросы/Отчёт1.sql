SELECT 
LocationName,
 COUNT(ID_Record) AS [���������]
  FROM Location 
  INNER JOIN Record 
  ON ID_Location=Location_ID 
  GROUP BY Location.LocationName 
  ORDER BY [���������] DESC