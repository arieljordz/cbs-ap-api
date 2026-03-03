using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.PO;

namespace CbsAp.Domain.Entities.TaxCodes
{
    public class TaxCode : BaseAuditableEntity
    {
        public long TaxCodeID { get; set; }

        public long EntityID { get; set; }

        public string? TaxCodeName { get; set; }

        public string? Code { get; set; }

        public decimal TaxRate { get; set; }

        public virtual EntityProfile EntityProfile { get; set; }

        public virtual ICollection<PurchaseOrderLine>? PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();
    }
}