using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.DimensionSetup;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.DimensionSetup;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.DimensionSetup.Commands.UpdateDimensionSetup
{
    public class UpdateDimensionSetupCommandHandler : ICommandHandler<UpdateDimensionSetupCommand, ResponseResult<bool>>
    {
        private readonly IDimensionSetupService _dimensionSetupService;

        private readonly IUnitofWork _unitofWork;

        private readonly ILogger<UpdateDimensionSetupCommandHandler> _logger;

        public UpdateDimensionSetupCommandHandler(IDimensionSetupService dimensionSetupService, IUnitofWork unitofWork, ILogger<UpdateDimensionSetupCommandHandler> logger)
        {
            _dimensionSetupService = dimensionSetupService;
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(UpdateDimensionSetupCommand request, CancellationToken cancellationToken)
        {
            var DimensionSetup = await _unitofWork.GetRepository<CbsAp.Domain.Entities.DimensionSetup.DimensionSetup>().GetByIdAsync(request.dimensionSetup.DimensionSetupId);
            DimensionSetup.CreatedBy = DimensionSetup.CreatedBy!;
            DimensionSetup.CreatedDate = DimensionSetup.CreatedDate!;

            var mapUpdateDimensionSetup = request.dimensionSetup.Adapt(DimensionSetup);
            mapUpdateDimensionSetup.Required = request.dimensionSetup.Required;
            mapUpdateDimensionSetup.Show = request.dimensionSetup.Show;
 
            mapUpdateDimensionSetup.SetAuditFieldsOnUpdate(request.updatedBy);

            if (!await _dimensionSetupService.UpdateDimensionSetup(mapUpdateDimensionSetup, cancellationToken))
            {
                _logger.LogWarning("Error in updating DimensionSetup  : {Name}", DimensionSetup.DimensionSetupName);
                return ResponseResult<bool>.BadRequest("Error on updating DimensionSetup");
            }

            return ResponseResult<bool>.OK(MessageConstants.Message(MessageOperationType.Update, "DimensionSetup"));
        }

    }
}