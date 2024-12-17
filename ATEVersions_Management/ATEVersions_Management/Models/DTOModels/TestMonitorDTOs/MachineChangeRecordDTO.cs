using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels.TestMonitorDTOs
{
    public class MachineChangeRecordDTO
    {
        // Database data
        [Column(Order = 0)]
        [StringLength(15)]
        public string HOST_NAME { get; set; }
        
        [Column(Order = 1)]
        public string ISSUE { get; set; }
        
        [Column(Order = 2)]
        public DateTime TIME_CHECK { get; set; }

        // Additional Data
        public string STR_TIME_CHECK
        {
            get
            {
                return TIME_CHECK.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }
    }
}