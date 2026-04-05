using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceArchiveActions;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.ActivityLog;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

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


            var prevQueue = invoice.QueueType;

            var newActivityLOg = new ActivityLog
            {
                InvoiceID = (int)request.dto.InvoiceID,
                ActionBy = request.UpdatedBy,
                Activity = "UPDATE",
                Module = invoice.QueueType.ToString(),
                OldValue = null,
                NewValue = string.Format("ROUTE TO EXCEPTION REASON: {0}",request.dto.Reason),
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

            invoice.QueueType = InvoiceQueueType.ExceptionQueue;
            invoice.StatusType = InvoiceStatusType.Exception;
            invoice.ApproverRole = null;
            invoice.InvInfoRoutingLevels.ForEach(f => { f.InvFlowStatus = 0; });
            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);

            await _unitofWork.GetRepository<InvoiceActivityLog>().AddAsync(actityLog);
            var module = Enum.GetValues(typeof(InvoiceQueueType)).Cast<InvoiceQueueType>().FirstOrDefault(s => s == prevQueue);
            var isSuccess = await _unitofWork.SaveChanges(request.UpdatedBy,module.ToString(),cancellationToken);
            if (!isSuccess)
            {
                return ResponseResult<bool>.BadRequest("Failed to route to exception");
            }

            return ResponseResult<bool>.OK(true, "Invoice routed to exception.");
        }
    }
}
