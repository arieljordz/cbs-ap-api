using CbsAp.Domain.Enums;

namespace CbsAp.Application.DTOs.InvoiceInquiry
{
    public class ExportInvoiceInquiryDto
    {
        public long InvoiceID { get; set; }
        public string? SupplierName { get; set; }
        public DateTimeOffset? InvoiceDate { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? PONumber { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public string? GrossAmount { get; set; }
        public DateTimeOffset? PaymemtDate { get; set; }
        public DateTimeOffset? ScanDate { get; set; }
        public string? Status { get; set; }
        public string? Role { get; set; }
    }
}
