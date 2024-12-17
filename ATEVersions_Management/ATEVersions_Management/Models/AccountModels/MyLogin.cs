using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace ATEVersions_Management.Models.AccountModels
{    
    public class MyLogin
    {
        public int UserID { get; set; }
        //Username
        [Required(AllowEmptyStrings = false,ErrorMessage = "Must input username!")]
        [RegularExpression("^(V)([0-9]){7}(?:[a-zA-Z])?$",
        ErrorMessage = "Username must start with 'V' and follow by 7 numbers and a character defines role if has any.")]
        public string Username { get; set; }
        //Password
        [Required(AllowEmptyStrings = false,ErrorMessage = "Must input password!")]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{6,}$",
        ErrorMessage = "Password must contain at least 6 characters, include: lowercase, uppercase, number and special character!")]
        public string Password { get; set; }
        //Role        
        public int Role { get; set; }
        //Name
        public string Name { get; set; }
        //Avatar
        public string Avatar { get; set; }
    }
}   