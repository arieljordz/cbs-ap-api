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
               request.pageNumber,
               request.pageSize,
               request.sortField,
               request.sortOrder,
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