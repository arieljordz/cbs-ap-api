using System.ComponentModel.DataAnnotations;

namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    /// <summary>
    ///  Display atribute optional for exporting to excel
    /// </summary>
    public class ExportInvoiceBaseDto
    {
        [Display(Order = 1)]
        public string? SupplierName { get; set; }

        [Display(Order = 2)]
        public string? InvoiceDate  { get; set; }

        [Display(Order = 3)]
        public string? InvoiceNumber { get; set; }

        [Display(Order = 4)]
        public string? PoNumber { get; set; }

        [Display(Order = 5)]
        public string? DueDate { get; set; }

        [Display(Order = 6)]
       
        public decimal? GrossAmount { get; set; }
    }

    public class ExportMyInvoiceDto : ExportInvoiceBaseDto
    {
        [Display(Order = 7)]
        public string? NextRole { get; set; }
        [Display(Order = 8)]
        public string? ExceptionReason { get; set; }
    }

    public class ExportRejectedInvoiceDto : ExportInvoiceBaseDto
    {
        [Display(Order = 7)]
        public string? Reason { get; set; }
    }

    public class ExportExceptionInvoiceDto : ExportInvoiceBaseDto
    {
        [Display(Order = 7)]
        public string? ExceptionReason { get; set; }
    }

    public class ExportArchiveInvoiceDto : ExportInvoiceBaseDto
    {
       // public string? ExceptionReason { get; set; }
    }
}