using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.TaxCodes;

namespace CbsAp.Domain.Entities.Supplier
{
    public class SupplierInfo : BaseAuditableEntity
    {
        public long SupplierInfoID { get; set; }

        public string? SupplierID { get; set; }

        public string? SupplierTaxID { get; set; }

        public long? EntityProfileID { get; set; }

        public virtual EntityProfile? EntityProfile { get; set; }

        public string? SupplierName { get; set; }

        public bool IsActive { get; set; }

        public string? Telephone { get; set; }

        public string? EmailAddress { get; set; }

        public string? Contact { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? AddressLine3 { get; set; }

        public string? AddressLine4 { get; set; }

        public string? AddressLine5 { get; set; }

        public string? AddressLine6 { get; set; }

        public long? AccountID { get; set; }

        public virtual Account? Account { get; set; }

        public long? TaxCodeID { get; set; }

        public virtual TaxCode? TaxCode { get; set; }

        public string? Currency { get; set; }

        public string? PaymentTerms { get; set; }

        public long? InvRoutingFlowID { get; set; }

        public virtual InvRoutingFlow? InvRoutingFlow { get; set; }

        public string? FreeField1 { get; set; }

        public string? FreeField2 { get; set; }

        public string? FreeField3 { get; set; }

        public string? Notes { get; set; }

        public virtual ICollection<InvInfoRoutingLevel>? InvInfoRoutingLevels { get; set; }
    }
}
