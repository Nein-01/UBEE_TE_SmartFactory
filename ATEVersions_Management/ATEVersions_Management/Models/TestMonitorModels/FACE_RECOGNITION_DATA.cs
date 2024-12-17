namespace ATEVersions_Management.Models.TestMonitorModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FACE_RECOGNITION_DATA
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(40)]
        public string MACHINE_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string CARD_ID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string NAME { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime TIME { get; set; }
    }
}
