using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvAllocationLine.Queries
{
    public class GetInvAllocLineQueryHandler : IQueryHandler<GetInvAllocLineQuery, ResponseResult<PaginatedList<InvAllocLineDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetInvAllocLineQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<PaginatedList<InvAllocLineDto>>> Handle(GetInvAllocLineQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository.GetInvAllocLinePerInvoice(
                request.invoiceID,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                request.sortOrder,
                cancellationToken);
            return result == null ?
                 ResponseResult<PaginatedList<InvAllocLineDto>>
                .NotFound(MessageConstants.Message("Invoice Allocation line", MessageOperationType.NotFound)) :
                ResponseResult<PaginatedList<InvAllocLineDto>>
                .SuccessRetrieveRecords(result);
        }
    }
}