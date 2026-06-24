using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PO.Queries.GetPurchaseOrderListByID
{
    public class GetPurchaseOrderListByIDQueryHandler : IQueryHandler<GetPurchaseOrderListByIDQuery, ResponseResult<PaginatedList<PurchaseHeaderLineDetailsDto>>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderListByIDQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<ResponseResult<PaginatedList<PurchaseHeaderLineDetailsDto>>> Handle(GetPurchaseOrderListByIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _purchaseOrderRepository.GetPurchaseOrderListByID(request.purchaseOrderId,request.pageNumber,
                request.pageSize,
                request.sortField,
                 request.sortOrder,request.searchLine,
                cancellationToken)!;
            return result == null ?
                ResponseResult<PaginatedList<PurchaseHeaderLineDetailsDto>>.BadRequest("Purchase order list not found") :
                ResponseResult<PaginatedList<PurchaseHeaderLineDetailsDto>>.OK(result);
        }
    }
}