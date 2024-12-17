
-- ========= FATP_TABLE =========
GO -- === Trigger Instead INSERT INTO FATP_TABLE ===
ALTER TRIGGER trgFATPInsteadInsert -- ALTER | CREATE | DROP
ON dbo.FATP_TABLE
INSTEAD OF INSERT 
AS
BEGIN
-- DECLARE INDENTIFIER
	DECLARE @fatpID int;
	DECLARE @atePC nvarchar(50);
	DECLARE @ateIP nvarchar(50);
	DECLARE @ateMAC nvarchar(50); 
	SELECT @fatpID = i.ID from inserted i; 
	SELECT @atePC = i.ATE_PC from inserted i; 
	SELECT @ateIP = i.ATE_IP from inserted i; 
	SELECT @ateMAC = i.ATE_MAC from inserted i;	
-- DECLARE UPDATER
	DECLARE @line nvarchar(50);
	DECLARE @station nvarchar(50);
	DECLARE @model nvarchar(50);
	DECLARE @postDate datetime;
	DECLARE @passNum int;
	DECLARE @failNum int;
	DECLARE @errorList nvarchar(max);
	DECLARE @counters nvarchar(max);
	DECLARE @equipments nvarchar(max);
	SELECT @line = i.LINE from inserted i;
	SELECT @station = i.STATION from inserted i;
	SELECT @model = i.MODEL from inserted i;	
	SELECT @postDate = i.POST_DATE from inserted i;	
	SELECT @passNum = i.PASS_NUM from inserted i;
	SELECT @failNum = i.FAIL_NUM from inserted i;
	SELECT @errorList = i.ERROR_LIST from inserted i;	
	SELECT @counters = i.COUNTERS from inserted i;	
	SELECT @equipments = i.EQUIPMENTS from inserted i;	
-- TRIGGER ACTION
	IF EXISTS (SELECT ID FROM FATP_TABLE WHERE ATE_PC = @atePC AND ATE_IP = @ateIP AND ATE_MAC = @ateMAC)
		BEGIN
			UPDATE dbo.FATP_TABLE 
			SET LINE = @line, STATION = @station, MODEL = @model, POST_DATE = GETDATE(), PASS_NUM = @passNum, FAIL_NUM = @failNum, FAIL_NUM_BUFFER = @failNum,
			ERROR_LIST = @errorList, COUNTERS = @counters, EQUIPMENTS = @equipments, PC_DATE = @postDate 
			WHERE ATE_PC = @atePC AND ATE_IP = @ateIP AND ATE_MAC = @ateMAC
		END
	ELSE
		BEGIN
			SET IDENTITY_INSERT dbo.FATP_TABLE OFF
			INSERT INTO dbo.FATP_TABLE(ATE_PC, ATE_IP, ATE_MAC, LINE, STATION, MODEL, POST_DATE, PASS_NUM, FAIL_NUM, FAIL_NUM_BUFFER, ERROR_LIST, COUNTERS, EQUIPMENTS, PC_DATE)
			VALUES(@atePC, @ateIP, @ateMAC, @line, @station, @model, GETDATE(), @passNum, @failNum, @failNum, @errorList, @counters, @equipments, @postDate)
		END	
END
GO -- Procedure Reset FAIL_NUM
ALTER PROCEDURE proResetFAIL_NUM(
	@atePC nvarchar(50),
	@ateIP nvarchar(50),
	@model nvarchar(50)
)
AS
BEGIN
	declare @originFailNum int
	declare @bufferFailNum int
	declare @isReset int = 0
	select @originFailNum = FAIL_NUM from FATP_TABLE WHERE ATE_PC LIKE @atePC AND ATE_IP LIKE @ateIP AND MODEL LIKE @model
	select @bufferFailNum = FAIL_NUM_BUFFER from FATP_TABLE WHERE ATE_PC LIKE @atePC AND ATE_IP LIKE @ateIP AND MODEL LIKE @model
	
	IF @originFailNum != 0 AND @bufferFailNum = 0
		BEGIN			
			SET @isReset = 1;
		END	
	
	SELECT @isReset AS RESET_FAIL;
