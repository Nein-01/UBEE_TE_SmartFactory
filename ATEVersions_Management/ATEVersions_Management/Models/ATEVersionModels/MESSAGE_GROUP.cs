namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MESSAGE_GROUP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MESSAGE_GROUP()
        {
            MESSAGE_USER_GROUP = new HashSet<MESSAGE_USER_GROUP>();
        }

        [Key]
        public int GROUP_ID { get; set; }

        [Required]
        [StringLength(200)]
        public string GROUP_NAME { get; set; }

        public int? CREATE_BY { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public bool? IS_ACTIVE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MESSAGE_USER_GROUP> MESSAGE_USER_GROUP { get; set; }
    }
}
