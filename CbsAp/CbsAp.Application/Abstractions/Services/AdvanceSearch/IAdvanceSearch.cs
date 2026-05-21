using CbsAp.Application.DTOs.AdvanceSearch;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.AdvanceSearch;

namespace CbsAp.Application.Abstractions.Services.AdvanceSearch
{
    public interface IAdvanceSearchService
    {

        Task<bool> CreateAdvanceSearch(CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch advanceSearch, CancellationToken cancellationToken);

        Task<bool> UpdateAdvanceSearch(CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch advanceSearch, CancellationToken cancellationToken);

        Task<AdvanceSearchDto?> GetAdvanceSearchByIdAsync(long advanceSearchID);

        Task<AdvanceSearchRequestForm> GetAdvanceSearchByUser(string formName, string userId);
    }
}