using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Shared;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.DimensionSetup;
using CbsAp.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.DimensionSetup.Commands.DeleteDimensionSetup
{
    public class DeleteDimensionSetupCommandHandler : ICommandHandler<DeleteDimensionSetupCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IDbSetDependencyChecker _modelDependencyChecker;
        private readonly ILogger<DeleteDimensionSetupCommandHandler> _logger;

        public DeleteDimensionSetupCommandHandler(
            IUnitofWork unitofWork,
            IDbSetDependencyChecker modelDependencyChecker,
            ILogger<DeleteDimensionSetupCommandHandler> logger)
        {
            _unitofWork = unitofWork;
            _modelDependencyChecker = modelDependencyChecker;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(DeleteDimensionSetupCommand request, CancellationToken cancellationToken)
        {

            if (!await DeleteDimensionSetup(request.dimensionSetupId, cancellationToken))
            {
                _logger.LogError("Error on Deleting DimensionSetup with DimensionSetup ID : {DimensionSetupID}",
                    request.dimensionSetupId);

                return ResponseResult<bool>.BadRequest("Error on Deleting DimensionSetup");
            }
            return ResponseResult<bool>.OK(MessageConstants.Message("DimensionSetup", MessageOperationType.Delete));
        }

        private async Task<bool> DeleteDimensionSetup(long dimensionSetupId, CancellationToken cancellationToken)
        {
            var DimensionSetupRepo = _unitofWork.GetRepository<CbsAp.Domain.Entities.DimensionSetup.DimensionSetup>();

            var DimensionSetupToDelete = await DimensionSetupRepo
                .SingleOrDefaultAsync(e => e.DimensionSetupId == dimensionSetupId);

            if (DimensionSetupToDelete != null)
            {
                await DimensionSetupRepo.DeleteAsync(DimensionSetupToDelete);
                return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
            }

            return false;
        }
    }
}