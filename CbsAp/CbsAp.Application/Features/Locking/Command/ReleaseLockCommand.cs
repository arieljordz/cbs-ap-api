using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Locking.Command
{
    public record ReleaseLockCommand(
        string UserName,
        long Id
        ) :
        ICommand<ResponseResult<bool>>;
}