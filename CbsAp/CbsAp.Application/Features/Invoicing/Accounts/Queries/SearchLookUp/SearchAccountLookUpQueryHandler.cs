using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Accounts;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.Accounts.Queries.SearchLookUp
{
    public class SearchAccountLookUpQueryHandler : IQueryHandler<SearchAccountLookUpQuery, ResponseResult<PaginatedList<SearchAccountLookupDto>>>
    {
        private readonly IAccountRepository _accountRepository;

        public SearchAccountLookUpQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ResponseResult<PaginatedList<SearchAccountLookupDto>>> Handle(SearchAccountLookUpQuery request, CancellationToken cancellationToken)
        {
            var results = await _accountRepository.SearchAccountLookUpPagination(
                request.AccountID,
                request.AccountName,
                request.EntityName,
                request.Active,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                request.sortOrder,
                cancellationToken
                );
            return results == null ?

          ResponseResult<PaginatedList<SearchAccountLookupDto>>
             .NotFound(MessageConstants.Message("Account Search", MessageOperationType.NotFound)) :
             ResponseResult<PaginatedList<SearchAccountLookupDto>>
             .SuccessRetrieveRecords(results);
        }
    }
}