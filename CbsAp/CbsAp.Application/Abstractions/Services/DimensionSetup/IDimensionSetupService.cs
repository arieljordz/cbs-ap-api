using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.DimensionSetup;
using DimensionSetupDomain = CbsAp.Domain.Entities.DimensionSetup;

namespace CbsAp.Application.Abstractions.Services.DimensionSetup
{
    public interface IDimensionSetupService
    {

        Task<bool> CreateDimensionSetup(DimensionSetupDomain.DimensionSetup dimensionSetup, CancellationToken cancellationToken);

        Task<bool> UpdateDimensionSetup(DimensionSetupDomain.DimensionSetup dimensionSetup, CancellationToken cancellationToken);

        Task<DimensionSetupDto?> GetDimensionSetupByIdAsync(long dimensionSetupId);

        Task<PaginatedList<DimensionSetupListDto>> SearchDimensionSetupPagination(
            string? dimensionSetupName,
            string? dimensionSetupCode,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token);

    }
}