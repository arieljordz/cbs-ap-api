using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Commands.LockUnlockUser
{
    public record LockUnlockUserCommand(bool IsLockedOut, long UserAccountID) : ICommand<ResponseResult<string>>
    {
    }
}
