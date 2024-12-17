namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EMPLOYEES")]
    public partial class EMPLOYEE
    {
        [Key]
        [StringLength(8)]
        public string CARD_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string NAME { get; set; }

        [StringLength(32)]
        public string PASSWORD { get; set; }

        public bool ACTIVE { get; set; }
    }
}
