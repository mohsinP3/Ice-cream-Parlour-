using System.ComponentModel.DataAnnotations;

namespace Ice_Cream_Parlour_Eproject.Models
{
    public class SignupModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? Address { get; set; }

        [Required(ErrorMessage = "Please select payment type")]
        public string PaymentType { get; set; } = string.Empty;
    }
}
 