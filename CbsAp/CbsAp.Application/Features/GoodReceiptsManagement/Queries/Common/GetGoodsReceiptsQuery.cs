using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.GoodsReceiptsManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.GoodReceiptsManagement.Queries.Common
{
    public record GetGoodsReceiptsQuery(
        string? Entity,
        string? Supplier,
        string? GoodsReceiptNumber,
        bool? Active,
        DateTimeOffset? DeliveryDateFrom,
        DateTimeOffset? DeliveryDateTo,
        int PageNumber,
        int PageSize,
        string? SortField,
        int? SortOrder) : IQuery<ResponseResult<PaginatedList<GoodsReceiptDTO>>>;
}
