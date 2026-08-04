using Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels;
using Ice_Cream_Parlour_Eproject.Models;

namespace Ice_Cream_Parlour_Eproject.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardAsync();
    }
}