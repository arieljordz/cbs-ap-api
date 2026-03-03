using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Users.Commands.Common
{
    public record GenerateUserPasswordCommand(
        string UserID,
        long UserLogInfoID,
        long UserAccountID,
        string? Password
        ) : IQuery<ResponseResult<UpdatePasswordDTO>>;
}