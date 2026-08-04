using System.ComponentModel.DataAnnotations;

namespace Ice_Cream_Parlour_Eproject.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        public bool IsRegistered { get; set; } = false;

        public bool IsRead { get; set; } = false;
    }
}