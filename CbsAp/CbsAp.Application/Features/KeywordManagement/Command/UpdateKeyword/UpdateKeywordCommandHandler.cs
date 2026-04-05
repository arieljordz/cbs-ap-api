using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.KeywordManagement.Command
{
    public class UpdateKeywordCommandHandler : ICommandHandler<UpdateKeywordCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitOfWork;

        public UpdateKeywordCommandHandler(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(UpdateKeywordCommand request, CancellationToken cancellationToken)
        {
            var keywordRepository = _unitOfWork.GetRepository<Keyword>();
            var entity = await keywordRepository.GetByIdAsync(request.KeywordID);

            if (entity == null)
            {
                return ResponseResult<bool>.NotFound("Keyword not found.");
            }

            if (!request.KeywordName.Equals(entity.KeywordName))
            {
                //check if keyword name already exists
                var keywordExists = keywordRepository.Query().Any(x => x.KeywordName == request.KeywordName && x.KeywordID != request.KeywordID);
                if (keywordExists)
                {
                    return ResponseResult<bool>.Confict("Keyword already exists.");
                }
            }

            var invoiceRepo = _unitOfWork.GetRepository<Invoice>();
            var isKeywordActive = await invoiceRepo.AnyAsync(a => a.KeywordID == request.KeywordID);

            if (isKeywordActive)
                return ResponseResult<bool>.BadRequest("This keyword is currently in use and cannot be edited.");

            entity.EntityProfileID = request.EntityProfileID;
            entity.InvoiceRoutingFlowID = request.InvoiceRoutingFlowID;
            entity.KeywordName = request.KeywordName;
            entity.IsActive = request.IsActive;
            entity.SetAuditFieldsOnUpdate(request.LastUpdatedBy);
            await keywordRepository.UpdateAsync(entity.KeywordID, entity);
            bool success = await _unitOfWork.SaveChanges(string.Empty,string.Empty,cancellationToken);

            return success ?
                ResponseResult<bool>.OK(MessageConstants.Message("Keyword", MessageOperationType.Update)):
                ResponseResult<bool>.BadRequest(MessageConstants.FormatMessage(MessageConstants.UpdateError, "Keyword"));

            
        }
    }
}