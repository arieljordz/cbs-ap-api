using CbsAp.Domain.Enums;


namespace CbsAp.Application.DTOs.InvoiceInquiry
{
    public class ExportInvoiceInquiryDto
    {

        public long InvoiceID { get; set; }
        public string? SupplierName { get; set; }
        public string? InvoiceDate { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? PONumber { get; set; }
        public string? DueDate { get; set; }
        public string? GrossAmount { get; set; }
        public string? PaymentDate { get; set; }
        public string? ScanDate { get; set; }
        public string? Status { get; set; }
        public string? Role { get; set; } 
        public string? ApprovedBy { get; set; }
    }
}
