using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.DimensionsManagement;
using CbsAp.Application.Features.DimensionsManagement.Queries.Common;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Dimensions;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.DimensionsManagement.Queries
{
    public class GetDimensionsQueryHandler : IQueryHandler<GetDimensionsQuery, ResponseResult<PaginatedList<DimensionDTO>>>
    {
        private readonly IDimensionRepository _dimensionRepository;

        public GetDimensionsQueryHandler(IDimensionRepository dimensionRepository)
        {
            _dimensionRepository = dimensionRepository;
        }

        public async Task<ResponseResult<PaginatedList<DimensionDTO>>> Handle(GetDimensionsQuery request, CancellationToken cancellationToken)
        {
            ExpressionStarter<Dimension> predicate = PredicateBuilder.New<Dimension>(true);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(request.Entity), d => d.EntityProfile != null && d.EntityProfile.EntityName.Contains(request.Entity!))
                .AndIf(!string.IsNullOrEmpty(request.Dimension), d => d.DimensionCode.Contains(request.Dimension!))
                .AndIf(!string.IsNullOrEmpty(request.DimensionName), d => d.Name.Contains(request.DimensionName!))
                .AndIf(request.Active.HasValue, d => d.IsActive == request.Active!.Value);

            var dimensionQuery = _dimensionRepository
                .GetDimensionsAsQueryable()
                .Include(d => d.EntityProfile)
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(request.SortField))
            {
                dimensionQuery = dimensionQuery.OrderByDescending(d => d.LastUpdatedDate ?? d.CreatedDate);
            }

            var projectedQuery = dimensionQuery.Select(d => new DimensionDTO
            {
                DimensionID = d.DimensionID,
                Entity = d.EntityProfile != null ? d.EntityProfile.EntityName : string.Empty,
                Dimension = d.DimensionCode,
                DimensionName = d.Name,
                Active = d.IsActive,
            });

            var paginatedDimensions = await projectedQuery.OrderByDynamic(request.SortField, request.SortOrder)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            return paginatedDimensions == null
                ? ResponseResult<PaginatedList<DimensionDTO>>.NotFound(MessageConstants.Message("Dimension", MessageOperationType.NotFound))
                : ResponseResult<PaginatedList<DimensionDTO>>.SuccessRetrieveRecords(paginatedDimensions);
        }
    }
}
