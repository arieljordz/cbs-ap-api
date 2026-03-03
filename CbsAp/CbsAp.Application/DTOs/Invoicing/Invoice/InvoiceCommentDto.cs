namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    public class InvoiceCommentDto
    {
        public long InvoiceCommentID { get; set; }

        public string? Comment { get; set; }

        public long InvoiceID { get; set; }
    }

    public class LoadInvoiceCommentsDto {

        public long InvoiceCommentID { get; set; }
        public string? Comment { get; set; }

        public string? CreatedBy { get; set; }
        public string? CreatedDate { get; set; }

    }
}
