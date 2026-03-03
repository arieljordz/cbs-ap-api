using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Application.Features.Locking.Command
{
    public class ReleaseLockCommandHandler : ICommandHandler<ReleaseLockCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitOfWork;

        public ReleaseLockCommandHandler(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseResult<bool>> Handle(ReleaseLockCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Invoice>();
            var invoice = await repository.GetByIdAsync(request.Id);
            if (invoice != null)
            {
                invoice.LockedBy = null; ;
                invoice.LockedDate = null;
                await repository.UpdateAsync(invoice.InvoiceID, invoice);
                await _unitOfWork.SaveChanges(cancellationToken);
            }
            return ResponseResult<bool>.Success(true);
        }
    }
}