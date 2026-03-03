using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Keywords;

namespace CbsAp.Application.Features.KeywordManagement.Command
{
    public class UpdateKeywordCommandHandler : ICommandHandler<UpdateKeywordCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;

        public UpdateKeywordCommandHandler(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public async Task<ResponseResult<string>> Handle(UpdateKeywordCommand request, CancellationToken cancellationToken)
        {
            var keywordRepository = _unitOfWork.GetRepository<Keyword>();
            var entity = await keywordRepository.GetByIdAsync(request.KeywordID);

            if (entity == null)
            {
                return ResponseResult<string>.NotFound("Keyword not found.");
            }

            if (!request.KeywordName.Equals(entity.KeywordName))
            {
                //check if keyword name already exists
                var keywordExists = keywordRepository.Query().Any(x => x.KeywordName == request.KeywordName && x.KeywordID != request.KeywordID);
                if (keywordExists)
                {
                    return ResponseResult<string>.Confict("Keyword already exists.");
                }
            }

            entity.EntityProfileID = request.EntityProfileID;
            entity.InvoiceRoutingFlowID = request.InvoiceRoutingFlowID;
            entity.KeywordName = request.KeywordName;
            entity.IsActive = request.IsActive;
            entity.SetAuditFieldsOnUpdate(request.LastUpdatedBy);
            await keywordRepository.UpdateAsync(entity.KeywordID, entity);
            bool success = await _unitOfWork.SaveChanges(cancellationToken);

            return success ?
                ResponseResult<string>.Success(MessageConstants.FormatMessage(MessageConstants.UpdateSuccess, request.KeywordName)) :
                ResponseResult<string>.BadRequest(MessageConstants.FormatMessage(MessageConstants.UpdateError, "Keyword"));
        }
    }
}