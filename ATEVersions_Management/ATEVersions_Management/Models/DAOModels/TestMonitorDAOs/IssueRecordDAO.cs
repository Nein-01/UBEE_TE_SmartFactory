using ATEVersions_Management.Models.DTOModels.TestMonitorDTOs;
using ATEVersions_Management.Models.TestMonitorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DAOModels.TestMonitorDAOs
{
    public class IssueRecordDAO
    {
        static readonly TestMonitorDBContext db = new TestMonitorDBContext();
        static public List<IssueRecordDTO> GetIssueRecordByHostName(string hostname)
        {
            //DateTime today = DateTime.Now.Date;
            return (from issues in db.ISSUE_RECORD
                    where issues.HOST_NAME.Trim().ToLower() == hostname.Trim().ToLower()
                    orderby issues.DETECT_TIME descending
                    select new IssueRecordDTO
                    {
                        HOST_NAME = issues.HOST_NAME,
                        IP = issues.IP,
                        MAC = issues.MAC,
                        ISSUE = issues.ISSUE,
                        DETECT_TIME = issues.DETECT_TIME,
                        DEAL_TIME = issues.DEAL_TIME,
                        STATUS = issues.STATUS
                    }).Distinct().ToList();
        }
    }
}