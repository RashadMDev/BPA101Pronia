using System.ComponentModel.DataAnnotations;

namespace BPA101Pronia.Areas.Admin.ViewModels.Account.Admin
{
    public record CreateAdminVM
    {
        [Required(ErrorMessage ="Username is required")]
        [StringLength(50, ErrorMessage = "Username must be 50 characters")]
        [MinLength(3, ErrorMessage = "Username must be minimum 3 characters")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="Email is not valid")]
        [StringLength(50, ErrorMessage = "Email must be 50 characters")]
        [MinLength(3, ErrorMessage = "Email must be minimum 3 characters")]
        public string Email { get; set; }



        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
