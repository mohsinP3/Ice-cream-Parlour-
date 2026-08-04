using System.ComponentModel.DataAnnotations;

namespace Ice_Cream_Parlour_Eproject.Models
{
    public class UserRecipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string RecipeName { get; set; } = string.Empty;

        [Required]
        public string Ingredients { get; set; } = string.Empty;

        [Required]
        public string Procedure { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";

        public decimal? PrizeMoney { get; set; }

        public bool IsCertificateIssued { get; set; } = false;

        public DateTime? ReviewedDate { get; set; }

        public string? AdminRemarks { get; set; }
    }
}