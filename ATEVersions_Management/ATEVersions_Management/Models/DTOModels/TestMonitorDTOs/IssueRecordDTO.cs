using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels.TestMonitorDTOs
{
    public class IssueRecordDTO
    {
        [Column(Order = 0)]
        [StringLength(15)]
        public string HOST_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string IP { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(17)]
        public string MAC { get; set; }

        [Key]
        [Column(Order = 3)]
        public string ISSUE { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime DETECT_TIME { get; set; }

        public DateTime? DEAL_TIME { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(10)]
        public string STATUS { get; set; }

        // Additional Data 
        public string STR_DETECT_TIME
        {
            get
            {
                return DETECT_TIME.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }
        public string STR_DEAL_TIME
        {
            get
            {
                if (DEAL_TIME.HasValue)
                {
                    return DEAL_TIME.Value.ToString("yyyy/MM/dd HH:mm:ss");
                }
                return "";
            }
        }
    }
}