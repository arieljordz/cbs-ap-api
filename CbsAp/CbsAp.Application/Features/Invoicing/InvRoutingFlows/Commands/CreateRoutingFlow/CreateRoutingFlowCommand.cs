using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Commands.CreateRoutingFlow
{
    public record CreateRoutingFlowCommand(InvRoutingFlowDto InvRoutingFlowDto, string createdBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}