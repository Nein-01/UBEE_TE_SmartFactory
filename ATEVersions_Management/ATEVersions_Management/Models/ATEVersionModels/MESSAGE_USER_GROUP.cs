namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MESSAGE_USER_GROUP
    {
        [Key]
        public int USER_GROUP_ID { get; set; }

        public int USER_ID { get; set; }

        public int GROUP_ID { get; set; }

        [StringLength(30)]
        public string ROLE_IN_GROUP { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public bool? IS_ACTIVE { get; set; }

        public virtual MESSAGE_GROUP MESSAGE_GROUP { get; set; }

        public virtual USER USER { get; set; }
    }
}
