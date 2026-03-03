using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Keyword;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.KeywordManagement.Queries
{
    public class ExportKeywordsQueryHandler : IQueryHandler<ExportKeywordsQuery, ResponseResult<byte[]>>
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IExcelService _excelService;

        public ExportKeywordsQueryHandler(IUnitofWork unitofWork, IExcelService excelService)
        {
            _unitOfWork = unitofWork;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportKeywordsQuery request, CancellationToken cancellationToken)
        {
            ExpressionStarter<Keyword> predicate = PredicateBuilder.New<Keyword>(true);

            predicate = predicate
               .AndIf(!string.IsNullOrEmpty(request.InvoiceRoutingFlowName), s => s.InvRoutingFlow!.InvRoutingFlowName!.Contains(request.InvoiceRoutingFlowName!))
               .AndIf(!string.IsNullOrEmpty(request.EntityName), s => s.EntityProfile!.EntityName.Contains(request.EntityName!))
               .AndIf(!string.IsNullOrEmpty(request.KeywordName), s => s.KeywordName!.Contains(request.KeywordName!));


            var keywordRepository = _unitOfWork.GetRepository<Keyword>();

            var keywordQuery = keywordRepository.Query()
                .Include(k => k.EntityProfile)
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);


            var keywords = keywordQuery.Select(k => new ExportKeywordDTO
            {
                EntityName = k.EntityProfile.EntityName,
                KeywordName = k.KeywordName ?? string.Empty,
                IsActive = k.IsActive?"Yes":"No",
                InvoiceRoutingFlowName = k.InvRoutingFlow != null ? k.InvRoutingFlow.InvRoutingFlowName ?? "" : string.Empty
            });

            var keywordsToExport = await keywords.ToListAsync(cancellationToken);

            if (keywordsToExport.Count == 0 || keywordsToExport == null)
            {
                return ResponseResult<byte[]>.NotFound(MessageConstants.Message("Keyword", MessageOperationType.NotFound));
            }

            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(keywordsToExport, "Keyword"));

            return ResponseResult<byte[]>.Success(excelBytes, "Export excel data");
        }
    }
}
