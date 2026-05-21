

using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.AutoMatching
{
    public record MatchPOCommand:ICommand<ResponseResult<bool>>
    {
    }
}
