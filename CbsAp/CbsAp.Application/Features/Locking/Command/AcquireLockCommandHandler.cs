using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Application.Features.Locking.Command
{
    public class AcquireLockCommandHandler : ICommandHandler<AcquireLockCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;

        public AcquireLockCommandHandler(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public async Task<ResponseResult<string>> Handle(AcquireLockCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Invoice>();
            var invoice = await repository.GetByIdAsync(request.Id);
            if (invoice != null)
            {
                string lockedBy = invoice.LockedBy ?? "";
                if (!string.IsNullOrEmpty(lockedBy) && lockedBy != request.UserName)
                {
                    if (request.OverrideLock)
                    {
                        invoice.LockedBy = request.UserName;
                        invoice.LockedDate = DateTime.UtcNow;
                        await repository.UpdateAsync(invoice.InvoiceID, invoice);
                        await _unitOfWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
                        return ResponseResult<string>.Success("");
                    }
                    else
                    {
                        int lockTimeoutMinutes = 15; //todo: put it to config
                        if (invoice.LockedDate != null && (DateTime.UtcNow - invoice.LockedDate) >= TimeSpan.FromMinutes(lockTimeoutMinutes))
                        {
                            invoice.LockedBy = request.UserName;
                            invoice.LockedDate = DateTime.UtcNow;
                            await repository.UpdateAsync(invoice.InvoiceID, invoice);
                            await _unitOfWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
                            return ResponseResult<string>.Success("");
                        }
                        else
                        {
                            return ResponseResult<string>.Success(lockedBy);
                        }
                    }
                }
                else
                {
                    invoice.LockedBy = request.UserName;
                    invoice.LockedDate = DateTime.UtcNow;
                    await repository.UpdateAsync(invoice.InvoiceID, invoice);
                    await _unitOfWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
                    return ResponseResult<string>.Success("");
                }

            }

            return ResponseResult<string>.Success(""); 
        }
    }
}