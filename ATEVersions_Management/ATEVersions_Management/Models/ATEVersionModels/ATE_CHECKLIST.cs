namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ATE_CHECKLIST")]
    public partial class ATE_CHECKLIST
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ATE_CHECKLIST()
        {
            CHECKLIST_DETAIL = new HashSet<CHECKLIST_DETAIL>();
        }

        [Key]
        [Display(Name = "Checklist ID")]
        public int CheckListID { get; set; }

        [Display(Name = "Version ID")]
        public int VersionID { get; set; }

        [Display(Name = "Checklist code")]
        [StringLength(250)]
        public string CheckListCode { get; set; }

        [Display(Name = "Product HW/SW")]
        [StringLength(250)]
        public string ProductHW_SW { get; set; }

        [Display(Name = "Prepared by")]
        [StringLength(250)]
        public string PreparedBy { get; set; }

        [Display(Name = "Is checked?")]
        public int? IsPrepared { get; set; }

        [Display(Name = "Preparer note")]
        [StringLength(250)]
        public string PreparerNote { get; set; }

        [Display(Name = "Prepared at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? PreparedAt { get; set; }

        [Display(Name = "Checked by")]
        [StringLength(250)]
        public string CheckedBy { get; set; }

        [Display(Name = "Is checked?")]
        public int? IsChecked { get; set; }

        [Display(Name = "Checker note")]
        [StringLength(250)]
        public string CheckerNote { get; set; }

        [Display(Name = "Checked at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CheckedAt { get; set; }

        [Display(Name = "Approved by")]
        [StringLength(250)]
        public string ApprovedBy { get; set; }

        [Display(Name = "Is approved?")]
        public int? IsApproved { get; set; }

        [Display(Name = "Approver note")]
        [StringLength(250)]
        public string ApproverNote { get; set; }

        [Display(Name = "Approved at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Stored time (year)")]
        public double? StoredTime { get; set; }

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

        public virtual VERSION VERSION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHECKLIST_DETAIL> CHECKLIST_DETAIL { get; set; }
    }
}
