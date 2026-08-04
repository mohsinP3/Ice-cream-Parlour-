using Ice_Cream_Parlour_Eproject.Areas.Models.Enums;
using Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels;

namespace Ice_Cream_Parlour_Eproject.Services
{
    public interface IOrderService
    {
        Task<OrderListViewModel> GetPagedAsync(string? search, OrderStatus? status, int page, int pageSize);
        Task<OrderViewModel?> GetByIdAsync(int id);
        Task<OrderCreateViewModel> GetCreateModelAsync();
        Task<int> CreateAsync(OrderCreateViewModel model, string? userId);
        Task UpdateStatusAsync(int id, OrderStatus orderStatus, PaymentStatus paymentStatus, string? userId, string? notes);
    }
}