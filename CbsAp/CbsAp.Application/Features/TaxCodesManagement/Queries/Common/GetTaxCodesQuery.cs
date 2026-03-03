using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Dashboard.Queries.Common
{
    public record GetTaxCodesQuery(
        string? EntityName,
        string? TaxCodeName,
        string? TaxCode,
        int PageNumber,
        int PageSize,
        string? SortField,
        int? SortOrder) : IQuery<ResponseResult<PaginatedList<TaxCodeDTO>>>;
}