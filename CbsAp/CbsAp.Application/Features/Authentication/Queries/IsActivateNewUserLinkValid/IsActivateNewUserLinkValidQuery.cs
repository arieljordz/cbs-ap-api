using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Authentication.Queries.IsActivateNewUserLinkValid
{
    public record IsActivateNewUserLinkValidQuery(string Token)
        : IQuery<ResponseResult<bool>>
    {
    }
}
