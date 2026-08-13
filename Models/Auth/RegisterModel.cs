using System.ComponentModel.DataAnnotations;

public class RegisterModel


{

    [Required(ErrorMessage = "Full Name is required")]

    [StringLength(50, MinimumLength = 3)]

    public string FullName { get; set; } = string.Empty;



    [Required(ErrorMessage = "Email is required")]

    [EmailAddress(ErrorMessage = "Invalid email address")]

    public string Email { get; set; } = string.Empty;



    [Required(ErrorMessage = "Password is required")]

    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]

    [DataType(DataType.Password)]

    public string Password { get; set; } = string.Empty;



    [Required(ErrorMessage = "Confirm Password is required")]

    [Compare("Password", ErrorMessage = "Passwords do not match")]

    [DataType(DataType.Password)]

    public string ConfirmPassword { get; set; } = string.Empty;

}

public class EditProfileModel
{
    [Required(ErrorMessage = "Full Name is required")]
    [StringLength(50, MinimumLength = 3)]
    public string FullName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }
}

public class ChangePasswordModel
{
    [Required(ErrorMessage = "Current password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "The password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}