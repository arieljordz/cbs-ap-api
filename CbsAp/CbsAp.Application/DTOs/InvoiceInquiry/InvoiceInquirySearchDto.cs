using CbsAp.Domain.Enums;

namespace CbsAp.Application.DTOs.InvoiceInquiry
{
    public class InvoiceInquirySearchDto
    {
        public string? SupplierName { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? PONumber { get; set; }
        public int? Status { get; set; }
        public DateTimeOffset? ScanDateFrom { get; set; }
        public DateTimeOffset? ScanDateTo { get; set; }
        public DateTimeOffset? InvoiceDateFrom { get; set; }
        public DateTimeOffset? InvoiceDateTo { get; set; }

    }
}