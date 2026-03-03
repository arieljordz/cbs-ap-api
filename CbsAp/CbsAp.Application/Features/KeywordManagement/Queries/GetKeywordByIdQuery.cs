using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Keyword;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.KeywordManagement.Queries
{
    public record GetKeywordByIdQuery(long KeywordID) : IQuery<ResponseResult<KeywordDTO>>
    {
    }
}
