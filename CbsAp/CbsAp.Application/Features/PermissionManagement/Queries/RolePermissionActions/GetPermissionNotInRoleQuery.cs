using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PermissionManagement.Queries.RolePermissionActions
{
    public record GetPermissionNotInRoleQuery(long RoleID)
        : IQuery<ResponseResult<IQueryable<PermissionDetailDTO>>>
    {
    }
}