using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CbsAp.Domain.Common;

namespace CbsAp.Domain.Entities.Supplier
{
    public class SupplierBankAccount : BaseAuditableEntity  
    {
        public long SupplierBankAccountID { get; set; }
        public long SupplierInfoID { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public bool IsActive { get; set; }
        public virtual SupplierInfo? SupplierInfo { get; set; }
    }
}
