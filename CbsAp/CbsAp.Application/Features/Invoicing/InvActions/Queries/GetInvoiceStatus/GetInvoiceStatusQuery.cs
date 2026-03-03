using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.GetInvoiceStatus
{
    public record GetInvoiceStatusQuery(long invoiceID)
        : IQuery<ResponseResult<GetInvoiceStatusDto>>
    {
    }
}