using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Accounts;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public AccountRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IQueryable<Account> GetAccountsAsQueryable()
        {
            return _dbcontext.Accounts.AsQueryable();
        }

        public async Task<PaginatedList<SearchAccountLookupDto>> SearchAccountLookUpPagination(
            long? AccountID,
            string? AccountName,
            string? EntityName,
            bool? IsActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder, CancellationToken token)
        {
            ExpressionStarter<Account> predicate =
              PredicateBuilder.New<Account>(true);

            predicate = predicate
                .AndIf(AccountID.HasValue, a => a.AccountID == AccountID!.Value)
                .AndIf(!string.IsNullOrEmpty(AccountName), a => a.AccountName!.Contains(AccountName!))
                .AndIf(!string.IsNullOrEmpty(EntityName), a => a.EntityProfile!.EntityName!.Contains(EntityName!))
               .AndIf(IsActive.HasValue, a => a.IsActive == IsActive);

            var query = _dbcontext.Accounts
                .Include(e => e.EntityProfile)
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate)
                .Select(a => new
                {
                    a.AccountID,
                    a.AccountName,
                    a.EntityProfile!.EntityName,
                    a.IsActive,
                    a.CreatedDate,
                    a.LastUpdatedDate,
                    a.Dimension1,
                    a.Dimension2,
                    a.Dimension3,
                    a.Dimension4,
                    a.Dimension5,
                    a.Dimension6,
                    a.Dimension7,
                    a.Dimension8,
                    a.IsTaxCodeMandatory
                });

            if (string.IsNullOrEmpty(sortField))
            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = query.Select(
                a => new SearchAccountLookupDto
                {
                    AccountID = a.AccountID,
                    AccountName = a.AccountName!,
                    EntityName = a.EntityName,
                    Active = a.IsActive ? "Yes" : "No",
                    Dimension1 = a.Dimension1,
                    Dimension2 = a.Dimension2,
                    Dimension3 = a.Dimension3,
                    Dimension4 = a.Dimension4,
                    Dimension5 = a.Dimension5,
                    Dimension6 = a.Dimension6,
                    Dimension7 = a.Dimension7,
                    Dimension8 = a.Dimension8,
                    IsTaxCodeMandatory = a.IsTaxCodeMandatory
                });

            var accountsPagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
             .ToPaginatedListAsync(pageNumber, pageSize, token);
            return accountsPagination;
        }
    }
}