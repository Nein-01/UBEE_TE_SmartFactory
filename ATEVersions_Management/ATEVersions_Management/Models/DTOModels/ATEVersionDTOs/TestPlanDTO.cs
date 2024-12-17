using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels
{
    public class TestPlanDTO
    {
        [Display(Name = "Test Plan ID")]
        public int TestPlanID { get; set; }

        [Display(Name = "User ID")]
        public int UserID { get; set; }

        [Display(Name = "Owner")]
        public string UserName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Can't leave blank!")]
        [RegularExpression("^(([a-zA-Z0-9]{7}[.a-zA-Z][0-9]{2}))?(?:[a-zA-Z])?$",
        ErrorMessage = "Model name must follow format: xxxxxxx.xx or xxxxxxxTxx")]
        [Display(Name = "Model")]
        public string ModelName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Can't leave blank!")]
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
        
        [Required(ErrorMessage = "Can't leave blank!")]
        [Display(Name = "Comments")]
        public string ModifyNote { get; set; }

        [Display(Name = "Modified at")]
        /*[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]*/
        public DateTime? ModifiedAt { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Can't leave blank!")]
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

        // Helper Fields
        [NotMapped]
        [Required(ErrorMessage = "Must attach a file!")]
        public HttpPostedFileBase FileUpload { get; set; }
    }
    public class TestPlanPreviewDTO
    {
        public string ModelName { get; set; }
        public string VersionLatest { get; set; }
    }
}