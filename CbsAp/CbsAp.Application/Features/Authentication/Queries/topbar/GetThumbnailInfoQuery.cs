using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.Authentication.Queries.topbar
{
    public record GetThumbnailInfoQuery(string UserName) : IQuery<ResponseResult<string>>
    {
    }
}
