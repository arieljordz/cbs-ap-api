using CbsAp.Domain.Common;
using CbsAp.Domain.Common.Interfaces;

namespace CbsAp.Domain.Entities.PermissionManagement
{
    public class Permission : BaseAuditableEntity, IIsActiveEntity
    {
        public long PermissionID { get; set; }
        public string PermissionName { get; set; }
        public bool IsActive { get; set; }
       
        public virtual ICollection<PermissionGroup> PermissionGroups { get; set; }
        public virtual ICollection<RolePermissionGroup> RolePermissionGroups { get; set; }
    }
}