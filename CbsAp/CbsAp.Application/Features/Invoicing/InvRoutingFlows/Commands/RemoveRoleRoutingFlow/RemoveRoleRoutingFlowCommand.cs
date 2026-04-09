using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.RemoveRoleRoutingFlow
{
    public record RemoveRoleRoutingFlowCommand(
        RoleRoutingFlowDTO RoleRoutingFlowDTO,
        string removedBy) : ICommand<ResponseResult<string>>

    {
    }
}
