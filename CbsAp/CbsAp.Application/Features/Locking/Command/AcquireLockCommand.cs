using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Locking.Command
{
    public record AcquireLockCommand(
        string UserName,
        long Id,
        bool OverrideLock
        ) :
        ICommand<ResponseResult<string>>;
}