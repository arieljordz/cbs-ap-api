using CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    public class InvoiceDto
    {
        public long InvoiceID { get; set; }

        public string? InvoiceNo { get; set; }

        public DateTimeOffset? InvoiceDate { get; set; }

        public string? MapID { get; set; }

        public string? ImageID { get; set; }

        public DateTimeOffset? ScanDate { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public long? EntityProfileID { get; set; }

        public long? SupplierInfoID { get; set; }

        public long? KeywordID { get; set; }
        public string Keyword { get; set; } = string.Empty;

        public string? SuppABN { get; set; }

        public string? SuppName { get; set; }

        public string? SuppBankAccount { get; set; }
        public string? SupplierNo { get; set; }

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

        public string? ApproverRole { get; set; }

        public string? ApprovedUser { get; set; }
        public string? RoutingFlowName { get; set; }

        public InvoiceQueueType? QueueType { get; set; }

        public InvoiceStatusType? StatusType { get; set; }

        public List<InvoiceFreeFieldDto> FreeFields { get; set; } = new();

        public List<InvoiceSpareAmountDto> SpareAmounts { get; set; } = new();

        public List<InvAllocLineDto> InvoiceAllocationLines { get; set; } = new List<InvAllocLineDto>();

        public List<InvInfoRoutingLevelDto> InvInfoRoutingLevels { get; set; } = new List<InvInfoRoutingLevelDto>();
    }

    public class InvoiceFreeFieldDto
    {
        public long InvoiceFreeFieldID { get; set; }
        public long? InvoiceID { get; set; }
        public string? FieldKey { get; set; }

        public string? FieldValue { get; set; }
    }

    public class InvoiceSpareAmountDto
    {
        public long InvoiceSpareAmountID { get; set; }
        public long? InvoiceID { get; set; }
        public string? FieldKey { get; set; }

        public string? FieldValue { get; set; }
    }
}