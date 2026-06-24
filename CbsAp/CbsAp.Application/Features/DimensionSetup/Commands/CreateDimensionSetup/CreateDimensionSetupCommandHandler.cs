using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.DimensionSetup;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.DimensionSetup;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.DimensionSetup.Commands.CreateDimensionSetup
{
    public class CreateDimensionSetupCommandHandler : ICommandHandler<CreateDimensionSetupCommand, ResponseResult<bool>>
    {
        private readonly IDimensionSetupService _dimensionSetupService;
        private readonly ILogger<CreateDimensionSetupCommandHandler> _logger;

        public CreateDimensionSetupCommandHandler(
           IDimensionSetupService dimensionSetupService,
           ILogger<CreateDimensionSetupCommandHandler> logger
            )
        {
            _dimensionSetupService = dimensionSetupService;
           _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(CreateDimensionSetupCommand request, CancellationToken cancellationToken)
        {
            var DimensionSetup = request.dimensionSetup.Adapt<CbsAp.Domain.Entities.DimensionSetup.DimensionSetup>();
            DimensionSetup.Required = request.dimensionSetup.Required;
            DimensionSetup.Show = request.dimensionSetup.Show;

            DimensionSetup.SetAuditFieldsOnCreate(request.createdBy);

            if (!await _dimensionSetupService.CreateDimensionSetup(DimensionSetup, cancellationToken))
            {
                _logger.LogError("Error in adding new DimensionSetup : {Name}", DimensionSetup.DimensionSetupName);
                return ResponseResult<bool>.BadRequest("Error adding new DimensionSetup");
            }

            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Create, "DimensionSetup"));
        }
    }
}
