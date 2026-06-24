using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.DimensionSetup.Commands.DeleteDimensionSetup
{
    public record DeleteDimensionSetupCommand(long dimensionSetupId) : ICommand<ResponseResult<bool>>
    {
    }
}