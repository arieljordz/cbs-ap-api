using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.DimensionSetup;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Services.DimensionSetup;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Entity.Queries.Pagination
{
    public class SearchDimensionSetupQueryHandler : IQueryHandler<SearchDimensionSetupQuery, ResponseResult<PaginatedList<DimensionSetupListDto>>>
    {
        private readonly IDimensionSetupService _dimensionSetupService;

        public SearchDimensionSetupQueryHandler(IDimensionSetupService dimensionSetupService)
        {
            _dimensionSetupService = dimensionSetupService;
        }

        public async Task<ResponseResult<PaginatedList<DimensionSetupListDto>>> Handle(SearchDimensionSetupQuery request, CancellationToken cancellationToken)
        {
            var result = await _dimensionSetupService.SearchDimensionSetupPagination(
                request.dimensionSetupName,
                request.dimensionSetupValue,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                request.sortOrder,
                cancellationToken
                );

            return result == null ?
                   ResponseResult<PaginatedList<DimensionSetupListDto>>
                  .NotFound(MessageConstants.Message("DimensionSetup", MessageOperationType.NotFound)) :
                  ResponseResult<PaginatedList<DimensionSetupListDto>>
                  .SuccessRetrieveRecords(result);
        }
    }
}