GO -- ======= begin::Function GetListLatestModelVersion =======
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
GO -- ======= end::Function GetListLatestModelVersion =======

GO -- ======= BEGIN:TRIGGER PROGRAM INSERT =======
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
GO -- ======= END:TRIGGER PROGRAM INSERT =======
-- DROP TRIGGER dbo.trgProgramInsert
GO
GO -- ======= BEGIN:TRIGGER PROGRAM INSTEAD INSERT =======
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
GO -- ======= END:TRIGGER PROGRAM INSTEAD INSERT =======
GO -- ATE TEST PLAN
SELECT * FROM TEST_PLAN

SELECT ModelName, TestPlanVersion FROM TEST_PLAN 
ORDER BY ModelName, CreatedAt DESC

DECLARE @tblModel TABLE(idx int identity(1,1), modelName nvarchar(50), modelVer nvarchar(50))
INSERT INTO @tblModel SELECT ModelName, TestPlanVersion FROM TEST_PLAN ORDER BY ModelName, CreatedAt DESC
SELECT * FROM @tblModel

SELECT * FROM TEST_PLAN
WHERE ModelName LIKE '%180.00%' AND TestPlanVersion LIKE '%.00%'

GO -- ======= begin::Function GetListLatestTestPlanVersion =======
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
	
GO -- ======= end::Function GetListLatestTestPlanVersion =======
GO 
SELECT * FROM funcGetListLatestTestPlanVersion() ORDER BY MODELNAME,VERSIONLATEST DESC
GO -- ======= begin::Function GetListLatestTestPlanVersion =======
CREATE FUNCTION funcGetListLatestTestPlanVersionOfProjectType(@projecttype nvarchar(10))
RETURNS @tblLatestTestPlanVersion TABLE(
			MODELNAME nvarchar(50),
			VERSIONLATEST nvarchar(50)
		)
AS
BEGIN
	DECLARE @tblModel TABLE(idx int identity(1,1), modelName nvarchar(50))
	INSERT INTO @tblModel SELECT DISTINCT ModelName FROM TEST_PLAN WHERE ProjectType = @projecttype ORDER BY ModelName
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
	SELECT * FROM funcGetListLatestTestPlanVersionOfProjectType('CABLE') ORDER BY MODELNAME,VERSIONLATEST DESC
	
GO -- ======= end::Function GetListLatestTestPlanVersion =======
GO
SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS TAB
SELECT * FROM INFORMATION_SCHEMA.ROUTINES