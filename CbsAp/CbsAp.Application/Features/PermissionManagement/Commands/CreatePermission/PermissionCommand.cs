using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PermissionManagement.Commands.CreatePermission
{
    public record PermissionCommand(CreatePermissionDto CreatePermissionDTO,
         string? CreatedBy)
        : ICommand<ResponseResult<string>>
    {
    }
}