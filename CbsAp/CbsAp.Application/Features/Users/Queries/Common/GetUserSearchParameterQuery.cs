using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Queries.Common
{
    public record GetUserSearchParameterQuery(
        string? FullName,
        string? UserId,
        bool? IsActive,
        string? sortField,
        int? sortOrder,
        int PageNumber,
        int PageSize) :
        IQuery<ResponseResult<PaginatedList<UserSearchPaginationDTO>>>
    {
    }
}