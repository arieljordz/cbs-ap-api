using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActivityLog.Queries
{
    public record GetInvActivityLogQuery(long InvoiceID) : IQuery<ResponseResult<List<InvActivityLogDto>>>
    {
    }
}
