using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Entity;

namespace CbsAp.Domain.Entities.RoleManagement
{
    public class RoleEntity : BaseAuditableEntity
    {
        public long RoleEntityID { get; set; }
        public long EntityProfileID { get; set; }
        public long RoleID { get; set; }
        public virtual Role Role { get; set; }
        public virtual EntityProfile EntityProfile { get; set; }
    }
}