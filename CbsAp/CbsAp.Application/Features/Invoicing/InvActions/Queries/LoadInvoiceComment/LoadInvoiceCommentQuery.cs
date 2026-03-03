using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.LoadInvoiceComment
{
    public record LoadInvoiceCommentQuery(
        long? InvoiceID,
        int pageNumber,
        int pageSize,
        string? sortField,
        int? sortOrder) :
        IQuery<ResponseResult<PaginatedList<LoadInvoiceCommentsDto>>>
    {
    }
}
