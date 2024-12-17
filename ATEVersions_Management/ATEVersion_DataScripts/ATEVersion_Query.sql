GO -- ========= ATE VERSION TEST =========
SELECT * FROM ROLES
GO -- ========= FATP_CPK_DATA TABLE DEFINATION =========
ALTER TABLE FATP_CPK_DATA
ADD OP_ID NVARCHAR(8) NULL,
	OP_ID_HISTORY NVARCHAR(MAX) NULL
ALTER TABLE FATP_CPK_DATA
ALTER COLUMN OP_ID_HISTORY NVARCHAR(MAX) NULL
GO
SELECT * FROM FATP_CPK_DATA WHERE  ATE_PC LIKE 'L01C153FT002' -- ID = 1 AND

UPDATE FATP_CPK_DATA SET OP_ID = 'V0908660', OP_ID_HISTORY = 'V0908660,2024-06-06 14:56:52|V0908661,2024-06-06 14:56:52|V0908662,2024-06-06 14:56:52|V0908663,2024-06-07 14:56:52|V0908664,2024-06-07 14:56:52|V0908665,2024-06-07 14:56:52|V0908666,2024-06-08 14:56:52|V0908667,2024-06-08 14:56:52|V0908669,2024-06-08 14:56:52'
WHERE ID = 1

UPDATE FATP_CPK_DATA SET OP_ID = NULL, OP_ID_HISTORY = NULL WHERE ID != 1

SELECT OP_ID, OP_ID_HISTORY FROM FATP_CPK_DATA
WHERE OP_ID IS NULL

GO -- ========= FATP_CPK_DATA TABLE PROCEDURES ========= 
GO -- === proFATP_InsertOPRecord ===
CREATE PROCEDURE proFATP_InsertOPRecord(
	@ATE_PC NVARCHAR(20),
	@ATE_IP NVARCHAR(20),
	@ATE_MAC NVARCHAR(20),
	@OP_ID NVARCHAR(8)
)
AS
BEGIN
	-- INSERT/UPDATE OP_ID
	-- UPDATE FATP_CPK_DATA SET OP_ID = 'V1060524' WHERE ATE_PC LIKE 'L01C153FT002'
	-- INSERT/UPDATE OPS_RECORD
	DECLARE @ate nvarchar(20) = 'L01C153FT002' -- L01C153FT002
	DECLARE @currOPRecord nvarchar(max)
	DECLARE @newOPRecord nvarchar(max)
	SELECT @currOPRecord = OP_ID_HISTORY FROM FATP_CPK_DATA WHERE ATE_PC LIKE @ate -- @ate | @ATE_PC
	IF @currOPRecord IS NULL 
		BEGIN
			set @newOPRecord = concat('V1060524',',',getdate())			
			PRINT(@newOPRecord)
		END
	IF @currOPRecord IS NOT NULL 
		BEGIN
			set @newOPRecord = concat(@currOPRecord, '|','V1060520',',',getdate())			
			PRINT(@newOPRecord)
		END
	UPDATE FATP_CPK_DATA SET OP_ID_HISTORY = @newOPRecord WHERE ATE_PC LIKE @ate
END
GO -- === procFATP_TABLE_OPID_RECORD ===
CREATE PROCEDURE procFATP_TABLE_OPID_RECORD
AS
BEGIN

	DECLARE @opHistory nvarchar(max)
	SELECT @opHistory = OP_ID_HISTORY FROM FATP_CPK_DATA WHERE ATE_PC LIKE 'L01C153FT002' -- ID = 1 | ATE_PC LIKE 'L01C153FT002'
	DECLARE @tbl_OPRecord TABLE(idx int identity(1,1), OpRecord nvarchar(50))
	INSERT INTO @tbl_OPRecord SELECT * FROM string_split(@opHistory,'|')
	DECLARE @iterStep int
	DECLARE @iterMax int	
	SELECT @iterStep = MIN(IDX), @iterMax = MAX(IDX) FROM @tbl_OPRecord
	
	--SELECT * FROM @tbl_OPRecord
	DECLARE @tbl_OPRecordEach TABLE(IDX int identity(1,1), OP_ID nvarchar(50), DATE_TIME DATETIME)
	WHILE @iterStep <= @iterMax
		BEGIN	
			PRINT(CONCAT('========= LOOP: ',@iterStep ,' ========='))
			DECLARE @currOPRecord nvarchar(50)
			SELECT @currOPRecord = OpRecord FROM @tbl_OPRecord WHERE IDX = @iterStep
			--SET @currOPRecord =@currOPRecord
			print(@currOPRecord)
			--SELECT CHARINDEX(',',@currOPRecord)
			-- === GET OP_ID FROM ===
			DECLARE @currOPRecord_OPID NVARCHAR(50)
			SET @currOPRecord_OPID = SUBSTRING(@currOPRecord,0,CHARINDEX(',',@currOPRecord))
			PRINT (@currOPRecord_OPID)
			-- === GET DATE_TIME ===
			DECLARE @currOPRecord_DATETIME DATETIME
			SET @currOPRecord_DATETIME = CAST(SUBSTRING(@currOPRecord,CHARINDEX(',',@currOPRecord)+1, LEN(@currOPRecord)) AS DATETIME)
			PRINT (@currOPRecord_DATETIME)	
			
			INSERT INTO @tbl_OPRecordEach(OP_ID,DATE_TIME) VALUES(@currOPRecord_OPID, @currOPRecord_DATETIME)
			SET @iterStep = @iterStep + 1
		END;
	SELECT * FROM @tbl_OPRecordEach
