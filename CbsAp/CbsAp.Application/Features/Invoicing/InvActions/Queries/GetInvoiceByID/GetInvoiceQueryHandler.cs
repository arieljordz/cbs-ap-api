using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.GetInvoiceByID
{
    public class GetInvoiceQueryHandler : IQueryHandler<GetInvoiceQuery, ResponseResult<InvoiceDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetInvoiceQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<InvoiceDto>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository.GetInvoiceInfo(request.InvoiceID, cancellationToken);

            return result == null ?
                ResponseResult<InvoiceDto>.NotFound("No Invoice Found") :
                ResponseResult<InvoiceDto>.OK(result);
        }
    }
}