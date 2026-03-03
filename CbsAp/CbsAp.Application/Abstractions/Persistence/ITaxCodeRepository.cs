
using CbsAp.Domain.Entities.TaxCodes;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface ITaxCodeRepository
    {
        Task<IEnumerable<TaxCode>> GetTaxCodes(long entityId,string taxCodeName,string taxCode);
        IQueryable<TaxCode> GetTaxCodesAsQueryableAsync();
    }
}