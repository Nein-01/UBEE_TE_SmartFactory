GO -- 


GO -- CHAT APPLICATION DESIGNING 
--| PRIMARY KEY (USER_ID, GROUP_ID),

CREATE TABLE MESSAGE_GROUP(
	GROUP_ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	GROUP_NAME NVARCHAR(200) NOT NULL UNIQUE,
	CREATE_BY INT NULL,
	CREATE_DATE DATETIME NULL,
	IS_ACTIVE BIT NULL
)
CREATE TABLE MESSAGE_USER_GROUP(
	USER_GROUP_ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[USER_ID] INT NOT NULL,
	GROUP_ID INT NOT NULL,
	ROLE_IN_GROUP NVARCHAR(30) NULL,
	CREATE_DATE DATETIME NULL,
	IS_ACTIVE BIT NULL,
    CONSTRAINT [FK_dbo.MESSAGE_USER_GROUP_dbo.USER_USER_ID] FOREIGN KEY ([USER_ID]) REFERENCES [dbo].[USERS] ([UserID]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.MESSAGE_USER_GROUP_dbo.MESSAGE_GROUP_GROUP_ID] FOREIGN KEY ([GROUP_ID]) REFERENCES [dbo].[MESSAGE_GROUP] ([GROUP_ID]) ON DELETE CASCADE
)
CREATE TABLE MESSAGE_SENDER(
	MESSAGE_ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	MESSAGE_PARENT_ID INT NULL,
	SENDER_ID INT NOT NULL,
	MESSAGE_TYPE NVARCHAR(100) NULL,
	MESSAGE_CONTENT NVARCHAR(MAX) NULL,
	DEVICE_NAME_IP NVARCHAR(100) NULL,
	SEND_DATE DATETIME NULL,
	--RECIPIENT_ID INT NULL,
	--RECIPIENT_GROUP_ID INT NULL,
	--RECEIVE_DATE DATETIME NULL
    CONSTRAINT [FK_dbo.MESSAGE_SENDER_dbo.USER_USER_ID] FOREIGN KEY ([SENDER_ID]) REFERENCES [dbo].[USERs] ([USERID]) ON DELETE CASCADE
)
CREATE TABLE MESSAGE_RECIPIENT(
	RECEIVE_ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	MESSAGE_ID INT NOT NULL,
	RECIPIENT_ID INT NULL,
	RECIPIENT_GROUP_ID INT NULL,
	RECEIVE_DATE DATETIME NULL
	CONSTRAINT [FK_dbo.MESSAGE_RECIPIENT_dbo.MESSAGE_SENDER_MESSAGE_ID] FOREIGN KEY ([MESSAGE_ID]) REFERENCES [dbo].[MESSAGE_SENDER] ([MESSAGE_ID]) ON DELETE CASCADE,	
)
GO -- MODIFYING SCRIPTS
-- ALTER TABLE MESSAGE_SENDER ADD CONSTRAINT [FK_dbo.MESSAGE_SENDER_dbo.USER_USER_ID] FOREIGN KEY ([SENDER_ID]) REFERENCES [dbo].[USERs] ([USERID]) ON DELETE CASCADE
TRUNCATE TABLE MESSAGE_SENDER
-- ALTER TABLE MESSAGE_RECIPIENT ADD CONSTRAINT [FK_dbo.MESSAGE_RECIPIENT_dbo.MESSAGE_SENDER_MESSAGE_ID] FOREIGN KEY ([MESSAGE_ID]) REFERENCES [dbo].[MESSAGE_SENDER] ([MESSAGE_ID]) ON DELETE CASCADE
TRUNCATE TABLE MESSAGE_RECIPIENT
TRUNCATE TABLE MESSAGE_GROUP
ALTER TABLE MESSAGE_GROUP ADD CONSTRAINT CONTRAINT_GROUP_NAME UNIQUE (GROUP_NAME)
TRUNCATE TABLE MESSAGE_USER_GROUP

GO -- QUERYING SCRIPTS
SELECT * FROM USERS
SELECT * FROM MESSAGE_GROUP
SELECT * FROM MESSAGE_USER_GROUP
SELECT * FROM MESSAGE_SENDER
SELECT * FROM MESSAGE_RECIPIENT

