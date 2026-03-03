using System.ComponentModel.DataAnnotations;

namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    /// <summary>
    ///  Display atribute optional for exporting to excel
    /// </summary>
    public class ExportInvoiceBaseDto
    {
        [Display(Order = 1)]
        public string? Entity { get; set; }

        [Display(Order = 2)]
        public string? SuppName { get; set; }

        [Display(Order = 3)]
        public string? InvoiceDate { get; set; }

        [Display(Order = 4)]
        public string? InvoiceNo { get; set; }

        [Display(Order = 5)]
        public string? PoNo { get; set; }

        [Display(Order = 6)]
        public string? DueDate { get; set; }

        [Display(Order = 7)]
        public decimal? GrossAmount { get; set; }
    }

    public class ExportMyInvoiceDto : ExportInvoiceBaseDto
    {
        public string? NextRole { get; set; }
    }

    public class ExportRejectedInvoiceDto : ExportInvoiceBaseDto
    {
        public string? InvoiceApprover { get; set; }
        public string? ArchiveDate { get; set; }
    }

    public class ExportExceptionInvoiceDto : ExportInvoiceBaseDto
    {
        public string? ExceptionReason { get; set; }
    }

    public class ExportArchiveInvoiceDto : ExportInvoiceBaseDto
    {
        public string? ExceptionReason { get; set; }
    }
}