END;
GO
EXEC proResetFAIL_NUM @ATEPC = 'L01C153RC007', @ateIP = '200.166.44.174', @model = 'U10C153.08'
SELECT * FROM FATP_TABLE WHERE ATE_PC = 'L01C153RC007' AND ATE_IP = '200.166.44.174' AND MODEL LIKE '%%'
UPDATE FATP_TABLE SET MODEL = 'U10C153.08' WHERE ATE_PC = 'L01C153RC007' AND ATE_IP = '200.166.44.174'
GO -- ====== Procedure SET CPK RESULT By MODEL And ATE_PC ======
CREATE PROCEDURE proFATPSetCPKResult(
	@atePC nvarchar(50),
	@model nvarchar(50),
	@cpkResult nvarchar(max),
	@cpkLowestItem nvarchar(500)
)
AS
BEGIN
	UPDATE FATP_TABLE SET CPK_RESULTS = @cpkResult, CPK_LOWEST_ITEM = @cpkLowestItem
	WHERE ATE_PC = @atePC AND MODEL = @model
END;
GO
EXEC proFATPSetCPKResult @ATEPC = @atePC, @MODEL = @model, @CPKRESULT = @cpkResult, @CPKLOWESTITEM = @cpkLowest
GO
GO -- ====== Procedure GET CPK BY ATE_PC and MODEL ======
ALTER PROCEDURE proFATPGetCPKResult(
	@atePC nvarchar(50),
	@model nvarchar(50),
	@fullOrlowest bit
)
AS
BEGIN
	-- Local variables
	DECLARE @currentDate datetime 
	SELECT @currentDate = DATEADD(DD,0,DATEDIFF(DD,0,GETDATE()))
	-- Procedure Action 
	IF(@fullOrlowest = 1)
		BEGIN
			SELECT TOP 1 CPK_LOWEST_ITEM FROM FATP_TABLE WHERE ATE_PC = @atePC AND MODEL = @model AND POST_DATE >= @currentDate 
		END
	ELSE
		BEGIN
			SELECT TOP 1 CPK_RESULTS FROM FATP_TABLE WHERE ATE_PC = @atePC AND MODEL = @model AND POST_DATE >= @currentDate
		END
END;
GO -- ====== Procedure GET ABNORMAL STATUS BY ATE_PC and MODEL ======
-- CREATE | ALTER | DROP
CREATE PROCEDURE proFATPAbnormalCheckStatus(
	@atePC nvarchar(50),
	@ateIP nvarchar(50),
	@model nvarchar(50),
	@station nvarchar(50),
	@pcDate datetime
)
AS
BEGIN
	-- Local variables
	DECLARE @currentDate datetime 
	SELECT @currentDate = DATEADD(DD,0,DATEDIFF(DD,0,GETDATE()))
	DECLARE @line nvarchar(50)
	SELECT @line = SUBSTRING(@atePC,0,4)
	PRINT CONCAT('Line of FATP: ',@line)	
	-- Check duplicate ATE_PC and MODEL 
	DECLARE @isPCDuplicate int = 0
	SELECT @isPCDuplicate = COUNT(ATE_PC) FROM FATP_TABLE WHERE ATE_PC LIKE @atePC AND MODEL LIKE @model AND POST_DATE >= @currentDate AND DATEDIFF(MINUTE, POST_DATE, GETDATE()) < 5
	
	--IF EXISTS (SELECT ID FROM FATP_TABLE WHERE ATE_PC LIKE @atePC AND MODEL LIKE @model AND DATEDIFF(MINUTE, POST_DATE, GETDATE()) < 30)
		--BEGIN
		--IF NOT EXISTS (SELECT ID FROM FATP_TABLE WHERE ATE_PC LIKE @atePC AND MODEL LIKE @model AND ATE_IP LIKE @ateIP)
			--SET @isPCDuplicate = 1;
		--END

	-- Check time on server and on line of FATP is in sync
	DECLARE @isTimeInSync int = 0
	DECLARE @timeRange int
	SELECT @timeRange = DATEDIFF(MINUTE,@pcDate,GETDATE())
	PRINT CONCAT('Time range of FATP: ',@timeRange)	
	IF(@timeRange > 60 OR @timeRange < -60)
		BEGIN
			SET @isTimeInSync = 1
		END

	-- Check ATE_PC is in order
	DECLARE @isPCInOrder int = 0
	DECLARE @stationModelPCNum int
	SELECT @stationModelPCNum = COUNT (DISTINCT ATE_PC) FROM FATP_TABLE WHERE LINE = @line AND MODEL = @model AND STATION = @station
	DECLARE @currentPCPos int = CAST(SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT)
	DECLARE @previousPC nvarchar(50) = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), CAST( SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) - 1)
	DECLARE @followingPC nvarchar(50) = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), CAST( SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) + 1)
	IF(@currentPCPos >= 0 AND @currentPCPos < 10)
		BEGIN
			SET @previousPC = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), '0',CAST(SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) - 1)
			SET @followingPC = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), '0',CAST(SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) + 1)
		END
	IF(@currentPCPos <= 0)
		BEGIN
			SET @isPCInOrder = 1
		END
	IF(@stationModelPCNum = 0 AND @currentPCPos != 1)
		BEGIN
			SET @isPCInOrder = 1
		END
	IF(@stationModelPCNum > 0 AND @stationModelPCNum <= @currentPCPos)
		BEGIN
			IF NOT EXISTS (SELECT ATE_PC FROM FATP_TABLE WHERE ATE_PC LIKE @previousPC AND MODEL = @model)
			SET @isPCInOrder = 1
		END
	IF(@stationModelPCNum > 0 AND @stationModelPCNum > @currentPCPos AND @currentPCPos != 1)
		BEGIN
			IF NOT EXISTS (SELECT ATE_PC FROM FATP_TABLE WHERE ATE_PC LIKE @previousPC AND MODEL = @model) OR
			   NOT EXISTS (SELECT ATE_PC FROM FATP_TABLE WHERE ATE_PC LIKE @followingPC AND MODEL = @model)
			SET @isPCInOrder = 1
		END

	-- To be added...

	-- Set all status to result table
	DECLARE @TableStatus TABLE(IS_DUPLICATE INT, IS_INORDER INT, IS_TIMESYNC INT)
	INSERT INTO @TableStatus VALUES(@isPCDuplicate, @isPCInOrder, @isTimeInSync)
	SELECT * FROM @TableStatus	
