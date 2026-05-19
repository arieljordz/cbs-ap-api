using CbsAp.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CbsAp.Application.FakeStoreData.FakeDTO
{
    public class FakeInvoiceDto
    {
        [NotMapped]
        public long InvoiceID { get; set; }

        public string? InvoiceNo { get; set; }
        public DateTimeOffset? InvoiceDate { get; set; }
        public long? EntityProfileID { get; set; }
        public long TaxCodeID { get; set; }
        public long? SupplierInfoID { get; set; }
        public long? KeywordID { get; set; }
        public string? Currency { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? PurchaseOrderNo { get; set; }
        public string? GoodsReceiptNo { get; set; }
        public InvoiceQueueType? QueueType { get; set; }
        public InvoiceStatusType? StatusType { get; set; } = InvoiceStatusType.Validation;
    }
}