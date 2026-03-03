using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Commands.DeleteRoutingFlow
{
    public record DeleteRoutingFlowCommand(long InvRoutingFlowID) : ICommand<ResponseResult<bool>>
    {
       
       
    }
}