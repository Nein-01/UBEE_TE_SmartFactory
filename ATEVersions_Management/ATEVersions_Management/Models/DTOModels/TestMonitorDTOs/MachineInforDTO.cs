using ATEVersions_Management.Models.DAOModels.TestMonitorDAOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels.TestMonitorDTOs
{
    public class MachineInforDTO
    {
        // MACHINE_INFORMATION table data fields
        [StringLength(15)]
        public string LINE 
        {
            get
            {
                return HOST_NAME.Substring(0, 3);
            }
        }

        [StringLength(15)]
        public string HOST_NAME { get; set; }

        [Required]
        [StringLength(15)]
        public string IP { get; set; }

        [Required]
        [StringLength(17)]
        public string MAC { get; set; }

        [StringLength(64)]
        public string OS_NAME { get; set; }

        [StringLength(5)]
        public string OS_VERSION { get; set; }

        [StringLength(64)]
        public string MAIN { get; set; }

        [StringLength(64)]
        public string CPU { get; set; }

        public double? CPU_TEMP { get; set; }

        [StringLength(10)]
        public string RAM { get; set; }

        public string HARD_DRIVE { get; set; }

        [StringLength(11)]
        public string SAVE_ENERGY_MODE { get; set; }

        public DateTime TIME_CHECK { get; set; }

        [Required]
        [StringLength(36)]
        public string TOOL_VERSION { get; set; }

        [StringLength(10)]
        public string STATUS { get; set; }

        // MACHINE_INFORMATION addition data
        public int ActiveStatus
        {
            get
            {
                DateTime timeNow = DateTime.Now;
                TimeSpan timeSpanDataSent = timeNow - TIME_CHECK;
                double timeRange = Math.Round(timeSpanDataSent.TotalMinutes, 2);
                if(timeRange >= 10 && timeRange < 30)
                {
                    return 1;
                }
                if(timeRange > 30)
                {
                    return 2;
                }               
                return 0;
            }
        }

        public double WorkTime
        {
            get
            {
                string nowTime = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
                TimeSpan timeRange = TIME_CHECK - DateTime.Parse(nowTime);

                if(timeRange.TotalMinutes > 0)
                {
                    return Math.Round(timeRange.TotalHours,2);
                }

                return 0;
            }
        }
        public double IdleTime
        {
            get
            {
                string nowTime = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
                TimeSpan timeRange = TIME_CHECK - DateTime.Parse(nowTime);

                if(timeRange.TotalSeconds < 0)
                {
                    timeRange = DateTime.Now - DateTime.Parse(nowTime);
                }
                else
                {
                    timeRange = DateTime.Now - TIME_CHECK;
                }

                if(timeRange.TotalMinutes > 30)
                {
                    return Math.Round(timeRange.TotalHours, 2);
                }

                return 0;
            }
        }
        public bool IsIPDup { get; set; }
    }
    public class MachineAirDTO
    {
        public string LINE
        {
            get
            {
                return HOST_NAME.Substring(0, 3);
            }
        }
        public string HOST_NAME { get; set; }
        public string IP { get; set; }
        public int AIR_STATUS { get; set; }
    }
}