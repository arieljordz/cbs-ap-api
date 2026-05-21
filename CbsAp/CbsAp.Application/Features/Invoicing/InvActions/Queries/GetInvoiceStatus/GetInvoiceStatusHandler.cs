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
        private readonly IInvoiceRepository _invoiceRepository;

        public GetInvoiceStatusHandler(IUnitofWork unitofWork, IInvoiceRepository invoiceRepository)
        {
            _unitofWork = unitofWork;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<GetInvoiceStatusDto>> Handle(GetInvoiceStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository.GetInvoiceStatusAsync(
                request.invoiceID,
                cancellationToken
            );

            return ResponseResult<GetInvoiceStatusDto>.OK(result);
        }
    }
}