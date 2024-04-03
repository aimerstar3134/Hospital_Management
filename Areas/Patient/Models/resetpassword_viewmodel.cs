using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.Areas.Patient.Models
{
    public class resetpassword_viewmodel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "Please enter your old password")]
            [DataType(DataType.Password)]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = "Please enter a new password")]
            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "The {0} must be at least {2} characters long.")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        
    }
}