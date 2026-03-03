using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvoiceAttachnment : BaseAuditableEntity
    {
        public long InvoiceAttachnmentID { get; set; }
        public long InvoiceID { get; set; }
        public string? OriginalFileName { get; set; }
        public string? StorageFileName { get; set; }
        public string? FileType { get; set; }
        public virtual Invoice? Invoice { get; set; }
    }
}