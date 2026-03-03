using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Accounts;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.Accounts.Queries.LookUps
{
    public record GetAccountsLookUpQuery() : IQuery<ResponseResult<IEnumerable<AccountLookupDto>>>
    {
    }
}