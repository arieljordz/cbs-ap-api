using CbsAp.Application.DTOs.Invoicing.Accounts;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IAccountRepository
    {
        Task<PaginatedList<SearchAccountLookupDto>> SearchAccountLookUpPagination(
            long? AccountID,
            string? AccountName,
            string? EntityName,
            bool? IsActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token);

        IQueryable<Account> GetAccountsAsQueryable();
    }
}