using Ice_Cream_Parlour_Eproject.Areas.Models;
using Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels;

namespace Ice_Cream_Parlour_Eproject.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerListViewModel> GetPagedAsync(string? search, string sortBy, string sortDir, int page, int pageSize);
        Task<CustomerViewModel?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CustomerViewModel model, IWebHostEnvironment env);
        Task<bool> UpdateAsync(CustomerViewModel model, IWebHostEnvironment env);
        Task<bool> DeleteAsync(int id, IWebHostEnvironment env);
        Task<List<Order>> GetOrderHistoryAsync(int customerId);
    }
}