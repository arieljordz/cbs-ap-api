using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.DTOs.PermissionManagement.OperationDTO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PermissionManagement.Commands.UpdatePermission
{
    public record UpdatePermissionCommand(UpdatePermissionDTO updatePermissionDTO, 
        string updatedBy
        ) : ICommand<ResponseResult<string>>;
}