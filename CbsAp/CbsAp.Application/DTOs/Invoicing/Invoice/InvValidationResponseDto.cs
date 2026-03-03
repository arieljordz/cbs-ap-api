using CbsAp.Domain.Enums;

namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    public class InvValidationResponseDto
    {
        public InvoiceQueueType QueueType { get; set; }
        public string InvoiceActionType { get; set; } = string.Empty;
        public string FailureMessages { get; set; } = string.Empty;
    }
}