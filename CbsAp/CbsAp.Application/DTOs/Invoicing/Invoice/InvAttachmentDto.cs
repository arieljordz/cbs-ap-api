using Microsoft.AspNetCore.Http;

namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    public class InvAttachmentDto
    {
        public long InvoiceAttachnmentID { get; set; }
        public long InvoiceID { get; set; }
        public string? OriginalFileName { get; set; }
        public string? StorageFileName { get; set; }
        public string? FileType { get; set; }
    }

    public class InvAttachmentFromDto
    {
        public IFormFile File { get; set; } = null!;
        public long InvoiceID { get; set; }
    }

    public class GetAllInvAttachmentDto
    {
        public long InvoiceAttachnmentID { get; set; }
        public string? OriginalFileName { get; set; }
        public string? StorageFileName { get; set; }
    }
}