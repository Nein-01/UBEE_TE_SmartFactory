namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MACHINE_INFORMATION_CHANGE_RECORD
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string HOST_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        public string ISSUE { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime TIME_CHECK { get; set; }
    }
}
