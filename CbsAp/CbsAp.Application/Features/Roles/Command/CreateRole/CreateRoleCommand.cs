using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Command.CreateRole
{
    // Command for  Role Basic Information
    public record CreateRoleCommand(CreateRoleDTO roleDTO, string CreatedBy) :
        ICommand<ResponseResult<string>>
    {
    }
}