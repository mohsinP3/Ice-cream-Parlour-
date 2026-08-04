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

            var todaySales = (await _context.Orders.Where(o => o.OrderDate.Date == today).Select(o => o.TotalAmount).ToListAsync()).Sum();
            var monthlySales = (await _context.Orders.Where(o => o.OrderDate >= startOfMonth).Select(o => o.TotalAmount).ToListAsync()).Sum();
            var yearlySales = (await _context.Orders.Where(o => o.OrderDate.Year == today.Year).Select(o => o.TotalAmount).ToListAsync()).Sum();

            var model = new DashboardViewModel
            {
                TotalOrders = await _context.Orders.CountAsync(),
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TodaySales = todaySales,
                MonthlySales = monthlySales,
                YearlySales = yearlySales
            };

            // Chart Data (Last 7 days) projected to memory
            var ordersInPeriod = await _context.Orders
                .Where(o => o.OrderDate >= today.AddDays(-6))
                .Select(o => new { o.OrderDate, o.TotalAmount })
                .ToListAsync();

            var chartData = ordersInPeriod
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(o => o.TotalAmount) })
                .OrderBy(g => g.Date)
                .ToList();

            model.RevenueChartLabels = chartData.Select(d => d.Date.ToString("dd MMM")).ToList();
            model.RevenueChartData = chartData.Select(d => d.Total).ToList();

            return model;
        }
    }
}