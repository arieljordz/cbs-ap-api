using CbsAp.Domain.Enums;

namespace CbsAp.Application.DTOs.InvoiceInquiry
{
    public class InvoiceInquirySearchDto
    {
        public int? SupplierInfoID { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? PONumber { get; set; }
        public string? Role { get; set; }
        public List<InvoiceStatusType>? Status { get; set; }
        public DateTimeOffset? InvoiceDateFrom { get; set; }
        public DateTimeOffset? InvoiceDateTo { get; set; }
        public DateTimeOffset? InvoiceDueDateFrom { get; set; }
        public DateTimeOffset? InvoiceDueDateTo { get; set; }
        public DateTimeOffset? PaymentDateFrom { get; set; }
        public DateTimeOffset? PaymentDateTo { get; set; }
        public DateTimeOffset? ScanDateFrom { get; set; }
        public DateTimeOffset? ScanDateTo { get; set; }


    }
}