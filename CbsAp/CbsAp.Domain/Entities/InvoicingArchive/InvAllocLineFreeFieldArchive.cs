using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.InvoicingArchive
{
    public class InvAllocLineFreeFieldArchive : BaseAuditableEntity
    {
        public long InvAllocLineFieldID { get; set; }
        public long? InvAllocLineID { get; set; }
        public string? FieldKey { get; set; }
        public string? FieldValue { get; set; }
        public virtual InvAllocLineArchive? AllocationLine { get; set; }
    }
}
