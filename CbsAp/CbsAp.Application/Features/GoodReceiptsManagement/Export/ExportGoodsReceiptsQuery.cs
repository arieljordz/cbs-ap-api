using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.GoodReceiptsManagement.Export
{
    public record ExportGoodsReceiptsQuery(
        string? EntityName,
        string? SupplierName,
        string? GoodsReceiptNumber,
        bool? Active,
        DateTimeOffset? DeliveryDateFrom,
        DateTimeOffset? DeliveryDateTo) : IQuery<ResponseResult<byte[]>>;
}
