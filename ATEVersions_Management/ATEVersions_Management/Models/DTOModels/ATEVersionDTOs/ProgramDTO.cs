﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ATEVersions_Management.Models.DTOModels
{
    public class ProgramDTO
    {
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
    }
}