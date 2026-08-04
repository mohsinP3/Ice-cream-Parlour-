using System;
using System.Collections.Generic;

namespace Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels
{
    // ===== DASHBOARD VIEW MODEL =====
    public class DashboardViewModel
    {
        // Stats
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }

        // Sales
        public decimal TodaySales { get; set; }
        public decimal MonthlySales { get; set; }
        public decimal YearlySales { get; set; }

        // Orders
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int CompletedOrders { get; set; }

        // Charts
        public List<string> RevenueChartLabels { get; set; } = new List<string>();
        public List<decimal> RevenueChartData { get; set; } = new List<decimal>();

        // Lists
        public List<LowStockProductDto> LowStockProducts { get; set; } = new List<LowStockProductDto>();
        public List<TopSellingProductDto> TopSellingProducts { get; set; } = new List<TopSellingProductDto>();
        public List<RecentOrderDto> RecentOrders { get; set; } = new List<RecentOrderDto>();
    }

    // ===== LOW STOCK PRODUCT =====
    public class LowStockProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public int LowStockThreshold { get; set; }
        public int StockPercent { get; set; }
    }

    // ===== TOP SELLING PRODUCT =====
    public class TopSellingProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
        public int ProgressPercent { get; set; }
    }

    // ===== RECENT ORDER =====
    public class RecentOrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
    }
}