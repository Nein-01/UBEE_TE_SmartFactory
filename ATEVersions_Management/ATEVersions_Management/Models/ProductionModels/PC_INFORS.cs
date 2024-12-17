namespace ATEVersions_Management.Models.ProductionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PC_INFORS
    {
        public long ID { get; set; }

        [StringLength(30)]
        public string ATE { get; set; }

        [StringLength(30)]
        public string IP_ADDRESS { get; set; }

        [StringLength(100)]
        public string OS { get; set; }
    }
}
