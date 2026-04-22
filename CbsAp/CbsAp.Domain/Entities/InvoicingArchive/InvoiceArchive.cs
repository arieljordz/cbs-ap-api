using CbsAp.Domain.Common;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;

namespace CbsAp.Domain.Entities.InvoicingArchive
{
    public class InvoiceArchive : BaseAuditableEntity
    {
        public long InvoiceID { get; set; }

        public string? InvoiceNo { get; set; }

        public DateTimeOffset? InvoiceDate { get; set; }

        public string? MapID { get; set; }

        public string? ImageID { get; set; }

        public DateTimeOffset? ScanDate { get; set; }

        public long? EntityProfileID { get; set; }

        public long? SupplierInfoID { get; set; }

        public long? KeywordID { get; set; }

        public string? SuppBankAccount { get; set; }

        public DateTimeOffset? DueDate { get; set; }

        public string? PoNo { get; set; }

        public string? GrNo { get; set; }

        public string? Currency { get; set; }

        public decimal NetAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public long TaxCodeID { get; set; }

        public string? PaymentTerm { get; set; }

        public string? Note { get; set; }

        public long? ApproverRole { get; set; }

        public long? ApprovedUser { get; set; }

        public FreeFieldSets FreeFields { get; set; } = new();

        public SpareAmountSets SpareAmount { get; set; } = new();

        public InvoiceQueueType? QueueType { get; set; }

        public InvoiceStatusType? StatusType { get; set; }

        // These navigation references stay pointed at master data; no archive variant required.
        public virtual EntityProfile? EntityProfile { get; set; }

        public virtual SupplierInfo? SupplierInfo { get; set; }

        public virtual Keyword? Keyword { get; set; }

        public virtual TaxCode? TaxCode { get; set; }

        public virtual Role? ApproverInvoices { get; set; }
        public virtual Role? ApprovedUserInvoices { get; set; }

        public virtual ICollection<InvAllocLineArchive>? InvoiceAllocationLines { get; set; } = new List<InvAllocLineArchive>();

        public virtual ICollection<InvoiceCommentArchive>? InvoiceComments { get; set; } = new List<InvoiceCommentArchive>();

        public virtual ICollection<InvoiceAttachnmentArchive>? InvoiceAttachnments { get; set; } = new List<InvoiceAttachnmentArchive>();

        public virtual ICollection<InvoiceActivityLogArchive>? InvoiceActivityLog { get; set; } = new List<InvoiceActivityLogArchive>();

        public virtual ICollection<PurchaseOrderMatchTrackingArchive>? PurchaseOrderMatchTrackings { get; set; } = new List<PurchaseOrderMatchTrackingArchive>();
    }
}
