namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MACHINE_INFORMATION
    {
        [Key]
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
    }
}
