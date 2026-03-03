using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Commands.DeleteUser
{
    public record DeactivateUserCommand(long UserAccountID, string updatedBy)
        : ICommand<ResponseResult<string>>
    {
    }
}