END

GO -- ========= PROGRAM TABLE =========
SELECT * FROM PROGRAM

SELECT * FROM PROGRAM WHERE ModelName LIKE '%U10H%' OR ModelName LIKE '%U10G%'

SELECT COUNT(ProgramID) 'COUNT', ProjectType FROM PROGRAM
GROUP BY ProjectType

UPDATE PROGRAM SET ProjectType = 'CABLE' WHERE ModelName LIKE '%U10C%'
UPDATE PROGRAM SET ProjectType = 'GPON' WHERE ModelName LIKE '%UBN%' OR ModelName LIKE '%UBH%' OR ModelName LIKE '%U10H%' OR ModelName LIKE '%U10G%'
UPDATE PROGRAM SET ProjectType = 'WIRELESS' WHERE ModelName NOT LIKE '%UBN%' AND ModelName NOT LIKE '%UBH%' AND ModelName NOT LIKE '%U10%'

SELECT ModelName FROM PROGRAM WHERE ProjectType LIKE '%CABLE%'
GO -- ========= ATE VERSION TABLE =========

SELECT * FROM VERSION 
WHERE VersionID = 297

UPDATE ATE_CHECKLIST 
SET PreparedAt = '2024-06-06 14:48:07.573', CheckedAt = '2024-06-06 14:52:16.573', ApprovedAt = '2024-06-06 14:56:52.573' 
WHERE VersionID = 297
GO -- ========= ATE LIST TABLE =========

SELECT * FROM ATE_CHECKLIST 
WHERE VersionID = 297

UPDATE ATE_CHECKLIST 
SET PreparedAt = '2024-06-06 14:48:07.573', CheckedAt = '2024-06-06 14:52:16.573', ApprovedAt = '2024-06-06 14:56:52.573' 
WHERE VersionID = 297
GO -- GR&R Table --
SELECT * FROM GRR_TABLE
DELETE FROM GRR_TABLE WHERE GRR_ID IN (1) -- GageModel LIKE 'U10C180.01'
GO
SELECT * FROM PROGRAM

SELECT PRG.ModelName, VRS.VersionName, ALST.Status FROM PROGRAM PRG JOIN VERSION VRS ON PRG.ProgramID = VRS.ProgramID JOIN ATE_CHECKLIST ALST ON VRS.VersionID = ALST.VersionID 
WHERE PRG.ModelName LIKE '%172%' AND ALST.Status = 1

SELECT COUNT(VRS.VersionID) FROM PROGRAM PRG JOIN VERSION VRS ON PRG.ProgramID = VRS.ProgramID

GO

-- COUNT(VRS.VersionID) | PRG.ModelName, VRS.VersionName
DECLARE @AllVersions INT 
DECLARE @ALSTVersions INT 
SELECT @AllVersions = COUNT(VRS.VersionID) FROM PROGRAM PRG JOIN VERSION VRS ON PRG.ProgramID = VRS.ProgramID
WHERE PRG.ModelName LIKE '%180.00%'

SELECT @ALSTVersions = COUNT(VRS.VersionID) FROM PROGRAM PRG JOIN VERSION VRS ON PRG.ProgramID = VRS.ProgramID JOIN ATE_CHECKLIST ALST ON VRS.VersionID = ALST.VersionID 
WHERE PRG.ModelName LIKE '%180.00%'
SELECT @AllVersions - @ALSTVersions
GO