END

GO -- ====== Procedures to handle OP_ID Monitoring ======
GO -- === proFATP_InsertOPRecord ===
ALTER PROCEDURE proFATP_InsertOPRecord(
	@ATE_PC NVARCHAR(20),
	@ATE_IP NVARCHAR(20),
	@ATE_MAC NVARCHAR(20),
	@OP_ID NVARCHAR(8)
)
AS
BEGIN
	-- INSERT/UPDATE OP_ID
	UPDATE FATP_TABLE SET OP_ID = @OP_ID WHERE ATE_PC LIKE @ATE_PC AND ATE_IP LIKE @ATE_IP AND ATE_MAC LIKE @ATE_MAC
	-- INSERT/UPDATE OP_RECORD
	--DECLARE @ate nvarchar(20) = 'L01C153FT002' -- L01C153FT002
	DECLARE @currOPRecord nvarchar(max)
	DECLARE @newOPRecord nvarchar(max)
	SELECT @currOPRecord = OP_RECORD FROM FATP_TABLE WHERE ATE_PC LIKE @ATE_PC AND ATE_IP LIKE @ATE_IP AND ATE_MAC LIKE @ATE_MAC
	IF @currOPRecord IS NULL 
		BEGIN
			set @newOPRecord = concat(@OP_ID,',',getdate())			
			PRINT(@newOPRecord)
		END
	IF @currOPRecord IS NOT NULL 
		BEGIN
			set @newOPRecord = concat(@currOPRecord, '|',@OP_ID,',',getdate())			
			PRINT(@newOPRecord)
		END
	UPDATE FATP_TABLE SET OP_RECORD = @newOPRecord WHERE ATE_PC LIKE @ATE_PC AND ATE_IP = @ATE_IP AND ATE_MAC = @ATE_MAC
	PRINT('INSERTED OP_RECORD!')
END

