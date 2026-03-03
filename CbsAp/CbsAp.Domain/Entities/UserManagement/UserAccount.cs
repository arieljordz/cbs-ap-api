using CbsAp.Domain.Common;
using CbsAp.Domain.Common.Interfaces;
using CbsAp.Domain.Entities.RoleManagement;
using System.ComponentModel.DataAnnotations;

namespace CbsAp.Domain.Entities.UserManagement
{
    public class UserAccount : BaseAuditableEntity, IIsActiveEntity, IUserIdEntity
    {
        public long UserAccountID { get; set; }

        public string UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public bool IsActive { get; set; }

        public bool IsUserPartialDeleted {  get; set; }

        public virtual UserLogInfo UserLogInfo { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<PasswordHistory> PasswordHistories { get; set; }

        public virtual ICollection<PasswordResetAudit> PasswordResetAudits { get; set; }

    }
}