using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.LookUps
{
    public class InvSupplierLookUpQueryHandler : IQueryHandler<InvSupplierLookUpQuery, ResponseResult<PaginatedList<InvSearchSupplierDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvSupplierLookUpQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<PaginatedList<InvSearchSupplierDto>>> Handle(InvSupplierLookUpQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository.SearchSupplierWithPagination(
                 request.SupplierID,
                request.SupplierName,
                request.pageNumber,
                request.pageSize,
                request.sortField,
                request.sortOrder,
                cancellationToken
                );

            return result == null ?
             ResponseResult<PaginatedList<InvSearchSupplierDto>>
                .NotFound(MessageConstants.Message("Supplier Search", MessageOperationType.NotFound)) :
                ResponseResult<PaginatedList<InvSearchSupplierDto>>
                .SuccessRetrieveRecords(result);
        }
    }
}
