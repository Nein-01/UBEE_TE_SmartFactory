namespace ATEVersions_Management.Models.ProductionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TE_TEST_ITEM_DATA
    {
        public long ID { get; set; }

        [StringLength(254)]
        public string VIRTUAL_SN { get; set; }

        [StringLength(254)]
        public string WORK_DATE { get; set; }

        public int? WORK_SECTION { get; set; }

        [StringLength(254)]
        public string FULL_WORK_DATE { get; set; }

        [StringLength(254)]
        public string CFT { get; set; }

        [StringLength(254)]
        public string PROJECT { get; set; }

        [StringLength(254)]
        public string MODEL { get; set; }

        [StringLength(254)]
        public string LINE { get; set; }

        [StringLength(254)]
        public string STATION { get; set; }

        [StringLength(254)]
        public string ATE { get; set; }

        [StringLength(254)]
        public string FIXTURE_CODE { get; set; }

        [StringLength(254)]
        public string CARRIER_CODE { get; set; }

        public int? TEST_MODE { get; set; }

        [StringLength(254)]
        public string TEST_VERSION { get; set; }

        [StringLength(254)]
        public string MO { get; set; }

        [StringLength(254)]
        public string BOARD_SN { get; set; }

        [StringLength(254)]
        public string SN { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? START_TIME { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? END_TIME { get; set; }

        public int? STATUS { get; set; }

        public int? STEP_NUMBER { get; set; }

        [StringLength(254)]
        public string TEST_ITEM { get; set; }

        [StringLength(254)]
        public string VALUE_TYPE { get; set; }

        [StringLength(254)]
        public string VALUE { get; set; }

        [StringLength(254)]
        public string LSL { get; set; }

        [StringLength(254)]
        public string USL { get; set; }
    }
}
