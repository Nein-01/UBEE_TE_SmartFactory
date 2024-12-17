namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ESD_INFORMATION
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(5)]
        public string FACTORY { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string STAGE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string LINE { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(3)]
        public string STATION { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(3)]
        public string DEVICE { get; set; }

        public DateTime TIME { get; set; }

        public bool POWER_STATUS { get; set; }

        public bool ESD_STATUS { get; set; }
    }
}
