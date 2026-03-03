using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Dimensions;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class DimensionRepository : IDimensionRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public DimensionRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Dimension>> GetDimensions(long entityProfileId, string? dimension, string? name, bool? isActive)
        {
            var dimensions = await _dbcontext.Dimensions
                .AsNoTracking()
                .Include(d => d.EntityProfile)
                .WhereIf(entityProfileId > 0, d => d.EntityProfileID == entityProfileId)
                .WhereIf(!string.IsNullOrEmpty(dimension), d => d.DimensionCode.Contains(dimension!))
                .WhereIf(!string.IsNullOrEmpty(name), d => d.Name.Contains(name!))
                .WhereIf(isActive.HasValue, d => d.IsActive == isActive!.Value)
                .ToListAsync();

            return dimensions;
        }

        public IQueryable<Dimension> GetDimensionsAsQueryable()
        {
            return _dbcontext.Dimensions.AsQueryable();
        }
    }
}
