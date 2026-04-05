using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Features.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.ActivityLog;
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


            var newActivityLOg = new ActivityLog
            {
                InvoiceID = (int)request.dto.InvoiceID,
                ActionBy = request.UpdatedBy,
                Activity = "UPDATE",
                Module = invoice.QueueType.ToString(),
                OldValue = null,
                NewValue = string.Format("ROUTE TO REACTIVATE REASON: {0}", request.dto.Reason),
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

            await _unitofWork.GetRepository<InvoiceActivityLog>().AddAsync(activityLog);
            var prevQueue = invoice.QueueType;
            invoice.QueueType = InvoiceQueueType.ExceptionQueue;
            invoice.StatusType = dto.Status.Value;
            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);
            
            var module = Enum.GetValues(typeof(InvoiceQueueType)).Cast<InvoiceQueueType>().FirstOrDefault(s => s == prevQueue);
            var isSuccess = await _unitofWork.SaveChanges(request.UpdatedBy,module.ToString(),cancellationToken);
            if (!isSuccess)
            {
                return ResponseResult<bool>.BadRequest("Failed to reactivate Invoice");
            }

            return ResponseResult<bool>.OK(true, "Invoice reactivated.");
        }
    }
}
