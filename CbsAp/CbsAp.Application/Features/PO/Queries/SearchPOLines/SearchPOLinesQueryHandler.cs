using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace CbsAp.Application.Features.PO.Queries.SearchPOLines
{
    public class SearchPOLinesQueryHandler : IQueryHandler<SearchPOLinesQuery, ResponseResult<List<SearchPoLinesDto>>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public SearchPOLinesQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<ResponseResult<List<SearchPoLinesDto>>> Handle(SearchPOLinesQuery request, CancellationToken cancellationToken)
        {
            var result = await _purchaseOrderRepository.SearchPoLines(
                 request.SupplierName,
                 request.SupplierTaxID,
                 request.PONo,
                 request.PODateFrom,
                 request.PODateTo,
                 request.DeliveryNo,
                 request.SupplierNo,
                 request.ExcludesMatchPOLineIds,
                 request.IsAvailableOrder,
                // bool? IsAvailableOrder  TODO
            
                cancellationToken
                        );

           
            return result == null ?
                ResponseResult<List<SearchPoLinesDto>>.NotFound("No PoLines") :
                ResponseResult<List<SearchPoLinesDto>>.SuccessRetrieveRecords(result);
        }
    }
}