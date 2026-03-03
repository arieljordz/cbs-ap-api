using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Shared
{
    public static class InvoicActionLogFactory
    {
        public static InvoiceActivityLog CreateInvoiceActivityLog(InvStatusChangeDto? dto,
             InvoiceStatusType? previouInvStatus, InvoiceActionType action)
        {

            var actityLog = new InvoiceActivityLog
            {
                InvoiceID = dto!.InvoiceID,
                PreviousStatus = previouInvStatus,
                CurrentStatus = dto.Status,
                Reason = dto.Reason,
                Action = action,

            };

            return actityLog;
        }
    }
}
