using CbsAp.Domain.Common;
using CbsAp.Domain.Common.Interfaces;

namespace CbsAp.Domain.Entities.UserManagement
{
    public class UserLogInfo : BaseAuditableEntity, IUserIdEntity
    {
        public long UserLogInfoID { get; set; }

        public string? PasswordHash { get; set; }

        public string? PasswordSalt { get; set; }

        //Authenticatioon and Recovery model
        public string? ConfirmationToken { get; set; }
        public DateTimeOffset? TokenGenerationTime { get; set; }
        public string? PasswordrecoveryToken { get; set; }
        public bool? IsPasswordRecoveryTokenUsed { get; set; }
        public bool ActivateNewUser { get; set; }

        public DateTimeOffset? RecoveryTokenTime { get; set; }

        public DateTimeOffset? LastLoginDateTime { get; set; }
        public int FailedLoginAttempts { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTimeOffset? LockoutEndUtc { get; set; }


        public string UserID { get; set; }
        public long UserAccountID { get; set; }

      
        public virtual UserAccount UserAccount { get; set; }
    }
}
