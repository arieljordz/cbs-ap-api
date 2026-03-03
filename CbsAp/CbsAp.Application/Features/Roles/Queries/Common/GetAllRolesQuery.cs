using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Queries.Common
{
    public record GetAllRolesQuery() : IQuery<ResponseResult<IEnumerable<SearchRoleDtO>>>;
}