using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Keyword;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Enums;

using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.KeywordManagement.Queries
{
    public class GetKeywordsQueryHandler : IQueryHandler<GetKeywordsQuery, ResponseResult<PaginatedList<KeywordDTO>>>
    {
        private readonly IUnitofWork _unitOfWork;

        public GetKeywordsQueryHandler(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public async Task<ResponseResult<PaginatedList<KeywordDTO>>> Handle(GetKeywordsQuery request, CancellationToken cancellationToken)
        {
            ExpressionStarter<Keyword> predicate =
                PredicateBuilder.New<Keyword>(true);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(request.InvoiceRoutingFlowName), s => s.InvRoutingFlow!.InvRoutingFlowName!.Contains(request.InvoiceRoutingFlowName!))
                .AndIf(!string.IsNullOrEmpty(request.EntityName), s => s.EntityProfile!.EntityName.Contains(request.EntityName!))
                .AndIf(!string.IsNullOrEmpty(request.KeywordName), s => s.KeywordName!.Contains(request.KeywordName!))
                .AndIf(request.IsActive.HasValue, d => d.IsActive == request.IsActive!.Value);

            var keywordRepository = _unitOfWork.GetRepository<Keyword>();

            var keywordQuery = keywordRepository.Query()
                .Include(k => k.EntityProfile)
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

          
            var keywords = keywordQuery.Select(k => new KeywordDTO
            {
                KeywordID = k.KeywordID,
                EntityProfileID = k.EntityProfileID,
                EntityName = k.EntityProfile.EntityName,
                InvoiceRoutingFlowID = k.InvoiceRoutingFlowID,
                KeywordName = k.KeywordName ?? string.Empty,
                IsActive = k.IsActive,
                InvoiceRoutingFlowName = k.InvRoutingFlow != null ? k.InvRoutingFlow.InvRoutingFlowName??"" : string.Empty
            });

            var paginatedKeywords = await keywords.OrderByDynamic(request.SortField, request.SortOrder)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            return keywords == null ? ResponseResult<PaginatedList<KeywordDTO>>.NotFound(MessageConstants.Message("Keyword", MessageOperationType.NotFound)) :
                    ResponseResult<PaginatedList<KeywordDTO>>.SuccessRetrieveRecords(paginatedKeywords);
        }
    }
}