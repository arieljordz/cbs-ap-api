using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.ArchiveSearch
{
    public record ArchiveSearchQuery : IQuery<ResponseResult<PaginatedList<ArchiveInvoiceSearchDto>>>
    {
        public string? SupplierName { get; init; }
        public string? InvoiceNo { get; init; }
        public string? PONo { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public string? SortField { get; init; }
        public int? SortOrder { get; init; }
        public int RoleId { get; set; }
    }
}