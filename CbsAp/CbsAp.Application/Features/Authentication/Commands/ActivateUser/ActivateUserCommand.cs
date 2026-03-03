using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Authentication.Commands.ActivateUser
{
    public record ActivateUserCommand(
        string ConfirmationToken,
        string TempPassword,
        string NewPassword,
        bool ActivateUser) : ICommand<ResponseResult<bool>>
    {
    }
}