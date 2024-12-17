using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace ATEVersions_Management.Models.AccountModels
{
    public class MyRegister
    {        

        public int UserID { get; set; }        
        //Email
        [Required(ErrorMessage = "Must input username!")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters!", MinimumLength = 1)]
        [RegularExpression("^(V)([0-9]){7}(?:[a-zA-Z])?$",
        ErrorMessage = "Username must start with 'V' and follow by 7 numbers and a character defines role if has any.")]
        public string Username { get; set; }
        //Password
        [Required(ErrorMessage = "Must input password!")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{6,}$",
        ErrorMessage = "Password must contain at least 6 characters, include: lowercase, uppercase, number and special character!")]
        public string Password { get; set; }
        //Confirm password
        [Required(ErrorMessage = "Confirm your password!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords are not matched!")]
        public string ConfirmPassword { get; set; }
        //Roles
        [Required(ErrorMessage = "Must select role!")]
        [Display(Name = "Role")]
        public int RoleID { get; set; }
        //Name
        [Required(ErrorMessage = "Must input department!")]
        [StringLength(50, ErrorMessage = "Max 50 character!", MinimumLength = 1)]
        public string Department { get; set; }
        //Name
        [Required(ErrorMessage = "Must input fullname!")]
        [StringLength(50, ErrorMessage = "Max name length is 50 character!",MinimumLength = 1)]
        public string Name { get; set; }
        //Phone Number
        [StringLength(11)]
        [Required(ErrorMessage = "Must input phone number!")]
        [MinLength(10, ErrorMessage = "Phone number has 10 number!")]
        [RegularExpression("^([0-9]{10})$")]
        public string Phone { get; set; }
        //Avatar
        [StringLength(500, ErrorMessage = "Avatar source is out of supported length!")]
        public string Avatar { get; set; }
        

    }
}