using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class TaxCodeRepository : ITaxCodeRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public TaxCodeRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<TaxCode>> GetTaxCodes(long entityId, string taxCodeName, string taxCode)
        {
            var taxcodes = await _dbcontext.TaxCodes
                .AsNoTracking()
                .Include(t => t.EntityProfile)
                .WhereIf(entityId > 0, t => t.EntityID == entityId)
                .WhereIf(!string.IsNullOrEmpty(taxCodeName), t => t.TaxCodeName!.Contains(taxCodeName))
                .WhereIf(!string.IsNullOrEmpty(taxCode), t => t.Code!.Contains(taxCode))
                .ToListAsync();
            return taxcodes;
        }

        public IQueryable<TaxCode> GetTaxCodesAsQueryableAsync()
        {
            return _dbcontext.TaxCodes.AsQueryable();
        }
    }
}