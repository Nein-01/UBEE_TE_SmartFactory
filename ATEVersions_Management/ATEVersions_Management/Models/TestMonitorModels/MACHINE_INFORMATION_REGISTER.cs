namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MACHINE_INFORMATION_REGISTER
    {
        [Key]
        [StringLength(15)]
        public string HOST_NAME { get; set; }

        [Required]
        [StringLength(64)]
        public string MAIN { get; set; }

        [Required]
        [StringLength(64)]
        public string CPU { get; set; }

        [Required]
        [StringLength(10)]
        public string RAM { get; set; }

        [Required]
        public string HARD_DRIVE { get; set; }

        public DateTime TIME_REGISTER { get; set; }
    }
}
