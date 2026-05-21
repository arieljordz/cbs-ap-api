namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    public class InvoiceSearchBaseDto
    {
        public long InvoiceID { get; set; }

        public string? Entity { get; set; }

        public string? SuppName { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public string? DisplayInvoiceDate 
        {
            get 
            { 
                return InvoiceDate.HasValue ? InvoiceDate.Value.ToString("dd/MM/yyyy") : null ; 
            }
        }

        public string? InvoiceNo { get; set; }

        public string? PoNo { get; set; }

        public DateTime? DueDate { get; set; }

        public string? DisplayDueDate
        {
            get
            {
                return DueDate.HasValue ? DueDate.Value.ToString("dd/MM/yyyy") : null;
            }
        }
        public decimal? GrossAmount { get; set; }
        public string? DisplayGrossAmount 
        {
            get
            {
                return GrossAmount.HasValue ? GrossAmount.Value.ToString("F2") : null;
            }
        }
    }

    public class InvMyInvoiceSearchDto : InvoiceSearchBaseDto
    {
        public string? NextRole { get; set; }

        public string ExceptionReason { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
    }

    // Reject Queue Search
    public class RejectedInvoiceSearchDto : InvoiceSearchBaseDto
    {
        public string FreeField1 { get; set; }

        public string FreeField2 { get; set; }

        public string FreeField3 { get; set; }

        public string? InvoiceApprover { get; set; }

        public string? ArchiveDate { get; set; }

        public string Reason { get; set; }
    }

    // Exception Queue Search
    public class ExceptionInvoiceSearchDto : InvoiceSearchBaseDto
    {
        public string? ExceptionReason { get; set; }

        public bool IsSelected { get; set; }
    }

    // Exception Queue Search
    public class ArchiveInvoiceSearchDto : InvoiceSearchBaseDto
    {
        public string? ExceptionReason { get; set; }

        public bool IsSelected { get; set; }
    }
}