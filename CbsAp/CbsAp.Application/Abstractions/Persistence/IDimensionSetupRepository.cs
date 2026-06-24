using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IDimensionSetupRepository
    {
        Task<DimensionSetupDto?> GetDimensionSetupByID(long dimensionSetupId);
        Task<PaginatedList<DimensionSetupListDto>> SearchDimensionSetupWithPagination(
            string? dimensionSetupName,
            string? dimensionSetupValue,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token);
    }
}