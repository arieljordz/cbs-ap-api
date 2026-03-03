using CbsAp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Domain.Entities.UserManagement
{
    public class PasswordResetAudit : BaseAuditableEntity
    {
        public long PasswordResetAuditID { get; set; }
        public long UserAccountID { get; set; }

        public bool IsSuccessfulLoginAfterReset { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
