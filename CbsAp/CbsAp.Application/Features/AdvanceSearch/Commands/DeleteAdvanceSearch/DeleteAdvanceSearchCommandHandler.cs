using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Shared;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.Entity.Commands.DeleteAdvanceSearch;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.AdvanceSearch;
using CbsAp.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.AdvanceSearch.Commands.DeleteAdvanceSearch
{
    public class DeleteAdvanceSearchCommandHandler : ICommandHandler<DeleteAdvanceSearchCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IDbSetDependencyChecker _modelDependencyChecker;
        private readonly ILogger<DeleteAdvanceSearchCommandHandler> _logger;

        public DeleteAdvanceSearchCommandHandler(
            IUnitofWork unitofWork,
            IDbSetDependencyChecker modelDependencyChecker,
            ILogger<DeleteAdvanceSearchCommandHandler> logger)
        {
            _unitofWork = unitofWork;
            _modelDependencyChecker = modelDependencyChecker;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(DeleteAdvanceSearchCommand request, CancellationToken cancellationToken)
        {
            var result = await _modelDependencyChecker
                .HasDependenciesAsync<CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch> (request.advanceSearchId);

            var responseForDeleteAction = ForDeletionChecker
                .DependencyCheckerResponseResult(result, "AdvanceSearch");

            if (!responseForDeleteAction.IsSuccess)
                return responseForDeleteAction;

            if (!await DeleteAdvanceSearch(request.advanceSearchId, cancellationToken))
            {
                _logger.LogError("Error on Deleting AdvanceSearch with AdvanceSearchProfileID : {AdvanceSearchProfileID}",
                    request.advanceSearchId);

                return ResponseResult<bool>.BadRequest("Error on Deleting AdvanceSearch");
            }
            return ResponseResult<bool>.OK(MessageConstants.Message("AdvanceSearch", MessageOperationType.Delete));
        }

        private async Task<bool> DeleteAdvanceSearch(long advanceSearchId, CancellationToken cancellationToken)
        {
            var AdvanceSearchRepo = _unitofWork.GetRepository<CbsAp.Domain.Entities.AdvanceSearch.AdvanceSearch>();

            var AdvanceSearchToDelete = await AdvanceSearchRepo
                .SingleOrDefaultAsync(e => e.AdvanceSearchId == advanceSearchId);

            if (AdvanceSearchToDelete != null)
            {
                await AdvanceSearchRepo.DeleteAsync(AdvanceSearchToDelete);
                return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
            }

            return false;
        }
    }
}