using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.PO.Queries.GetPOMatchingByInvID
{
    public class GetPOMatchByInvIDQueryHandler : IQueryHandler<GetPOMatchByInvIDQuery, ResponseResult<List<SearchPoLinesDto>>>

    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        public GetPOMatchByInvIDQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }
        public async Task<ResponseResult<List<SearchPoLinesDto>>> Handle(GetPOMatchByInvIDQuery request, CancellationToken cancellationToken)
        {


            var result = await _purchaseOrderRepository.GetPOMatchingByInvID(
                request.PONo,
                request.InvoiceID,
               cancellationToken
                       );

            return result == null ?
                ResponseResult<List<SearchPoLinesDto>>.NotFound("No PoLines") :
                ResponseResult<List<SearchPoLinesDto>>.SuccessRetrieveRecords(result);
        }
    }
}
