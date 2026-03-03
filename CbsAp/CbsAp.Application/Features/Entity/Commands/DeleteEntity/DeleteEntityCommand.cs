using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Entity.Commands.DeleteEntity
{
    public record DeleteEntityCommand(long EntityProfileID) : ICommand<ResponseResult<bool>>
    {
    }
}