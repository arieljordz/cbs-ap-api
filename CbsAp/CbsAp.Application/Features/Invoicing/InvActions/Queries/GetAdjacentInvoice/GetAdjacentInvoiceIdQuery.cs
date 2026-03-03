using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.GetAdjacentInvoice
{
    public record GetAdjacentInvoiceIdQuery(
        long InvoiceID,
        bool IsNext,
        InvoiceStatusType? StatusType = null,
        InvoiceQueueType? QueueType = null) : IQuery<ResponseResult<long?>>
    {
    }
}
