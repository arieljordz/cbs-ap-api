using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Queries.SearchActions
{
    public record SearchRolePaginationParamQuery(
            string? Entity,
            string? RoleName,
            bool? IsActive,
            int PageNumber,
            int PageSize,
            string? SortField,
            int? SortOrder
        ) : IQuery<ResponseResult<PaginatedList<RoleSearchDTO>>>;
}