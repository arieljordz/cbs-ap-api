using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Entities.TaxCodes;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class InvAllocLine : BaseAuditableEntity
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

        public virtual Invoice? Invoice { get; set; }
        public virtual TaxCode? TaxCode { get; set; }

        public virtual Account? Account { get; set; }

        public virtual ICollection<InvAllocLineFreeField> FreeFields { get; set; } = new List<InvAllocLineFreeField>();
        public virtual ICollection<InvAllocLineDimension> Dimensions { get; set; } = new List<InvAllocLineDimension>();

        public virtual ICollection<PurchaseOrderMatchTracking> PurchaseOrderMatchTrackings { get; set; } = new List<PurchaseOrderMatchTracking>();
    }
}