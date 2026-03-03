using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Queries.UserRoleActions
{
    public record GetUsersByRoleQuery(long RoleID)
        : IQuery<ResponseResult<IQueryable<ActiveUsersDTO>>>
    {
    }
}