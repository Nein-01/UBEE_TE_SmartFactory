namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PROGRAM")]
    public partial class PROGRAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROGRAM()
        {
            VERSIONs = new HashSet<VERSION>();
        }
        [Key]
        [Display(Name = "Program ID")]
        public int ProgramID { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Cannot leave blank!")]
        [Display(Name = "Program")]
        public string ProgramName { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Cannot leave blank!")]
        [RegularExpression("^(([a-zA-Z0-9]{7}[.a-zA-Z][0-9]{2}))(?:_OBA)?$|^(([a-zA-Z0-9]{2}.[a-zA-Z0-9]{3,}T[a-zA-Z0-9]{2}))(?:_OBA)?$",
        ErrorMessage = "Model name must follow format: xxxxxxx.xx, xxxxxxxTxx or xx.xxxxTxx (_OBA)")]
        [Display(Name = "Model")]
        public string ModelName { get; set; }

        [StringLength(50)]        
        [Display(Name = "Test Phase")]
        public string TestPhase { get; set; }

        [StringLength(50)]
        [Display(Name = "Project type")]
        public string ProjectType { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Cannot leave blank!")]
        [Display(Name = "Developed tool")]
        public string DevelopTool { get; set; }

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
        public virtual ICollection<VERSION> VERSIONs { get; set; }
    }
}
