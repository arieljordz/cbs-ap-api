using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.InvoiceInquiry;
using CbsAp.Application.DTOs.InvoiceInquiry;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Services.InvoiceInquiry
{
    // REFACTOR : this service should be extracted on specified invoiceInquiry cqrs handler.
    public class InvoiceInquiryService : IInvoiceInquiryService
    {
        private readonly IUnitofWork _unitofWork;

        private readonly IInvoiceInquiryRepository _invoiceInquiryRepository;

        public InvoiceInquiryService(IUnitofWork unitofWork, IInvoiceInquiryRepository invoiceInquiryRepository)
        {
            _unitofWork = unitofWork;
            _invoiceInquiryRepository = invoiceInquiryRepository;
        }

        public async Task<PaginatedList<InvoiceInquiryDto>> SearchInvoiceInquiryPagination(
            InvoiceInquirySearchDto dto,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            var invoiceInquiryPagination =
                await _invoiceInquiryRepository.SearchInvoiceInquiryWithPagination(
                    dto,
                    pageNumber,
                    pageSize,
                    sortField,
                    sortOrder,
                    token);

            return invoiceInquiryPagination!;
        }

        public async Task<List<ExportInvoiceInquiryDto>> ExportInvoiceInquiryToExcel(
            int? SupplierInfoID,
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

            CancellationToken token)
        {
            var result = await _invoiceInquiryRepository.ExportInvoiceInquiryToExcel(
                SupplierInfoID,
                InvoiceNumber,
                PONumber,
                RoleID,
                Status,
                InvoiceDateFrom,
                InvoiceDateTo,
                InvoiceDueDateFrom,
                InvoiceDueDateTo,
                PaymentDateFrom,
                PaymentDateTo,
                ScanDateFrom,
                ScanDateTo,
                token);

            return result;
        }
    }
}