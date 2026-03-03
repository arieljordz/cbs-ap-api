using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvAllocLineDimension : BaseAuditableEntity
    {
        public long InvAllocLineDimensionID { get; set; }
        public long? InvAllocLineID { get; set; }
        public string DimensionKey { get; set; } = default!;
        public string DimensionValue { get; set; } = default!;

        public virtual InvAllocLine? AllocationLine { get; set; } = default!;
    }
}