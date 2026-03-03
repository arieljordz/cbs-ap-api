using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.RoleManagement;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvRoutingFlowLevels : BaseAuditableEntity
    {
        public long InvRoutingFlowLevelID { get; set; }

        public long InvRoutingFlowID { get; set; }

        public long RoleID { get; set; }

        public int Level { get; set; }

        public virtual Role? Role { get; set; }
        public virtual InvRoutingFlow? InvRoutingFlow { get; set; }
    }
}