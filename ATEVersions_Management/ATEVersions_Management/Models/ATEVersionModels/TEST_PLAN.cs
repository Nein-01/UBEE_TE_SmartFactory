
namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("TEST_PLAN")]
    public class TEST_PLAN
    {
        [Key]
        [Display(Name = "Test Plan ID")]
        public int TestPlanID { get; set; }

        [Display(Name = "User ID")]
        public int UserID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Cannot leave blank!")]
        [Display(Name = "Model")]
        public string ModelName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Cannot leave blank!")]
        [Display(Name = "Version")]
        public string TestPlanVersion { get; set; }

        [StringLength(50)]
        [Display(Name = "Test Phase")]
        public string TestPhase { get; set; }

        [StringLength(50)]
        [Display(Name = "Project type")]
        public string ProjectType { get; set; }

        [StringLength(50)]        
        [Display(Name = "Author")]
        public string Author { get; set; }

        
        [Required(ErrorMessage = "Cannot leave blank!")]
        [Display(Name = "Modification Comments")]
        public string ModifyNote { get; set; }

        [Display(Name = "Modified at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedAt { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Cannot leave blank!")]
        [Display(Name = "Stored Directory")]
        public string StoredDir { get; set; }

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
        public virtual USER USER { get; set; }
    }
}