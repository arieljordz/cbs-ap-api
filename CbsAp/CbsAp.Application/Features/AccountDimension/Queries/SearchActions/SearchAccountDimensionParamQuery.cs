using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.AccountDimension;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.AccountDimension.Queries.SearchActions
{
    public record SearchAccountDimensionParamQuery(long? RoleId,
            string? SortField,
            int? SortOrder,
            int PageNumber,
            int PageSize) :
        IQuery<ResponseResult<PaginatedList<AccountDimensionSearchDTO>>>;
}