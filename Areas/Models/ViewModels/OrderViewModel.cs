using Ice_Cream_Parlour_Eproject.Areas.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
        public List<OrderHistoryDto> History { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class OrderHistoryDto
    {
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? ChangedBy { get; set; }
    }

    public class OrderListViewModel
    {
        public PaginatedList<OrderViewModel> Orders { get; set; } = new();
        public string? SearchTerm { get; set; }
        public OrderStatus? StatusFilter { get; set; }
    }

    public class OrderCreateViewModel
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public List<CustomerOrderSelectDto> Customers { get; set; } = new();
        public List<ProductOrderSelectDto> Products { get; set; } = new();
        public List<OrderLineDto> Lines { get; set; } = new() { new OrderLineDto() };
    }

    public class OrderLineDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class CustomerOrderSelectDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class ProductOrderSelectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal FinalPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
