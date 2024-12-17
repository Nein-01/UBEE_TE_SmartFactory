namespace ATEVersions_Management.Models.CPKModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CPK_TABLE
    {
        public int ID { get; set; }

        [Required]
        [StringLength(25)]
        public string MO_NUMBER { get; set; }

        [Required]
        [StringLength(25)]
        public string MODEL_NAME { get; set; }

        [Required]
        [StringLength(25)]
        public string STATION_NAME { get; set; }

        [Required]
        [StringLength(25)]
        public string PCB_SN { get; set; }

        [StringLength(25)]
        public string SSN { get; set; }

        [StringLength(25)]
        public string MAC { get; set; }

        public DateTime? DATE_TIME { get; set; }

        [StringLength(10)]
        public string STATUS { get; set; }

        public string CONTENT { get; set; }

        [StringLength(50)]
        public string ATE_PC { get; set; }
    }
}
