using ATEVersions_Management.Models.DTOModels.TestMonitorDTOs;
using ATEVersions_Management.Models.TestMonitorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DAOModels.TestMonitorDAOs
{
    public class MachineInforDAO
    {
        static private TestMonitorDBContext db = new TestMonitorDBContext();
        static public List<MachineInforDTO> GetTodayAllMachineInfor()
        {
            DateTime today = DateTime.Now.AddDays(-30).Date;

            return (from machine in db.MACHINE_INFORMATION
                    where machine.TIME_CHECK >= today
                    orderby machine.HOST_NAME
                    select new MachineInforDTO
                    {
                        HOST_NAME = machine.HOST_NAME,
                        IP = machine.IP,
                        MAC = machine.MAC,
                        OS_NAME = machine.OS_NAME,
                        OS_VERSION = machine.OS_VERSION,
                        MAIN = machine.MAIN,
                        CPU = machine.CPU,
                        CPU_TEMP = machine.CPU_TEMP,
                        RAM = machine.RAM,
                        HARD_DRIVE = machine.HARD_DRIVE,
                        SAVE_ENERGY_MODE = machine.SAVE_ENERGY_MODE,
                        TIME_CHECK = machine.TIME_CHECK,
                        TOOL_VERSION = machine.TOOL_VERSION,
                        STATUS = machine.STATUS,
                    }).ToList();
        }
        static public List<MachineAirDTO> GetCurrentAirEnableMachine()
        {
            return (from air in db.MACHINE_INFORMATION
                    where air.SAVE_ENERGY_MODE.Contains(";AIR;")
                    orderby air.HOST_NAME
                    select new MachineAirDTO
                    {
                        HOST_NAME = air.HOST_NAME,
                        IP = air.IP,
                    }
                    ).ToList();
        }
    }
}