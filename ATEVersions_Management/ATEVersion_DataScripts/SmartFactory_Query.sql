GO -- ========= MESSAGE SYSTEM TABLE MODIFICATION =========
TRUNCATE TABLE MESSAGE_SENDER
ALTER TABLE MESSAGE_RECIPIENT ADD CONSTRAINT [FK_dbo.MESSAGE_RECIPIENT_dbo.MESSAGE_SENDER_MESSAGE_ID] FOREIGN KEY ([MESSAGE_ID]) REFERENCES [dbo].[MESSAGE_SENDER] ([MESSAGE_ID]) ON DELETE CASCADE
TRUNCATE TABLE MESSAGE_RECIPIENT

TRUNCATE TABLE MESSAGE_GROUP
ALTER TABLE MESSAGE_USER_GROUP ADD CONSTRAINT [FK_dbo.MESSAGE_USER_GROUP_dbo.MESSAGE_GROUP_GROUP_ID] FOREIGN KEY ([GROUP_ID]) REFERENCES [dbo].[MESSAGE_GROUP] ([GROUP_ID]) ON DELETE CASCADE
TRUNCATE TABLE MESSAGE_USER_GROUP
GO -- ========= MESSAGE SYSTEM DATA QUERY =========
SELECT * FROM USERS
SELECT * FROM MESSAGE_GROUP
SELECT * FROM MESSAGE_USER_GROUP
SELECT * FROM MESSAGE_SENDER
SELECT * FROM MESSAGE_RECIPIENT
GO -- QUERY TO GET LIST USER
SELECT * FROM USERS U INNER JOIN MESSAGE_RECIPIENT MR ON U.UserID = MR.RECIPIENT_ID INNER JOIN MESSAGE_SENDER MS ON MR.MESSAGE_ID = MS.MESSAGE_ID
WHERE U.UserID != 3

SELECT U.UserID USER_ID, U.UserName USER_NAME, U.FullName FULL_NAME, R.RoleName ROLE_NAME, (SELECT COUNT(MR.MESSAGE_ID) FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID
WHERE MR.RECIPIENT_ID = 3 AND MS.SENDER_ID = U.UserID AND MR.RECEIVE_DATE IS NULL) UNREAD_NUM FROM USERS U INNER JOIN ROLES R ON U.RoleID = R.RoleID
WHERE U.UserID != 3 ORDER BY UNREAD_NUM DESC, USER_NAME ASC

SELECT * FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID
WHERE MR.RECIPIENT_ID = 3 AND MR.RECEIVE_DATE IS NULL -- MR.RECIPIENT_ID = 3 | MS.SENDER_ID = 3 

GO -- QUERY TO UPDATE UNREAD TO READ

SELECT * FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID
WHERE MR.RECIPIENT_ID = 3 AND MS.SENDER_ID = 2 AND MR.RECEIVE_DATE IS NULL

UPDATE MESSAGE_RECIPIENT SET RECEIVE_DATE = '' WHERE RECEIVE_ID IN (SELECT RECEIVE_ID FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID WHERE MR.RECIPIENT_ID = 3 AND MS.SENDER_ID = 2 AND MR.RECEIVE_DATE IS NULL)

UPDATE MESSAGE_RECIPIENT SET RECEIVE_DATE = '' 
WHERE RECEIVE_ID IN 
(SELECT RECEIVE_ID FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID 
WHERE MR.RECIPIENT_ID = 3 AND MS.SENDER_ID = 2 AND MR.RECEIVE_DATE IS NULL)

SELECT RECEIVE_ID FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID INNER JOIN MESSAGE_USER_GROUP MUG ON MUG.USER_GROUP_ID = MR.RECIPIENT_GROUP_ID
WHERE MR.RECEIVE_DATE IS NULL AND MUG.GROUP_ID = 1 AND MR.RECIPIENT_GROUP_ID = (SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE GROUP_ID = 1 AND USER_ID = 3)

GO -- QUERY TO GET LIST GROUP 
SELECT GROUP_ID, GROUP_NAME, CREATE_DATE, (SELECT COUNT(USER_GROUP_ID) FROM MESSAGE_USER_GROUP WHERE GROUP_ID = MG.GROUP_ID) AS MEMBER_NUM FROM MESSAGE_GROUP MG 

SELECT GROUP_ID, GROUP_NAME, CREATE_DATE, 
(SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE GROUP_ID = MG.GROUP_ID AND USER_ID = 3 ) AS RECIPIENT_GROUP_ID, 
(SELECT COUNT(USER_GROUP_ID) FROM MESSAGE_USER_GROUP WHERE GROUP_ID = MG.GROUP_ID) AS MEMBER_NUM,
(SELECT COUNT(MS.MESSAGE_ID) UNREAD_NUM FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID INNER JOIN MESSAGE_USER_GROUP MUG ON MUG.USER_GROUP_ID = MR.RECIPIENT_GROUP_ID WHERE MR.RECEIVE_DATE IS NULL AND MUG.GROUP_ID = MG.GROUP_ID AND MR.RECIPIENT_GROUP_ID = (SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE GROUP_ID = MG.GROUP_ID AND USER_ID = 3)) AS UNREAD_NUM
FROM MESSAGE_GROUP MG WHERE GROUP_ID IN (SELECT GROUP_ID FROM MESSAGE_USER_GROUP WHERE USER_ID = 3)
ORDER BY UNREAD_NUM DESC, GROUP_NAME ASC


