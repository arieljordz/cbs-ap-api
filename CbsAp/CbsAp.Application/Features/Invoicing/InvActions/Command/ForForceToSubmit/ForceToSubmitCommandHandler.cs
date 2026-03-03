using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Features.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
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

            invoice.QueueType = InvoiceQueueType.MyInvoices;
            invoice.StatusType = dto.Status.Value;
            invoice.SetAuditFieldsOnUpdate(request.UpdatedBy);

            var isSuccess = await _unitofWork.SaveChanges(cancellationToken);
            if (!isSuccess)
            {
                return ResponseResult<bool>.BadRequest("Failed to Force Submit");
            }

            return ResponseResult<bool>.OK(true, "Invoice Force Submit Successfully.");
        }


    
    }
}
