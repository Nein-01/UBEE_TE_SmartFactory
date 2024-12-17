using ATEVersions_Management.Models.DAOModels.ATEVersionDAOs;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.DTOModels.ATEVersionDTOs;
using ATEVersions_Management.Models.HelperModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace ATEVersions_Management.Controllers
{
    public class MessageSystemController : Controller
    {
        #region ====== Views Transition Functions ======
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _PartSectionMessageSystem()
        {

            return PartialView();
        }
        #endregion
        #region ====== AJAX Request Functions ======
        //
        public JsonResult GET_ListAllUser()
        {
            try
            {
                List<UserDTO> listAllUser = MessageSystemDAO.GET_ListAllUser();
                return Json(listAllUser, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n"+ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ListUserExceptLoggedUser(int loggedUserID)
        {
            try
            {
                List<UserDTO> UserExceptLoggedUser = MessageSystemDAO.GET_ListUserExceptLoggedUser(loggedUserID);
                return Json(UserExceptLoggedUser, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ListSingleRecipientOfLoggedUser(int loggedUserID)
        {
            try
            {
                List<MessageInvidualDTO> listSingleRecipientLoggedUser = MessageSystemDAO.GET_ListSingleRecipientOfLoggedUser(loggedUserID);
                return Json(listSingleRecipientLoggedUser, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_MessageRecordBetween2Users(int senderID, int recipientID)
        {
            try
            {
                List<MessageSenderDTO> messageRecordBetween2Users = MessageSystemDAO.GET_MessageRecordBetween2Users(senderID, recipientID);
                return Json(messageRecordBetween2Users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ListGroupContainLoggedUser(int loggedUserID)
        {
            try
            {
                List<MessageGroupDTO> listGroupContainLoggedUser = MessageSystemDAO.GET_ListGroupContainLoggedUser(loggedUserID);
                return Json(listGroupContainLoggedUser, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_MessageRecordOfUsersInGroup(int groupID, int senderID, int loggedUserGroupID)
        {
            try
            {
                List<MessageSenderDTO> messageRecordOfUsersInGroup = MessageSystemDAO.GET_MessageRecordOfUsersInGroup(groupID, senderID, loggedUserGroupID);
                return Json(messageRecordOfUsersInGroup, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ListUserInGroup(int groupID)
        {
            try
            {
                List<MessageUserGroupDTO> listUserInGroup = MessageSystemDAO.GET_ListUserInGroup(groupID);
                return Json(listUserInGroup, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ListUserNotInGroup(int groupID, int loggedUserID)
        {
            try
            {
                List<MessageUserGroupDTO> listUserNotInGroup = MessageSystemDAO.GET_ListUserNotInGroup(groupID, loggedUserID);
                return Json(listUserNotInGroup, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_MessageUnreadCountOfLoggedUser(int loggedUserID)
        {
            try
            {
                int iUnreadCount = MessageSystemDAO.GET_MessageUnreadCountOfLoggedUser(loggedUserID);
                return Json(iUnreadCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        //
        [HttpPost]
        public JsonResult POST_SendMessageBetween2User(int senderID, string senderUserName, int recipientID, string recipientUserName, string messageType, string messageContent, HttpPostedFileBase messageFile)
        {
            try
            {
                string deviceNameIP = HttpContext.Request.UserHostAddress;

                if (messageType == "text")
                {
                    return Json(MessageSystemDAO.POST_SendMessageBetween2User(senderID, recipientID, messageType, messageContent, deviceNameIP), JsonRequestBehavior.AllowGet);
                }
                if (messageType == "file" || messageType == "img")
                {                    
                    if (messageFile != null)
                    {                        
                        string fileSavedDir = "/Client_Data/MessageSystemFiles/Single/" + senderUserName + "/" + recipientUserName + "/";
                        string serverDirLocale = Server.MapPath("~" + fileSavedDir);                        
                        string fileSavedFullPath = serverDirLocale + messageFile.FileName;
                        
                        if (!System.IO.Directory.Exists(serverDirLocale))
                        {
                            System.IO.Directory.CreateDirectory(serverDirLocale);
                        }
                        messageFile.SaveAs(fileSavedFullPath);
                        messageContent = fileSavedDir + messageFile.FileName;
                        //
                        return Json(MessageSystemDAO.POST_SendMessageBetween2User(senderID, recipientID, messageType, messageContent, deviceNameIP), JsonRequestBehavior.AllowGet);
                    }                                                         
                }               
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult POST_UpdateUnreadMessagesBetween2User(int loggedUserID, int recipientID)
        {
            try
            {                
                return Json(MessageSystemDAO.POST_UpdateUnreadMessagesBetween2User(loggedUserID, recipientID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult POST_SendMessageToGroup(int senderID, string senderUserName, string groupName, string strArrGroupUserIDExceptSender, string messageType, string messageContent, HttpPostedFileBase messageFile)
        {
            try
            {
                string deviceNameIP = HttpContext.Request.UserHostAddress;
                int[] arrGroupUserIDExceptSender = JsonConvert.DeserializeObject<int[]>(strArrGroupUserIDExceptSender);
                //
                if (messageType == "text")
                {
                    return Json(MessageSystemDAO.POST_SendMessageToGroup(senderID, arrGroupUserIDExceptSender, messageType, messageContent, deviceNameIP), JsonRequestBehavior.AllowGet);
                }
                if (messageType == "file" || messageType == "img")
                {
                    if (messageFile != null)
                    {                        
                        string fileSavedDir = "/Client_Data/MessageSystemFiles/Group/" + groupName + "/" + senderUserName + "/" ;
                        string serverDirLocale = Server.MapPath("~" + fileSavedDir);                        
                        string fileSavedFullPath = serverDirLocale + messageFile.FileName;

                        if (!System.IO.Directory.Exists(serverDirLocale))
                        {
                            System.IO.Directory.CreateDirectory(serverDirLocale);
                        }

                        messageFile.SaveAs(fileSavedFullPath);
                        messageContent = fileSavedDir + messageFile.FileName;
                        //
                        return Json(MessageSystemDAO.POST_SendMessageToGroup(senderID, arrGroupUserIDExceptSender, messageType, messageContent, deviceNameIP), JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(false, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult POST_UpdateUnreadMessagesInGroup(int groupID, int loggedUserID)
        {
            try
            {
                return Json(MessageSystemDAO.POST_UpdateUnreadMessagesInGroup(groupID, loggedUserID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult POST_CreateNewMessageGroup(int loggedUserID, string strArrGroupUserIDExceptSender, string groupName)
        {
            try
            {                
                int[] arrGroupUserIDExceptSender = JsonConvert.DeserializeObject<int[]>(strArrGroupUserIDExceptSender);                
                return Json(MessageSystemDAO.POST_CreateNewMessageGroup(loggedUserID, arrGroupUserIDExceptSender, groupName), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult POST_AddNewMemberToGroup(int groupID, string strArrGroupUserIDExceptSender)
        {
            try
            {
                int[] arrGroupUserIDExceptSender = JsonConvert.DeserializeObject<int[]>(strArrGroupUserIDExceptSender);
                return Json(MessageSystemDAO.POST_AddNewMemberToGroup(groupID, arrGroupUserIDExceptSender), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        //
        public JsonResult MQTT_SendNoticeMessage(string mqttPublishTopic, string senderInfo, string recipientInfo, string messageContent)
        {
            try
            {
                //
                string webServerErrorLogDirPath = Server.MapPath(@"~\App_Data\ErrorLogs\");
                //
                //string deviceNameIP = HttpContext.Request.UserHostAddress;
                MQTTNoticeMessageDTO mqttNoticeMessage = new MQTTNoticeMessageDTO()
                {
                    SENDER = senderInfo,
                    RECIPIENT = recipientInfo,
                    DEVICE_IP = HttpContext.Request.UserHostAddress,
                    MESSAGE_CONTENT = messageContent,
                    SEND_DATE = DateTime.Now,
                };
                string mqttMessageContent = JsonConvert.SerializeObject(mqttNoticeMessage);
                return Json(MQTTConnector.MQTT_MessageSystem_SendNoticeMessage(mqttPublishTopic, mqttMessageContent, webServerErrorLogDirPath), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region ====== Additional Funtions ======
        public ActionResult GET_DownloadMessageFileFromServer(int messageID)
        {
            try
            {
                MessageSenderDTO messageSenderByID = MessageSystemDAO.GET_MessageRecordByMessageID(messageID);
                string serverFileSavedFulPath = Server.MapPath(messageSenderByID.MESSAGE_CONTENT);
                return File(serverFileSavedFulPath, MediaTypeNames.Application.Octet, Path.GetFileName(serverFileSavedFulPath));
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ": \n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}