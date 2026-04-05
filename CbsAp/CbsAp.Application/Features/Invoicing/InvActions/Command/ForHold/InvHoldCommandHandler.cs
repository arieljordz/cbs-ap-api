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

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForHold
{
    public class InvHoldCommandHandler : ICommandHandler<InvHoldCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        private readonly IMediator _mediator;

        public InvHoldCommandHandler(IUnitofWork unitofWork, IMediator mediator)
        {
            _unitofWork = unitofWork;
            _mediator = mediator;
        }

        public async Task<ResponseResult<bool>> Handle(InvHoldCommand request, CancellationToken cancellationToken)
        {
            var invRepo = _unitofWork.GetRepository<Invoice>();

            var invoice = await invRepo
                 .Query()
                 .FirstOrDefaultAsync(i => i.InvoiceID == request.dto.InvoiceID);

            if (invoice == null)
            {
                return ResponseResult<bool>.BadRequest("No Invoice ID found");
            }

            var activityLog = InvoicActionLogFactory.CreateInvoiceActivityLog(
                request.dto,
                invoice.StatusType,
                InvoiceActionType.Hold);

            var newActivityLOg = new ActivityLog
            {
                InvoiceID = (int)request.dto.InvoiceID,
                ActionBy = request.UpdatedBy,
                Activity = "UPDATE",
                Module = invoice.QueueType.ToString(),
                OldValue = null,
                NewValue = string.Format("ROUTE TO HOLD REASON: {0}", request.dto.Reason),
                ColumnName = "Reason",
                metaDataOld = null,
                metaDataNew = null,
                MetaData = null,
                ActivityDate = DateTime.UtcNow,
                CreatedBy = null,
                CreatedDate = null,
                LastUpdatedBy = null,
                LastUpdatedDate = null
            };

            await _unitofWork.GetRepository<ActivityLog>().AddAsync(newActivityLOg);


            invoice.StatusType = request.dto.Status;

            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);
            activityLog.SetAuditFieldsOnCreate(request.UpdatedBy);

            await _unitofWork.GetRepository<InvoiceActivityLog>().AddAsync(activityLog);
            var module = Enum.GetValues(typeof(InvoiceQueueType)).Cast<InvoiceQueueType>().FirstOrDefault(s => s == invoice.QueueType);
            var saved = await _unitofWork.SaveChanges(request.UpdatedBy,module.ToString(),cancellationToken);
            if (!saved)
            {
                return ResponseResult<bool>.BadRequest("Failed to update Invoice to Approve on Hold");
            }

            return ResponseResult<bool>.OK("Invoice is now OnHold.");
        }
    }
}
