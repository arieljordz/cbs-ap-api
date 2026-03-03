using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Features.Authentication.Common;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Authentication.Commands.SwitchRole
{
    public record SwitchRoleCommand(
        int RoleID,
        string UserName
       ) : ICommand<ResponseResult<AuthenticationTokenResult>>
    {
    }
}