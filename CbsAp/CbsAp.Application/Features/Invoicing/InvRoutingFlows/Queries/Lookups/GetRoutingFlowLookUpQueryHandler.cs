using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Invoicing;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Queries.Lookups
{
    public class GetRoutingFlowLookUpQueryHandler
        : IQueryHandler<GetRoutingFlowLookUpQuery, ResponseResult<IEnumerable<InvRoutingFlowLookupDto>>>
    {
        private readonly IInvRoutingFlowService _invRoutingFlowService;

        public GetRoutingFlowLookUpQueryHandler(IInvRoutingFlowService invRoutingFlowService)
        {
            _invRoutingFlowService = invRoutingFlowService;
        }

        public async Task<ResponseResult<IEnumerable<InvRoutingFlowLookupDto>>> Handle(GetRoutingFlowLookUpQuery request, CancellationToken cancellationToken)
        {
            var results = await _invRoutingFlowService.GetInvRoutingFlowLookupsAsync();

            return !results.Any() ?
                ResponseResult<IEnumerable<InvRoutingFlowLookupDto>>.NotFound("No invoice routing flow lookup found ")
                : ResponseResult<IEnumerable<InvRoutingFlowLookupDto>>.SuccessRetrieveRecords(results);
        }
    }
}
