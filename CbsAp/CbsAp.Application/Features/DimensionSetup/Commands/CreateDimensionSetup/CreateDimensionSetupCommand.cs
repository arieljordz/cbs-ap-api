using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.DimensionSetup.Commands.CreateDimensionSetup
{
    public record CreateDimensionSetupCommand(DimensionSetupDto dimensionSetup, string createdBy) : ICommand<ResponseResult<bool>>
    {
    }
}
