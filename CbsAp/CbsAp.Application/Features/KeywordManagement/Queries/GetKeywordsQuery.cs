using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Keyword;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.KeywordManagement.Queries
{
    public record GetKeywordsQuery(
        string? KeywordName,
        string? EntityName,
        string? InvoiceRoutingFlowName,
        bool? IsActive,
        int PageNumber,
        int PageSize,
        string? SortField,
        int? SortOrder) : IQuery<ResponseResult<PaginatedList<KeywordDTO>>>;
}