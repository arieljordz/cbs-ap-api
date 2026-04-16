using CbsAp.Application.DTOs.InvoiceInquiry;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Entity;

namespace CbsAp.Application.Abstractions.Services.InvoiceInquiry
{
    public interface IInvoiceInquiryService
    {
        Task<PaginatedList<InvoiceInquiryDto>> SearchInvoiceInquiryPagination(
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
            CancellationToken token);
    }
}