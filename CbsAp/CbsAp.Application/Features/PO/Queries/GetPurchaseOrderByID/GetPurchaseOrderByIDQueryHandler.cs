using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PO.Queries.GetPurchaseOrderByID
{
    public class GetPurchaseOrderByIDQueryHandler : IQueryHandler<GetPurchaseOrderByIDQuery, ResponseResult<PurchaseOrderHeaderDto>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderByIDQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<ResponseResult<PurchaseOrderHeaderDto>> Handle(GetPurchaseOrderByIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _purchaseOrderRepository.GetPurchaseOrderByID(request.purchaseOrderID)!;
            return result == null ?
                ResponseResult<PurchaseOrderHeaderDto>.BadRequest("Purchase order not found") :
                ResponseResult<PurchaseOrderHeaderDto>.OK(result);
        }
    }
}