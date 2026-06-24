using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.DimensionSetup.Commands.UpdateDimensionSetup
{
    public record UpdateDimensionSetupCommand(DimensionSetupDto dimensionSetup, string updatedBy) : ICommand<ResponseResult<bool>>
    {
    }
}
