using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Entity.Commands.DeleteAdvanceSearch
{
    public record DeleteAdvanceSearchCommand(long advanceSearchId) : ICommand<ResponseResult<bool>>
    {
    }
}