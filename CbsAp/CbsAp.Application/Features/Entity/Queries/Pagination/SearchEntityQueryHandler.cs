using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Entity.Queries.Pagination
{
    public class SearchEntityQueryHandler : IQueryHandler<SearchEntityQuery, ResponseResult<PaginatedList<EntitySearchDto>>>
    {
        private readonly IEntityService _entityService;

        public SearchEntityQueryHandler(IEntityService entityService)
        {
            _entityService = entityService;
        }

        public async Task<ResponseResult<PaginatedList<EntitySearchDto>>> Handle(SearchEntityQuery request, CancellationToken cancellationToken)
        {
            var result = await _entityService.SearchEntityPagination(
                request.EntityName,
                request.EntityCode,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                request.sortOrder,
                cancellationToken
                );

            return result == null ?
                   ResponseResult<PaginatedList<EntitySearchDto>>
                  .NotFound(MessageConstants.Message("Entity", MessageOperationType.NotFound)) :
                  ResponseResult<PaginatedList<EntitySearchDto>>
                  .SuccessRetrieveRecords(result);
        }
    }
}