using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.DimensionsManagement.Command.Common;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Dimensions;

namespace CbsAp.Application.Features.DimensionsManagement.Command
{
    public class UpdateDimensionCommandHandler : ICommandHandler<UpdateDimensionCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;

        public UpdateDimensionCommandHandler(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseResult<string>> Handle(UpdateDimensionCommand request, CancellationToken cancellationToken)
        {
            var dimensionRepository = _unitOfWork.GetRepository<Dimension>();

            var dimensionQuery = await dimensionRepository.ApplyPredicateAsync(d => d.DimensionID == request.DimensionID);
            var existingDimension = dimensionQuery.FirstOrDefault();

            if (existingDimension == null)
            {
                return ResponseResult<string>.NotFound("Dimension not found.");
            }

            var updatedDimension = new Dimension
            {
                DimensionID = existingDimension.DimensionID,
                DimensionCode = request.Dimension,
                Name = request.Name,
                IsActive = request.Active,
                FreeField1 = request.FreeField1,
                FreeField2 = request.FreeField2,
                FreeField3 = request.FreeField3,
                EntityProfileID = request.EntityProfileID,
                CreatedBy = existingDimension.CreatedBy,
                CreatedDate = existingDimension.CreatedDate,
                LastUpdatedBy = request.LastUpdatedBy,
                LastUpdatedDate = DateTimeOffset.UtcNow
            };

            await dimensionRepository.UpdateAsync(updatedDimension.DimensionID, updatedDimension);
            var success = await _unitOfWork.SaveChanges(cancellationToken);

            return success
                ? ResponseResult<string>.Success(MessageConstants.FormatMessage(MessageConstants.UpdateSuccess, request.Dimension))
                : ResponseResult<string>.BadRequest(MessageConstants.FormatMessage(MessageConstants.UpdateError, "dimension"));
        }
    }
}
