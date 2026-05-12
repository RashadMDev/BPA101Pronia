using System.ComponentModel.DataAnnotations;

namespace BPA101Pronia.ViewModels.Account
{
    public record RegisterVM
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(30, ErrorMessage = "Username cannot exceed 30 characters")]
        [MinLength(2, ErrorMessage = "Username must be at least 2 characters long")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, ErrorMessage = "Name cannot exceed 30 characters")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Surname is required")]
        [StringLength(30, ErrorMessage = "Surname cannot exceed 30 characters")]
        [MinLength(2, ErrorMessage = "Surname must be at least 2 characters long")]
        public string Surname { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
