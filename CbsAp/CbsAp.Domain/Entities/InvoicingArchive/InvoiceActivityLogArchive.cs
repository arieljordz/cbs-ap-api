using CbsAp.Domain.Common;
using CbsAp.Domain.Enums;

namespace CbsAp.Domain.Entities.InvoicingArchive
{
    public class InvoiceActivityLogArchive : BaseAuditableEntity
    {
        public long ActivityLogID { get; set; }

        public long InvoiceID { get; set; }

        public InvoiceStatusType? PreviousStatus { get; set; }

        public InvoiceStatusType? CurrentStatus { get; set; }

        public string? Reason { get; set; }
        public InvoiceActionType? Action { get; set; }

        public virtual InvoiceArchive? Invoice { get; set; }
    }
}
