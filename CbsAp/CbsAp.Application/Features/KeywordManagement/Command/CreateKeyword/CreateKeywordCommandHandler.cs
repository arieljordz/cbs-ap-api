using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Keywords;

namespace CbsAp.Application.Features.KeywordManagement.Command
{
    public class CreateKeywordCommandHandler : ICommandHandler<CreateKeywordCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;

        public CreateKeywordCommandHandler(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public async Task<ResponseResult<string>> Handle(CreateKeywordCommand request, CancellationToken cancellationToken)
        {
            var keyword = new Keyword
            {
                CreatedBy = request.CreatedBy,
                KeywordName = request.KeywordName,
                EntityProfileID = request.EntityProfileID,
                IsActive = request.IsActive,
                InvoiceRoutingFlowID = request.InvoiceRoutingFlowID
            };
            var keywordRepository = _unitOfWork.GetRepository<Keyword>();
            //check if keyword name already exists
            var keywordExists = keywordRepository.Query().Any(x => x.KeywordName == request.KeywordName);
            if (keywordExists)
            {
                return ResponseResult<string>.NotFound("Keyword already exists.");
            }
            await keywordRepository.AddAsync(keyword);
            bool success = await _unitOfWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
            if (!success)
            {
                return ResponseResult<string>.BadRequest(
                   MessageConstants.FormatMessage(MessageConstants.AddError, "keyword"));
            }

            return ResponseResult<string>.Created(request.KeywordName,
                       MessageConstants.FormatMessage(MessageConstants.AddSuccess, request.KeywordName));
        }
    }
}