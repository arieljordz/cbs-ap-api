using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Shared;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.KeywordManagement.Command
{
    public class DeleteKeywordCommandHandler : ICommandHandler<DeleteKeywordCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IDbSetDependencyChecker _modelDependencyChecker;
        private readonly ILogger<DeleteKeywordCommandHandler> _logger;

        public DeleteKeywordCommandHandler(
            IUnitofWork unitofWork,
            IDbSetDependencyChecker modelDependencyChecker,
            ILogger<DeleteKeywordCommandHandler> logger)
        {
            _unitofWork = unitofWork;
            _modelDependencyChecker = modelDependencyChecker;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(DeleteKeywordCommand request, CancellationToken cancellationToken)
        {
            var keywordRepo = _unitofWork.GetRepository<Keyword>();

            var keywordToDelete = await keywordRepo
                .SingleOrDefaultAsync(e => e.KeywordID == request.KeywordID);

            var invoiceRepo = _unitofWork.GetRepository<Invoice>();
            var isKeywordActive = await invoiceRepo.AnyAsync(a => a.KeywordID == request.KeywordID);

            if (isKeywordActive)
                return ResponseResult<bool>.BadRequest("This keyword is currently in use and cannot be deleted.");

            if (keywordToDelete != null)
            {
                await keywordRepo.DeleteAsync(keywordToDelete);
                await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
            }
            
            return ResponseResult<bool>.OK(MessageConstants.Message("Keyword", MessageOperationType.Delete));
        }
        
    }
}