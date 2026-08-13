using System.ComponentModel.DataAnnotations;

namespace Ice_Cream_Parlour_Eproject.Models
{
    public class OrderCheckoutViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Delivery Address is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Address must be at least 10 characters")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment Method is required")]
        public string PaymentMethod { get; set; } = "Cash on Delivery";

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
