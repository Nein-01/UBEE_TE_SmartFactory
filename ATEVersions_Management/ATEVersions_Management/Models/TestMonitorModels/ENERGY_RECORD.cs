namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENERGY_RECORD
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string HOST_NAME { get; set; }

        [Required]
        [StringLength(15)]
        public string IP { get; set; }

        [Required]
        [StringLength(17)]
        public string MAC { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime DATE { get; set; }

        public double ACTIVE_TIME { get; set; }

        public double STANDBY_TIME { get; set; }

        public double SHUTDOWN_TIME { get; set; }

        public DateTime TIME_CHECK { get; set; }
    }
}
