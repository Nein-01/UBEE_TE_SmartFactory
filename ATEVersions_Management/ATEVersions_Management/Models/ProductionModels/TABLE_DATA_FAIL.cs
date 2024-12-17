namespace ATEVersions_Management.Models.ProductionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TABLE_DATA_FAIL
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string MODEL_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string STATION { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string SN { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string MAC { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string MO { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string VERSION { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(100)]
        public string ERROR_CODE { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(254)]
        public string ERROR_DETAIL { get; set; }

        [StringLength(254)]
        public string FAIL_VALUE { get; set; }

        [Key]
        [Column(Order = 8)]
        public DateTime WORK_DATE { get; set; }
    }
}
