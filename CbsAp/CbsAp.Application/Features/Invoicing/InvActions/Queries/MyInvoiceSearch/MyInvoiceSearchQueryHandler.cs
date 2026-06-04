using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
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