using CbsAp.Domain.Common;
using CbsAp.Domain.Common.Interfaces;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.PO;

using CbsAp.Domain.Entities.TaxCodes;

namespace CbsAp.Domain.Entities.Invoicing
{
    public class Account : BaseAuditableEntity, IIsActiveEntity
    {
        public long AccountID { get; set; }

        public string? AccountName { get; set; }

        public bool IsActive { get; set; }

        public long? TaxCodeID { get; set; }

        public string? TaxCodeName { get; set; }

        public bool? IsTaxCodeMandatory { get; set; }

        public string? Dimension1 { get; set; }

        public string? Dimension2 { get; set; }

        public string? Dimension3 { get; set; }

        public string? Dimension4 { get; set; }

        public string? Dimension5 { get; set; }

        public string? Dimension6 { get; set; }

        public string? Dimension7 { get; set; }

        public string? Dimension8 { get; set; }

        public string? FreeField1 { get; set; }

        public string? FreeField2 { get; set; }

        public string? FreeField3 { get; set; }

        public long? EntityProfileID { get; set; }

        public virtual EntityProfile? EntityProfile { get; set; }
        public virtual TaxCode? TaxCode { get; set; }

        public virtual ICollection<InvAllocLine>? InvoiceAllocationLines { get; set; } = new List<InvAllocLine>();

        public virtual ICollection<PurchaseOrderLine>? PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();
    }
}