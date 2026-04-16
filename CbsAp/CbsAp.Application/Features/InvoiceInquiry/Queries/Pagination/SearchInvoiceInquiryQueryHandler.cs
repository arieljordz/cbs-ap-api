using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.InvoiceInquiry;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.InvoiceInquiry;
using CbsAp.Application.Services.InvoiceInquiry;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.InvoiceInquiry.Queries.Pagination
{
    public class SearchInvoiceInquiryQueryHandler : IQueryHandler<SearchInvoiceInquiryQuery, ResponseResult<PaginatedList<InvoiceInquiryDto>>>
    {
        private readonly IInvoiceInquiryService _invoiceInquiryService;

        public SearchInvoiceInquiryQueryHandler(IInvoiceInquiryService invoiceInquiryService)
        {
            _invoiceInquiryService = invoiceInquiryService;
        }

        public async Task<ResponseResult<PaginatedList<InvoiceInquiryDto>>> Handle(
            SearchInvoiceInquiryQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _invoiceInquiryService.SearchInvoiceInquiryPagination(
                request.invoiceInquirySearchDto,
                request.PageNumber,
                request.PageSize,
                request.SortField,
                request.SortOrder,
                cancellationToken);

            return result == null
                ? ResponseResult<PaginatedList<InvoiceInquiryDto>>
                    .NotFound(MessageConstants.Message("InvoiceInquiry", MessageOperationType.NotFound))
                : ResponseResult<PaginatedList<InvoiceInquiryDto>>
                    .SuccessRetrieveRecords(result);
        }
    }
}