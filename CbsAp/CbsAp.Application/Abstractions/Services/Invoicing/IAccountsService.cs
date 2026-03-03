using CbsAp.Application.DTOs.Invoicing.Accounts;

namespace CbsAp.Application.Abstractions.Services.Invoicing
{
    public interface IAccountsService
    {
        Task<IEnumerable<AccountLookupDto>> GetAccountLookupsAsync();
    }
}
