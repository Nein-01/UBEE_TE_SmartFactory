using ATEVersions_Management.Models.DTOModels.TestMonitorDTOs;
using ATEVersions_Management.Models.TestMonitorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DAOModels.TestMonitorDAOs
{
    public class MachineChangeRecordDAO
    {
        static private readonly TestMonitorDBContext db = new TestMonitorDBContext();
        static public List<MachineChangeRecordDTO> GetMachineChangeRecordByHostName(string hostname) 
        {
            return (from machine in db.MACHINE_INFORMATION_CHANGE_RECORD
                    where machine.HOST_NAME.Trim().ToLower() == hostname.Trim().ToLower()
                    orderby machine.TIME_CHECK descending
                    select new MachineChangeRecordDTO
                    {
                        HOST_NAME = machine.HOST_NAME,
                        ISSUE = machine.ISSUE,
                        TIME_CHECK = machine.TIME_CHECK,
                    }).ToList();
        }
    }
}