-- GET CONVERSTATION BETWEEN 2 USERS
SELECT MS.MESSAGE_ID, US.UserID AS SENDER_ID, US.FullName AS SENDER_NAME, MS.MESSAGE_TYPE, MS.MESSAGE_CONTENT, MS.SEND_DATE FROM MESSAGE_SENDER AS MS INNER JOIN USERS AS US ON SENDER_ID = UserID
WHERE SENDER_ID = 3 AND RECIPIENT_ID = 4
UNION
SELECT MS.MESSAGE_ID, US.UserID AS SENDER_ID, US.FullName AS SENDER_NAME, MS.MESSAGE_TYPE, MS.MESSAGE_CONTENT, MS.SEND_DATE FROM MESSAGE_SENDER AS MS INNER JOIN USERS AS US ON SENDER_ID = UserID
WHERE SENDER_ID = 4 AND RECIPIENT_ID = 3 ORDER BY SEND_DATE 

SELECT SENDER_ID, (SELECT FullName FROM USERS WHERE UserID = MS.SENDER_ID) AS SENDER_NAME, MR.RECIPIENT_ID, (SELECT FullName FROM USERS WHERE UserID = MR.RECIPIENT_ID) AS RECIPIENT_NAME, MS.MESSAGE_CONTENT, MS.SEND_DATE, MR.RECEIVE_DATE FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID WHERE (MS.SENDER_ID = 1 AND MR.RECIPIENT_ID = 3) OR (MS.SENDER_ID = 3 AND MR.RECIPIENT_ID = 1)

-- GET CONVERSTATION IN GROUP
SELECT DISTINCT SENDER_ID, (SELECT FullName FROM USERS WHERE UserID = MS.SENDER_ID) AS SENDER_NAME, MS.MESSAGE_CONTENT, MS.SEND_DATE, MR.RECEIVE_DATE 
FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID INNER JOIN MESSAGE_USER_GROUP MUG ON MUG.USER_GROUP_ID = MR.RECIPIENT_GROUP_ID
WHERE MUG.GROUP_ID = 3 AND (MS.SENDER_ID = 3 OR MR.RECIPIENT_GROUP_ID = 4) --  MUG.GROUP_ID = 1 AND ((MS.SENDER_ID = 3 AND MR.RECIPIENT_GROUP_ID = 2) OR (MS.SENDER_ID = 2 AND MR.RECIPIENT_GROUP_ID = 3))
ORDER BY SEND_DATE

SELECT * FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID WHERE MS.MESSAGE_ID = 11

-- GET UNREAD MESSAGES IN CONVERSATION
SELECT COUNT(*) FROM MESSAGE_SENDER
WHERE SENDER_ID = 3 AND RECIPIENT_ID = 4 AND RECEIVE_DATE IS NULL

GO -- GET USER INFOR IN GROUP
SELECT UserID, UserName,FullName, MG.GROUP_NAME FROM USERS U 
	   INNER JOIN MESSAGE_USER_GROUP MUG ON U.UserID = MUG.USER_ID 
	   INNER JOIN MESSAGE_GROUP MG ON MUG.GROUP_ID = MG.GROUP_ID

SELECT * FROM MESSAGE_GROUP
SELECT * FROM MESSAGE_USER_GROUP

SELECT UGRP.USER_GROUP_ID, GRP.GROUP_ID, GRP.GROUP_NAME, UGRP.ROLE_IN_GROUP , U.UserName AS USER_NAME, U.FullName FULL_NAME FROM MESSAGE_GROUP GRP INNER JOIN MESSAGE_USER_GROUP UGRP ON GRP.GROUP_ID = UGRP.GROUP_ID  INNER JOIN USERS U ON U.UserID = UGRP.USER_ID WHERE UGRP.USER_ID = 5

SELECT UG.USER_GROUP_ID, U.UserID AS USER_ID, UG.GROUP_ID, U.FullName AS FULL_NAME, U.UserName AS USER_NAME FROM MESSAGE_USER_GROUP UG INNER JOIN USERS U ON UG.USER_ID = U.UserID WHERE USER_ID != 3 AND GROUP_ID = 1

GO -- 
SELECT * FROM MESSAGE_USER_GROUP WHERE GROUP_ID = 1 AND USER_ID != 3
GO --
UPDATE MESSAGE_SENDER SET MESSAGE_TYPE = N'img' where message_id = 100
SELECT * FROM MESSAGE_SENDER WHERE MESSAGE_ID = 106;

SELECT MS.MESSAGE_ID, SENDER_ID ,(SELECT FullName FROM USERS WHERE UserID = MS.SENDER_ID) AS SENDER_NAME, MR.RECIPIENT_ID, (SELECT FullName FROM USERS WHERE UserID = MR.RECIPIENT_ID) AS RECIPIENT_NAME, MESSAGE_TYPE, MESSAGE_CONTENT, SEND_DATE 
FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID 
WHERE (MS.SENDER_ID = 5 AND MR.RECIPIENT_ID = 3) OR (MS.SENDER_ID = 3 AND MR.RECIPIENT_ID = 5) ORDER BY SEND_DATE DESC