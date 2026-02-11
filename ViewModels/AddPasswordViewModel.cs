using System.ComponentModel.DataAnnotations;

namespace LOTS3.ViewModels
{
    public class AddPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required, Display(Name = "Confirm new password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage =
            "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
