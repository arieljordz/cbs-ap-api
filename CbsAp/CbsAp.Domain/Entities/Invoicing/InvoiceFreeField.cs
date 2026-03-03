using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvoiceFreeField : BaseAuditableEntity
    {
        public long InvoiceFreeFieldID { get; set; }
        public long? InvoiceID { get; set; }
        public string? FieldKey { get; set; }
        public string? FieldValue { get; set; }
        public virtual Invoice? Invoice { get; set; }
    }
}