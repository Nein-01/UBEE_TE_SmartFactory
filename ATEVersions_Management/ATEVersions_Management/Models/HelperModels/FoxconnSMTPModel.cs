using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATEVersions_Management.FoxconnSMTP;
namespace ATEVersions_Management.Models.HelperModels
{
    public class FoxconnSMTPModel
    {
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        static public bool Foxconn_SMTP_SendMail(string toMail, string fromMail, string cc, string subject, string message)
        {
            try
            {
                SmtpServiceSoapClient foxconnSMTPClient = new SmtpServiceSoapClient();
                return foxconnSMTPClient.WMSendMail(toMail, fromMail, cc, subject, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}