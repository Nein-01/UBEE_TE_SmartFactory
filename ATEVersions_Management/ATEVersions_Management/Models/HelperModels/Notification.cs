using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.HelperModels
{
    public class Notification
    {
        public string Msg { get; set; }
        public string MsgType { get; set; }
        public string Msg1s { get; set; }
        public string Msg1sType { get; set; }

        public static bool hasFlash()
        {
            if (HttpContext.Current.Application["Notification"].Equals("")) return false;
            return true;
        }
        public static void setFlash(string msg, string msg_type)
        {
            var notify = new Notification
            {
                Msg = msg,
                MsgType = msg_type
            };
            HttpContext.Current.Application["Notification"] = notify;
        }
        public static void setFlash1s(string msg1s, string msg1s_type)
        {
            var notify = new Notification
            {
                Msg1s = msg1s,
                Msg1sType = msg1s_type
            };
            HttpContext.Current.Application["Notification"] = notify;
        }
        public static Notification getFlash()
        {
            var notify = (Notification)HttpContext.Current.Application["Notification"];
            HttpContext.Current.Application["Notification"] = "";
            return notify;
        }
    }
}