﻿-- ============ TestMachineMonitoring DB ============
GO -- === STATION_INFORMATION TABLE ===
SELECT TOP 1000 * FROM STATION_INFORMATION
WHERE PRODUCT_NAME LIKE '%172%' --AND MACHINE_NAME LIKE '%L02%FT%'
ORDER BY MACHINE_NAME ASC

GO -- === ENERGY_RECORD ===

SELECT * FROM ENERGY_RECORD
WHERE DATE >= CONVERT(DATE, GETDATE()) AND STANDBY_TIME > 0
 --AND TIME_CHECK <= '2024-03-14 23:59:00' 002197006979

SELECT DATE AS WorkDate, COUNT(HOST_NAME) AS TotalMachine, SUM(ACTIVE_TIME) AS TotalActive, SUM(STANDBY_TIME) AS TotalIdle FROM ENERGY_RECORD 
WHERE DATE >= CONVERT(DATE, GETDATE()) -- AND STANDBY_TIME > 0
GROUP BY DATE

SELECT DATE AS WorkDate, COUNT(HOST_NAME) AS TotalMachine, SUM(ACTIVE_TIME) AS TotalActive, SUM(STANDBY_TIME) AS TotalIdle FROM ENERGY_RECORD
WHERE DATE >= CONVERT(DATE, GETDATE() - 7) AND DATE <= CONVERT(DATE, GETDATE()) 
GROUP BY DATE ORDER BY DATE DESC

SELECT DATE AS WorkDate, COUNT(HOST_NAME) AS TotalMachine, SUM(ACTIVE_TIME) AS TotalActive, SUM(STANDBY_TIME) AS TotalIdle FROM ENERGY_RECORD
WHERE DATE >= CONVERT(DATE, GETDATE() - 7) AND DATE <= CONVERT(DATE, GETDATE()) AND STANDBY_TIME > 0
GROUP BY DATE ORDER BY DATE DESC

GO -- === MACHINE_INFORMATION ===

SELECT COUNT(*) FROM MACHINE_INFORMATION 
WHERE TIME_CHECK >= CONVERT(DATE, GETDATE())

SELECT CONVERT(DATE, GETDATE())
SELECT DATEADD(DD,0,DATEDIFF(DD,0,GETDATE()))

SELECT * FROM MACHINE_INFORMATION
WHERE TOOL_VERSION  LIKE '%%' AND HOST_NAME LIKE '%L07C181RC001%' AND TIME_CHECK >= CONVERT(DATE, GETDATE()) -- AND SAVE_ENERGY_MODE LIKE'%AIR%' --   AND IP LIKE '%41.59%'
ORDER BY HOST_NAME DESC

SELECT * FROM MACHINE_INFORMATION WHERE SAVE_ENERGY_MODE LIKE '%;AIR;%'

GO -- === AIR_INFORMATION ===
SELECT * FROM AIR_INFORMATION

GO -- === MACHINE_INFORMATION_CHANGE ===

SELECT * FROM MACHINE_INFORMATION_CHANGE_RECORD

GO -- === ISSUE RECORD ===
SELECT DISTINCT * FROM ISSUE_RECORD
WHERE HOST_NAME LIKE '%BT004%'
ORDER BY DETECT_TIME DESC

SELECT * FROM ISSUE_RECORD
WHERE DETECT_TIME >= CONVERT(DATE, GETDATE())

GO -- === FACE_RECOGNIZATION ===
SELECT * FROM FACE_RECOGNITION_DATA
WHERE TIME >= CONVERT(DATE, GETDATE())
ORDER BY TIME DESC

SELECT CARD_ID, NAME, COUNT(TIME) AS ACTION_TIME FROM FACE_RECOGNITION_DATA
WHERE MACHINE_NAME LIKE '%L01%' AND
	  TIME >= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME),' 07:30:00')) AND 
	  TIME <= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME),' 18:30:00'))
GROUP BY CARD_ID, NAME
ORDER BY COUNT(TIME) DESC

SELECT TOP 1 CARD_ID, NAME, COUNT(TIME) AS ACTION_TIME FROM FACE_RECOGNITION_DATA
WHERE MACHINE_NAME LIKE '%L01%'
	AND CARD_ID IN (SELECT CARD_ID FROM EMPLOYEES WHERE ACTIVE = 1)AND
	  (TIME >= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME),' 19:30:00')) AND 
	  TIME <= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME),' 23:59:00'))) OR
	  (TIME >= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME + 1),' 00:00:00')) AND 
	  TIME <= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME + 1),' 06:30:00')))
GROUP BY CARD_ID, NAME
ORDER BY COUNT(TIME) DESC

SELECT  * FROM FACE_RECOGNITION_DATA
WHERE MACHINE_NAME LIKE '%L05C172RC108%'AND CARD_ID IN (SELECT CARD_ID FROM EMPLOYEES WHERE ACTIVE = 1) -- TIME >= CONVERT(DATE, GETDATE())
ORDER BY TIME DESC


SELECT CONVERT(DATETIME,CONCAT(CONVERT(DATE, GETDATE()+1),' 07:30:00'))
SELECT CONVERT(DATETIME,CONCAT(CONVERT(DATE, GETDATE()),' 19:30:00'))

SELECT CONCAT('L',CAST(SUBSTRING(MACHINE_NAME,11,2) AS INT)) AS LINE,MACHINE_NAME, CARD_ID, NAME 
FROM FACE_RECOGNITION_DATA 
WHERE CONVERT(DATE,TIME) = CONVERT(DATE,GETDATE()-7)
AND CARD_ID IN (SELECT CARD_ID FROM EMPLOYEES WHERE ACTIVE = 1)
GROUP BY CONCAT('L',CAST(SUBSTRING(MACHINE_NAME,11,2) AS INT)), MACHINE_NAME, CARD_ID, NAME