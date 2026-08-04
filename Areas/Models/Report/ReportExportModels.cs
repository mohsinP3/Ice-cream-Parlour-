using Ice_Cream_Parlour_Eproject.Models;
using System;
using System.Collections.Generic;

namespace Ice_Cream_Parlour_Eproject.Areas.Models.Report
{
    // ===== PDF INVOICE MODEL =====
    public class InvoicePdfModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;

        public List<InvoiceItemDto> Items { get; set; } = new List<InvoiceItemDto>();

        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
        public string CompanyName { get; set; } = "iCREAM";
        public string CompanyAddress { get; set; } = "123 Ice Cream Street, New York, NY 10001";
        public string CompanyPhone { get; set; } = "+1 (012) 345 6789";
        public string CompanyEmail { get; set; } = "info@icream.com";
    }

    // ===== INVOICE ITEM =====
    public class InvoiceItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }

    // ===== PDF REPORT MODEL =====
    public class PdfReportModel
    {
        public string ReportTitle { get; set; } = string.Empty;
        public string ReportType { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime GeneratedDate { get; set; } = DateTime.Now;

        public SalesSummary Summary { get; set; } = new SalesSummary();
        public List<OrderReportDto> Orders { get; set; } = new List<OrderReportDto>();
        public List<TopProductDto> TopProducts { get; set; } = new List<TopProductDto>();
        public List<DailySalesReport> DailySales { get; set; } = new List<DailySalesReport>();
    }

    // ===== EXCEL REPORT MODEL =====
    public class ExcelReportModel
    {
        public string SheetName { get; set; } = "Report";
        public List<ExcelRowData> Rows { get; set; } = new List<ExcelRowData>();
        public List<string> Headers { get; set; } = new List<string>();
    }

    // ===== EXCEL ROW DATA =====
    public class ExcelRowData
    {
        public List<object> Columns { get; set; } = new List<object>();
    }
}