using System.ComponentModel.DataAnnotations;

namespace Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? ProfileImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ===== Not Mapped =====
        public IFormFile? ProfileImage { get; set; }

        // ===== Stats =====
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public string? LastOrderDate { get; set; }
    }

    public class CustomerListViewModel
    {
        public PaginatedList<CustomerViewModel> Customers { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "name";
        public string SortDir { get; set; } = "asc";
        public int TotalCustomers { get; set; }
    }

    public class PaginatedList<T>
    {
        public List<T> Items { get; set; } = new();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}