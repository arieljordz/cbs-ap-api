using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.AssignRoleRoutingFlow
{
    public record AssignRoleRoutingFlowCommand(
        RoleRoutingFlowDTO RoleRoutingFlowDTO,
        string assignedBy) : ICommand<ResponseResult<bool>>

    {
    }
}
