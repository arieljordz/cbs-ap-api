using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.DimensionsManagement.Command.Common
{
    public record CreateDimensionCommand(
        long EntityProfileID,
        string Dimension,
        string Name,
        bool Active,
        string? FreeField1,
        string? FreeField2,
        string? FreeField3,
        string CreatedBy
    ) : ICommand<ResponseResult<string>>;
}
