using Bogus.Bson;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.AdvanceSearch;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using System.Text.Json;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class AdvanceSearchRepository : IAdvanceSearchRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public AdvanceSearchRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<AdvanceSearchDto?> GetAdvanceSearchByID(long advanceSearchID)
        {
            var entity = await _dbcontext.AdvanceSearches
                .SingleOrDefaultAsync(e => e.AdvanceSearchId == advanceSearchID);

            var dto = entity.Adapt<AdvanceSearchDto>();

            return dto!;
        }

        public async Task<AdvanceSearchRequestForm> GetAdvanceSearchByUser(string formName, string userId)
        {
            var entity = await _dbcontext.AdvanceSearches.SingleOrDefaultAsync(e => e.FormName == formName && e.UserId == userId);
            var result = new AdvanceSearchRequestForm();

            if (entity != null && !string.IsNullOrEmpty(entity.JsonFilter))
            {
                result = JsonSerializer.Deserialize<AdvanceSearchRequestForm>(entity.JsonFilter!);
                result.AdvanceSearchId = entity.AdvanceSearchId;
            }

            return result;
        }
    }
}