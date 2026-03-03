using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.LoadInvoiceComment
{
    public class LoadInvoiceCommentQueryHandler : IQueryHandler<LoadInvoiceCommentQuery, ResponseResult<PaginatedList<LoadInvoiceCommentsDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public LoadInvoiceCommentQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<PaginatedList<LoadInvoiceCommentsDto>>> Handle(LoadInvoiceCommentQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository.LoadInvoiceComments(
                request.InvoiceID,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                request.sortOrder,
                cancellationToken
                );

            return result == null ?
                 ResponseResult<PaginatedList<LoadInvoiceCommentsDto>>
                .NotFound(MessageConstants.Message("InvoiceComments", MessageOperationType.NotFound)) :
                ResponseResult<PaginatedList<LoadInvoiceCommentsDto>>
                .SuccessRetrieveRecords(result);
        }
    }
}
