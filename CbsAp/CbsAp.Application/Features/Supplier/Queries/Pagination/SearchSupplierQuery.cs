using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Supplier.Queries.Pagination
{
    public record SearchSupplierQuery(
           string? EntityName,
            string? SupplierID,
            string? SupplierName,
            bool? IsActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder) : IQuery<ResponseResult<PaginatedList<SupplierSearchDto>>>
    {
    }
}