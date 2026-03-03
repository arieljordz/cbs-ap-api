using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.ExceptionsSearch
{
    public class ExceptionsSearchQueryHandler : IQueryHandler<ExceptionsSearchQuery, ResponseResult<PaginatedList<ExceptionInvoiceSearchDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public ExceptionsSearchQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<PaginatedList<ExceptionInvoiceSearchDto>>> Handle(ExceptionsSearchQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository.GetExceptionInvoiceSearch(
                request.SupplierName,
                request.InvoiceNo,
                request.PONo,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                request.sortOrder,
                cancellationToken
                );

            return result == null ?
                 ResponseResult<PaginatedList<ExceptionInvoiceSearchDto>>
                .NotFound(MessageConstants.Message("ExceptionInvoice", MessageOperationType.NotFound)) :
                ResponseResult<PaginatedList<ExceptionInvoiceSearchDto>>
                .SuccessRetrieveRecords(result);
        }
    }
}