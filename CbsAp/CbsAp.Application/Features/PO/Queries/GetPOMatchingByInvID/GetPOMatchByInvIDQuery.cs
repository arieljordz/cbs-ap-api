using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PO.Queries.GetPOMatchingByInvID
{
    public record GetPOMatchByInvIDQuery(string PONo, long InvoiceID) : IQuery<ResponseResult<List<SearchPoLinesDto>>>
    {
    }
}