SELECT SENDER_ID, (SELECT FullName FROM USERS WHERE UserID = MS.SENDER_ID) AS SENDER_NAME, MS.MESSAGE_CONTENT, MS.SEND_DATE, MR.RECEIVE_DATE 
FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID INNER JOIN MESSAGE_USER_GROUP MUG ON MUG.USER_GROUP_ID = MR.RECIPIENT_GROUP_ID
WHERE RECEIVE_DATE IS NULL AND MUG.GROUP_ID = 1 AND MR.RECIPIENT_GROUP_ID = 1 
ORDER BY SEND_DATE

SELECT COUNT(MS.MESSAGE_ID) UNREAD_NUM FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID INNER JOIN MESSAGE_USER_GROUP MUG ON MUG.USER_GROUP_ID = MR.RECIPIENT_GROUP_ID WHERE MR.RECEIVE_DATE IS NULL AND MUG.GROUP_ID = 1 AND MR.RECIPIENT_GROUP_ID = 1

GO -- QUERY TO GET USER IN A GROUP
SELECT UG.USER_GROUP_ID, U.UserID AS USER_ID, UG.GROUP_ID, U.FullName AS FULL_NAME, U.UserName AS USER_NAME, R.RoleName AS ROLE_NAME, UG.ROLE_IN_GROUP FROM MESSAGE_USER_GROUP UG INNER JOIN USERS U ON UG.USER_ID = U.UserID INNER JOIN ROLES R ON U.RoleID = R.RoleID WHERE GROUP_ID = 1

GO -- QUERY TO GET USER NOT IN GROUP
SELECT UG.USER_GROUP_ID, U.UserID AS USER_ID, UG.GROUP_ID, U.FullName AS FULL_NAME, U.UserName AS USER_NAME, R.RoleName AS ROLE_NAME, UG.ROLE_IN_GROUP 
FROM MESSAGE_USER_GROUP UG INNER JOIN USERS U ON UG.USER_ID = U.UserID INNER JOIN ROLES R ON U.RoleID = R.RoleID 
WHERE GROUP_ID != 1

SELECT DISTINCT U.UserID AS USER_ID
FROM USERS U INNER JOIN MESSAGE_USER_GROUP MUG ON U.UserID = MUG.USER_ID
WHERE MUG.GROUP_ID = 3 OR U.UserID = 3

SELECT U.UserID USER_ID, U.UserName USER_NAME, U.FullName FULL_NAME, R.RoleName FROM USERS U INNER JOIN ROLES R ON U.RoleID = R.RoleID
WHERE USERID NOT IN (SELECT DISTINCT U.UserID AS USER_ID
FROM USERS U INNER JOIN MESSAGE_USER_GROUP MUG ON U.UserID = MUG.USER_ID
WHERE MUG.GROUP_ID = 1 OR U.UserID = 3)

GO -- QUERY TO GET MESSAGES IN GROUP
SELECT DISTINCT SENDER_ID, (SELECT FullName FROM USERS WHERE UserID = MS.SENDER_ID) AS SENDER_NAME, MS.MESSAGE_CONTENT, MS.SEND_DATE, MR.RECEIVE_DATE 
FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID INNER JOIN MESSAGE_USER_GROUP MUG ON MUG.USER_GROUP_ID = MR.RECIPIENT_GROUP_ID
WHERE MUG.GROUP_ID = 1 AND (MS.SENDER_ID = 3 OR MR.RECIPIENT_GROUP_ID = 1) --  MUG.GROUP_ID = 1 AND ((MS.SENDER_ID = 3 AND MR.RECIPIENT_GROUP_ID = 2) OR (MS.SENDER_ID = 2 AND MR.RECIPIENT_GROUP_ID = 3))
ORDER BY SEND_DATE

GO -- QUERY TO GET NUMBER OF UNREAD MESSAGES COUNT
SELECT COUNT(MS.MESSAGE_ID) UNREAD_COUNT FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID
WHERE MR.RECEIVE_DATE IS NULL AND (MR.RECIPIENT_ID = 3 OR MR.RECIPIENT_GROUP_ID IN (SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE USER_ID = 3))

SELECT MS.MESSAGE_ID, MS.MESSAGE_CONTENT, MR.RECIPIENT_ID, MR.RECIPIENT_GROUP_ID FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID
WHERE (MR.RECIPIENT_ID = 3 OR MR.RECIPIENT_GROUP_ID IN (SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE USER_ID = 3))
GO -- 
SELECT SENDER_ID ,(SELECT FullName FROM USERS WHERE UserID = MS.SENDER_ID) AS SENDER_NAME, MR.RECIPIENT_ID, (SELECT FullName FROM USERS WHERE UserID = MR.RECIPIENT_ID) AS RECIPIENT_NAME, MESSAGE_CONTENT, SEND_DATE 
FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID 
WHERE (MS.SENDER_ID = 1 AND MR.RECIPIENT_ID = 9) OR (MS.SENDER_ID = 9 AND MR.RECIPIENT_ID = 1) ORDER BY SEND_DATE
GO --
SELECT * FROM MESSAGE_USER_GROUP
WHERE GROUP_ID = 3
DELETE FROM MESSAGE_USER_GROUP WHERE USER_GROUP_ID IN (9,10,11)
GO -- 

SELECT * FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID WHERE MS.MESSAGE_ID = 11

SET IDENTITY_INSERT dbo.MESSAGE_SENDER OFF INSERT INTO dbo.MESSAGE_SENDER(SENDER_ID, MESSAGE_TYPE, MESSAGE_CONTENT, DEVICE_NAME_IP, SEND_DATE) OUTPUT INSERTED.MESSAGE_ID VALUES(1, N'text', N'test', N'::1', GETDATE())

SELECT * FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID 
WHERE MR.RECEIVE_DATE IS NULL AND (MR.RECIPIENT_ID = 3 OR MR.RECIPIENT_GROUP_ID IN (SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE USER_ID = 3))

