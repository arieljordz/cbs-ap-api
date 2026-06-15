using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.AdvanceSearch;
using CbsAp.Application.DTOs.AdvanceSearch;
using CbsAp.Application.Features.AdvanceSearch.Queries.getAdvanceSearchByFormName;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.AdvanceSearch;

namespace CbsAp.Application.Features.AdvanceSearch.Queries.GetAdvanceSearchByFormName
{
    public class GetAdvanceSearchByIDQueryHandler : IQueryHandler<GetAdvanceSearchByFormNameQuery, ResponseResult<AdvanceSearchRequestForm>>
    {
        private readonly IAdvanceSearchService _AdvanceSearchService;
        public GetAdvanceSearchByIDQueryHandler(IAdvanceSearchService AdvanceSearchService)
        {
            _AdvanceSearchService = AdvanceSearchService;
        }

        public async Task<ResponseResult<AdvanceSearchRequestForm>> Handle(GetAdvanceSearchByFormNameQuery request, CancellationToken cancellationToken)
        {
            var AdvanceSearch = await _AdvanceSearchService.GetAdvanceSearchByUser(request.formName, request.userId);

            return AdvanceSearch == null ?
                ResponseResult<AdvanceSearchRequestForm>.BadRequest("AdvanceSearch not found") :
                ResponseResult<AdvanceSearchRequestForm>.OK(AdvanceSearch);
        }

    }
}