SELECT VRS.VersionID, PRG.ModelName, VRS.VersionName FROM PROGRAM AS PRG JOIN VERSION AS VRS ON PRG.ProgramID = VRS.ProgramID
ORDER BY PRG.ModelName DESC, CONVERT(FLOAT, SUBSTRING(VRS.VersionName,4,LEN(VRS.VersionName))) DESC

SELECT CONVERT(FLOAT, SUBSTRING('V1.32',2,LEN('V1.32'))) AS VerCode

GO -- begin::Function GetListLatestModelVersion
CREATE FUNCTION funcGetListLatestModelVersion()
RETURNS @tblLatestModelVersions TABLE(
			MODELNAME nvarchar(50),
			VERSIONLATEST nvarchar(50)
		)
AS
BEGIN
	DECLARE @tblModel TABLE(idx int identity(1,1), modelId int , model nvarchar(50))
	INSERT INTO @tblModel SELECT ProgramID, ModelName FROM PROGRAM

	DECLARE @iterCount int
	DECLARE @iterEnd int	
	SELECT @iterCount = MIN(IDX), @iterEnd = MAX(IDX) FROM @tblModel

	WHILE @iterCount <= @iterEnd
		BEGIN
			
			DECLARE @currModelID int
			DECLARE @currModelName nvarchar(50)
			SELECT @currModelID = ModelID, @currModelName = model FROM @tblModel WHERE IDX = @iterCount
			
			DECLARE @latestModelVersion nvarchar(50)
			SET @latestModelVersion = (SELECT TOP 1 VersionName FROM VERSION WHERE ProgramID = @currModelID ORDER BY CONVERT(FLOAT, SUBSTRING(VersionName,4,LEN(VersionName))) DESC)
			PRINT( @currModelName + ' | ' + @latestModelVersion);
			--INSERT INTO @tblLatestModelVersions VALUES(@currModelName, @latestModelVersion)
			SET @iterCount = @iterCount + 1;
		END;
		
	RETURN;
END;
GO
GO
	SELECT * FROM funcGetListLatestModelVersion() ORDER BY MODELNAME DESC,VERSIONLATEST DESC
	SELECT PRG.ModelName, VRS.VersionName FROM PROGRAM AS PRG JOIN VERSION AS VRS ON PRG.ProgramID = VRS.ProgramID
ORDER BY PRG.ModelName DESC, CONVERT(FLOAT, SUBSTRING(VRS.VersionName,4,LEN(VRS.VersionName))) DESC
GO -- end::Function GetListLatestModelVersion

SELECT * FROM ROLES
UPDATE ROLES SET RoleName = 'Client' WHERE RoleID = 2

SELECT * FROM TEST_PLAN
TRUNCATE TABLE TEST_PLAN

SELECT * FROM FATP_CPK_DATA
WHERE ATE_PC LIKE '%' AND MODEL LIKE '%153%' AND STATION LIKE '%FT%' AND DATEDIFF(HOUR,POST_DATE,GETDATE()) > 24
ORDER BY ATE_PC ASC

SELECT ATE_PC, DATEDIFF(MILLISECOND,POST_DATE,GETDATE()) FROM FATP_CPK_DATA
WHERE ATE_PC LIKE '%' AND MODEL LIKE '%153%' AND STATION LIKE '%FT%'
ORDER BY ATE_PC ASC

GO

CREATE TRIGGER dbo.trgProgramInsert
ON dbo.PROGRAMS
FOR INSERT 
AS
BEGIN
	DECLARE @nameProgram nvarchar(50);
	DECLARE @nameDvlWith nvarchar(50);
	SELECT @nameProgram = i.ProgramName from inserted i;
	SELECT @nameDvlWith = i.DevelopedWith from inserted i;

	IF EXISTS (SELECT ProgramID FROM PROGRAMS WHERE ProgramName = @nameProgram)
	begin
		update dbo.PROGRAMS set DevelopedWith = @nameDvlWitH WHERE ProgramName = @nameProgram
	end
	
