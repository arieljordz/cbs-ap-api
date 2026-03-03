using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Commands.UpdateRoutingFlow
{
    public record UpdateRoutingFlowCommand(InvRoutingFlowDto InvRoutingFlowDto, string UpdateBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}