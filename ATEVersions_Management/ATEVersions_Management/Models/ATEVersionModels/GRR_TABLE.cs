
namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("GRR_TABLE")]
    public class GRR_TABLE
    {
        [Key]
        [Display(Name = "GRR_ID")]
        public int GRR_ID { get; set; }

        [Display(Name = "UserID")]
        public int UserID { get; set; }

        [Display(Name = "Dept")]
        [StringLength(50)]
        public string Dept { get; set; }

        [Display(Name = "Gage Model")]
        [StringLength(50)]
        public string GageModel { get; set; }

        [Display(Name = "Gage Name")]
        [StringLength(50)]
        public string GageName { get; set; }

        [Display(Name = "Gage No")]
        [StringLength(50)]
        public string GageNo { get; set; }

        [Display(Name = "Part Name")]
        [StringLength(100)]
        public string PartName { get; set; }

        [Display(Name = "Specification")]
        [StringLength(50)]
        public string Specification { get; set; }

        [Display(Name = "Characteristic")]
        [StringLength(250)]
        public string Characteristic { get; set; }

        [Display(Name = "JSON_OperTestResult")]
        public string JSON_OperTestResult { get; set; }

        [Display(Name = "Prepared by")]
        [StringLength(50)]
        public string PreparedBy { get; set; }

        [Display(Name = "Prepared at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? PreparedAt { get; set; }

        [Display(Name = "Preparer note")]
        [StringLength(500)]
        public string PreparedNote { get; set; }       

        [Display(Name = "Approved by")]
        [StringLength(50)]
        public string ApprovedBy { get; set; }

        [Display(Name = "Approved at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Approver note")]
        [StringLength(250)]
        public string ApproverNote { get; set; }

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
        public virtual USER USER { get; set; }
    }
}