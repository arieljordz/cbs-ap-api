using CbsAp.Application.DTOs.InvoiceInquiry;
using CbsAp.Application.Shared;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IInvoiceInquiryRepository
    {
        Task<PaginatedList<InvoiceInquiryDto>> SearchInvoiceInquiryWithPagination(
        InvoiceInquirySearchDto dto,
        int pageNumber,
        int pageSize,
        string? sortField,
        int? sortOrder,
        CancellationToken token);

        Task<List<ExportInvoiceInquiryDto>> ExportInvoiceInquiryToExcel(
            string? SupplierName,
            string? InvoiceNumber,
            string? PONumber,
            int? Status, 
            DateTimeOffset? ScanDateFrom,
            DateTimeOffset? ScanDateTo,
            DateTimeOffset? InvoiceDateFrom,
            DateTimeOffset? InvoiceDateTo,
            CancellationToken token
     );
    }
}