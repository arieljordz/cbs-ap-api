using CbsAp.Domain.Enums;

namespace CbsAp.Application.DTOs.InvoiceInquiry
{
    public class InvoiceInquiryDto
    {
        public long InvoiceID { get; set; }
        public string? SupplierName { get; set; }
        public DateTimeOffset? InvoiceDate { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? PONumber { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public string? GrossAmount { get; set; }
        public string? NextRole { get; set; }
        public string? ExceptionReason { get; set; }
        public string? Status { get; set; }
        public DateTimeOffset? ScanDate { get; set; }
    }
}
