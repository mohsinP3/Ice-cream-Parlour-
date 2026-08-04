using Ice_Cream_Parlour_Eproject.Areas.Models.ViewModels;

namespace Ice_Cream_Parlour_Eproject.Services
{
    public interface IReportService
    {
        Task<ReportViewModel> GetReportAsync(string reportType, DateTime? startDate, DateTime? endDate);
        Task<byte[]> ExportPdfAsync(string reportType, DateTime? startDate, DateTime? endDate);
        Task<byte[]> ExportExcelAsync(string reportType, DateTime? startDate, DateTime? endDate);
        Task<byte[]> ExportOrderInvoicePdfAsync(int orderId);
    }
}
