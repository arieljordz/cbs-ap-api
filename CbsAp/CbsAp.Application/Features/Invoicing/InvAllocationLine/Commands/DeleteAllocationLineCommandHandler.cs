using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Application.Features.Invoicing.InvAllocationLine.Commands
{
    public class DeleteAllocationLineCommandHandler : ICommandHandler<DeleteAllocationLineCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public DeleteAllocationLineCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(DeleteAllocationLineCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitofWork.GetRepository<InvAllocLine>();
            var invAllocLine = await repo.GetByIdAsync(request.AllocationLineID);
            if (invAllocLine == null)
                return ResponseResult<bool>.NotFound("Allocaction Line is not found");
           
            await repo.DeleteAsync(invAllocLine);

            var isSaved = await _unitofWork.SaveChanges(request.UpdatedBy, "InvoiceAllocation", cancellationToken);
            if (!isSaved)
            {
                return ResponseResult<bool>.BadRequest("Failed to delete Allocation Line ");
            }

            return ResponseResult<bool>.OK("Allocation Line deleted successfully.");
        }

      
    }
}