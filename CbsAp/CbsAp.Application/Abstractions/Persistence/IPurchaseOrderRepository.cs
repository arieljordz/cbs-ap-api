using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IPurchaseOrderRepository
    {
        Task<List<SearchPoLinesDto>> SearchPoLines(
        string? SupplierName,
        string? SupplierTaxID,
        string? PONo,
        DateTime? PODateFrom,
        DateTime? PODateTo,
        string? DeliveryNo,
        string? supplierNo,
        List<long>? ExcludesMatchPOLineIds,
        bool IsAvailableOrder,
        CancellationToken token);

        Task<List<SearchPoLinesDto>> GetPOMatchingByInvID(string PONo, long InvoiceID, CancellationToken cancellationToken);

        Task<PaginatedList<POSearchDto>> PoSearch(
        string? EntityName,
        string? PONo,
        string? Supplier,
        bool? IsActive,
        int pageNumber,
        int pageSize,
        string? sortField,
        int? sortOrder,
        CancellationToken token);

        Task<List<ExportPoSearchDto>> ExportPoSearch(
       string? EntityName,
       string? PONo,
       string? Supplier,
       bool? IsActive,

       CancellationToken token);
    }
}