using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Features.Authentication.Common;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Authentication.Queries
{
    public record LoginQuery(string Username, string Password)
        : IQuery<ResponseResult<AuthenticationTokenResult>>;
}