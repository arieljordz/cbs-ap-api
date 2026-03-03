using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PermissionManagement.Queries.SearchActions
{
    public record SearchPermissionParamQuery(long? PermissionID,
            string? PermissionName,
            bool? IsActive,
            string? SortField,
            int? SortOrder,
            int PageNumber,
            int PageSize) :
        IQuery<ResponseResult<PaginatedList<PermissionSearchDTO>>>;
}