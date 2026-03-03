using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.InvoicingArchive
{
    public class InvoiceAttachnmentArchive : BaseAuditableEntity
    {
        public long InvoiceAttachnmentID { get; set; }
        public long InvoiceID { get; set; }
        public string? OriginalFileName { get; set; }
        public string? StorageFileName { get; set; }
        public string? FileType { get; set; }
        public virtual InvoiceArchive? Invoice { get; set; }
    }
}
