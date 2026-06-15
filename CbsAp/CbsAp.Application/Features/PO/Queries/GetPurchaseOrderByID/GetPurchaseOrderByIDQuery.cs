using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.PO.Queries.GetPurchaseOrderByID
{
    public record GetPurchaseOrderByIDQuery(long purchaseOrderID) : IQuery<ResponseResult<PurchaseOrderHeaderDto>>
    {
    }
}
