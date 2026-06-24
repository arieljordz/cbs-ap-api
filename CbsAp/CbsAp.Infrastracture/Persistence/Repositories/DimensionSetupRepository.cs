using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class DimensionSetupRepository : IDimensionSetupRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public DimensionSetupRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<DimensionSetupDto?> GetDimensionSetupByID(long dimensionSetupId)
        {
            var dimensionSetup = await _dbcontext.DimensionSetups
                .SingleOrDefaultAsync(e => e.DimensionSetupId == dimensionSetupId);

            var dto = dimensionSetup.Adapt<DimensionSetupDto>();
            dto.Required = dimensionSetup.Required;

            return dto!;
        }


        public async Task<PaginatedList<DimensionSetupListDto>> SearchDimensionSetupWithPagination(
            string? dimensionSetupName,
            string? dimentionSetupValue,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            ExpressionStarter<CbsAp.Domain.Entities.DimensionSetup.DimensionSetup> predicate = PredicateBuilder.New<CbsAp.Domain.Entities.DimensionSetup.DimensionSetup>(true);


            var query = _dbcontext.DimensionSetups
                .AsNoTracking()
                .Where(predicate);

            var dtoSearchEntity = query.Select(e => new DimensionSetupListDto
            {
                DimensionSetupId = e.DimensionSetupId,
                DimensionSetupName = e.DimensionSetupName,
                DisplayOrder = e.DisplayOrder,
                DimensionName = e.DimensionName,
                DimensionValueId = e.DimensionValueId,
                Required = e.Required,
                Show = e.Show
            });

            var entityPagination = await dtoSearchEntity.OrderByDynamic(sortField, sortOrder)
                .ToPaginatedListAsync(pageNumber, pageSize, token);
            return entityPagination;
        }
    }
}