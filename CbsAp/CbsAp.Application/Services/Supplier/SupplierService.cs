using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Supplier;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Supplier;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Services.Supplier
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitofWork _unitofWork;

        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(IUnitofWork unitofWork, ISupplierRepository supplierRepository)
        {
            _unitofWork = unitofWork;
            _supplierRepository = supplierRepository;
        }

        public async Task<bool> IsSupplierExist(string supplierID, string supplierName, long? supplierInfoID = null)
        {
            //  existing record
            if (supplierInfoID != null)
            {
                string normalizedSupplierID = supplierID.ToLower().Trim();
                string normalizedSupplierName = supplierName.ToLower().Trim();

                var supplierInfo = await _unitofWork.GetRepository<SupplierInfo>()
                    .ApplyPredicateAsync(e => e.SupplierInfoID != supplierInfoID);

                var duplicates =
                  await supplierInfo.Select(e => new { e.SupplierID, e.SupplierName })
                    .ToListAsync();

                bool idExists = duplicates.Any(e => e.SupplierID!.ToLower().Trim() == normalizedSupplierID);
                bool nameExists = duplicates.Any(e => e.SupplierName!.ToLower().Trim() == normalizedSupplierName);

                return idExists || nameExists;
            }
            //new record
            return await _unitofWork.GetRepository<SupplierInfo>()
                  .AnyAsync(e => e.SupplierID!.ToLower().Trim() == supplierID.ToLower().Trim()
                             || e.SupplierName!.ToLower().Trim() == supplierName.ToLower().Trim());
        }

        public async Task<bool> CreateSupplier(SupplierInfo supplier, CancellationToken cancellationToken)
        {
            await _unitofWork.GetRepository<SupplierInfo>()
                .AddAsync(supplier);
            return await _unitofWork.SaveChanges(string.Empty,string.Empty,cancellationToken);
        }

        public async Task<bool> UpdateSupplier(SupplierInfo supplier, CancellationToken cancellationToken)
        {
            await _unitofWork.GetRepository<SupplierInfo>()
                .UpdateAsync(supplier.SupplierInfoID, supplier);
            return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }

        public async Task<SupplierInfo> GetSupplierInfoById(long supplierInfoID)
        {
            var result = await _unitofWork.GetRepository<SupplierInfo>().GetByIdAsync(supplierInfoID);

            return result;
        }

        public async Task<PaginatedList<SupplierSearchDto>> SearchSupplierWithPagination(
            string? EntityName,
            string? SupplierID,
            string? SupplierName,
            bool? IsActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            var supplierPagination = await _supplierRepository.SearchSupplierWithPagination(
                EntityName,
                SupplierID,
                SupplierName,
                IsActive,
                pageNumber,
                pageSize,
                sortField,
                sortOrder,
                token);

            return supplierPagination!;
        }

        public async Task<List<ExportSupplierDto>> ExportSupplierToExcel(string? EntityName, string? SupplierID, string? SupplierName, bool? IsActive, CancellationToken token)
        {
            var result = await _supplierRepository.ExportSupplierToExcel(
                EntityName,
                SupplierID,
                SupplierName,
                IsActive,
                token);
            return result;
        }
    }
}