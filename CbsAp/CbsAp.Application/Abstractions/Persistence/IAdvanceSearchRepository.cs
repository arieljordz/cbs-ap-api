using CbsAp.Application.DTOs.AdvanceSearch;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IAdvanceSearchRepository
    {
        Task<AdvanceSearchDto?> GetAdvanceSearchByID(long entityProfileID);

        Task<AdvanceSearchRequestForm> GetAdvanceSearchByUser(string formName, string userId);
    }
}