END
--
GO
DROP TRIGGER dbo.trgProgramInsert
GO
CREATE TRIGGER dbo.trgProgramInsteadInsert
ON dbo.PROGRAMS
INSTEAD OF INSERT 
AS
BEGIN
	DECLARE @idProgram int;
	DECLARE @nameProgram nvarchar(50);
	DECLARE @nameDvlWith nvarchar(50);
	SELECT @idProgram = i.ProgramID from inserted i;
	SELECT @nameProgram = i.ProgramName from inserted i;
	SELECT @nameDvlWith = i.DevelopedWith from inserted i;

	IF EXISTS (SELECT ProgramID FROM PROGRAMS WHERE ProgramName = @nameProgram)
	begin
		update dbo.PROGRAMS set DevelopedWith = @nameDvlWitH WHERE ProgramName = @nameProgram
	end
	ELSE
	begin
		INSERT INTO dbo.PROGRAMS(ProgramID, ProgramName, DevelopedWith)
		SELECT ProgramID,ProgramName, DevelopedWith  FROM inserted
	end
	
END
GO
SET IDENTITY_INSERT [dbo].[PROGRAMS] ON
INSERT INTO PROGRAMS VALUES('U10C181.00','Two')
DELETE FROM PROGRAMS WHERE ProgramID = 15
GO

SELECT * FROM dbo.PROGRAM

GO -- ========= ATE TEST PLAN TABLE =========
SELECT * FROM TEST_PLAN

SELECT ModelName, ProjectType
FROM TEST_PLAN
GROUP BY ModelName, ProjectType

SELECT ModelName, TestPlanVersion FROM TEST_PLAN 
ORDER BY ModelName, CreatedAt DESC

DECLARE @tblModel TABLE(idx int identity(1,1), modelName nvarchar(50), modelVer nvarchar(50))
INSERT INTO @tblModel SELECT ModelName, TestPlanVersion FROM TEST_PLAN ORDER BY ModelName, CreatedAt DESC
SELECT * FROM @tblModel

SELECT * FROM TEST_PLAN WHERE ProjectType = 'CABLE'
SELECT * FROM TEST_PLAN WHERE ProjectType = 'GPON'
SELECT * FROM TEST_PLAN WHERE ProjectType = 'WIRELESS'

UPDATE TEST_PLAN SET ProjectType = 'CABLE' WHERE ModelName LIKE '%U10C%'
UPDATE TEST_PLAN SET ProjectType = 'GPON' WHERE ModelName LIKE '%UBN%' OR ModelName LIKE '%UBH%' OR ModelName LIKE '%U10H%' OR ModelName LIKE '%U10G%'
UPDATE TEST_PLAN SET ProjectType = 'WIRELESS' WHERE ModelName NOT LIKE '%UBN%' AND ModelName NOT LIKE '%UBH%' AND ModelName NOT LIKE '%U10%'

GO -- begin::Function GetListLatestTestPlanVersion
CREATE FUNCTION funcGetListLatestTestPlanVersion()
RETURNS @tblLatestTestPlanVersion TABLE(
			MODELNAME nvarchar(50),
			VERSIONLATEST nvarchar(50)
		)
AS
BEGIN
	DECLARE @tblModel TABLE(idx int identity(1,1), modelName nvarchar(50))
	INSERT INTO @tblModel SELECT DISTINCT ModelName FROM TEST_PLAN ORDER BY ModelName

	DECLARE @iterCount int
	DECLARE @iterEnd int	
	SELECT @iterCount = MIN(IDX), @iterEnd = MAX(IDX) FROM @tblModel

	WHILE @iterCount <= @iterEnd
		BEGIN
						
			DECLARE @currModelName nvarchar(50)
			SELECT  @currModelName = modelName FROM @tblModel WHERE IDX = @iterCount
			
			DECLARE @latestModelVersion nvarchar(50)
			SET @latestModelVersion = (SELECT TOP 1 TestPlanVersion FROM TEST_PLAN WHERE ModelName LIKE @currModelName ORDER BY CreatedAt DESC)
			
			-- PRINT(@currModelName + ' | ' + @latestModelVersion);

			INSERT INTO @tblLatestTestPlanVersion VALUES(@currModelName, @latestModelVersion)
			SET @iterCount = @iterCount + 1;
		END;
		
	RETURN;
END;
GO
GO
	SELECT * FROM funcGetListLatestTestPlanVersion() ORDER BY MODELNAME,VERSIONLATEST DESC
	
GO -- end::Function GetListLatestTestPlanVersion
GO
SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS TAB
SELECT * FROM INFORMATION_SCHEMA.ROUTINES

GO

SELECT * FROM VERSION