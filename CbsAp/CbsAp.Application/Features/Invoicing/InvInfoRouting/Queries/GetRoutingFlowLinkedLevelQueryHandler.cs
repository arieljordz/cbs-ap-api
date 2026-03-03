using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvInfoRouting.Queries
{
    public class GetRoutingFlowLinkedLevelQueryHandler :
        IQueryHandler<GetRoutingFlowLinkedLevelQuery, ResponseResult<List<InvInfoRoutingLevelDto>>>
    {
        private readonly IInvRoutingFlowRepository _invRoutingFlowRepository;

        public GetRoutingFlowLinkedLevelQueryHandler( IInvRoutingFlowRepository invRoutingFlowRepository)
        {
            _invRoutingFlowRepository = invRoutingFlowRepository;
        }
        public async Task<ResponseResult<List<InvInfoRoutingLevelDto>>> Handle(GetRoutingFlowLinkedLevelQuery request, CancellationToken cancellationToken)
        {
            var result = await _invRoutingFlowRepository
                .GetInvInfoRoutingFlow(
                request.InvoiceID,
                request.KeywordID,
                request.SupplierInfoID,
                cancellationToken);

            return result.Count == 0 ?
                ResponseResult<List<InvInfoRoutingLevelDto>>.NotFound("No Invoice Routing Flow") :
                ResponseResult<List<InvInfoRoutingLevelDto>>.SuccessRetrieveRecords(result);
        }
    }
}