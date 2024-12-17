using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ATEVersions_Management.Models.DAOModels;
namespace ATEVersions_Management.Models.DTOModels
{
    public class VersionDTO
    {
        // === DB Data Process ===

        [Display(Name = "Version ID")]
        public int VersionID { get; set; }

        [Display(Name = "Program ID")]
        public int ProgramID { get; set; }

        [Display(Name = "Program")]
        public string ProgramName { get; set; }

        [Display(Name = "Model")]
        public string ModelName { get; set; }
        [Display(Name = "ProjectType")]
        public string ProjectType { get; set; }

        [StringLength(150)]
        [Display(Name = "Version")]
        public string VersionName { get; set; }

        [StringLength(250)]
        [Display(Name = "Engineer")]
        public string Engineer { get; set; }

        [Display(Name = "Build time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? BuildTime { get; set; }

        [Required]
        [Display(Name = "Release time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
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

        // === External Data Process ===

        [Display(Name = "Latest ATE Version")]
        public int IsLastestDBVersion { get; set; }

        [Display(Name = "Latest Version")]
        [StringLength(50)]
        public string LastestVersionName { get; set; }

        [Display(Name = "Latest")]        
        public int IsLastest { get; set; }

        [Display(Name = "Usage")]        
        public int Usage { get; set; }        

        public int IsATEListAvailable { get;set; }
        public int IsATEListChecked { get; set; }
    }

    public class VersionLatestDTO
    {
        public string ModelName { get; set; }
        public string VersionLatest { get; set; }
        public string VersionOnline { get; set; }
        public bool IsVersionsMatched
        {
            get
            {
                if (!string.IsNullOrEmpty(VersionLatest) && !string.IsNullOrEmpty(VersionOnline))
                {
                    return VersionLatest.Trim().ToLower() == VersionOnline.Trim().ToLower();
                }
                return true;
            }
        }
        public int UncheckCount { get; set; }
        public int NoATEListCount { get; set; }
    }
}