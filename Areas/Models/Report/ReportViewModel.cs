using System;
using System.Collections.Generic;

namespace Ice_Cream_Parlour_Eproject.Areas.Models.Report
{
    // ===== REPORT VIEW MODEL =====
    public class ReportViewModel
    {
        public string ReportType { get; set; } = "daily";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Summary Stats
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int TotalItemsSold { get; set; }

        // Chart Data
        public List<string> ChartLabels { get; set; } = new List<string>();
        public List<decimal> ChartData { get; set; } = new List<decimal>();

        // Recent Orders
        public List<OrderReportDto> RecentOrders { get; set; } = new List<OrderReportDto>();

        // Top Products
        public List<TopProductDto> TopProducts { get; set; } = new List<TopProductDto>();
    }

    // ===== ORDER REPORT DTO =====
    public class OrderReportDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
    }

    // ===== TOP PRODUCT DTO =====
    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
        public int ProgressPercent { get; set; }
    }

    // ===== DAILY SALES REPORT =====
    public class DailySalesReport
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalSales { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    // ===== MONTHLY SALES REPORT =====
    public class MonthlySalesReport
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int OrderCount { get; set; }
        public decimal TotalSales { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    // ===== PRODUCT PERFORMANCE REPORT =====
    public class ProductPerformanceReport
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal AveragePrice { get; set; }
    }

    // ===== CUSTOMER REPORT =====
    public class CustomerReportDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrderValue { get; set; }
        public DateTime LastOrderDate { get; set; }
        public bool IsActive { get; set; }
    }

    // ===== REPORT FILTERS =====
    public class ReportFilters
    {
        public string ReportType { get; set; } = "daily";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CategoryId { get; set; }
        public int? CustomerId { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
    }

    // ===== SALES SUMMARY =====
    public class SalesSummary
    {
        public decimal TodaySales { get; set; }
        public decimal WeeklySales { get; set; }
        public decimal MonthlySales { get; set; }
        public decimal YearlySales { get; set; }
        public decimal TotalSales { get; set; }

        public int TodayOrders { get; set; }
        public int WeeklyOrders { get; set; }
        public int MonthlyOrders { get; set; }
        public int YearlyOrders { get; set; }
        public int TotalOrders { get; set; }

        public decimal GrowthPercentage { get; set; }
        public string GrowthTrend { get; set; } = "stable";
    }

    // ===== REVENUE CHART DATA =====
    public class RevenueChartData
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<decimal> Revenue { get; set; } = new List<decimal>();
        public List<int> Orders { get; set; } = new List<int>();
    }

    // ===== EXPORT FORMATS =====
    public enum ExportFormat
    {
        PDF,
        Excel,
        CSV
    }
}