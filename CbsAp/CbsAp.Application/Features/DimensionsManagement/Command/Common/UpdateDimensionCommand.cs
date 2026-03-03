using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.DimensionsManagement.Command.Common
{
    public record UpdateDimensionCommand(
        long DimensionID,
        long EntityProfileID,
        string Dimension,
        string Name,
        bool Active,
        string? FreeField1,
        string? FreeField2,
        string? FreeField3,
        string LastUpdatedBy
    ) : ICommand<ResponseResult<string>>;
}
