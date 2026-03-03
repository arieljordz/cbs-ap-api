using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Queries.UserRoleActions
{
    public class ActiveUsersQuery :
        IQuery<ResponseResult<IQueryable<ActiveUsersDTO>>>
    {
    }
}