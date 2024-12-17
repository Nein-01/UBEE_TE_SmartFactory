using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.DTOModels.ATEVersionDTOs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ATEVersions_Management.Models.DAOModels.ATEVersionDAOs
{
    public class MessageSystemDAO
    {
        #region GET Data Functions
        static public UserDTO GET_UserByUserName(string username)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    UserDTO dataUser = (from user in db.USERS
                                        where user.UserName.ToUpper() == username.ToUpper()
                                        select new UserDTO()
                                        {
                                            UserID = user.UserID,
                                            UserName = user.UserName,
                                            RoleName = user.ROLE.RoleName,
                                            FullName = user.FullName,
                                            Email = user.Email,
                                        }).SingleOrDefault();
                    if (dataUser == null)
                    {
                        dataUser = new UserDTO();
                    }
                    return dataUser;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<UserDTO> GET_ListAllUser()
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return (from user in db.USERS
                            select new UserDTO()
                            {
                                UserID = user.UserID,
                                UserName = user.UserName,
                                RoleName = user.ROLE.RoleName,
                                FullName = user.FullName,
                                Email = user.Email,
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<UserDTO> GET_ListUserExceptLoggedUser(int loggedUserID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return (from user in db.USERS
                            where user.UserID != loggedUserID
                            select new UserDTO()
                            {
                                UserID = user.UserID,
                                UserName = user.UserName,
                                RoleName = user.ROLE.RoleName,
                                FullName = user.FullName,
                                Email = user.Email,
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<MessageInvidualDTO> GET_ListSingleRecipientOfLoggedUser(int loggedUserID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    string sqlCommand = "SELECT U.UserID USER_ID, U.UserName USER_NAME, U.FullName FULL_NAME, R.RoleName ROLE_NAME, (SELECT COUNT(MR.MESSAGE_ID) FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID " +
                        "WHERE MR.RECIPIENT_ID = "+loggedUserID+" AND MS.SENDER_ID = U.UserID AND MR.RECEIVE_DATE IS NULL) UNREAD_NUM FROM USERS U INNER JOIN ROLES R ON U.RoleID = R.RoleID WHERE U.UserID != " + loggedUserID + " ORDER BY UNREAD_NUM DESC, USER_NAME ASC";

                    return db.Database.SqlQuery<MessageInvidualDTO>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<MessageSenderDTO> GET_MessageRecordBetween2Users(int senderID, int recipientID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    //string sqlCommand = "SELECT US.UserID AS SENDER_ID, US.FullName AS SENDER_NAME, MS.MESSAGE_TYPE, MS.MESSAGE_CONTENT, MS.SEND_DATE FROM MESSAGE_SENDER AS MS INNER JOIN USERS AS US ON SENDER_ID = UserID WHERE SENDER_ID = "+senderID.ToString()+" AND RECIPIENT_ID = "+recipientID.ToString() + " UNION "+"SELECT US.UserID AS SENDER_ID, US.FullName AS SENDER_NAME, MS.MESSAGE_TYPE, MS.MESSAGE_CONTENT, MS.SEND_DATE FROM MESSAGE_SENDER AS MS INNER JOIN USERS AS US ON SENDER_ID = UserID WHERE SENDER_ID = "+ recipientID.ToString() + " AND RECIPIENT_ID = "+ senderID.ToString();
                    string sqlCommand = "SELECT MS.MESSAGE_ID, SENDER_ID ,(SELECT FullName FROM USERS WHERE UserID = MS.SENDER_ID) AS SENDER_NAME, MR.RECIPIENT_ID, (SELECT FullName FROM USERS WHERE UserID = MR.RECIPIENT_ID) AS RECIPIENT_NAME, MESSAGE_TYPE, MESSAGE_CONTENT, SEND_DATE "+
                        "FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID " +
                        "WHERE (MS.SENDER_ID = " + senderID.ToString() + " AND MR.RECIPIENT_ID = " + recipientID.ToString() + ") OR (MS.SENDER_ID = " + recipientID.ToString() + " AND MR.RECIPIENT_ID = " + senderID.ToString() + ") ORDER BY SEND_DATE";

                    //string sqlCommand = "SELECT SENDER_ID ,(SELECT FullName FROM USERS WHERE UserID = MSG.SENDER_ID) AS SENDER_NAME, RECIPIENT_ID, (SELECT FullName FROM USERS WHERE UserID = MSG.RECIPIENT_ID) AS RECIPIENT_NAME, MESSAGE_CONTENT, SEND_DATE FROM MESSAGE_SENDER AS MSG WHERE (SENDER_ID = " + senderID.ToString() + " AND RECIPIENT_ID = " + recipientID.ToString() + ") OR (SENDER_ID = " + recipientID.ToString() + " AND RECIPIENT_ID = " + senderID.ToString() + ")";

                    return db.Database.SqlQuery<MessageSenderDTO>(sqlCommand).ToList();
                    /*
                    return (from message in db.MESSAGE_SENDER
                            where (message.SENDER_ID == senderID && message.RECIPIENT_ID == recipientID) ||
                                  (message.SENDER_ID == recipientID && message.RECIPIENT_ID == senderID)
                            select new MessageSenderDTO()
                            {
                                SENDER_ID = message.SENDER_ID,
                                RECIPIENT_ID = message.RECIPIENT_ID,
                                MESSAGE_CONTENT = message.MESSAGE_CONTENT,
                                DEVICE_NAME_IP = message.DEVICE_NAME_IP,
                                SEND_DATE = message.SEND_DATE,
                                RECEIVE_DATE = message.RECEIVE_DATE,
                            }).ToList();   */
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public MessageSenderDTO GET_MessageRecordByMessageID(int MessageID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    string sqlCommand = "SELECT MESSAGE_ID, SENDER_ID, MESSAGE_TYPE, MESSAGE_CONTENT, SEND_DATE FROM MESSAGE_SENDER WHERE MESSAGE_ID = " + MessageID;                    

                    return db.Database.SqlQuery<MessageSenderDTO>(sqlCommand).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<MessageGroupDTO> GET_ListGroupContainLoggedUser(int loggedUserID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    string sqlCommand = "SELECT GROUP_ID, GROUP_NAME, CREATE_DATE, " +
                        "(SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE GROUP_ID = MG.GROUP_ID AND USER_ID = " + loggedUserID + " ) AS RECIPIENT_GROUP_ID, " +
                        "(SELECT COUNT(USER_GROUP_ID) FROM MESSAGE_USER_GROUP WHERE GROUP_ID = MG.GROUP_ID) AS MEMBER_NUM, " +
                        "(SELECT COUNT(MS.MESSAGE_ID) UNREAD_NUM FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID INNER JOIN MESSAGE_USER_GROUP MUG ON MUG.USER_GROUP_ID = MR.RECIPIENT_GROUP_ID WHERE MR.RECEIVE_DATE IS NULL AND MUG.GROUP_ID = MG.GROUP_ID AND MR.RECIPIENT_GROUP_ID = (SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE GROUP_ID = MG.GROUP_ID AND USER_ID = " + loggedUserID + ")) AS UNREAD_NUM "+
                        "FROM MESSAGE_GROUP MG WHERE GROUP_ID IN (SELECT GROUP_ID FROM MESSAGE_USER_GROUP WHERE USER_ID = "+loggedUserID+") "+
                        "ORDER BY UNREAD_NUM DESC, GROUP_NAME ASC";
                    //string sqlCommand = "SELECT GROUP_ID, GROUP_NAME, CREATE_DATE, (SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE GROUP_ID = MG.GROUP_ID AND USER_ID = "+loggedUserID+" ) AS RECIPIENT_GROUP_ID, (SELECT COUNT(USER_GROUP_ID) FROM MESSAGE_USER_GROUP WHERE GROUP_ID = MG.GROUP_ID) AS MEMBER_NUM FROM MESSAGE_GROUP MG WHERE GROUP_ID IN (SELECT GROUP_ID FROM MESSAGE_USER_GROUP WHERE USER_ID = " + loggedUserID+")";
                    return db.Database.SqlQuery<MessageGroupDTO>(sqlCommand).ToList();
                }
                //return new List<MessageUserGroupDTO>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<MessageUserGroupDTO> GET_ListUserInGroup(int groupID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    string sqlCommand = "SELECT UG.USER_GROUP_ID, U.UserID AS USER_ID, UG.GROUP_ID, U.FullName AS FULL_NAME, U.UserName AS USER_NAME, R.RoleName AS ROLE_NAME, UG.ROLE_IN_GROUP FROM MESSAGE_USER_GROUP UG INNER JOIN USERS U ON UG.USER_ID = U.UserID INNER JOIN ROLES R ON U.RoleID = R.RoleID WHERE GROUP_ID = " + groupID;
                    //string sqlCommand = "SELECT UG.USER_GROUP_ID, U.UserID AS USER_ID, UG.GROUP_ID, U.FullName AS FULL_NAME, U.UserName AS USER_NAME FROM MESSAGE_USER_GROUP UG INNER JOIN USERS U ON UG.USER_ID = U.UserID WHERE GROUP_ID = " + groupID;
                    return db.Database.SqlQuery<MessageUserGroupDTO>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<MessageUserGroupDTO> GET_ListUserNotInGroup(int groupID, int loggedUserID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    /*
                    string sqlCommand = "SELECT U.UserID AS USER_ID, MUG.GROUP_ID, U.FullName AS FULL_NAME, U.UserName AS USER_NAME, R.RoleName AS ROLE_NAME " +
                        "FROM USERS U INNER JOIN MESSAGE_USER_GROUP MUG ON U.UserID = MUG.USER_ID INNER JOIN ROLES R ON R.RoleID = U.RoleID " +
                        "WHERE MUG.GROUP_ID != " + groupID + " AND U.UserID != " + loggedUserID;
                    */
                    string sqlCommand = "SELECT U.UserID USER_ID, U.UserName USER_NAME, U.FullName FULL_NAME, R.RoleName ROLE_NAME FROM USERS U INNER JOIN ROLES R ON U.RoleID = R.RoleID " +
                        "WHERE USERID NOT IN (SELECT DISTINCT U.UserID AS USER_ID " +
                        "FROM USERS U INNER JOIN MESSAGE_USER_GROUP MUG ON U.UserID = MUG.USER_ID " +
                        "WHERE MUG.GROUP_ID = " + groupID + " OR U.UserID = " + loggedUserID + ") ";
                    return db.Database.SqlQuery<MessageUserGroupDTO>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<MessageSenderDTO> GET_MessageRecordOfUsersInGroup(int groupID, int senderID, int loggedUserGroupID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {

                    string sqlCommand = "SELECT DISTINCT MS.MESSAGE_ID, SENDER_ID, (SELECT FullName FROM USERS WHERE UserID = MS.SENDER_ID) AS SENDER_NAME, MS.MESSAGE_CONTENT, MS.MESSAGE_TYPE, MS.SEND_DATE " +
                        "FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID INNER JOIN MESSAGE_USER_GROUP MUG ON MUG.USER_GROUP_ID = MR.RECIPIENT_GROUP_ID " +
                        "WHERE MUG.GROUP_ID = " + groupID + " AND (MS.SENDER_ID = " + senderID + " OR MR.RECIPIENT_GROUP_ID = " + loggedUserGroupID + ") ORDER BY SEND_DATE";

                    return db.Database.SqlQuery<MessageSenderDTO>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public int GET_MessageUnreadCountOfLoggedUser(int loggedUserID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {

                    string sqlCommand = "SELECT COUNT(MS.MESSAGE_ID) UNREAD_COUNT FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID " +
                        "WHERE MR.RECEIVE_DATE IS NULL AND (MR.RECIPIENT_ID = "+loggedUserID+" OR MR.RECIPIENT_GROUP_ID IN (SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE USER_ID = "+loggedUserID+"))";

                    return db.Database.SqlQuery<int>(sqlCommand).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region POST Data Functions
        static public bool POST_SendingMessage(string sendTarget, int senderID, int? recipientID, int[] arrGroupUserID, string messageType, string messageContent, string deviceNameIP)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    // Insert sent message to MESSAGE_SENDER table
                    MESSAGE_SENDER insertMessageSender = new MESSAGE_SENDER
                    {
                        SENDER_ID = senderID,
                        MESSAGE_TYPE = messageType,
                        MESSAGE_CONTENT = messageContent,
                        DEVICE_NAME_IP = deviceNameIP,
                        SEND_DATE = DateTime.Now
                    };
                    //insertMessageSender.RECIPIENT_ID = recipientID;
                    db.MESSAGE_SENDER.Add(insertMessageSender);
                    // Insert recipient to MESSAGE_RECIPIENT table
                    //int justInsertedMessageSender = db.MESSAGE_SENDER.Where(msg => msg.SENDER_ID == senderID).Select(msg => msg.MESSAGE_ID).SingleOrDefault();

                    if (sendTarget.Equals("group"))
                    {
                        // Send to all users in group
                        if (arrGroupUserID.Any())
                        {
                            for (int i = 0; i < arrGroupUserID.Length; i++)
                            {
                                MESSAGE_RECIPIENT insertMessageRecipient = new MESSAGE_RECIPIENT()
                                {
                                    MESSAGE_ID = insertMessageSender.MESSAGE_ID,
                                    //RECIPIENT_ID = recipientID,
                                    RECIPIENT_GROUP_ID = arrGroupUserID[i],
                                };
                                db.MESSAGE_RECIPIENT.Add(insertMessageRecipient);
                            }
                        }
                    }
                    else
                    {
                        // Send to single user
                        if (recipientID.HasValue)
                        {
                            MESSAGE_RECIPIENT insertMessageRecipient = new MESSAGE_RECIPIENT()
                            {
                                MESSAGE_ID = insertMessageSender.MESSAGE_ID,
                                RECIPIENT_ID = recipientID,
                            };
                            db.MESSAGE_RECIPIENT.Add(insertMessageRecipient);
                        }
                    }


                    db.SaveChanges();

                    //db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public bool POST_SendMessageBetween2User(int senderID, int recipientID, string messageType, string messageContent, string deviceNameIP)
        {
            try
            {

                /*using (ATEVersionContext db = new ATEVersionContext())
                {
                    // Insert sent message to MESSAGE_SENDER table
                    //
                    MESSAGE_SENDER insertMessageSender = new MESSAGE_SENDER
                    {
                        SENDER_ID = senderID,
                        MESSAGE_TYPE = messageType,
                        MESSAGE_CONTENT = messageContent,
                        DEVICE_NAME_IP = deviceNameIP,
                        SEND_DATE = DateTime.Now
                    };
                    db.MESSAGE_SENDER.Add(insertMessageSender);
                    // Insert recipient to MESSAGE_RECIPIENT table                                        
                    // 
                    MESSAGE_RECIPIENT insertMessageRecipient = new MESSAGE_RECIPIENT()
                    {
                        MESSAGE_ID = insertMessageSender.MESSAGE_ID,
                        RECIPIENT_ID = recipientID,
                    };
                    db.MESSAGE_RECIPIENT.Add(insertMessageRecipient);
                    db.SaveChanges();

                }*/

                using (ATEVersionContext db = new ATEVersionContext())
                {
                    //
                    string sqlQuery = "INSERT INTO dbo.MESSAGE_SENDER(SENDER_ID, MESSAGE_TYPE, MESSAGE_CONTENT, DEVICE_NAME_IP, SEND_DATE) " +
                        "OUTPUT INSERTED.MESSAGE_ID " +
                        "VALUES(" + senderID + ", N'" + messageType + "', N'" + messageContent + "', N'" + deviceNameIP + "', GETDATE()) ";
                    SqlConnection parsedSQLConnection = db.Database.Connection as SqlConnection;
                    SqlCommand sqlCommand = new SqlCommand(sqlQuery, parsedSQLConnection);
                    //
                    parsedSQLConnection.Open();
                    int justInsertedMessageID = (int)sqlCommand.ExecuteScalar();
                    parsedSQLConnection.Close();
                    //
                    // Insert recipient to MESSAGE_RECIPIENT table                                        
                    // 
                    MESSAGE_RECIPIENT insertMessageRecipient = new MESSAGE_RECIPIENT()
                    {
                        MESSAGE_ID = justInsertedMessageID,
                        RECIPIENT_ID = recipientID,
                    };
                    db.MESSAGE_RECIPIENT.Add(insertMessageRecipient);
                    db.SaveChanges();

                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public bool POST_UpdateUnreadMessagesBetween2User(int loggedUserID, int recipientID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    string sqlCommand = "UPDATE MESSAGE_RECIPIENT SET RECEIVE_DATE = '" + DateTime.Now.ToString() + "' WHERE RECEIVE_ID IN (SELECT RECEIVE_ID FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID WHERE MR.RECIPIENT_ID = "+loggedUserID+" AND MS.SENDER_ID = "+recipientID+" AND MR.RECEIVE_DATE IS NULL)";
                    db.Database.ExecuteSqlCommand(sqlCommand);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public bool POST_SendMessageToGroup(int senderID, int[] arrGroupUserIDExceptSender, string messageType, string messageContent, string deviceNameIP)
        {
            try
            {
                /*using (ATEVersionContext db = new ATEVersionContext())
                {
                    // Insert sent message to MESSAGE_SENDER table
                    //
                    MESSAGE_SENDER insertMessageSender = new MESSAGE_SENDER
                    {
                        SENDER_ID = senderID,
                        MESSAGE_TYPE = messageType,
                        MESSAGE_CONTENT = messageContent,
                        DEVICE_NAME_IP = deviceNameIP,
                        SEND_DATE = DateTime.Now
                    };
                    db.MESSAGE_SENDER.Add(insertMessageSender);
                    // Insert recipient to MESSAGE_RECIPIENT table                                        
                    // Send to all users in group
                    if (arrGroupUserIDExceptSender.Any())
                    {
                        for (int i = 0; i < arrGroupUserIDExceptSender.Length; i++)
                        {
                            MESSAGE_RECIPIENT insertMessageRecipient = new MESSAGE_RECIPIENT()
                            {
                                MESSAGE_ID = insertMessageSender.MESSAGE_ID,
                                //RECIPIENT_ID = recipientID,
                                RECIPIENT_GROUP_ID = arrGroupUserIDExceptSender[i],
                            };
                            db.MESSAGE_RECIPIENT.Add(insertMessageRecipient);
                        }
                    }
                    db.SaveChanges();

                }*/

                using (ATEVersionContext db = new ATEVersionContext())
                {
                    //
                    string sqlQuery = "INSERT INTO dbo.MESSAGE_SENDER(SENDER_ID, MESSAGE_TYPE, MESSAGE_CONTENT, DEVICE_NAME_IP, SEND_DATE) " +
                        "OUTPUT INSERTED.MESSAGE_ID " +
                        "VALUES(" + senderID + ", N'" + messageType + "', N'" + messageContent + "', N'" + deviceNameIP + "', GETDATE()) ";
                    SqlConnection parsedSQLConnection = db.Database.Connection as SqlConnection;
                    SqlCommand sqlCommand = new SqlCommand(sqlQuery, parsedSQLConnection);
                    //
                    parsedSQLConnection.Open();
                    int justInsertedMessageID = (int)sqlCommand.ExecuteScalar();
                    parsedSQLConnection.Close();
                    //
                    // Insert recipient to MESSAGE_RECIPIENT table                                        
                    // Send to all users in group
                    if (arrGroupUserIDExceptSender.Any())
                    {
                        for (int i = 0; i < arrGroupUserIDExceptSender.Length; i++)
                        {
                            MESSAGE_RECIPIENT insertMessageRecipient = new MESSAGE_RECIPIENT()
                            {
                                MESSAGE_ID = justInsertedMessageID,
                                //RECIPIENT_ID = recipientID,
                                RECIPIENT_GROUP_ID = arrGroupUserIDExceptSender[i],
                            };
                            db.MESSAGE_RECIPIENT.Add(insertMessageRecipient);
                        }
                    }
                    db.SaveChanges();

                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public bool POST_UpdateUnreadMessagesInGroup(int groupID, int loggedUserID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    string sqlCommand = "UPDATE MESSAGE_RECIPIENT SET RECEIVE_DATE = '" + DateTime.Now.ToString() + "' WHERE RECEIVE_ID IN (" +
                        "SELECT RECEIVE_ID FROM MESSAGE_SENDER MS INNER JOIN MESSAGE_RECIPIENT MR ON MS.MESSAGE_ID = MR.MESSAGE_ID INNER JOIN MESSAGE_USER_GROUP MUG ON MUG.USER_GROUP_ID = MR.RECIPIENT_GROUP_ID " +
                        "WHERE MR.RECEIVE_DATE IS NULL AND MUG.GROUP_ID = " + groupID + " AND MR.RECIPIENT_GROUP_ID = (SELECT USER_GROUP_ID FROM MESSAGE_USER_GROUP WHERE GROUP_ID = " + groupID + " AND USER_ID = " + loggedUserID + "))";
                    
                    db.Database.ExecuteSqlCommand(sqlCommand);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public bool POST_CreateNewMessageGroup(int loggedUserID, int[] arrGroupUserIDExceptSender, string groupName)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {                    
                    MESSAGE_GROUP newMsgGroup = new MESSAGE_GROUP()
                    {
                        GROUP_NAME = groupName,
                        CREATE_BY = loggedUserID,
                        CREATE_DATE = DateTime.Now,
                        IS_ACTIVE = true,
                    };                    
                    db.MESSAGE_GROUP.Add(newMsgGroup);                                            

                    MESSAGE_USER_GROUP userToGroup = new MESSAGE_USER_GROUP()
                        {
                            GROUP_ID = newMsgGroup.GROUP_ID,
                            USER_ID = loggedUserID,
                            ROLE_IN_GROUP = "manager",
                            CREATE_DATE = DateTime.Now,
                            IS_ACTIVE = true,
                        };
                    db.MESSAGE_USER_GROUP.Add(userToGroup);
                    
                    for (int i = 0; i < arrGroupUserIDExceptSender.Length; i++)
                    {
                        int currentUserID = arrGroupUserIDExceptSender[i];                        
                        userToGroup = new MESSAGE_USER_GROUP()
                        {
                            GROUP_ID = newMsgGroup.GROUP_ID,
                            USER_ID = currentUserID,
                            ROLE_IN_GROUP = "member",
                            CREATE_DATE = DateTime.Now,
                            IS_ACTIVE = true,
                        };
                        db.MESSAGE_USER_GROUP.Add(userToGroup);                        
                    }
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        static public bool POST_AddNewMemberToGroup(int groupID, int[] arrGroupUserIDExceptLoggedUser)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    MESSAGE_USER_GROUP userToGroup = new MESSAGE_USER_GROUP();
                    for (int i = 0; i < arrGroupUserIDExceptLoggedUser.Length; i++)
                    {
                        int currentUserID = arrGroupUserIDExceptLoggedUser[i];
                        userToGroup = new MESSAGE_USER_GROUP()
                        {
                            GROUP_ID = groupID,
                            USER_ID = currentUserID,
                            ROLE_IN_GROUP = "member",
                            CREATE_DATE = DateTime.Now,
                            IS_ACTIVE = true,
                        };
                        db.MESSAGE_USER_GROUP.Add(userToGroup);
                    }
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}