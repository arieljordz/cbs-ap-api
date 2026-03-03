using CbsAp.Domain.Entities.Dimensions;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IDimensionRepository
    {
        Task<IEnumerable<Dimension>> GetDimensions(long entityProfileId, string? dimension, string? name, bool? isActive);

        IQueryable<Dimension> GetDimensionsAsQueryable();
    }
}