GO -- === procFATP_GET_TABLE_OP_RECORD ===
ALTER PROCEDURE proFATP_GET_TABLE_OP_RECORD(
	@ATE_PC NVARCHAR(20),
	@ATE_IP NVARCHAR(20),
	@ATE_MAC NVARCHAR(20),
	@OP_ID NVARCHAR(8)
)
AS
BEGIN

	DECLARE @opRecordFull nvarchar(max)
	SELECT @opRecordFull = OP_RECORD FROM FATP_TABLE WHERE ATE_PC LIKE @ATE_PC AND ATE_IP = @ATE_IP AND ATE_MAC = @ATE_MAC
	DECLARE @tbl_OpRecordFull TABLE(idx int identity(1,1), OpRecord nvarchar(50))
	INSERT INTO @tbl_OpRecordFull SELECT * FROM string_split(@opRecordFull,'|')
	DECLARE @iterStep int
	DECLARE @iterMax int	
	SELECT @iterStep = MIN(IDX), @iterMax = MAX(IDX) FROM @tbl_OpRecordFull
	
	--SELECT * FROM @tbl_OPRecord
	DECLARE @tbl_OpRecordEach TABLE(IDX int identity(1,1), OP_ID nvarchar(50), DATE_TIME DATETIME)
	WHILE @iterStep <= @iterMax
		BEGIN	
			--PRINT(CONCAT('========= LOOP: ',@iterStep ,' ========='))
			DECLARE @currOPRecord nvarchar(50)
			SELECT @currOPRecord = OpRecord FROM @tbl_OpRecordFull WHERE IDX = @iterStep
			--SET @currOPRecord =@currOPRecord
			--print(@currOPRecord)
			--SELECT CHARINDEX(',',@currOPRecord)
			-- === GET OP_ID FROM ===
			DECLARE @currOPRecord_OPID NVARCHAR(50)
			SET @currOPRecord_OPID = SUBSTRING(@currOPRecord,0,CHARINDEX(',',@currOPRecord))
			--PRINT (@currOPRecord_OPID)
			-- === GET DATE_TIME ===
			DECLARE @currOPRecord_DATETIME DATETIME
			SET @currOPRecord_DATETIME = CAST(SUBSTRING(@currOPRecord,CHARINDEX(',',@currOPRecord)+1, LEN(@currOPRecord)) AS DATETIME)  --  CONVERT(nvarchar,SUBSTRING(@currOPRecord,CHARINDEX(',',@currOPRecord)+1, LEN(@currOPRecord)),103)
			--PRINT (@currOPRecord_DATETIME)	
			
			INSERT INTO @tbl_OpRecordEach(OP_ID,DATE_TIME) VALUES(@currOPRecord_OPID, @currOPRecord_DATETIME)
			SET @iterStep = @iterStep + 1
		END;
	SELECT IDX, OP_ID, DATE_TIME  FROM @tbl_OpRecordEach

END
GO

