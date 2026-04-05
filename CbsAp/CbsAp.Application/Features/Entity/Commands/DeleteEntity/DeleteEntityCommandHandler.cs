using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Shared;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Entity.Commands.DeleteEntity
{
    public class DeleteEntityCommandHandler : ICommandHandler<DeleteEntityCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IDbSetDependencyChecker _modelDependencyChecker;
        private readonly ILogger<DeleteEntityCommandHandler> _logger;

        public DeleteEntityCommandHandler(
            IUnitofWork unitofWork,
            IDbSetDependencyChecker modelDependencyChecker,
            ILogger<DeleteEntityCommandHandler> logger)
        {
            _unitofWork = unitofWork;
            _modelDependencyChecker = modelDependencyChecker;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(DeleteEntityCommand request, CancellationToken cancellationToken)
        {
            var result = await _modelDependencyChecker
                .HasDependenciesAsync<EntityProfile>(request.EntityProfileID);

            var responseForDeleteAction = ForDeletionChecker
                .DependencyCheckerResponseResult(result, "entity");

            if (!responseForDeleteAction.IsSuccess)
                return responseForDeleteAction;

            if (!await DeleteEntity(request.EntityProfileID, cancellationToken))
            {
                _logger.LogError("Error on Deleting Entity with EntityProfileID : {EntityProfileID}",
                    request.EntityProfileID);

                return ResponseResult<bool>.BadRequest("Error on Deleting Entity");
            }
            return ResponseResult<bool>.OK(MessageConstants.Message("entity", MessageOperationType.Delete));
        }

        private async Task<bool> DeleteEntity(long entityProfileID, CancellationToken cancellationToken)
        {
            var entityRepo = _unitofWork.GetRepository<EntityProfile>();

            var entityToDelete = await entityRepo
                .SingleOrDefaultAsync(e => e.EntityProfileID == entityProfileID);

            if (entityToDelete != null)
            {
                await entityRepo.DeleteAsync(entityToDelete);
                return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
            }

            return false;
        }
    }
}