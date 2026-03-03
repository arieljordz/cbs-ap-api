using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Authentication.Commands.SetNewPassword
{
    public record SetNewPasswordCommand(string PasswordrecoveryToken, string NewPassword)
        : ICommand<ResponseResult<bool>>
    {

    }
}