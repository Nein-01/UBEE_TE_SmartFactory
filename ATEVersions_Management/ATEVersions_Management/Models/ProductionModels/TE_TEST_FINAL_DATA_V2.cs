namespace ATEVersions_Management.Models.ProductionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TE_TEST_FINAL_DATA_V2
    {
        public long ID { get; set; }

        [StringLength(254)]
        public string FACTORY { get; set; }

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
        public string WORK_DATE { get; set; }

        [StringLength(254)]
        public string WORK_SECTION { get; set; }

        [StringLength(254)]
        public string FULL_WORK_DATE { get; set; }

        [StringLength(254)]
        public string TEST_ITEM { get; set; }

        public string VALUES { get; set; }
    }
}
