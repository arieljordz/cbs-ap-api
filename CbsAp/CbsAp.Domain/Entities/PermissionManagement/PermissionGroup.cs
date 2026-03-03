using CbsAp.Domain.Common;
using System.Security;

namespace CbsAp.Domain.Entities.PermissionManagement
{
    public class PermissionGroup : BaseAuditableEntity
    {
        public long PermissionGroupID { get; set; }

        public long PermissionID { get; set; }
        public long OperationID { get; set; }
        public bool Access { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual Operation Operations { get; set; }
        public virtual ICollection<RolePermissionGroup> RolePermissions { get; set; }
    }
}