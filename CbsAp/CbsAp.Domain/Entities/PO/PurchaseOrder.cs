using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Supplier;

namespace CbsAp.Domain.Entities.PO
{
    public class PurchaseOrder : BaseAuditableEntity
    {
        public long PurchaseOrderID { get; set; }

        public string? PoNo { get; set; }

        public DateTime PurchaseDate { get; set; }

        public long? EntityProfileID { get; set; }

        public long? SupplierInfoID { get; set; }

        public string? SupplierTaxID { get; set; }
        public string? SupplierNo { get; set; }

        public string? Currency { get; set; }

        public decimal? NetAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool? IsActive { get; set; }

        public string? MatchReference1 { get; set; }

        public string? MatchReference2 { get; set; }

        public string? Note { get; set; }

        public string? FreeField1 { get; set; }

        public string? FreeField2 { get; set; }

        public string? FreeField3 { get; set; }

        public virtual EntityProfile? EntityProfile { get; set; }

        public virtual SupplierInfo? SupplierInfo { get; set; }

        public virtual ICollection<PurchaseOrderLine>? PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();

        public virtual ICollection<PurchaseOrderMatchTracking>? PurchaseOrderMatchTrackings { get; set; } = new List<PurchaseOrderMatchTracking>();

    }
}