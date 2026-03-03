using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.InvoicingArchive
{
    public class InvoiceFreeFieldArchive : BaseAuditableEntity
    {
        public long InvoiceFreeFieldID { get; set; }
        public long? InvoiceID { get; set; }
        public string? FieldKey { get; set; }
        public string? FieldValue { get; set; }
        public virtual InvoiceArchive? Invoice { get; set; }
    }
}
