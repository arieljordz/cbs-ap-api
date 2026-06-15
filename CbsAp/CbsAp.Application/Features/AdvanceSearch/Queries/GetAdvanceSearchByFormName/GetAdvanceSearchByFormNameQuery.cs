using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.AdvanceSearch;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.AdvanceSearch.Queries.getAdvanceSearchByFormName
{
    public record GetAdvanceSearchByFormNameQuery(string formName,string userId) : IQuery<ResponseResult<AdvanceSearchRequestForm>>
    {
    }
}
