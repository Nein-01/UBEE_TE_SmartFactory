using System.ComponentModel.DataAnnotations;

namespace ATEVersions_Management.Models.AccountModels
{
    public class MyResetPassword
    {
        [Required(ErrorMessage = "Enter new password!", AllowEmptyStrings = false)]
        [StringLength(100)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{6,}$",
        ErrorMessage = "Password must contain at least 6 characters, include: lowercase, uppercase, number and 1 special character!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm your password!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords are not matched!")]
        public string ConfirmPassword { get; set; }

        public string ResetCode { get; set; }
    }
}