using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Supplier;

namespace CbsAp.Application.Abstractions.Services.Supplier
{
    public interface ISupplierService
    {
        Task<bool> IsSupplierExist(string supplierID, string supplierName, long? supplierInfoID = null);
        Task<bool> CreateSupplier(SupplierInfo supplier, CancellationToken cancellationToken);

        Task<bool> UpdateSupplier(SupplierInfo supplier, CancellationToken cancellationToken);
      
        Task<SupplierInfo> GetSupplierInfoById(long supplierInfoID);
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

        Task<List<ExportSupplierDto>> ExportSupplierToExcel(string? EntityName, string? SupplierID, string? SupplierName, bool? IsActive, CancellationToken token);
    }
}
