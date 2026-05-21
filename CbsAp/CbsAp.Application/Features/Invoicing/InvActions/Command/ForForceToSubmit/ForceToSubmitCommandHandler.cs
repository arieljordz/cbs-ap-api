using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Features.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForForceToSubmit
{
    public class ForceToSubmitCommandHandler : ICommandHandler<ForceToSubmitCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        public ForceToSubmitCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(ForceToSubmitCommand request, CancellationToken cancellationToken)
        {
           var dto = request.dto;

            var invRepo = _unitofWork.GetRepository<Invoice>();

            var invoice = await invRepo.Query()
                .FirstOrDefaultAsync(i => i.InvoiceID == dto.InvoiceID, cancellationToken);

            if (invoice == null)
            {
                return ResponseResult<bool>.BadRequest("No Invoice ID found");
            }

            dto.Status ??= InvoiceStatusType.ForApproval;

            var activityLog = InvoicActionLogFactory.CreateInvoiceActivityLog(
                dto,
                invoice.StatusType,
                InvoiceActionType.Submit);

            activityLog.SetAuditFieldsOnCreate(request.UpdatedBy);

            await _unitofWork.GetRepository<InvoiceActivityLog>().AddAsync(activityLog);
            var prevQueue = invoice.QueueType;
            invoice.QueueType = InvoiceQueueType.MyInvoices;
            invoice.StatusType = dto.Status.Value;
            //invoice.ApproverRole = invoice.InvInfoRoutingLevels?.Where(w => w.InvFlowStatus == (int?)InvFlowStatus.Pending).Select(s => s.RoleID).FirstOrDefault().ToString();
            var approver = invoice.InvInfoRoutingLevels?.Where(w => w.InvFlowStatus == 1).FirstOrDefault();
            invoice.InvInfoRoutingLevels.ForEach(f => 
            {
                //f.InvFlowStatus = invoice.QueueType == InvoiceQueueType.MyInvoices ? f.Level == 1 ? (int?)InvFlowStatus.Assigned : (int)InvFlowStatus.Pending
                //    : (int?)InvFlowStatus.Pending;
                if (prevQueue == InvoiceQueueType.MyInvoices)
                {
                    if (f.Level - approver?.Level == 0)
                    {
                        f.InvFlowStatus = (int?)InvFlowStatus.Submitted;
                    }
                    else if (f.Level - approver?.Level == 1)
                    {
                        f.InvFlowStatus = (int?)InvFlowStatus.Assigned;
                        invoice.ApproverRole = f.RoleID;
                    }
                    else 
                    {
                        if(f.InvFlowStatus != 2)
                            f.InvFlowStatus = (int?)InvFlowStatus.Pending;
                    }
                }
                else
                {
                    f.InvFlowStatus = invoice.QueueType == InvoiceQueueType.MyInvoices ? f.Level == 1 ? (int?)InvFlowStatus.Assigned : (int)InvFlowStatus.Pending : (int?)InvFlowStatus.Pending;
                    invoice.ApproverRole = invoice.InvInfoRoutingLevels?.Where(w => w.Level == 1).Select(s => s.RoleID).FirstOrDefault();
                }
            });
            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);
            var module = Enum.GetValues(typeof(InvoiceQueueType)).Cast<InvoiceQueueType>().FirstOrDefault(s => s == prevQueue);
            var isSuccess = await _unitofWork.SaveChanges(request.UpdatedBy,module.ToString(),cancellationToken);
            if (!isSuccess)
            {
                return ResponseResult<bool>.BadRequest("Failed to Force Submit");
            }

            return ResponseResult<bool>.OK(true, "Invoice Force Submit Successfully.");
        }


    
    }
}
