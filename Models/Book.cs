using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ice_Cream_Parlour_Eproject.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 999.99)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string? ImagePath { get; set; }

        [Range(0, 1000)]
        public int StockQuantity { get; set; } = 0;

        public bool IsAvailable => StockQuantity > 0;

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}