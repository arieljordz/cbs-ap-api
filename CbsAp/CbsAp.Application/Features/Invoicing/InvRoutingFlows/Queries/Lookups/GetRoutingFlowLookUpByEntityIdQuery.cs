using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Queries.Lookups
{
    public record GetRoutingFlowLookUpByEntityIdQuery(long EntityID)
        : IQuery<ResponseResult<IEnumerable<InvRoutingFlowLookupDto>>>
    {
    }
}
