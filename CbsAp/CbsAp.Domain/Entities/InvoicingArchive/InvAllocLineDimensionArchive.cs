using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.InvoicingArchive
{
    public class InvAllocLineDimensionArchive : BaseAuditableEntity
    {
        public long InvAllocLineDimensionID { get; set; }
        public long? InvAllocLineID { get; set; }
        public string DimensionKey { get; set; } = default!;
        public string DimensionValue { get; set; } = default!;

        public virtual InvAllocLineArchive? AllocationLine { get; set; } = default!;
    }
}
