using CbsAp.Domain.Common;
using CbsAp.Domain.Common.Interfaces;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Supplier;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvRoutingFlow : BaseAuditableEntity, IIsActiveEntity
    {
        public long InvRoutingFlowID { get; set; }

        public string? InvRoutingFlowName { get; set; }

        public long? EntityProfileID { get; set; }

        public long? SupplierInfoID { get; set; }

        public string? MatchReference { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<InvRoutingFlowLevels>? Levels { get; set; }

        public virtual SupplierInfo? SupplierInfo { get; set; }

        public virtual EntityProfile? EntityProfile { get; set; }

        public virtual ICollection<InvInfoRoutingLevel>? InvInfoRoutingLevels { get; set; }
    }
}