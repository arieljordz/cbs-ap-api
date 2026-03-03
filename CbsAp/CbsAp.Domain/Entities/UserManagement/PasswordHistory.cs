using CbsAp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Domain.Entities.UserManagement
{
    public class PasswordHistory :BaseAuditableEntity
    {
        public long PasswordHistoryID { get; set; }
        public long UserAccountID { get; set; }
        public string? PasswordHash { get; set; }

        public string? PasswordSalt { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
