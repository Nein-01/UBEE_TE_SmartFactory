using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels.ATEVersionDTOs
{
    public class MessageSystemDTO
    {

    }
    public class MessageInvidualDTO
    {
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string FULL_NAME { get; set; }
        public string ROLE_NAME { get; set; }
        public int UNREAD_NUM { get; set; }
    }
    public class MessageGroupDTO
    {
        public int GROUP_ID { get; set; }
        [StringLength(200)]
        public string GROUP_NAME { get; set; }
        public int? CREATE_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public int MEMBER_NUM { get; set; }
        public int RECIPIENT_GROUP_ID { get; set; }
        public int UNREAD_NUM { get; set; }
    }
    public class MessageUserGroupDTO
    {
        public int USER_GROUP_ID { get; set; }
        public int GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string FULL_NAME { get; set; }
        public string ROLE_NAME { get; set; }
        [StringLength(30)]
        public string ROLE_IN_GROUP { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public bool? IS_ACTIVE { get; set; }
    }
    public class MessageSenderDTO
    {
        public int MESSAGE_ID { get; set; }
        public int? MESSAGE_PARENT_ID { get; set; }
        public int SENDER_ID { get; set; }
        public string SENDER_NAME { get; set; }
        [StringLength(100)]
        public string MESSAGE_TYPE { get; set; }
        public string MESSAGE_CONTENT { get; set; }
        [StringLength(100)]
        public string DEVICE_NAME_IP { get; set; }
        public DateTime? SEND_DATE { get; set; }
        public int? RECIPIENT_ID { get; set; }
        public string RECIPIENT_NAME { get; set; }
        public int? RECIPIENT_GROUP_ID { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }
    }
    public class MessageRecipientDTO
    {
        public int RECEIVE_ID { get; set; }
        public int MESSAGE_ID { get; set; }
        public int? RECIPIENT_ID { get; set; }
        public int? RECIPIENT_GROUP_ID { get; set; }
        public DateTime? RECEIVE_DATE { get; set; }

    }
    public class MQTTNoticeMessageDTO
    {
        public string SENDER { get; set; }
        public string RECIPIENT { get; set; }
        public string DEVICE_IP { get; set; }
        public string MESSAGE_CONTENT { get; set; }
        public DateTime SEND_DATE { get; set; }
    }
}