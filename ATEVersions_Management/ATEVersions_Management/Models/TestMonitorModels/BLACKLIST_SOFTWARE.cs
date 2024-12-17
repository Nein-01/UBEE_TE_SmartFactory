namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BLACKLIST_SOFTWARE
    {
        [Key]
        [StringLength(50)]
        public string SOFTWARE_NAME { get; set; }

        [StringLength(50)]
        public string NOTE { get; set; }

        [StringLength(50)]
        public string PROCESS_NAME { get; set; }
    }
}
