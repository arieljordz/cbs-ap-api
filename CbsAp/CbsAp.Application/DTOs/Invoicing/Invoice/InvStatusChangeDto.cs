using CbsAp.Domain.Enums;

namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    public class InvStatusChangeDto
    {
        public long InvoiceID { get; set; }

        public InvoiceStatusType? Status { get; set; }

        public string? Reason { get; set; }
    }

    public class GetInvoiceStatusDto
    {
        public InvoiceStatusType? Status { get; set; }
        public InvoiceQueueType? Queue { get; set; }
    }
}
