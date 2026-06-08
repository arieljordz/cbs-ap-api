using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.ArchiveSearch
{
    public class ArchiveSearchQueryHandler : IQueryHandler<ArchiveSearchQuery, ResponseResult<PaginatedList<ArchiveInvoiceSearchDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public ArchiveSearchQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<PaginatedList<ArchiveInvoiceSearchDto>>> Handle(ArchiveSearchQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository.GetArchiveInvoiceSearch(
               request.SupplierName,
               request.InvoiceNo,
               request.PONo,
               request.PageNumber,
               request.PageSize,
               request.SortField,
               request.SortOrder,
               request.RoleId,
                // Supplier Info
                request.PaymentTerm,
                request.SupplierNo,
                request.SuppABN,
                request.SuppBankAccount,

                // Invoice Detail
                request.EntityProfileID,
                request.GrNo,
                request.StartInvoiceDate,
                request.EndInvoiceDate,
                request.StartDueDate,
                request.EndDueDate,
                request.DaystillDue,

                // Amounts
                request.NetAmount,
                request.TaxCodeID,
                request.TaxAmount,
                request.Currency,
                request.TotalAmount,

                // Routing Flow
                request.InvRoutingFlowName,
                request.NextRole,
                request.Keyword,

                // Transaction Info
                request.MapID,
                request.StartScanDate,
                request.EndScanDate,
                request.InvoiceID,
               cancellationToken
               );

            return result == null ?
                 ResponseResult<PaginatedList<ArchiveInvoiceSearchDto>>
                .NotFound(MessageConstants.Message("ArchiveInvoice", MessageOperationType.NotFound)) :
                ResponseResult<PaginatedList<ArchiveInvoiceSearchDto>>
                .SuccessRetrieveRecords(result);
        }
    }
}