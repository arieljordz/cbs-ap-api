using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.TaxCodes;

namespace CbsAp.Domain.Entities.PO
{
    public class PurchaseOrderLine : BaseAuditableEntity
    {
        public long PurchaseOrderLineID { get; set; }

        public long PurchaseOrderID { get; set; }

        public long? TaxCodeID { get; set; }
        public long? AccountID { get; set; }

        public long? LineNo { get; set; }

        public string? Description { get; set; }

        public decimal Qty { get; set; }

        public string? Unit { get; set; }

        public decimal? Price { get; set; }

        public decimal? InvoicedPrice { get; set; }

        public decimal? Amount { get; set; }

        public decimal? NetAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public string? Item { get; set; }

        public bool? IsActive { get; set; }

        public string? FreeField1 { get; set; }

        public string? FreeField2 { get; set; }

        public string? FreeField3 { get; set; }

        public decimal? SpareAmount1 { get; set; }

        public decimal? SpareAmount2 { get; set; }

        public decimal? SpareAmount3 { get; set; }

        public bool? FullyInvoiced { get; set; }

        public virtual PurchaseOrder? PurchaseOrder { get; set; }

        public virtual TaxCode? TaxCode { get; set; }

        public virtual Account? Account { get; set; }

        public virtual ICollection<PurchaseOrderMatchTracking>? PurchaseOrderMatchTrackings { get; set; } = new List<PurchaseOrderMatchTracking>();
    }
}