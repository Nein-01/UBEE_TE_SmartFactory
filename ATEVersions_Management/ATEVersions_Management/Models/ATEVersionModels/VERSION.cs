namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("VERSION")]
    public partial class VERSION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VERSION()
        {
            ATE_CHECKLIST = new HashSet<ATE_CHECKLIST>();
        }
        [Key]
        [Display(Name = "Version ID")]
        public int VersionID { get; set; }
        
        [Display(Name = "Program ID")]
        public int ProgramID { get; set; }

        [Required]
        [StringLength(150)]        
        [Display(Name = "Version")]
        public string VersionName { get; set; }

        [StringLength(250)]
        [Display(Name = "Engineer")]
        public string Engineer { get; set; }

        [Display(Name = "Build time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? BuildTime { get; set; }

        [Required]
        [Display(Name = "Release time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseTime { get; set; }

        [Required]
        [Display(Name = "Release note")]
        public string ReleaseNote { get; set; }

        [Display(Name = "Status")]
        public int? Status { get; set; }

        [Display(Name = "Created at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Updated at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Updated by")]
        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ATE_CHECKLIST> ATE_CHECKLIST { get; set; }

        public virtual PROGRAM PROGRAM { get; set; }

        
    }
}
