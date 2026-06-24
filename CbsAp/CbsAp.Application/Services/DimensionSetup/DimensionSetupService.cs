using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.DimensionSetup;
using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.DimensionSetup;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Enums;
using Mapster;
using System.Collections;
using System.Security.Principal;
using DimensionSetupDomain = CbsAp.Domain.Entities.DimensionSetup;

namespace CbsAp.Application.Services.DimensionSetup
{
    // REFACTOR : this service should be extracted on specified DimensionSetup cqrs handler.
    public class DimensionSetupService : IDimensionSetupService
    {
        private readonly IUnitofWork _unitofWork;

        private readonly IDimensionSetupRepository _dimensionSetupRepository;

        public DimensionSetupService(IUnitofWork unitofWork, IDimensionSetupRepository dimensionSetupRepository)
        {
            _unitofWork = unitofWork;
            _dimensionSetupRepository = dimensionSetupRepository;
        }

        public async Task<bool> CreateDimensionSetup(DimensionSetupDomain.DimensionSetup dimensionSetup, CancellationToken cancellationToken)
        {
            await _unitofWork.GetRepository<DimensionSetupDomain.DimensionSetup>()
                 .AddAsync(dimensionSetup);
            return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }

        public async Task<bool> UpdateDimensionSetup(DimensionSetupDomain.DimensionSetup dimensionSetup, CancellationToken cancellationToken)
        {
            return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }

        public async Task<DimensionSetupDto?> GetDimensionSetupByIdAsync(long dimensionSetupProfileID)
        {
            var DimensionSetup = await _dimensionSetupRepository.GetDimensionSetupByID(dimensionSetupProfileID)!;
            

            var result = DimensionSetup.Adapt<DimensionSetupDto>();


            return result;
        }

        public async Task<PaginatedList<DimensionSetupListDto>> SearchDimensionSetupPagination(string? dimensionSetupName, string? dimensionSetupValue, int pageNumber, int pageSize, string? sortField, int? sortOrder, CancellationToken token)
        {
            var DimensionSetupPagination = await _dimensionSetupRepository.SearchDimensionSetupWithPagination(
                dimensionSetupName,
                dimensionSetupValue,
                pageNumber,
                pageSize,
                sortField,
                sortOrder,
                token);
            return DimensionSetupPagination!;
        }
    }
}