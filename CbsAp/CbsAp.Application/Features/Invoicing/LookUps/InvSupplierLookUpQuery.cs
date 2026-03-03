using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.LookUps
{
    public record InvSupplierLookUpQuery(
            string? SupplierID,
            string? SupplierName,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder) : IQuery<ResponseResult<PaginatedList<InvSearchSupplierDto>>>
    {
    }
}