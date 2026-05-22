using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Supplier
{
    public class SupplierBankAccountDto
    {
        public long SupplierBankAccountID { get; set; }
        public long SupplierInfoID { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public bool IsActive { get; set; }
    }
}