GO -- ====== TEST: Procedure GET ABNORMAL STATUS BY ATE_PC and MODEL ======
-- CREATE | ALTER | DROP
ALTER PROCEDURE proFATPAbnormalCheckStatus_TEST(
	@atePC nvarchar(50),
	@ateIP nvarchar(50),
	@atemac nvarchar(50),
	@model nvarchar(50),
	@station nvarchar(50),
	@pcDate datetime,
	@opid nvarchar(8)
)
AS
BEGIN
	-- === Result variables ===
	DECLARE @isPCDuplicate int = 0	
	DECLARE @isTimeInSync int = 0
	DECLARE @isPCInOrder int = 0
	DECLARE @isOPAcceptable int = 0
	-- === Local variables ===
	-- Local variables
	DECLARE @currentDate datetime 
	SELECT @currentDate = DATEADD(DD,0,DATEDIFF(DD,0,GETDATE()))
	DECLARE @line nvarchar(50)
	SELECT @line = SUBSTRING(@atePC,0,4)
	--PRINT CONCAT('Line of FATP: ',@line)	
	-- Check duplicate ATE_PC and MODEL 
	--DECLARE @isPCDuplicate int = 0
	SELECT @isPCDuplicate = COUNT(ATE_PC) FROM FATP_TABLE WHERE ATE_PC LIKE @atePC AND MODEL LIKE @model AND POST_DATE >= @currentDate AND DATEDIFF(MINUTE, POST_DATE, GETDATE()) < 5
	
	--IF EXISTS (SELECT ID FROM FATP_TABLE WHERE ATE_PC LIKE @atePC AND MODEL LIKE @model AND DATEDIFF(MINUTE, POST_DATE, GETDATE()) < 30)
		--BEGIN
		--IF NOT EXISTS (SELECT ID FROM FATP_TABLE WHERE ATE_PC LIKE @atePC AND MODEL LIKE @model AND ATE_IP LIKE @ateIP)
			--SET @isPCDuplicate = 1;
		--END

	-- Check time on server and on line of FATP is in sync
	--DECLARE @isTimeInSync int = 0
	DECLARE @timeRange int
	SELECT @timeRange = DATEDIFF(MINUTE,@pcDate,GETDATE())
	--PRINT CONCAT('Time range of FATP: ',@timeRange)	
	IF(@timeRange > 60 OR @timeRange < -60)
		BEGIN
			SET @isTimeInSync = 1
		END

	-- Check ATE_PC is in order
	--DECLARE @isPCInOrder int = 0
	DECLARE @stationModelPCNum int
	SELECT @stationModelPCNum = COUNT (DISTINCT ATE_PC) FROM FATP_TABLE WHERE LINE = @line AND MODEL = @model AND STATION = @station
	DECLARE @currentPCPos int = CAST(SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT)
	DECLARE @previousPC nvarchar(50) = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), CAST( SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) - 1)
	DECLARE @followingPC nvarchar(50) = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), CAST( SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) + 1)
	IF(@currentPCPos >= 0 AND @currentPCPos < 10)
		BEGIN
			SET @previousPC = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), '0',CAST(SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) - 1)
			SET @followingPC = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), '0',CAST(SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) + 1)
		END
	IF(@currentPCPos <= 0)
		BEGIN
			SET @isPCInOrder = 1
		END
	IF(@stationModelPCNum = 0 AND @currentPCPos != 1)
		BEGIN
			SET @isPCInOrder = 1
		END
	IF(@stationModelPCNum > 0 AND @stationModelPCNum <= @currentPCPos)
		BEGIN
			IF NOT EXISTS (SELECT ATE_PC FROM FATP_TABLE WHERE ATE_PC LIKE @previousPC AND MODEL = @model)
			SET @isPCInOrder = 1
		END
	IF(@stationModelPCNum > 0 AND @stationModelPCNum > @currentPCPos AND @currentPCPos != 1)
		BEGIN
			IF NOT EXISTS (SELECT ATE_PC FROM FATP_TABLE WHERE ATE_PC LIKE @previousPC AND MODEL = @model) OR
			   NOT EXISTS (SELECT ATE_PC FROM FATP_TABLE WHERE ATE_PC LIKE @followingPC AND MODEL = @model)
			SET @isPCInOrder = 1
		END

	-- === Check OP_ID is acceptable or not ===
	--SELECT * FROM @tbl_OPRecord | 'L02H175FT014' | '200.166.44.31' | '50-3E-AA-10-DD-2B' | 'V0908666'
	--SELECT len(OP_RECORD) FROM FATP_TABLE WHERE ATE_PC = 'L03G178FT204' AND ATE_IP = '200.166.49.34' AND ATE_MAC = 'E0-D5-5E-8F-F1-41'
	DECLARE @tbl_OPRecord TABLE(IDX INT, OP_ID nvarchar(50), DATE_TIME DATETIME)
	INSERT @tbl_OPRecord EXEC proFATP_GET_TABLE_OP_RECORD @ATE_PC = @ATEPC, @ate_IP = @ATEIP, @ate_MAC = @ATEMAC, @op_id = ''

	IF((SELECT OP_RECORD FROM FATP_TABLE WHERE ATE_PC = @atePC AND ATE_IP = @ateIP AND ATE_MAC = @atemac) IS NULL) OR
		(SELECT LEN(OP_RECORD) FROM FATP_TABLE WHERE ATE_PC = @atePC AND ATE_IP = @ateIP AND ATE_MAC = @atemac) = 0
		BEGIN
			-- SET @isOPAcceptable = 1
			EXEC proFATP_InsertOPRecord @ATE_PC = @atePC, @ATE_IP = @ateIP, @ATE_MAC = @atemac, @OP_ID = @opid
		END
	ELSE
		
		BEGIN
		-- GET LIST RECORDS OF OPERATORS
		
		-- CHECK OP_ID IS IDENTICAL OR NOT BETWEEN 2 SHIFTS
		DECLARE @YEAR INT = YEAR(GETDATE())
		DECLARE @MONTH INT = MONTH(GETDATE())
		DECLARE @DAY INT = DAY(GETDATE())
		DECLARE @DayShiftStart DATETIME = CAST(CONCAT(@YEAR,'-',@MONTH,'-',@DAY,' 07:30:00') AS DATETIME)
		DECLARE @DayShiftEnd DATETIME = CAST(CONCAT(@YEAR,'-',@MONTH,'-',@DAY,' 18:30:00') AS DATETIME)
		DECLARE @LastNightShiftStart DATETIME = CAST(CONCAT(@YEAR,'-',@MONTH,'-',@DAY-1,' 19:30:00') AS DATETIME)
		DECLARE @NextNightShiftEnd DATETIME
		BEGIN TRY
			SET @NextNightShiftEnd = CAST(CONCAT(@YEAR,'-',@MONTH,'-',@DAY+1,' 07:30:00') AS DATETIME)	
		END TRY
		BEGIN CATCH
			SET @NextNightShiftEnd = CAST(CONCAT(@YEAR,'-',@MONTH+1,'-01',' 07:30:00') AS DATETIME)	
		END CATCH								
			IF(GETDATE() >= @DayShiftStart AND GETDATE() <= @DayShiftEnd)
				BEGIN
					IF(SELECT COUNT(IDX) FROM @tbl_OPRecord WHERE OP_ID = @opid AND DATE_TIME >= @LastNightShiftStart AND DATE_TIME <= @DayShiftStart) > 0
						BEGIN							
							SET @isOPAcceptable = 1
						END
				END			
			IF(GETDATE() >= @DayShiftEnd AND GETDATE() <= @NextNightShiftEnd)
				BEGIN
					IF(SELECT COUNT(IDX) FROM @tbl_OPRecord WHERE OP_ID = @opid AND DATE_TIME >= @DayShiftStart AND DATE_TIME <= @DayShiftEnd) > 0
						BEGIN							
							SET @isOPAcceptable = 1
						END
				END	
		END
	-- IF NOTHING HAPPENDS UPDATE OP_RECORD
	IF (SELECT TOP 1 OP_ID FROM @tbl_OPRecord ORDER BY DATE_TIME DESC) != @opid
		BEGIN
			IF(@isOPAcceptable = 0)
				BEGIN								
					EXEC proFATP_InsertOPRecord @ATE_PC = @atePC, @ATE_IP = @ateIP, @ATE_MAC = @atemac, @OP_ID = @opid				
				END
		END

	
	-- To be added...

	-- === Set all status to result table ===
	-- PRINT('FINISHING!')
	DECLARE @TableStatus TABLE(IS_DUPLICATE INT, IS_INORDER INT, IS_TIMESYNC INT, IS_OPACCEPTABLE INT)
	INSERT INTO @TableStatus VALUES(@isPCDuplicate, @isPCInOrder, @isTimeInSync, @isOPAcceptable)
	SELECT IS_DUPLICATE, IS_INORDER,IS_TIMESYNC, IS_OPACCEPTABLE FROM @TableStatus	
