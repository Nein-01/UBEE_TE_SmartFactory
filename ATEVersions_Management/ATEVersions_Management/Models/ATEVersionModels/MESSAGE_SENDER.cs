namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MESSAGE_SENDER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MESSAGE_SENDER()
        {
            MESSAGE_RECIPIENT = new HashSet<MESSAGE_RECIPIENT>();
        }

        [Key]
        public int MESSAGE_ID { get; set; }

        public int? MESSAGE_PARENT_ID { get; set; }

        public int SENDER_ID { get; set; }

        [StringLength(100)]
        public string MESSAGE_TYPE { get; set; }

        public string MESSAGE_CONTENT { get; set; }

        [StringLength(100)]
        public string DEVICE_NAME_IP { get; set; }

        public DateTime? SEND_DATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MESSAGE_RECIPIENT> MESSAGE_RECIPIENT { get; set; }

        public virtual USER USER { get; set; }
    }
}
