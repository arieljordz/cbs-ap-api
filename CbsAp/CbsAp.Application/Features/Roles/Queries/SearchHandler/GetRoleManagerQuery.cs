using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Queries.SearchHandler
{
    public record GetRoleManagerQuery(string? Name, string? UserID) :
        IQuery<ResponseResult<IEnumerable<RoleManagerDTO>>>
    {
    }
}