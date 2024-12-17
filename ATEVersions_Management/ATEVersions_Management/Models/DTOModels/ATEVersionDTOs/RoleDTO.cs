using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels.ATEVersionDTOs
{
    public class RoleDTO
    {
        public int RoleID { get; set; }

        [Display(Name = "Role")]
        [StringLength(100)]
        public string RoleName { get; set; }

    }
}