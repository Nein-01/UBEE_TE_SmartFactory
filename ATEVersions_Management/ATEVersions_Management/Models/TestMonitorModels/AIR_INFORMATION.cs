namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AIR_INFORMATION
    {
        [Key]
        [StringLength(15)]
        public string HOST_NAME { get; set; }

        public DateTime? TIME_CHECK { get; set; }

        public byte? AIR_STATUS { get; set; }

        [StringLength(254)]
        public string IP { get; set; }

        [StringLength(254)]
        public string MAC { get; set; }
    }
}
