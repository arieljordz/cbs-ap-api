using CbsAp.Application.DTOs.InvoiceInquiry;
using CbsAp.Application.Shared;
using CbsAp.Domain.Enums;

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
            int? SupplierInfoID ,
            string? InvoiceNumber,
            string? PONumber,
            int? RoleID,
            List<InvoiceStatusType>? Status,
            DateTimeOffset? InvoiceDateFrom,
            DateTimeOffset? InvoiceDateTo,
            DateTimeOffset? InvoiceDueDateFrom,
            DateTimeOffset? InvoiceDueDateTo,
            DateTimeOffset? PaymentDateFrom,
            DateTimeOffset? PaymentDateTo,
            DateTimeOffset? ScanDateFrom,
            DateTimeOffset? ScanDateTo,

            CancellationToken token
     );
    }
}