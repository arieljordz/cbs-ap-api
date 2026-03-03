using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface ISupplierRepository
    {
        Task<PaginatedList<SupplierSearchDto>> SearchSupplierWithPagination(
            string? EntityName,
            string? SupplierID,
            string? SupplierName,
            bool? IsActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token);

        Task<List<ExportSupplierDto>> ExportSupplierToExcel(
           string? EntityName,
           string? SupplierID,
           string? SupplierName,
           bool? IsActive,
           CancellationToken token);
    }
}
