using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Entity.Queries.Pagination
{
    public record SearchEntityQuery(
            string? EntityName,
            string? EntityCode, 
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder
           )
        : IQuery<ResponseResult<PaginatedList<EntitySearchDto>>>
    {
    }
}