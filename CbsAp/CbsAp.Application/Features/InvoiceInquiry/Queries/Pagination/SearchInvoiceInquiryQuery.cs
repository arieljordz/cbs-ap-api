using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.InvoiceInquiry;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.InvoiceInquiry.Queries.Pagination
{
    public record SearchInvoiceInquiryQuery(
        InvoiceInquirySearchDto invoiceInquirySearchDto,
        int PageNumber,
        int PageSize,
        string? SortField,
        int? SortOrder
    ) : IQuery<ResponseResult<PaginatedList<InvoiceInquiryDto>>>
    {
    }
}