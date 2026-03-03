using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Invoicing;
using CbsAp.Application.DTOs.Invoicing.Accounts;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.Accounts.Queries.LookUps
{
    public class GetAccountsLookUpQueryHandler : IQueryHandler<GetAccountsLookUpQuery, ResponseResult<IEnumerable<AccountLookupDto>>>
    {
        private readonly IAccountsService _accountsService;

        public GetAccountsLookUpQueryHandler(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        public async Task<ResponseResult<IEnumerable<AccountLookupDto>>> Handle(GetAccountsLookUpQuery request, CancellationToken cancellationToken)
        {
            var results = await _accountsService.GetAccountLookupsAsync();

            return !results.Any() ?
                ResponseResult<IEnumerable<AccountLookupDto>>.NotFound("No accounts lookup found ")
                : ResponseResult<IEnumerable<AccountLookupDto>>.SuccessRetrieveRecords(results);
        }
    }
}