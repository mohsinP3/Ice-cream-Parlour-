using Microsoft.EntityFrameworkCore;
using Ice_Cream_Parlour_Eproject.Data;
using Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels;

namespace Ice_Cream_Parlour_Eproject.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportViewModel> GetReportAsync(string reportType, DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue) startDate = DateTime.Now.AddDays(-30);
            if (!endDate.HasValue) endDate = DateTime.Now;

            var query = _context.Orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate);

            var model = new ReportViewModel
            {
                ReportType = reportType,
                StartDate = startDate,
                EndDate = endDate,
                TotalOrders = await query.CountAsync(),
                TotalRevenue = await query.SumAsync(o => o.TotalAmount),
                RecentOrders = await query.OrderByDescending(o => o.OrderDate).Take(10)
                    .Select(o => new OrderReportDto
                    {
                        Id = o.Id,
                        OrderNumber = o.OrderNumber,
                        CustomerName = o.CustomerName,
                        OrderDate = o.OrderDate,
                        TotalAmount = o.TotalAmount,
                        OrderStatus = o.OrderStatus,
                        PaymentStatus = o.PaymentStatus
                    }).ToListAsync()
            };

            model.AverageOrderValue = model.TotalOrders > 0 ? model.TotalRevenue / model.TotalOrders : 0;

            // Chart Data
            var chartData = await query.GroupBy(o => o.OrderDate.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(o => o.TotalAmount) })
                .OrderBy(g => g.Date).Take(7).ToListAsync();

            model.ChartLabels = chartData.Select(d => d.Date.ToString("dd MMM")).ToList();
            model.ChartData = chartData.Select(d => d.Total).ToList();

            return model;
        }

        public async Task<byte[]> ExportPdfAsync(string reportType, DateTime? startDate, DateTime? endDate)
        {
            // Placeholder - return empty byte array
            return Array.Empty<byte>();
        }

        public async Task<byte[]> ExportExcelAsync(string reportType, DateTime? startDate, DateTime? endDate)
        {
            // Placeholder - return empty byte array
            return Array.Empty<byte>();
        }

        public async Task<byte[]> ExportOrderInvoicePdfAsync(int orderId)
        {
            // Placeholder - return empty byte array
            return Array.Empty<byte>();
        }
    }
}