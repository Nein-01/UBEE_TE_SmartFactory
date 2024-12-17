namespace ATEVersions_Management.Models.ATEVersionModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("USERS")]
    public partial class USER
    {
        public USER()
        {
            TEST_PLANs = new HashSet<TEST_PLAN>();
            GRR_TABLEs = new HashSet<GRR_TABLE>();
            MESSAGE_SENDER = new HashSet<MESSAGE_SENDER>();
            MESSAGE_USER_GROUP = new HashSet<MESSAGE_USER_GROUP>();
        }

        [Key]
        [Display(Name = "User ID")]
        public int UserID { get; set; }

        [Display(Name = "Role ID")]
        public int RoleID { get; set; }

        [Display(Name = "Username")]
        [StringLength(150)]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [StringLength(250)]
        public string Password { get; set; }

        [Display(Name = "Full name")]
        [StringLength(250)]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [StringLength(20)]
        [RegularExpression("^([0-9]{10})$|^([0-9]{3}[-][0-9]{5})$",
        ErrorMessage = "Phone number must have 10 numbers ,or in form of xxx-xxxxx!")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Avatar")]
        public string Avatar { get; set; }

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
        [Display(Name = "Department")]
        [StringLength(50)]
        public string Department { get; set; }
        public virtual ROLE ROLE { get; set; }        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEST_PLAN> TEST_PLANs { get; set; }
        public virtual ICollection<GRR_TABLE> GRR_TABLEs { get; set; }        
        public virtual ICollection<MESSAGE_SENDER> MESSAGE_SENDER { get; set; }        
        public virtual ICollection<MESSAGE_USER_GROUP> MESSAGE_USER_GROUP { get; set; }
        //
        // ========= Helper fields =========
        [NotMapped]
        public HttpPostedFileBase FileUpload { get; set; }
    }
}
