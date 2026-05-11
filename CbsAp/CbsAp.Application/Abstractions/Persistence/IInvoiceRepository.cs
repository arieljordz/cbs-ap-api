using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IInvoiceRepository
    {
        Task<PaginatedList<InvSearchSupplierDto>> SearchSupplierWithPagination(
            string? SupplierID,
            string? SupplierName,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token);

        Task<InvoiceDto> GetInvoiceInfo(long invoiceID, CancellationToken token);
        Task<List<InvAllocEntryDto>> GetInvoiceAllocationInfo(long? invoiceID, CancellationToken token);

        Task<PaginatedList<InvAllocLineDto>> GetInvAllocLinePerInvoice(
            long? invoiceID,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token);

        Task<PaginatedList<InvMyInvoiceSearchDto>> GetMyInvoiceSearch(
           string? SupplierName,
           string? InvoiceNo,
           string? PONo,
           int pageNumber,
           int pageSize,
           string? sortField,
           int? sortOrder,
           int roleId,
           CancellationToken token);

        Task<PaginatedList<RejectedInvoiceSearchDto>> GetRejectedInvoiceSearch(
           string? SupplierName,
           string? InvoiceNo,
           string? PONo,
           int pageNumber,
           int pageSize,
           string? sortField,
           int? sortOrder,
           CancellationToken token);

        Task<PaginatedList<ExceptionInvoiceSearchDto>> GetExceptionInvoiceSearch(
           string? SupplierName,
           string? InvoiceNo,
           string? PONo,
           int pageNumber,
           int pageSize,
           string? sortField,
           int? sortOrder,
           CancellationToken token);

        Task<PaginatedList<ArchiveInvoiceSearchDto>> GetArchiveInvoiceSearch(
         string? SupplierName,
         string? InvoiceNo,
         string? PONo,
         int pageNumber,
         int pageSize,
         string? sortField,
         int? sortOrder,
         CancellationToken token);

        Task<List<ExportMyInvoiceDto>> ExportMyInvoiceToExcel(
           string? SupplierName,
           string? InvoiceNo,
           string? PONo,
           long roleId,
           CancellationToken token);

        Task<List<ExportRejectedInvoiceDto>> ExportRejectedInvoice(
           string? SupplierName,
           string? InvoiceNo,
           string? PONo,
           CancellationToken token);

        Task<List<ExportExceptionInvoiceDto>> ExportExceptionInvoice(
          string? SupplierName,
          string? InvoiceNo,
          string? PONo,
          CancellationToken token);

        Task<List<ExportArchiveInvoiceDto>> ExportArchiveInvoice(
          string? SupplierName,
          string? InvoiceNo,
          string? PONo,
          CancellationToken token);


        Task<PaginatedList<LoadInvoiceCommentsDto>> LoadInvoiceComments(
        long? InvoiceID,
        int pageNumber,
        int pageSize,
        string? sortField,
        int? sortOrder,
        CancellationToken token);

        Task<InvoiceNavigationResultDto> GetAdjacentInvoiceId(
           long invoiceID,
           bool isNext,
           InvoiceStatusType? statusType,
           InvoiceQueueType? queueType,
           CancellationToken token);

        Task<GetInvoiceStatusDto?> GetInvoiceStatusAsync(long invoiceId, CancellationToken token);

        Task<bool> ChangeHoldStateAsync(InvStatusChangeDto dto, string updatedBy, CancellationToken token);
    }
}