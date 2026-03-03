using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Domain.Entities.Keywords
{
    public class Keyword : BaseAuditableEntity
    {
        public long KeywordID { get; set; }

        public long? EntityProfileID { get; set; }
        public long InvoiceRoutingFlowID { get; set; }
        public string KeywordName { get; set; }
        public bool IsActive { get; set; }
        public virtual EntityProfile EntityProfile { get; set; } = null!;
        public virtual InvRoutingFlow InvRoutingFlow { get; set; } = null!;
        public virtual ICollection<InvInfoRoutingLevel>? InvInfoRoutingLevels { get; set; }
    }
}
