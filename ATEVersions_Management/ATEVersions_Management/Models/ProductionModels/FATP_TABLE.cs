namespace ATEVersions_Management.Models.ProductionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FATP_TABLE
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string ATE_PC { get; set; }

        [StringLength(50)]
        public string ATE_IP { get; set; }

        [StringLength(50)]
        public string ATE_MAC { get; set; }

        [StringLength(50)]
        public string LINE { get; set; }

        [StringLength(50)]
        public string STATION { get; set; }

        [StringLength(50)]
        public string MODEL { get; set; }

        public DateTime? POST_DATE { get; set; }

        public int? PASS_NUM { get; set; }

        public int? FAIL_NUM { get; set; }

        public int? FAIL_NUM_BUFFER { get; set; }

        public string ERROR_LIST { get; set; }

        public string COUNTERS { get; set; }

        public string EQUIPMENTS { get; set; }

        public DateTime? PC_DATE { get; set; }

        public string CPK_RESULTS { get; set; }

        public string CPK_LOWEST_ITEM { get; set; }

        public string LOSS_MEASUREMENT { get; set; }
        public string OP_ID { get; set; }
        public string OP_RECORD { get; set; }
    }
}
