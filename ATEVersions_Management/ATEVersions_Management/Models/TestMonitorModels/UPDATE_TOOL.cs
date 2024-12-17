namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UPDATE_TOOL
    {
        [Key]
        [StringLength(64)]
        public string NAME { get; set; }

        [Required]
        public byte[] DATA { get; set; }

        [Required]
        [StringLength(32)]
        public string CHECKSUM { get; set; }
    }
}
