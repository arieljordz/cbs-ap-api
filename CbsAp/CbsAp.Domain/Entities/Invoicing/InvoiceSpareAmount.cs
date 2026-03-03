using CbsAp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvoiceSpareAmount : BaseAuditableEntity
    {
        public long InvoiceSpareAmountID { get; set; }
        public long? InvoiceID { get; set; }
        public string? FieldKey { get; set; } 
        public string? FieldValue { get; set; } 
        public virtual Invoice? Invoice { get; set; }
    }
}
