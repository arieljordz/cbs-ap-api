using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.AdvanceSearch;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.AdvanceSearch.Commands.UpdateAdvanceSearch
{
    public record UpdateAdvanceSearchCommand(AdvanceSearchRequestForm AdvanceSearch, string updatedBy) : ICommand<ResponseResult<long>>
    {
    }
}