END

GO
UPDATE FATP_TABLE SET OP_ID = 'V0908660', OP_RECORD = 'V0908660,2024-06-06 14:56:52|V0908661,2024-06-06 21:56:52|V0908662,2024-06-06 21:56:52|V0908663,2024-06-07 14:56:52|V0908664,2024-06-07 21:56:52|V0908665,2024-06-07 14:56:52|V0908666,2024-06-19 14:56:52|V0908667,2024-06-19 21:56:52|V0908669,2024-06-20 14:56:52'
WHERE ATE_PC = 'L02H175FT014' AND ATE_IP = '200.166.44.31' AND ATE_MAC = '50-3E-AA-10-DD-2B'

EXEC proFATPAbnormalCheckStatus_TEST 'L09G182PT105','200.166.41.57','40-8D-5C-DC-BF-B2','U10G182.00','PT1','2024-07- 07:43:28','V1093107'

--INSERT INTO FATP_TABLE(ATE_PC,ATE_IP,ATE_MAC,LINE,STATION,MODEL,POST_DATE)
-- 49.34

EXEC proFATPAbnormalCheckStatus 'L09G182PT208','200.166.49.34','U10G182.00','FT2','2024-06-20 15:32:31'

EXEC proFATPAbnormalCheckStatus_TEST 'L08G182PT103','200.166.41.103','AC-15-A2-14-19-D3','U10G182.00','PT1','2024-07-03 08:21:32','V0904880'

SELECT * FROM FATP_TABLE WHERE MODEL LIKE '%182%' -- ATE_PC = 'L03G178FT204' MODEL LIKE '%182%'

GO
DECLARE @tbl_OPRecord TABLE(IDX INT, OP_ID nvarchar(50), DATE_TIME DATETIME)
INSERT @tbl_OPRecord EXEC proFATP_GET_TABLE_OP_RECORD @ATE_PC = 'L03G178FT204', @ate_IP = '200.166.49.34', @ate_MAC = 'E0-D5-5E-8F-F1-41', @op_id = ''
IF (SELECT TOP 1 OP_ID FROM @tbl_OPRecord ORDER BY DATE_TIME DESC) = 'V0952342'
	BEGIN 
		PRINT('MATCH!')
	END
ELSE
	BEGIN
		PRINT('NOT MATCH!')
	END
GO

