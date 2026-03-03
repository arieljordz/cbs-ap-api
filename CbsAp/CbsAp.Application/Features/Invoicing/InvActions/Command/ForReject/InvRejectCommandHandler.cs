using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Features.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForReject
{
    public class InvRejectCommandHandler : ICommandHandler<InvRejectCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public InvRejectCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(InvRejectCommand request, CancellationToken cancellationToken)
        {
            var invRepo = _unitofWork.GetRepository<Invoice>();

            var dto = request.dto;

            var invoice = await invRepo
                 .Query()
                 .FirstOrDefaultAsync(i => i.InvoiceID == dto.InvoiceID);

            if (invoice == null)
            {
                return ResponseResult<bool>.BadRequest("No Invoice ID found");
            }

            dto.Status ??= InvoiceStatusType.Rejected;

            var activityLog = InvoicActionLogFactory.CreateInvoiceActivityLog(
               dto,
               invoice.StatusType,
               InvoiceActionType.Reject);

            activityLog.SetAuditFieldsOnCreate(request.UpdatedBy);

            await _unitofWork.GetRepository<InvoiceActivityLog>().AddAsync(activityLog);
            invoice.QueueType = InvoiceQueueType.RejectionQueue;
            invoice.StatusType = dto.Status.Value;
            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);

            var saved = await _unitofWork.SaveChanges(cancellationToken);
            if (!saved)
            {
                return ResponseResult<bool>.BadRequest("Failed to update Invoice to Rejected.");
            }

            return ResponseResult<bool>.OK("Invoice has been rejected.");
        }
    }
}