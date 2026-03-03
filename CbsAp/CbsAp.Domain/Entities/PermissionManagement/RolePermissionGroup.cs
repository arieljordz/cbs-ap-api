using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.RoleManagement;

namespace CbsAp.Domain.Entities.PermissionManagement
{
    public class RolePermissionGroup : BaseAuditableEntity
    {
        public long RolePermissionGroupID { get; set; }
        public long PermissionID { get; set; }
        public long RoleID { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}