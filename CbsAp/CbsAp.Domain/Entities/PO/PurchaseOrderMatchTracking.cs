using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;

namespace CbsAp.Domain.Entities.PO
{
    public class PurchaseOrderMatchTracking : BaseAuditableEntity
    {
        public long PurchaseOrderMatchTrackingID { get; set; }

        public long PurchaseOrderLineID { get; set; }

        public virtual PurchaseOrderLine? PurchaseOrderLine { get; set; }

        public long PurchaseOrderID { get; set; }

        public virtual PurchaseOrder? PurchaseOrder { get; set; }

        public long? InvoiceID { get; set; }

        public virtual Invoice? Invoice { get; set; }

        public long? InvAllocLineID { get; set; }

        public virtual InvAllocLine? InvAllocLine { get; set; }

        public string? Account { get; set; }

        public decimal Qty { get; set; }

        public decimal? RemainingQty { get; set; }

        public decimal? Amount { get; set; }

        public decimal? NetAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public DateTimeOffset MatchingDate { get; set; }

        public bool IsSystemMatching { get; set; }

        public POMatchingStatus? MatchingStatus { get; set; }
    }
}
