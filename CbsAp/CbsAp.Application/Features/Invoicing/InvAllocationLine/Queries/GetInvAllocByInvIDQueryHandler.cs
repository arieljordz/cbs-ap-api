using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvAllocationLine.Queries
{
    public class GetInvAllocByInvIDQueryHandler : IQueryHandler<GetInvAllocByInvIDQuery, ResponseResult<List<InvAllocEntryDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetInvAllocByInvIDQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<List<InvAllocEntryDto>>> Handle(GetInvAllocByInvIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository
                .GetInvoiceAllocationInfo(request.InvoiceID, cancellationToken);

            return result == null ?
                ResponseResult<List<InvAllocEntryDto>>.NotFound("No Invoice Found") :
                ResponseResult<List<InvAllocEntryDto>>.OK(result);
        }
    }
}