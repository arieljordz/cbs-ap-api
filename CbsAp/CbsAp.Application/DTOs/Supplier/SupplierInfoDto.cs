using CbsAp.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CbsAp.Application.DTOs.Supplier
{
    public class SupplierInfoDto : BaseAuditableEntity
    {
        public long SupplierInfoID { get; set; }

        public string? SupplierID { get; set; }

        public string? SupplierTaxID { get; set; }

        public long? EntityProfileID { get; set; }

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

        public long? TaxCodeID { get; set; }

        public string? Currency { get; set; }
        public string? PaymentTerms { get; set; }

        public long? InvRoutingFlowID { get; set; }

        public string? FreeField1 { get; set; }

        public string? FreeField2 { get; set; }

        public string? FreeField3 { get; set; }

        public string? Notes { get; set; }

        [JsonIgnore]
        public string? CreatedBy { get; set; }

        [JsonIgnore]
        public DateTimeOffset? CreatedDate { get; set; }
        [JsonIgnore]

        public string? LastUpdatedBy { get; set; }
        [JsonIgnore]
        public DateTimeOffset? LastUpdatedDate { get; set; }
    }
}