ALTER PROCEDURE proFATPAbnormalCheckStatus_TEST_1(
	@atePC nvarchar(50),
	@ateIP nvarchar(50),
	@atemac nvarchar(50),
	@model nvarchar(50),
	@station nvarchar(50),
	@pcDate datetime,
	@opid nvarchar(8)
)
AS
BEGIN
	-- === Result variables ===
	DECLARE @isPCDuplicate int = 0	
	DECLARE @isTimeInSync int = 0
	DECLARE @isPCInOrder int = 0
	DECLARE @isOPAcceptable int = 0
	-- === Local variables ===
	-- Local variables
	DECLARE @currentDate datetime 
	SELECT @currentDate = DATEADD(DD,0,DATEDIFF(DD,0,GETDATE()))
	DECLARE @line nvarchar(50)
	SELECT @line = SUBSTRING(@atePC,0,4)
	--PRINT CONCAT('Line of FATP: ',@line)	
	-- Check duplicate ATE_PC and MODEL 
	--DECLARE @isPCDuplicate int = 0
	SELECT @isPCDuplicate = COUNT(ATE_PC) FROM FATP_TABLE WHERE ATE_PC LIKE @atePC AND MODEL LIKE @model AND POST_DATE >= @currentDate AND DATEDIFF(MINUTE, POST_DATE, GETDATE()) < 5
	
	--IF EXISTS (SELECT ID FROM FATP_TABLE WHERE ATE_PC LIKE @atePC AND MODEL LIKE @model AND DATEDIFF(MINUTE, POST_DATE, GETDATE()) < 30)
		--BEGIN
		--IF NOT EXISTS (SELECT ID FROM FATP_TABLE WHERE ATE_PC LIKE @atePC AND MODEL LIKE @model AND ATE_IP LIKE @ateIP)
			--SET @isPCDuplicate = 1;
		--END

	-- Check time on server and on line of FATP is in sync
	--DECLARE @isTimeInSync int = 0
	DECLARE @timeRange int
	SELECT @timeRange = DATEDIFF(MINUTE,@pcDate,GETDATE())
	--PRINT CONCAT('Time range of FATP: ',@timeRange)	
	IF(@timeRange > 60 OR @timeRange < -60)
		BEGIN
			SET @isTimeInSync = 1
		END

	-- Check ATE_PC is in order
	--DECLARE @isPCInOrder int = 0
	DECLARE @stationModelPCNum int
	SELECT @stationModelPCNum = COUNT (DISTINCT ATE_PC) FROM FATP_TABLE WHERE LINE = @line AND MODEL = @model AND STATION = @station
	DECLARE @currentPCPos int = CAST(SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT)
	DECLARE @previousPC nvarchar(50) = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), CAST( SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) - 1)
	DECLARE @followingPC nvarchar(50) = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), CAST( SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) + 1)
	IF(@currentPCPos >= 0 AND @currentPCPos < 10)
		BEGIN
			SET @previousPC = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), '0',CAST(SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) - 1)
			SET @followingPC = CONCAT(SUBSTRING(@atepc,0,LEN(@atepc)-1), '0',CAST(SUBSTRING(@atepc,LEN(@atepc)-1,2) AS INT) + 1)
		END
	IF(@currentPCPos <= 0)
		BEGIN
			SET @isPCInOrder = 1
		END
	IF(@stationModelPCNum = 0 AND @currentPCPos != 1)
		BEGIN
			SET @isPCInOrder = 1
		END
	IF(@stationModelPCNum > 0 AND @stationModelPCNum <= @currentPCPos)
		BEGIN
			IF NOT EXISTS (SELECT ATE_PC FROM FATP_TABLE WHERE ATE_PC LIKE @previousPC AND MODEL = @model)
			SET @isPCInOrder = 1
		END
	IF(@stationModelPCNum > 0 AND @stationModelPCNum > @currentPCPos AND @currentPCPos != 1)
		BEGIN
			IF NOT EXISTS (SELECT ATE_PC FROM FATP_TABLE WHERE ATE_PC LIKE @previousPC AND MODEL = @model) OR
			   NOT EXISTS (SELECT ATE_PC FROM FATP_TABLE WHERE ATE_PC LIKE @followingPC AND MODEL = @model)
			SET @isPCInOrder = 1
		END

	-- === Check OP_ID is acceptable or not ===
	--SELECT * FROM @tbl_OPRecord | 'L02H175FT014' | '200.166.44.31' | '50-3E-AA-10-DD-2B' | 'V0908666'
	--SELECT len(OP_RECORD) FROM FATP_TABLE WHERE ATE_PC = 'L03G178FT204' AND ATE_IP = '200.166.49.34' AND ATE_MAC = 'E0-D5-5E-8F-F1-41'
	
	DECLARE @tbl_OPRecord TABLE(IDX INT, OP_ID nvarchar(50), DATE_TIME DATETIME)
	INSERT @tbl_OPRecord EXEC proFATP_GET_TABLE_OP_RECORD @ATE_PC = @ATEPC, @ate_IP = @ATEIP, @ate_MAC = @ATEMAC, @op_id = ''

	IF((SELECT OP_RECORD FROM FATP_TABLE WHERE ATE_PC = @atePC AND ATE_IP = @ateIP AND ATE_MAC = @atemac) IS NULL) OR
		(SELECT LEN(OP_RECORD) FROM FATP_TABLE WHERE ATE_PC = @atePC AND ATE_IP = @ateIP AND ATE_MAC = @atemac) = 0
		BEGIN
			-- SET @isOPAcceptable = 1
			EXEC proFATP_InsertOPRecord @ATE_PC = @atePC, @ATE_IP = @ateIP, @ATE_MAC = @atemac, @OP_ID = @opid
		END
	ELSE
		
		BEGIN
		-- GET LIST RECORDS OF OPERATORS
		
		-- CHECK OP_ID IS IDENTICAL OR NOT BETWEEN 2 SHIFTS
		DECLARE @YEAR INT = YEAR(GETDATE())
		DECLARE @MONTH INT = MONTH(GETDATE())
		DECLARE @DAY INT = DAY(GETDATE())
		DECLARE @DayShiftStart DATETIME = CAST(CONCAT(@YEAR,'-',@MONTH,'-',@DAY,' 07:30:00') AS DATETIME)
		DECLARE @DayShiftEnd DATETIME = CAST(CONCAT(@YEAR,'-',@MONTH,'-',@DAY,' 18:30:00') AS DATETIME)
		DECLARE @LastNightShiftStart DATETIME = CAST(CONCAT(@YEAR,'-',@MONTH,'-',@DAY-1,' 19:30:00') AS DATETIME)
		DECLARE @NextNightShiftEnd DATETIME
		BEGIN TRY
			SET @NextNightShiftEnd = CAST(CONCAT(@YEAR,'-',@MONTH,'-',@DAY+1,' 07:30:00') AS DATETIME)	
		END TRY
		BEGIN CATCH
			SET @NextNightShiftEnd = CAST(CONCAT(@YEAR,'-',@MONTH+1,'-01',' 07:30:00') AS DATETIME)	
		END CATCH
		
		
		
		
			IF(GETDATE() >= @DayShiftStart AND GETDATE() <= @DayShiftEnd)
				BEGIN
					IF(SELECT COUNT(IDX) FROM @tbl_OPRecord WHERE OP_ID = @opid AND DATE_TIME >= @LastNightShiftStart AND DATE_TIME <= @DayShiftStart) > 0
						BEGIN							
							SET @isOPAcceptable = 1
						END
				END			
			IF(GETDATE() >= @DayShiftEnd AND GETDATE() <= @NextNightShiftEnd)
				BEGIN
					IF(SELECT COUNT(IDX) FROM @tbl_OPRecord WHERE OP_ID = @opid AND DATE_TIME >= @DayShiftStart AND DATE_TIME <= @DayShiftEnd) > 0
						BEGIN							
							SET @isOPAcceptable = 1
						END
				END	
		END
	-- IF NOTHING HAPPENDS UPDATE OP_RECORD
	IF (SELECT TOP 1 OP_ID FROM @tbl_OPRecord ORDER BY DATE_TIME DESC) != @opid
		BEGIN
			IF(@isOPAcceptable = 0)
				BEGIN								
					EXEC proFATP_InsertOPRecord @ATE_PC = @atePC, @ATE_IP = @ateIP, @ATE_MAC = @atemac, @OP_ID = @opid				
				END
		END

	
	-- To be added...


	-- === Set all status to result table ===
	-- PRINT('FINISHING!')
	DECLARE @TableStatus TABLE(IS_DUPLICATE INT, IS_INORDER INT, IS_TIMESYNC INT, IS_OPACCEPTABLE INT)
	INSERT INTO @TableStatus VALUES(@isPCDuplicate, @isPCInOrder, @isTimeInSync, @isOPAcceptable)
	SELECT IS_DUPLICATE, IS_INORDER,IS_TIMESYNC, IS_OPACCEPTABLE FROM @TableStatus	
END
GO
EXEC proFATPAbnormalCheckStatus_TEST_1 'L09G182PT105','200.166.41.57','40-8D-5C-DC-BF-B2','U10G182.00','PT1','2024-07-31 07:43:28','V1093107'
EXEC proFATP_GET_TABLE_OP_RECORD 'L03G178FT204','200.166.49.34','E0-D5-5E-8F-F1-41',''
SELECT ATE_PC,ATE_IP,ATE_MAC, OP_RECORD FROM FATP_TABLE WHERE OP_RECORD IS NOT NULL