using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Application.Features.Invoicing.InvAllocationLine.Commands
{
    public class AddAllocationLineCommandHandler : ICommandHandler<AddAllocationLineCommand, ResponseResult<long>>
    {
        private readonly IUnitofWork _unitofWork;

        public AddAllocationLineCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<long>> Handle(AddAllocationLineCommand request, CancellationToken cancellationToken)
        {

            var repo = _unitofWork.GetRepository<InvAllocLine>();
            var dto = request.invoiceDto;
            var invAllocLine = new InvAllocLine() {
                InvAllocLineID = dto.InvAllocLineID,
                InvoiceID = dto.InvoiceID,
                LineNo = dto.LineNo,
                PoNo = dto.PoNo,
                PoLineNo = dto.PoLineNo,
                Qty = dto.Qty,
                LineDescription = dto.LineDescription,
                Note = dto.Note,
                LineNetAmount = dto.LineNetAmount,
                LineTaxAmount = dto.LineTaxAmount,
                LineAmount = dto.LineAmount,
                TaxCodeID = dto.TaxCodeID,
                AccountID = dto.Account,
            };

            invAllocLine.SetAuditFieldsOnCreate(request.CreatedBy);
            await repo.AddAsync(invAllocLine);

            var isSaved = await _unitofWork.SaveChanges(cancellationToken);
            if (!isSaved)
            {
                return ResponseResult<long>.BadRequest("Failed to update Allocation Line ");
            }

            return ResponseResult<long>.OK(invAllocLine.InvAllocLineID);
        }

      
    }
}