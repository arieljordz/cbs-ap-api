using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.GetInvoiceStatus
{
    public class GetInvoiceStatusHandler
        : IQueryHandler<GetInvoiceStatusQuery, ResponseResult<GetInvoiceStatusDto>>
    {
        private readonly IUnitofWork _unitofWork;

        public GetInvoiceStatusHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<GetInvoiceStatusDto>> Handle(GetInvoiceStatusQuery request, CancellationToken cancellationToken)
        {
            var invRepo = _unitofWork.GetRepository<Invoice>();
            var query = await invRepo
                .Query()
                .AsNoTracking()
                .Where(i => i.InvoiceID == request.invoiceID)
                .Select(i => new GetInvoiceStatusDto
                {
                    Status = i.StatusType!,
                    Queue = i.QueueType

                })
                .FirstOrDefaultAsync(cancellationToken);

            return ResponseResult<GetInvoiceStatusDto>.OK(query);
        }
    }
}