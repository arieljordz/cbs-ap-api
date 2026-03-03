using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.TaxCodes;

namespace CbsAp.Domain.Entities.InvoicingArchive
{
    public class InvAllocLineArchive : BaseAuditableEntity
    {
        public long InvAllocLineID { get; set; }
        public long? InvoiceID { get; set; }
        public long? LineNo { get; set; }
        public string? PoNo { get; set; }
        public string? PoLineNo { get; set; }
        public long? AccountID { get; set; }
        public string? LineDescription { get; set; }
        public decimal Qty { get; set; }
        public decimal LineNetAmount { get; set; }
        public decimal LineTaxAmount { get; set; }
        public decimal LineAmount { get; set; }
        public string? Currency { get; set; }
        public long? TaxCodeID { get; set; }
        public string? LineApproved { get; set; }
        public string? Note { get; set; }

        public virtual InvoiceArchive? Invoice { get; set; }
        public virtual TaxCode? TaxCode { get; set; }

        public virtual Account? Account { get; set; }

        public virtual ICollection<InvAllocLineFreeFieldArchive> FreeFields { get; set; } = new List<InvAllocLineFreeFieldArchive>();
        public virtual ICollection<InvAllocLineDimensionArchive> Dimensions { get; set; } = new List<InvAllocLineDimensionArchive>();

        public virtual ICollection<PurchaseOrderMatchTrackingArchive> PurchaseOrderMatchTrackings { get; set; } = new List<PurchaseOrderMatchTrackingArchive>();
    }
}
