using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.RejectedSearch
{
    public class RejectedSearchQueryHandler : IQueryHandler<RejectedSearchQuery, ResponseResult<PaginatedList<RejectedInvoiceSearchDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public RejectedSearchQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<PaginatedList<RejectedInvoiceSearchDto>>> Handle(RejectedSearchQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository.GetRejectedInvoiceSearch(
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
                 ResponseResult<PaginatedList<RejectedInvoiceSearchDto>>
                .NotFound(MessageConstants.Message("RejectedInvoice", MessageOperationType.NotFound)) :
                ResponseResult<PaginatedList<RejectedInvoiceSearchDto>>
                .SuccessRetrieveRecords(result);
        }
    }
}