namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class STATION_INFORMATION
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string PRODUCT_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string MACHINE_NAME { get; set; }

        public bool? STATUS { get; set; }

        [StringLength(1024)]
        public string ROOT_CAUSE { get; set; }

        [StringLength(1024)]
        public string UNLOCK_BY { get; set; }
    }
}
