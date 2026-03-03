using CbsAp.Domain.Common;
using CbsAp.Domain.Enums;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvoiceActivityLog : BaseAuditableEntity
    {
        public long ActivityLogID { get; set; }

        public long InvoiceID { get; set; }

        public InvoiceStatusType? PreviousStatus { get; set; }

        public InvoiceStatusType? CurrentStatus { get; set; }

        public string? Reason { get; set; }
        public InvoiceActionType? Action { get; set; }

        public bool? IsCurrentValidationContext { get; set; }

        public virtual Invoice? Invoice { get; set; }
    }
}
