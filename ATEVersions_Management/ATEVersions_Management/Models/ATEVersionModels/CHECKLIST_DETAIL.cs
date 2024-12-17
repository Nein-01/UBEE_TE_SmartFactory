namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CHECKLIST_DETAIL")]
    public partial class CHECKLIST_DETAIL
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Checklist ID")]
        public int CheckListID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Item ID")]
        public int ItemID { get; set; }

        [Display(Name = "Result")]
        public int? Result { get; set; }

        public virtual ATE_CHECKLIST ATE_CHECKLIST { get; set; }

        public virtual CHECKLIST_ITEM CHECKLIST_ITEM { get; set; }
    }
}
