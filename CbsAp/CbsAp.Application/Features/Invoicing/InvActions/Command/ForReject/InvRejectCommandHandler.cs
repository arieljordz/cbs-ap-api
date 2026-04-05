using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Features.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.ActivityLog;
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

            var newActivityLOg = new ActivityLog
            {
                InvoiceID = (int)request.dto.InvoiceID,
                ActionBy = request.UpdatedBy,
                Activity = "UPDATE",
                Module = invoice.QueueType.ToString(),
                OldValue = null,
                NewValue = string.Format("ROUTE TO REJECT REASON: {0}", request.dto.Reason),
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

            activityLog.SetAuditFieldsOnCreate(request.UpdatedBy);
            var prevQueue = invoice.QueueType;
            await _unitofWork.GetRepository<InvoiceActivityLog>().AddAsync(activityLog);
            invoice.QueueType = InvoiceQueueType.RejectionQueue;
            invoice.StatusType = dto.Status.Value;
            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);
            var module = Enum.GetValues(typeof(InvoiceQueueType)).Cast<InvoiceQueueType>().FirstOrDefault(s => s == prevQueue);
            var saved = await _unitofWork.SaveChanges(request.UpdatedBy,module.ToString(),cancellationToken);
            if (!saved)
            {
                return ResponseResult<bool>.BadRequest("Failed to update Invoice to Rejected.");
            }

            return ResponseResult<bool>.OK("Invoice has been rejected.");
        }
    }
}