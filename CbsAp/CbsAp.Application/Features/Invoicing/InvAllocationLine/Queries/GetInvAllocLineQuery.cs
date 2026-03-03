using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvAllocationLine.Queries
{
    public record GetInvAllocLineQuery(
            long? invoiceID,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder) : IQuery<ResponseResult<PaginatedList<InvAllocLineDto>>>
    {
    }
}