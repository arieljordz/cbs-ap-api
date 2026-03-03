using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.DimensionsManagement.Command.Common;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Dimensions;

namespace CbsAp.Application.Features.DimensionsManagement.Command
{
    public class CreateDimensionCommandHandler : ICommandHandler<CreateDimensionCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;

        public CreateDimensionCommandHandler(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseResult<string>> Handle(CreateDimensionCommand request, CancellationToken cancellationToken)
        {
            var dimension = new Dimension
            {
                DimensionCode = request.Dimension,
                Name = request.Name,
                IsActive = request.Active,
                FreeField1 = request.FreeField1,
                FreeField2 = request.FreeField2,
                FreeField3 = request.FreeField3,
                EntityProfileID = request.EntityProfileID,
                CreatedBy = request.CreatedBy,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _unitOfWork.GetRepository<Dimension>().AddAsync(dimension);
            var success = await _unitOfWork.SaveChanges(cancellationToken);

            if (!success)
            {
                return ResponseResult<string>.BadRequest(
                    MessageConstants.FormatMessage(MessageConstants.AddError, "dimension"));
            }

            return ResponseResult<string>.Created(request.Dimension,
                MessageConstants.FormatMessage(MessageConstants.AddSuccess, request.Dimension));
        }
    }
}
