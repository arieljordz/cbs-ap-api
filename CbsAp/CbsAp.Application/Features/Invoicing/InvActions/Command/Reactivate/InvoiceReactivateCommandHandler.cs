using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Features.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.Reactivate
{
    public class InvoiceReactivateCommandHandler : ICommandHandler<InvoiceReactivateCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public InvoiceReactivateCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(InvoiceReactivateCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;

            var invoiceContext = _unitofWork.GetRepository<Invoice>();

            var invoice = await invoiceContext
                             .Query()
                             .FirstOrDefaultAsync(i => i.InvoiceID == dto.InvoiceID, cancellationToken: cancellationToken);

            if( invoice == null)
            {
                return ResponseResult<bool>.BadRequest("No Invoice ID found");
            }

            dto.Status ??= InvoiceStatusType.Exception;

            var activityLog = InvoicActionLogFactory.CreateInvoiceActivityLog(
                dto,
                invoice.StatusType,
                InvoiceActionType.Reactive);

            activityLog.SetAuditFieldsOnCreate(request.UpdatedBy);

            await _unitofWork.GetRepository<InvoiceActivityLog>().AddAsync(activityLog);

            invoice.QueueType = InvoiceQueueType.ExceptionQueue;
            invoice.StatusType = dto.Status.Value;
            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);

            var isSuccess = await _unitofWork.SaveChanges(cancellationToken);
            if (!isSuccess)
            {
                return ResponseResult<bool>.BadRequest("Failed to reactivate Invoice");
            }

            return ResponseResult<bool>.OK(true, "Invoice reactivated.");
        }
    }
}
