namespace ATEVersions_Management.Models.ProductionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TEST_LOGS_DATA
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string MODEL_NAME { get; set; }

        [StringLength(20)]
        public string MO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string TSN { get; set; }

        [StringLength(254)]
        public string RSN { get; set; }

        [StringLength(20)]
        public string GROUP_NAME { get; set; }

        public DateTime? TIME_START { get; set; }

        public DateTime? TIME_END { get; set; }

        public double? TEST_TIME { get; set; }

        [StringLength(20)]
        public string ERROR_CODE { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(350)]
        public string LOGS { get; set; }

        [StringLength(150)]
        public string EQUIPMENT { get; set; }

        [StringLength(50)]
        public string STATION_NAME { get; set; }

        [StringLength(254)]
        public string MAC { get; set; }

        [StringLength(50)]
        public string PROGRAM_VERSION { get; set; }

        [StringLength(1000)]
        public string REASON { get; set; }

        [StringLength(100)]
        public string OWNER { get; set; }

        [StringLength(25)]
        public string BUILD_TIME { get; set; }
    }
}
