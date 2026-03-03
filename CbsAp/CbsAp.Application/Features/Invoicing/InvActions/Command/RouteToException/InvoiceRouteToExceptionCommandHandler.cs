using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.RouteToException
{
    internal class InvoiceRouteToExceptionCommandHandler : ICommandHandler<InvoiceRouteToExceptionCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public InvoiceRouteToExceptionCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(InvoiceRouteToExceptionCommand request, CancellationToken cancellationToken)
        {
            var invoiceContext = _unitofWork.GetRepository<Invoice>();

            var invoice = await invoiceContext
                             .Query()
                             .FirstOrDefaultAsync(i => i.InvoiceID == request.dto.InvoiceID, cancellationToken: cancellationToken);

            if (invoice == null)
            {
                return ResponseResult<bool>.BadRequest("No Invoice ID found");
            }

            //TODO : // VALIDATION IN the next user story

            var actityLog = new InvoiceActivityLog
            {
                InvoiceID = request.dto.InvoiceID,
                PreviousStatus = invoice.StatusType,
                CurrentStatus = request.dto.Status,
                Reason = request.dto.Reason

            };

            invoice.QueueType = InvoiceQueueType.ExceptionQueue;
            invoice.StatusType = InvoiceStatusType.Exception;
            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);

            await _unitofWork.GetRepository<InvoiceActivityLog>().AddAsync(actityLog);

            var isSuccess = await _unitofWork.SaveChanges(cancellationToken);
            if (!isSuccess)
            {
                return ResponseResult<bool>.BadRequest("Failed to route to exception");
            }

            return ResponseResult<bool>.OK(true, "Invoice routed to exception.");
        }
    }
}
