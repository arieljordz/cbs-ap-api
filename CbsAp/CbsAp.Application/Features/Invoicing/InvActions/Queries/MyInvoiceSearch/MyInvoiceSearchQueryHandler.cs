using Bogus.DataSets;
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.MyInvoiceSearch
{
    public class MyInvoiceSearchQueryHandler : IQueryHandler<MyInvoiceSearchQuery, ResponseResult<PaginatedList<InvMyInvoiceSearchDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public MyInvoiceSearchQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<PaginatedList<InvMyInvoiceSearchDto>>> Handle(MyInvoiceSearchQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository.GetMyInvoiceSearch(
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
                 ResponseResult<PaginatedList<InvMyInvoiceSearchDto>>
                .NotFound(MessageConstants.Message("Invoice", MessageOperationType.NotFound)) :
                ResponseResult<PaginatedList<InvMyInvoiceSearchDto>>
                .SuccessRetrieveRecords(result);
        }
    }
}