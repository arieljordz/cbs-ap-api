using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Queries.SearchHandler
{
    public record GetActiveRolesQuery(
        string? RoleName,
        string? FirstName,
        string? LastName) :
        IQuery<ResponseResult<IEnumerable<RoleManagerDTO>>>
    {
    }
}