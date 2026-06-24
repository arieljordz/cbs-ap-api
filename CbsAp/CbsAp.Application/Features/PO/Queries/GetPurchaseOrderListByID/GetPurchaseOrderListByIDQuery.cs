using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.PO.Queries.GetPurchaseOrderListByID
{
    public record GetPurchaseOrderListByIDQuery(long purchaseOrderId,
    int pageNumber,
    int pageSize,
    string? sortField,
    int? sortOrder,
    string? searchLine,
    CancellationToken token) : IQuery<ResponseResult<PaginatedList<PurchaseHeaderLineDetailsDto>>>
    {
    }
}
