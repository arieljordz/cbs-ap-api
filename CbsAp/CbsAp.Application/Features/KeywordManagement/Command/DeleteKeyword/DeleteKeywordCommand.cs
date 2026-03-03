using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.KeywordManagement.Command
{
    public record DeleteKeywordCommand(long KeywordID) : ICommand<ResponseResult<bool>>
    {
    }
}