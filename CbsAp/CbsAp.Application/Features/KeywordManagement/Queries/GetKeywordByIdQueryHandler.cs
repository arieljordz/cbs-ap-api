using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Keyword;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Keywords;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.KeywordManagement.Queries
{
    public class GetKeywordByIdQueryHandler : IQueryHandler<GetKeywordByIdQuery, ResponseResult<KeywordDTO>>
    {
        private readonly IUnitofWork _unitOfWork;


        public GetKeywordByIdQueryHandler(IUnitofWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public async Task<ResponseResult<KeywordDTO>> Handle(GetKeywordByIdQuery request, CancellationToken cancellationToken)
        {
            var keywordRepository = _unitOfWork.GetRepository<Keyword>();

            var keyword = await keywordRepository.Query()
                .Include(k => k.EntityProfile)
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.KeywordID == request.KeywordID, cancellationToken);
                

            if (keyword == null)
                return ResponseResult<KeywordDTO>.BadRequest("Keyword not found");

            var keywordDto = new KeywordDTO
            {
                KeywordID = keyword.KeywordID,
                EntityProfileID = keyword.EntityProfileID,
                EntityName = keyword.EntityProfile != null ? keyword.EntityProfile.EntityName ?? "" : string.Empty,
                InvoiceRoutingFlowID = keyword.InvoiceRoutingFlowID,
                KeywordName = keyword.KeywordName ?? string.Empty,
                IsActive = keyword.IsActive,
            };

            return ResponseResult<KeywordDTO>.OK(keywordDto);

        }
    }
}
