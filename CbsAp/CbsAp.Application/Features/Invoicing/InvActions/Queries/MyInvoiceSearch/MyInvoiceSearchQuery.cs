using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.MyInvoiceSearch
{
    public record MyInvoiceSearchQuery(
        string? SupplierName,
        string? InvoiceNo,
        string? PONo,
        int pageNumber,
        int pageSize,
        string? sortField,
        int? sortOrder) : IQuery<ResponseResult<PaginatedList<InvMyInvoiceSearchDto>>>
    {
    }
}