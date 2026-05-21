using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Features.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.ActivityLog;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ChangeHoldState
{
    public class InvChangeHoldStateCommandHandler : ICommandHandler<InvChangeHoldStateCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMediator _mediator;

        public InvChangeHoldStateCommandHandler(IUnitofWork unitofWork, IMediator mediator, IInvoiceRepository invoiceRepository)
        {
            _unitofWork = unitofWork;
            _mediator = mediator;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<bool>> Handle( InvChangeHoldStateCommand request, CancellationToken cancellationToken)
        {
            var success = await _invoiceRepository.ChangeHoldStateAsync(
                request.dto,
                request.UpdatedBy,
                cancellationToken
            );

            if (!success)
            {
                return ResponseResult<bool>.BadRequest("No Invoice ID found");
            }

            return ResponseResult<bool>.OK("Invoice status updated successfully.");
        }
    }
}
