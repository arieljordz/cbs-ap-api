using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Command.UpdateRole
{
    public record UpdateRoleCommand(
        UpdateRoleDTO UpdateRoleDTO,
        string updatedBy) : ICommand<ResponseResult<string>>

    {
    }
}
