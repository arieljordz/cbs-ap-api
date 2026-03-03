using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Accounts;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.Accounts.Queries.SearchLookUp
{
    public record SearchAccountLookUpQuery(
            long? AccountID,
            string? AccountName,
            string? EntityName,
            bool? Active,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder) : IQuery<ResponseResult<PaginatedList<SearchAccountLookupDto>>>
    {
    }
}