namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EQUIPMENT_MANAGEMENT
    {
        [Key]
        [StringLength(32)]
        public string SN { get; set; }

        [Required]
        [StringLength(64)]
        public string NAME { get; set; }

        [Required]
        [StringLength(5)]
        public string FACTORY { get; set; }

        [Required]
        [StringLength(5)]
        public string FLOOR { get; set; }

        [StringLength(5)]
        public string LINE { get; set; }

        [StringLength(5)]
        public string STATION { get; set; }

        [StringLength(15)]
        public string HOST_NAME { get; set; }

        [StringLength(15)]
        public string TEST_MODEL { get; set; }

        public DateTime? TIME { get; set; }

        [StringLength(254)]
        public string CONTROL_NO { get; set; }
    }
}
