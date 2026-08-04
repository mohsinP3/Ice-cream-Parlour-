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