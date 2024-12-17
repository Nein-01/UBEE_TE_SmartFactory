namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MESSAGE_RECIPIENT
    {
        [Key]
        public int RECEIVE_ID { get; set; }

        public int MESSAGE_ID { get; set; }

        public int? RECIPIENT_ID { get; set; }

        public int? RECIPIENT_GROUP_ID { get; set; }

        public DateTime? RECEIVE_DATE { get; set; }

        public virtual MESSAGE_SENDER MESSAGE_SENDER { get; set; }
    }
}
