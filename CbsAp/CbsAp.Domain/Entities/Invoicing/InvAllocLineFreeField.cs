using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvAllocLineFreeField : BaseAuditableEntity
    {
        public long InvAllocLineFieldID { get; set; }
        public long? InvAllocLineID { get; set; }
        public string? FieldKey { get; set; }
        public string? FieldValue { get; set; }
        public virtual InvAllocLine? AllocationLine { get; set; }
    }
}