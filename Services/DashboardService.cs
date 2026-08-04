using Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels;
using Ice_Cream_Parlour_Eproject.Data;
using Ice_Cream_Parlour_Eproject.Models;
using Microsoft.EntityFrameworkCore;

namespace Ice_Cream_Parlour_Eproject.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardAsync()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var model = new DashboardViewModel
            {
                TotalOrders = await _context.Orders.CountAsync(),
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TodaySales = await _context.Orders.Where(o => o.OrderDate.Date == today).SumAsync(o => o.TotalAmount),
                MonthlySales = await _context.Orders.Where(o => o.OrderDate >= startOfMonth).SumAsync(o => o.TotalAmount),
                YearlySales = await _context.Orders.Where(o => o.OrderDate.Year == today.Year).SumAsync(o => o.TotalAmount)
            };

            // Chart Data (Last 7 days)
            var chartData = await _context.Orders
                .Where(o => o.OrderDate >= today.AddDays(-6))
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(o => o.TotalAmount) })
                .OrderBy(g => g.Date)
                .ToListAsync();

            model.RevenueChartLabels = chartData.Select(d => d.Date.ToString("dd MMM")).ToList();
            model.RevenueChartData = chartData.Select(d => d.Total).ToList();

            return model;
        }
    }
}