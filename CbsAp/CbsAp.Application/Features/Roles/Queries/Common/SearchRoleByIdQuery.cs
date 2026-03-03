using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Queries.Common
{
    public record SearchRoleByIdQuery(long roleID) :
        IQuery<ResponseResult<SearchRoleDtO>>;
}
