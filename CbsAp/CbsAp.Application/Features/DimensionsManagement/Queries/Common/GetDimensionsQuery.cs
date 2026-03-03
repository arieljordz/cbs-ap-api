using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.DimensionsManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.DimensionsManagement.Queries.Common
{
    public record GetDimensionsQuery(
        string? Entity,
        string? Dimension,
        string? DimensionName,
        bool? Active,
        int PageNumber,
        int PageSize,
        string? SortField,
        int? SortOrder) : IQuery<ResponseResult<PaginatedList<DimensionDTO>>>;
}
