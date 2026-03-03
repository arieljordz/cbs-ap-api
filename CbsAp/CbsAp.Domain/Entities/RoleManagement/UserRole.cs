using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.UserManagement;

namespace CbsAp.Domain.Entities.RoleManagement
{
    public class UserRole : BaseAuditableEntity
    {
        public long UserRoleID { get; set; }
        public long UserAccountID { get; set; }
        public long RoleID { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual Role Role { get; set; }
    }
}