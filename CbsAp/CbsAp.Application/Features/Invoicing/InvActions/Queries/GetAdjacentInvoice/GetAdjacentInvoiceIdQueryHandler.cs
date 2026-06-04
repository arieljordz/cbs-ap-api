using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.GetAdjacentInvoice
{
    public class GetAdjacentInvoiceIdQueryHandler : IQueryHandler<GetAdjacentInvoiceIdQuery, ResponseResult<long?>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetAdjacentInvoiceIdQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<long?>> Handle(GetAdjacentInvoiceIdQuery request, CancellationToken cancellationToken)
        {
            var navigationResult = await _invoiceRepository.GetAdjacentInvoiceId(
                request.InvoiceID,
                request.IsNext,
                request.StatusType,
                request.QueueType,
                cancellationToken);

            if (!navigationResult.CurrentInvoiceExists)
            {
                return ResponseResult<long?>.NotFound("Invoice not found.");
            }

            return ResponseResult<long?>.OK(navigationResult.InvoiceId);
        }
    }
}
