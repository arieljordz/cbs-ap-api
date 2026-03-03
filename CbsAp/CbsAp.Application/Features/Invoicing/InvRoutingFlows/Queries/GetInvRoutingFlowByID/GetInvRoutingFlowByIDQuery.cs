using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Queries.GetInvRoutingFlowByID
{
    public record GetInvRoutingFlowByIDQuery(long InvRoutingFlowID)
        : IQuery<ResponseResult<InvRoutingFlowDto>>

    {
    }
}