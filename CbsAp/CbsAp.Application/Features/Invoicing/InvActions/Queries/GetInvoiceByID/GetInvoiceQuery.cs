using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.GetInvoiceByID
{
    public record GetInvoiceQuery(long InvoiceID) : IQuery<ResponseResult<InvoiceDto>>
    {
    }
}