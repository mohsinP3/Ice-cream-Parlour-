using System.ComponentModel.DataAnnotations;

namespace Ice_Cream_Parlour_Eproject.Areas.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        public string? ProfileImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastActivityDate { get; set; }

        // Navigation
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}