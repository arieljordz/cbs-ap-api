using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.AdvanceSearch;
using CbsAp.Application.DTOs.AdvanceSearch;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.AdvanceSearch;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Principal;

namespace CbsAp.Application.Services.AdvanceSearch
{
    // REFACTOR : this service should be extracted on specified AdvanceSearch cqrs handler.
    public class AdvanceSearchService : IAdvanceSearchService
    {
        private readonly IUnitofWork _unitofWork;

        private readonly IAdvanceSearchRepository _AdvanceSearchRepository;

        public AdvanceSearchService(IUnitofWork unitofWork, IAdvanceSearchRepository AdvanceSearchRepository)
        {
            _unitofWork = unitofWork;
            _AdvanceSearchRepository = AdvanceSearchRepository;
        }

        public async Task<bool> CreateAdvanceSearch(CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch AdvanceSearch, CancellationToken cancellationToken)
        {
            await _unitofWork.GetRepository<CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch>()
                 .AddAsync(AdvanceSearch);
            return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }


        public async Task<bool> UpdateAdvanceSearch(CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch advanceSearch, CancellationToken cancellationToken)
        {
            await _unitofWork.GetRepository<CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch>()
                .UpdateAsync(advanceSearch.AdvanceSearchId, advanceSearch);
            return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }

        public async Task<AdvanceSearchDto?> GetAdvanceSearchByIdAsync(long advanceSearchID)
        {
            var AdvanceSearch = await _AdvanceSearchRepository.GetAdvanceSearchByID(advanceSearchID)!;

            return AdvanceSearch;
        }

        public async Task<AdvanceSearchRequestForm> GetAdvanceSearchByUser(string formName, string userId)
        {

            var advanceSearchRequestForm = await _AdvanceSearchRepository.GetAdvanceSearchByUser(formName, userId);


            return advanceSearchRequestForm;
        }
    }
}